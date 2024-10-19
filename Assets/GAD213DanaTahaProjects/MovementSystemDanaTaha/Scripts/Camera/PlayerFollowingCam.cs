using System.Collections;
using UnityEngine;

public class PlayerFollowingCam : MonoBehaviour
{
    #region Variables.
    [Header("Camera Speed Settings")]
    [SerializeField] private float camRotationSpeed = 140f;
    [SerializeField] private float cameraMovementSpeed = 8f;
    [SerializeField] private float zoomSpeed = 4f;

    [Header("Camera Zoom settings")]
    [SerializeField] private float _minZoomDistance = 5f;
    [SerializeField] private float _maxZoomDistance = 30f;

    [Header("Player, offset and ground")]
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 camOffset = new Vector3(0, 2, -10);
    [SerializeField] private LayerMask groundLayer;

    private Vector3 _targetOffset;
    #endregion

    private void Start()
    {
        _targetOffset = camOffset;
    }

    private void Update()
    {
        HandleZoomInput();
        CamRotationLogic();
    }

    #region Private Functions.

    private void HandleZoomInput()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0)
        {
            float zoomChange = scrollInput * zoomSpeed;
            _targetOffset.z = Mathf.Clamp(_targetOffset.z + zoomChange, -_maxZoomDistance, -_minZoomDistance);
        }
    }

    private void CamRotationLogic()
    {
        float _mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        if (Mathf.Abs(_mouseX) > 0.01f || Mathf.Abs(mouseY) > 0.01f)
        {
            transform.RotateAround(player.position, Vector3.up, _mouseX * camRotationSpeed * Time.deltaTime);
            Vector3 currentEulerAngles = transform.eulerAngles;
            float pitch = currentEulerAngles.x - mouseY * camRotationSpeed * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, 5, 80);
            transform.rotation = Quaternion.Euler(pitch, transform.eulerAngles.y, 0f);
        }

        _targetOffset.y = Mathf.Lerp(_targetOffset.y, camOffset.y, Time.deltaTime * cameraMovementSpeed);

        Vector3 camAtPlayerForwardPosition = player.position + transform.rotation * _targetOffset;
        transform.position = Vector3.Lerp(transform.position, camAtPlayerForwardPosition, cameraMovementSpeed * Time.deltaTime);
    }
    #endregion
}