using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FollowOtherUI : MonoBehaviour
{

    public RectTransform rectTransformToFollow;

    private RectTransform rectTransform;
    private Vector3 offset;


    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        offset = rectTransform.position - rectTransformToFollow.position;
    }


    private void LateUpdate()
    {
        rectTransform.position = rectTransformToFollow.position + offset;
    }

}
