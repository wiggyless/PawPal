importScripts('https://www.gstatic.com/firebasejs/12.14.0/firebase-app-compat.js');
importScripts('https://www.gstatic.com/firebasejs/12.14.0/firebase-messaging-compat.js');

firebase.initializeApp({
    apiKey: "AIzaSyAu667azcE7cZCwE0PAMBKcfjo0myydnYE",
    authDomain: "pawpal-b7f74.firebaseapp.com",
    projectId: "pawpal-b7f74",
    storageBucket: "pawpal-b7f74.firebasestorage.app",
    messagingSenderId: "1054153560955",
    appId: "1:1054153560955:web:c2feef7e4c364b4ae8e3de",
    measurementId: "G-2NPTKPYLXS"
});
const messaging = firebase.messaging();

messaging.onBackgroundMessage((payload) => {
  console.log('SW: background message received', payload); 
 self.clients.matchAll({ type: 'window', includeUncontrolled: true }).then(clients => {
    console.log('SW: found clients:', clients.length);
    clients.forEach(client => {
      client.postMessage({
        type: 'BACKGROUND_NOTIFICATION',
        payload
      });
    });
  });

  // still show browser popup
  self.registration.showNotification(payload.notification.title, {
    body: payload.notification.body,
    data: payload.data
  });
});

self.addEventListener('notificationclick', (event) => {
  event.notification.close();

  const redirectUrl = event.notification.data?.redirectUrl ?? '/';
  const fullUrl = 'http://localhost:4200' + redirectUrl; // replace with your actual domain

  event.waitUntil(
    clients.matchAll({ type: 'window', includeUncontrolled: true }).then((clientList) => {
      // if app is already open, focus it and navigate
      for (const client of clientList) {
        if (client.url.includes('localhost:4200') && 'focus' in client) {
          client.focus();
          client.postMessage({ type: 'NOTIFICATION_CLICK', redirectUrl });
          return;
        }
      }
      // if app is not open, open a new window
      if (clients.openWindow) {
        return clients.openWindow(fullUrl);
      }
    })
  );
});