using UnityEngine;
using UnityEngine.AI;
using Fusion;

// This class controls multiplayer network-synchronized non-player character intelligence through an automated behavioral state machine
public class NPCMovement : NetworkBehaviour
{
    // Declares an enumerated set representing the specific behavioral operational modes available to this non-player character controller
    public enum AIState { WaitingForPlayers, Patrolling, Chasing }

    [Header("AI State Machine")]
    [SerializeField] private AIState _currentState = AIState.WaitingForPlayers;

    [Header("Patrol Settings")]
    [SerializeField] private Transform[] _Points;
    private int _positionIndex = 0;

    [Header("Chase Settings")]
    [SerializeField] private float _detectionRadius = 5f;
    private Transform _chaseTarget;

    private NavMeshAgent _agent;
    private FusionConnectionManager _connectionManager;

    // Built-in Unity initialization method that caches hardware components attached to the local object transform hierarchy
    private void Awake()
    {
        // Finds and stores the navigation mesh movement driver component associated with this game object
        _agent = GetComponent<NavMeshAgent>();
    }

    // Overrides the network framework instantiation method to handle setup operations when the asset spawns across the digital session
    public override void Spawned()
    {
        // Locates and registers the central active multiplayer session coordination manager active in the current scene hierarchy
        _connectionManager = FindFirstObjectByType<FusionConnectionManager>();

        // Populates an array structure holding all active game object references currently flagged with the specific layout tag string
        GameObject[] nodeObjects = GameObject.FindGameObjectsWithTag("PatrolNode");

        // Evaluates if the object search found a valid collection containing one or more matching entries in memory
        if (nodeObjects != null && nodeObjects.Length > 0)
        {
            // Reserves an empty array container scaled to match the exact size allocation of discovered waypoint nodes
            _Points = new Transform[nodeObjects.Length];

            // Runs a sequential loop to extract the coordinate conversion component from each object and cache it in the waypoint index positions
            for (int i = 0; i < nodeObjects.Length; i++)
            {
                _Points[i] = nodeObjects[i].transform;
            }
        }

        // Evaluates if the navigation mesh movement driver is active and ready to accept operational instructions
        if (_agent != null)
        {
            // Samples the physical environment topology coordinates to discover the nearest valid location point layer on the navigation mesh map
            if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
            {
                // Force-snaps the navigation agent positions directly onto the discovered navigation mesh coordinate surfaces safely
                _agent.Warp(hit.position);
            }
            // Disables active pathfinding navigation travel forces until the initial match initialization thresholds are passed
            _agent.isStopped = true;
        }

        // Evaluates if the waypoint reference collection holds valid indices before executing path calculations
        if (_Points != null && _Points.Length > 0)
        {
            // Transmits target positional coordinate configurations to the pathfinding system using the current tracking index pointer
            _agent.SetDestination(_Points[_positionIndex].position);
        }
    }

    // Overrides the continuous network execution update method to handle state machine operations synchronized across the cloud session
    public override void FixedUpdateNetwork()
    {
        // Evaluates if this specific local machine holds primary executive master control permissions over this entity, and drops out if false
        if (!Object.HasStateAuthority) return;

        // Evaluates if the connection manager is missing or if the server confirms that matchmaking countdown timers are still holding players back
        if (_connectionManager == null || !_connectionManager.CanPlayersMove)
        {
            // Forces the structural state tracking variable back to the initial match baseline operational mode
            _currentState = AIState.WaitingForPlayers;

            // Pauses character movement logic updates if the underlying pathfinding component remains active in memory
            if (_agent.enabled) _agent.isStopped = true;

            // Halts further downward processing lines inside this frame execution cycle
            return;
        }

        // Routes runtime execution flow to distinct behavioral methods depending on the active state variable assignment
        switch (_currentState)
        {
            case AIState.WaitingForPlayers:
                // Unpauses the pathfinding component movement properties since session conditions confirm game logic is unlocked
                if (_agent.enabled) _agent.isStopped = false;

                // Overwrites the active behavioral tracking variable to push the character into path navigation routes
                _currentState = AIState.Patrolling;

                // Recalculates paths toward the designated waypoint coordinate destination values
                UpdatePatrolDestination();
                break;

            case AIState.Patrolling:
                // Triggers routine distance monitoring checks along the configured node travel routes
                PerformPatrolBehavior();

                // Executes environmental search sweeps to discover if any player targets have stepped near the detection bounds
                CheckForPlayersToChase();
                break;

            case AIState.Chasing:
                // Triggers accelerated pursuit translation operations chasing down the locked player targets
                PerformChaseBehavior();
                break;
        }
    }

    // Internal processing method that handles waypoints cycling when navigating patrol tracks
    private void PerformPatrolBehavior()
    {
        // Evaluates if the target path coordinate array structure is empty, and exits the routine early if confirmed true
        if (_Points == null || _Points.Length == 0) return;

        // Checks if the calculation engine finished processing the path vectors and whether the character length distance from the point drops below thresholds
        if (!_agent.pathPending && _agent.remainingDistance < 0.2f)
        {
            // Increments the structural lookup index variable pointer forward by one to focus on the next consecutive point target
            _positionIndex++;

            // Checks if the incremented tracking value matches or passes beyond the total boundary limits of the collection
            if (_positionIndex >= _Points.Length)
            {
                // Resets the tracking pointer back to zero to loop back toward the initial starting location point node
                _positionIndex = 0;
            }
            // Transmits the updated structural coordinates into the navigation mesh path processing threads
            UpdatePatrolDestination();
        }
    }

    // Internal navigation method that re-applies path destination coordinates to the agent system components
    private void UpdatePatrolDestination()
    {
        // Confirms waypoint array variables hold data indices and that the character path component is active in memory
        if (_Points != null && _Points.Length > 0 && _agent.enabled)
        {
            // Feeds spatial coordinates into the navigation system to focus character pathing directions on the current waypoint index location
            _agent.SetDestination(_Points[_positionIndex].position);
        }
    }

    // Internal search method that loops through character entities to identify prospective tracking targets
    private void CheckForPlayersToChase()
    {
        // Discovers and populates an array with all player tracking script instances currently loaded across the world environment space
        PlayerMovement[] players = FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None);

        // Runs a search scan analyzing spatial coordinates for each character component stored inside the player target array
        foreach (PlayerMovement player in players)
        {
            // Calculates the absolute scalar length space vector between this non-player character and the active player transform object
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            // Evaluates if the calculated distance math is less than or equal to the designated sensory boundary limitation fields
            if (distanceToPlayer <= _detectionRadius)
            {
                // Anchors the target reference identity to lock pursuit coordinates on this specific character transform
                _chaseTarget = player.transform;

                // Shifts the state machine state into pursuit mode to switch active behavioral updates
                _currentState = AIState.Chasing;

                // Breaks out of the loop layer immediately since a target has been successfully verified for pursuit
                break;
            }
        }
    }

    // Internal behavioral method that overrides standard waypoints travel paths to track player transformations
    private void PerformChaseBehavior()
    {
        // Evaluates if the player target transformation reference variable was dropped or became empty
        if (_chaseTarget == null)
        {
            // Re-routes state flags back to waypoint loops since active pursuit properties are no longer valid
            _currentState = AIState.Patrolling;

            // Directs path calculations back toward the last registered route waypoint coordinates
            UpdatePatrolDestination();
            return;
        }

        // Calculates the absolute distance scale between this entity and the targeted player asset transformation position
        float distanceToTarget = Vector3.Distance(transform.position, _chaseTarget.position);

        // Checks if the target distance coordinates pass beyond an expanded boundary padding zone to determine if pursuit tracking was broken
        if (distanceToTarget > _detectionRadius * 1.5f)
        {
            // Erases the stored tracking transformation properties to clear out player targeted references
            _chaseTarget = null;

            // Re-routes operational state variables back to standard node routines
            _currentState = AIState.Patrolling;

            // Directs path calculations back toward the last registered route waypoint coordinates
            UpdatePatrolDestination();
            return;
        }

        // Updates destination paths toward the changing coordinate vectors of the player as long as navigation scripts stay enabled
        if (_agent.enabled)
        {
            // Overwrites active path vectors to point continuously toward the localized coordinate space of the locked player target
            _agent.SetDestination(_chaseTarget.position);
        }
    }

    // Built-in scene editor rendering method that draws non-compiled visual aid objects for design layout operations
    private void OnDrawGizmosSelected()
    {
        // Updates scene view rendering brush parameters to a solid red tone channel option
        Gizmos.color = Color.red;

        // Draws an editor-only outline wire representation of a sphere to visualize sensory detection boundaries around the character position
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }
}