using System.IO;
using UnityEditor;
using UnityEngine;

public class GenerateBgm
{
    [MenuItem("Tools/Generate Placeholder BGM")]
    public static void CreateBgmWav()
    {
        int sampleRate = 44100;
        float duration = 4f; // seconds
        int samples = (int)(sampleRate * duration);
        float frequency = 220f; // A3

        float[] data = new float[samples];
        for (int i = 0; i < samples; i++)
        {
            float t = (float)i / sampleRate;
            // simple sine with slow amplitude envelope
            float env = Mathf.Clamp01(1f - t / duration);
            data[i] = 0.25f * Mathf.Sin(2f * Mathf.PI * frequency * t) * env;
        }

        // Convert to 16-bit PCM
        byte[] bytes = new byte[samples * 2];
        int idx = 0;
        for (int i = 0; i < samples; i++)
        {
            short val = (short)(data[i] * short.MaxValue);
            bytes[idx++] = (byte)(val & 0xff);
            bytes[idx++] = (byte)((val >> 8) & 0xff);
        }

        // WAV header
        MemoryStream ms = new MemoryStream();
        BinaryWriter bw = new BinaryWriter(ms);
        bw.Write(new char[4] { 'R','I','F','F' });
        bw.Write(36 + bytes.Length);
        bw.Write(new char[4] { 'W','A','V','E' });
        bw.Write(new char[4] { 'f','m','t',' ' });
        bw.Write(16);
        bw.Write((short)1);
        bw.Write((short)1);
        bw.Write(sampleRate);
        bw.Write(sampleRate * 2);
        bw.Write((short)2);
        bw.Write((short)16);
        bw.Write(new char[4] { 'd','a','t','a' });
        bw.Write(bytes.Length);
        bw.Write(bytes);

        string outDir = "Assets/Audio";
        if (!Directory.Exists(outDir)) Directory.CreateDirectory(outDir);
        string outPath = Path.Combine(outDir, "bgm_placeholder.wav");
        File.WriteAllBytes(outPath, ms.ToArray());
        bw.Close();
        ms.Close();

        AssetDatabase.Refresh();
        Debug.Log("Generated placeholder BGM at " + outPath);
    }
}
