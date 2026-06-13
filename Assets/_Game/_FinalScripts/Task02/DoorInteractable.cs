using UnityEngine;

// This class extends the abstract base class to implement specific object behavior for an interactive door obstacle in the scene
public class DoorInteractable : InteractableBase
{
    // Overrides the inherited abstract method to define the distinct sequence that occurs when the player triggers this object
    public override void Interact()
    {
        // Searches the active scene hierarchy to locate and reference the operational player interaction management component
        PlayerInteraction2 player = FindFirstObjectByType<PlayerInteraction2>();

        // Evaluates if the scene search successfully found a valid, active instance of the player management script
        if (player != null)
        {
            // Checks the boolean inventory tracking flag on the found player component to see if they possess the key item
            if (player.hasKey)
            {
                // Dispatches a confirmation message string to the editor log view indicating successful access verification
                Debug.Log("interacting with door, Key verified! The door disappears.");

                // Permanently removes this entire door game object from the active scene memory heap
                Destroy(gameObject);
            }
            // Executes this alternative operational block if the player tracking component evaluates its inventory flag as false
            else
            {
                // Dispatches an alert warning message string to the editor log view explaining that entry access remains blocked
                Debug.LogWarning("interacting with door, The door is locked. Go find the key first!");
            }
        }
    }
}