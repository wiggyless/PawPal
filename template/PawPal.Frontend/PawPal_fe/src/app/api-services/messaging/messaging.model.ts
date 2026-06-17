import { SafeUrl } from "@angular/platform-browser";

export interface MessageDto {
  messageId: number;
  conversationId: number;
  senderId: number;
  senderUsername: string;
  content: string;
  sentAt: Date;
  isRead: boolean;
}

export interface ConversationDto {
  conversationId: number;
  otherUserId: number;
  otherUsername: string;
  lastMessage: string | null;
  lastMessageAt: Date | null;
  unreadCount: number;
  imageData?: SafeUrl | string;
}

export interface SendMessageCommand {
  senderId: number;
  recipientId: number;
  content: string;
}