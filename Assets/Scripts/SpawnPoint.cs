using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{

    public static List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

    private void Awake()
    {
        if (spawnPoints.Count > 0)
            spawnPoints.Clear();
    }
    
    private void OnLevelWasLoaded(int level)
    {
        if (spawnPoints.Count <= 0)
        {
            spawnPoints = new List<SpawnPoint>();
        }

        spawnPoints.Add(this);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 1f);

        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2f);
    }

}
