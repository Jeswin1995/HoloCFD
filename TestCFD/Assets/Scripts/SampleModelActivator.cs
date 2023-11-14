using System.Collections.Generic;
using UnityEngine;
public class SampleModelActivator : MonoBehaviour
{
    private List<GameObject> childGameObjects;
    public TMPro.TMP_Dropdown pipeDropdown; // Assign in the inspector
    public TMPro.TMP_Dropdown baffleDropdown; // Assign in the inspector

    void Start()
    {
        // Initialize the list
        childGameObjects = new List<GameObject>();

        // Loop through each child of the GameObject this script is attached to
        foreach (Transform child in transform)
        {
            // Add the child GameObject to the list
            childGameObjects.Add(child.gameObject);
            // Make the child GameObject inactive
            child.gameObject.SetActive(false);
        }

        // Add listener for when the value of the Dropdown changes
        pipeDropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(); });
        baffleDropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(); });
    }

    void DropdownValueChanged()
    {
        // Deactivate all child GameObjects
        childGameObjects.ForEach(go => go.SetActive(false));

        // Create a name based on the selected dropdown values
        string requiredName = "HE_" + pipeDropdown.options[pipeDropdown.value].text + baffleDropdown.options[baffleDropdown.value].text;

        // Try to find the GameObject with the matching name
        GameObject foundGameObject = childGameObjects.Find(go => go.name == requiredName);

        if (foundGameObject != null)
        {
            // If found, activate the corresponding GameObject
            foundGameObject.SetActive(true);
        }
        else
        {
            // If not found, log a debug message
            Debug.LogWarning("No GameObject found with the name: " + requiredName);
        }
    }
}
