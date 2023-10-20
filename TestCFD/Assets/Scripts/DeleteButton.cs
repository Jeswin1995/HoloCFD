using UnityEngine;

public class DeleteButton : MonoBehaviour
{
    public GameObject modelToDelete;

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
}
