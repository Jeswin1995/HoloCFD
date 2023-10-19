using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine.Events;

public class sliderTextUpdater : MonoBehaviour
{
    public enum MeasurementType
    {
        Velocity,
        Temperature
    }
    public MeasurementType selectedMeasurement;
    private PinchSlider pinchSlider; // Reference to the PinchSlider component on the same GameObject
    public TMPro.TMP_Text textElement; // Reference to the TMP Text element
    //public float newValue ;
    public float velocityUpdatedValue;
    public int temperatureUpdatedValue;
    // Create a Unity Event to be triggered when the value changes
    [System.Serializable]
    public class ValueChangedEvent : UnityEvent<string> { }
    public ValueChangedEvent onValueChanged;

    private void Start()
    {
        // Get the PinchSlider component on the same GameObject
        pinchSlider = GetComponent<PinchSlider>();

        // Ensure that the PinchSlider and TMP Text element are assigned
        if (pinchSlider == null || textElement == null)
        {
            Debug.LogError("PinchSliderValueUpdater: Pinch Slider or TMP Text element is not assigned!");
            return;
        }

        // Subscribe to the OnValueUpdated event of the Pinch Slider
        pinchSlider.OnValueUpdated.AddListener(UpdateTextValue);
    }

    // Callback function to update the TMP Text value when the slider value changes
    private void UpdateTextValue(SliderEventData eventData)
    {
        // Update the TMP Text element with the current value of the Pinch Slider
        //newValue = eventData.NewValue;
        // Perform different calculations based on the selected measurement type
        if (selectedMeasurement == MeasurementType.Velocity)
        {
            velocityUpdatedValue = eventData.NewValue*0.1f;
            string velocityUpdatedValueString = velocityUpdatedValue.ToString("F2");
            // Update the TMP Text element with the calculated value
            textElement.text = velocityUpdatedValueString; // Format the value as desired
            onValueChanged.Invoke(velocityUpdatedValueString);

        }
        else if (selectedMeasurement == MeasurementType.Temperature)
        {
            //newValue *= 1000f;
            temperatureUpdatedValue = Mathf.RoundToInt(eventData.NewValue*1000f);
            string temperatureUpdatedValueString = temperatureUpdatedValue.ToString();
            // Update the TMP Text element with the calculated value
            textElement.text = temperatureUpdatedValue.ToString("F2"); // Format the value as desired
            onValueChanged.Invoke(temperatureUpdatedValueString);
        }
        
    }

}
