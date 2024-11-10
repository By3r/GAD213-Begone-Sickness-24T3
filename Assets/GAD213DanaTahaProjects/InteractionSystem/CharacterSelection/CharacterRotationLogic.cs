using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotationLogic : MonoBehaviour
{
    #region Variables.
    public CharacterSelectionMenu menu;
    public Material characterMaterial;               
    public Material mainMenuMaterial;                
    public float rotationSpeed = 20f;
    public Camera mMCharacterSelectionCamera;

    private bool _isHovering = false;
    private Quaternion _initialRotation;
    #endregion

    private void Start()
    {
        _initialRotation = transform.rotation;
    }

    private void Update()
    {
        RotateCharacterIfMouseIsHovering();
    }

    #region Private Functions.
    private void RotateCharacterIfMouseIsHovering()
    {
        Ray _ray = mMCharacterSelectionCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(_ray, out hit))
        {
            if (hit.transform == transform)
            {
                _isHovering = true;
                if (Input.GetMouseButtonDown(0))
                {
                    menu.SetCurrentSelectedMaterials(characterMaterial, mainMenuMaterial);
                    // Debug.Log("You Selected: " + characterMaterial.name);
                }
            }
            else
            {
                _isHovering = false;
            }
        }
        else
        {
            _isHovering = false;
        }

        if (_isHovering)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, _initialRotation, Time.deltaTime * rotationSpeed);
        }
    }
    #endregion
}
