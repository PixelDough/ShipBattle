using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letterbox : MonoBehaviour
{

    public RectTransform topBar;
    public RectTransform bottomBar;

    private void Start()
    {
        topBar.position += Vector3.up * topBar.rect.height;
        bottomBar.position += Vector3.down * bottomBar.rect.height;
        
        topBar.LeanMoveY(0, 2f).setEaseOutQuart();
        bottomBar.LeanMoveY(0, 2f).setEaseOutQuart();
    }

}
