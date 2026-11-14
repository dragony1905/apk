// Oyuncunun üzerinde durur; en yakın Etkilesimli nesneyi seçer ve kullanır.
using UnityEngine;

public class EtkilesimTarayici : MonoBehaviour
{
    /// <summary>Şu an kullanılabilecek nesne; yoksa null.</summary>
    public static Etkilesimli Secili { get; private set; }

    /// <summary>Ekrandaki butona basıldığında Arayuz tarafından true yapılır.</summary>
    public static bool ButonBasildi;

    void Update()
    {
        Etkilesimli enYakin = null;
        float enIyi = float.MaxValue;
        Vector3 konum = transform.position;

        for (int i = 0; i < Etkilesimli.Hepsi.Count; i++)
        {
            Etkilesimli e = Etkilesimli.Hepsi[i];
            Vector3 fark = e.transform.position - konum;
            fark.y = 0f;
            float uzaklik = fark.magnitude;
            if (uzaklik <= e.menzil && uzaklik < enIyi)
            {
                enIyi = uzaklik;
                enYakin = e;
            }
        }

        Secili = enYakin;

        if (Secili != null && (Girdi.EtkilesimBasildi || ButonBasildi))
        {
            Secili.Kullan();
        }
        ButonBasildi = false;
    }
}
