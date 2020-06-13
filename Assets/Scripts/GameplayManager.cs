﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameplayManager : MonoBehaviour
{

    public CinemachineVirtualCamera zoomCamera;

    float spawnTime = 0f;
    int shipCountAlive = 0;
    int numShipsToSpawn = 0;
    int numShipsSpawned = 0;

    public static GameplayManager Instance;

    private void OnLevelWasLoaded(int level)
    {
        foreach (GameManager.PlayerData pd in GameManager.Instance.playersPlaying)
        {
            if (pd != null)
            {
                numShipsToSpawn++;
            }
        }

        shipCountAlive = 0;
        spawnTime = Time.time + 0.5f;
    }


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }


    private void Start()
    {
        spawnTime = Time.time + 0.5f;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            if (shipCountAlive <= 0)
            {
                foreach (Player p in ReInput.players.GetPlayers())
                {
                    if (p.controllers.joystickCount > 0 || p.controllers.hasKeyboard) GameManager.Instance.AddPlayer(p.id, p.id);
                }
            }
        }

        // GAMEPLAY SPAWNING CODE
        List<GameManager.PlayerData> finalPlayers = new List<GameManager.PlayerData>();
        foreach (GameManager.PlayerData pd in GameManager.Instance.playersPlaying)
        {
            if (pd != null)
            {
                finalPlayers.Add(pd);
            }
        }
        if (SpawnPoint.spawnPoints.Count > 0 && numShipsSpawned < numShipsToSpawn)
        {

            if (Time.time >= spawnTime)
            {
                // Pick a random spawn point
                SpawnPoint point = SpawnPoint.spawnPoints[Random.Range(0, SpawnPoint.spawnPoints.Count)];

                // Remove spawn point so it isn't used again
                SpawnPoint.spawnPoints.Remove(point);

                // Spawn a player ship at spawn point
                if (point != null)
                {
                    PlayerController playerShip = Instantiate(GameManager.Instance.shipPrefab, point.transform.position, point.transform.rotation);

                    // Set ship variables
                    // Set the player data
                    playerShip.playerData = finalPlayers[shipCountAlive];

                    // Spawn poof particle
                    Object poof = Resources.Load("Poof Particle");
                    Instantiate(poof, point.transform.position, Quaternion.identity);

                    // Increment the shipCount variable
                    shipCountAlive++;
                    numShipsSpawned++;

                    spawnTime = Time.time + 0.5f;
                }
            }
        }
    }


    public void ShipDie()
    {
        StartCoroutine(_ShipDie());
    }


    private IEnumerator _ShipDie()
    {
        yield return new WaitForEndOfFrame();

        shipCountAlive--;

        if (shipCountAlive == 1)
        {
            Transform t = FindObjectOfType<PlayerController>().transform;
            if (t)
            {
                zoomCamera.Follow = t;
                zoomCamera.Priority = 100;
            }
            Debug.Log("A player has won!");

            yield return new WaitForSeconds(5f);

            GameManager.Instance.ChangeScenes(SceneManager.GetActiveScene().name);
        }
    }


}
