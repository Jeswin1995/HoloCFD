using TMPro;
using UnityEngine;

public class DeleteButton : MonoBehaviour
{
    public GameObject modelToDelete;
    public TextMeshProUGUI fileNameText;
    public TextMeshProUGUI lowbound;
    public TextMeshProUGUI highbound;
    public void OnDeleteButtonClick()
    {
        if (modelToDelete != null)
        {
            // Delete the model
            Destroy(modelToDelete);
        }
        // Optionally, you can destroy the delete button itself if needed
        Destroy(gameObject);
    }

    public void SetFileNameText(string fileName)
    {
        if (fileNameText != null)
        {
            // Remove the file extension
            string baseName = fileName.Replace(".glb", "");

            // Split the string by the '+' character
            string[] parts = baseName.Split('+');

            string temperature = parts.Length > 0 ? $"{parts[0]} °C" : "";
            string velocity = parts.Length > 1 ? $"{parts[1]}m/s" : "";
            string pipes = parts.Length > 2 ? $"{parts[2].Substring(2, 1)} pipes" : "";
            string baffles = parts.Length > 2 ? $"{parts[2].Substring(3, 1)} baffles" : "";
            string heatmap = parts.Length > 3 && parts[3] == "T" ? "Temperature HeatMap" : (parts.Length > 3 && parts[3] == "U" ? "Velocity HeatMap" : "");
            if (parts[3] == "T")
            {
                lowbound.text = "300°C";
                highbound.text = "320°C";
            }
            else if (parts[3] == "U")
            {
                lowbound.text = "0m/s";
                highbound.text = "0.1m/s";
            }
            // Combine all parts into a single formatted string
            fileNameText.text = $"{temperature}\n{velocity}\n{pipes}\n{baffles}\n{heatmap}";
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component not found on fileName_text");
        }
    }
}
