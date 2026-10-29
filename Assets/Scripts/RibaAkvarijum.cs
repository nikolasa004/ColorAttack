using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RibaAkvarijum : MonoBehaviour
{
    public float minX = -160f; // Minimalna x pozicija
    public float maxX = 150f;  // Maksimalna x pozicija
    public float minY = -40f;  // Minimalna y pozicija
    public float maxY = 130f;   // Maksimalna y pozicija
    public float minSpeed = 25f;   // Brzina kretanja
    public float maxSpeed = 150f;   // Brzina kretanja
    public float waitTimeMin = 0.5f;
    public float waitTimeMax = 5f;

    private RectTransform rectTransform;
    private bool isWaiting = false;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (true)
        {
            float targetX = Random.Range(minX, maxX);
            float targetY = Random.Range(minY, maxY);
            float speed = Random.Range(minSpeed, maxSpeed);

            if(targetX>rectTransform.anchoredPosition.x)
            {
                rectTransform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                rectTransform.localScale = new Vector3(1, 1, 1);
            }

            while (Mathf.Abs(rectTransform.anchoredPosition.x - targetX) > 0.1f && Mathf.Abs(rectTransform.anchoredPosition.y-targetY)>0.1f)
            {
                if (!isWaiting)
                {
                    // Pomeranje objekta prema meti
                    rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, new Vector2(targetX, targetY), speed * Time.deltaTime);
                }

                yield return null;
            }

            // Kada stignemo do mete, stani na neko vreme
            if (!isWaiting)
            {
                isWaiting = true;
                float waitTime = Random.Range(waitTimeMin, waitTimeMax);
                yield return new WaitForSeconds(waitTime);
                isWaiting = false;
            }
        }
    }
}
