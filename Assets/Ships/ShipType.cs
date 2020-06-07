using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu()]
public class ShipType : ScriptableObject
{

    [Header("Basics")]
    public string characterName = "NULL";
    public LocalizedString localName;
    public Color color = Color.white;
    
    [Header("Profile Selection")]
    public Sprite imageMain;
    public Sprite imageIcon;

    [Header("Flag")]
    public Sprite flagSprite;
    public Material flagMaterial;

}
