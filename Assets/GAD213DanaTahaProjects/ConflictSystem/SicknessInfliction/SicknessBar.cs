using UnityEngine;
using UnityEngine.UI;

public class SicknessBar : MonoBehaviour
{
    #region Variables
    public float sicknessIncreaseRate;
    public Slider sicknessSlider;

    [SerializeField] private float maxSickness = 100f;
    [SerializeField] private float decreasingRate = 2f;
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 playerRespawnLocation;
    [SerializeField] private SicknessRoom sicknessRoom;
    [SerializeField] private TreeCurer treeCurer;
    [SerializeField] private Door door;

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
    public void ResetSicknessBar()
    {
        _currentSickness = 0f;
        sicknessSlider.value = _currentSickness;
        sicknessSlider.gameObject.SetActive(false);
    }

    public bool IsSicknessBarActive()
    {
        return sicknessSlider.gameObject.activeSelf;
    }

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
        _currentSickness = 0f;

        if (sicknessSlider != null && _currentSickness <= 0f)
        {
            sicknessSlider.gameObject.SetActive(false);
        }
    }

    public void IncreaseSickness(float amount)
    {
        _currentSickness = Mathf.Clamp(_currentSickness + amount, 0, maxSickness);

        if (_currentSickness >= maxSickness)
        {
            SetSicknessIncreaseRate(0.5f);
            player.SetActive(false);
            _currentSickness = 0f;
            player.transform.position = playerRespawnLocation;

            ResetSicknessBar();
            sicknessRoom.ResetSicknessRoom();
            treeCurer.ResetTreeCurer();
            door.ResetDoor();

            Invoke("SetPlayerBackAsActive", 3f);
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

    public void SetSicknessIncreaseRate(float newIncreaseRate)
    {
        sicknessIncreaseRate = newIncreaseRate;
    }
    #endregion

    #region Private Functions
    private void SetDefaultSicknessBarValues(string boolDescription, bool ttrue, float maxSicknessValue, float currentSicknessValue)
    {
        sicknessSlider.gameObject.SetActive(ttrue);
        sicknessSlider.maxValue = maxSicknessValue;
        sicknessSlider.value = currentSicknessValue;
    }

    private void SetPlayerBackAsActive()
    {
        player.SetActive(true);
    }
    #endregion
}
