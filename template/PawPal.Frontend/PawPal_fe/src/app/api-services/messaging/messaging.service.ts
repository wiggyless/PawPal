// messaging.service.ts
import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ConversationDto, MessageDto, SendMessageCommand } from './messaging.model';
import { PageResult } from '../../core/models/paging/page-result';

@Injectable({ providedIn: 'root' })
export class MessagingService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl + '/Messages';

  getConversations(userId: number): Observable<ConversationDto[]> {
    return this.http.get<ConversationDto[]>(`${this.apiUrl}/conversations/${userId}`);
  }

  getMessages(conversationId: number, requestingUserId: number, page = 1): Observable<PageResult<MessageDto>> {
  return this.http.get<PageResult<MessageDto>>(
    `${this.apiUrl}/${conversationId}/messages`,
    { params: { requestingUserId, page } }
  );
}
  sendMessage(cmd: SendMessageCommand): Observable<MessageDto> {
  return this.http.post<MessageDto>(`${this.apiUrl}/send`, cmd);
}

  getOrCreateConversation(params: { senderId: number, recipientId: number }): Observable<ConversationDto> {
  return this.http.get<ConversationDto>(`${this.apiUrl}/conversation`, { params: { ...params } });
}
}