using UnityEngine;

public class MessageReader : MonoBehaviour
{
    public mqttReceiver mqttReceiver; // Reference to the mqttReceiver script
    private InstantiateFBX instantiateScript;

    private void OnEnable()
    {
        // Subscribe to the OnMessageArrived event
        instantiateScript = transform.GetComponent<InstantiateFBX>();
        mqttReceiver.OnMessageArrived += HandleMessage;
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

        // Check if the message is "Error" or "Success" and take action accordingly
        if (newMsg == "Success")
        {
            instantiateScript.CheckLocalFile(); 
        }
       
    }
}