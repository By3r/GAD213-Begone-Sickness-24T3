using System.Collections;
using UnityEngine;

public class PlayerFollowingCam : MonoBehaviour
{
    #region Variables.
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 camOffset = new Vector3(0, 2, -15);
    [SerializeField] private float camRotationSpeed = 10f;
    [SerializeField] private float camAutoFocusTime = 3f;
    [SerializeField] private float cameraMovementSpeed = 2f;


    private float _lastMovementTime; 
    private bool _isAutoFocused = false;
    #endregion 

    private void Start()
    {
        _lastMovementTime = Time.time;
    }
    private void Update()
    {
        CamRotationLogic();

        if (Time.time - _lastMovementTime > camAutoFocusTime && !_isAutoFocused)
        {
            StartCoroutine(AutoFocusCamera());
        }
    }

    #region Private Functions.
    private void CamRotationLogic()
    {
        float _mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        if (Mathf.Abs(_mouseX) > 0.01f || Mathf.Abs(mouseY) > 0.01f)
        {
            _lastMovementTime = Time.time;
            _isAutoFocused = false;
            transform.RotateAround(player.position, Vector3.up, _mouseX * camRotationSpeed * Time.deltaTime);
            float newRotationX = Mathf.Clamp(transform.eulerAngles.x - mouseY * camRotationSpeed * Time.deltaTime, -10, 80);
            transform.eulerAngles = new Vector3(newRotationX, transform.eulerAngles.y, 0);
        }

        Vector3 _camAtPlayerForwardPosition = player.position + transform.rotation * camOffset;
        transform.position = Vector3.Lerp(transform.position, _camAtPlayerForwardPosition, cameraMovementSpeed * Time.deltaTime);
    }

    private IEnumerator AutoFocusCamera()
    {
        _isAutoFocused = true;
        Quaternion targetRotation = Quaternion.LookRotation(player.forward); 

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, cameraMovementSpeed * Time.deltaTime); 
            yield return null;
        }

        _isAutoFocused = false;
    }

    #endregion


}
