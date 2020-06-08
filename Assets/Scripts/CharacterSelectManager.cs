using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;

public class CharacterSelectManager : MonoBehaviour
{

    public List<UI_PlayerProfileSelector> playerProfileSelectors = new List<UI_PlayerProfileSelector>();

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

        //foreach (GameManager.PlayerData p in GameManager.Instance.playersPlaying)
        //{
        //    playerProfileSelectors[p.playerID].playerID = p.playerID;
        //    //playerProfileSelectors[p.playerID].selectedShip = GameManager.Instance.shipTypes[p.shipType];
        //}
    }


    private void Update()
    {
        
        foreach(Player p in ReInput.players.GetPlayers())
        {
            // Player Joining
            if (p.GetButtonDown(RewiredConsts.Action.MenuSelect))
            {
                bool isAlreadyIn = false;
                UI_PlayerProfileSelector firstEmptySlot = default;
                foreach(UI_PlayerProfileSelector pps in playerProfileSelectors)
                {
                    if (pps.playerID == p.id)
                    {
                        isAlreadyIn = true;
                        if (pps.playerState != UI_PlayerProfileSelector.PlayerState.Selected) pps.playerState = UI_PlayerProfileSelector.PlayerState.Selected;
                        break;
                    }

                    if (pps.playerID == -1 && firstEmptySlot == default)
                        firstEmptySlot = pps;
                }

                if (!isAlreadyIn)
                {
                    if (firstEmptySlot != default)
                    {
                        firstEmptySlot.playerID = p.id;
                        //firstEmptySlot.playerData = new GameManager.PlayerData(p.id);
                        //GameManager.Instance.playersPlaying.Add(firstEmptySlot.playerData);

                        GameManager.Instance.AddPlayer(p.id, firstEmptySlot.selectedShip);
                        firstEmptySlot.playerData = GameManager.Instance.GetPlayer(p.id);
                    }
                }
            }

            if (p.GetButtonDown(RewiredConsts.Action.Start))
            {
                bool startBattle = true;
                foreach (UI_PlayerProfileSelector pps in playerProfileSelectors)
                {
                    if (pps.playerID == -1) continue;
                    if (pps.playerState != UI_PlayerProfileSelector.PlayerState.Selected)
                    {
                        startBattle = false;
                        break;
                    }
                }
                if (startBattle)
                {
                    SceneManager.LoadScene("Game");
                }
            }

            // Player Quitting
            if (p.GetButtonDown(RewiredConsts.Action.MenuBack))
            {
                foreach (UI_PlayerProfileSelector pps in playerProfileSelectors)
                {
                    if (pps.playerID == p.id)
                    {
                        if (pps.playerState == UI_PlayerProfileSelector.PlayerState.Selected)
                        {
                            pps.playerState = UI_PlayerProfileSelector.PlayerState.Active;
                            break;
                        }
                        else
                        {
                            pps.playerID = -1;
                            //GameManager.Instance.playersPlaying.Remove(pps.playerData);
                            GameManager.Instance.RemovePlayer(p.id);
                            break;
                        }
                    }
                }
            }
        }
    }

}
