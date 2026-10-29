using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kraj : MonoBehaviour
{
    public static bool kraj=false;
    public static bool uZoni=false;

    [HideInInspector]
    public static bool dno = false;

    public bool kvadrat=false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "kraj")
        {
            if (kvadrat)
            {
                kraj = true;
                dno = true;
            }
            else
            {
                dno = true;
            }
        }
        else
        {
            uZoni= true;
        }
    }
}
