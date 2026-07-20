import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { MessageDto } from '../../api-services/messaging/messaging.model';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class SignalRService {
  private commentHubConnection!: signalR.HubConnection;
  private commentReceivedSource = new Subject<any>();
  commentReceived$ = this.commentReceivedSource.asObservable();

  private messageHubConnection!: signalR.HubConnection;
  private messageReceivedSource = new Subject<MessageDto>();
  private userTypingSource = new Subject<string>();
  private userStoppedTypingSource = new Subject<string>();

  messageReceived$ = this.messageReceivedSource.asObservable();
  userTyping$ = this.userTypingSource.asObservable();
  userStoppedTyping$ = this.userStoppedTypingSource.asObservable();

  constructor() {
    this.initCommentHub();
    this.initMessageHub();
  }

  private getToken(): string {
    return localStorage.getItem('accessToken') ?? '';
  }

  private initCommentHub() {
    this.commentHubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/commentHub`, {
        accessTokenFactory: () => this.getToken(),
      })
      .withAutomaticReconnect()
      .build();

    this.commentHubConnection
      .start()
      .then(() => console.log('SignalR: Comment hub connected'))
      .catch((err) => console.error('SignalR: Comment hub error', err));

    this.commentHubConnection.on('ReceiveComment', (data: any) => {
      this.commentReceivedSource.next(data);
    });
  }

  private initMessageHub() {
    this.messageHubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/messageHub`, {
        accessTokenFactory: () => this.getToken(),
      })
      .withAutomaticReconnect()
      .build();

    this.messageHubConnection
      .start()
      .then(() => console.log('SignalR: Message hub connected'))
      .catch((err) => console.error('SignalR: Message hub error', err));

    this.messageHubConnection.on('ReceiveMessage', (msg: MessageDto) => {
      this.messageReceivedSource.next(msg);
    });

    this.messageHubConnection.on('UserTyping', (userId: string) => {
      this.userTypingSource.next(userId);
    });

    this.messageHubConnection.on('UserStoppedTyping', (userId: string) => {
      this.userStoppedTypingSource.next(userId);
    });
  }
  notifyTyping(recipientId: number) {
    this.messageHubConnection.invoke('UserTyping', recipientId);
  }
  notifyStoppedTyping(recipientId: number) {
    this.messageHubConnection.invoke('UserStoppedTyping', recipientId);
  }
}
