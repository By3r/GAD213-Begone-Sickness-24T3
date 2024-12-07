using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;

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
            }
        }
    }

    public bool PlaceFlaskInSlot(Sprite flaskSprite)
    {
        if (_currentFlask == null)
        {
            _currentFlask = flaskSprite;
            cureSlot.sprite = flaskSprite;
            return true;
        }
        return false;
    }

    public void ConfirmSelection()
    {
        if (_currentFlask == null)
        {
            Debug.Log("No flask placed.");
            return;
        }

        if (_currentFlask == correctFlask)
        {
            isCured = true;
            sicknessBar.DecreaseScikness(50f);
            _treeRenderer.material = curedTreeMaterial;
            sicknessBar.sicknessSlider.gameObject.SetActive(false); 
            Debug.Log("Correct flask! Tree cured.");
            ClosePanel();
        }
        else
        {
            sicknessBar.IncreaseSickness(sicknessBar.sicknessIncreaseRate * 2); 
            Debug.Log($"Incorrect flask! Sickness increased. Flask {_currentFlask.name} destroyed.");
            _currentFlask = null; 
            cureSlot.sprite = null; 
        }
    }
}
