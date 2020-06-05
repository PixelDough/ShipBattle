using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ShipType : ScriptableObject
{

    [Header("Basics")]
    public string characterName = "NULL";
    public Color color = Color.white;
    
    [Header("Profile Selection")]
    public Sprite imageMain;
    public Sprite imageIcon;

    [Header("Flag")]
    public Sprite flagSprite;
    public Material flagMaterial;

}
