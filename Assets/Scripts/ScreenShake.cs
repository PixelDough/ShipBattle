using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{

    CinemachineBrain brain;

    Vector3 cameraStartPos;

    private void Start()
    {
        brain = FindObjectOfType<CinemachineBrain>();

        cameraStartPos = transform.position;
    }


    public void Shake()
    {
        Shake(2f);
    }

    public void Shake(float amplitude)
    {
        StartCoroutine(_Shake(amplitude));
    }


    private IEnumerator _Shake(float amplitude)
    {
        CinemachineVirtualCamera vc = brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        CinemachineBasicMultiChannelPerlin vcp = vc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        //transform.position = cameraStartPos;
        //cameraStartPos = transform.position;

        vcp.m_AmplitudeGain = amplitude;

        yield return new WaitForSeconds(0.2f);

        vcp.m_AmplitudeGain = 0f;

        //transform.position = cameraStartPos;


    }
    

}
