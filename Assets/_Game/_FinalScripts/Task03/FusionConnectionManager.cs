using Fusion;
using Fusion.Photon.Realtime;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// This class manages network matchmaking, player initialization, and synchronized AI creation by implementing multiplayer pipeline interface listeners
public class FusionConnectionManager : MonoBehaviour, INetworkRunnerCallbacks
{
    private NetworkRunner _runner;
    private bool _matchmakingComplete = false;

    [Header("UI Panels")]
    public GameObject menuPanel;
    public GameObject lobbyPanel;

    [Header("UI Text")]
    public TextMeshProUGUI lobbyStatusText;

    [Header("Player Prefab")]
    public NetworkObject playerPrefab;

    [Header("AI Spawning Settings")]
    public NetworkObject aiPrefab;
    public Transform aiSpawnPoint1;
    public Transform aiSpawnPoint2;

    // Public reading property that exposes a conditional state check determining if input controls are unlocked for characters
    public bool CanPlayersMove => _matchmakingComplete;

    // Built-in initialization method that resets matchmaking milestones and configures default UI panel visibilities on game startup
    private void Start()
    {
        // Initializes the session tracking variable to a default state indicating the room requirements are not yet complete
        _matchmakingComplete = false;

        // Evaluates if the main canvas menu interface assignment exists, and sets its visibility status to true if confirmed
        if (menuPanel != null) menuPanel.SetActive(true);

        // Evaluates if the network room interface assignment exists, and sets its visibility status to false if confirmed
        if (lobbyPanel != null) lobbyPanel.SetActive(false);
    }

    // Public execution method linked to menu buttons that spins up the custom online engine instance and room connection sequence
    public void StartSingleSceneMultiplayer()
    {
        // Evaluates if the main selection screen layer exists, and disables its visual rendering if true
        if (menuPanel != null) menuPanel.SetActive(false);

        // Evaluates if the waiting room screen layer exists, and enables its visual rendering if true
        if (lobbyPanel != null) lobbyPanel.SetActive(true);

        // Evaluates if the text reporting field exists, and updates its display characters to reflect the connection state
        if (lobbyStatusText != null) lobbyStatusText.text = "Connecting to Photon Cloud...";

        // Instantiates a blank game object container into the active hierarchy to serve as a hardware anchor for networking
        GameObject runnerObject = new GameObject("NetworkRunner");

        // Dynamically appends the master network driver component to the newly created game object and assigns it to a field reference
        _runner = runnerObject.AddComponent<NetworkRunner>();

        // Sets a boolean property toggle on the network driver to confirm that local hardware inputs will be captured for processing
        _runner.ProvideInput = true;

        // Subscribes this current script instance to the network driver to listen for incoming event notification triggers
        _runner.AddCallbacks(this);

        // Queries global database configuration parameters to pull down active server connection settings allocations
        var photonAppSettings = PhotonAppSettings.Global;

        // Triggers the master connection process on the network driver using an argument block containing setup specifications
        _runner.StartGame(new StartGameArgs()
        {
            // Assigns the network architecture topology style to a decentralized model where every player retains ownership over their assets
            GameMode = GameMode.Shared,

            // Defines the dedicated text registration string name for the virtual match room container
            SessionName = "JudeSingleSceneRoom",

            // Uses a ternary conditional evaluation to safely pass active server app details or return null if empty
            CustomPhotonAppSettings = photonAppSettings != null ? photonAppSettings.AppSettings : null
        });
    }

    // Network callback method triggered automatically whenever any connection node successfully joins the active virtual server room
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        // Evaluates if the newly registered participant reference matches the specific identification key assigned to this local machine
        if (player == runner.LocalPlayer)
        {
            // Sends a status tracking message to the editor log view outlining asset creation parameters
            Debug.Log("Local player authenticated. Instant spawning capsule asset at (0,0,0)");

            // Assigns an initial spatial translation position vector pointing directly to the world origin coordinates
            Vector3 spawnLocation = Vector3.zero;

            // Commands the network execution layer to allocate, transmit, and instantiate a synchronized network game object for the user
            runner.Spawn(playerPrefab, spawnLocation, Quaternion.identity, player);
        }

        // Extracts the total tally configuration integer representing all active machines currently registered inside the match room
        int currentPlayers = runner.SessionInfo.PlayerCount;

        // Evaluates if the room status layout text component is assigned in the inspector slots
        if (lobbyStatusText != null)
        {
            // Updates the visual layout string using interpolation formatting to reveal current matchmaking capacity tracking numbers
            lobbyStatusText.text = $"Connecting to Lobby...\nLive Player Count: ({currentPlayers} / 4 Connected)";
        }

        // Evaluates if the active player accumulation integer satisfies or passes beyond the established threshold boundary
        if (currentPlayers >= 4)
        {
            // Sends a coordination notice message to the log view confirming that the threshold limit was met
            Debug.Log("Lobby full! Deactivating UI panels.");

            // Evaluates if the waiting room text panel is active, and turns off its visual state if confirmed
            if (lobbyPanel != null) lobbyPanel.SetActive(false);

            // Overwrites the tracking milestone variable to true to formally declare that the game room conditions are completed
            _matchmakingComplete = true;

            // Evaluates if this specific hardware machine holds administrative control permissions over the shared scene instance
            if (runner.IsSceneAuthority)
            {
                // Accesses internal spawning mechanisms to generate non-playable characters managed by the host machine
                SpawnNetworkedAI(runner);
            }
        }
    }

    // Internal execution method that spawns global network synchronized computer-controlled characters across the map
    private void SpawnNetworkedAI(NetworkRunner runner)
    {
        // Evaluates if the assigned network entity reference blueprint is entirely missing from its inspector tracking slot
        if (aiPrefab == null)
        {
            // Outputs a critical asset notification warning string to the logging console view and breaks out of the execution line
            Debug.LogWarning("FusionConnectionManager: AI Prefab is missing from inspector slots!");
            return;
        }

        // Outputs a status tracking message to the engine console confirming host authorization tasks are active
        Debug.Log("Scene Authority tracking active. Spawning 2 synchronized AI characters...");

        // Uses ternary conditional blocks to set coordinate vectors from design anchors or fallback to hardcoded positioning values
        Vector3 pos1 = aiSpawnPoint1 != null ? aiSpawnPoint1.position : new Vector3(-3f, 0f, 3f);
        Vector3 pos2 = aiSpawnPoint2 != null ? aiSpawnPoint2.position : new Vector3(3f, 0f, -3f);

        // Instructs the network manager to instantiate the first synchronized computer asset assigning structural control to no player
        runner.Spawn(aiPrefab, pos1, Quaternion.identity, PlayerRef.None);

        // Instructs the network manager to instantiate the second synchronized computer asset assigning structural control to no player
        runner.Spawn(aiPrefab, pos2, Quaternion.identity, PlayerRef.None);
    }

    // Network callback method triggered automatically whenever any connection node disconnects or drops out from the active room
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        // Evaluates if room data tracking states remain valid and whether the game has not yet officially launched its match session
        if (runner.SessionInfo != null && !_matchmakingComplete)
        {
            // Extracts the updated total count integer representing the remaining active machine nodes inside the room
            int currentPlayers = runner.SessionInfo.PlayerCount;

            // Evaluates if the room text field assignment is verified in memory
            if (lobbyStatusText != null)
            {
                // Overwrites the interface text displaying the fresh structural connection tracking numbers after a departure
                lobbyStatusText.text = $"Live Player Count: ({currentPlayers} / 4 Connected)";
            }
        }
    }

    // Explicit interface method configuration that handles binary chunk metadata download rate updates
    void INetworkRunnerCallbacks.OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }

    // Explicit interface method configuration that handles processing sequences when a network room link-up operation encounters errors
    void INetworkRunnerCallbacks.OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }

    // Explicit interface method configuration that handles reading incoming byte array streams sent across targeted connection lanes
    void INetworkRunnerCallbacks.OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }

    // Explicit interface method configuration that handles clearing tracking registers when a networked object leaves an interest area zone
    void INetworkRunnerCallbacks.OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }

    // Explicit interface method configuration that handles initializing tracking registers when a networked object enters an interest area zone
    void INetworkRunnerCallbacks.OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }

    // Explicit interface method configuration that handles polling and packing local input data parameters to send over the network frame loop
    void INetworkRunnerCallbacks.OnInput(NetworkRunner runner, NetworkInput input) { }

    // Explicit interface method configuration that handles alternative prediction corrections if a timeline execution frame drops input metadata
    void INetworkRunnerCallbacks.OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }

    // Explicit interface method configuration that handles clearing operational memory structures when the network process is disconnected
    void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }

    // Explicit interface method configuration that handles logic steps immediately when the network engine validates an internal link
    void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner) { }

    // Explicit interface method configuration that handles clean-up steps if the communication link drops off from the destination host
    void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }

    // Explicit interface method configuration that handles processing admission authorization credentials before welcoming a room connection request
    void INetworkRunnerCallbacks.OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }

    // Explicit interface method configuration that handles translating custom binary data instructions across active client simulation states
    void INetworkRunnerCallbacks.OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }

    // Explicit interface method configuration that handles updating local directory data menus when lobby rooms change parameters on the cloud
    void INetworkRunnerCallbacks.OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }

    // Explicit interface method configuration that handles execution tasks upon receiving data packets from dedicated security systems
    void INetworkRunnerCallbacks.OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

    // Explicit interface method configuration that handles bridging game management permissions if a session manager host leaves the room
    void INetworkRunnerCallbacks.OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

    // Explicit interface method configuration that handles executing operational script initialization setup tasks after a network scene load finishes
    void INetworkRunnerCallbacks.OnSceneLoadDone(NetworkRunner runner) { }

    // Explicit interface method configuration that handles executing cleaning cycles right before the network engine starts loading a new scene
    void INetworkRunnerCallbacks.OnSceneLoadStart(NetworkRunner runner) { }
}