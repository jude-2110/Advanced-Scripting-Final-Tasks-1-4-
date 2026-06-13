using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// This class handles loading and instantiating an asset dynamically at runtime using the addressables system asset pipeline
public class LoadPlayerAddressable : MonoBehaviour
{
    public string assetAddress = "KeyPrefab";
    AsyncOperationHandle<GameObject> handle;

    // Built-in Unity execution method that starts automatically when the object enters the active scene
    private void Start()
    {
        // Begins an asynchronous data loading process to find and load the asset file mapped to the designated string address
        handle = Addressables.LoadAssetAsync<GameObject>(assetAddress);

        // Subscribes a listener method to the asset loading operation to execute automatically as soon as the process completely finishes
        handle.Completed += OnLoadCompleted;
    }

    // Callback method triggered automatically when the underlying asynchronous file loading sequence updates its final status
    void OnLoadCompleted(AsyncOperationHandle<GameObject> op)
    {
        // Evaluates if the finished operation successfully located, verified, and loaded the asset without encountering errors
        if (op.Status == AsyncOperationStatus.Succeeded)
        {
            // Creates a new orientation structure mathematically rotated ninety degrees around the horizontal directional coordinate axis
            Quaternion spawnRotation = Quaternion.Euler(90f, 0f, 0f);

            // Spawns a physical instance of the successfully loaded asset into the active scene environment at the exact specified coordinate location and orientation
            Instantiate(op.Result, new Vector3(-9.86f, 6.48f, 4.93f), spawnRotation);
        }
    }
}