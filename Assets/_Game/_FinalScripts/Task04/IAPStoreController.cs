using UnityEngine;

// This class coordinates the in-app purchase store operations by routing successful purchase transactions directly to the player inventory
public class IAPStoreController : MonoBehaviour
{
    // Public receiver method triggered automatically when the monetization platform completes a transaction successfully
    public void OnPurchaseCompleteString(string productId)
    {
        Debug.Log($"Store Layer: Purchase event caught for Product ID: {productId}. Directing to inventory system.");

        // Bypasses external web API loops and directly credits the local data system
        ProcessInventoryCredit(productId);
    }

    // Internal routing method that processes item distribution branches depending on the verified product text code identifier
    private void ProcessInventoryCredit(string productId)
    {
        // Evaluates if the centralized storage structural management instance is completely missing from memory slots, and exits early if true
        if (InventoryManager.Instance == null)
        {
            Debug.LogError("Store Layer Error: InventoryManager Instance could not be found!");
            return;
        }

        // Switches execution flow paths to match the specific text code identifier string argument
        switch (productId)
        {
            case "gold_100": InventoryManager.Instance.AddGold(100); break;
            case "gold_200": InventoryManager.Instance.AddGold(200); break;
            case "gold_300": InventoryManager.Instance.AddGold(300); break;
            case "gold_400": InventoryManager.Instance.AddGold(400); break;
            case "gold_500": InventoryManager.Instance.AddGold(500); break;
            case "double_jump": InventoryManager.Instance.UnlockItem("double_jump"); break;
            case "speed_boost": InventoryManager.Instance.UnlockItem("speed_boost"); break;
            default: Debug.LogWarning($"Store Layer: Unrecognized Product ID received: {productId}"); break;
        }
    }
}