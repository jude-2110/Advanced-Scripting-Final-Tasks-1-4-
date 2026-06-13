using Fusion;
using UnityEngine;

// This class handles network-synchronized player movement inside a shared multiplayer session by interacting with a character controller component
public class PlayerMovement : NetworkBehaviour
{
    private CharacterController _controller;
    private FusionConnectionManager _connectionManager;

    public float PlayerSpeed = 2f;

    // Built-in Unity initialization method that caches hardware components attached to the local object transform hierarchy
    private void Awake()
    {
        // Finds and stores the character controller movement component attached to this specific game object
        _controller = GetComponent<CharacterController>();
    }

    // Overrides the network framework instantiation method to handle setup operations when the asset spawns across the digital session
    public override void Spawned()
    {
        // Locates and registers the central active multiplayer session coordination manager active in the current scene hierarchy
        _connectionManager = FindFirstObjectByType<FusionConnectionManager>();

        // Evaluates if the component search returned empty, indicating the session manager is completely missing from the scene
        if (_connectionManager == null)
        {
            // Outputs an asset notification warning string to the logging console view to warn about a missing coordination script
            Debug.LogWarning("PlayerMovement: FusionConnectionManager could not be found in the scene!");
        }
    }

    // Overrides the continuous network execution update method to handle positional movement calculations synchronized across the cloud session
    public override void FixedUpdateNetwork()
    {
        // Evaluates if the connection manager reference is missing or if the server confirms that matchmaking loops are still holding players back
        if (_connectionManager == null || !_connectionManager.CanPlayersMove)
        {
            // Halts further downward processing lines inside this network frame update loop
            return;
        }

        // Synthesizes horizontal and vertical keyboard input axes into a spatial translation direction vector, scaled by network frame timing splits and speed variables
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Runner.DeltaTime * PlayerSpeed;

        // Feeds the calculated displacement direction vector straight into the character controller component to execute physical position changes
        _controller.Move(move);

        // Evaluates if the calculated movement translation vector contains directional magnitude changes away from a static resting position
        if (move != Vector3.zero)
        {
            // Directs the forward directional facing vector of the game object to point instantly toward the active translation vector path
            gameObject.transform.forward = move;
        }
    }
}