using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System;
using System.IO;
using UnityEditor;

public class MQTTassetbundle : MonoBehaviour
{
    private MqttClient mqttClient;

    [Header("MQTT Broker Settings")]
    public string brokerAddress = "mqttBrokerAddress";
    public int brokerPort = 1883; // Default MQTT port
    public string clientId = "UnityClient";
    public string username = "YourUsername";
    public string password = "YourPassword";

    [Header("Asset Bundle Settings")]
    public string outputBundlePath = "Assets/AssetBundles";
    public string bundleName = "myassetbundle";

    private void Start()
    {
        // Create MQTT client instance
        mqttClient = new MqttClient(brokerAddress, brokerPort, false, null, null, MqttSslProtocols.None);

        // Register to message received event
        mqttClient.MqttMsgPublishReceived += HandleMessageReceived;

        // Connect to the MQTT broker
        mqttClient.Connect(clientId, username, password);

        // Subscribe to the MQTT topic where folder information will be sent
        mqttClient.Subscribe(new string[] { "topicName" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
    }

    private void HandleMessageReceived(object sender, MqttMsgPublishEventArgs e)
    {
        // Convert the received MQTT message to a string (it contains the folder path)
        string objFilesFolderPath = System.Text.Encoding.UTF8.GetString(e.Message);

        // Ensure the output directory exists
        if (!Directory.Exists(outputBundlePath))
        {
            Directory.CreateDirectory(outputBundlePath);
        }

        // Asset bundle building process (similar to the previous example)
        // You can use objFilesFolderPath to specify the end folder
        // ...

        Debug.Log("Asset Bundle built from folder: " + objFilesFolderPath);
    }

    private void OnDestroy()
    {
        // Disconnect from the MQTT broker when the object is destroyed
        if (mqttClient != null && mqttClient.IsConnected)
        {
            mqttClient.Disconnect();
        }
    }
}