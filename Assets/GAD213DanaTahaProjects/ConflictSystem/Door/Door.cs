using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple door openning and closing mechanic.
/// </summary>

public class Door : MonoBehaviour
{
    #region
    [SerializeField] private Animator animator;
    [SerializeField] private PoisonRoom poisonRoom;

    private bool _playerEntered;
    #endregion

    private void OnTriggerEnter()
    {
        if (_playerEntered) { Debug.Log("Can't open");  return; }
        animator.SetBool("isDoorOpen", true);
    }

    private void OnTriggerExit()
    {
        animator.SetBool("isDoorOpen", false);
        if (poisonRoom.isInRoom == true)
        {
            _playerEntered = true;
        }
    }
}
