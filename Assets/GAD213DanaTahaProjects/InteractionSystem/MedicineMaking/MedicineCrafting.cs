using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MedicineCrafting : MonoBehaviour
{
    #region Variables
    public CraftingRecipe[] recipes;
    public GameObject craftingUI;
    public PlayerInventory playerInventory;
    public InventoryBarToggler inventoryBarToggler;

    public UnityEvent onPlayerEnterTrigger;
    public UnityEvent onPlayerExitTrigger;

    public Image[] craftingSlots;
    public Sprite errorSprite;
    private GameObject[] _craftingItems = new GameObject[2];
    private Sprite _resultSprite;
    private string _resultName;

    private bool _isPlayerInRange = false;
    private bool _isCraftingUIOpen = false;
    #endregion

    private void Update()
    {
        if (_isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!_isCraftingUIOpen)
            {
                OpenCraftingUI();
                onPlayerExitTrigger.Invoke();
                inventoryBarToggler.ToggleInventoryBar();
            }
        }

        if (_isCraftingUIOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseCraftingUI();
            inventoryBarToggler.ToggleInventoryBar();
        }
    }

    #region Triggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onPlayerEnterTrigger.Invoke();
            _isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onPlayerExitTrigger.Invoke();
            _isPlayerInRange = false;
            if (_isCraftingUIOpen)
            {
                CloseCraftingUI();
            }
        }
    }
    #endregion

    #region Public Functions
    public void OpenCraftingUI()
    {
        craftingUI.SetActive(true);
        _isCraftingUIOpen = true;
        ClearCraftingSlots();
    }

    public void CloseCraftingUI()
    {
        craftingUI.SetActive(false);
        _isCraftingUIOpen = false;
        ReturnIngredients();
        ClearCraftingSlots();
    }

    public void OnInventorySlotClicked(int inventorySlot)
    {
        if (!_isCraftingUIOpen) return;

        GameObject flask = playerInventory.inventory[inventorySlot];
        if (flask != null && (_craftingItems[0] == null || _craftingItems[1] == null))
        {
            FlaskPickup flaskData = flask.GetComponent<FlaskPickup>();
            if (flaskData != null)
            {
                for (int i = 0; i < _craftingItems.Length; i++)
                {
                    if (_craftingItems[i] == null)
                    {
                        _craftingItems[i] = flask;
                        craftingSlots[i].sprite = flaskData.flaskSprite;
                        playerInventory.RemoveFlask(inventorySlot);
                        CheckRecipe();
                        return;
                    }
                }
            }
        }
    }


    public void OnCraftingSlotClicked(int slotIndex)
    {
        if (!_isCraftingUIOpen || _craftingItems[slotIndex] == null) return;

        FlaskPickup flaskData = _craftingItems[slotIndex].GetComponent<FlaskPickup>();
        if (flaskData != null)
        {
            playerInventory.AddFlask(_craftingItems[slotIndex], flaskData.flaskSprite);
        }

        _craftingItems[slotIndex] = null;
        craftingSlots[slotIndex].sprite = null;
        craftingSlots[2].sprite = null;
        _resultSprite = null;
        _resultName = null;
    }

    public void OnResultSlotClicked()
    {
        if (_resultSprite == errorSprite || _resultSprite == null) return;

        for (int i = 0; i < _craftingItems.Length; i++)
        {
            _craftingItems[i] = null;
            craftingSlots[i].sprite = null;
        }

        playerInventory.AddFlask(new GameObject(_resultName), _resultSprite);

        craftingSlots[2].sprite = null;
        _resultSprite = null;
        _resultName = null;
    }
    #endregion

    #region Private Functions
    private void CheckRecipe()
    {
        if (_craftingItems[0] == null || _craftingItems[1] == null) return;

        string[] ingredientNames = new string[2];
        ingredientNames[0] = _craftingItems[0].GetComponent<FlaskPickup>().flaskName;
        ingredientNames[1] = _craftingItems[1].GetComponent<FlaskPickup>().flaskName;

        foreach (CraftingRecipe recipe in recipes)
        {
            if (System.Array.Exists(recipe.ingredients, x => x == ingredientNames[0]) &&
                System.Array.Exists(recipe.ingredients, x => x == ingredientNames[1]))
            {
                _resultName = recipe.resultName;
                _resultSprite = recipe.resultSprite;
                craftingSlots[2].sprite = _resultSprite;
                return;
            }
        }

        _resultSprite = errorSprite;
        craftingSlots[2].sprite = errorSprite;
        _resultName = null;
    }


    private void ClearCraftingSlots()
    {
        for (int i = 0; i < craftingSlots.Length; i++)
        {
            craftingSlots[i].sprite = null;
        }
        _craftingItems[0] = null;
        _craftingItems[1] = null;
        _resultSprite = null;
        _resultName = null;

    }


    private void ReturnIngredients()
    {
        for (int i = 0; i < _craftingItems.Length; i++)
        {
            if (_craftingItems[i] != null)
            {
                FlaskPickup flaskData = _craftingItems[i].GetComponent<FlaskPickup>();
                if (flaskData != null)
                {
                    playerInventory.AddFlask(_craftingItems[i], flaskData.flaskSprite);
                }
                _craftingItems[i] = null;
            }
        }
    }
    #endregion
}
