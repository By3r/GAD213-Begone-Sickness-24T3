using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using TMPro;

public class TreeCurer : MonoBehaviour
{
    #region Variables
    public GameObject treeCurePanel;
    public Image cureSlot;
    public Material curedTreeMaterial;
    public PlayerInventory playerInventory;
    public Sprite correctFlask;
    public bool isCured = false;

    public UnityEvent onPlayerEnter;
    public UnityEvent onPlayerExit;

    [SerializeField] private SicknessBar sicknessBar;
    [SerializeField] private TMP_Text flaskNameText;
    [SerializeField] private GameObject interactionPanel;
    [SerializeField] private Door door;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject distressedNPC;
    [SerializeField] private InventoryBarToggler inventoryBarToggler;

    private Sprite _currentFlask;
    private bool _isPlayerInRange = false;
    private Renderer _treeRenderer;
    private Material originalTreeMaterial;
    #endregion

    private void Start()
    {
        treeCurePanel.SetActive(false);
        interactionPanel.SetActive(false);
        _treeRenderer = GetComponent<Renderer>();
        originalTreeMaterial = _treeRenderer.material;
        UpdateFlaskName("");
    }

    private void Update()
    {
        if (_isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !isCured && player.activeSelf)
        {
            OpenPanel();
            if (!inventoryBarToggler.inventoryBarOpen)
            {
                inventoryBarToggler.UpdateInventoryBarState(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && player.activeSelf)
        {
            if (isCured)
            {
                ToggleInteractionPanel(false);
                return;
            }

            _isPlayerInRange = true;
            ToggleInteractionPanel(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = false;
            ToggleInteractionPanel(false);

            if (_currentFlask != null)
            {
                _currentFlask = null;
                cureSlot.sprite = null;
                UpdateFlaskName("");
            }

            ClosePanel();
        }
    }

    public void OpenPanel()
    {
        treeCurePanel.SetActive(true);
        sicknessBar.sicknessSlider.gameObject.SetActive(false);
    }

    #region Public Functions.
    public void ClosePanel()
    {
        treeCurePanel.SetActive(false);
        inventoryBarToggler.UpdateInventoryBarState(false); 

        if (_currentFlask != null)
        {
            _currentFlask = null;
            cureSlot.sprite = null;
            UpdateFlaskName("");
        }

        if (!isCured)
        {
            sicknessBar.sicknessSlider.gameObject.SetActive(true);
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
                UpdateFlaskName("");
            }
        }
    }

    public bool PlaceFlaskInSlot(Sprite flaskSprite)
    {
        if (_currentFlask == null)
        {
            _currentFlask = flaskSprite;
            cureSlot.sprite = flaskSprite;
            UpdateFlaskName(flaskSprite.name);

            if (flaskSprite != correctFlask)
            {
                sicknessBar.SetSicknessIncreaseRate(sicknessBar.sicknessIncreaseRate + 0.5f);
                sicknessBar.IncreaseSickness(sicknessBar.sicknessIncreaseRate);
            }

            return true;
        }

        return false;
    }

    public void ConfirmSelection()
    {
        if (_currentFlask == null)
        {
            return;
        }

        if (_currentFlask == correctFlask)
        {
            isCured = true;
            distressedNPC.SetActive(false);
            sicknessBar.DecreaseScikness(50f);
            _treeRenderer.material = curedTreeMaterial;
            sicknessBar.ResetSicknessBar();
            ClosePanel();

            if (door != null)
            {
                door.ForceOpen();
            }

            ToggleInteractionPanel(false);
        }
        else
        {
            sicknessBar.IncreaseSickness(sicknessBar.sicknessIncreaseRate + 1);
            _currentFlask = null;
            cureSlot.sprite = null;
            UpdateFlaskName("");
            ClosePanel();
        }
    }

    public void ToggleInteractionPanel(bool isVisible)
    {
        if (interactionPanel != null)
        {
            interactionPanel.SetActive(isVisible);
        }
    }

    public void ResetTreeCurer()
    {
        isCured = false;
        _isPlayerInRange = false;
        _treeRenderer.material = originalTreeMaterial;
        _currentFlask = null;
        cureSlot.sprite = null;
        UpdateFlaskName("");
        treeCurePanel.SetActive(false);
        ToggleInteractionPanel(false);
    }
    #endregion

    #region Private Functions
    private void UpdateFlaskName(string flaskName)
    {
        if (flaskNameText != null)
        {
            flaskNameText.text = flaskName;
        }
    }
    #endregion
}
