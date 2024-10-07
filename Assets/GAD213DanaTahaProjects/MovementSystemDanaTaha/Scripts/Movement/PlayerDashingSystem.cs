using UnityEngine;
using UnityEngine.UI;

public class PlayerDashingSystem : BasePlayerMovement
{
    #region Variables.
    [Header("Dash Settings")]
    public KeyCode dashKey = KeyCode.LeftShift;
    public float dashDuration = 0.5f;
    public float dashStaminaCost = 20f;

    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaRecoveryRate = 15f;

    // Animation Parameter Label.
    private string _dash = "PlayerDashing";

    // Dash State.
    private bool _isDashing = false;
    private float _dashTimer = 0f;

    // Stamina Bar.
    public Slider staminaBarSlider;
    public Image staminaFillImage;
    #endregion

    #region Protected Functions.
    protected override void Start()
    {
        base.Start();
        StaminaValuesInitialiser();
    }

    protected override void Update()
    {
        base.Update();

        HandleDashInput();
        HandleDashDuration();
        HandleStaminaRecovery();
        UpdateStaminaUI();
    }
    /// <summary>
    /// Adds dash parameter.
    /// </summary>
    protected override void UpdateAnimatorParameters()
    {
        if (_isDashing)
        {
            _animator.SetBool(_dash, true);
            _animator.SetBool(_run, false);
            _animator.SetBool(_idle, false);
        }
        else if (_inputDirection != Vector3.zero)
        {
            _animator.SetBool(_dash, false);
            _animator.SetBool(_run, _isRunning);
            _animator.SetBool(_idle, false);
        }
        else
        {
            _animator.SetBool(_dash, false);
            _animator.SetBool(_run, false);
            _animator.SetBool(_idle, true);
        }
    }
    #endregion

    #region Private Functions.
    /// <summary>
    /// Handles dash input.
    /// </summary>
    private void HandleDashInput()
    {
        if (!_isDashing && Input.GetKeyDown(dashKey) && currentStamina >= dashStaminaCost)
        {
            StartDash();
        }
    }

    /// <summary>
    /// Initiates the dash anim state.
    /// </summary>
    private void StartDash()
    {
        _isDashing = true;
        _dashTimer = dashDuration;

        CharacterController controller = GetComponent<CharacterController>();

        currentStamina -= dashStaminaCost;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

        _animator.SetBool(_dash, true);
        _animator.SetBool(_run, false);
        _animator.SetBool(_idle, false);

        Vector3 dashDirection = transform.forward;
        transform.position += dashDirection * dashDuration * 5f; 
    }

    /// <summary>
    /// Handles dash duration.
    /// </summary>
    private void HandleDashDuration()
    {
        if (_isDashing)
        {
            _dashTimer -= Time.deltaTime;
            if (_dashTimer <= 0f)
            {
                EndDash();
            }
        }
    }

    /// <summary>
    /// Ends the dash animation state.
    /// </summary>
    private void EndDash()
    {
        _isDashing = false;
        _animator.SetBool(_dash, false);
    }

    /// <summary>
    /// Handles stamina recovery over time.
    /// </summary>
    private void HandleStaminaRecovery()
    {
        if (!_isDashing && currentStamina < maxStamina)
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }
    }

    /// <summary>
    /// Updates the stamina UI if implemented.
    /// </summary>
    private void UpdateStaminaUI()
    {
        if (staminaBarSlider != null)
        {
            staminaBarSlider.value = currentStamina;
        }

        if (staminaFillImage != null)
        {
            staminaFillImage.color = currentStamina <= 20f ? Color.red : Color.green;
        }

    }

    /// <summary>
    /// Ensures stamina slider settings are as wanted.
    /// </summary>
    private void StaminaValuesInitialiser()
    {
        currentStamina = maxStamina;

        if (staminaBarSlider != null)
        {
            staminaBarSlider.maxValue = maxStamina;
            staminaBarSlider.value = currentStamina;
        }
    }
    #endregion

}
