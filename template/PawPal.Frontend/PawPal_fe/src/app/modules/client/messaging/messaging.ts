import {
  Component,
  OnInit,
  OnDestroy,
  inject,
  ChangeDetectorRef,
  ViewChild,
  ElementRef,
  AfterViewChecked,
} from '@angular/core';
import { Subject, Subscription, debounceTime } from 'rxjs';
import { MessagingService } from '../../../api-services/messaging/messaging.service';
import { SignalRService } from '../../../core/services/signalr.service';
import { ConversationDto, MessageDto } from '../../../api-services/messaging/messaging.model';
import { ActivatedRoute } from '@angular/router';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { UserImageService } from '../../../api-services/userImage/userImage-service';

@Component({
  selector: 'app-messaging',
  standalone: false,
  templateUrl: './messaging.html',
  styleUrl: './messaging.scss',
})
export class Messaging implements OnInit, OnDestroy, AfterViewChecked {
  @ViewChild('messagesContainer') private messagesContainer!: ElementRef;

  conversations: ConversationDto[] = [];
  selectedConversation: ConversationDto | null = null;
  messages: MessageDto[] = [];
  newMessage = '';
  isOtherUserTyping = false;
  isOtherUserOnline = false;
  isLoadingMore = false;

  currentUser = inject(CurrentUserService);
  private cd = inject(ChangeDetectorRef);
  userImgService = inject(UserImageService);

  private messageSub!: Subscription;
  private typingSub!: Subscription;
  private stoppedTypingSub!: Subscription;
  private typingDebounceSub!: Subscription;
  private typingSubject = new Subject<void>();
  private shouldScrollToBottom = false;
  private currentPage = 1;
  private totalItems = 0;
  private pageSize = 10;

  constructor(
    private messagingService: MessagingService,
    private signalRService: SignalRService,
    private route: ActivatedRoute,
  ) {}

  ngOnInit() {
    this.loadConversations();

    this.route.queryParams.subscribe((params) => {
      const recipientId = params['recipientId'];
      if (recipientId) {
        this.messagingService
          .getOrCreateConversation({
            senderId: this.currentUser.userId() as number,
            recipientId: Number(recipientId),
          })
          .subscribe((convo) => {
            const exists = this.conversations.find(
              (c) => c.conversationId === convo.conversationId,
            );
            if (!exists) this.conversations = [convo, ...this.conversations];
            this.selectConversation(convo);
          });
      }
    });

    this.messageSub = this.signalRService.messageReceived$.subscribe((msg) => {
      if (
        this.selectedConversation &&
        msg.conversationId === this.selectedConversation.conversationId
      ) {
        this.messages = [...this.messages, msg];
        this.shouldScrollToBottom = true;
      }
      this.loadConversations();
      this.cd.detectChanges();
    });

    this.typingSub = this.signalRService.userTyping$.subscribe(() => {
      this.isOtherUserTyping = true;
      this.shouldScrollToBottom = true;
      this.cd.detectChanges();
    });

    this.stoppedTypingSub = this.signalRService.userStoppedTyping$.subscribe(() => {
      this.isOtherUserTyping = false;
      this.cd.detectChanges();
    });

    this.typingDebounceSub = this.typingSubject.pipe(debounceTime(2000)).subscribe(() => {
      if (this.selectedConversation) {
        this.signalRService.notifyStoppedTyping(this.selectedConversation.otherUserId);
      }
    });
  }

  ngAfterViewChecked() {
    if (this.shouldScrollToBottom) {
      this.scrollToBottom();
      this.shouldScrollToBottom = false;
    }
  }

  loadConversations() {
    this.messagingService
      .getConversations(this.currentUser.userId()! as number)
      .subscribe((convos) => {
        if (convos.length === 0) {
          this.conversations = [];
          this.cd.detectChanges();
          return;
        }
        let counter = 0;
        convos.forEach((convo) => {
          this.userImgService.getUserImageByID(convo.otherUserId).subscribe({
            next: (safeUrl) => {
              convo.imageData = safeUrl;
            },
            error: () => {
              convo.imageData = '';
            },
            complete: () => {
              counter++;
              if (counter >= convos.length) {
                this.conversations = convos;
                if (this.selectedConversation) {
                  const updated = convos.find(
                    (c) => c.conversationId === this.selectedConversation!.conversationId,
                  );
                  if (updated) this.selectedConversation = updated;
                }
                this.cd.detectChanges();
              }
            },
          });
        });
      });
  }

  selectConversation(convo: ConversationDto) {
    this.conversations = this.conversations.map((c) =>
      c.conversationId === convo.conversationId ? { ...c, unreadCount: 0 } : c,
    );
    this.selectedConversation = convo;
    this.messages = [];
    this.currentPage = 1;
    this.isOtherUserTyping = false;
    this.cd.detectChanges();

    this.messagingService
      .getMessages(convo.conversationId, this.currentUser.userId() as number, 1)
      .subscribe((result) => {
        this.messages = [...result.items].reverse();
        this.totalItems = result.totalItems;
        this.shouldScrollToBottom = true;
        this.cd.detectChanges();
      });
  }

  loadMoreMessages() {
    if (!this.selectedConversation || this.isLoadingMore || !this.hasMoreMessages) return;

    this.isLoadingMore = true;
    this.cd.detectChanges();
    this.currentPage++;

    this.messagingService
      .getMessages(
        this.selectedConversation.conversationId,
        this.currentUser.userId() as number,
        this.currentPage,
      )
      .subscribe((result) => {
        this.totalItems = result.totalItems;
        this.messages = [...result.items.reverse(), ...this.messages];
        this.isLoadingMore = false;
        this.cd.detectChanges();
      });
  }

  onTyping() {
    if (this.selectedConversation) {
      this.signalRService.notifyTyping(this.selectedConversation.otherUserId);
      this.typingSubject.next();
    }
  }

  send() {
    if (!this.newMessage.trim() || !this.selectedConversation) return;

    const content = this.newMessage.trim();
    this.newMessage = '';
    this.isOtherUserTyping = false;
    this.typingSubject.next();

    this.messagingService
      .sendMessage({
        senderId: this.currentUser.userId() as number,
        recipientId: this.selectedConversation.otherUserId,
        content,
      })
      .subscribe((msg) => {
        this.messages = [...this.messages, msg];
        this.shouldScrollToBottom = true;

        this.conversations = this.conversations.map((c) =>
          c.conversationId === msg.conversationId
            ? { ...c, lastMessage: msg.content, lastMessageAt: msg.sentAt }
            : c,
        );

        this.cd.detectChanges();
      });
  }

  private scrollToBottom() {
    try {
      this.messagesContainer.nativeElement.scrollTop =
        this.messagesContainer.nativeElement.scrollHeight;
    } catch {}
  }

  ngOnDestroy() {
    this.messageSub?.unsubscribe();
    this.typingSub?.unsubscribe();
    this.stoppedTypingSub?.unsubscribe();
    this.typingDebounceSub?.unsubscribe();
  }

  get hasMoreMessages(): boolean {
    return this.currentPage * this.pageSize < this.totalItems;
  }

  onScroll() {
    const el = this.messagesContainer.nativeElement;
    if (el.scrollTop <= 5 && !this.isLoadingMore && this.hasMoreMessages) {
      this.loadMoreMessages();
    }
  }
}
