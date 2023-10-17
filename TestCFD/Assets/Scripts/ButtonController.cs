using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public GameObject mqttReceiver;
    public InstantiateFBX buttonReceiver;
    public sliderTextUpdater velocitySlider;
    public sliderTextUpdater  temperatureSlider;
    public TMPro.TMP_Text text;

    string velocity_Topic = "sim_control_velocity";
    string temperature_Topic = "sim_control_temperature";
    public IEnumerator OnVelocityChanged()
    {
        string velocitySliderValue = velocitySlider.velocityUpdatedValue.ToString();
        Debug.Log(velocitySliderValue);
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
        string temperatureSliderValue = temperatureSlider.temperatureUpdatedValue.ToString();
        if (temperatureSliderValue != null)
        {
            mqttReceiver.GetComponent<MeasurementPublisher>().publishMessage(temperature_Topic, temperatureSliderValue);
            Debug.Log("temperaturevaluepublished"+temperatureSliderValue);
        }
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
    }
    private IEnumerator CallAllFunctionsSequentially()
    {
        yield return StartCoroutine(OnVelocityChanged());
        yield return StartCoroutine(OnTemperatureChanged());
        yield return StartCoroutine (OnSimulateButtonPressed());
    
    }

    //public IEnumerator OnRefreshButton()
    //{
    //    StartCoroutine(buttonReceiver.SaveAndDownload("http://localhost:5000/download", "reference_velocity.py_timestep1_.glb"));
    //    //yield return new WaitForSeconds(1);
    //    yield return null;
    //}
}
