using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenTransition : MonoBehaviour
{

    public string targetSceneName = "";

    [SerializeField] private RawImage image;
    [SerializeField] private float transitionTime = 0.75f;


    private void Start()
    {
        image.color = new Color(1, 1, 1, 0);
        StartCoroutine(SceneTransition());
    }


    private IEnumerator SceneTransition()
    {

        if (Application.CanStreamedLevelBeLoaded(targetSceneName))
        {
            //Debug.Log("Level " + targetSceneName + " can be loaded.");

            LeanTween.alpha(image.rectTransform, 1f, transitionTime).setEaseOutCubic();
            while (image.color.a < 1f) yield return null;

            DontDestroyOnLoad(gameObject);
            AsyncOperation sync = SceneManager.LoadSceneAsync(targetSceneName, LoadSceneMode.Single);

            while (!sync.isDone) yield return null;

            LeanTween.alpha(image.rectTransform, 0f, transitionTime).setEaseInCubic();
            while (image.color.a > 0f) yield return null;
        }

        yield return null;
        Destroy(gameObject);
    }


}
