using Microsoft.MixedReality.Toolkit.UI;
using System.Threading.Tasks;
using UnityEngine;

public class MessageReader : MonoBehaviour
{
    [SerializeField]
    private GameObject indicatorObject;
    private IProgressIndicator indicator;

    public mqttReceiver mqttReceiver; // Reference to the mqttReceiver script
    public Interactable simulateButton;
    private InstantiateFBX instantiateScript;

    private bool isProcessing = false; // Flag to check if processing is ongoing

    private void OnEnable()
    {
        mqttReceiver.OnMessageArrived += HandleMessage;
        instantiateScript = transform.GetComponent<InstantiateFBX>();
    }

    private void OnDisable()
    {
        if (mqttReceiver != null)
        {
            mqttReceiver.OnMessageArrived -= HandleMessage;
        }
    }

    private async void Start()
    {
        if (indicatorObject != null)
        {
            indicator = indicatorObject.GetComponent<IProgressIndicator>();
        }
        else
        {
            Debug.LogError("Indicator object is not assigned.");
        }
    }

    private async void HandleMessage(string newMsg)
    {
        string[] messages = newMsg.Split(new[] { ',' });
        if (messages.Length >= 2)
        {
            string firstPart = messages[0].Trim();
            string statusPart = messages[1].Trim();

            if (!isProcessing && firstPart == "Preconfig" && statusPart == "Success")
            {
                isProcessing = true; // Set the flag to indicate processing has started
                await UpdateProgressAsync(); // Start the progress update process
            }
            else if (isProcessing)
            {
                if (statusPart == "Success")
                {
                    // Handle success message and update progress
                    await UpdateProgressBasedOnMessage(firstPart);
                }
                else if (statusPart == "Error")
                {
                    // If there's an error, reset the progress and complete the task
                    CompleteProgressWithError();
                }
            }
        }
    }

    private async Task UpdateProgressAsync()
    {
        if (indicator != null)
        {
            await indicator.OpenAsync();
            indicator.Progress = 0.1f;
            indicator.Message = "Simulating...";
        }
    }

    private async Task UpdateProgressBasedOnMessage(string part)
    {
        if (indicator == null || instantiateScript == null)
        {
            return;
        }

        switch (part)
        {
            case "Simulation":
                indicator.Progress = 0.5f;
                indicator.Message = "Postprocessing data...";
                break;
            case "Postprocess":
                indicator.Progress = 0.75f;
                indicator.Message = "Uploading to cloud...";
                break;
            case "Cloudupload":
                indicator.Progress = 0.9f;
                indicator.Message = "Finishing Up...";
                break;
            default:
                Debug.LogError("Unhandled message part: " + part);
                indicator.Progress = 1f;
                indicator.Message = "Loading results";
                // Call CheckOnline with the appropriate file
                instantiateScript.CheckOnline(part + ".glb");
                await indicator.CloseAsync();
                isProcessing = false; // Reset the flag
                simulateButton.IsEnabled = true;
                return;  
        }
    }

    private async void CompleteProgressWithError()
    {
        if (indicator != null)
        {
            indicator.Progress = 0;
            indicator.Message = "Error occurred. Resetting progress...";
            await indicator.CloseAsync();
        }
        isProcessing = false; // Reset the flag
        simulateButton.IsEnabled = true;
    }
}
