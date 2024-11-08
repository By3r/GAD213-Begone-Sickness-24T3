using UnityEngine;
using Cinemachine;

public class PlayerFollowingCam : MonoBehaviour
{
    #region Variables.
    [Header("Camera Speed Settings")]
    [SerializeField] private float zoomSpeed = 4f;

    [Header("Cinemachine Reference")]
    [SerializeField] private CinemachineFreeLook freeLookCamera;

    [Header("Camera Zoom settings")]
    [SerializeField] private float minZoomFOV = 30f;
    [SerializeField] private float maxZoomFOV = 70f;

    private float targetFOV;
    #endregion

    private void Start()
    {
        if (freeLookCamera == null)
        {
            Debug.LogError("no cinemach found.");
            enabled = false;
            return;
        }
        targetFOV = freeLookCamera.m_Lens.FieldOfView;
    }

    private void Update()
    {
        HandleZoomInput();
    }

    #region Private Functions

    /// <summary>
    /// Handles zoom input using the mouse scroll wheel.
    /// </summary>
    private void HandleZoomInput()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scrollInput) > 0.01f)
        {
            targetFOV -= scrollInput * zoomSpeed;
            targetFOV = Mathf.Clamp(targetFOV, minZoomFOV, maxZoomFOV);
            freeLookCamera.m_Lens.FieldOfView = targetFOV;
        }
    }

    #endregion
}
