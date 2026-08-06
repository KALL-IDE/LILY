using System.IO;
using UnityEditor;
using UnityEngine;
#if UNITY_VECTOR_GRAPHICS
using Unity.VectorGraphics;
#endif

public class SvgToPngImporter
{
    [MenuItem("Tools/Generate Lily Icons from SVG")]
    public static void GenerateIcons()
    {
#if !UNITY_VECTOR_GRAPHICS
        Debug.LogError("Please add the Vector Graphics package (Window -> Package Manager -> com.unity.vectorgraphics) and re-open the project.");
        return;
#endif
        string svgPath = "Assets/Icons/lily.svg";
        if (!File.Exists(svgPath)) { Debug.LogError("SVG not found at " + svgPath); return; }
        string svgText = File.ReadAllText(svgPath);

        // Parse SVG to SceneNode
        var sceneInfo = SVGParser.ImportSVG(new System.IO.StringReader(svgText));

        int[] sizes = new int[] { 48, 72, 96, 144, 192, 512 };
        var tessOptions = new VectorUtils.TessellationOptions()
        {
            StepDistance = 1f,
            MaxCordDeviation = 0.5f,
            MaxCordAngle = 20f,
            SamplingStepSize = 0.01f
        };

        foreach (var s in sizes)
        {
            var geoms = VectorUtils.TessellateScene(sceneInfo.Scene, tessOptions);
            var texture = VectorUtils.RenderToTexture2D(geoms, s, s, VectorUtils.Alignment.Center, Vector2.zero, Color.clear);
            byte[] png = texture.EncodeToPNG();
            string outDir = "Assets/Icons/PNG";
            if (!Directory.Exists(outDir)) Directory.CreateDirectory(outDir);
            string outPath = Path.Combine(outDir, $"icon_{s}.png");
            File.WriteAllBytes(outPath, png);
            Debug.Log($"Wrote icon: {outPath}");
        }

        AssetDatabase.Refresh();
        Debug.Log("Finished generating icons. Assign them in Player Settings -> Icon -> Android.");
    }
}
