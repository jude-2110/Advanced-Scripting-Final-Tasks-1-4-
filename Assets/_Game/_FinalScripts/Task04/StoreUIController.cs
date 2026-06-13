using UnityEngine;
using TMPro;

// This class updates the store's visual user interface layout text fields by listening to inventory event data changes from the InventoryManager singleton
public class StoreUIController : MonoBehaviour
{
    [Header("Responsive UI Text Targets")]
    public TextMeshProUGUI goldBalanceText;
    public TextMeshProUGUI doubleJumpButtonText;
    public TextMeshProUGUI speedBoostButtonText;

    // Built-in Unity execution method triggered automatically whenever this specific game object components are toggled to an active state
    private void OnEnable()
    {
        // Subscribes the interface refreshing method to the static inventory event channel to listen for data modifications automatically
        InventoryManager.OnInventoryUpdated += RefreshStoreInterface;
    }

    // Built-in Unity execution method triggered automatically whenever this specific game object components are toggled to an inactive state
    private void OnDisable()
    {
        // Unsubscribes the interface refreshing method from the static inventory event channel to cleanly avoid memory leaks or dangling null reference executions
        InventoryManager.OnInventoryUpdated -= RefreshStoreInterface;
    }

    // Built-in Unity execution method that triggers automatically on the first frame of gameplay initialization
    private void Start()
    {
        // Invokes an initial rendering update loop to populate the store's text interfaces with the player's active startup data states
        RefreshStoreInterface();
    }

    // Public modification method that evaluates player data properties to overwrite user interface display elements dynamically
    public void RefreshStoreInterface()
    {
        // Evaluates if the centralized storage management system is completely missing from memory slots, and breaks out early if true
        if (InventoryManager.Instance == null) return;

        // Evaluates if the canvas wallet text display component reference allocation is verified in memory
        if (goldBalanceText != null)
        {
            // Overwrites the display text using string interpolation formatting to reveal the player's immediate updated currency amount balance
            goldBalanceText.text = $"Gold: {InventoryManager.Instance.totalGold}";
        }

        // Evaluates if the target canvas double jump interface component reference allocation is verified in memory
        if (doubleJumpButtonText != null)
        {
            // Evaluates the boolean condition flag on the instance and uses a ternary operator to assign either an ownership confirmation string or a pricing string layout
            doubleJumpButtonText.text = InventoryManager.Instance.hasDoubleJumpUnlocks ? "OWNED" : "BUY ($0.99)";
        }

        // Evaluates if the target canvas speed boost interface component reference allocation is verified in memory
        if (speedBoostButtonText != null)
        {
            // Evaluates the boolean condition flag on the instance and uses a ternary operator to assign either an ownership confirmation string or a pricing string layout
            speedBoostButtonText.text = InventoryManager.Instance.hasSpeedBoostUnlocks ? "OWNED" : "BUY ($1.99)";
        }
    }
}