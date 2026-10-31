# ORBİT — Android APK indir

Halkaları döndürüp aynı renkleri buluşturduğun bir bulmaca oyunu.

**İndir:** [Releases](../../releases/latest) sayfasından en son `ORBIT-<sürüm>.apk` dosyasını indir.

## Kurulum
1. APK dosyasını telefonda aç.
2. İstenirse o uygulamaya (Chrome, Dosyalar vb.) **"Bilinmeyen uygulamaları yükle"** izni ver.
3. Play Protect uyarısı çıkarsa **"Yine de yükle"**ye dokun. Bu uyarı mağaza dışından gelen her uygulamada çıkar.

Yeni sürüm eskisinin üstüne kurulur; oyun ilerlemen silinmez (1.8.3'ten geçiş hariç, aşağıya bak).


## Her zaman en son sürüm
Doğrudan indirme (QR kodu da bu adresi açar): https://raw.githubusercontent.com/dragony1905/apk/main/ORBIT.apk — her yeni sürümde güncellenir.

## iPhone, iPad ve bilgisayar: web sitesi
APK yalnızca Android'de çalışır. iPhone'da ve bilgisayarda oyun tarayıcıdan oynanır:

**https://orbit-pocket-play.lovable.app**

- iPhone/iPad: sayfayı **Safari**'de aç → alttaki **Paylaş** düğmesi → **Ana Ekrana Ekle** → **Ekle**. ORBİT ana ekranda uygulama gibi tam ekran açılır, internetsiz de oynanır.
- Android'de Chrome'da da açılabilir ("Ana ekrana ekle"), ama Android için APK önerilir.
- Web sürümü her açılışta en son sürümü yükler; ilerleme o cihazın tarayıcısında saklanır (hesapla giriş yaparsan bulutta da).

## Dosyanın değişmediğini doğrula (isteğe bağlı)
Her sürümün yanında `.sha256` dosyası vardır. Bilgisayarda şu komutu çalıştır:

```
sha256sum ORBIT-<sürüm>.apk
```

Çıkan değer `.sha256` dosyasındakiyle aynı olmalı.

## Bu depo hakkında
- Bu depoda oyunun kodu bulunmaz; yalnızca derlenmiş APK'lar yayınlanır.
- **1.8.3'ten geçiyorsan:** 1.8.3 geçici bir anahtarla imzalıydı. 1.9.5 ve sonrası kalıcı anahtarla imzalıdır, bu yüzden 1.9.5'i kurmadan önce eski ORBİT'i **bir kez kaldırman** gerekir (o telefondaki misafir ilerlemesi silinir; hesapla giriş yaptıysan ilerlemen buluttan geri gelir). Bundan sonraki bütün sürümler aynı anahtarla imzalanır ve eskisinin üstüne kurulur.
- Kalıcı imza anahtarının SHA-1 parmak izi: `79:EF:FA:49:25:14:9A:AC:6D:72:12:25:9D:0C:5D:CC:98:89:D4:68`

## Uygulama içi güncelleme (2.3.0 ve sonrası)
2.3.0'dan itibaren ORBİT yeni sürümü kendi içinden bulup kurabilir: Ayarlar → Güncellemeler → "Güncel sürümü kontrol et".
- Oyun bu depodaki `guncelleme.json` dosyasını okur. Dosyada sürüm adı, sürüm kodu, yenilikler, APK adresi, boyut ve SHA-256 değeri bulunur.
- APK yalnızca bu deponun `surumler/` klasöründen indirilir. Kurulmadan önce boyut, SHA-256, paket adı, sürüm kodu ve imza sertifikası (yüklü ORBİT'le aynı anahtar) kontrol edilir; biri tutmazsa dosya silinir ve kurulmaz.
- Kurulumu Android'in kendi onay ekranı yapar. İlk seferde Android, ORBİT için "Bilinmeyen uygulamaları yükle" izni ister.
- Oyun ilerlemen silinmez; ayrıca indirmeden önce kayıtların bir yedeği alınır.
- 2.2.0 ve öncesinde bu özellik yok: 2.3.0'ı bir kez elle kurman gerekir.
