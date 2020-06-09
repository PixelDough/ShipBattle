using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Spin : MonoBehaviour
{

    [SerializeField]
    private float spinSpeedPerSecond = 30;

    private void Update()
    {
        //transform.Rotate(transform.forward, spinSpeedPerSecond * Time.deltaTime, Space.Self);
        GetComponent<RectTransform>().Rotate(new Vector3(0f, 0f, spinSpeedPerSecond * Time.deltaTime));
    }

}
