// GitHub Actions (GameCI) bu sınıfı çağırarak Android APK üretir.
// Editör elle açılmadığı için sahne, temel malzeme ve oyuncu ayarları da burada hazırlanır.
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class Derleyici
{
    const string SahneYolu = "Assets/Sahneler/Ana.unity";
    const string MalzemeYolu = "Assets/Resources/TemelMalzeme.mat";

    public static void AndroidDerle()
    {
        try
        {
            Hazirla();

            string cikti = Arguman("-customBuildPath");
            if (string.IsNullOrEmpty(cikti)) cikti = "build/Android/CigKofte.apk";
            if (!cikti.EndsWith(".apk", StringComparison.OrdinalIgnoreCase)) cikti += ".apk";
            string klasor = Path.GetDirectoryName(cikti);
            if (!string.IsNullOrEmpty(klasor)) Directory.CreateDirectory(klasor);

            var secenekler = new BuildPlayerOptions
            {
                scenes = new[] { SahneYolu },
                locationPathName = cikti,
                target = BuildTarget.Android,
                options = BuildOptions.None
            };

            BuildReport rapor = BuildPipeline.BuildPlayer(secenekler);
            BuildSummary ozet = rapor.summary;
            Debug.Log("Derleme sonucu: " + ozet.result + ", boyut: " + ozet.totalSize + " bayt, çıktı: " + cikti);
            EditorApplication.Exit(ozet.result == BuildResult.Succeeded ? 0 : 1);
        }
        catch (Exception e)
        {
            Debug.LogError("Derleme hatası: " + e);
            EditorApplication.Exit(1);
        }
    }

    static void Hazirla()
    {
        Directory.CreateDirectory("Assets/Sahneler");
        Directory.CreateDirectory("Assets/Resources");
        AssetDatabase.Refresh();

        // Boş sahne: içerik açılışta OyunKurucu tarafından oluşturulur.
        var sahne = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        EditorSceneManager.SaveScene(sahne, SahneYolu);

        // Kodla üretilen nesnelerin gölgelendiricisi derlemeye girsin diye bir malzeme dosyası.
        if (AssetDatabase.LoadAssetAtPath<Material>(MalzemeYolu) == null)
        {
            Shader golgelendirici = Shader.Find("Standard");
            if (golgelendirici != null)
            {
                var malzeme = new Material(golgelendirici);
                malzeme.SetFloat("_Glossiness", 0.15f);
                AssetDatabase.CreateAsset(malzeme, MalzemeYolu);
            }
            else
            {
                Debug.LogWarning("Standard gölgelendirici bulunamadı; varsayılan malzeme kullanılacak.");
            }
        }
        AssetDatabase.SaveAssets();

        PlayerSettings.companyName = "dragony";
        PlayerSettings.productName = "Çiğköfte Oyunu";
        PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, "com.dragony.cigkofte");

        string surum = Arguman("-buildVersion");
        PlayerSettings.bundleVersion = string.IsNullOrEmpty(surum) || surum == "none" ? "0.1.0" : surum;
        int surumKodu;
        if (int.TryParse(Arguman("-androidVersionCode"), out surumKodu) && surumKodu > 0)
        {
            PlayerSettings.Android.bundleVersionCode = surumKodu;
        }

        // Güncel telefonlar 64 bit ister; bu da IL2CPP gerektirir.
        PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        // Nesneler kodla oluşturulduğu için kullanılmıyor sanılan motor parçaları atılmasın.
        PlayerSettings.stripEngineCode = false;
        PlayerSettings.SetManagedStrippingLevel(NamedBuildTarget.Android, ManagedStrippingLevel.Minimal);

        PlayerSettings.defaultInterfaceOrientation = UIOrientation.AutoRotation;
        PlayerSettings.allowedAutorotateToLandscapeLeft = true;
        PlayerSettings.allowedAutorotateToLandscapeRight = true;
        PlayerSettings.allowedAutorotateToPortrait = false;
        PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;

        EditorUserBuildSettings.buildAppBundle = false;
    }

    static string Arguman(string ad)
    {
        string[] hepsi = Environment.GetCommandLineArgs();
        for (int i = 0; i < hepsi.Length - 1; i++)
        {
            if (hepsi[i] == ad) return hepsi[i + 1];
        }
        return null;
    }
}
