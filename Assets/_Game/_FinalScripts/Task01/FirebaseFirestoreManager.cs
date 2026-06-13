using Firebase.Extensions;
using Firebase.Firestore;
using System.Collections.Generic;
using UnityEngine;

// This class implements the IGameDataSystem interface to manage database storage operations inside cloud collections
public class FirebaseFirestoreManager : IGameDataSystem
{
    FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

    // Public method to create a new profile data document structure using identity strings and attributes
    public void CreateUserRecord(string userID, string email, string name)
    {
        // Define a reference tracking path pointing to a specific document named after the user ID inside a collection group
        DocumentReference docRef = db.Collection("users").Document(userID);

        // Construct a structured data map consisting of text keys matched with object values to define the profile attributes
        Dictionary<string, object> user = new Dictionary<string, object>
        {
            { "email", email },
            { "name", name },
            { "score", 0 }
        };

        // Commit the mapped data attributes to the remote database document reference asynchronously
        docRef.SetAsync(user);
    }

    // Public method to locate a specific user document and overwrite its numeric attribute value
    public void UpdateScore(string userID, int newScore)
    {
        // Evaluate if the provided identification tracking text is empty, and exit early to prevent database errors if true
        if (string.IsNullOrEmpty(userID)) return;

        // Establish an operational path directly to the target user document located inside the data repository
        DocumentReference docRef = db.Collection("users").Document(userID);

        // Send a request to alter only the specified attribute entry asynchronously, then pass execution back to the primary thread loop
        docRef.UpdateAsync("score", newScore).ContinueWithOnMainThread(task => {

            // Output a confirmation message containing the updated numerical value directly into the editor logs
            Debug.Log("Score updated to: " + newScore);
        });
    }
}