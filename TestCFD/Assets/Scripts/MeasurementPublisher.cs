using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeasurementPublisher : MonoBehaviour
{
    mqttReceiver mqttReceiver;
    private void Awake()
    {
        mqttReceiver = GetComponent<mqttReceiver>();
    }
    public void publishMessage(string msg)
    {
        mqttReceiver.messagePublish = msg;
        mqttReceiver.Publish();
    }
}
