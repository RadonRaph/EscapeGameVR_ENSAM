using SilhouetteOutline;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.XR.Management;
using UnityEditor.XR.Management.Metadata;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR.Management;
using UnityEngine.XR.OpenXR;
using UnityEngine.XR.OpenXR.Features;
using UnityEngine.XR.OpenXR.Features.Interactions;
using UnityEngine.XR.OpenXR.Features.MetaQuestSupport;

/// <summary>
/// [CORE - Éditeur] Configure le projet pour le Meta Quest 3 :
///  - Player Settings Android (ARM64, IL2CPP, Vulkan, API 32+)
///  - XR Plug-in Management + OpenXR (Meta Quest, manettes Touch)
///  - URP : Adaptive Probe Volumes + Renderer Feature "Silhouette Outline"
///  - TextMesh Pro (ressources essentielles)
/// Les étudiants n'ont pas besoin de le relancer.
/// </summary>
public static class EscapeGameProjectSetup
{
    const string OpenXRLoader = "UnityEngine.XR.OpenXR.OpenXRLoader";
    const string XRSettingsPath = "Assets/XR/XRGeneralSettingsPerBuildTarget.asset";
    const string TMPEssentials = "Packages/com.unity.ugui/Package Resources/TMP Essential Resources.unitypackage";

    [MenuItem("EscapeGame/Configuration/Configurer le projet (Quest 3)", false, 200)]
    public static void ConfigureAll()
    {
        ConfigurePlayer();
        ConfigureXR();
        ConfigureURP();
        ImportTextMeshPro();
        AssetDatabase.SaveAssets();
        Debug.Log("EscapeGame : projet configuré pour le Quest 3.");
    }

    // Pour la ligne de commande : configure, passe en Android puis quitte Unity
    public static void ConfigureAllBatch()
    {
        ConfigureAll();
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        EditorApplication.Exit(0);
    }

    static void ConfigurePlayer()
    {
        PlayerSettings.companyName = "TP VR";
        PlayerSettings.productName = "EscapeGameVR";
        PlayerSettings.colorSpace = ColorSpace.Linear;

        NamedBuildTarget android = NamedBuildTarget.Android;
        PlayerSettings.SetApplicationIdentifier(android, "com.tpvr.escapegame");
        PlayerSettings.SetScriptingBackend(android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel32;
        PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.Android, false);
        PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new GraphicsDeviceType[] { GraphicsDeviceType.Vulkan });
    }

    static void ConfigureXR()
    {
        XRGeneralSettingsPerBuildTarget perTarget;
        EditorBuildSettings.TryGetConfigObject(XRGeneralSettings.k_SettingsKey, out perTarget);
        if (perTarget == null)
        {
            perTarget = AssetDatabase.LoadAssetAtPath<XRGeneralSettingsPerBuildTarget>(XRSettingsPath);
        }
        if (perTarget == null)
        {
            if (!AssetDatabase.IsValidFolder("Assets/XR"))
            {
                AssetDatabase.CreateFolder("Assets", "XR");
            }
            perTarget = ScriptableObject.CreateInstance<XRGeneralSettingsPerBuildTarget>();
            AssetDatabase.CreateAsset(perTarget, XRSettingsPath);
        }
        EditorBuildSettings.AddConfigObject(XRGeneralSettings.k_SettingsKey, perTarget, true);

        AssignOpenXR(perTarget, BuildTargetGroup.Android);
        AssignOpenXR(perTarget, BuildTargetGroup.Standalone);

        // Quest 3 en build Android
        OpenXRSettings androidSettings = OpenXRSettings.GetSettingsForBuildTargetGroup(BuildTargetGroup.Android);
        EnableFeature<MetaQuestFeature>(androidSettings);
        EnableFeature<OculusTouchControllerProfile>(androidSettings);
        EnableFeature<MetaQuestTouchPlusControllerProfile>(androidSettings);

        // Quest 3 branché au PC (Link) pour tester dans l'éditeur
        OpenXRSettings standaloneSettings = OpenXRSettings.GetSettingsForBuildTargetGroup(BuildTargetGroup.Standalone);
        EnableFeature<OculusTouchControllerProfile>(standaloneSettings);
        EnableFeature<MetaQuestTouchPlusControllerProfile>(standaloneSettings);
    }

    static void AssignOpenXR(XRGeneralSettingsPerBuildTarget perTarget, BuildTargetGroup group)
    {
        if (!perTarget.HasManagerSettingsForBuildTarget(group))
        {
            perTarget.CreateDefaultManagerSettingsForBuildTarget(group);
        }
        XRGeneralSettings general = perTarget.SettingsForBuildTarget(group);
        general.InitManagerOnStart = true;
        XRPackageMetadataStore.AssignLoader(general.Manager, OpenXRLoader, group);
        EditorUtility.SetDirty(perTarget);
    }

    static void EnableFeature<T>(OpenXRSettings settings) where T : OpenXRFeature
    {
        if (settings == null)
        {
            Debug.LogError("EscapeGame : réglages OpenXR introuvables.");
            return;
        }
        T feature = settings.GetFeature<T>();
        if (feature == null)
        {
            Debug.LogError("EscapeGame : feature OpenXR introuvable : " + typeof(T).Name);
            return;
        }
        feature.enabled = true;
        EditorUtility.SetDirty(feature);
    }

    static void ConfigureURP()
    {
        string[] guids = AssetDatabase.FindAssets("t:UniversalRenderPipelineAsset", new string[] { "Assets/Settings" });
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            UniversalRenderPipelineAsset asset = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(path);
            // Light Probe System = Adaptive Probe Volumes (pas modifiable par script autrement)
            SerializedObject serialized = new SerializedObject(asset);
            serialized.FindProperty("m_LightProbeSystem").intValue = (int)LightProbeSystem.ProbeVolumes;
            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(asset);
        }

        guids = AssetDatabase.FindAssets("t:UniversalRendererData", new string[] { "Assets/Settings" });
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            AddOutlineFeature(AssetDatabase.LoadAssetAtPath<UniversalRendererData>(path));
        }
    }

    static void AddOutlineFeature(UniversalRendererData renderer)
    {
        for (int i = 0; i < renderer.rendererFeatures.Count; i++)
        {
            if (renderer.rendererFeatures[i] is SilhouetteOutlineFeature)
            {
                return;
            }
        }

        SilhouetteOutlineFeature feature = ScriptableObject.CreateInstance<SilhouetteOutlineFeature>();
        feature.name = "Silhouette Outline";
        AssetDatabase.AddObjectToAsset(feature, renderer);

        // Référence les shaders pour qu'ils soient inclus dans le build
        SerializedObject serialized = new SerializedObject(feature);
        serialized.FindProperty("_silhouetteShader").objectReferenceValue = Shader.Find(SilhouetteOutlineFeature.SILHOUETTE_SHADER);
        serialized.FindProperty("_compositeShader").objectReferenceValue = Shader.Find(SilhouetteOutlineFeature.COMPOSITE_SHADER);
        serialized.ApplyModifiedPropertiesWithoutUndo();

        renderer.rendererFeatures.Add(feature);
        renderer.SetDirty();
        EditorUtility.SetDirty(renderer);
    }

    static void ImportTextMeshPro()
    {
        if (AssetDatabase.IsValidFolder("Assets/TextMesh Pro"))
        {
            return;
        }
        AssetDatabase.ImportPackage(TMPEssentials, false);
    }
}
