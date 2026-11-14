// Karakter hareketi: kameraya göre yürüme/koşma, yumuşak dönüş, yer çekimi.
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class OyuncuHareket : MonoBehaviour
{
    public float yurumeHizi = 2.4f;
    public float kosmaHizi = 4.8f;
    public float ivme = 12f;
    public float donusHizi = 12f;
    public float yerCekimi = -20f;
    public Transform kamera;

    /// <summary>Ekrandaki sanal joystick değeri; Arayuz tarafından yazılır.</summary>
    public static Vector2 Joystick;

    CharacterController kontrol;
    Vector3 yatayHiz;
    float dikeyHiz;

    void Awake()
    {
        kontrol = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector2 girdi = Girdi.KlavyeHareket + Joystick;
        float siddet = Mathf.Clamp01(girdi.magnitude);
        if (siddet > 1e-4f) girdi = girdi.normalized;

        Vector3 ileri = Vector3.forward;
        Vector3 sag = Vector3.right;
        if (kamera != null)
        {
            ileri = kamera.forward;
            ileri.y = 0f;
            if (ileri.sqrMagnitude < 1e-4f) ileri = kamera.up;
            ileri.y = 0f;
            ileri.Normalize();
            sag = new Vector3(ileri.z, 0f, -ileri.x);
        }

        Vector3 yon = ileri * girdi.y + sag * girdi.x;

        // Joystick sonuna kadar itilince ya da Shift basılıyken koşar.
        bool kosuyor = Girdi.KosTusu || Joystick.magnitude > 0.9f;
        float hedefHiz = (kosuyor ? kosmaHizi : yurumeHizi) * siddet;
        yatayHiz = Vector3.MoveTowards(yatayHiz, yon * hedefHiz, ivme * Time.deltaTime);

        if (yon.sqrMagnitude > 1e-4f && siddet > 0.05f)
        {
            Quaternion hedef = Quaternion.LookRotation(yon, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, hedef, donusHizi * Time.deltaTime);
        }

        if (kontrol.isGrounded && dikeyHiz < 0f) dikeyHiz = -2f;
        dikeyHiz += yerCekimi * Time.deltaTime;

        Vector3 hareket = yatayHiz;
        hareket.y = dikeyHiz;
        kontrol.Move(hareket * Time.deltaTime);
    }
}
