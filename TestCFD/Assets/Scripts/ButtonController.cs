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
    public TMPro.TMP_Dropdown dropdown;
    string dropdown_Topic = "sim_control_mesh"; // MQTT topic for the dropdown value

    string velocity_Topic = "sim_control_velocity";
    string temperature_Topic = "sim_control_temperature";
    public string velocitySliderValue;
    public string temperatureSliderValue;
    public string dropdownValue;

    public void Start()
    {
        velocitySlider.onValueChanged.AddListener(OnVelocityValueChanged);
        temperatureSlider.onValueChanged.AddListener(OnTemperatureValueChanged);
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    public void OnVelocityValueChanged(string value)
    {
        velocitySliderValue = value;
    }

    public void OnTemperatureValueChanged(string value)
    {
        temperatureSliderValue = value;
    }

    private void OnDropdownValueChanged(int index)
    {
        // Get the currently selected dropdown option
        dropdownValue = dropdown.options[dropdown.value].text;
        Debug.Log(dropdownValue);
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

    private IEnumerator CheckDropdownValue()
    {
        // Check if the value has changed since the last time
        string messageToSend = dropdown.options[dropdown.value].text;

        // Publish the message
        mqttReceiver.GetComponent<MeasurementPublisher>().publishMessage(dropdown_Topic, messageToSend);
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
        StartCoroutine(CallAllFunctionsSequentially());
        simulatButton.IsEnabled = false;
        Debug.Log("Simulate started");
    }
    private IEnumerator CallAllFunctionsSequentially()
    {
        yield return StartCoroutine(OnVelocityChanged());
        yield return StartCoroutine(OnTemperatureChanged());
        yield return StartCoroutine(CheckDropdownValue());
        //yield return StartCoroutine (OnSimulateButtonPressed());

    }

    
}
