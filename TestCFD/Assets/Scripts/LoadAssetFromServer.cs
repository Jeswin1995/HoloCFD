using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;


public class LoadAssetFromServer : MonoBehaviour
{
    public float retryInterval = 5f; // Adjust the retry interval as needed
    public int maxRetries = 10; // Maximum number of retries before giving up

    private int retryCount = 0;
    // Signal to indicate that the file is available
    public bool isFileCheckDone = false;

    
    private readonly string username = "czr6402";
    private readonly string password = "7Zdcg-kAeJQ-pEZJH-Nt7oL-RcaHH";
    

   

    public event Action<UnityWebRequest, bool, string> OnDownloadCompleted;

    // Method to be called from outside the script
    public void DownloadAssets(string url,  string saveTo)
    {
        StartCoroutine(DownloadAndSaveFile(url, saveTo, (req) =>
        {
            if (req.isNetworkError || req.isHttpError)
            {
                Debug.LogError("Download failed: " + req.error);
            }
            else
            {
                Debug.Log("Download successful. File saved at: " + saveTo);
                // You can also process the downloaded data here if needed.
            }

            if (OnDownloadCompleted != null)
            {
                OnDownloadCompleted(req, !req.isNetworkError && !req.isHttpError, saveTo);
            }
        }));
    }

    private IEnumerator DownloadAndSaveFile(string url, string saveTo, Action<UnityWebRequest> callback)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(username + ":" + password)));
        request.downloadHandler = new DownloadHandlerFile(saveTo);
        yield return request.SendWebRequest();
        // Wait until the download is complete
        yield return new WaitUntil(() => request.isDone);
        if (!request.isNetworkError && !request.isHttpError)
        {

            // Wait until the download is complete (progress reaches 1.0)
            yield return new WaitUntil(() => request.downloadProgress == 1.0f);
            while (!System.IO.File.Exists(saveTo) && retryCount < maxRetries)
            {  
                yield return new WaitForSeconds(retryInterval);
                Debug.Log("File not available yet");
                retryCount++;
            }

            if (File.Exists(saveTo))
            {
                // The file is available, process the request here
                Debug.Log("File is available!");
            }
            else
            {
                Debug.Log("File not found");
            }
        }

        

        callback(request);
        isFileCheckDone = true;
    }
}
