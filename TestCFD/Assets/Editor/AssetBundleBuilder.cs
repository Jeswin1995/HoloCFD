using UnityEngine;
using System.IO;
using System.Collections;
using UnityEditor;
using UnityEngine.Networking;
using Unity.EditorCoroutines.Editor;


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
        // specify the output directory
        string outputDirectory = Application.dataPath + "/AssetBundles";
        // Build the asset bundles using the build map.
        BuildPipeline.BuildAssetBundles(outputDirectory, buildMap, BuildAssetBundleOptions.None, BuildTarget.WSAPlayer);
        //Calling Upload Method
        //MonoBehaviour camMono = Camera.main.GetComponent<MonoBehaviour>();
        //camMono.StartCoroutine(UploadAssetsToServer(outputDirectory));
        EditorCoroutineUtility.StartCoroutineOwnerless(UploadAssetsToServer(outputDirectory));
        //File.WriteAllText("Assets/AssetBundles/BuildCompleted.txt", "Build completed. You can now close Unity.");
    }
    static IEnumerator UploadAssetsToServer(string path)
    {
        string uploadURL = "http://localhost:5000/upload";
        WWWForm form = new WWWForm();
        //Storing the files in a form to upload
        string[] filePaths = Directory.GetFiles(path);
        foreach (string filePath in filePaths)
        {
            UnityWebRequest file = UnityWebRequest.Get("file://" + filePath);
            yield return file.SendWebRequest();
            form.AddBinaryData("assetbundles", file.downloadHandler.data, Path.GetFileName(filePath));
        }
        //Upload request
        UnityWebRequest req = UnityWebRequest.Post(uploadURL, form);
        yield return req.SendWebRequest();

        if (req.isHttpError || req.isNetworkError)
        {
            Debug.Log(req.error);
        }
        else
        {
            Debug.Log("Upload Success");
            Debug.Log(req.downloadHandler.text);
            EditorApplication.Exit(0);
        }


    }
}