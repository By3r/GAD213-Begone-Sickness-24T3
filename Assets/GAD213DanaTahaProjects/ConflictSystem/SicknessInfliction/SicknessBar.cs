using UnityEngine;
using UnityEngine.UI;

public class SicknessBar : MonoBehaviour
{
    #region Variables
    public float sicknessIncreaseRate = 0.5f;
    public Slider sicknessSlider;

    [SerializeField] private float maxSickness = 100f;
    [SerializeField] private float decreasingRate = 2f;

    private float _currentSickness;
    private bool _isPlayerInSicknessRoom = false;
    #endregion

    private void Start()
    {
        _currentSickness = 0f;

        if (sicknessSlider != null)
        {
            SetDefaultSicknessBarValues("The sickness bar's Activeness is:", false, maxSickness, _currentSickness);
        }
    }

    private void Update()
    {
        if (_isPlayerInSicknessRoom)
        {
            IncreaseSickness(sicknessIncreaseRate * Time.deltaTime);
        }
        else
        {
            DecreaseScikness(decreasingRate * Time.deltaTime);
        }

        UpdateUI();
    }

    #region Public Functions
    public void EnterSicknessRoom()
    {
        _isPlayerInSicknessRoom = true;

        if (sicknessSlider != null)
        {
            sicknessSlider.gameObject.SetActive(true);
        }
    }

    public void ExitSicknessRoom()
    {
        _isPlayerInSicknessRoom = false;

        if (sicknessSlider != null && _currentSickness <= 0)
        {
            sicknessSlider.gameObject.SetActive(false);
        }
    }

    public void IncreaseSickness(float amount)
    {
        _currentSickness = Mathf.Clamp(_currentSickness + amount, 0, maxSickness);

        if (_currentSickness >= maxSickness)
        {
            Debug.Log("Player dies");
        }
    }
    public void DecreaseScikness(float amount)
    {
        _currentSickness = Mathf.Clamp(_currentSickness - amount, 0, maxSickness);
    }

    public void UpdateUI()
    {
        if (sicknessSlider != null)
        {
            sicknessSlider.value = _currentSickness;
        }
    }
    #endregion

    #region Private Functions
    private void SetDefaultSicknessBarValues(string boolDescription, bool ttrue, float maxSicknessValue, float currentSicknessValue)
    {
        sicknessSlider.gameObject.SetActive(ttrue);
        sicknessSlider.maxValue = maxSicknessValue;
        sicknessSlider.value = currentSicknessValue;
    }
    #endregion
}
