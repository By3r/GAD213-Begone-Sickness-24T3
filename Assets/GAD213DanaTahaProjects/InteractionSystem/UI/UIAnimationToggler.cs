using UnityEngine;

public class UIAnimationToggler : MonoBehaviour
{
    [SerializeField] private Animator menuAnimator;
    private bool _isMenuOpen = false;

    #region For ONClick() Events
    public void ToggleTopLeftMenu()
    {
        _isMenuOpen = !_isMenuOpen;
        menuAnimator.SetBool("MenuOpen", _isMenuOpen);
    }
    #endregion
private void Update()
    {
        EscapeKeyActions();
    }

    #region For Keyboard keys

    private void EscapeKeyActions()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ToggleTopLeftMenu();
        }
    }
    #endregion
}