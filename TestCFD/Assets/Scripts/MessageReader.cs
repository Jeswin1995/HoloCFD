using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public class MessageReader : MonoBehaviour
{
    public mqttReceiver mqttReceiver; // Reference to the mqttReceiver script
    public Interactable simulateButton;
    private InstantiateFBX instantiateScript;

    private void OnEnable()
    {
        // Subscribe to the OnMessageArrived event
        mqttReceiver.OnMessageArrived += HandleMessage;
        instantiateScript = transform.GetComponent<InstantiateFBX>();
    }

    private void OnDisable()
    {
        // Unsubscribe from the event to prevent memory leaks
        mqttReceiver.OnMessageArrived -= HandleMessage;
    }

    private void HandleMessage(string newMsg)
    {
        // Handle the received message here
        Debug.Log("Received message: " + newMsg);
        // Split the message by '\n' character
        string[] messages = newMsg.Split('\n');

        if (messages.Length >= 2)
        {
            string firstPart = messages[0];
            string secondPart = messages[1];
            if (secondPart.Trim() == "Success")
            {
                // The second part of the message is "Success," so you can use the first part as a parameter for a function
                instantiateScript.CheckOnline(firstPart + ".glb");
                if (simulateButton.IsEnabled == false)
                {
                    simulateButton.IsEnabled = true;
                }
            }
            else if (secondPart.Trim() == "Error")
            {
                if (simulateButton.IsEnabled == false)
                {
                    simulateButton.IsEnabled = true;
                }
            }
        }
    }
}