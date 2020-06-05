using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class UI_PlayerProfileSelector : MonoBehaviour
{
    [Range(-1, 3)]
    public int playerID = -1;
    public ShipType selectedShip;
    public Image flagImage;
    public TMPro.TextMeshProUGUI shipName;

    public RectTransform inactiveSign; 
    public Image inactiveCover; 
    public RectTransform nameSign;
    public RectTransform readyBar;

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


    private void StateChangeEvents(PlayerState _state)
    {
        switch (playerState)
        {
            case PlayerState.Inactive:
                inactiveSign.LeanCancel();
                inactiveSign.LeanScaleY(1, 1f).setEase(LeanTweenType.easeOutBounce);

                nameSign.LeanCancel();
                nameSign.LeanScaleY(0, 0.2f).setEase(LeanTweenType.easeInOutCubic);

                readyBar.LeanCancel();
                readyBar.LeanSize(new Vector2(readyBar.rect.size.x, 100), 0.5f).setEase(LeanTweenType.easeOutCubic);

                inactiveCover.enabled = true;

                break;
            case PlayerState.Active:
                inactiveSign.LeanCancel();
                inactiveSign.LeanScaleY(0, 0.2f).setEase(LeanTweenType.easeInOutCubic);

                nameSign.LeanCancel();
                nameSign.LeanScaleY(1, 1f).setEase(LeanTweenType.easeOutBounce);

                readyBar.LeanCancel();
                readyBar.LeanSize(new Vector2(readyBar.rect.size.x, 0), 0.25f).setEase(LeanTweenType.easeInCubic);

                inactiveCover.enabled = false;

                break;
        }
    }


    private void Update()
    {
        if (selectedShip)
        {
            flagImage.sprite = selectedShip.flagSprite;
            shipName.text = selectedShip.characterName;
        }

        if (Application.isPlaying)
        {
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
