using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrabaAkvarijum : MonoBehaviour
{
    public float minX = -140f; // Minimalna x pozicija
    public float maxX = 120f;  // Maksimalna x pozicija
    public float speed = 1f;   // Brzina kretanja
    public float waitTimeMin = 0.5f;
    public float waitTimeMax = 2f;

    private RectTransform rectTransform;
    private Animator animator;
    private bool isWaiting = false;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        animator = GetComponent<Animator>();
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (true)
        {
            float targetX = Random.Range(minX, maxX);

            while (Mathf.Abs(rectTransform.anchoredPosition.x - targetX) > 0.1f)
            {
                if (!isWaiting)
                {
                    // Pomeranje objekta prema meti
                    rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, new Vector2(targetX, rectTransform.anchoredPosition.y), speed * Time.deltaTime);
                    // Setovanje boola na animatoru
                    animator.SetBool("Kretanje", true);
                }

                yield return null;
            }

            // Kada stignemo do mete, stani na neko vreme
            if (!isWaiting)
            {
                isWaiting = true;
                animator.SetBool("Kretanje", false);
                float waitTime = Random.Range(waitTimeMin, waitTimeMax);
                yield return new WaitForSeconds(waitTime);
                isWaiting = false;
            }
        }
    }
}
