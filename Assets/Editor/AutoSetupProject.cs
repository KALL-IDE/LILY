using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoSetupProject
{
    [MenuItem("Tools/Auto Setup Project/Generate Scenes & Prefabs")]
    public static void Generate()
    {
        Debug.Log("Auto setup started: creating folders, sprites, prefabs, scenes, and GameManager.");

        // Ensure folders
        EnsureFolder("Assets/Sprites");
        EnsureFolder("Assets/Prefabs");
        EnsureFolder("Assets/Scenes");
        EnsureFolder("Assets/Audio");

        // Create tags
        AddTag("Player");
        AddTag("Crate");
        AddTag("Goal");

        // Create simple sprite PNGs
        string catPath = CreateColoredPng("Assets/Sprites/cat.png", 256, 256, new Color(1f, 0.6f, 0.8f)); // pink
        string cratePath = CreateColoredPng("Assets/Sprites/crate.png", 256, 256, new Color(0.85f, 0.6f, 0.6f)); // darker pink
        string goalPath = CreateColoredPng("Assets/Sprites/goal.png", 256, 256, new Color(1f, 0.6f, 0.45f)); // salmon
        string particlePath = CreateColoredPng("Assets/Sprites/particle.png", 64, 64, new Color(1f,1f,1f));

        AssetDatabase.ImportAsset(catPath, ImportAssetOptions.ForceUpdate);
        AssetDatabase.ImportAsset(cratePath, ImportAssetOptions.ForceUpdate);
        AssetDatabase.ImportAsset(goalPath, ImportAssetOptions.ForceUpdate);
        AssetDatabase.ImportAsset(particlePath, ImportAssetOptions.ForceUpdate);

        // Ensure textures are imported as sprites
        ConfigureTextureAsSprite(catPath);
        ConfigureTextureAsSprite(cratePath);
        ConfigureTextureAsSprite(goalPath);
        ConfigureTextureAsSprite(particlePath);

        AssetDatabase.Refresh();

        // Create prefabs
        GameObject catPrefab = CreatePrefabFromSprite(catPath, "Assets/Prefabs/Cat.prefab", "Player", typeof(UnityEngine.BoxCollider2D), typeof(PlayerController));
        GameObject cratePrefab = CreatePrefabFromSprite(cratePath, "Assets/Prefabs/Crate.prefab", "Crate", typeof(UnityEngine.BoxCollider2D), typeof(Crate));
        GameObject goalPrefab = CreatePrefabFromSprite(goalPath, "Assets/Prefabs/Goal.prefab", "Goal");

        // Create simple particle prefab
        GameObject psGO = new GameObject("Explosion_Particles");
        var ps = psGO.AddComponent<ParticleSystem>();
        var main = ps.main;
        main.startColor = Color.yellow;
        PrefabUtility.SaveAsPrefabAsset(psGO, "Assets/Prefabs/Explosion.prefab");
        Object.DestroyImmediate(psGO);

        // Create GameManager prefab
        GameObject gmGO = new GameObject("GameManager");
        var gm = gmGO.AddComponent<GameManager>();
        gm.explosionParticles = null; // leave null; assign prefab in scene
        gm.explodeClip = null;
        PrefabUtility.SaveAsPrefabAsset(gmGO, "Assets/Prefabs/GameManager.prefab");
        Object.DestroyImmediate(gmGO);

        // Create Level1 scene
        Scene levelScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        EditorSceneManager.SetActiveScene(levelScene);

        // Instantiate manager in scene and assign win panel
        GameObject sceneGM = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/GameManager.prefab"));
        sceneGM.name = "GameManager";
        var audioSource = sceneGM.AddComponent<AudioSource>();
        audioSource.loop = true;

        // Create Canvas and WinPanel
        GameObject canvas = new GameObject("Canvas", typeof(UnityEngine.UI.CanvasScaler), typeof(UnityEngine.Canvas));
        var canvasComp = canvas.GetComponent<UnityEngine.Canvas>();
        canvasComp.renderMode = RenderMode.ScreenSpaceOverlay;
        GameObject winPanel = new GameObject("WinPanel", typeof(RectTransform), typeof(UnityEngine.UI.Image));
        winPanel.transform.SetParent(canvas.transform);
        winPanel.SetActive(false);
        var gmComp = sceneGM.GetComponent<GameManager>();
        if (gmComp != null)
            gmComp.winPanel = winPanel;

        // Instantiate Player and prefabs
        var catInstance = (GameObject)PrefabUtility.InstantiatePrefab(catPrefab);
        catInstance.transform.position = new Vector3(-2, 0, 0);

        var crate1 = (GameObject)PrefabUtility.InstantiatePrefab(cratePrefab);
        crate1.transform.position = new Vector3(0, 0, 0);
        var crate2 = (GameObject)PrefabUtility.InstantiatePrefab(cratePrefab);
        crate2.transform.position = new Vector3(1, 0, 0);

        var goal1 = (GameObject)PrefabUtility.InstantiatePrefab(goalPrefab);
        goal1.transform.position = new Vector3(2, 0, 0);
        var goal2 = (GameObject)PrefabUtility.InstantiatePrefab(goalPrefab);
        goal2.transform.position = new Vector3(3, 0, 0);

        // Save Level1 scene
        string levelPath = "Assets/Scenes/Level1.unity";
        EditorSceneManager.SaveScene(levelScene, levelPath);

        // Create MainMenu scene
        Scene menuScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        EditorSceneManager.SetActiveScene(menuScene);

        // Create MainMenu UI
        GameObject menuCanvas = new GameObject("Canvas", typeof(UnityEngine.UI.CanvasScaler), typeof(UnityEngine.Canvas));
        menuCanvas.GetComponent<UnityEngine.Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        GameObject menuController = new GameObject("MainMenuController");
        menuController.AddComponent<MainMenuController>();

        // Save MainMenu scene
        string menuPath = "Assets/Scenes/MainMenu.unity";
        EditorSceneManager.SaveScene(menuScene, menuPath);

        // Set Player Settings defaults
        PlayerSettings.applicationIdentifier = "com.yourname.lily";
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeRight;

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Auto setup complete. Open Level1 scene to inspect and build the project.");
    }

    static void EnsureFolder(string path)
    {
        if (!AssetDatabase.IsValidFolder(path))
        {
            string parent = Path.GetDirectoryName(path).Replace("\\", "/");
            string folder = Path.GetFileName(path);
            AssetDatabase.CreateFolder(parent == "" ? "Assets" : parent, folder);
        }
    }

    static string CreateColoredPng(string path, int w, int h, Color color)
    {
        Texture2D tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
        Color[] pix = new Color[w * h];
        for (int i = 0; i < pix.Length; i++) pix[i] = color;
        tex.SetPixels(pix);
        tex.Apply();
        byte[] png = tex.EncodeToPNG();
        Object.DestroyImmediate(tex);
        File.WriteAllBytes(path, png);
        return path;
    }

    static void ConfigureTextureAsSprite(string assetPath)
    {
        var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        if (importer != null)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.mipmapEnabled = false;
            importer.SaveAndReimport();
        }
    }

    static GameObject CreatePrefabFromSprite(string spriteAssetPath, string prefabPath, string tag = null, params System.Type[] extraComponents)
    {
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spriteAssetPath);
        GameObject go = new GameObject(Path.GetFileNameWithoutExtension(prefabPath));
        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        if (!string.IsNullOrEmpty(tag)) go.tag = tag;
        foreach (var t in extraComponents)
        {
            go.AddComponent(t);
        }
        // Save prefab
        string dir = Path.GetDirectoryName(prefabPath).Replace("\\", "/");
        if (!AssetDatabase.IsValidFolder(dir)) AssetDatabase.CreateFolder("Assets", "Prefabs");
        var prefab = PrefabUtility.SaveAsPrefabAsset(go, prefabPath);
        Object.DestroyImmediate(go);
        return prefab as GameObject;
    }

    static void AddTag(string tag)
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");

        // Check if tag already present
        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals(tag)) return;
        }

        tagsProp.InsertArrayElementAtIndex(0);
        tagsProp.GetArrayElementAtIndex(0).stringValue = tag;
        tagManager.ApplyModifiedProperties();
    }
}
