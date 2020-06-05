using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

[ExecuteAlways]
public class UI_PlayerProfileSelector : MonoBehaviour
{
    [Range(-1, 3)]
    public int playerID = -1;
    public int selectedShip;
    public Image flagImage;
    public TMPro.TextMeshProUGUI shipName;

    public RectTransform inactiveSign; 
    public Image inactiveCover; 
    public RectTransform nameSign;
    public RectTransform readyBar;

    private Player p;

    public enum PlayerState
    {
        Inactive,
        Active,
        Selected
    }
    private PlayerState _playerState = PlayerState.Inactive;
    public PlayerState playerState {
        get { return _playerState; }
        set {
            _playerState = value;
            StateChangeEvents(_playerState); 
        }
    }


    private void Start()
    {
        nameSign.LeanScaleY(0, 0f);
        
    }


    private void StateChangeEvents(PlayerState _state)
    {
        switch (playerState)
        {
            case PlayerState.Inactive:
                inactiveSign.LeanCancel();
                inactiveSign.LeanScaleY(1, 1f).setEase(LeanTweenType.easeOutBounce);

                nameSign.LeanCancel();
                nameSign.LeanScaleY(0, 0.2f).setEase(LeanTweenType.easeInOutCubic);

                inactiveCover.enabled = true;
                break;
            case PlayerState.Active:
                inactiveSign.LeanCancel();
                inactiveSign.LeanScaleY(0, 0.2f).setEase(LeanTweenType.easeInOutCubic);

                nameSign.LeanCancel();
                nameSign.LeanScaleY(1, 1f).setEase(LeanTweenType.easeOutBounce);

                readyBar.LeanCancel();
                readyBar.LeanSize(new Vector2(readyBar.rect.size.x, 0), 0.25f).setEase(LeanTweenType.easeOutCubic);

                inactiveCover.enabled = false;
                break;
            case PlayerState.Selected:

                readyBar.LeanCancel();
                readyBar.LeanSize(new Vector2(readyBar.rect.size.x, 100), 0.25f).setEase(LeanTweenType.easeInCubic);

                break;
        }
    }


    private void Update()
    {

        
        

        if (Application.isPlaying)
        {

            flagImage.sprite = GameManager.Instance.shipTypes[selectedShip].flagSprite;
            shipName.text = GameManager.Instance.shipTypes[selectedShip].characterName;


            if (p == null)
            {
                if (playerID >= 0 && playerID < 4)
                    p = ReInput.players.GetPlayer(playerID);
            }
            else
            {
                if (p.id != playerID)
                    p = ReInput.players.GetPlayer(playerID);
                if (playerState == PlayerState.Active)
                {
                    int inputDirection = p.GetButtonDown(RewiredConsts.Action.MenuHorizontal) ? Mathf.RoundToInt(p.GetAxis(RewiredConsts.Action.MenuHorizontal)) : 0;
                    selectedShip = Mathf.Clamp(selectedShip + inputDirection, 0, 3);
                }
            }

            if (playerID >= 0)
            {
                if (playerState == PlayerState.Inactive) playerState = PlayerState.Active;
            }
            else
            {
                if (playerState != PlayerState.Inactive) playerState = PlayerState.Inactive;
            }

            
        }
    }

}
