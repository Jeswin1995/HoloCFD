using UnityEngine;
using TMPro; // Make sure to include the TextMeshPro namespace

public class TextSwitcher : MonoBehaviour
{
    public TextMeshProUGUI textDisplay; // Assign this in the inspector
    private string intro_text = "Welcome to HoloCFD.\r\n\r\nAll the 3D objects are interactable using your hand. \r\n\r\nTry changing the temperature slider value\r\n\r\nPress next";
    private string second_text = "pipe and baffles can be selected using the dropdown menus\r\n\r\nMove a bit far and point your hand at the dropdown menu when a pointer is displayed pinch your thumb finger and index finger together. This will open a dropdown and move the pointer to the value required and do the pinch action again";
    private string third_text = "Select the heatmap from the dropdown and also a velocity value in the slider\r\n\r\nIf all parameters selected press Simulate button";
    private string fourth_text = "Solver: BouyantBousinessqSimpleFoam\r\n\r\nMesh: cfMesh\r\n\r\nFlow: Laminar\r\n\r\n";
    private string[] texts; 
    private int currentIndex = 0;


    private void Start()
    {
        texts = new string[] { intro_text, second_text, third_text, fourth_text };
        textDisplay.text = intro_text;
    }
    public void ShowNextText()
    {
        if (currentIndex < texts.Length - 1) // Check if the next index is within the array
        {
            currentIndex++;
            textDisplay.text = texts[currentIndex];
        }
        // Else, do nothing
    }

    public void ShowPreviousText()
    {
        if (currentIndex > 0) // Check if the previous index is within the array
        {
            currentIndex--;
            textDisplay.text = texts[currentIndex];
        }
        // Else, do nothing
    }
}
