using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization.Settings;


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
    public RectTransform nameSignSwingJoint;
    public RectTransform nameSignImage;
    public RectTransform readyBar;

    public GameManager.PlayerData playerData;

    private Player p;

    private LTDescr signScaleTween;
    

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
        nameSignImage.LeanScaleY(0, 0f);
        UpdateData();
    }


    private void StateChangeEvents(PlayerState _state)
    {
        switch (playerState)
        {
            case PlayerState.Inactive:
                inactiveSign.LeanCancel();
                inactiveSign.LeanScaleY(1, 1f).setEase(LeanTweenType.easeOutBounce);

                nameSignImage.LeanCancel();
                nameSignImage.LeanScaleY(0, 0.2f).setEase(LeanTweenType.easeInOutCubic);

                inactiveCover.enabled = true;
                break;
            case PlayerState.Active:
                inactiveSign.LeanCancel();
                inactiveSign.LeanScaleY(0, 0.2f).setEase(LeanTweenType.easeInOutCubic);

                nameSignImage.LeanCancel();
                nameSignImage.LeanScaleY(1, 1f).setEase(LeanTweenType.easeOutBounce);

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


    private void UpdateData()
    {
        StartCoroutine(_UpdateData());
    }

    private IEnumerator _UpdateData()
    {
        if (Application.isPlaying) {

            if (playerState != PlayerState.Selected)
            {
                int shipCheck = selectedShip;
                for (int i = 0; i < GameManager.Instance.shipTypes.Length; i++)
                {
                    if (GameManager.Instance.shipsOccupied.Contains(shipCheck))
                    {
                        shipCheck = (int)Mathf.Repeat(shipCheck + 1, GameManager.Instance.shipTypes.Length);
                    }
                }
                selectedShip = shipCheck;
            }


            flagImage.sprite = GameManager.Instance.shipTypes[selectedShip].flagSprite;
            //shipName.text = TranslationManager.Instance.TranslatedString(GameManager.Instance.shipTypes[selectedShip].characterName);

            yield return LocalizationSettings.InitializationOperation;
            var async = GameManager.Instance.shipTypes[selectedShip].localName.GetLocalizedString();
            while (!async.IsDone) { yield return null; }
            shipName.text = async.Result;
        }
        yield return null;
    }


    private void Update()
    {

        if (Application.isPlaying)
        {

            UpdateData();

            //shipName.transform.rotation = Quaternion.Lerp(shipName.transform.rotation, Quaternion.identity, 10 * Time.deltaTime);

            if (p == null || (p.id != playerID))
            {
                if (playerID >= 0 && playerID < 4)
                    p = ReInput.players.GetPlayer(playerID);
            }
            else
            {
                if (playerState == PlayerState.Active)
                {
                    int inputDirection = p.GetButtonDown(RewiredConsts.Action.MenuHorizontal) ? Mathf.RoundToInt(p.GetAxis(RewiredConsts.Action.MenuHorizontal)) : 0;

                    if (inputDirection != 0)
                    {
                        if (true || selectedShip != Mathf.Clamp(selectedShip + inputDirection, 0, GameManager.Instance.shipTypes.Length - 1))
                        {
                            //nameSignSwingJoint.Rotate(Vector3.forward, 5 * inputDirection);
                            nameSignSwingJoint.LeanCancel();
                            nameSignSwingJoint.LeanRotateZ(10 * inputDirection, 0.1f).setEaseOutCubic();
                            nameSignSwingJoint.LeanRotateZ(0, 2f).setEaseOutElastic().setDelay(0.1f);
                        }

                        int shipCheck = selectedShip;
                        for (int i = 0; i < GameManager.Instance.shipTypes.Length; i++)
                        {
                            shipCheck = (int)Mathf.Repeat(shipCheck + inputDirection, GameManager.Instance.shipTypes.Length);
                            if (!GameManager.Instance.shipsOccupied.Contains(shipCheck))
                            {
                                selectedShip = shipCheck;
                                break;
                            }
                        }
                        selectedShip = shipCheck;
                        GameManager.Instance.GetPlayer(playerID).shipType = selectedShip;
                        UpdateData();
                    }
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
