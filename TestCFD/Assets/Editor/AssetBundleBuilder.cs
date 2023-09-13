using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using System;


public class AssetBundleBuilder : MonoBehaviour
{
    [MenuItem("Example/Build Asset Bundles Using BuildMap")]
    public static void AssetBundleBuilderFunction()
    {
        // Specify the source directory where folders will be considered as individual asset bundles.
        string sourceDirectory = "Assets/BundleCAD";
        //string sourceDirectory = "C:/Users/XR-Lab/Documents/JeswinFiles/velocity_reference";
        // Get all directories in the specified source directory.
        string[] folders = Directory.GetDirectories(sourceDirectory);

        // Create an array to store AssetBundleBuild information.
        AssetBundleBuild[] buildMap = new AssetBundleBuild[folders.Length];

        for (int i = 0; i < folders.Length; i++)
        {
            string folderPath = folders[i];
            string folderName = Path.GetFileName(folderPath);

            // Define the asset bundle for this folder.
            buildMap[i] = new AssetBundleBuild
            {
                assetBundleName = folderName, // Use the folder name as the asset bundle name.
                assetNames = Directory.GetFiles(folderPath, "*.fbx", SearchOption.AllDirectories) // Get all files recursively in the folder.
            };
        }

        // Build the asset bundles using the build map.
        BuildPipeline.BuildAssetBundles("Assets/AssetBundles", buildMap, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}