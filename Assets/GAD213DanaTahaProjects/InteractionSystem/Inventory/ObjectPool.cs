using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 10;
    private Queue<GameObject> _objectPoolQuee = new Queue<GameObject>();
 
}
