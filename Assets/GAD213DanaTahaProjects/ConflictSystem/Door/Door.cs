using TMPro;
using UnityEngine;

/// <summary>
/// A simple door opening and closing mechanic.
/// </summary>
public class Door : MonoBehaviour
{
    #region Variables
    [SerializeField] private Animator animator;
    [SerializeField] private SicknessRoom sicknessRoom;
    [SerializeField] private TreeCurer treeCurer;
    [SerializeField] private TMP_Text informativeText;

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
            if (!treeCurer.isCured && sicknessRoom.isPlayerInRoom)
            {
                informativeText.text = "Door is locked!";
                Invoke("EmptyInformativeText", 0.3f);
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
            Debug.Log("Door closed.");

            if (sicknessRoom.isPlayerInRoom)
            {
                _playerEntered = true;
            }
        }
    }

    private void EmptyInformativeText()
    {
        informativeText.text = string.Empty;
    }
}
