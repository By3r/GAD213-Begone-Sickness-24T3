using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    #region Variables
    public GameObject[] inventory = new GameObject[8];
    public Sprite[] inventorySprites = new Sprite[8];
    public InventoryUI inventoryUI;
    #endregion

    public bool AddFlask(GameObject flask, Sprite flaskSprite)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null && inventorySprites[i] == null) 
            {
                inventory[i] = flask;
                inventorySprites[i] = flaskSprite;
                inventoryUI.UpdateInventoryUI();
                return true;
            }
        }
        Debug.Log("Inventory is full!");
        return false;
    }



    public void RemoveFlask(int index)
    {
        if (index >= 0 && index < inventory.Length)
        {
            inventory[index] = null;
            inventorySprites[index] = null;
            inventoryUI.UpdateInventoryUI();
        }
    }

    public Sprite GetFlaskSprite(int index)
    {
        if (index >= 0 && index < inventorySprites.Length)
        {
            return inventorySprites[index];
        }

        return null;
    }
}
