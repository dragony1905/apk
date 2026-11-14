// Üçüncü şahıs kamera: oyuncunun etrafında döner, yakınlaşır, duvarın içine girmez.
using UnityEngine;

public class KameraTakip : MonoBehaviour
{
    public Transform hedef;
    public float hedefYuksekligi = 1.5f;
    public float mesafe = 4.5f;
    public float enYakin = 1.8f;
    public float enUzak = 8f;
    public float enAltAci = -10f;
    public float enUstAci = 65f;
    public float dokunmaHassasiyeti = 0.18f;
    public float fareHassasiyeti = 0.25f;
    public float yumusaklik = 14f;

    /// <summary>Ekranda sürüklemeyle gelen kamera dönüşü; Arayuz tarafından yazılır.</summary>
    public static Vector2 DokunmaDonusu;

    float yatayAci;
    float dikeyAci = 22f;
    float gecerliMesafe;
    int engelMaskesi;

    void Start()
    {
        gecerliMesafe = mesafe;
        // Oyuncu "Ignore Raycast" (2) katmanında; kamera onu engel saymaz.
        engelMaskesi = ~(1 << 2);
        if (hedef != null) yatayAci = hedef.eulerAngles.y;
    }

    void LateUpdate()
    {
        if (hedef == null) return;

        Vector2 donus = DokunmaDonusu * dokunmaHassasiyeti;
        if (Girdi.FareKameraTusu) donus += Girdi.FareDelta * fareHassasiyeti;
        DokunmaDonusu = Vector2.zero;

        yatayAci += donus.x;
        dikeyAci = Mathf.Clamp(dikeyAci - donus.y, enAltAci, enUstAci);
        mesafe = Mathf.Clamp(mesafe - Girdi.Tekerlek * 0.6f, enYakin, enUzak);

        Quaternion donme = Quaternion.Euler(dikeyAci, yatayAci, 0f);
        Vector3 odak = hedef.position + Vector3.up * hedefYuksekligi;
        Vector3 geri = donme * Vector3.back;

        float istenen = mesafe;
        RaycastHit carpma;
        if (Physics.SphereCast(odak, 0.25f, geri, out carpma, mesafe, engelMaskesi, QueryTriggerInteraction.Ignore))
        {
            istenen = Mathf.Max(0.6f, carpma.distance);
        }

        // Engele anında yaklaş, engel kalkınca yumuşakça uzaklaş.
        if (istenen < gecerliMesafe) gecerliMesafe = istenen;
        else gecerliMesafe = Mathf.Lerp(gecerliMesafe, istenen, yumusaklik * Time.deltaTime);

        transform.position = odak + geri * gecerliMesafe;
        transform.rotation = donme;
    }
}
