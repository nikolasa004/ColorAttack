using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlokIzborSkin : MonoBehaviour
{

    public int indeksSkina;

    public Animator bobAnimator;
    public List<RuntimeAnimatorController> animatoriSkinova = new List<RuntimeAnimatorController>();
    public RuntimeAnimatorController obicanBob;

    public void click()
    {
        if (indeksSkina != -1)
        {
            bobAnimator.runtimeAnimatorController = animatoriSkinova[indeksSkina];
        }
        else
        {
            bobAnimator.runtimeAnimatorController = obicanBob;
        } 

        PlayerPrefs.SetInt("TrenutniSkin", indeksSkina);
        House.zatvori=true;
    }
}
