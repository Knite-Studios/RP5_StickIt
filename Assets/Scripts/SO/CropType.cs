using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CropType", menuName = "Crop/Type")]
public class CropType : ScriptableObject
{
    public int healthPoints;
    public int scoreValue;
    public AudioClip damageSFX;
}
