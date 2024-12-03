using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dana.ConflictSystem
{
    /// <summary>
    /// Created this script so that the camera does not rotate but instead simply follows
    /// the player on the x and z axis for better minimap readability.
    /// </summary>

    public class MinimapCamera : MonoBehaviour
    {
        public Transform player; 

        private void LateUpdate()
        {
            if (player != null)
            {
                Vector3 newPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
                transform.position = newPosition;
            }
        }
    }

}