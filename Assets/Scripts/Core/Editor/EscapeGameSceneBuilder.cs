using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// [CORE - Éditeur] Construit les scènes du TP :
///  - EscapeGame_Exemple : 2 salles (donjon + station), un bouton ouvre la grille
///  - EscapeGame_Enigmes : banc d'essai des 4 énigmes à compléter
///  - EscapeGame_TP      : point de départ du niveau des étudiants
/// Menu EscapeGame > Configuration > Construire les scènes (écrase les scènes existantes).
/// </summary>
public static class EscapeGameSceneBuilder
{
    const string SceneFolder = "Assets/Scenes";
    const string XROriginPath = "Assets/Samples/XR Interaction Toolkit/3.6.1/Starter Assets/Prefabs/XR Origin (XR Rig).prefab";
    const string Dungeon = "Assets/Kenney/ModularDungeonKit/";
    const string Station = "Assets/Kenney/SpaceStationKit/";
    const string Furniture = "Assets/Kenney/FurnitureKit/";
    const string Puzzles = EscapeGamePrefabBuilder.PuzzleFolder + "/";

    const float DungeonTile = 2.4f;
    const float StationTile = 2.5f;

    [MenuItem("EscapeGame/Configuration/Construire les scènes", false, 202)]
    public static void BuildAll()
    {
        BuildAll(false);
    }

    [MenuItem("EscapeGame/Configuration/Construire les scènes + Generate Lighting", false, 203)]
    public static void BuildAllAndBake()
    {
        BuildAll(true);
    }

    // Pour la ligne de commande : prefabs + scènes + lighting, puis quitte Unity
    public static void BuildAllBatch()
    {
        BuildAllBatchNoExit();
        EditorApplication.Exit(0);
    }

    public static void BuildAllBatchNoExit()
    {
        EscapeGamePrefabBuilder.BuildAll();
        BuildAll(true);
    }

    public static void BuildAll(bool bake)
    {
        BuildExampleScene(bake);
        BuildPuzzleScene(bake);
        BuildStudentScene();

        EditorBuildSettings.scenes = new EditorBuildSettingsScene[]
        {
            new EditorBuildSettingsScene(SceneFolder + "/EscapeGame_TP.unity", true),
            new EditorBuildSettingsScene(SceneFolder + "/EscapeGame_Exemple.unity", true),
            new EditorBuildSettingsScene(SceneFolder + "/EscapeGame_Enigmes.unity", true),
        };
        Debug.Log("EscapeGame : scènes construites.");
    }

    // ---------------------------------------------------------------- Scènes

    static void BuildExampleScene(bool bake)
    {
        Scene scene = NewScene();

        // Salle 1 : donjon (3 x 3 tuiles), grille au nord
        GameObject room1 = new GameObject("Salle1_Donjon");
        DungeonRoom(room1.transform, 3, 3, Vector3.zero, 1);
        GameObject door = Prefab(EscapeGamePrefabBuilder.PrefabFolder + "/Porte_Donjon.prefab", room1.transform, new Vector3(0f, 0f, 1.5f * DungeonTile - 0.35f), 0f);
        Teleport(room1.transform, Vector3.zero, new Vector3(5.6f, 0.02f, 5.6f));
        PointLight(room1.transform, new Vector3(0f, 1.8f, -0.5f), new Color(1f, 0.65f, 0.35f), 6f, 9f);

        // Décor et un objet à attraper
        Static(Model(Furniture + "bookcaseOpen.fbx", room1.transform, new Vector3(-2.6f, 0f, 0.5f), 90f));
        GameObject table = Static(Centered(Model(Furniture + "table.fbx", room1.transform, Vector3.zero, 0f), new Vector3(0f, 0f, 1.2f)));
        Grabbable(Model(Furniture + "bear.fbx", room1.transform, new Vector3(2.2f, 0f, -1.5f), -140f));

        // L'énigme : un bouton posé sur la table, qui ouvre la grille
        GameObject button = Prefab(Puzzles + "BoutonSimple/BoutonSimple.prefab", room1.transform, TopCenter(table) + new Vector3(0f, 0.04f, 0f), 0f);
        button.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        EscapeGamePrefabBuilder.OpenOnSolved(button.GetComponent<SimpleButtonPuzzle>(), button.GetComponent<SimpleButtonPuzzle>().onSolved, Grille(door));
        Label(room1.transform, "Appuie 3 fois sur le bouton rouge", new Vector3(0f, 1.6f, 2.8f), 180f, 0.15f);

        // Salle 2 : station spatiale (3 x 3 tuiles), ouverte au sud vers le donjon
        GameObject room2 = new GameObject("Salle2_Station");
        Vector3 room2Center = new Vector3(0f, 0f, 1.5f * DungeonTile + 1.5f * StationTile);
        StationRoom(room2.transform, 3, 3, room2Center, 1);
        Teleport(room2.transform, room2Center, new Vector3(6.6f, 0.02f, 6.6f));
        PointLight(room2.transform, room2Center + new Vector3(0f, 2.2f, 0f), new Color(0.6f, 0.8f, 1f), 5f, 8f);
        Static(Model(Station + "computer-wide.fbx", room2.transform, room2Center + new Vector3(0f, 0f, 2.6f), 180f));
        Label(room2.transform, "Bravo, vous vous êtes échappé !", room2Center + new Vector3(0f, 1.9f, 3.3f), 180f, 0.2f);

        Spawn(new Vector3(0f, 0f, -2f), 0f);
        Probes(new Vector3(0f, 1.25f, room2Center.z / 2f), new Vector3(12f, 4f, 20f));
        Save(scene, "EscapeGame_Exemple", bake);
    }

    static void BuildPuzzleScene(bool bake)
    {
        Scene scene = NewScene();

        // Une grande salle : une grille par énigme sur le mur nord
        GameObject room = new GameObject("Salle_BancDessai");
        DungeonRoom(room.transform, 5, 3, Vector3.zero, 4);
        Teleport(room.transform, Vector3.zero, new Vector3(10.4f, 0.02f, 5.6f));
        PointLight(room.transform, new Vector3(-3f, 2.1f, 0f), new Color(1f, 0.75f, 0.5f), 4f, 8f);
        PointLight(room.transform, new Vector3(3f, 2.1f, 0f), new Color(1f, 0.75f, 0.5f), 4f, 8f);

        float wallZ = 1.5f * DungeonTile - 0.35f;
        float[] gateX = { -2f * DungeonTile, -DungeonTile, DungeonTile, 2f * DungeonTile };
        GameObject[] doors = new GameObject[4];
        for (int i = 0; i < 4; i++)
        {
            PointLight(room.transform, new Vector3(gateX[i], 2.2f, 0.8f), new Color(1f, 0.85f, 0.65f), 1.5f, 3.5f);
            GameObject gate = Prefab(EscapeGamePrefabBuilder.PrefabFolder + "/Porte_Donjon.prefab", room.transform, new Vector3(gateX[i], 0f, wallZ), 0f);
            doors[i] = Grille(gate);
        }

        // Keypad sur un socle
        Pedestal(room.transform, new Vector3(gateX[0], 0f, 1.6f), 1.0f);
        GameObject keypad = Prefab(Puzzles + "Keypad/Keypad.prefab", room.transform, new Vector3(gateX[0], 1.25f, 1.62f), 180f);
        EscapeGamePrefabBuilder.OpenOnSolved(keypad.GetComponent<Keypad>(), keypad.GetComponent<Keypad>().onSolved, doors[0]);
        Label(room.transform, "Keypad\nCode : 1234", new Vector3(gateX[0], 2.0f, 2.2f), 180f, 0.12f);

        // Serrure sur un socle, la clé sur une table
        Pedestal(room.transform, new Vector3(gateX[1], 0f, 1.6f), 1.0f);
        GameObject keyLock = Prefab(Puzzles + "KeyLock/Serrure.prefab", room.transform, new Vector3(gateX[1], 1.15f, 1.62f), 180f);
        EscapeGamePrefabBuilder.OpenOnSolved(keyLock.GetComponent<KeyLock>(), keyLock.GetComponent<KeyLock>().onSolved, doors[1]);
        GameObject sideTable = Static(Centered(Model(Furniture + "sideTable.fbx", room.transform, Vector3.zero, 0f), new Vector3(gateX[1] + 1f, 0f, 0.6f)));
        Prefab(Puzzles + "KeyLock/Cle_Rouge.prefab", room.transform, TopCenter(sideTable) + new Vector3(0f, 0.05f, 0f), 90f);
        Label(room.transform, "Serrure\nTrouve la clé rouge", new Vector3(gateX[1], 2.0f, 2.2f), 180f, 0.12f);

        // Molettes sur un socle
        Pedestal(room.transform, new Vector3(gateX[2], 0f, 1.6f), 1.0f);
        GameObject dials = Prefab(Puzzles + "RotationPuzzle/Molettes.prefab", room.transform, new Vector3(gateX[2], 1.2f, 1.62f), 180f);
        EscapeGamePrefabBuilder.OpenOnSolved(dials.GetComponent<RotationPuzzle>(), dials.GetComponent<RotationPuzzle>().onSolved, doors[2]);
        Label(room.transform, "Molettes\nSolution : B D C", new Vector3(gateX[2], 2.0f, 2.2f), 180f, 0.12f);

        // Cible contre le mur, projectiles sur une table à 3 m
        GameObject target = Prefab(Puzzles + "ThrowTarget/Cible.prefab", room.transform, new Vector3(gateX[3], 1.4f, 2.2f), 180f);
        EscapeGamePrefabBuilder.OpenOnSolved(target.GetComponent<ThrowTarget>(), target.GetComponent<ThrowTarget>().onSolved, doors[3]);
        GameObject ballTable = Static(Centered(Model(Furniture + "table.fbx", room.transform, Vector3.zero, 0f), new Vector3(gateX[3], 0f, -1.2f)));
        for (int i = 0; i < 3; i++)
        {
            Prefab(Puzzles + "ThrowTarget/Projectile.prefab", room.transform, TopCenter(ballTable) + new Vector3(-0.3f + i * 0.3f, 0.05f, 0f), 0f);
        }
        Label(room.transform, "Cible\nLance les 3 balles", new Vector3(gateX[3], 2.3f, 2.2f), 180f, 0.12f);

        Spawn(new Vector3(0f, 0f, -2f), 0f);
        Probes(new Vector3(0f, 1.25f, 0f), new Vector3(14f, 4f, 9f));
        Save(scene, "EscapeGame_Enigmes", bake);
    }

    static void BuildStudentScene()
    {
        Scene scene = NewScene();

        // Juste un sol, de la lumière et le joueur : à vous de construire le reste !
        GameObject room = new GameObject("Salle1");
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                Static(Model(Dungeon + "template-floor.fbx", room.transform, new Vector3(x * DungeonTile, 0f, z * DungeonTile), 0f));
            }
        }
        Teleport(room.transform, Vector3.zero, new Vector3(7.2f, 0.02f, 7.2f));
        PointLight(room.transform, new Vector3(0f, 2.2f, 0f), new Color(1f, 0.8f, 0.6f), 4f, 8f);

        Spawn(Vector3.zero, 0f);
        Probes(new Vector3(0f, 1.25f, 0f), new Vector3(10f, 4f, 10f));
        Save(scene, "EscapeGame_TP", false);
    }

    // ---------------------------------------------------------------- Salles

    // Salle de donjon : sol + murs. Le mur nord laisse une ouverture sur les tuiles "openings" (bits)
    static void DungeonRoom(Transform parent, int sizeX, int sizeZ, Vector3 center, int northOpenings)
    {
        float halfX = sizeX * DungeonTile / 2f;
        float halfZ = sizeZ * DungeonTile / 2f;

        for (int x = 0; x < sizeX; x++)
        {
            float px = center.x - halfX + DungeonTile * (x + 0.5f);
            for (int z = 0; z < sizeZ; z++)
            {
                float pz = center.z - halfZ + DungeonTile * (z + 0.5f);
                Static(Model(Dungeon + "template-floor.fbx", parent, new Vector3(px, 0f, pz), 0f));
            }

            // Mur nord : ouvert sur les tuiles prévues pour les portes
            if (!IsOpening(x, sizeX, northOpenings))
            {
                Static(Model(Dungeon + "template-wall.fbx", parent, new Vector3(px, 0f, center.z + halfZ), 0f));
            }
            Static(Model(Dungeon + "template-wall.fbx", parent, new Vector3(px, 0f, center.z - halfZ), 180f));
        }

        for (int z = 0; z < sizeZ; z++)
        {
            float pz = center.z - halfZ + DungeonTile * (z + 0.5f);
            Static(Model(Dungeon + "template-wall.fbx", parent, new Vector3(center.x + halfX, 0f, pz), 90f));
            Static(Model(Dungeon + "template-wall.fbx", parent, new Vector3(center.x - halfX, 0f, pz), -90f));
        }
    }

    // 1 ouverture : la tuile du milieu. 4 ouvertures : toutes sauf celle du milieu.
    static bool IsOpening(int x, int sizeX, int openings)
    {
        int middle = sizeX / 2;
        if (openings == 1)
        {
            return x == middle;
        }
        return x != middle;
    }

    // Salle de station spatiale. Le mur sud laisse une ouverture au milieu (southOpenings = 1)
    static void StationRoom(Transform parent, int sizeX, int sizeZ, Vector3 center, int southOpenings)
    {
        float halfX = sizeX * StationTile / 2f;
        float halfZ = sizeZ * StationTile / 2f;

        for (int x = 0; x < sizeX; x++)
        {
            float px = center.x - halfX + StationTile * (x + 0.5f);
            for (int z = 0; z < sizeZ; z++)
            {
                float pz = center.z - halfZ + StationTile * (z + 0.5f);
                // Les dalles font 0.75 m d'épaisseur : on les descend pour que le dessus soit à 0
                Static(Model(Station + "floor.fbx", parent, new Vector3(px, -0.75f, pz), 0f));
            }

            Static(Model(Station + "wall.fbx", parent, new Vector3(px, 0f, center.z + halfZ), 0f));
            if (!(southOpenings == 1 && x == sizeX / 2))
            {
                Static(Model(Station + "wall.fbx", parent, new Vector3(px, 0f, center.z - halfZ), 0f));
            }
        }

        for (int z = 0; z < sizeZ; z++)
        {
            float pz = center.z - halfZ + StationTile * (z + 0.5f);
            Static(Model(Station + "wall.fbx", parent, new Vector3(center.x + halfX, 0f, pz), 90f));
            Static(Model(Station + "wall.fbx", parent, new Vector3(center.x - halfX, 0f, pz), 90f));
        }
    }

    // ---------------------------------------------------------------- Outils

    static Scene NewScene()
    {
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        RenderSettings.skybox = null;
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.18f, 0.18f, 0.22f);
        new GameObject("XR Interaction Manager").AddComponent<XRInteractionManager>();
        return scene;
    }

    static void Spawn(Vector3 position, float angle)
    {
        GameObject origin = Prefab(XROriginPath, null, position, angle);
        origin.name = "XR Origin (Joueur)";
    }

    static GameObject Prefab(string path, Transform parent, Vector3 position, float angle)
    {
        GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        if (asset == null)
        {
            Debug.LogError("EscapeGame : prefab introuvable " + path);
            return new GameObject("MANQUANT " + path);
        }
        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(asset);
        if (parent != null)
        {
            instance.transform.SetParent(parent, false);
        }
        instance.transform.position = position;
        instance.transform.rotation = Quaternion.Euler(0f, angle, 0f);
        return instance;
    }

    static GameObject Model(string path, Transform parent, Vector3 position, float angle)
    {
        return Prefab(path, parent, position, angle);
    }

    // Décor fixe : participe au lighting baked (Contribute GI uniquement).
    // Pas de Batching Static : en VR, le static batching casse le tile-based rendering du Quest.
    static GameObject Static(GameObject target)
    {
        StaticEditorFlags flags = StaticEditorFlags.ContributeGI;
        Transform[] all = target.GetComponentsInChildren<Transform>();
        for (int i = 0; i < all.Length; i++)
        {
            GameObjectUtility.SetStaticEditorFlags(all[i].gameObject, flags);
        }
        return target;
    }

    // Objet de décor que l'on peut attraper
    static void Grabbable(GameObject target)
    {
        MeshCollider[] colliders = target.GetComponentsInChildren<MeshCollider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].convex = true;
        }
        target.AddComponent<Rigidbody>();
        EscapeGamePrefabBuilder.AddHoverOutline(target.AddComponent<XRGrabInteractable>());
        target.AddComponent<ResetIfFallen>();
    }

    // Les modèles Kenney n'ont pas toujours leur pivot au centre :
    // on déplace l'objet pour que le centre de sa boîte soit sur "position" (au sol)
    static GameObject Centered(GameObject target, Vector3 position)
    {
        Bounds bounds = GetBounds(target);
        Vector3 offset = new Vector3(position.x - bounds.center.x, position.y - bounds.min.y, position.z - bounds.center.z);
        target.transform.position += offset;
        return target;
    }

    // Point au centre du dessus d'un objet (pour poser quelque chose dessus)
    static Vector3 TopCenter(GameObject target)
    {
        Bounds bounds = GetBounds(target);
        return new Vector3(bounds.center.x, bounds.max.y, bounds.center.z);
    }

    static Bounds GetBounds(GameObject target)
    {
        Renderer[] renderers = target.GetComponentsInChildren<Renderer>();
        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }
        return bounds;
    }

    // La grille d'une Porte_Donjon : c'est elle que onSolved cache
    static GameObject Grille(GameObject door)
    {
        return door.transform.Find("Grille").gameObject;
    }

    static void Pedestal(Transform parent, Vector3 position, float height)
    {
        GameObject pedestal = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pedestal.name = "Socle";
        pedestal.transform.SetParent(parent, false);
        pedestal.transform.position = position + new Vector3(0f, height / 2f, 0f);
        pedestal.transform.localScale = new Vector3(0.4f, height, 0.4f);
        pedestal.GetComponent<MeshRenderer>().sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Pierre.mat");
        Static(pedestal);
    }

    static void Teleport(Transform parent, Vector3 center, Vector3 size)
    {
        GameObject zone = Prefab(EscapeGamePrefabBuilder.PrefabFolder + "/ZoneTeleportation.prefab", parent, center, 0f);
        zone.transform.localScale = size;
    }

    static void PointLight(Transform parent, Vector3 position, Color color, float intensity, float range)
    {
        GameObject go = new GameObject("Lumiere");
        go.transform.SetParent(parent, false);
        go.transform.position = position;
        Light light = go.AddComponent<Light>();
        light.type = LightType.Point;
        light.color = color;
        light.intensity = intensity;
        light.range = range;
        light.shadows = LightShadows.Soft;
        light.lightmapBakeType = LightmapBakeType.Baked;
        // Ombres précalculées douces : évite les ombres dures en escalier sur les murs
        light.shadowRadius = 0.5f;
    }

    static void Label(Transform parent, string text, Vector3 position, float angle, float height)
    {
        GameObject go = new GameObject("Texte");
        go.transform.SetParent(parent, false);
        go.transform.position = position;
        go.transform.rotation = Quaternion.Euler(0f, angle + 180f, 0f);
        TextMeshPro tmp = go.AddComponent<TextMeshPro>();
        tmp.text = text;
        tmp.fontSize = height * 10f;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.textWrappingMode = TextWrappingModes.NoWrap;
        tmp.rectTransform.sizeDelta = new Vector2(3f, 1f);
    }

    // Adaptive Probe Volume qui couvre tout le niveau
    static void Probes(Vector3 center, Vector3 size)
    {
        GameObject go = new GameObject("Adaptive Probe Volume");
        go.transform.position = center;
        ProbeVolume volume = go.AddComponent<ProbeVolume>();
        volume.mode = ProbeVolume.Mode.Scene;
        volume.size = size;
    }

    static void Save(Scene scene, string name, bool bake)
    {
        string folder = SceneFolder + "/" + name;
        EscapeGamePrefabBuilder.CreateFolder(SceneFolder, name);
        string path = SceneFolder + "/" + name + ".unity";

        // Réglages de lumière : tout en baked, lightmaps légères pour le Quest
        LightingSettings settings = new LightingSettings();
        settings.name = name + "_Lighting";
        settings.bakedGI = true;
        settings.realtimeGI = false;
        settings.lightmapper = LightingSettings.Lightmapper.ProgressiveCPU;
        settings.lightmapResolution = 20f;
        settings.lightmapMaxSize = 1024;
        settings.directionalityMode = LightmapsMode.NonDirectional;
        settings.lightmapCompression = LightmapCompression.NormalQuality;
        AssetDatabase.CreateAsset(settings, folder + "/" + name + "_Lighting.lighting");
        Lightmapping.lightingSettings = settings;

        EditorSceneManager.SaveScene(scene, path);

        // Baking set APV : un par scène (mode "Single Scene" de la fenêtre Lighting)
        ProbeVolumeBakingSet bakingSet = ScriptableObject.CreateInstance<ProbeVolumeBakingSet>();
        AssetDatabase.CreateAsset(bakingSet, folder + "/" + name + "_APV.asset");
        bakingSet.TryAddScene(AssetDatabase.AssetPathToGUID(path));
        EditorUtility.SetDirty(bakingSet);
        AssetDatabase.SaveAssets();

        if (bake)
        {
            Lightmapping.Bake();
        }
        EditorSceneManager.SaveScene(scene, path);
    }
}
