using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SicknessRoom : MonoBehaviour
{
    #region Variables.

    public bool isInRoom = false;
    #endregion

    private void OnTriggerEnter()
    {
        isInRoom = true;
    }

    void Update()
    {

    }
}
