using UnityEngine;
using UnityEngine.UI;

public class SicknessBar : MonoBehaviour
{
    #region Variables

    [SerializeField] private float maxSickness = 100f; 
    [SerializeField] private float sicknessIncreaseRate = 5f;
    [SerializeField] private float regenerationRate = 2f; 
    [SerializeField] private Slider sicknessBar; 

    private float _currentSickness;

    private bool isPlayerInSicknessRoom = false;

    #endregion

    private void Start()
    {
        _currentSickness = 0f;

        if (sicknessBar != null)
        {
            sicknessBar.gameObject.SetActive(false); 
            sicknessBar.maxValue = maxSickness;
            sicknessBar.value = _currentSickness;
        }
    }

    private void Update()
    {
        if (isPlayerInSicknessRoom)
        {
            IncreaseSickness(sicknessIncreaseRate * Time.deltaTime);
        }
        else
        {
            RegenerateSickness(regenerationRate * Time.deltaTime);
        }

        UpdateUI();
    }

    public void EnterSicknessRoom()
    {
        isPlayerInSicknessRoom = true;

        if (sicknessBar != null)
        {
            sicknessBar.gameObject.SetActive(true); 
        }
    }

    public void ExitSicknessRoom()
    {
        isPlayerInSicknessRoom = false;

        if (sicknessBar != null && _currentSickness <= 0)
        {
            sicknessBar.gameObject.SetActive(false); 
        }
    }

    public void IncreaseSickness(float amount)
    {
        _currentSickness = Mathf.Clamp(_currentSickness + amount, 0, maxSickness);
    }

    private void RegenerateSickness(float amount)
    {
        _currentSickness = Mathf.Clamp(_currentSickness - amount, 0, maxSickness);
    }

    private void UpdateUI()
    {
        if (sicknessBar != null)
        {
            sicknessBar.value = _currentSickness;
        }
    }
}
