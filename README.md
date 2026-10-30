# ORBİT — Android APK indir

Halkaları döndürüp aynı renkleri buluşturduğun bir bulmaca oyunu.

**İndir:** [Releases](../../releases/latest) sayfasından en son `ORBIT-<sürüm>.apk` dosyasını indir.

## Kurulum
1. APK dosyasını telefonda aç.
2. İstenirse o uygulamaya (Chrome, Dosyalar vb.) **"Bilinmeyen uygulamaları yükle"** izni ver.
3. Play Protect uyarısı çıkarsa **"Yine de yükle"**ye dokun. Bu uyarı mağaza dışından gelen her uygulamada çıkar.

Yeni sürüm eskisinin üstüne kurulur; oyun ilerlemen silinmez (1.8.3'ten geçiş hariç, aşağıya bak).

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
