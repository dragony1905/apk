// Oyunu açılışta kodla kurar: zemin, deneme dükkânı, oyuncu, kamera, ışık ve arayüz.
// Unity editörü kullanılmadığı için sahne dosyası boştur; her şey buradan gelir.
// Buradaki kutular yer tutucudur, gerçek modeller geldiğinde değiştirilecek.
using UnityEngine;
using UnityEngine.Rendering;

public static class OyunKurucu
{
    static readonly Color Krem = new Color(0.93f, 0.89f, 0.78f);
    static readonly Color Kahve = new Color(0.42f, 0.29f, 0.20f);
    static readonly Color Zeytin = new Color(0.42f, 0.47f, 0.25f);
    static readonly Color Kirmizi = new Color(0.75f, 0.26f, 0.20f);
    static readonly Color Kaldirim = new Color(0.62f, 0.60f, 0.55f);

    static Material temelMalzeme;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Kur()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        temelMalzeme = Resources.Load<Material>("TemelMalzeme");

        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.62f, 0.58f, 0.52f);

        KayitVerisi kayit = KayitSistemi.Yukle();

        var dunya = new GameObject("Dunya").transform;
        DunyayiKur(dunya);

        // Işık
        var isikNesnesi = new GameObject("Gunes");
        var gunes = isikNesnesi.AddComponent<Light>();
        gunes.type = LightType.Directional;
        gunes.color = new Color(1f, 0.95f, 0.86f);
        gunes.shadows = LightShadows.Soft;
        gunes.shadowStrength = 0.7f;

        // Oyuncu
        Vector3 baslangic = new Vector3(0f, 0.1f, 6f);
        float yon = 180f;
        if (kayit.konumVar && Mathf.Abs(kayit.x) < 38f && Mathf.Abs(kayit.z) < 38f && kayit.y > -1f && kayit.y < 5f)
        {
            baslangic = new Vector3(kayit.x, kayit.y + 0.05f, kayit.z);
            yon = kayit.yon;
        }
        GameObject oyuncu = OyuncuyuKur(baslangic, yon);

        // Kamera
        var kameraNesnesi = new GameObject("Kamera");
        kameraNesnesi.tag = "MainCamera";
        var kamera = kameraNesnesi.AddComponent<Camera>();
        kamera.clearFlags = CameraClearFlags.SolidColor;
        kamera.backgroundColor = new Color(0.74f, 0.83f, 0.90f);
        kamera.nearClipPlane = 0.1f;
        kamera.farClipPlane = 150f;
        kamera.fieldOfView = 60f;
        kameraNesnesi.AddComponent<AudioListener>();
        var takip = kameraNesnesi.AddComponent<KameraTakip>();
        takip.hedef = oyuncu.transform;
        oyuncu.GetComponent<OyuncuHareket>().kamera = kameraNesnesi.transform;

        // Oyun durumu
        var durum = new GameObject("Oyun").AddComponent<OyunDurumu>();
        durum.oyuncu = oyuncu.transform;
        durum.gunes = gunes;
        durum.Uygula(kayit);

        // Arayüz
        new GameObject("Arayuz").AddComponent<Arayuz>();

        OyunDurumu.Bildir("Hoş geldin! Dükkâna yürü ve tezgâhı dene.");
    }

    static void DunyayiKur(Transform dunya)
    {
        // Zemin: 80 x 80 metre
        Parca(PrimitiveType.Plane, "Zemin", new Vector3(0f, 0f, 0f), new Vector3(8f, 1f, 8f), Kaldirim, dunya);

        // Deneme dükkânı: 8 x 6 metre, önü açık, çatısız (kamera içeriyi görebilsin diye)
        Parca(PrimitiveType.Cube, "DukkanZemini", new Vector3(0f, 0.025f, 0f), new Vector3(8f, 0.05f, 6f), Krem, dunya);
        Parca(PrimitiveType.Cube, "ArkaDuvar", new Vector3(0f, 1.5f, -3f), new Vector3(8.2f, 3f, 0.2f), Krem, dunya);
        Parca(PrimitiveType.Cube, "SolDuvar", new Vector3(-4f, 1.5f, 0f), new Vector3(0.2f, 3f, 6f), Krem, dunya);
        Parca(PrimitiveType.Cube, "SagDuvar", new Vector3(4f, 1.5f, 0f), new Vector3(0.2f, 3f, 6f), Krem, dunya);
        Parca(PrimitiveType.Cube, "Tabela", new Vector3(0f, 3.2f, -3f), new Vector3(5f, 0.6f, 0.25f), Kirmizi, dunya);

        // Tezgâh
        GameObject tezgah = Parca(PrimitiveType.Cube, "Tezgah", new Vector3(0f, 0.5f, -1.6f), new Vector3(3.2f, 1f, 0.8f), Kahve, dunya);
        Etkilesim(tezgah, "Tezgâh", "Hazırla", 2.2f, () =>
            OyunDurumu.Bildir("Çiğköfte hazırlama Aşama 2'de eklenecek."));

        // Kasa
        Parca(PrimitiveType.Cube, "KasaSehpasi", new Vector3(2.8f, 0.5f, 0.6f), new Vector3(1f, 1f, 1f), Kahve, dunya);
        GameObject kasa = Parca(PrimitiveType.Cube, "Kasa", new Vector3(2.8f, 1.15f, 0.6f), new Vector3(0.5f, 0.3f, 0.4f), Kirmizi, dunya);
        Etkilesim(kasa, "Kasa", "Deneme satışı", 1.8f, () =>
        {
            OyunDurumu.Ornek.ParaEkle(25);
            OyunDurumu.Bildir("+25 TL (deneme satışı)");
        });

        // Kayıt defteri
        Parca(PrimitiveType.Cube, "DefterSehpasi", new Vector3(-2.8f, 0.5f, 0.6f), new Vector3(0.8f, 1f, 0.8f), Zeytin, dunya);
        GameObject defter = Parca(PrimitiveType.Cube, "Defter", new Vector3(-2.8f, 1.05f, 0.6f), new Vector3(0.45f, 0.08f, 0.6f), Krem, dunya);
        Etkilesim(defter, "Defter", "Kaydet", 1.8f, () =>
            OyunDurumu.Bildir(OyunDurumu.Ornek.Kaydet() ? "Oyun kaydedildi." : "Kayıt başarısız oldu!"));

        // Sokakta yön bulmayı kolaylaştıran birkaç ağaç ve bir bank
        Agac(new Vector3(-9f, 0f, 7f), dunya);
        Agac(new Vector3(9f, 0f, 8f), dunya);
        Agac(new Vector3(-13f, 0f, -6f), dunya);
        Agac(new Vector3(14f, 0f, -4f), dunya);
        Parca(PrimitiveType.Cube, "Bank", new Vector3(7f, 0.25f, 10f), new Vector3(2f, 0.5f, 0.6f), Kahve, dunya);
    }

    static GameObject OyuncuyuKur(Vector3 konum, float yon)
    {
        var oyuncu = new GameObject("Oyuncu");
        oyuncu.layer = 2; // Ignore Raycast: kamera oyuncuyu engel saymaz
        oyuncu.transform.SetPositionAndRotation(konum, Quaternion.Euler(0f, yon, 0f));

        GameObject govde = Parca(PrimitiveType.Capsule, "Govde", Vector3.zero, new Vector3(0.6f, 0.85f, 0.6f), Zeytin, oyuncu.transform);
        govde.transform.localPosition = new Vector3(0f, 0.85f, 0f);
        govde.transform.localRotation = Quaternion.identity;
        CarpistiriciyiKaldir(govde);

        // Hangi yöne baktığı anlaşılsın diye küçük bir "yüz"
        GameObject yuz = Parca(PrimitiveType.Cube, "Yuz", Vector3.zero, new Vector3(0.3f, 0.12f, 0.12f), Krem, oyuncu.transform);
        yuz.transform.localPosition = new Vector3(0f, 1.4f, 0.28f);
        yuz.transform.localRotation = Quaternion.identity;
        CarpistiriciyiKaldir(yuz);

        var kontrol = oyuncu.AddComponent<CharacterController>();
        kontrol.height = 1.7f;
        kontrol.radius = 0.3f;
        kontrol.center = new Vector3(0f, 0.85f, 0f);
        kontrol.stepOffset = 0.3f;

        oyuncu.AddComponent<OyuncuHareket>();
        oyuncu.AddComponent<EtkilesimTarayici>();
        return oyuncu;
    }

    static void Agac(Vector3 konum, Transform ust)
    {
        Parca(PrimitiveType.Cylinder, "AgacGovdesi", konum + new Vector3(0f, 1f, 0f), new Vector3(0.4f, 1f, 0.4f), Kahve, ust);
        Parca(PrimitiveType.Sphere, "AgacTaci", konum + new Vector3(0f, 2.9f, 0f), new Vector3(2.4f, 2.4f, 2.4f), Zeytin, ust);
    }

    static GameObject Parca(PrimitiveType tip, string ad, Vector3 konum, Vector3 olcek, Color renk, Transform ust)
    {
        GameObject nesne = GameObject.CreatePrimitive(tip);
        nesne.name = ad;
        nesne.transform.SetParent(ust, false);
        nesne.transform.position = konum;
        nesne.transform.localScale = olcek;
        nesne.layer = ust != null ? ust.gameObject.layer : 0;

        var cizici = nesne.GetComponent<Renderer>();
        if (temelMalzeme != null)
        {
            var malzeme = new Material(temelMalzeme);
            malzeme.color = renk;
            cizici.sharedMaterial = malzeme;
        }
        else
        {
            cizici.material.color = renk;
        }
        return nesne;
    }

    static void CarpistiriciyiKaldir(GameObject nesne)
    {
        var carpistirici = nesne.GetComponent<Collider>();
        if (carpistirici != null)
        {
            carpistirici.enabled = false;
            Object.Destroy(carpistirici);
        }
    }

    static void Etkilesim(GameObject nesne, string ad, string eylem, float menzil, System.Action islem)
    {
        var e = nesne.AddComponent<Etkilesimli>();
        e.ad = ad;
        e.eylem = eylem;
        e.menzil = menzil;
        e.Kullanildi = islem;
    }
}
