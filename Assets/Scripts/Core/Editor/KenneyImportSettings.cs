using UnityEditor;
using UnityEngine;

/// <summary>
/// [CORE - Éditeur] Réglages d'import automatiques des modèles Kenney.
/// Tout modèle importé dans Assets/Kenney/ reçoit :
///  - Generate Lightmap UVs activé, Min Lightmap Resolution = 5 (lighting baked)
///  - une échelle par kit, pour que 1 unité = 1 mètre en VR
///  - des colliders (Mesh Collider) pour marcher, se téléporter et bloquer les objets
///  - les petits objets (meubles, accessoires) sont éclairés par les probes (APV) et non par les lightmaps :
///    trop petits pour une lightmap, leurs UV se chevaucheraient (overlap)
/// Les réglages faits à la main dans l'Inspector sont écrasés à chaque import.
/// </summary>
public class KenneyImportSettings : AssetPostprocessor
{
    const string KenneyFolder = "Assets/Kenney/";
    const float MinLightmapResolution = 5f;

    // En dessous de cette taille (en mètres), l'objet reçoit sa lumière des probes
    const float PropMaxSize = 2f;

    // Échelle d'import de chaque kit (dossier dans Assets/Kenney/).
    // Pour régler : menu EscapeGame > Kenney > Rapport des tailles, puis ajuster ici.
    const string FurnitureFolder = "Assets/Kenney/FurnitureKit/";
    const float FurnitureScale = 0.2f;     // chaise ~0.9 m, porte ~2 m

    const string DungeonFolder = "Assets/Kenney/ModularDungeonKit/";
    const float DungeonScale = 0.6f;       // couloir ~2.5 m de haut

    const string SpaceStationFolder = "Assets/Kenney/SpaceStationKit/";
    const float SpaceStationScale = 2.5f;  // mur ~2.5 m de haut

    // À incrémenter après avoir changé un réglage ci-dessus :
    // Unity réimporte alors tous les modèles concernés.
    public override uint GetVersion()
    {
        return 3;
    }

    void OnPreprocessModel()
    {
        if (!assetPath.StartsWith(KenneyFolder))
        {
            return;
        }

        ModelImporter importer = (ModelImporter)assetImporter;

        importer.generateSecondaryUV = true;
        importer.secondaryUVMinLightmapResolution = MinLightmapResolution;

        importer.useFileScale = true;
        importer.globalScale = GetKitScale(assetPath);

        importer.addCollider = true;
    }

    void OnPostprocessModel(GameObject model)
    {
        if (!assetPath.StartsWith(KenneyFolder))
        {
            return;
        }

        Vector3 size = GetSize(model);
        float biggest = Mathf.Max(size.x, Mathf.Max(size.y, size.z));
        if (biggest >= PropMaxSize)
        {
            return;
        }

        MeshRenderer[] renderers = model.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].receiveGI = ReceiveGI.LightProbes;
        }
    }

    static float GetKitScale(string path)
    {
        if (path.StartsWith(FurnitureFolder))
        {
            return FurnitureScale;
        }
        if (path.StartsWith(DungeonFolder))
        {
            return DungeonScale;
        }
        if (path.StartsWith(SpaceStationFolder))
        {
            return SpaceStationScale;
        }

        Debug.LogWarning("Kenney : dossier de kit inconnu, échelle 1 utilisée pour " + path);
        return 1f;
    }

    [MenuItem("EscapeGame/Kenney/Réappliquer les réglages d'import", false, 100)]
    static void ReimportAll()
    {
        string[] guids = AssetDatabase.FindAssets("t:Model", new string[] { "Assets/Kenney" });

        AssetDatabase.StartAssetEditing();
        try
        {
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            }
        }
        finally
        {
            AssetDatabase.StopAssetEditing();
        }

        Debug.Log("Kenney : " + guids.Length + " modèles réimportés.");
    }

    // Affiche la taille (en mètres) de chaque modèle dans la Console,
    // pour vérifier l'échelle : une porte doit faire ~2 m de haut, une chaise ~0.9 m.
    [MenuItem("EscapeGame/Kenney/Rapport des tailles", false, 101)]
    static void SizeReport()
    {
        string[] guids = AssetDatabase.FindAssets("t:Model", new string[] { "Assets/Kenney" });
        string report = "Kenney : taille des modèles (largeur x hauteur x profondeur, en m)\n";

        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            GameObject model = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (model == null)
            {
                continue;
            }

            Vector3 size = GetSize(model);
            report += path.Substring(KenneyFolder.Length) + " : "
                + size.x.ToString("0.00") + " x "
                + size.y.ToString("0.00") + " x "
                + size.z.ToString("0.00") + "\n";
        }

        Debug.Log(report);
    }

    static Vector3 GetSize(GameObject model)
    {
        MeshFilter[] filters = model.GetComponentsInChildren<MeshFilter>();
        bool hasBounds = false;
        Bounds bounds = new Bounds();

        for (int i = 0; i < filters.Length; i++)
        {
            if (filters[i].sharedMesh == null)
            {
                continue;
            }

            // Boîte du mesh, avec toutes les scales appliquées (import + transforms)
            Bounds local = filters[i].sharedMesh.bounds;
            Matrix4x4 toModel = filters[i].transform.localToWorldMatrix;
            Vector3 min = local.min;
            Vector3 max = local.max;

            for (int corner = 0; corner < 8; corner++)
            {
                Vector3 point = new Vector3(
                    (corner & 1) == 0 ? min.x : max.x,
                    (corner & 2) == 0 ? min.y : max.y,
                    (corner & 4) == 0 ? min.z : max.z);
                point = toModel.MultiplyPoint3x4(point);

                if (!hasBounds)
                {
                    bounds = new Bounds(point, Vector3.zero);
                    hasBounds = true;
                }
                else
                {
                    bounds.Encapsulate(point);
                }
            }
        }

        return bounds.size;
    }
}
