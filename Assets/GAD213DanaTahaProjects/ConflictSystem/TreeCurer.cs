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

    private Sprite _currentFlask;
    private bool _isPlayerInRange = false;
    private Renderer _treeRenderer;
    #endregion

    private void Start()
    {
        treeCurePanel.SetActive(false);
        _treeRenderer = GetComponent<Renderer>();
        UpdateFlaskName(""); 
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
        if (other.CompareTag("Player") && !isCured)
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
                UpdateFlaskName(""); 
            }

            ClosePanel();
        }
    }

    public void OpenPanel()
    {
        treeCurePanel.SetActive(true);

        if (!isCured)
        {
            sicknessBar.sicknessSlider.gameObject.SetActive(false);
        }
    }

    public void ClosePanel()
    {
        treeCurePanel.SetActive(false);

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
            sicknessBar.DecreaseScikness(50f);
            _treeRenderer.material = curedTreeMaterial;
            sicknessBar.sicknessSlider.gameObject.SetActive(false);
            ClosePanel();
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

    private void UpdateFlaskName(string flaskName)
    {
        if (flaskNameText != null)
        {
            flaskNameText.text = flaskName; 
        }
    }
}
