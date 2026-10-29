using UnityEngine;

[CreateAssetMenu(fileName = "Dialog", menuName = "Scriptable Objects/Dialog")]
public class Dialog : ScriptableObject
{
    public Character character;
    public string text;
    public bool onRightSide;
    public Dialog nextDialog;
    public bool hasEffect;
}
