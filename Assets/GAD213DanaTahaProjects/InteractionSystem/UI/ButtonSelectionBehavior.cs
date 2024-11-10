using TMPro;
using UnityEngine;

public class ButtonSelectionBehavior : MonoBehaviour
{
    #region Variables
    public TMP_Text buttonText;

    private Color _selectedColor = Color.black;
    private Color _notselectedColor = new Color(216, 255, 0);
    #endregion

    private void Start()
    {
        buttonText.color = _notselectedColor;
    }

    #region Public Functions
    public void IfSelected()
    {
        buttonText.color = _selectedColor;
    }

    public void NotSelected()
    {
        buttonText.color = _notselectedColor;
    }
    #endregion
}
