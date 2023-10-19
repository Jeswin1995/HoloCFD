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


#if WINDOWS_UWP
using System.Threading.Tasks;
using Windows.Storage;
#endif

public class InstantiateFBX : MonoBehaviour
{
    string assetName = "reference_velocity.py_timestep1_.glb";
    string saveTo = "";
    public Vector3 spawnPosition;
    public ButtonController controller;
    private LoadAssetFromServer serverScript;
    public float retryInterval = 5f; // Adjust the retry interval as needed
    public int maxCount = 100; // Maximum number of retries before giving up
    private int retryCount = 0;

    //server variables

    public string nextcloudURL = "https://cloud.tuhh.de";
    private string username = "czr6402";
    private string password = "7Zdcg-kAeJQ-pEZJH-Nt7oL-RcaHH";
    public string remoteFilePath = "/remote.php/dav/files/";
    public string localFilePath = "/TestCFD/";
  
    public void CheckLocalFile()
    {
        assetName = controller.temperatureSliderValue + "+" + controller.velocitySliderValue + ".glb";
        // Create a folder inside UWP's local storage
#if WINDOWS_UWP
        StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        saveTo = Path.Combine(localFolder.Path, assetName);
#else
        saveTo = Application.streamingAssetsPath + '/' + assetName;
#endif
        string url = nextcloudURL + remoteFilePath + username + localFilePath;

        if (System.IO.File.Exists(saveTo))
        {
            LoadGLBFile(saveTo);
        }
        else
        {
            StartCoroutine(SaveAndDownload(url, assetName, saveTo));
        }
        
    }

    public IEnumerator SaveAndDownload(string url, string assetName, string saveTo)
    {
        Debug.Log(saveTo);
        transform.gameObject.AddComponent<LoadAssetFromServer>();
        serverScript = transform.GetComponent<LoadAssetFromServer>();
        UnityWebRequest request = serverScript.DownloadAssets(url, assetName, saveTo);
        yield return request;
        Debug.Log("beforeloadingfile");
        if (!request.isHttpError && !request.isNetworkError)
        {
            while (!serverScript.isFileCheckDone && retryCount<maxCount)
            {
                yield return new WaitForSeconds(retryInterval);
                retryCount++;
            }
            Debug.Log("FileCheckDone");
            serverScript.isFileCheckDone = false;
            try
            {
                LoadGLBFile(saveTo);
            }
            catch(Exception e)
            {
                controller.OnSimulate();
                Debug.Log("File dont exist publishing");
            }
        }
        else
        {
            Debug.Log("File not available online");

            controller.OnSimulate();

        }
    }
 
    private void LoadGLBFile(string saveTo)
    {
        GameObject model = Importer.LoadFromFile(saveTo);
        Debug.Log("Load done");
        model.transform.position = spawnPosition;
        MeshCollider meshcollider = model.AddComponent<MeshCollider>();
        meshcollider.convex = true;
        model.AddComponent<ObjectManipulator>();
        model.AddComponent<NearInteractionGrabbable>();
        //File.Delete(saveTo);
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
