using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeniCity : MonoBehaviour
{
    public List<GameObject> blokovi = new List<GameObject>();
    private Vector2 startPos, endPos;
    public static int pozicija = 0;
    public float minSwipeDistance = 50f;

    private GameObject selectedBlock;
    private Animator animator;

    private void Start()
    {
        pozicija = 0;
        animator = Camera.main.GetComponent<Animator>();
    }

    void Update()
    {
        Slide();
    }

    public void Slide()
    {
        if (Input.touchCount > 0 && (animator.GetCurrentAnimatorStateInfo(0).IsName("New State") || animator.GetCurrentAnimatorStateInfo(0).IsName("LevoStoji") || animator.GetCurrentAnimatorStateInfo(0).IsName("DesnoStoji")))
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startPos = touch.position;

                Ray ray = Camera.main.ScreenPointToRay(startPos);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                if (hit.collider != null)
                {
                    foreach (GameObject blok in blokovi)
                    {
                        if (hit.collider.gameObject == blok)
                        {
                            selectedBlock = blok;
                            break;
                        }
                    }
                }
            }

            if (touch.phase == TouchPhase.Ended && selectedBlock != null)
            {
                endPos = touch.position;
                float deltaX = endPos.x - startPos.x;

                if (Mathf.Abs(deltaX) > minSwipeDistance)
                {
                    if (deltaX < 0 && pozicija != 1)
                    {
                        Desno();
                    }
                    else if (deltaX > 0 && pozicija != -1)
                    {
                        Levo();
                    }
                }

                selectedBlock = null;
            }
        }
    }


    public void Levo(bool two=false)
    {
        pozicija--;
        animator.SetTrigger("Levo");

        if (two)
        {
            pozicija--;
            StartCoroutine(DoubleSlide("left"));
        }
    }
    public void Desno(bool two=false)
    {
        pozicija++;
        animator.SetTrigger("Desno");

        if (two)
        {
            pozicija++;
            StartCoroutine(DoubleSlide("right"));
        }
    }

    private IEnumerator DoubleSlide(string side)
    {
        yield return new WaitForSeconds(0.5f);
        if (side == "left")
        {
            Levo();
        }
        else if (side == "right")
        {
            Desno();
        }
    }
}
