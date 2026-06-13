using TMPro;
using UnityEngine;
using UnityEngine.UI;

// This class controls the user interface events for the authentication screen by capturing inputs and sending them to the flow manager
public class LoginUIController : MonoBehaviour
{
    [Header("UI Inputs")]
    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private TMP_InputField passwordInputField;

    [Header("UI Buttons")]
    [SerializeField] private Button signUpButton;
    [SerializeField] private Button loginButton;

    private AuthAndDataFlowManager _flowManager;

    // Public initialization method to connect the interface elements to the master sequence flow manager
    public void Setup(AuthAndDataFlowManager flowManager)
    {
        // Stores the passed sequential manager component instance inside an internal backing reference
        _flowManager = flowManager;

        // Attaches a functional event listener to the registration button to run automatically when pressed
        signUpButton.onClick.AddListener(OnSignUpClicked);

        // Attaches a functional event listener to the connection entry button to run automatically when pressed
        loginButton.onClick.AddListener(OnLoginClicked);
    }

    // Event listener method triggered automatically when the registration button is pressed by a user
    private void OnSignUpClicked()
    {
        // Extracts the textual entry from the email text input area while eliminating trailing or leading spaces
        string email = emailInputField.text.Trim();

        // Extracts the textual entry string from the password input area
        string password = passwordInputField.text;

        // Evaluates if the collected credential format conditions are satisfied before continuing
        if (ValidateInputs(email, password))
        {
            // Forwards the verified text data parameters to start the profile creation process inside the flow manager
            _flowManager.ExecuteSignUpFlow(email, password);
        }
    }

    // Event listener method triggered automatically when the validation log button is pressed by a user
    private void OnLoginClicked()
    {
        // Extracts the textual entry from the email text input area while eliminating trailing or leading spaces
        string email = emailInputField.text.Trim();

        // Extracts the textual entry string from the password input area
        string password = passwordInputField.text;

        // Evaluates if the collected credential format conditions are satisfied before continuing
        if (ValidateInputs(email, password))
        {
            // Forwards the verified text data parameters to start the account checking process inside the flow manager
            _flowManager.ExecuteLoginFlow(email, password);
        }
    }

    // Internal safety validation method that evaluates input text criteria requirements before processing web operations
    private bool ValidateInputs(string email, string password)
    {
        // Checks if either the extracted account address text or verification key text fields are completely missing text
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            // Generates a status alert message inside the editor logger warning about blank registration parameters
            Debug.LogWarning("Please don't leave either of the fields empty.");

            // Rejects continuing processing since one or more mandatory items were submitted blank
            return false;
        }
        // Checks if the recorded security verification text contains fewer than the required total symbol count
        if (password.Length < 6)
        {
            // Generates a status alert message inside the editor logger warning about insufficient length parameters
            Debug.LogWarning("Password needs to be at least 6 characters long.");

            // Rejects continuing processing since the security credentials do not meet complexity structural baselines
            return false;
        }
        // Confirms that all structured text requirements pass evaluation successfully
        return true;
    }
}