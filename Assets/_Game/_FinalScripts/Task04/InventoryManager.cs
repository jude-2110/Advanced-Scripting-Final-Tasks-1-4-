using UnityEngine;
using System;

// This class implements a persistent Singleton pattern to manage the global player inventory state, currency balance, and skill upgrades across game scenes
public class InventoryManager : MonoBehaviour
{
    // Public reading gateway that provides global static access to this distinct class instance from any script in the project
    public static InventoryManager Instance { get; private set; }

    [Header("Player Wallet Balance")]
    public int totalGold = 0;

    [Header("Character Skill Unlocks")]
    public bool hasDoubleJumpUnlocks = false;
    public bool hasSpeedBoostUnlocks = false;

    // Static event broadcast channel that notifies listening systems automatically whenever an inventory property changes value
    public static event Action OnInventoryUpdated;

    // Built-in Unity initialization method used to configure and enforce the strict Singleton architectural design pattern
    private void Awake()
    {
        // Evaluates if an instance reference already exists in memory and ensures it is not pointing to this specific script execution link
        if (Instance != null && Instance != this)
        {
            // Instantly terminates this duplicate game object component to prevent memory leaks and maintain data integrity across scenes
            Destroy(gameObject);
            return;
        }

        // Binds this specific active component instance as the primary master authority instance for global tracking
        Instance = this;

        // Instructs the engine infrastructure to protect this game object from being wiped out when transitioning between different scene loads
        DontDestroyOnLoad(gameObject);
    }

    // Public modification method that increases the numerical gold currency balance stored inside the user profile data registers
    public void AddGold(int amount)
    {
        // Adds the incoming numerical parameter value directly to the total accumulated wallet balance variable
        totalGold += amount;

        // Outputs a status tracking message string to the console window detailing the transaction amount and new balance
        Debug.Log($"Inventory Manager: Successfully credited {amount} gold. Current balance: {totalGold}");

        // Safely triggers the static update event to inform UI layers or achievements systems to refresh their contents
        OnInventoryUpdated?.Invoke();
    }

    // Public modification method that processes state flag switches to activate specific gameplay character capability mechanics
    public void UnlockItem(string itemID)
    {
        // Checks if the lookup key string matches the double jump condition and overwrites its structural state flag to true if confirmed
        if (itemID == "double_jump") hasDoubleJumpUnlocks = true;

        // Checks if the lookup key string matches the speed boost condition and overwrites its structural state flag to true if confirmed
        if (itemID == "speed_boost") hasSpeedBoostUnlocks = true;

        // Outputs a verification tracking string to the editor log view confirming successful capability profile updates
        Debug.Log($"Inventory Manager: Item unlocked successfully: {itemID}");

        // Safely triggers the static update event to inform UI layers or player controllers to refresh their capability rules
        OnInventoryUpdated?.Invoke();
    }
}