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
using UnityEngine.UI;


#if WINDOWS_UWP
using System.Threading.Tasks;
using Windows.Storage;
#endif

public class InstantiateFBX : MonoBehaviour
{
    string assetName = "reference_velocity.py_timestep1_.glb";
    string saveToFolder = "";
    string saveTo = "";
    string urlFolder = "";
    public Vector3 spawnPosition;
    public ButtonController controller;
    private LoadAssetFromServer serverScript;
    public GameObject deleteButtonPrefab;

    public float retryInterval = 5f; // Adjust the retry interval as needed
    public int maxCount = 10; // Maximum number of retries before giving up
    private int retryCount = 0;

    //server variables

    public string nextcloudURL = "https://cloud.tuhh.de";
    private readonly string username = "czr6402";
    public string remoteFilePath = "/remote.php/dav/files/";
    public string localFilePath = "/TestCFD/";

    public void Start()
    {
        serverScript = transform.gameObject.AddComponent<LoadAssetFromServer>();
        serverScript.OnDownloadCompleted += HandleDownloadCompleted;
        saveToFolder = Application.streamingAssetsPath;
        urlFolder = nextcloudURL + remoteFilePath + username + localFilePath;
    }
    public void CheckLocalFile()
    {
        assetName = controller.temperatureSliderValue + "+" + controller.velocitySliderValue + "+" + controller.dropdownValue + ".glb";
        Debug.Log(assetName);
        // Create a folder inside UWP's local storage
#if WINDOWS_UWP
        StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        saveTo = Path.Combine(localFolder.Path, assetName);
#else
        saveTo = saveToFolder + '/' + assetName;
#endif
        if (System.IO.File.Exists(saveTo))
        {
            LoadGLBFile(saveTo);
        }
        else
        {
            CheckOnline(assetName);
        }
        
    }

    public void CheckOnline(string assetName)
    {
        StartCoroutine(SaveAndDownload(assetName));
    }

    public IEnumerator SaveAndDownload(string assetName)
    {
#if WINDOWS_UWP
        StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        saveTo = Path.Combine(localFolder.Path, assetName);
#else
        saveTo = saveToFolder + '/' + assetName;
#endif
        string url = urlFolder + assetName; 
        Debug.Log(saveTo);
        serverScript.DownloadAssets(url, saveTo);
        Debug.Log("beforeloadingfile");
        while (!serverScript.isFileCheckDone || retryCount < maxCount)
        {
            yield return new WaitForSeconds(retryInterval);
            retryCount++;
        }
        Debug.Log("FileCheckDone");
        serverScript.isFileCheckDone = false;
    }

    private void HandleDownloadCompleted(UnityWebRequest request, bool isSuccess, string saveTo)
    {
        if (isSuccess)
        {
            try
            {
                LoadGLBFile(saveTo);
            }
            catch
            {
                controller.OnSimulate();
                Debug.Log("File exists but not loadable");
            }
            // Process the downloaded data here if needed
        }
        else
        {
            Debug.Log("File dont exist publishing");
            controller.OnSimulate();
        }
    }

    private async void LoadGLBFile(string saveTo)
    {
        GameObject model = await Load3DModelAsync(saveTo);
        if (model != null)
        {
            // Attach a delete button script to the model
            GameObject deleteButton = Instantiate(deleteButtonPrefab);
            deleteButton.transform.position = model.transform.position + Vector3.up * 0.4f; // Position above the model

            // Assign the model reference to the delete button script
            DeleteButton deleteButtonScript = deleteButton.GetComponent<DeleteButton>();
            deleteButtonScript.modelToDelete = model;

            // Optionally, you can parent the delete button to the model for easier management
            deleteButton.transform.parent = model.transform;
        }
        Debug.Log("Load done");
        model.transform.position = spawnPosition;
        MeshCollider meshcollider = model.AddComponent<MeshCollider>();
        meshcollider.convex = true;
        model.AddComponent<ObjectManipulator>();
        model.AddComponent<NearInteractionGrabbable>();
        //File.Delete(saveTo);
    }
    async Task<GameObject> Load3DModelAsync(string saveTo)
    {
        ImportSettings importSettings = new ImportSettings(); // You can customize import settings here

        // Wrap the asynchronous call in a Task to return the loaded GameObject
        TaskCompletionSource<GameObject> tcs = new TaskCompletionSource<GameObject>();

        Importer.LoadFromFileAsync(saveTo, importSettings, (model, animations) =>
        {
            // Use the loaded model and animations as needed
            if (model != null)
            {
                tcs.SetResult(model);
            }
            else
            {
                tcs.SetResult(null);
            }
        });

        // Wait for the task to complete and return the result
        return await tcs.Task;
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
