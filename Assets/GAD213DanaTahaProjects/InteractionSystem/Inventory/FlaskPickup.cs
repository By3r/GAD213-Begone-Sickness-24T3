using UnityEngine;
using UnityEngine.Events;

public class FlaskPickup : MonoBehaviour
{
    #region Variables
    public string flaskName;
    public Sprite flaskSprite;
    public UnityEvent onPlayerEnterTrigger;
    public UnityEvent onPlayerExitTrigger;
    public PlayerInventory inventory;
    public GameObject ePanel;

    private bool _isPlayerInRange = false;
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = true;
            onPlayerEnterTrigger.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = false;
            onPlayerExitTrigger.Invoke();
        }
    }

    private void Update()
    {
        if (_isPlayerInRange && Input.GetKeyDown(KeyCode.E) && inventory != null)
        {
            FlaskPickup flaskData = GetComponent<FlaskPickup>();
            if (flaskData != null)
            {
                bool added = inventory.AddFlask(gameObject, flaskData.flaskSprite);
                if (added)
                {
                    Debug.Log($"{flaskData.flaskName} picked up and added to inventory.");
                }
                else
                {
                    Debug.Log("Inventory is full!");
                }
            }
        }
    }


    public void EForPickupToggler()
    {
        if (ePanel != null)
        {
            ePanel.SetActive(!ePanel.activeSelf);
        }
    }
}
