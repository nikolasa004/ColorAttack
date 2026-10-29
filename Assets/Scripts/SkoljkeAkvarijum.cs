using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkoljkeAkvarijum : MonoBehaviour
{
    private Animator animator;
    private ParticleSystem mehuri;
    public float waitTimeMin = 0.5f;
    public float waitTimeMax = 10f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        mehuri = GetComponentInChildren<ParticleSystem>();
        StartCoroutine(OpenClose());
    }

    public IEnumerator OpenClose()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(waitTimeMin, waitTimeMax));
            animator.SetBool("zatvori",true);
            mehuri.Play();
            yield return new WaitForSeconds(Random.Range(waitTimeMin, waitTimeMax));
            animator.SetBool("zatvori", false);
        }
    }
}
