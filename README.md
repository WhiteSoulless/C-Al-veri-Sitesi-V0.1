# 🛍️ SalesApp - Modern Mobil Satış Platformu

![.NET MAUI](https://img.shields.io/badge/.NET%20MAUI-9.0%20%2F%2010.0-512BD4?logo=dotnet)
![C#](https://img.shields.io/badge/C%23-13-239120?logo=csharp)
![Platform](https://img.shields.io/badge/Platforms-Android%20%7C%20iOS%20%7C%20Windows-blue)
![Architecture](https://img.shields.io/badge/Architecture-MVVM%20CommunityToolkit-orange)

**SalesApp**, C# ve .NET MAUI kullanılarak geliştirilmiş modern, cross-platform bir e-ticaret ve satış platformu mobil uygulamasıdır. Endüstri standardı **MVVM (Model-View-ViewModel)** mimarisi ve gevşek bağlı servis yapısı ile inşa edilmiştir.

---

## ✨ Özellikler

### 📱 Kimlik Doğrulama & SMS Güvenliği
- **Telefon ile Giriş:** Kullanıcı dostu telefon numarası giriş arayüzü.
- **SMS OTP Doğrulama:** 6 haneli zaman aşımı denetimli tek kullanımlık güvenlik kodu (OTP) simülasyonu.
- **Misafir Kullanıcı Desteği:** Giriş yapmadan vitrini ve ürünleri gezebilme imkanı.

### 🛒 E-Ticaret & Vitrin Deneyimi
- **Dinamik Vitrin:** Popüler ve öne çıkan ürünler, indirim yüzdeleri ve kullanıcı puanları.
- **Hızlı Kategori Filtreleme:** Elektronik, Moda, Ayakkabı, Aksesuar kategorileri arasında tek tıkla geçiş.
- **Anlık Arama:** Başlık veya kategoriye göre canlı ürün arama.
- **Sepet Yönetimi:** 
  - Adet artırma/azaltma ve silme kontrolleri.
  - İndirim Kuponu Sistemi (Örn: `%20` indirim sağlayan `CLASS20` kuponu).
  - 1000 TL üzeri siparişlerde dinamik ücretsiz kargo kuralı.

### 🌐 Akıllı Ağ ve Çevrimdışı Yönetimi
- **Connectivity Service:** Cihazın internet bağlantısını anlık izleme.
- **Offline Bildirimi:** İnternet bağlantısı koptuğunda vitrinde otomatik beliren uyarı şeridi ve kullanıcı bildirimleri.

### ⚙️ Kapsamlı Ayarlar & Tema
- **Karanlık Mod (Dark Mode):** Göz yormayan koyu tema desteği ve anlık tema geçişi.
- **Para Birimi Seçimi:** ₺ (TL), $ (USD), € (EUR) desteği.
- **Bildirim & İletişim Tercihleri:** Uygulama içi ve SMS bildirim izinlerinin yönetimi.
- **Güvenli Çıkış:** Onay pencereli oturum kapatma.

---

## 🏗️ Mimari & Katmanlar

```text
SalesApp/
├── Models/             # Veri Modelleri (Product, Category, CartItem, Order, User, OtpRequest, AppSettings)
├── Services/           # İş Mantığı & Bağımsız Servisler (Auth, Network, Shop, Settings, Notification)
├── ViewModels/         # MVVM Katmanı (CommunityToolkit.Mvvm)
├── Views/              # XAML Arayüzleri (Home, Cart, Settings, Login, Otp, AppShell)
└── Resources/          # Renkler, Stiller, İkonlar ve Fontlar
```

---

## 🚀 Kurulum ve Çalıştırma

Projeyi yerel ortamınızda çalıştırmak için .NET 9 veya .NET 10 SDK gereklidir:

```bash
# Projeyi klonlayın
git clone <REPO_URL>
cd SalesApp

# Bağımlılıkları yükleyin
dotnet restore

# Windows üzerinde çalıştırma
dotnet run -f net9.0-windows10.0.19041.0

# Android üzerinde çalıştırma
dotnet build -t:Run -f net9.0-android
```

---

## 👨‍💻 Geliştirici
**Kerem** - *C# & .NET MAUI Developer*
