using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

// This class handles downloading text data from an external testing server by sending an asynchronous web request
public class MockApiHandler : MonoBehaviour
{
    private const string TargetUrl = "https://jsonplaceholder.typicode.com/todos/1";

    // Public coroutine method that initiates a network connection sequence and accepts a callback method to return the final execution results
    public IEnumerator FetchDataFromAPI(Action<bool, string> onResult)
    {
        // Creates and configures an isolated network pipeline setup designed to send a standard download query to the specified address
        using (UnityWebRequest webRequest = UnityWebRequest.Get(TargetUrl))
        {
            // Pauses processing along this background execution line until the transmission completely finishes transferring across the network
            yield return webRequest.SendWebRequest();

            // Evaluates if the finished web transaction ran into hardware connectivity dropouts or invalid server response codes
            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                // Formats and logs a critical error report text string summarizing the specific failure into the editor console window
                Debug.LogError($"API Error: {webRequest.error}");

                // Triggers the provided callback method immediately while supplying parameters indicating a failed attempt and empty data
                onResult?.Invoke(false, null);
            }
            // Executes this block if the network system confirms the transmission reached its destination and returned status signals successfully
            else
            {
                // Extracts the completely unedited text data payload received back from the remote endpoint into a local memory string
                string jsonResponse = webRequest.downloadHandler.text;

                // Prints a tracking message containing the raw text payload straight to the engine console view for verification
                Debug.Log($"API Data Fetched Successfully: {jsonResponse}");

                // Triggers the provided callback method immediately while supplying parameters indicating a successful transfer along with the downloaded text data
                onResult?.Invoke(true, jsonResponse);
            }
        }
    }
}