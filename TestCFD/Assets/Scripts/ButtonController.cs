using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public GameObject mqttReceiver;
    public InstantiateFBX buttonReceiver;
    public sliderTextUpdater velocitySlider;
    public sliderTextUpdater  temperatureSlider;
    public TMPro.TMP_Text text;
    public Interactable simulatButton; 
    public TMPro.TMP_Dropdown dropdown_mesh_pipe;
    public TMPro.TMP_Dropdown dropdown_mesh_baffle;
    public TMPro.TMP_Dropdown dropdown_postprocess;
    string dropdown_mesh_Topic = "sim_control_mesh"; // MQTT topic for the dropdown_mesh_pipe value
    string dropdown_postprocess_Topic = "sim_control_postprocess"; // MQTT topic for the dropdown_postprocess value
    
    string velocity_Topic = "sim_control_velocity";
    string temperature_Topic = "sim_control_temperature";
    public string velocitySliderValue;
    public string temperatureSliderValue;
    public string dropdown_mesh_Value;
    public string dropdown_postprocessValue;

    public void Start()
    {
        velocitySlider.onValueChanged.AddListener(OnVelocityValueChanged);
        temperatureSlider.onValueChanged.AddListener(OnTemperatureValueChanged);
        dropdown_mesh_pipe.onValueChanged.AddListener(OnDropdownMeshValueChanged);
        dropdown_mesh_baffle.onValueChanged.AddListener(OnDropdownMeshValueChanged);
        dropdown_postprocess.onValueChanged.AddListener(OnDropdownPostprocessValueChanged);
    }

    public void OnVelocityValueChanged(string value)
    {
        velocitySliderValue = value;
    }

    public void OnTemperatureValueChanged(string value)
    {
        temperatureSliderValue = value;
    }

    private void OnDropdownMeshValueChanged(int index)
    {
        // Update the dropdown_mesh with the concatenated values of both dropdowns
        dropdown_mesh_Value = "HE" + dropdown_mesh_pipe.options[dropdown_mesh_pipe.value].text +
                        dropdown_mesh_baffle.options[dropdown_mesh_baffle.value].text;
        Debug.Log("Dropdown mesh value updated: " + dropdown_mesh_Value);
    }


    private void OnDropdownPostprocessValueChanged(int index)
    {
        // Get the currently selected dropdown_mesh_pipe option
        dropdown_postprocessValue = dropdown_postprocess.options[dropdown_postprocess.value].text;
        Debug.Log(dropdown_postprocessValue);
    }
    public IEnumerator OnVelocityChanged()
    {
        if (velocitySliderValue != null)
        {
            Debug.Log(velocity_Topic);
            mqttReceiver.GetComponent<MeasurementPublisher>().publishMessage(velocity_Topic, velocitySliderValue);
            Debug.Log("velocityvaluepublished"+ velocitySliderValue);
        }
        yield return new WaitForSeconds(1);
    }
   
  
    public IEnumerator OnTemperatureChanged()
    {
        if (temperatureSliderValue != null)
        {
            mqttReceiver.GetComponent<MeasurementPublisher>().publishMessage(temperature_Topic, temperatureSliderValue);
            Debug.Log("temperaturevaluepublished"+temperatureSliderValue);
        }
        yield return new WaitForSeconds(1);
    }

    private IEnumerator CheckMeshDropdownValue()
    {
        string messageToSend = dropdown_mesh_Value;

        // Publish the message
        mqttReceiver.GetComponent<MeasurementPublisher>().publishMessage(dropdown_mesh_Topic, messageToSend);
        Debug.Log("Dropdown value published: " + messageToSend);

        yield return new WaitForSeconds(1);
    }
    private IEnumerator CheckPostprocessDropdownValue()
    {
        // Check if the value has changed since the last time
        string messageToSend = dropdown_postprocess.options[dropdown_postprocess.value].text;

        // Publish the message
        mqttReceiver.GetComponent<MeasurementPublisher>().publishMessage(dropdown_postprocess_Topic, messageToSend);
        Debug.Log("Dropdown value published: " + messageToSend);

        yield return new WaitForSeconds(1);
    }


    public IEnumerator OnSimulateButtonPressed()
    {
        string topic = "sim_start";
        string start_message = "start";
        mqttReceiver.GetComponent<MeasurementPublisher>().publishMessage(topic, start_message);
        yield return new WaitForSeconds(1);
    }

    public void OnSimulate()
    {
        // Check if the temperatureSliderValue and velocitySliderValue are not null
        // and the dropdown text does not start with "Select"
        if (temperatureSliderValue != null && velocitySliderValue != null &&
            !dropdown_mesh_pipe.options[dropdown_mesh_pipe.value].text.StartsWith("Select") &&
            !dropdown_mesh_pipe.options[dropdown_mesh_baffle.value].text.StartsWith("Select") &&
            !dropdown_postprocess.options[dropdown_postprocess.value].text.StartsWith("Select"))
        {
            StartCoroutine(CallAllFunctionsSequentially());
            simulatButton.IsEnabled = false;
            Debug.Log("Simulate started");
        }
        else
        {
            // Handle the error condition, log an error message or call an error function
            Debug.LogError("Simulation cannot start. Please ensure all selections are made correctly.");
        }
    }

    private IEnumerator CallAllFunctionsSequentially()
    {
        yield return StartCoroutine(OnVelocityChanged());
        yield return StartCoroutine(OnTemperatureChanged());
        yield return StartCoroutine(CheckMeshDropdownValue());
        yield return StartCoroutine(CheckPostprocessDropdownValue());
        //yield return StartCoroutine (OnSimulateButtonPressed());

    }

    
}
