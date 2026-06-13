using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

// This class acts as the central controller to manage the sequence of authentication, API fetching, and database saving
public class AuthAndDataFlowManager : MonoBehaviour
{
    private IAuthSystem _authSystem;
    private IGameDataSystem _dataSystem;
    private MockApiHandler _apiHandler;

    // Public method to manually inject the required dependencies to keep the system modular and decoupled
    public void InjectDependencies(IAuthSystem authSystem, IGameDataSystem dataSystem, MockApiHandler apiHandler)
    {
        _authSystem = authSystem;
        _dataSystem = dataSystem;
        _apiHandler = apiHandler;
    }

    // Public entry point to initiate the user registration process
    public void ExecuteSignUpFlow(string email, string password)
    {
        // Log to the console that the registration process has officially started
        Debug.Log("Executing cloud sign-up phase...");

        // Automatically use the provided email string as the user's initial display name
        string dummyDisplayName = email;

        // Call the signup method on the authentication system and pass a callback to handle the result asynchronously
        _authSystem.SignUp(email, password, dummyDisplayName, (uid) => {
            // Check if the returned user ID string from the server is valid and not empty
            if (!string.IsNullOrEmpty(uid))
            {
                // Log that registration succeeded and print the unique user ID to the console
                Debug.Log($"Sign up successful! UID: {uid}. Moving to API fetch phase...");

                // Start the sequential coroutine to download web data and create the database records
                StartCoroutine(FetchAndSaveSequence(uid, email));
            }
            // Execute this block if the server failed to return a valid user ID string
            else
            {
                // Print an error message to the console indicating that registration failed
                Debug.LogError("Sign up failed. Received empty UID.");
            }
        });
    }

    // Public entry point to initiate the user login verification process
    public void ExecuteLoginFlow(string email, string password)
    {
        // Log to the console that the login verification sequence has started
        Debug.Log("Executing cloud login phase...");

        // Trigger the internal login operations within the connected authentication system
        _authSystem.Login(email, password);

        // Immediately request the current user ID string from the authentication system
        string uid = _authSystem.GetUserID();

        // Check if a valid, authenticated user ID string is currently available
        if (!string.IsNullOrEmpty(uid))
        {
            // Log that a valid login session was detected and display the user ID in the console
            Debug.Log($"Login recognized! UID: {uid}. Moving to API fetch phase...");

            // Start the sequential coroutine to handle web data downloading and database saving
            StartCoroutine(FetchAndSaveSequence(uid, email));
        }
        // Execute this block if no valid user ID could be immediately retrieved
        else
        {
            // Log a warning indicating that the async network authentication process might still be running in the background
            Debug.LogError("Login initiated. (Note: If this is the first click, Firebase might still be authenticating asynchronously).");
        }
    }

    // Coroutine that handles the exact execution order of downloading API data, saving to the database, and changing scenes
    private IEnumerator FetchAndSaveSequence(string uid, string email)
    {
        // Initialize an empty string to hold the raw text response from the API web request
        string fetchedData = "";

        // Initialize a boolean flag to track whether the web request completed successfully
        bool apiSuccess = false;

        // Start the API fetch coroutine and pause execution here until the network request completely finishes
        yield return StartCoroutine(_apiHandler.FetchDataFromAPI((success, data) => {
            // Save the network success status into the local boolean flag
            apiSuccess = success;

            // Save the downloaded string response text into the local data string
            fetchedData = data;
        }));

        // Check if the API web request failed to communicate with the server
        if (!apiSuccess)
        {
            // Log a critical error message stating that the flow is being stopped due to network failure
            Debug.LogError("Aborting flow: Failed to communicate with Mock API.");

            // Terminate this coroutine entirely to prevent corrupt data from saving
            yield break;
        }

        // Pass the user ID, email, and downloaded API data to the database system to create a permanent user profile record
        _dataSystem.CreateUserRecord(uid, email, fetchedData);

        // Pause execution for a single frame to ensure the database write operations have time to register
        yield return null;

        // Log that all data pipelines have successfully completed and the game is ready to transition
        Debug.Log("Pipeline complete! Loading Gameplay Scene.");

        // Command the scene manager to instantly load the main gameplay level scene by its file name
        SceneManager.LoadScene("KeyDoorScene");
    }
}