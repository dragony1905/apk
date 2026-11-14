// Kayıt sistemi: tek JSON dosyası + yedek.
// Önce geçici dosyaya yazılır, sonra asıl dosyanın yerine konur; böylece
// yazma sırasında uygulama kapanırsa eski kayıt bozulmaz.
using System;
using System.IO;
using UnityEngine;

[Serializable]
public class KayitVerisi
{
    public int surum = 1;
    public int para;
    public float saat = 9f;
    public bool konumVar;
    public float x, y, z;
    public float yon;
}

public static class KayitSistemi
{
    static string AnaYol => Path.Combine(Application.persistentDataPath, "kayit.json");
    static string YedekYol => AnaYol + ".yedek";
    static string GeciciYol => AnaYol + ".gecici";

    public static bool Kaydet(KayitVerisi veri)
    {
        try
        {
            File.WriteAllText(GeciciYol, JsonUtility.ToJson(veri, true));
            if (File.Exists(AnaYol))
            {
                File.Copy(AnaYol, YedekYol, true);
                File.Delete(AnaYol);
            }
            File.Move(GeciciYol, AnaYol);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("Kayıt yazılamadı: " + e.Message);
            return false;
        }
    }

    /// <summary>Kayıt yoksa ya da ikisi de bozuksa yeni bir kayıt döner.</summary>
    public static KayitVerisi Yukle()
    {
        return Oku(AnaYol) ?? Oku(YedekYol) ?? new KayitVerisi();
    }

    static KayitVerisi Oku(string yol)
    {
        try
        {
            if (!File.Exists(yol)) return null;
            string metin = File.ReadAllText(yol);
            if (string.IsNullOrWhiteSpace(metin)) return null;
            return JsonUtility.FromJson<KayitVerisi>(metin);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Kayıt okunamadı (" + yol + "): " + e.Message);
            return null;
        }
    }
}
