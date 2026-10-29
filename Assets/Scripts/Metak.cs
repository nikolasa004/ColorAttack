using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metak : MonoBehaviour
{
    public static bool metak = false;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        metak = true;
    }
}
