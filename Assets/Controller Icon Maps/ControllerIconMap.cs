using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

[CreateAssetMenu()]
public class ControllerIconMap : SerializedScriptableObject
{

    public string controllerName;

    public string controllerGUID;

    //[DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine, KeyLabel = "Input ID", ValueLabel = "Sprite")]
    //public Dictionary<int, Sprite> buttonSprites;

    [ListDrawerSettings(AddCopiesLastElement = true)]
    [TableList(NumberOfItemsPerPage = 4, ShowPaging = true)]
    public List<ControllerElement> controllerElements;


    public ControllerElement FindElementByID(int elementID)
    {
        foreach(ControllerElement element in controllerElements)
        {
            if (elementID == element.elementIdentifierId) return element;
        }

        return default;
    }


    public struct ControllerElement
    {
        public int elementIdentifierId;

        [PreviewField]
        public Sprite buttonSprite;

        
    }

}
