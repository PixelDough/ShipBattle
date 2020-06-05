using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class GameManager : Singleton<GameManager>
{

    public PlayerController shipPrefab;
    public ShipType[] shipTypes;

    float spawnTime = 0f;
    int shipCount = 0;
    List<PlayerData> playersPlaying = new List<PlayerData>();
    

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        spawnTime = Time.time + 0.5f;

        foreach(Player p in ReInput.players.GetPlayers())
        {
            if (p.controllers.joystickCount > 0 || p.controllers.hasKeyboard) playersPlaying.Add(new PlayerData(p.id));
        }
    }

    private void Update()
    {

        if (SpawnPoint.spawnPoints.Count > 0 && shipCount < playersPlaying.Count)
        {
            if (Time.time >= spawnTime)
            {
                // Pick a random spawn point
                SpawnPoint point = SpawnPoint.spawnPoints[Random.Range(0, SpawnPoint.spawnPoints.Count)];

                // Remove spawn point so it isn't used again
                SpawnPoint.spawnPoints.Remove(point);

                // Spawn a player ship at spawn point
                PlayerController playerShip = Instantiate(shipPrefab, point.transform.position, point.transform.rotation);

                // Set ship variables
                // Set the player data
                playerShip.playerData = playersPlaying[shipCount];

                // Spawn poof particle
                Object poof = Resources.Load("Poof Particle");
                Instantiate(poof, point.transform.position, Quaternion.identity);

                // Increment the shipCount variable
                shipCount++;

                spawnTime = Time.time + 0.5f;
                
            }
        }
        
    }

    public class PlayerData
    {
        public int playerID;
        public int shipType;

        public PlayerData()
        {
            playerID = 0;
            shipType = 0;
        }

        public PlayerData(int _playerID)
        {
            playerID = _playerID;
            shipType = _playerID;
        }
    }

}
