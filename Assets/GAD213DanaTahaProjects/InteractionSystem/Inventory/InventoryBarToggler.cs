using UnityEngine;

public class InventoryBarToggler : MonoBehaviour
{
    #region Variables
    public Animator animator;
    public bool inventoryBarOpen = false;

    #endregion

    void Start()
    {
        ToggleInventoryBar();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && !inventoryBarOpen)
        {
            ToggleInventoryBar();
        }
    }

    public void ToggleInventoryBar()
    {
        animator.SetBool("InventoryOpen", inventoryBarOpen);
        inventoryBarOpen = !inventoryBarOpen;
    }
}
