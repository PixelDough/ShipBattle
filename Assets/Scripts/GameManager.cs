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
    public int playerCount = 0;
    public List<int> shipsOccupied = new List<int>();

    public bool debugOpen = false;

    [Space]
    public ControllerIconMap[] controllerIconMaps;

    [Header("Scene Changing")]
    public ScreenTransition screenTransition;

    [Header("FMOD")]
    public FMODUnity.StudioEventEmitter musicEventEmitter;

    [Header("Game Rules")]
    [Range(1, 10)]
    public int pointsToWin = 5;

    public GameRules defaultGameRules = new GameRules();

    public static GameManager Instance;


    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            musicEventEmitter.Play();
        }
        else
        {
            
            if (Instance.musicEventEmitter.Event != musicEventEmitter.Event)
            {
                Instance.musicEventEmitter.Stop();

                DestroyImmediate(Instance.musicEventEmitter.gameObject);

                musicEventEmitter.transform.SetParent(Instance.transform);

                Instance.musicEventEmitter = musicEventEmitter;

                Instance.musicEventEmitter.Play();
            }

            Destroy(this.gameObject);
        }
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
            ChangeScenes(SceneManager.GetActiveScene().name, false);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name != "Player Select") 
                ChangeScenes("Player Select");
        }
        // Debug menu toggle
        if (Input.GetKey(KeyCode.P) && Input.GetKey(KeyCode.D))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                debugOpen = !debugOpen;
            if (Input.GetKeyDown(KeyCode.Alpha7))
                AddPlayer(ReInput.players.SystemPlayer.id, 5);
            if (Input.GetKeyDown(KeyCode.Alpha6))
                RemovePlayer(ReInput.players.SystemPlayer.id);
        }
        

    }


    public PlayerData AddPlayer(int _controllerID, int _shipType)
    {
        for (int i = 0; i < playersPlaying.Length; i++)
        {
            if (playersPlaying[i] != null) continue;

            PlayerData playerData = new PlayerData(_controllerID: _controllerID, _playerSlot: i, _shipType: _shipType);
            playersPlaying[i] = playerData;
            playerCount++;
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

    public void RemovePlayer(int _controllerID)
    {
        for (int i = 0; i < playersPlaying.Length; i++)
        {
            if (playersPlaying[i] == null) continue;

            if (playersPlaying[i].controllerID == _controllerID)
            {
                playersPlaying[i] = null;
                playerCount--;
                break;
            }
        }
    }

    public void ChangeScenes(string sceneName, bool fadeMusic = true)
    {
        if (FindObjectsOfType<ScreenTransition>().Length < 1)
        {
            ScreenTransition st = Instantiate(screenTransition);
            st.targetSceneName = sceneName;
            st.fadeMusic = fadeMusic;
        }
    }

    public void ChangeScenes(Object sceneAsset, bool fadeMusic = true)
    {
        string sceneName = sceneAsset.name;

        ChangeScenes(sceneName, fadeMusic);
    }


    public ControllerIconMap FindIconMapByID(string hardwareID)
    {
        foreach(ControllerIconMap map in controllerIconMaps)
        {
            if (map.controllerGUID == hardwareID) return map;
        }

        return null;
    }


    public class PlayerData
    {
        public int controllerID;
        public int playerSlot;
        public int shipType;
        public int score = 0;

        public PlayerData()
        {
            controllerID = 0;
            playerSlot = 0;
            shipType = 0;
            score = 0;
        }

        public PlayerData(int _controllerID)
        {
            controllerID = _controllerID;
            playerSlot = _controllerID;
            shipType = _controllerID;
            score = 0;
        }

        public PlayerData(int _controllerID, int _playerSlot)
        {
            controllerID = _controllerID;
            playerSlot = _playerSlot;
            shipType = _controllerID;
            score = 0;
        }

        public PlayerData(int _controllerID, int _playerSlot, int _shipType)
        {
            controllerID = _controllerID;
            playerSlot = _playerSlot;
            shipType = _shipType;
            score = 0;
        }
    }

    public class GameRules
    {
        [Range(1, 10)]
        public int pointsToWin = 2;

        public GameRules()
        {
            
        }
    }

}
