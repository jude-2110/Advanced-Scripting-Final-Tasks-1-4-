using UnityEngine;

// This class extends the abstract base class to implement specific object behavior for an interactive key asset in the scene
public class KeyInteractable2 : InteractableBase
{
    // Overrides the inherited abstract method to define the distinct sequence that occurs when the player triggers this object
    public override void Interact()
    {
        // Searches the active scene hierarchy to locate and reference the operational player interaction management component
        PlayerInteraction2 player = FindFirstObjectByType<PlayerInteraction2>();

        // Evaluates if the scene search successfully found a valid, active instance of the player management script
        if (player != null)
        {
            // Changes the status flag on the referenced player component to true to confirm the object is collected
            player.hasKey = true;

            // Dispatches a confirmation message string to the editor log view confirming inventory modification
            Debug.Log("interacting with key - Added to Inventory!");

            // Permanently removes this entire key game object from the active scene memory heap
            Destroy(gameObject);
        }
    }
}