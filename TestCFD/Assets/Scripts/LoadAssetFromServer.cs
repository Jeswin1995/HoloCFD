using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;


public class LoadAssetFromServer : MonoBehaviour
{
    private string saveTo = "";
    public float retryInterval = 5f; // Adjust the retry interval as needed
    public int maxRetries = 1000; // Maximum number of retries before giving up

    private int retryCount = 0;
    // Signal to indicate that the file is available
    public bool isFileAvailable = false;

    private string nextcloudURL = "https://cloud.tuhh.de";
    private string username = "czr6402";
    private string password = "7Zdcg-kAeJQ-pEZJH-Nt7oL-RcaHH";
    private string remoteFilePath = "/remote.php/dav/files/";
    private string localFilePath = "/TestCFD/";


    // Method to be called from outside the script
    public void DownloadAssets(string url, string assetName, string saveTo)
    {
        StartCoroutine(SaveAndDownload(url, assetName, saveTo));
    }

    // Coroutine to download the asset
    public IEnumerator SaveAndDownload(string url, string assetName, string saveTo)
    {
        while (retryCount < maxRetries)
        {
            url = nextcloudURL + remoteFilePath + username + localFilePath;
            UnityWebRequest request = new UnityWebRequest(url + '/' + assetName);
            request.SetRequestHeader("Authorization", "Basic " + System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(username + ":" + password)));
            request.downloadHandler = new DownloadHandlerFile(saveTo);
            yield return request.SendWebRequest();

            if (!request.isNetworkError && !request.isHttpError)
            {
                // The file is available, process the request here
                Debug.Log("File is available!");
                isFileAvailable = true; // Signal that the file is available
                break; // Exit the loop
            }
            else
            {
                // The file is not available yet, retry after the specified interval
                Debug.LogWarning("File not available yet, retrying in " + retryInterval + " seconds...");
                retryCount++;
                yield return new WaitForSeconds(retryInterval);
            }
            //UnityWebRequest request = new UnityWebRequest(url + '/' + assetName);
            //request.downloadHandler = new DownloadHandlerFile(saveTo);
            //yield return request.SendWebRequest();

            //if (!request.isNetworkError && !request.isHttpError)
            //{
            //    // The file is available, process the request here
            //    Debug.Log("File is available!");
            //    isFileAvailable = true; // Signal that the file is available
            //    break; // Exit the loop
            //}
            //else
            //{
            //    // The file is not available yet, retry after the specified interval
            //    Debug.LogWarning("File not available yet, retrying in " + retryInterval + " seconds...");
            //    retryCount++;
            //    yield return new WaitForSeconds(retryInterval);
            //}
        }
    }
}