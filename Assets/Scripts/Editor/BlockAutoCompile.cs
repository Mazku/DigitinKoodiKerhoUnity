using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

[InitializeOnLoad]
public class BlockAutoCompile
{
    const int fileCountPerTick = 10;

    static public bool blockAutoCompile = true;

    static bool blocked = false;

    static DateTime lastSync;

    static string[] directoriesToWatch = new string[]{ "Shaders", "Models", "Textures", "Sprites" };

    static BlockAutoCompile()
    {
        EditorApplication.update += Update;
        lastSync = DateTime.Now; 
    }

    static void Update()
    {
        if (blockAutoCompile)
        {
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode && !blocked)
            {
                Debug.Log("Locking auto compile");
                UnityEditor.EditorPrefs.SetBool("kAutoRefresh", false);
                blocked = true;
                lastSync = DateTime.Now;
            }

            if (blocked && !EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
            {
                Debug.Log("Unlocking auto compile");
                blocked = false;
                UnityEditor.EditorPrefs.SetBool("kAutoRefresh", true);

                AssetDatabase.Refresh();
            }
        }
        DoRefreshJob();
    }

    static List<string> filesToCheck = new List<string>();

    static void DoRefreshJob()
    {
        if (EditorApplication.isPlaying)
        {
            if (filesToCheck.Count == 0)
            {
                UpdateFiles();
            }
            for (int i = 0; i < fileCountPerTick; i++)
            {
                DoWork();
            }
        }
    }

    static void UpdateFiles()
    {
        filesToCheck.Clear();
        foreach (var dir in directoriesToWatch)
        {
            var path = Application.dataPath + "/" + dir + "/";
            if (Directory.Exists(dir))
            {
                filesToCheck.AddRange(Directory.GetFiles(path, "*", SearchOption.AllDirectories));
            }
        }
    }

    static void DoWork()
    {
        if (filesToCheck.Count == 0)
        {
            return;
        }
        var filePath = filesToCheck[0];
        filesToCheck.RemoveAt(0);
        if (filePath.Contains(".meta") || filePath.Contains(".ds_store"))
        {
            return;
        }
        var updateTime = File.GetLastWriteTime(filePath);
        if (updateTime >= lastSync)
        {
            var assetPath = filePath.Substring(filePath.IndexOf("Assets"));
            Debug.Log(assetPath);
            AssetDatabase.ImportAsset(assetPath);
            lastSync = DateTime.Now;
        }
    }
}