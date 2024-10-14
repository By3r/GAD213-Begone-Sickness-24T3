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


    private float _lastMovementTime; // last time the mouse have moved by.
    private bool _isMouseMoving = false;
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

        if (Mathf.Abs(_mouseX) > 0.01f)
        {
            _lastMovementTime = Time.time;
            _isAutoFocused = false;
            _isMouseMoving = true;
            transform.RotateAround(player.position, Vector3.up, _mouseX * camRotationSpeed * Time.deltaTime);
        }
        else { _isMouseMoving = false; }

        Vector3 _camAtPlayerForwardPosition = player.position + camOffset;
        transform.position = Vector3.Lerp(transform.position, _camAtPlayerForwardPosition, cameraMovementSpeed * Time.deltaTime);
        transform.LookAt(player.position + Vector3.up * camOffset.y);
    }
    private IEnumerator AutoFocusCamera()
    {

        _isAutoFocused = true;
        Quaternion _targetRotation = Quaternion.LookRotation(player.forward);

        while (Quaternion.Angle(transform.rotation, _targetRotation) > 0.1f && !_isMouseMoving)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, camAutoFocusTime * Time.deltaTime);
            yield return null;
        }
        _isAutoFocused = false;
    }
    #endregion


}
