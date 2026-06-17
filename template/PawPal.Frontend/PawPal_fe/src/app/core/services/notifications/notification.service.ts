import { getMessaging, getToken, onMessage } from 'firebase/messaging';
import { Router } from '@angular/router';
import { computed, Injectable, signal, inject, NgZone } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment.development';

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
    console.log('checkPermission called, state:', permission);
    this.permissionState.set(permission);

    // already granted - register token silently
    if (permission === 'granted') {
      await this.registerToken();
      this.listenForeground();
    }
  }

  // extract token registration into its own method
  private async registerToken() {
    const token = await getToken(this.messaging, {
      vapidKey:
        'BGeG2YJ7c5X-SuyD9LkB36cssHI1FEflumZcdaBo8sfz0ATutIqzTMU2RzREPnWs2w0zoCTDR_g9S2EExadeLNM',
    });
    this.http.post(`${this.apiUrl}/register-token`, { token }).subscribe({
      next: () => console.log('FCM token registered'),
      error: (err) => console.error('Failed to register token', err),
    });
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
    if (this.foregroundListening) return; // prevent duplicate listeners
    this.foregroundListening = true;
    console.log('listenForeground registered');

    // FCM foreground messages
    onMessage(this.messaging, (payload) => {
      console.log('Foreground message received', payload);
      this.zone.run(() => {
        this.addNotification(payload);
      });
    });

    // Service worker messages (background click / background notification)
    navigator.serviceWorker.addEventListener('message', (event) => {
      // CRITICAL FIX: Wrap this inside zone execution
      this.zone.run(() => {
        if (event.data?.type === 'NOTIFICATION_CLICK') {
          this.router.navigate([event.data.redirectUrl]);
        }
        if (event.data?.type === 'BACKGROUND_NOTIFICATION') {
          console.log('App received notification from SW:', event.data.payload);
          this.addNotification(event.data.payload);
        }
      });
    });
  }

  // Extract into a helper to avoid duplication
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
