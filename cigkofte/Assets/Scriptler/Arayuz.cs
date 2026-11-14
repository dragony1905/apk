// Temel arayüz: para ve saat göstergesi, bildirim, sanal joystick ve etkileşim butonu.
// Her şey kodla kurulur. Dokunuşlar burada dağıtılır: sol alt bölge joystick,
// etkileşim butonu, geri kalan her yer kamera.
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-100)]
public class Arayuz : MonoBehaviour
{
    static readonly Color Krem = new Color(0.96f, 0.93f, 0.85f);
    static readonly Color Kahve = new Color(0.30f, 0.21f, 0.15f);
    static readonly Color Zeytin = new Color(0.42f, 0.47f, 0.25f);

    const float JoystickYaricapi = 110f;
    const float ButonBoyutu = 190f;

    Canvas canvas;
    Font yaziTipi;
    RectTransform guvenliAlan;
    RectTransform joyTaban, joyTopuz, buton, bildirimKutu;
    Text paraYazi, saatYazi, bildirimYazi, butonYazi;

    int joyParmak = -1;
    int kameraParmak = -1;
    Vector2 joyMerkez;
    float bildirimSuresi;
    Rect sonGuvenliAlan;
    int sonPara = -1;
    int sonDakika = -1;

    void Awake()
    {
        canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var olcekleyici = gameObject.AddComponent<CanvasScaler>();
        olcekleyici.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        olcekleyici.referenceResolution = new Vector2(1920f, 1080f);
        olcekleyici.matchWidthOrHeight = 0.5f;

        yaziTipi = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (yaziTipi == null)
        {
            string[] adlar = Font.GetOSInstalledFontNames();
            if (adlar != null && adlar.Length > 0) yaziTipi = Font.CreateDynamicFontFromOSFont(adlar[0], 32);
        }

        Sprite daire = DaireOlustur();

        var alan = new GameObject("GuvenliAlan", typeof(RectTransform));
        alan.transform.SetParent(transform, false);
        guvenliAlan = (RectTransform)alan.transform;
        guvenliAlan.offsetMin = Vector2.zero;
        guvenliAlan.offsetMax = Vector2.zero;
        GuvenliAlanUygula();

        // Sol üst: para
        RectTransform paraKutu = Kutu("Para", guvenliAlan, new Vector2(0f, 1f), new Vector2(24f, -24f),
            new Vector2(300f, 72f), new Color(Kahve.r, Kahve.g, Kahve.b, 0.85f), null);
        paraYazi = Yazi(paraKutu, 34, TextAnchor.MiddleCenter, Krem);

        // Sağ üst: saat
        RectTransform saatKutu = Kutu("Saat", guvenliAlan, new Vector2(1f, 1f), new Vector2(-24f, -24f),
            new Vector2(180f, 72f), new Color(Kahve.r, Kahve.g, Kahve.b, 0.85f), null);
        saatYazi = Yazi(saatKutu, 34, TextAnchor.MiddleCenter, Krem);

        // Üst orta: bildirim
        bildirimKutu = Kutu("Bildirim", guvenliAlan, new Vector2(0.5f, 1f), new Vector2(0f, -120f),
            new Vector2(1000f, 80f), new Color(Zeytin.r, Zeytin.g, Zeytin.b, 0.92f), null);
        bildirimYazi = Yazi(bildirimKutu, 30, TextAnchor.MiddleCenter, Krem);
        bildirimKutu.gameObject.SetActive(false);

        // Sağ alt: etkileşim butonu
        buton = Kutu("EtkilesimButonu", guvenliAlan, new Vector2(1f, 0f), new Vector2(-150f, 170f),
            new Vector2(ButonBoyutu, ButonBoyutu), new Color(0.75f, 0.26f, 0.20f, 0.92f), daire);
        buton.pivot = new Vector2(0.5f, 0.5f);
        butonYazi = Yazi(buton, 26, TextAnchor.MiddleCenter, Krem);
        buton.gameObject.SetActive(false);

        // Joystick: parmağın değdiği yerde belirir.
        joyTaban = Kutu("JoystickTaban", transform, new Vector2(0f, 0f), Vector2.zero,
            new Vector2(JoystickYaricapi * 2f, JoystickYaricapi * 2f), new Color(1f, 1f, 1f, 0.22f), daire);
        joyTaban.pivot = new Vector2(0.5f, 0.5f);
        joyTopuz = Kutu("JoystickTopuz", transform, new Vector2(0f, 0f), Vector2.zero,
            new Vector2(96f, 96f), new Color(1f, 1f, 1f, 0.55f), daire);
        joyTopuz.pivot = new Vector2(0.5f, 0.5f);
        joyTaban.gameObject.SetActive(false);
        joyTopuz.gameObject.SetActive(false);

        OyunDurumu.BildirimGeldi += BildirimGoster;
    }

    void OnDestroy()
    {
        OyunDurumu.BildirimGeldi -= BildirimGoster;
    }

    void Update()
    {
        Girdi.Guncelle();

        if (Screen.safeArea != sonGuvenliAlan) GuvenliAlanUygula();

        DokunuslariIsle();
        ButonuGuncelle();
        GostergeleriGuncelle();
    }

    void DokunuslariIsle()
    {
        float olcek = canvas.scaleFactor;
        float yaricap = JoystickYaricapi * olcek;
        bool joyGoruldu = false;
        bool kameraGoruldu = false;

        for (int i = 0; i < Girdi.Dokunuslar.Count; i++)
        {
            Dokunus d = Girdi.Dokunuslar[i];

            if (d.basladi)
            {
                if (buton.gameObject.activeSelf && ButonIcinde(d.konum, olcek))
                {
                    EtkilesimTarayici.ButonBasildi = true;
                    continue;
                }
                if (joyParmak == -1 && d.konum.x < Screen.width * 0.45f && d.konum.y < Screen.height * 0.75f)
                {
                    joyParmak = d.id;
                    joyMerkez = d.konum;
                }
                else if (kameraParmak == -1)
                {
                    kameraParmak = d.id;
                }
            }

            if (d.id == joyParmak)
            {
                joyGoruldu = true;
                if (d.bitti)
                {
                    joyParmak = -1;
                    joyGoruldu = false;
                }
                else
                {
                    Vector2 v = (d.konum - joyMerkez) / yaricap;
                    OyuncuHareket.Joystick = Vector2.ClampMagnitude(v, 1f);
                }
            }
            else if (d.id == kameraParmak)
            {
                kameraGoruldu = true;
                if (d.bitti)
                {
                    kameraParmak = -1;
                    kameraGoruldu = false;
                }
                else if (!d.basladi)
                {
                    KameraTakip.DokunmaDonusu += d.delta;
                }
            }
        }

        // Parmak "bitti" bildirmeden kaybolduysa (ör. uygulama arka plana alındı) bırak.
        if (!joyGoruldu)
        {
            joyParmak = -1;
            OyuncuHareket.Joystick = Vector2.zero;
        }
        if (!kameraGoruldu) kameraParmak = -1;

        bool joyAcik = joyParmak != -1;
        if (joyTaban.gameObject.activeSelf != joyAcik)
        {
            joyTaban.gameObject.SetActive(joyAcik);
            joyTopuz.gameObject.SetActive(joyAcik);
        }
        if (joyAcik)
        {
            joyTaban.position = joyMerkez;
            joyTopuz.position = joyMerkez + OyuncuHareket.Joystick * yaricap;
        }
    }

    bool ButonIcinde(Vector2 nokta, float olcek)
    {
        Vector2 merkez = buton.position;
        float yaricap = ButonBoyutu * 0.5f * olcek * 1.15f;
        return (nokta - merkez).sqrMagnitude <= yaricap * yaricap;
    }

    void ButonuGuncelle()
    {
        Etkilesimli secili = EtkilesimTarayici.Secili;
        bool goster = secili != null;
        if (buton.gameObject.activeSelf != goster) buton.gameObject.SetActive(goster);
        if (goster)
        {
            string ipucu = Application.isMobilePlatform ? "" : "[E] ";
            butonYazi.text = ipucu + secili.eylem + "\n" + secili.ad;
        }
    }

    void GostergeleriGuncelle()
    {
        OyunDurumu durum = OyunDurumu.Ornek;
        if (durum != null)
        {
            if (durum.Para != sonPara)
            {
                sonPara = durum.Para;
                paraYazi.text = "Para: " + sonPara + " TL";
            }
            int dakika = Mathf.FloorToInt(durum.Saat * 60f);
            if (dakika != sonDakika)
            {
                sonDakika = dakika;
                saatYazi.text = (dakika / 60 % 24).ToString("00") + ":" + (dakika % 60).ToString("00");
            }
        }

        if (bildirimSuresi > 0f)
        {
            bildirimSuresi -= Time.unscaledDeltaTime;
            if (bildirimSuresi <= 0f) bildirimKutu.gameObject.SetActive(false);
        }
    }

    void BildirimGoster(string mesaj)
    {
        bildirimYazi.text = mesaj;
        bildirimSuresi = 3.5f;
        bildirimKutu.gameObject.SetActive(true);
    }

    void GuvenliAlanUygula()
    {
        // Çentik ve yuvarlak köşeler göstergelerin üstüne gelmesin.
        Rect alan = Screen.safeArea;
        sonGuvenliAlan = alan;
        if (Screen.width <= 0 || Screen.height <= 0) return;
        guvenliAlan.anchorMin = new Vector2(alan.xMin / Screen.width, alan.yMin / Screen.height);
        guvenliAlan.anchorMax = new Vector2(alan.xMax / Screen.width, alan.yMax / Screen.height);
    }

    RectTransform Kutu(string ad, Transform ust, Vector2 kose, Vector2 konum, Vector2 boyut, Color renk, Sprite sekil)
    {
        var nesne = new GameObject(ad, typeof(RectTransform));
        nesne.transform.SetParent(ust, false);
        var rt = (RectTransform)nesne.transform;
        rt.anchorMin = kose;
        rt.anchorMax = kose;
        rt.pivot = kose;
        rt.anchoredPosition = konum;
        rt.sizeDelta = boyut;
        var resim = nesne.AddComponent<Image>();
        resim.sprite = sekil;
        resim.color = renk;
        resim.raycastTarget = false;
        return rt;
    }

    Text Yazi(RectTransform ust, int boyut, TextAnchor hiza, Color renk)
    {
        var nesne = new GameObject("Yazi", typeof(RectTransform));
        nesne.transform.SetParent(ust, false);
        var rt = (RectTransform)nesne.transform;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = new Vector2(10f, 4f);
        rt.offsetMax = new Vector2(-10f, -4f);
        var yazi = nesne.AddComponent<Text>();
        yazi.font = yaziTipi;
        yazi.fontSize = boyut;
        yazi.alignment = hiza;
        yazi.color = renk;
        yazi.raycastTarget = false;
        yazi.horizontalOverflow = HorizontalWrapMode.Wrap;
        yazi.verticalOverflow = VerticalWrapMode.Overflow;
        return yazi;
    }

    static Sprite DaireOlustur()
    {
        const int boyut = 128;
        var doku = new Texture2D(boyut, boyut, TextureFormat.RGBA32, false);
        doku.wrapMode = TextureWrapMode.Clamp;
        var pikseller = new Color32[boyut * boyut];
        float merkez = (boyut - 1) * 0.5f;
        float yaricap = boyut * 0.5f - 1f;
        for (int y = 0; y < boyut; y++)
        {
            for (int x = 0; x < boyut; x++)
            {
                float uzaklik = Mathf.Sqrt((x - merkez) * (x - merkez) + (y - merkez) * (y - merkez));
                float alfa = Mathf.Clamp01(yaricap - uzaklik);
                pikseller[y * boyut + x] = new Color32(255, 255, 255, (byte)(alfa * 255f));
            }
        }
        doku.SetPixels32(pikseller);
        doku.Apply();
        return Sprite.Create(doku, new Rect(0f, 0f, boyut, boyut), new Vector2(0.5f, 0.5f));
    }
}
