using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_MenuScreen : MonoBehaviour
{

    public Button buttonStartActive;

    private void OnEnable()
    {
        //if (buttonStartActive && EventSystem.current)
        //    EventSystem.current.SetSelectedGameObject(buttonStartActive.gameObject);

        //if (buttonStartActive) 
        //    buttonStartActive.Select();
    }

}
