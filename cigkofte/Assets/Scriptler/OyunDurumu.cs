// Oyunun ortak durumu: para, oyun içi saat, bildirimler ve otomatik kayıt.
using System;
using UnityEngine;

public class OyunDurumu : MonoBehaviour
{
    public static OyunDurumu Ornek { get; private set; }

    /// <summary>Ekranda kısa süreli mesaj göstermek için.</summary>
    public static event Action<string> BildirimGeldi;

    public int Para { get; private set; }
    /// <summary>0–24 arası oyun saati.</summary>
    public float Saat { get; private set; } = 9f;

    [Tooltip("Gerçek 1 saniyede kaç oyun dakikası geçer.")]
    public float zamanHizi = 1f;
    public float otomatikKayitSuresi = 60f;

    public Transform oyuncu;
    public Light gunes;

    float kayitSayaci;

    void Awake()
    {
        Ornek = this;
    }

    public void Uygula(KayitVerisi veri)
    {
        Para = Mathf.Max(0, veri.para);
        Saat = Mathf.Repeat(veri.saat, 24f);
    }

    public void ParaEkle(int miktar)
    {
        Para = Mathf.Max(0, Para + miktar);
    }

    public static void Bildir(string mesaj)
    {
        BildirimGeldi?.Invoke(mesaj);
    }

    void Update()
    {
        Saat = Mathf.Repeat(Saat + Time.deltaTime * zamanHizi / 60f, 24f);
        GunesiGuncelle();

        kayitSayaci += Time.unscaledDeltaTime;
        if (kayitSayaci >= otomatikKayitSuresi)
        {
            Kaydet();
        }
    }

    void GunesiGuncelle()
    {
        if (gunes == null) return;
        // 06:00 gün doğumu, 12:00 tepe, 18:00 gün batımı.
        float aci = (Saat - 6f) / 24f * 360f;
        gunes.transform.rotation = Quaternion.Euler(aci, -35f, 0f);
        float yukseklik = Mathf.Sin(aci * Mathf.Deg2Rad);
        gunes.intensity = Mathf.Lerp(0.05f, 1.15f, Mathf.Clamp01(yukseklik * 2f));
        RenderSettings.ambientLight = Color.Lerp(
            new Color(0.16f, 0.17f, 0.24f), new Color(0.62f, 0.58f, 0.52f), Mathf.Clamp01(yukseklik * 2f + 0.2f));
    }

    public bool Kaydet()
    {
        kayitSayaci = 0f;
        var veri = new KayitVerisi { para = Para, saat = Saat };
        if (oyuncu != null)
        {
            veri.konumVar = true;
            veri.x = oyuncu.position.x;
            veri.y = oyuncu.position.y;
            veri.z = oyuncu.position.z;
            veri.yon = oyuncu.eulerAngles.y;
        }
        return KayitSistemi.Kaydet(veri);
    }

    void OnApplicationPause(bool duraklatildi)
    {
        // Android'de uygulama arka plana alınınca çağrılır.
        if (duraklatildi) Kaydet();
    }

    void OnApplicationQuit()
    {
        Kaydet();
    }
}
