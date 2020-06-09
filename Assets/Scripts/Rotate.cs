using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

    public Space space = Space.World;
    public float speed = 5f;

    private void Update()
    {
        transform.Rotate(Vector3.up, speed * Time.deltaTime, space);
    }
}
