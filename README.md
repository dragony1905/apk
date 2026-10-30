# ORBİT — Android APK indir

Halkaları döndürüp aynı renkleri buluşturduğun bir bulmaca oyunu.

**İndir:** [Releases](../../releases/latest) sayfasından en son `ORBIT-<sürüm>.apk` dosyasını indir.

## Kurulum
1. APK dosyasını telefonda aç.
2. İstenirse o uygulamaya (Chrome, Dosyalar vb.) **"Bilinmeyen uygulamaları yükle"** izni ver.
3. Play Protect uyarısı çıkarsa **"Yine de yükle"**ye dokun. Bu uyarı mağaza dışından gelen her uygulamada çıkar.

Yeni sürüm eskisinin üstüne kurulur; oyun ilerlemen silinmez.

## Dosyanın değişmediğini doğrula (isteğe bağlı)
Her sürümün yanında `.sha256` dosyası vardır. Bilgisayarda şu komutu çalıştır:

```
sha256sum ORBIT-<sürüm>.apk
```

Çıkan değer `.sha256` dosyasındakiyle aynı olmalı.

## Bu depo hakkında
- Bu depoda oyunun kodu bulunmaz; yalnızca derlenmiş APK'lar yayınlanır.
- İlk sürüm (1.8.3) geçici bir anahtarla imzalıdır; kalıcı anahtara geçildiğinde bir kez kaldırıp yeniden kurman gerekebilir. Sonraki bütün sürümler aynı anahtarla imzalanır.
