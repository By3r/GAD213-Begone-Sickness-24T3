using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

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
    public TMP_Text[] craftingSlotTexts;
    public TMP_Text resultNameText;
    public Sprite errorSprite;
    private Sprite[] _craftingItems = new Sprite[2];
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
                inventoryBarToggler.UpdateInventoryBarState(true); 
                onPlayerExitTrigger.Invoke();
            }
            else
            {
                CloseCraftingUI();
                inventoryBarToggler.UpdateInventoryBarState(false);
            }
        }
    }

    #region Trigger Events
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = true;
            onPlayerEnterTrigger.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = false;
            onPlayerExitTrigger.Invoke();

            if (_isCraftingUIOpen)
            {
                CloseCraftingUI();
                inventoryBarToggler.UpdateInventoryBarState(false); 
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

    public bool PlaceInCraftingSlot(Sprite flaskSprite)
    {
        if (!_isCraftingUIOpen)
        {
            return false;
        }

        for (int i = 0; i < _craftingItems.Length; i++)
        {
            if (_craftingItems[i] == null)
            {
                _craftingItems[i] = flaskSprite;
                craftingSlots[i].sprite = flaskSprite;
                craftingSlotTexts[i].text = flaskSprite.name;
                CheckRecipe();
                return true;
            }
        }
        return false;
    }

    public void OnCraftingSlotClicked(int slotIndex)
    {
        if (!_isCraftingUIOpen || _craftingItems[slotIndex] == null) return;

        Sprite flaskSprite = _craftingItems[slotIndex];
        bool added = playerInventory.AddFlask(null, flaskSprite);
        if (!added) return;

        _craftingItems[slotIndex] = null;
        craftingSlots[slotIndex].sprite = null;
        craftingSlotTexts[slotIndex].text = "";
        craftingSlots[2].sprite = null;
        resultNameText.text = "";
        _resultSprite = null;
        _resultName = null;

        CheckRecipe();
    }

    public void OnResultSlotClicked()
    {
        if (_resultSprite == null || _resultSprite == errorSprite) return;

        bool added = playerInventory.AddFlask(null, _resultSprite);
        if (added)
        {
            craftingSlots[2].sprite = null;
            resultNameText.text = "";
            _resultSprite = null;
            _resultName = null;

            for (int i = 0; i < _craftingItems.Length; i++)
            {
                _craftingItems[i] = null;
                craftingSlots[i].sprite = null;
                craftingSlotTexts[i].text = "";
            }
        }
    }
    #endregion

    #region Private Functions
    private void CheckRecipe()
    {
        if (craftingSlots[0].sprite == null || craftingSlots[1].sprite == null) return;

        string[] ingredientNames = new string[2];
        ingredientNames[0] = craftingSlots[0].sprite.name;
        ingredientNames[1] = craftingSlots[1].sprite.name;

        foreach (CraftingRecipe recipe in recipes)
        {
            if (System.Array.Exists(recipe.ingredients, x => x == ingredientNames[0]) &&
                System.Array.Exists(recipe.ingredients, x => x == ingredientNames[1]))
            {
                _resultName = recipe.resultName;
                _resultSprite = recipe.resultSprite;
                craftingSlots[2].sprite = _resultSprite;
                resultNameText.text = _resultName;
                return;
            }
        }

        _resultSprite = errorSprite;
        craftingSlots[2].sprite = errorSprite;
        resultNameText.text = "Invalid Recipe";
        _resultName = null;
    }

    private void ClearCraftingSlots()
    {
        for (int i = 0; i < craftingSlots.Length; i++)
        {
            craftingSlots[i].sprite = null;
            craftingSlotTexts[i].text = "";
        }
        _craftingItems[0] = null;
        _craftingItems[1] = null;
        craftingSlots[2].sprite = null;
        resultNameText.text = "";
        _resultSprite = null;
        _resultName = null;
    }

    private void ReturnIngredients()
    {
        for (int i = 0; i < _craftingItems.Length; i++)
        {
            if (_craftingItems[i] != null)
            {
                Sprite flaskSprite = _craftingItems[i];
                bool added = playerInventory.AddFlask(null, flaskSprite);
                _craftingItems[i] = null;
                craftingSlotTexts[i].text = "";
            }
        }
    }
    #endregion
}
