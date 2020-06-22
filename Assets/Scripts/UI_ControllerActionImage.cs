using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class UI_ControllerActionImage : MonoBehaviour
{

    public Image image;
    public string actionName;

    public ControllerIconMap controllerIconMap;

    public List<System.Guid> controllerGUIDs = new List<System.Guid>();

    Coroutine iconRunningCoroutine;

    private void Start()
    {
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerPreDisconnectEvent += OnControllerDisconnected;

        foreach(Controller c in ReInput.controllers.Controllers)
        {
            controllerGUIDs.Add(c.hardwareTypeGuid);
        }

        //iconRunningCoroutine = StartCoroutine(IconLoop());
    }


    private void Update()
    {
        //int elementID = -1;
        //elementID = ReInput.players.GetPlayer(1).controllers.maps.GetFirstElementMapWithAction(ControllerType.Joystick, actionName, true).elementIdentifierId;
        
        //if (elementID == -1) return;

        //foreach (ControllerIconMap.ControllerElement controllerElement in controllerIconMap.controllerElements)
        //{
        //    //if (p.controllers.joystickCount > 0)
        //    //{
        //    //    p.controllers.GetLastActiveController();
        //    //    Debug.Log(p.controllers.maps.GetFirstElementMapWithAction("MenuSelect", true).elementIdentifierId);
        //    //}


        //    if (controllerElement.elementIdentifierId == elementID)
        //        image.sprite = controllerElement.buttonSprite;
        //}


    }


    private IEnumerator IconLoop()
    {

        IList<Player> players = ReInput.players.GetPlayers();

        //// Loop through all GUIDs stored
        //foreach(System.Guid guid in controllerGUIDs)
        //{
        //    // Loop through all icon maps stored
        //    foreach (ControllerIconMap iconMap in GameManager.Instance.controllerIconMaps)
        //    {
        //        // IF the guid and icon map guid match...
        //        if (iconMap.controllerGUID == guid.ToString())
        //        {
        //            // Loop through every player in the game
        //            foreach(Player p in players)
        //            {
        //                // Loop through every joystick on every player
        //                foreach (Joystick j in p.controllers.Joysticks)
        //                {
        //                    // If the joystick guid is the same as the current checking guid
        //                    if (j.deviceInstanceGuid == guid)
        //                    {
        //                        // Loop through the controller elements in the icon map
        //                        foreach (ControllerIconMap.ControllerElement controllerElement in iconMap.controllerElements)
        //                        {
        //                            // If the current controller element id is the same as the first element map with the action name id
        //                            if (controllerElement.elementIdentifierId == p.controllers.maps.GetFirstElementMapWithAction(actionName, true).elementIdentifierId)
        //                            {
        //                                image.sprite = controllerElement.buttonSprite;
        //                                Debug.Log("Set Sprite");
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //// Loop through all GUIDs stored
        //foreach (System.Guid guid in controllerGUIDs.ToArray())
        //{
        //    // Loop through all icon maps stored
        //    foreach (ControllerIconMap iconMap in GameManager.Instance.controllerIconMaps)
        //    {
        //        // IF the guid and icon map guid match...
        //        if (iconMap.controllerGUID == guid.ToString())
        //        {
        //            // Loop through every player in the game
        //            foreach (Player p in players)
        //            {
        //                foreach (ControllerIconMap.ControllerElement controllerElement in iconMap.controllerElements)
        //                {
        //                    // If the current controller element id is the same as the first element map with the action name id
        //                    int elementID = -1;
        //                    ActionElementMap actionElementMap;
        //                    actionElementMap = p.controllers.maps.GetFirstElementMapWithAction(p.controllers.GetLastActiveController(), actionName, true);
        //                    if (actionElementMap == null) continue;
        //                    elementID = actionElementMap.elementIdentifierId;
        //                    if (elementID == -1) continue;
        //                    if (elementID == controllerElement.elementIdentifierId)
        //                    {
        //                        image.sprite = controllerElement.buttonSprite;
        //                        Debug.Log(iconMap.controllerName);
        //                        yield return new WaitForSeconds(1f);
        //                        break;
        //                    }
        //                }
        //            }
                    
        //        }
        //    }
        //}


        foreach(Player p in players)
        {
            if (p.controllers.joystickCount < 1 && !p.controllers.hasKeyboard && !p.controllers.hasMouse) continue;

            foreach(Controller c in p.controllers.Controllers)
            {
                //if (!controllerGUIDs.Contains(c.hardwareTypeGuid)) continue;

                //int indexOf = controllerGUIDs.IndexOf(c.hardwareTypeGuid);
                //System.Guid guidMatch = controllerGUIDs[indexOf];

                ControllerIconMap matchingMap = GameManager.Instance.FindIconMapByID(c.hardwareTypeGuid.ToString());

                if (matchingMap)
                {
                    ActionElementMap actionElementMap = p.controllers.maps.GetFirstElementMapWithAction(c, actionName, true);
                    if (actionElementMap == null) continue;
                    int elementID = actionElementMap.elementIdentifierId;

                    if (c.type == ControllerType.Keyboard) elementID = (int)actionElementMap.keyCode;

                    ControllerIconMap.ControllerElement controllerElement = matchingMap.FindElementByID(elementID);
                    if (controllerElement.Equals(default)) continue;

                    image.sprite = controllerElement.buttonSprite;
                }
            }

            yield return new WaitForSeconds(1f);
        }


        yield return null;

        iconRunningCoroutine = StartCoroutine(IconLoop());

    }


    private void OnEnable()
    {
        iconRunningCoroutine = StartCoroutine(IconLoop());
    }


    private void OnDisable()
    {
        if (iconRunningCoroutine != null)
            StopCoroutine(iconRunningCoroutine);
    }


    private void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        controllerGUIDs.Add(args.controller.hardwareTypeGuid);
    }


    private void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
    {
        if (controllerGUIDs.Contains(args.controller.hardwareTypeGuid))
            controllerGUIDs.Remove(args.controller.hardwareTypeGuid);
    }

}
