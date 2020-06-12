using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public PlayerController shipPrefab;
    public ShipType[] shipTypes;

    
    public PlayerData[] playersPlaying = new PlayerData[4];
    public List<int> shipsOccupied = new List<int>();

    public bool debugOpen = false;

    [Header("Scene Changing")]
    public ScreenTransition screenTransition;

    public static GameManager Instance;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }


    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);


        //if (shipsOccupied.Count <= 0)
        //{
        //    foreach (Player p in ReInput.players.GetPlayers())
        //    {
        //        if (p.controllers.joystickCount > 0 || p.controllers.hasKeyboard) AddPlayer(p.id, p.id);
        //    }
        //}
    }

    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ChangeScenes(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name != "Player Select") 
                ChangeScenes("Player Select");
        }
        // Debug menu toggle
        if (Input.GetKey(KeyCode.P) && Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.Alpha1))
        {
            debugOpen = !debugOpen;
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

    public void ChangeScenes(string sceneName)
    {
        if (FindObjectsOfType<ScreenTransition>().Length < 1)
            Instantiate(screenTransition).targetSceneName = sceneName;
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
