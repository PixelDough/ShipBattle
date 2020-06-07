using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class TranslationManager : Singleton<TranslationManager>
{

    //TODO: load localized string
    [SerializeField]
    LocalizedString _localizedString;

    private string translatedString = "";

    void Awake()
    {
        _localizedString.TableEntryReference = "height_is_low";
        
    }

    public string TranslatedString(string _key)
    {
        Ref<string> final = new Ref<string>(_key);
        StartCoroutine(LoadLocalization(final));

        return final.Value;
    }

    private IEnumerator LoadLocalization(Ref<string> @ref)
    {
        yield return LocalizationSettings.InitializationOperation;
        UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<string> localizedString = _localizedString.GetLocalizedString();
        yield return localizedString;
        if (localizedString.IsDone)
        {
            Debug.Log(localizedString.Result);
            yield return localizedString.Result;
            @ref.Value = localizedString.Result;
        }
    }

    public class Ref<T>
    {
        private T backing;
        public T Value { get { return backing; } set { backing = value; } }
        public Ref(T reference)
        {
            backing = reference;
        }
    }

}
