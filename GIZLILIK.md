# ORBİT — Gizlilik Politikası

Son güncelleme: 30 Ekim 2026

ORBİT, halkaları döndürüp renkleri eşleştirdiğin bir bulmaca oyunudur. Bu sayfa, oyunun hangi bilgileri neden kullandığını anlatır.

## Giriş yapmadan oynarken
- Profil adın, seçtiğin avatar ya da fotoğraf, skorların, kupaların, altının ve ayarların **yalnızca telefonunda** saklanır.
- Oyunu geliştirmek için maçların özeti (hamleler, skor, süre, cihaz türü) **adın ya da e-postan olmadan**, rastgele bir cihaz kimliğiyle sunucumuza gönderilebilir. Bunu oyunun ayarlarından kapatabilirsin.

## Kullanıcı adı ve şifreyle hesap açtığında
- Hesap için yalnızca seçtiğin **kullanıcı adı** ve **şifre** istenir; e-posta adresi istenmez.
- Şifren düz metin olarak saklanmaz, yalnızca geri çevrilemeyen bir özeti (bcrypt) saklanır.
- E-posta olmadığı için unutulan şifre e-postayla sıfırlanamaz. Hesabına Google hesabı eklersen Google ile de giriş yapabilirsin.
- Kullanıcı adın başka oyunculara gösterilmez; sıralamada profil adın ve avatarın görünür.

## Google ile giriş yaptığında ya da Google hesabı eklediğinde
- Google hesabının **kimliği ve e-posta adresi** yalnızca seni tanımak ve ilerlemeni cihazlar arasında taşımak için kullanılır.
- Oyun ilerlemen (profil, kupalar, ayarlar) buluta kaydedilir.
- Sıralamalarda ve arkadaş listesinde oyunda seçtiğin **profil adı ve avatar** görünür; e-posta adresin başkalarına gösterilmez.
- Profil fotoğrafın yalnızca kendi cihazında ve kendi bulut kaydında tutulur, diğer oyunculara gösterilmez.

## Reklamlar
- Uygulama Google AdMob ile reklam gösterebilir. Reklamlar yalnızca maç bittikten sonra çıkar; oyun sırasında ve açılışta reklam yoktur. Ödüllü reklamı izlemek tamamen isteğe bağlıdır.
- AdMob, reklam göstermek ve ölçmek için cihazın **reklam kimliğini**, IP adresini ve cihaz bilgilerini kullanabilir. Bu bilgileri Google işler ([Google'ın reklam ortağı uygulamalarındaki veri kullanımı](https://policies.google.com/technologies/partner-sites), [Google gizlilik politikası](https://policies.google.com/privacy)).
- İzin gereken bölgelerde (ör. Avrupa Ekonomik Alanı, Birleşik Krallık) reklamdan önce Google'ın izin ekranı gösterilir. Kararını oyunda **Ayarlar → Reklam gizlilik seçenekleri**'nden değiştirebilirsin.
- Reklam kimliğini telefonunun ayarlarından sıfırlayabilir ya da silebilirsin (Ayarlar → Google → Reklamlar).
- Kutular ve oyun içi ödüller reklamla satılmaz.

## Paylaşım ve saklama
- Bilgilerin satılmaz. Profil ve oyun verilerin reklam amacıyla paylaşılmaz; reklamlar için yalnızca yukarıda yazan, AdMob'un doğrudan topladığı bilgiler kullanılır.
- Veriler Lovable Cloud (Supabase) altyapısında, şifreli bağlantı (HTTPS) üzerinden saklanır.

## Hesabını silmek
- Oyunda **Ayarlar → Hesap → Hesabımı sil** ile hesabını ve buluttaki bütün verilerini kalıcı olarak silebilirsin.

## İletişim
- Sorular için: [github.com/dragony1905/apk/issues](https://github.com/dragony1905/apk/issues)

---

# ORBİT — Privacy Policy (English)

ORBİT stores your profile, scores and settings on your device. Anonymous match summaries (no name or email, random device ID) may be sent to improve the game; you can turn this off in settings. You can create an account with a username and password (no email; the password is stored only as a bcrypt hash, and a forgotten password cannot be reset by email). If you sign in with Google or add Google to your account, your Google account ID and email are used only to identify you and sync your progress; other players only see your chosen profile name and avatar. The app may show Google AdMob ads after matches (never during play); AdMob may use your advertising ID, IP address and device information, and in regions that require it (e.g. EEA, UK) Google's consent form is shown first — you can change your choice in Settings → Ad privacy options. Your data is never sold, and your profile and game data are not shared for advertising. You can permanently delete your account and cloud data in Settings → Account → Delete my account. Contact: github.com/dragony1905/apk/issues
