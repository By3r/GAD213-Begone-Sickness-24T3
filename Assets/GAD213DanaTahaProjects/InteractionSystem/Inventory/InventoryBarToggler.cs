using UnityEngine;

public class InventoryBarToggler : MonoBehaviour
{
    #region Variables
    public Animator animator;
    public bool inventoryBarOpen = false;

    #endregion

    void Start()
    {
        UpdateInventoryBarState(false); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleInventoryBar();
        }
    }

    #region Public Functions.
    public void ToggleInventoryBar()
    {
        UpdateInventoryBarState(!inventoryBarOpen);
    }

    public void UpdateInventoryBarState(bool isOpen)
    {
        inventoryBarOpen = isOpen;
        animator.SetBool("InventoryOpen", inventoryBarOpen);
    }
    #endregion
}
