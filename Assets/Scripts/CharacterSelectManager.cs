using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class CharacterSelectManager : MonoBehaviour
{

    public List<UI_PlayerProfileSelector> playerProfileSelectors = new List<UI_PlayerProfileSelector>();
    public RectTransform allReadyBar;
    public FMODUnity.StudioEventEmitter allReadySoundEmitter;

    public RectTransform optionsMenu;

    public UnityEvent returnEvent;

    private bool allPlayersReady = false;

    private Player p;

    private void Start()
    {

        Debug.Log(GameManager.Instance.name + " ensured.");
        //GameManager.Instance.AddPlayer(0);
        //GameManager.Instance.AddPlayer(1);

        //foreach (Player p in ReInput.players.GetPlayers())
        //{
        //    if (p.controllers.joystickCount > 0 || p.controllers.hasKeyboard) GameManager.Instance.playersPlaying.Add(new GameManager.PlayerData(p.id));
        //}

        GameManager.Instance.shipsOccupied.Clear();

        for(int i = 0; i < playerProfileSelectors.Count; i++)
        {
            playerProfileSelectors[i].localPlayerData.shipType = i;
        }

        foreach (GameManager.PlayerData p in GameManager.Instance.playersPlaying)
        {
            if (p == null) continue;
            playerProfileSelectors[p.playerSlot].controllerID = p.controllerID;
            playerProfileSelectors[p.playerSlot].localPlayerData = p;
            //playerProfileSelectors[p.playerSlot].localPlayerData.shipType = p.shipType;
            //playerProfileSelectors[p.playerSlot].UpdateData();
            GameManager.Instance.shipsOccupied.Add(p.shipType);
            playerProfileSelectors[p.playerSlot].playerState = UI_PlayerProfileSelector.PlayerState.Selected;
            
            
            
        }

        allReadyBar.localScale = Vector2.zero;

    }


    private void Update()
    {
        
        foreach(Player p in ReInput.players.GetPlayers())
        {

            if (p.GetButtonDown(RewiredConsts.Action.Start))
            {
                if (allPlayersReady)
                {
                    foreach (GameManager.PlayerData pd in GameManager.Instance.playersPlaying) if (pd != null) pd.score = 0;
                    GameManager.Instance.ChangeScenes("Game");
                }
            }

            // Player Joining
            if (p.GetButtonDown(RewiredConsts.Action.MenuSelect))
            {
                bool isAlreadyIn = false;
                UI_PlayerProfileSelector firstEmptySlot = default;
                foreach(UI_PlayerProfileSelector pps in playerProfileSelectors)
                {
                    if (pps.controllerID == p.id)
                    {
                        isAlreadyIn = true;
                        if (pps.playerState != UI_PlayerProfileSelector.PlayerState.Selected)
                        {
                            pps.playerState = UI_PlayerProfileSelector.PlayerState.Selected;
                            GameManager.Instance.shipsOccupied.Add(pps.localPlayerData.shipType);
                            CheckIfAllPlayersReady();
                        }
                        break;
                    }

                    if (pps.controllerID == -1 && firstEmptySlot == default)
                        firstEmptySlot = pps;
                }

                if (!isAlreadyIn)
                {
                    if (firstEmptySlot != default)
                    {
                        firstEmptySlot.controllerID = p.id;
                        //firstEmptySlot.playerData = new GameManager.PlayerData(p.id);
                        //GameManager.Instance.playersPlaying.Add(firstEmptySlot.playerData);

                        GameManager.Instance.AddPlayer(p.id, firstEmptySlot.localPlayerData.shipType);
                        firstEmptySlot.localPlayerData = GameManager.Instance.GetPlayer(p.id);
                    }
                }
            }

            // Player Quitting
            if (p.GetButtonDown(RewiredConsts.Action.MenuBack))
            {
                bool isInGame = false;
                foreach (UI_PlayerProfileSelector pps in playerProfileSelectors)
                {
                    if (pps.controllerID == p.id)
                    {
                        isInGame = true;
                        if (pps.playerState == UI_PlayerProfileSelector.PlayerState.Selected)
                        {
                            pps.playerState = UI_PlayerProfileSelector.PlayerState.Active;
                            GameManager.Instance.shipsOccupied.Remove(pps.localPlayerData.shipType);
                            CheckIfAllPlayersReady();
                            break;
                        }
                        else
                        {
                            pps.controllerID = -1;
                            //GameManager.Instance.playersPlaying.Remove(pps.playerData);
                            GameManager.Instance.RemovePlayer(p.id);
                            break;
                        }
                    }
                }

                if (!isInGame) returnEvent.Invoke();
            }
        }
    }


    public bool CheckIfAllPlayersReady()
    {
        bool _allPlayersReady = true;
        int playerReadyCount = 0;
        foreach (UI_PlayerProfileSelector pps in playerProfileSelectors)
        {
            if (pps.controllerID == -1) continue;
            if (pps.playerState != UI_PlayerProfileSelector.PlayerState.Selected)
            {
                _allPlayersReady = false;
                break;
            }
            else
            {
                playerReadyCount++;
            }
        }

        if (_allPlayersReady && playerReadyCount < 2)
            _allPlayersReady = false;

        if (allPlayersReady != _allPlayersReady)
        {
            switch(_allPlayersReady)
            {
                case true:
                    //allReadyBar.gameObject.SetActive(true);

                    //allReadyBar.transform.localScale = new Vector2(1.1f, 0.8f);

                    allReadyBar.LeanCancel();
                    allReadyBar.transform.LeanScaleX(1, .75f).setEaseOutElastic();
                    allReadyBar.transform.LeanScaleY(1, 1f).setEaseOutElastic();

                    allReadySoundEmitter.Play();

                    break;
                case false:
                    allReadyBar.LeanCancel();
                    allReadyBar.transform.LeanScaleX(0f, .1f);
                    allReadyBar.transform.LeanScaleY(0f, .1f);
                    //allReadyBar.gameObject.SetActive(false);
                    break;
            }
        }

        allPlayersReady = _allPlayersReady;
        return allPlayersReady;

    }


}
