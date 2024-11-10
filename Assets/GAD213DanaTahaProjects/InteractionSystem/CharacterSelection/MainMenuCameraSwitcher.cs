using UnityEngine;

public class MainMenuCameraSwitcher : MonoBehaviour
{
    #region
    public Camera mainCamera;
    public Camera selectionCamera;
    #endregion

    #region Public Functions.
    public void ShowCharacterSelectionCamera()
    {
        mainCamera.gameObject.SetActive(false);
        selectionCamera.gameObject.SetActive(true);
    }

    public void ShowMainCamera()
    {
        mainCamera.gameObject.SetActive(true);
        selectionCamera.gameObject.SetActive(false);
    }
    #endregion
}
