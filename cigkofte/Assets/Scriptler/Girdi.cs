// Girdi katmanı: dokunmatik, klavye ve fareyi tek yerden okur.
// Unity'nin yeni Input System'i de eski Input Manager'ı da etkin olsa çalışır;
// oyunun geri kalanı yalnızca bu sınıfı kullanır.
using System.Collections.Generic;
using UnityEngine;

public struct Dokunus
{
    public int id;
    public Vector2 konum;
    public Vector2 delta;
    public bool basladi;
    public bool bitti;
}

public static class Girdi
{
    public static readonly List<Dokunus> Dokunuslar = new List<Dokunus>();

    public static Vector2 KlavyeHareket { get; private set; }
    public static bool KosTusu { get; private set; }
    public static bool EtkilesimBasildi { get; private set; }
    public static bool FareKameraTusu { get; private set; }
    public static Vector2 FareDelta { get; private set; }
    public static float Tekerlek { get; private set; }

    static bool hazir;

    /// <summary>Her karede bir kez, diğer scriptlerden önce çağrılır.</summary>
    public static void Guncelle()
    {
        Dokunuslar.Clear();
        KlavyeHareket = Vector2.zero;
        KosTusu = false;
        EtkilesimBasildi = false;
        FareKameraTusu = false;
        FareDelta = Vector2.zero;
        Tekerlek = 0f;

#if ENABLE_INPUT_SYSTEM
        YeniSistemOku();
#elif ENABLE_LEGACY_INPUT_MANAGER
        EskiSistemOku();
#endif
    }

#if ENABLE_INPUT_SYSTEM
    static void YeniSistemOku()
    {
        if (!hazir)
        {
            UnityEngine.InputSystem.EnhancedTouch.EnhancedTouchSupport.Enable();
            hazir = true;
        }

        var dokunuslar = UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches;
        for (int i = 0; i < dokunuslar.Count; i++)
        {
            var t = dokunuslar[i];
            var faz = t.phase;
            Dokunuslar.Add(new Dokunus
            {
                id = t.touchId,
                konum = t.screenPosition,
                delta = t.delta,
                basladi = faz == UnityEngine.InputSystem.TouchPhase.Began,
                bitti = faz == UnityEngine.InputSystem.TouchPhase.Ended
                        || faz == UnityEngine.InputSystem.TouchPhase.Canceled
            });
        }

        var klavye = UnityEngine.InputSystem.Keyboard.current;
        if (klavye != null)
        {
            float x = 0f, y = 0f;
            if (klavye.aKey.isPressed || klavye.leftArrowKey.isPressed) x -= 1f;
            if (klavye.dKey.isPressed || klavye.rightArrowKey.isPressed) x += 1f;
            if (klavye.sKey.isPressed || klavye.downArrowKey.isPressed) y -= 1f;
            if (klavye.wKey.isPressed || klavye.upArrowKey.isPressed) y += 1f;
            KlavyeHareket = new Vector2(x, y);
            KosTusu = klavye.leftShiftKey.isPressed;
            EtkilesimBasildi = klavye.eKey.wasPressedThisFrame;
        }

        var fare = UnityEngine.InputSystem.Mouse.current;
        if (fare != null)
        {
            FareKameraTusu = fare.rightButton.isPressed;
            FareDelta = fare.delta.ReadValue();
            // Yeni sistem tekerleği yaklaşık 120'lik adımlarla verir.
            Tekerlek = fare.scroll.ReadValue().y / 120f;
        }
    }
#elif ENABLE_LEGACY_INPUT_MANAGER
    static Vector3 oncekiFare;

    static void EskiSistemOku()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch t = Input.GetTouch(i);
            Dokunuslar.Add(new Dokunus
            {
                id = t.fingerId,
                konum = t.position,
                delta = t.deltaPosition,
                basladi = t.phase == TouchPhase.Began,
                bitti = t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled
            });
        }

        float x = 0f, y = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) x -= 1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) x += 1f;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) y -= 1f;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) y += 1f;
        KlavyeHareket = new Vector2(x, y);
        KosTusu = Input.GetKey(KeyCode.LeftShift);
        EtkilesimBasildi = Input.GetKeyDown(KeyCode.E);

        if (Input.mousePresent)
        {
            Vector3 simdi = Input.mousePosition;
            if (!hazir)
            {
                oncekiFare = simdi;
                hazir = true;
            }
            FareKameraTusu = Input.GetMouseButton(1);
            FareDelta = simdi - oncekiFare;
            oncekiFare = simdi;
            Tekerlek = Input.mouseScrollDelta.y;
        }
    }
#endif
}
