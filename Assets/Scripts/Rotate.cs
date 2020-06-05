using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(Vector3.up, 5 * Time.deltaTime, Space.World);
    }
}
