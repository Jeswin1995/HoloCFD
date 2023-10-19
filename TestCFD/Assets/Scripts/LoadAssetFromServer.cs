using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


public class LoadAssetFromServer : MonoBehaviour
{
    private string saveTo = "";
    public float retryInterval = 5f; // Adjust the retry interval as needed
    public int maxRetries = 100; // Maximum number of retries before giving up

    private int retryCount = 0;
    // Signal to indicate that the file is available
    public bool isFileCheckDone = false;

    private string nextcloudURL = "https://cloud.tuhh.de";
    private string username = "czr6402";
    private string password = "7Zdcg-kAeJQ-pEZJH-Nt7oL-RcaHH";
    private string remoteFilePath = "/remote.php/dav/files/";
    private string localFilePath = "/TestCFD/";

    public event Action<UnityWebRequest, bool> OnDownloadCompleted;

    // Method to be called from outside the script
    public UnityWebRequest DownloadAssets(string url, string assetName, string saveTo)
    {
        UnityWebRequest request = CreateDownloadRequest(url, assetName, saveTo);
        StartCoroutine(ProcessRequest(request));
        return request;
    }

    private UnityWebRequest CreateDownloadRequest(string url, string assetName, string saveTo)
    {
        url = nextcloudURL + remoteFilePath + username + localFilePath;
        UnityWebRequest request = UnityWebRequest.Get(url + '/' + assetName);
        request.downloadHandler = new DownloadHandlerFile(saveTo);
        request.SetRequestHeader("Authorization", "Basic " + System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(username + ":" + password)));
        return request;
    }

    private IEnumerator ProcessRequest(UnityWebRequest request)
    {
        yield return request.SendWebRequest();

        if (!request.isNetworkError && !request.isHttpError)
        {
            while (!System.IO.File.Exists(saveTo) && retryCount<maxRetries)
            {
                yield return new WaitForSeconds(retryInterval);
                Debug.LogWarning("File not available yet");
                retryCount++;
            }
            // The file is available, process the request here
            Debug.Log("File is available!");
             // Signal that the file is available

        }
        else
        {
            // The file is not available yet, retry after the specified interval
            Debug.LogWarning("File not found");
        }
        isFileCheckDone = true;
    }
}