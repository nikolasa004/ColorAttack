using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Oblaci : MonoBehaviour
{
    public GameObject oblaci;
    public GameObject oblaci1;

    private Vector2 pos = new Vector2(0,0);
    private Vector2 pos1 = new Vector2(0, 0);

    private void Start()
    {
        pos=oblaci.transform.position;
        pos1= oblaci1.transform.position;
    }
    private void Update()
    {
        Kretanje();
    }

    public void Kretanje()
    {
        oblaci.transform.position=new Vector2(oblaci.transform.position.x+0.2f*Time.deltaTime,oblaci.transform.position.y);
        oblaci1.transform.position = new Vector2(oblaci1.transform.position.x + 0.2f*Time.deltaTime, oblaci1.transform.position.y);

        if(oblaci1.transform.position.x>pos.x)
        {
            oblaci.transform.position = pos1;

            GameObject pom = oblaci;
            oblaci=oblaci1;
            oblaci1= pom;
        }
    }
}
