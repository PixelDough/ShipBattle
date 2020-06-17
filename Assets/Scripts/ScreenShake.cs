using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{

    CinemachineImpulseSource impulseSource;

    Vector3 cameraStartPos;

    private void Start()
    {
        impulseSource = FindObjectOfType<CinemachineImpulseSource>();

        cameraStartPos = transform.position;
    }


    public void Shake()
    {
        Shake(2f);
    }

    public void Shake(float amplitude)
    {
        impulseSource.GenerateImpulse();
        //StartCoroutine(_Shake(amplitude));
    }


    private IEnumerator _Shake(float amplitude)
    {

        //transform.position = cameraStartPos;
        //cameraStartPos = transform.position;

        impulseSource.GenerateImpulse();

        yield return null;

        //transform.position = cameraStartPos;


    }
    

}
