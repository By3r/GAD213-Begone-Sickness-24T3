using UnityEngine;

public class InventoryBarToggler : MonoBehaviour
{
    #region Variables
    public Animator animator;

    private bool _inventoryBarOpen = false;

    #endregion

    void Start()
    {
        ToggleInventoryBar();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleInventoryBar();
        }
    }

    public void ToggleInventoryBar()
    {
        animator.SetBool("InventoryOpen", _inventoryBarOpen);
        _inventoryBarOpen = !_inventoryBarOpen;
    }
}
