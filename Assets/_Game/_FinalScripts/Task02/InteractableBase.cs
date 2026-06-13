using JetBrains.Annotations;
using UnityEngine;

// This abstract base class serves as the parent blueprint component that all interactable object variations will inherit from
public class InteractableBase : MonoBehaviour
{

    // Defines a public placeholder method that establishes a unified action name while allowing children to implement their own custom gameplay behaviors
    public virtual void Interact() //can be overridden in the child class
    {
        // Outputs a fallback tracking text string to the console window if a child class forgets to override this method
        Debug.Log("Interacting with parent");
    }

}