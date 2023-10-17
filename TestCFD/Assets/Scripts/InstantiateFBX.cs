using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Siccity.GLTFUtility;
using System.IO;
using System.ComponentModel;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;
using System.Threading.Tasks;
using Microsoft.MixedReality.Toolkit.Utilities.Gltf.Serialization;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Utilities.Gltf.Schema;

#if WINDOWS_UWP
using System.Threading.Tasks;
using Windows.Storage;
#endif

public class InstantiateFBX : MonoBehaviour
{
    string assetName = "reference_velocity.py_timestep1_.glb";
    string defaultLineMaterialName = "Default-Line";
    string saveTo = "";
    bool assetsLoaded = false;
    int counter = 0;
    int index = 0;
    public Vector3 spawnPosition;
    private async void Start()
    {
        // Create a folder inside UWP's local storage
#if WINDOWS_UWP
        StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        saveTo = Path.Combine(localFolder.Path, assetName);
#else
        saveTo = Application.streamingAssetsPath + '/' + assetName;
#endif

        StartCoroutine(SaveAndDownload("http://192.168.8.120:5000/download", assetName));
    }

    public IEnumerator SaveAndDownload(string url, string assetName)
    {

     
        
        //saveTo =  Application.streamingAssetsPath  + '/' + asstName;
        
        Debug.Log(saveTo);
        transform.gameObject.AddComponent<LoadAssetFromServer>();
        transform.GetComponent<LoadAssetFromServer>().DownloadAssets(url, assetName, saveTo);
        //yield return new WaitUntil(() =>File.Exists(saveTo));
        //byte[] gltfData = File.ReadAllBytes(saveTo);

        while (!transform.GetComponent<LoadAssetFromServer>().isFileAvailable)
        {
            yield return null;
            
        }
        Debug.Log("beforeloadingfile");
        try
        {
            GameObject model = Importer.LoadFromFile(saveTo);
            Debug.Log("Load done using Siccity");
            model.transform.position = spawnPosition;
            MeshCollider meshcollider = model.AddComponent<MeshCollider>();
            meshcollider.convex = true;
            model.AddComponent<ObjectManipulator>();
            model.AddComponent<NearInteractionGrabbable>();
            //File.Delete(saveTo);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            LoadGLTFAsync(saveTo);
            Debug.Log("Load done using MRTk");
            
        }
           
                
   

    }
    public async Task<GltfObject> LoadGLTFAsync(string saveTo)
    {
        try
        {
            // Import the GLTF file asynchronously using MRTK GLTFSerialization
            GltfObject importedObject = await GltfUtility.ImportGltfObjectFromPathAsync(saveTo);
            return importedObject;
        }
        catch (Exception ex)
        {
            // Handle any exceptions that may occur during the import
            Debug.LogError($"Error loading GLTF file: {ex.Message}");
            return null; // You may want to handle this error gracefully in your application
        }
    }


#if WINDOWS_UWP
    // Helper method to create a folder asynchronously
    private async Task<StorageFolder> CreateCustomFolder(StorageFolder parentFolder, string folderName)
    {
        try
        {
            StorageFolder customFolder = await parentFolder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);
            return customFolder;
        }
        catch (Exception ex)
        {
            Debug.LogError("Error creating folder: " + ex.Message);
            return null;
        }
    }
#endif

}
