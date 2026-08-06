using System.IO;
using UnityEditor;
using UnityEngine;

public class ImportLilyIcons
{
    [MenuItem("Tools/Import Lily Icons from base64")]
    public static void ImportIcons()
    {
        string baseDir = "Assets/Icons/PNG";
        string[] sizes = new string[] { "48", "72", "96", "144", "192", "512" };
        foreach (var s in sizes)
        {
            string b64Path = Path.Combine(baseDir, $"icon_{s}.b64");
            string outPath = Path.Combine(baseDir, $"icon_{s}.png");
            if (!File.Exists(b64Path))
            {
                Debug.LogWarning($"Base64 file not found: {b64Path}");
                continue;
            }
            string b64 = File.ReadAllText(b64Path).Trim();
            byte[] bytes = System.Convert.FromBase64String(b64);
            File.WriteAllBytes(outPath, bytes);
            Debug.Log($"Wrote PNG: {outPath}");
        }
        AssetDatabase.Refresh();
        Debug.Log("Imported lily icons. Assign them in Player Settings -> Icon -> Android.");
    }
}
