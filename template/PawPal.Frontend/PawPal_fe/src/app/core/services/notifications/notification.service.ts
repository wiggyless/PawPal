import { getMessaging, getToken, onMessage } from 'firebase/messaging';
import { Router } from '@angular/router';
import { computed, Injectable, signal, inject, NgZone } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment.development';
import { Observable, of, catchError } from 'rxjs'; 

export interface AppNotification {
  id: string;
  title: string;
  body: string;
  redirectUrl: string;
  read: boolean;
  timestamp: Date;
}

@Injectable({ providedIn: 'root' })
export class NotificationService {
  private messaging = getMessaging();
  private apiUrl = environment.apiUrl + '/Notifications';
  private zone = inject(NgZone);

  notifications = signal<AppNotification[]>([]);
  unreadCount = computed(() => this.notifications().filter((n) => !n.read).length);

  constructor(
    private http: HttpClient,
    private router: Router,
  ) {}

  async requestPermission(): Promise<boolean> {
    const permission = await Notification.requestPermission();
    return permission === 'granted';
  }

  permissionState = signal<NotificationPermission | null>(null);

  async checkPermission() {
    const permission = Notification.permission;
    this.permissionState.set(permission);

    if (permission === 'granted') {
      await this.registerToken();
      this.listenForeground();
    }
  }

  private async registerToken() {
    const token = await getToken(this.messaging, {
      vapidKey:
        'BGeG2YJ7c5X-SuyD9LkB36cssHI1FEflumZcdaBo8sfz0ATutIqzTMU2RzREPnWs2w0zoCTDR_g9S2EExadeLNM',
    });
    this.http.post(`${this.apiUrl}/register-token`, { token }).subscribe();
  }

  clearToken(): Observable<void> {
  return this.http.post<void>(`${this.apiUrl}/clear-token`, {}).pipe(
    catchError((err) => {
      console.error('Failed to clear FCM token', err);
      return of(void 0);
    }),
  );
}

  async requestPermissionAndRegister() {
    const permission = await Notification.requestPermission();
    this.permissionState.set(permission);

    if (permission !== 'granted') {
      console.warn('Permission denied');
      return;
    }

    await this.registerToken();
    this.listenForeground();
  }
  private foregroundListening = false;

  listenForeground() {
    if (this.foregroundListening) return; 
    this.foregroundListening = true;

    onMessage(this.messaging, (payload) => {
      this.zone.run(() => {
        this.addNotification(payload);
      });
    });

    navigator.serviceWorker.addEventListener('message', (event) => {
      this.zone.run(() => {
        if (event.data?.type === 'NOTIFICATION_CLICK') {
          this.router.navigate([event.data.redirectUrl]);
        }
        if (event.data?.type === 'BACKGROUND_NOTIFICATION') {
          this.addNotification(event.data.payload);
        }
      });
    });
  }

  private addNotification(payload: any) {
    const notification: AppNotification = {
      id: crypto.randomUUID(),
      title: payload.notification?.title ?? 'New notification',
      body: payload.notification?.body ?? '',
      redirectUrl: payload.data?.['redirectUrl'] ?? '/',
      read: false,
      timestamp: new Date(),
    };
    this.notifications.update((current) => [notification, ...current]);
  }

  navigateTo(notification: AppNotification) {
    this.markAsRead(notification.id);
    this.router.navigate([notification.redirectUrl]);
  }

  markAsRead(id: string) {
    this.notifications.update((current) =>
      current.map((n) => (n.id === id ? { ...n, read: true } : n)),
    );
  }

  markAllAsRead() {
    this.notifications.update((current) => current.map((n) => ({ ...n, read: true })));
  }

  clearAll() {
    this.notifications.set([]);
  }
}
