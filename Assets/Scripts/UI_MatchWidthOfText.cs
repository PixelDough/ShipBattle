using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways()]
public class UI_MatchWidthOfText : MonoBehaviour
{

    public TMPro.TextMeshProUGUI textMeshProUGUI;

    private RectTransform rectTransform;


    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }


    private void Update()
    {
        if (textMeshProUGUI)
            rectTransform.rect.Set(rectTransform.rect.x, rectTransform.rect.y, textMeshProUGUI.GetRenderedValues(false).x, rectTransform.rect.height);
    }

}
