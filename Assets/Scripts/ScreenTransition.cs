using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenTransition : MonoBehaviour
{

    public string targetSceneName = "";
    public bool fadeMusic = true;

    [SerializeField] private RawImage image;
    [SerializeField] private float transitionTime = 0.75f;
    [SerializeField] private FMODUnity.StudioGlobalParameterTrigger fadeOutTrigger;
    [SerializeField] private FMODUnity.StudioGlobalParameterTrigger fadeInTrigger;


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

            if (fadeMusic) fadeOutTrigger.TriggerParameters();
            LeanTween.alpha(image.rectTransform, 1f, transitionTime).setEaseOutCubic();
            while (image.color.a < 1f) yield return null;

            DontDestroyOnLoad(gameObject);
            AsyncOperation sync = SceneManager.LoadSceneAsync(targetSceneName, LoadSceneMode.Single);

            while (!sync.isDone) yield return null;

            if (fadeMusic) fadeInTrigger.TriggerParameters();
            LeanTween.alpha(image.rectTransform, 0f, transitionTime).setEaseInCubic();
            while (image.color.a > 0f) yield return null;
        }

        yield return null;
        Destroy(gameObject);
    }


}
