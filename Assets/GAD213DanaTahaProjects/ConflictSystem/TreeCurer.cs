using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;


/// <summary>
/// Created this for tree curing logic, It opens the curepanel of a connected panel and players utilise
/// their inventory to place and remove flasks.
/// </summary>

public class TreeCurer : MonoBehaviour
{
    #region Variables
    public GameObject treeCurePanel;
    public Image cureSlot;
    public Material curedTreeMaterial;
    public PlayerInventory playerInventory;
    public Sprite correctFlask;

    public UnityEvent onPlayerEnter;
    public UnityEvent onePlayerExit;

    private bool _isCured = false;
    private Sprite _currentFlask;
    private bool _isPlayerInRange = false;
    private Renderer _treeRenderer;
    #endregion

    private void Start()
    {
        treeCurePanel.SetActive(false);

        _treeRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (_isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            OpenPanel();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isCured)
        {
            _isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = false;

            if (_currentFlask != null)
            {
                _currentFlask = null;
                cureSlot.sprite = null;
            }

            ClosePanel();
        }
    }

    public void OpenPanel()
    {
        treeCurePanel.SetActive(true);
    }

    public void ClosePanel()
    {
        treeCurePanel.SetActive(false);

        if (_currentFlask != null)
        {
            _currentFlask = null;
            cureSlot.sprite = null;
        }
    }

    public void OnSlotClicked()
    {
        if (_currentFlask != null)
        {
            bool added = playerInventory.AddFlask(null, _currentFlask);
            if (added)
            {
                _currentFlask = null;
                cureSlot.sprite = null;
            }
        }
    }

    public void PlaceFlaskInSlot(Sprite flaskSprite)
    {
        if (_currentFlask == null)
        {
            _currentFlask = flaskSprite;
            cureSlot.sprite = flaskSprite;
        }
    }

    public void ConfirmSelection()
    {
        if (_currentFlask == null)
        {
            return;
        }

        if (_currentFlask == correctFlask)
        {
            _isCured = true;
            _treeRenderer.material = curedTreeMaterial;
            ClosePanel();
        }
    }
}
