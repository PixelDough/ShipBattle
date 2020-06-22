using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MainMenuController : MonoBehaviour
{

    [SerializeField] RectTransform screensParent;

    private UI_MenuScreen currentScreen;
    private Dictionary<UI_MenuScreen, Vector2> screenPositions = new Dictionary<UI_MenuScreen, Vector2>();

    private Rewired.InputManager rewiredInputManager;

    private bool changingScreens = false;

    private void Start()
    {
        currentScreen = screensParent.GetChild(0).GetComponent<UI_MenuScreen>();

        rewiredInputManager = FindObjectOfType<Rewired.InputManager>();

        for(int i = 0; i < screensParent.childCount; i++)
        {
            UI_MenuScreen menu = screensParent.GetChild(i).GetComponent<UI_MenuScreen>();

            if (menu)
                screenPositions.Add(menu, menu.GetComponent<RectTransform>().localPosition);
        }
    }


    public void ChangeScreen(UI_MenuScreen target)
    {
        if (!changingScreens)
            StartCoroutine(_ChangeScreen(target));
    }


    private IEnumerator _ChangeScreen(UI_MenuScreen target)
    {
        Vector2 v2;
        screenPositions.TryGetValue(target, out v2);

        if (v2 != null)
        {
            changingScreens = true;

            target.gameObject.SetActive(true);
            
            foreach(Rewired.Player p in Rewired.ReInput.players.Players)
            {
                p.controllers.maps.SetAllMapsEnabled(false);
            }

            LTDescr leanX = screensParent.LeanMoveLocalX(-v2.x, 0.5f).setEaseOutCubic();
            LTDescr leanY = screensParent.LeanMoveLocalY(-v2.y, 0.5f).setEaseOutCubic();

            while(LeanTween.isTweening(screensParent.gameObject))
            {
                yield return null;
            }

            currentScreen.gameObject.SetActive(false);
            currentScreen = target;

            foreach (Rewired.Player p in Rewired.ReInput.players.Players)
            {
                p.controllers.maps.SetAllMapsEnabled(true);
            }

            changingScreens = false;
        }

        changingScreens = false;

        yield return null;
    }


}
