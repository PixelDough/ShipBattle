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
    public int controllerID = -1;
    //public int selectedShip;
    public Image flagImage;
    public TMPro.TextMeshProUGUI shipName;
    public MeshRenderer shipBodyRenderer;
    public RectTransform tinyShipImage;

    public RectTransform inactiveSign; 
    public Image inactiveCover; 
    public RectTransform nameSignSwingJoint;
    public RectTransform nameSignImage;
    public RectTransform readyBar;

    public GameManager.PlayerData localPlayerData = new GameManager.PlayerData(-1, -1, 0);

    [Header("FMOD")]
    public FMODUnity.StudioEventEmitter woodSwingEmitter;

    private Player p;
    private CharacterSelectManager characterSelectManager;

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
        tinyShipImage.LeanScaleX(0, 0f);
        characterSelectManager = FindObjectOfType<CharacterSelectManager>();
        characterSelectManager.CheckIfAllPlayersReady();
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

                tinyShipImage.LeanCancel();
                tinyShipImage.LeanScaleX(0.0f, 0.1f);

                inactiveCover.enabled = true;
                break;
            case PlayerState.Active:
                inactiveSign.LeanCancel();
                inactiveSign.LeanScaleY(0, 0.2f).setEase(LeanTweenType.easeInOutCubic);

                nameSignImage.LeanCancel();
                nameSignImage.LeanScaleY(1, 1f).setEase(LeanTweenType.easeOutBounce);

                readyBar.LeanCancel();
                readyBar.LeanSize(new Vector2(readyBar.rect.size.x, 0), 0.25f).setEase(LeanTweenType.easeOutCubic);

                tinyShipImage.LeanCancel();
                tinyShipImage.LeanScale(new Vector3(1f, 1f, 1f), 0.1f);

                inactiveCover.enabled = false;
                break;
            case PlayerState.Selected:

                inactiveSign.LeanCancel();
                inactiveSign.LeanScaleY(0, 0.2f).setEase(LeanTweenType.easeInOutCubic);

                nameSignImage.LeanCancel();
                nameSignImage.LeanScaleY(1, 1f).setEase(LeanTweenType.easeOutBounce);

                readyBar.LeanCancel();
                readyBar.LeanSize(new Vector2(readyBar.rect.size.x, 100), 0.25f).setEase(LeanTweenType.easeInCubic);

                tinyShipImage.LeanCancel();
                tinyShipImage.LeanScale(new Vector3(1f, 1f, 1f), 0.1f);

                inactiveCover.enabled = false;
                break;
        }
    }


    public void UpdateData()
    {
        StartCoroutine(_UpdateData());
    }

    private IEnumerator _UpdateData()
    {
        if (Application.isPlaying) {

            shipBodyRenderer.material.SetColor("_BaseColor", GameManager.Instance.shipTypes[localPlayerData.shipType].color);

            if (playerState != PlayerState.Selected)
            {
                int shipCheck = localPlayerData.shipType;
                for (int i = 0; i < GameManager.Instance.shipTypes.Length; i++)
                {
                    if (GameManager.Instance.shipsOccupied.Contains(shipCheck))
                    {
                        shipCheck = (int)Mathf.Repeat(shipCheck + 1, GameManager.Instance.shipTypes.Length);
                    }
                }
                localPlayerData.shipType = shipCheck;
            }


            flagImage.sprite = GameManager.Instance.shipTypes[localPlayerData.shipType].flagSprite;
            //shipName.text = TranslationManager.Instance.TranslatedString(GameManager.Instance.shipTypes[selectedShip].characterName);

            yield return LocalizationSettings.InitializationOperation;
            var async = GameManager.Instance.shipTypes[localPlayerData.shipType].localName.GetLocalizedString();
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

            if (p == null || (p.id != controllerID))
            {
                if (controllerID >= 0 && controllerID < 4)
                    p = ReInput.players.GetPlayer(controllerID);
            }
            else
            {
                if (playerState == PlayerState.Active)
                {
                    int inputDirection = p.GetButtonDown(RewiredConsts.Action.MenuHorizontal) || p.GetNegativeButtonDown(RewiredConsts.Action.MenuHorizontal) ? Mathf.RoundToInt(p.GetAxis(RewiredConsts.Action.MenuHorizontal)) : 0;

                    if (inputDirection != 0)
                    {
                        if (true || localPlayerData.shipType != Mathf.Clamp(localPlayerData.shipType + inputDirection, 0, GameManager.Instance.shipTypes.Length - 1))
                        {
                            //nameSignSwingJoint.Rotate(Vector3.forward, 5 * inputDirection);
                            woodSwingEmitter.Play();
                            nameSignSwingJoint.LeanCancel();
                            nameSignSwingJoint.LeanRotateZ(10 * inputDirection, 0.1f).setEaseOutCubic();
                            nameSignSwingJoint.LeanRotateZ(0, 2f).setEaseOutElastic().setDelay(0.1f);
                        }

                        int shipCheck = localPlayerData.shipType;
                        for (int i = 0; i < GameManager.Instance.shipTypes.Length; i++)
                        {
                            shipCheck = (int)Mathf.Repeat(shipCheck + inputDirection, GameManager.Instance.shipTypes.Length);
                            if (!GameManager.Instance.shipsOccupied.Contains(shipCheck))
                            {
                                localPlayerData.shipType = shipCheck;
                                break;
                            }
                        }
                        localPlayerData.shipType = shipCheck;
                        GameManager.Instance.GetPlayer(controllerID).shipType = localPlayerData.shipType;
                        UpdateData();
                    }
                }
            }

            if (controllerID >= 0)
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
