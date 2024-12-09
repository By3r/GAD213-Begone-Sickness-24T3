using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{
    #region Variables
    private Transform _playerPositionBeforeEntering;
    #endregion

    private void Start()
    {
        _playerPositionBeforeEntering = transform;
    }
}
