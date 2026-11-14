# Çiğköfte Oyunu

Unity ile geliştirilen çiğköfte dükkânı simülasyonu. Tasarım belgesindeki (GDD 1.0)
**Aşama 1 — Teknik temel** burada duruyor.

> Durum: Kod henüz hiç derlenmedi ve hiçbir cihazda denenmedi. Unity lisansı eklenip
> ilk derleme çalıştırılana kadar hatasız olduğu bilinmiyor.

## Şu an ne var

- Karakter hareketi: yürüme, koşma, kameraya göre yön
- Üçüncü şahıs kamera: döndürme, yakınlaştırma, duvarın içine girmeme
- Etkileşim sistemi: yakındaki nesne seçilir, butonla ya da `E` ile kullanılır
- Temel arayüz: para, saat, bildirim, sanal joystick, etkileşim butonu
- Kayıt sistemi: otomatik kayıt (60 sn ve uygulama arka plana alınınca), elle kayıt, yedek dosya
- Kutulardan oluşan bir deneme dükkânı: tezgâh, kasa, kayıt defteri

Kontroller:

| | Telefon | PC |
|---|---|---|
| Hareket | Ekranın sol alt bölgesinde sürükle | WASD / ok tuşları |
| Koşma | Joystick'i sonuna kadar it | Sol Shift |
| Kamera | Ekranın başka bir yerinde sürükle | Sağ tık + fare, tekerlek |
| Etkileşim | Sağ alttaki buton | E |

## Tasarım belgesinden bilinçli sapmalar

- **URP yok.** Şimdilik Unity'nin yerleşik çizim hattı kullanılıyor. URP, editör olmadan
  kurulması riskli bir ayar gerektiriyor; ilk derleme çalıştıktan sonra eklenecek.
- **Sahne dosyası yok.** Editör kullanılmadığı için her şey `OyunKurucu.cs` içinde kodla kuruluyor.
- **`.meta` dosyaları depoda yok.** Unity bunları her derlemede yeniden üretiyor. Proje bir gün
  editörde açılırsa üretilen `.meta` dosyaları depoya eklenmeli.

## Derleme (GitHub Actions)

Derleme `.github/workflows/cigkofte-derle.yml` ile yapılır ve elle başlatılır:
**Actions → "Çiğköfte Oyunu - Android derle" → Run workflow**. Bitince APK, çalışmanın
sayfasındaki **Artifacts** bölümünden `CigKofte-Android` adıyla indirilir.

### Bir kerelik kurulum: Unity lisansı

Derleme için depoya üç secret eklenmeli (**Settings → Secrets and variables → Actions**):

| Secret | İçerik |
|---|---|
| `UNITY_EMAIL` | Unity hesabının e-postası |
| `UNITY_PASSWORD` | Unity hesabının şifresi |
| `UNITY_LICENSE` | `.ulf` lisans dosyasının tüm içeriği |

`.ulf` dosyası yalnızca bilgisayarda üretilebiliyor:

1. Telefondan <https://id.unity.com> adresinde ücretsiz bir Unity hesabı aç.
2. Bir bilgisayarda Unity Hub'ı kur ve hesabınla giriş yap.
3. Unity Hub → **Preferences → Licenses → Add → Get a free personal license**.
4. Oluşan dosyayı bul:
   - Windows: `C:\ProgramData\Unity\Unity_lic.ulf`
   - macOS: `/Library/Application Support/Unity/Unity_lic.ulf`
   - Linux: `~/.local/share/unity3d/Unity/Unity_lic.ulf`
5. Dosyayı not defteriyle aç, tüm içeriği kopyala ve `UNITY_LICENSE` secret'ına yapıştır.

Ücretsiz lisansın süresi bir süre sonra doluyor; derleme lisans hatası verirse 3–5. adımlar
tekrarlanır. Güncel yönerge: <https://game.ci/docs/github/activation>

Not: Bu depo herkese açık. Secret'lar gizli kalır, ama buradaki kodun tamamını herkes görebilir.

## Klasörler

```
cigkofte/
  Assets/Scriptler/   oyun kodu
  Assets/Editor/      derleme betiği (Derleyici.cs)
  Packages/           Unity paket listesi
  ProjectSettings/    Unity sürümü (6000.3.8f1)
```
