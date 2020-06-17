using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Score_ScoreBoard : MonoBehaviour
{

    [SerializeField]
    private CanvasGroup canvasGroup;

    public Transform playerBarContainer;
    public GameObject playerBarPrefab;

    private List<UI_Score_PlayerBar> playerBars = new List<UI_Score_PlayerBar>();

    private void Awake()
    {
        canvasGroup.alpha = 1;
        transform.localScale = Vector3.zero;

        foreach (GameManager.PlayerData pd in GameManager.Instance.playersPlaying)
        {
            if (pd != null)
            {
                UI_Score_PlayerBar bar = Instantiate(playerBarPrefab, playerBarContainer).GetComponent<UI_Score_PlayerBar>();
                bar.controllerID = pd.controllerID;
                bar.flagImage.sprite = GameManager.Instance.shipTypes[pd.shipType].flagSprite;
                playerBars.Add(bar);
            }
        }

        
        gameObject.LeanCancel();
        //canvasGroup.LeanAlpha(1, 0.5f);

        transform.LeanScaleX(1, .75f).setEaseOutElastic();
        transform.LeanScaleY(1, 1f).setEaseOutElastic();

        

    }


    public void UpdateScores(bool setInstantly = false)
    {
        foreach(UI_Score_PlayerBar pb in playerBars)
        {
            foreach(GameManager.PlayerData data in GameManager.Instance.playersPlaying)
            {
                if (pb.controllerID == data.controllerID)
                {
                    if (data.score <= 0) break;

                    if (setInstantly)
                        pb.SetFlagAtPoint(data.score);
                    else
                        pb.MoveFlagToPoint(data.score);

                    break;
                }
            }
        }
    }

}
