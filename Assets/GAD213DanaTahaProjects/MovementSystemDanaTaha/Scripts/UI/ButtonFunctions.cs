using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    #region Variables.
    [SerializeField] private GameObject creditsPanel;
    #endregion

    #region Public Functions.
    // ----------------------------
    #region Play Button.
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    #endregion

    #region Credits Button.
    public void CreditsPanelToggler()
    {
        bool isActive = creditsPanel.activeSelf;
        creditsPanel.SetActive(!isActive);
    }
    #endregion

    #region Quit button.
    public void Quit()
    {
        Application.Quit();
    }
    #endregion
    // ----------------------------
    #endregion
}
