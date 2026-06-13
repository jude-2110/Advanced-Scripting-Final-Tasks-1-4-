using UnityEngine;

// This class monitors and processes player inputs when the character stands inside interaction trigger zones
public class PlayerInteraction2 : MonoBehaviour
{
    [Header("Inventory State")]
    public bool hasKey = false;

    // Built-in Unity physics engine event method that runs repeatedly every frame another object collider rests inside this trigger area
    private void OnTriggerStay(Collider other)
    {
        // Evaluates if the object inside the zone possesses the correct tag string and if the user pressed the interaction key on their keyboard simultaneously
        if (other.gameObject.CompareTag("Interactable") && Input.GetKeyDown(KeyCode.E))
        {
            // Queries the specific game object inside the zone to look up and reference its parent interactable base script component
            InteractableBase interactable = other.gameObject.GetComponent<InteractableBase>();

            // Evaluates if the component search successfully found a valid script attached to the target object
            if (interactable != null)
            {
                // Dispatches an execution signal to trigger the polymorphism action sequence configured on that specific object class
                interactable.Interact();
            }
        }

    }
}