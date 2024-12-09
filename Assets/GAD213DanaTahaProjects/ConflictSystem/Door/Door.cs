using TMPro;
using UnityEngine;

/// <summary>
/// A simple door opening and closing mechanic.
/// </summary>
public class Door : MonoBehaviour
{
    #region Variables
    [SerializeField] private Animator animator;
    [SerializeField] private TMP_Text informativeText;
    [SerializeField] private SicknessBar sicknessBar; 

    private bool _playerEntered;
    #endregion

    private void Start()
    {
        informativeText.text = string.Empty;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (sicknessBar != null && sicknessBar.IsSicknessBarActive())
            {
                informativeText.text = "The door is locked, Cure the tree.";
                Invoke("EmptyInformativeText", 3f);
                return;
            }

            animator.SetBool("isDoorOpen", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("isDoorOpen", false);
        }
    }

    private void EmptyInformativeText()
    {
        informativeText.text = string.Empty;
    }

    /// <summary>
    /// Resets the door to its default state.
    /// </summary>
    public void ResetDoor()
    {
        animator.SetBool("isDoorOpen", false);
        _playerEntered = false;
        informativeText.text = string.Empty;
    }

    /// <summary>
    /// Forces the door to open programmatically.
    /// </summary>
    public void ForceOpen()
    {
        animator.SetBool("isDoorOpen", true);
    }
}
