using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable Objects/Character")]
public class Character : ScriptableObject
{
    public string characterName;
    public Material nameMaterial;
    public Sprite normalFace;
    public Sprite openMouthFace;
    public Sprite closedEyesFace;
    public AudioClip sound;
}
