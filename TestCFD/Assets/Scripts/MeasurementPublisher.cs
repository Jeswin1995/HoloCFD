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
    public void publishMessage(string msg1, string msg2)
    {
        mqttReceiver.topicPublish = msg1;
        mqttReceiver.messagePublish = msg2;
        mqttReceiver.Publish();
    }
}
