using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Filtering;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

/// <summary>
/// [CORE - Éditeur] Construit les prefabs du TP (énigmes, portes, zone de téléportation).
/// Menu EscapeGame > Configuration > Construire les prefabs.
/// Le devant de chaque énigme est du côté de la flèche bleue (Z) : elle doit pointer vers le joueur.
/// </summary>
public static class EscapeGamePrefabBuilder
{
    public const string PrefabFolder = "Assets/Prefabs";
    public const string PuzzleFolder = "Assets/Prefabs/Enigmes";
    const string MaterialFolder = "Assets/Materials";

    // Couche d'interaction "Teleport" des Starter Assets XRI (bit 31)
    const int TeleportLayerBits = 1 << 31;

    [MenuItem("EscapeGame/Configuration/Construire les prefabs", false, 201)]
    public static void BuildAll()
    {
        CreateFolder("Assets", "Prefabs");
        CreateFolder(PrefabFolder, "Enigmes");
        CreateFolder("Assets", "Materials");

        BuildTeleportZone();
        BuildDungeonDoor();
        BuildStationDoor();
        BuildSimpleButton();
        BuildKeypad();
        BuildKeyLock();
        BuildRotationPuzzle();
        BuildThrowTarget();

        AssetDatabase.SaveAssets();
        Debug.Log("EscapeGame : prefabs construits.");
    }

    // ---------------------------------------------------------------- Prefabs

    static void BuildTeleportZone()
    {
        GameObject root = new GameObject("ZoneTeleportation");
        root.transform.localScale = new Vector3(4f, 0.02f, 4f);
        root.AddComponent<BoxCollider>();
        TeleportationArea area = root.AddComponent<TeleportationArea>();
        area.interactionLayers = TeleportLayerBits;
        root.AddComponent<TeleportZone>();
        Save(root, PrefabFolder + "/ZoneTeleportation.prefab");
    }

    static void BuildDungeonDoor()
    {
        // Arche fixe + grille qui disparaît quand la porte s'ouvre
        GameObject root = new GameObject("Porte_Donjon");
        AddModel(root.transform, "Assets/Kenney/ModularDungeonKit/gate.fbx", "Arche");
        GameObject bars = AddModel(root.transform, "Assets/Kenney/ModularDungeonKit/gate-metal-bars.fbx", "Grille");
        bars.AddComponent<Door>();
        Save(root, PrefabFolder + "/Porte_Donjon.prefab");
    }

    static void BuildStationDoor()
    {
        GameObject root = new GameObject("Porte_Station");
        AddModel(root.transform, "Assets/Kenney/SpaceStationKit/door-double-closed.fbx", "Porte");
        root.AddComponent<Door>();
        Save(root, PrefabFolder + "/Porte_Station.prefab");
    }

    static void BuildSimpleButton()
    {
        CreateFolder(PuzzleFolder, "BoutonSimple");

        GameObject root = new GameObject("BoutonSimple");
        Box(root.transform, "Socle", new Vector3(0f, 0f, -0.02f), new Vector3(0.2f, 0.2f, 0.04f), Mat("Metal_Sombre", new Color(0.2f, 0.2f, 0.22f)), true);

        GameObject button = Cylinder(root.transform, "Bouton", new Vector3(0f, 0f, 0.015f), new Vector3(0.12f, 0.015f, 0.12f), Mat("Bouton_Rouge", new Color(0.85f, 0.1f, 0.1f), 0.6f));
        button.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        ReplaceWithBoxCollider(button);
        MakePressable(root);
        root.AddComponent<SimpleButtonPuzzle>();

        Save(root, PuzzleFolder + "/BoutonSimple/BoutonSimple.prefab");
    }

    static void BuildKeypad()
    {
        CreateFolder(PuzzleFolder, "Keypad");

        GameObject root = new GameObject("Keypad");
        Box(root.transform, "Boitier", new Vector3(0f, 0f, -0.02f), new Vector3(0.3f, 0.44f, 0.04f), Mat("Metal_Sombre", new Color(0.2f, 0.2f, 0.22f)), true);
        Box(root.transform, "Ecran", new Vector3(0f, 0.16f, 0.002f), new Vector3(0.24f, 0.07f, 0.004f), Mat("Ecran", new Color(0.02f, 0.08f, 0.04f)), false);
        TextMeshPro display = Text(root.transform, "Affichage", "", new Vector3(0f, 0.16f, 0.006f), 0.05f, new Color(0.3f, 1f, 0.4f));
        display.rectTransform.sizeDelta = new Vector2(0.22f, 0.06f);

        Keypad keypad = root.AddComponent<Keypad>();
        keypad.display = display;

        string[] keys = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "C", "0", "OK" };
        Material keyMaterial = Mat("Touche", new Color(0.75f, 0.75f, 0.78f));
        for (int i = 0; i < keys.Length; i++)
        {
            int column = i % 3;
            int row = i / 3;
            // Vu de face (Z vers le joueur), la gauche du joueur est en +X
            Vector3 position = new Vector3(0.075f - column * 0.075f, 0.06f - row * 0.075f, 0.01f);

            GameObject key = new GameObject("Touche_" + keys[i]);
            key.transform.SetParent(root.transform, false);
            key.transform.localPosition = position;
            Box(key.transform, "Forme", Vector3.zero, new Vector3(0.06f, 0.06f, 0.02f), keyMaterial, false);
            key.AddComponent<BoxCollider>().size = new Vector3(0.06f, 0.06f, 0.02f);
            Text(key.transform, "Texte", keys[i], new Vector3(0f, 0f, 0.0115f), 0.03f, Color.black);

            MakePressable(key);
            KeypadButton button = key.AddComponent<KeypadButton>();
            button.key = keys[i];
        }

        Save(root, PuzzleFolder + "/Keypad/Keypad.prefab");
    }

    static void BuildKeyLock()
    {
        CreateFolder(PuzzleFolder, "KeyLock");

        // Clé : un anneau, une tige et des dents. La tige pointe vers +Z.
        GameObject key = new GameObject("Cle");
        Material keyMaterial = Mat("Cle_Rouge", new Color(0.8f, 0.1f, 0.1f), 0.8f);
        GameObject ring = Cylinder(key.transform, "Anneau", new Vector3(0f, 0f, -0.035f), new Vector3(0.05f, 0.006f, 0.05f), keyMaterial);
        ring.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
        RemoveCollider(ring);
        Box(key.transform, "Tige", new Vector3(0f, 0f, 0.02f), new Vector3(0.012f, 0.012f, 0.08f), keyMaterial, false);
        Box(key.transform, "Dent1", new Vector3(0f, -0.012f, 0.045f), new Vector3(0.008f, 0.015f, 0.01f), keyMaterial, false);
        Box(key.transform, "Dent2", new Vector3(0f, -0.012f, 0.03f), new Vector3(0.008f, 0.015f, 0.008f), keyMaterial, false);
        BoxCollider keyCollider = key.AddComponent<BoxCollider>();
        keyCollider.center = new Vector3(0f, 0f, 0.005f);
        keyCollider.size = new Vector3(0.015f, 0.05f, 0.13f);
        Rigidbody keyBody = key.AddComponent<Rigidbody>();
        keyBody.mass = 0.1f;
        key.AddComponent<XRGrabInteractable>();
        key.AddComponent<HoverOutline>();
        key.AddComponent<ResetIfFallen>();
        Key keyScript = key.AddComponent<Key>();
        keyScript.keyName = "Clé rouge";
        Save(key, PuzzleFolder + "/KeyLock/Cle.prefab");

        // Serrure : boîtier + trou de serrure. Le trigger détecte la clé.
        GameObject root = new GameObject("Serrure");
        Box(root.transform, "Boitier", new Vector3(0f, 0f, -0.03f), new Vector3(0.16f, 0.2f, 0.06f), Mat("Laiton", new Color(0.7f, 0.55f, 0.2f)), true);
        Box(root.transform, "Trou", new Vector3(0f, 0f, 0.0005f), new Vector3(0.02f, 0.05f, 0.002f), Mat("Noir", Color.black), false);

        GameObject slot = new GameObject("EmplacementCle");
        slot.transform.SetParent(root.transform, false);
        // La clé est enfoncée : sa tige (+Z) rentre dans la serrure (-Z)
        slot.transform.localPosition = new Vector3(0f, 0f, 0.04f);
        slot.transform.localRotation = Quaternion.Euler(0f, 180f, 90f);

        BoxCollider trigger = root.AddComponent<BoxCollider>();
        trigger.isTrigger = true;
        trigger.center = new Vector3(0f, 0f, 0.03f);
        trigger.size = new Vector3(0.12f, 0.14f, 0.08f);

        KeyLock keyLock = root.AddComponent<KeyLock>();
        keyLock.keyName = "Clé rouge";
        keyLock.keySlot = slot.transform;
        Save(root, PuzzleFolder + "/KeyLock/Serrure.prefab");
    }

    static void BuildRotationPuzzle()
    {
        CreateFolder(PuzzleFolder, "RotationPuzzle");

        GameObject root = new GameObject("Molettes");
        Box(root.transform, "Panneau", new Vector3(0f, 0f, -0.02f), new Vector3(0.6f, 0.3f, 0.04f), Mat("Pierre", new Color(0.45f, 0.42f, 0.4f)), true);

        RotationPuzzle puzzle = root.AddComponent<RotationPuzzle>();
        puzzle.dials = new RotatingDial[3];

        string[] symbols = { "A", "B", "C", "D" };
        Material dialMaterial = Mat("Molette", new Color(0.55f, 0.4f, 0.25f));
        Material arrowMaterial = Mat("Bouton_Rouge", new Color(0.85f, 0.1f, 0.1f), 0.6f);

        for (int i = 0; i < 3; i++)
        {
            // Molette 0 à gauche du joueur (+X), molette 2 à droite
            float x = 0.18f - i * 0.18f;

            Box(root.transform, "Repere_" + i, new Vector3(x, 0.105f, 0.005f), new Vector3(0.02f, 0.02f, 0.01f), arrowMaterial, false);

            GameObject dial = new GameObject("Molette_" + i);
            dial.transform.SetParent(root.transform, false);
            dial.transform.localPosition = new Vector3(x, -0.01f, 0.01f);

            GameObject disc = Cylinder(dial.transform, "Disque", Vector3.zero, new Vector3(0.15f, 0.01f, 0.15f), dialMaterial);
            disc.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            RemoveCollider(disc);
            SphereCollider collider = dial.AddComponent<SphereCollider>();
            collider.radius = 0.075f;

            for (int s = 0; s < symbols.Length; s++)
            {
                // Symboles dans le sens des aiguilles d'une montre, vus par le joueur
                float angle = s * Mathf.PI * 2f / symbols.Length;
                Vector3 position = new Vector3(-Mathf.Sin(angle) * 0.05f, Mathf.Cos(angle) * 0.05f, 0.0105f);
                TextMeshPro label = Text(dial.transform, "Symbole_" + symbols[s], symbols[s], position, 0.03f, Color.white);
                label.transform.localRotation = Quaternion.Euler(0f, 180f, -s * 360f / symbols.Length);
            }

            MakePressable(dial);
            RotatingDial rotatingDial = dial.AddComponent<RotatingDial>();
            rotatingDial.symbolCount = symbols.Length;
            puzzle.dials[i] = rotatingDial;
        }

        Save(root, PuzzleFolder + "/RotationPuzzle/Molettes.prefab");
    }

    static void BuildThrowTarget()
    {
        CreateFolder(PuzzleFolder, "ThrowTarget");

        // Projectile
        GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ball.name = "Projectile";
        ball.transform.localScale = Vector3.one * 0.08f;
        ball.GetComponent<MeshRenderer>().sharedMaterial = Mat("Projectile", new Color(1f, 0.75f, 0.1f));
        Rigidbody ballBody = ball.AddComponent<Rigidbody>();
        ballBody.mass = 0.2f;
        ballBody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        ball.AddComponent<XRGrabInteractable>();
        ball.AddComponent<HoverOutline>();
        ball.AddComponent<ResetIfFallen>();
        ball.AddComponent<Throwable>();
        Save(ball, PuzzleFolder + "/ThrowTarget/Projectile.prefab");

        // Cible
        GameObject root = new GameObject("Cible");
        Material white = Mat("Cible_Blanc", new Color(0.95f, 0.95f, 0.95f));
        Material red = Mat("Bouton_Rouge", new Color(0.85f, 0.1f, 0.1f), 0.6f);
        float[] sizes = { 0.6f, 0.45f, 0.3f, 0.15f };
        for (int i = 0; i < sizes.Length; i++)
        {
            Material ringMaterial = white;
            if (i % 2 == 0)
            {
                ringMaterial = red;
            }
            GameObject ring = Cylinder(root.transform, "Anneau_" + i, new Vector3(0f, 0f, 0.002f * i), new Vector3(sizes[i], 0.01f, sizes[i]), ringMaterial);
            ring.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            RemoveCollider(ring);
        }
        BoxCollider targetCollider = root.AddComponent<BoxCollider>();
        targetCollider.size = new Vector3(0.6f, 0.6f, 0.04f);

        TextMeshPro counter = Text(root.transform, "Compteur", "0 / 3", new Vector3(0f, 0.4f, 0f), 0.1f, Color.white);
        counter.rectTransform.sizeDelta = new Vector2(0.5f, 0.15f);

        ThrowTarget target = root.AddComponent<ThrowTarget>();
        target.counter = counter;
        Save(root, PuzzleFolder + "/ThrowTarget/Cible.prefab");
    }

    // ---------------------------------------------------------------- Outils

    // Interactions "appui" : doigt (poke) ou rayon + contour au survol
    static void MakePressable(GameObject target)
    {
        target.AddComponent<XRSimpleInteractable>();
        XRPokeFilter poke = target.AddComponent<XRPokeFilter>();
        poke.pokeConfiguration.Value.pokeDirection = PokeAxis.NegativeZ;
        target.AddComponent<HoverOutline>();
    }

    static GameObject AddModel(Transform parent, string path, string name)
    {
        GameObject model = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        if (model == null)
        {
            Debug.LogError("EscapeGame : modèle introuvable " + path);
            return new GameObject(name);
        }
        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(model);
        instance.name = name;
        instance.transform.SetParent(parent, false);
        return instance;
    }

    static GameObject Box(Transform parent, string name, Vector3 position, Vector3 size, Material material, bool keepCollider)
    {
        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.name = name;
        box.transform.SetParent(parent, false);
        box.transform.localPosition = position;
        box.transform.localScale = size;
        box.GetComponent<MeshRenderer>().sharedMaterial = material;
        if (!keepCollider)
        {
            RemoveCollider(box);
        }
        return box;
    }

    // Cylindre Unity : hauteur 2 sur Y, donc size.y = demi-épaisseur
    static GameObject Cylinder(Transform parent, string name, Vector3 position, Vector3 size, Material material)
    {
        GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        cylinder.name = name;
        cylinder.transform.SetParent(parent, false);
        cylinder.transform.localPosition = position;
        cylinder.transform.localScale = size;
        cylinder.GetComponent<MeshRenderer>().sharedMaterial = material;
        return cylinder;
    }

    static void RemoveCollider(GameObject target)
    {
        Collider collider = target.GetComponent<Collider>();
        if (collider != null)
        {
            Object.DestroyImmediate(collider);
        }
    }

    static void ReplaceWithBoxCollider(GameObject target)
    {
        RemoveCollider(target);
        target.AddComponent<BoxCollider>();
    }

    // Texte 3D tourné vers +Z (vers le joueur). height = hauteur des lettres en mètres
    static TextMeshPro Text(Transform parent, string name, string text, Vector3 position, float height, Color color)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);
        go.transform.localPosition = position;
        go.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);

        TextMeshPro tmp = go.AddComponent<TextMeshPro>();
        tmp.text = text;
        tmp.fontSize = height * 10f;
        tmp.color = color;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.textWrappingMode = TextWrappingModes.NoWrap;
        tmp.rectTransform.sizeDelta = new Vector2(height * 2f, height * 1.5f);
        return tmp;
    }

    static Material Mat(string name, Color color)
    {
        return Mat(name, color, 0f);
    }

    static Material Mat(string name, Color color, float emission)
    {
        string path = MaterialFolder + "/" + name + ".mat";
        Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (material == null)
        {
            material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            AssetDatabase.CreateAsset(material, path);
        }
        material.SetColor("_BaseColor", color);
        if (emission > 0f)
        {
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", color * emission);
            material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.None;
        }
        EditorUtility.SetDirty(material);
        return material;
    }

    static void Save(GameObject root, string path)
    {
        PrefabUtility.SaveAsPrefabAsset(root, path);
        Object.DestroyImmediate(root);
    }

    public static void CreateFolder(string parent, string name)
    {
        if (!AssetDatabase.IsValidFolder(parent + "/" + name))
        {
            AssetDatabase.CreateFolder(parent, name);
        }
    }
}
