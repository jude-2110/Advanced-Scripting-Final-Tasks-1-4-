using UnityEngine;

// This class acts as the initial startup script that runs before anything else to initialize, link, and inject dependencies across systems
public class GameBootstrapper : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField] private LoginUIController uiController;
    [SerializeField] private AuthAndDataFlowManager flowManager;

    // Built-in Unity initialization method that runs instantly when the game object is loaded into memory
    void Awake()
    {
        // Instantiate the concrete backend authentication module framework
        IAuthSystem authBackend = new FirebaseAuthManager();

        // Instantiate the concrete backend database data tracking system module
        IGameDataSystem dataBackend = new FirebaseFirestoreManager();

        // Search this specific game object to locate an attached network communication component instance
        MockApiHandler apiBackend = GetComponent<MockApiHandler>();

        // Evaluate if the search returned empty, indicating the component is completely missing from this object
        if (apiBackend == null)
        {
            // Dynamically attach a brand new instance of the network communication script to this game object at runtime
            apiBackend = gameObject.AddComponent<MockApiHandler>();
        }

        // Pass the initialized authentication, database, and network modules into the data stream manager script
        flowManager.InjectDependencies(authBackend, dataBackend, apiBackend);

        // Link the user interface controller directly to the initialized data stream manager so it can relay user inputs
        uiController.Setup(flowManager);
    }
}