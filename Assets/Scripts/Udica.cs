using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Udica : MonoBehaviour
{
    public static bool udica=false; 

    public void OnCollisionEnter2D(Collision2D collision)
    {
        udica = true;
    }
}
