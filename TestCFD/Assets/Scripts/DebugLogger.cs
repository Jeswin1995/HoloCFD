using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugLogger : MonoBehaviour
{
    TMPro.TMP_Text textMesh;

    // Use this for initialization
    void Start()
    {
        textMesh = gameObject.GetComponentInChildren<TextMeshPro>();
    }

    void OnEnable()
    {
        Application.logMessageReceived += LogMessage;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= LogMessage;
    }

    public void LogMessage(string message, string stackTrace, LogType type)
    {
        if (textMesh.text.Length > 300)
        {
            textMesh.text = message + "\n";
        }
        else
        {
            textMesh.text += message + "\n";
        }
    }

}
