using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ScenePack : ScriptableObject
{

    public UnityEditor.SceneAsset scene;

    [FMODUnity.EventRef]
    public string mainMusicEventRef;

}
