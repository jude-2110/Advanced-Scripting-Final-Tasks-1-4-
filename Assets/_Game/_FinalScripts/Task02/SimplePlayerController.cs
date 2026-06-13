using UnityEngine;

// This class provides a basic first-person character controller script that handles keyboard movement and mouse-look navigation
public class SimplePlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Look Settings")]
    public Transform playerCamera;
    public float mouseSensitivity = 2f;
    public float upDownLookLimit = 80f;

    private Rigidbody _rb;
    private float _verticalRotation = 0f;

    // Built-in Unity execution method that initializes configurations on the first frame of gameplay
    void Start()
    {
        // Searches for and grabs a reference to the physics engine body component attached to this game object
        _rb = GetComponent<Rigidbody>();

        // Overwrites the physics component settings to lock rotational movement along all three coordinate axes entirely
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        // Centers the operating system mouse pointer on the screen center and hides it from visual rendering inside the game frame
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Built-in Unity execution method that triggers continuously once every rendering frame refresh cycle
    void Update()
    {
        // Extracts the immediate frame update displacement value from the horizontal side-to-side mouse navigation tracker multiplied by scaling modifiers
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;

        // Extracts the immediate frame update displacement value from the vertical up-and-down mouse navigation tracker multiplied by scaling modifiers
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Applies a structural turn modification to rotate the entire character container sideways around the vertical coordinate axis
        transform.Rotate(Vector3.up * mouseX);

        // Adjusts the internal vertical orientation reference by subtracting the vertical mouse input value
        _verticalRotation -= mouseY;

        // Enforces threshold boundaries on the calculated numerical variable value to completely block it from passing beyond specific min and max limits
        _verticalRotation = Mathf.Clamp(_verticalRotation, -upDownLookLimit, upDownLookLimit);

        // Translates the bounded numeric variable value into positional orientation degrees to directly tilt the attached visual view camera up or down
        playerCamera.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);

        // Captures immediate keyboard directional input along the horizontal navigation keys without applying smoothing filters
        float inputX = Input.GetAxisRaw("Horizontal");

        // Captures immediate keyboard directional input along the vertical navigation keys without applying smoothing filters
        float inputZ = Input.GetAxisRaw("Vertical");

        // Synthesizes the relative forward direction vector and right direction vector components based on the user keyboard button clicks
        Vector3 moveDirection = (transform.forward * inputZ) + (transform.right * inputX);

        // Scales down the calculated directional values to keep the travel intensity vector mathematically proportional to a length of one
        moveDirection.Normalize();

        // Assigns calculated spatial speed properties directly to the physics engine movement state variables while keeping vertical falling calculations unedited
        _rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, _rb.linearVelocity.y, moveDirection.z * moveSpeed);
    }
}