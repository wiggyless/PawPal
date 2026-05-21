import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class SignalRService {
  private hubConnection!: signalR.HubConnection; // Use definite assignment assertion

  private commentReceivedSource = new Subject<any>();
  commentReceived$ = this.commentReceivedSource.asObservable();

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7260/commentHub', {
        accessTokenFactory: () => {
          const token = localStorage.getItem('accessToken');
          return token ? token : '';
        },
      })
      .withAutomaticReconnect()
      .build();

    this.startConnection();
    this.registerOnServerEvents();
  }

  private startConnection() {
    this.hubConnection
      .start()
      .then(() => console.log('SignalR: Connection started'))
      .catch((err) => console.error('SignalR: Error while starting connection: ', err));
  }

  private registerOnServerEvents() {
    this.hubConnection.on('ReceiveComment', (data: any) => {
      this.commentReceivedSource.next(data);
    });
  }
}
