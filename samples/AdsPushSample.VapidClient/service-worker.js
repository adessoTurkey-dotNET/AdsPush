self.addEventListener("push", (event) => {
    if (!(self.Notification && self.Notification.permission === "granted")) {
        return;
    }

    const data = event.data?.json() ?? {};
    const icon = "icon.png";

    const options = {
        lang: data.lang || "en-US",
        title: data.title,
        body: data.message,
        tag: data.tag,
        image: data.image,
        vibrate: [200, 100, 200],
        actions: data.actions || [],
        icon,
        data: {
            url: data.click_action // Bu veriyi tıklandığında kullanıyoruz
        }
    };

    event.waitUntil(self.registration.showNotification(data.title, options));
});

self.addEventListener('notificationclick', function (event) {
    event.notification.close(); // Bildirimi kapat
    if (clients.openWindow && event.notification.data.url) {
        event.waitUntil(clients.openWindow(event.notification.data.url));
    }
});

self.addEventListener('install', function (event) {
    self.skipWaiting(); // Hemen yüklenmesini sağla
});

self.addEventListener('activate', function (event) {
    event.waitUntil(clients.claim()); // Hemen etkinleşmesini sağla
});
