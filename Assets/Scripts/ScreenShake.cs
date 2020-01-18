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
        Shake(0.2f);
    }

    public void Shake(float amplitude)
    {
        StartCoroutine(_Shake(amplitude));
    }


    private IEnumerator _Shake(float amplitude)
    {
        CinemachineVirtualCamera vc = brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();

        transform.position = cameraStartPos;
        cameraStartPos = transform.position;

        float duration = Time.time + 0.2f;

        while (Time.time < duration)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(cameraStartPos.x + Random.Range(-amplitude, amplitude), cameraStartPos.y + Random.Range(-amplitude, amplitude), cameraStartPos.z + Random.Range(-amplitude, amplitude)), 0.5f);
            yield return null;
        }

        transform.position = cameraStartPos;


    }
    

}
