// Etkileşime girilebilen nesne. Oyuncuya en yakın olan EtkilesimTarayici
// tarafından seçilir; E tuşu ya da ekrandaki buton ile kullanılır.
using System;
using System.Collections.Generic;
using UnityEngine;

public class Etkilesimli : MonoBehaviour
{
    public static readonly List<Etkilesimli> Hepsi = new List<Etkilesimli>();

    public string ad = "Nesne";
    public string eylem = "Kullan";
    public float menzil = 2.2f;
    public Action Kullanildi;

    void OnEnable()
    {
        Hepsi.Add(this);
    }

    void OnDisable()
    {
        Hepsi.Remove(this);
    }

    public void Kullan()
    {
        Kullanildi?.Invoke();
    }
}
