using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirLevoDesno : MonoBehaviour
{
    void Update()
    {
        StartCoroutine(Kraj());
    }
    public IEnumerator Kraj()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
