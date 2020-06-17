using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Score_PlayerBar : MonoBehaviour
{

    public Image flagImage;
    public RectTransform ringImage;
    public HorizontalLayoutGroup tickGroup;
    public GameObject tickPrefab;

    [HideInInspector]
    public int controllerID = -1;

    private bool initialized = false;

    private void Start()
    {
        for(int i = 0; i < GameManager.Instance.defaultGameRules.pointsToWin; i ++)
        {
            Instantiate(tickPrefab, tickGroup.transform).transform.SetAsFirstSibling();
        }


        
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    SetFlagAtPoint(0);
        //}

        if (!initialized && controllerID != -1)
        {
            SetFlagAtPoint(GameManager.Instance.GetPlayer(controllerID).score);
            initialized = true;
        }
    }

    public void MoveFlagToPoint(int point)
    {
        point--;

        if (point == -1) return;

        point = Mathf.Clamp(point, 0, GameManager.Instance.defaultGameRules.pointsToWin - 1);

        RectTransform tick = tickGroup.transform.GetChild(point) as RectTransform;

        ringImage.transform.LeanMoveX(tick.position.x, 0.5f).setEaseInOutCubic();
    }

    public void SetFlagAtPoint(int point)
    {

        point--;

        if (point == -1) return;

        point = Mathf.Clamp(point, 0, GameManager.Instance.defaultGameRules.pointsToWin - 1);

        RectTransform tick = tickGroup.transform.GetChild(point) as RectTransform;

        ringImage.position = tick.position;
    }

}
