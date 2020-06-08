using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public PlayerController shipPrefab;
    public ShipType[] shipTypes;

    float spawnTime = 0f;
    int shipCount = 0;
    public PlayerData[] playersPlaying = new PlayerData[4];
    public List<int> shipsOccupied = new List<int>();

    public static GameManager Instance;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }


    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        

        spawnTime = Time.time + 0.5f;

        //foreach (Player p in ReInput.players.GetPlayers())
        //{
        //    if (p.controllers.joystickCount > 0 || p.controllers.hasKeyboard) playersPlaying.Add(new PlayerData(p.id));
        //}
    }

    private void OnLevelWasLoaded(int level)
    {
        shipCount = 0;
        spawnTime = Time.time + 0.5f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // GAMEPLAY SPAWNING CODE
        List<PlayerData> finalPlayers = new List<PlayerData>();
        foreach(PlayerData pd in playersPlaying)
        {
            if (pd != null)
            {
                finalPlayers.Add(pd);
            }
        }
        if (SpawnPoint.spawnPoints.Count > 0 && shipCount < finalPlayers.Count)
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
                playerShip.playerData = finalPlayers[shipCount];

                // Spawn poof particle
                Object poof = Resources.Load("Poof Particle");
                Instantiate(poof, point.transform.position, Quaternion.identity);

                // Increment the shipCount variable
                shipCount++;

                spawnTime = Time.time + 0.5f;

            }
        }

    }

    public PlayerData AddPlayer(int _controllerID, int _shipType)
    {
        for (int i = 0; i < playersPlaying.Length; i++)
        {
            if (playersPlaying[i] != null) continue;

            PlayerData playerData = new PlayerData(_controllerID: _controllerID, _playerSlot: i, _shipType: _shipType);
            playersPlaying[i] = playerData;
            return playerData;

        }
        return null;
        //playersPlaying.Add(new PlayerData(_id));
    }

    public PlayerData GetPlayer(int _controllerID)
    {
        for (int i = 0; i < playersPlaying.Length; i++)
        {
            if (playersPlaying[i] == null) continue;
            if (playersPlaying[i].controllerID == _controllerID) return playersPlaying[i];

        }
        return null;
    }

    public void RemovePlayer(int _id)
    {
        for (int i = 0; i < playersPlaying.Length; i++)
        {
            if (playersPlaying[i] == null) continue;

            if (playersPlaying[i].controllerID == _id)
            {
                playersPlaying[i] = null;
                break;
            }
        }
    }

    public class PlayerData
    {
        public int controllerID;
        public int playerSlot;
        public int shipType;

        public PlayerData()
        {
            controllerID = 0;
            playerSlot = 0;
            shipType = 0;
        }

        public PlayerData(int _controllerID)
        {
            controllerID = _controllerID;
            playerSlot = _controllerID;
            shipType = _controllerID;
        }

        public PlayerData(int _controllerID, int _playerSlot)
        {
            controllerID = _controllerID;
            playerSlot = _playerSlot;
            shipType = _controllerID;
        }

        public PlayerData(int _controllerID, int _playerSlot, int _shipType)
        {
            controllerID = _controllerID;
            playerSlot = _playerSlot;
            shipType = _shipType;
        }
    }

}
