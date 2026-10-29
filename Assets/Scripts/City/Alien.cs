using UnityEngine;
using System.Collections;

public class Alien : MonoBehaviour
{
    public Sprite profileImage;
    public Sprite walk1Image;
    public Sprite walk2Image;

    public Sprite additionDownLeft;
    public Sprite additionDownRight;
    public Sprite additionUpLeft;
    public Sprite additionUpRight;

    public Sprite additionDownLeftCover;
    public Sprite additionDownRightCover;
    public Sprite additionUpLeftCover;
    public Sprite additionUpRightCover;

    public Color additionColor;

    public int side;

    public GameObject backGround;

    private SpriteRenderer additionDownLeftHolder;
    private SpriteRenderer additionDownRightHolder;
    private SpriteRenderer additionUpLeftHolder;
    private SpriteRenderer additionUpRightHolder;

    private SpriteRenderer additionDownLeftCoverHolder;
    private SpriteRenderer additionDownRightCoverHolder;
    private SpriteRenderer additionUpLeftCoverHolder;
    private SpriteRenderer additionUpRightCoverHolder;

    private float speed = 2.5f;

    private void Start()
    {
        additionDownLeftHolder = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        additionDownRightHolder = gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>();
        additionUpLeftHolder = gameObject.transform.GetChild(2).GetComponent<SpriteRenderer>();
        additionUpRightHolder = gameObject.transform.GetChild(3).GetComponent<SpriteRenderer>();

        additionDownLeftCoverHolder = gameObject.transform.GetChild(4).GetComponent<SpriteRenderer>();
        additionDownRightCoverHolder = gameObject.transform.GetChild(5).GetComponent<SpriteRenderer>();
        additionUpLeftCoverHolder = gameObject.transform.GetChild(6).GetComponent<SpriteRenderer>();
        additionUpRightCoverHolder = gameObject.transform.GetChild(7).GetComponent<SpriteRenderer>();

        StartCoroutine(Animate());
    }

    private void Update()
    {
        if (backGround != null)
        {
            if (!backGround.activeInHierarchy)
            {
                Destroy(gameObject);
            }
        }

        if (transform.position.x < -57f || transform.position.x > 57f)
        {
            Destroy(gameObject);
        }

        if(side == 0)
        {
            transform.position += new Vector3(speed*Time.deltaTime, 0, 0);
        }
        else
        {
            transform.position -= new Vector3(speed*Time.deltaTime, 0, 0);
        }
    }

    private IEnumerator Animate()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        int animationState = 0;
        int nextState = 1;
        additionColor = new Color(additionColor.r, additionColor.g, additionColor.b, 0.5f);

        if (additionDownLeft != null)
        {
            float randomRotation = Random.Range(0, 360);

            additionDownLeftHolder.sprite = additionDownLeft;
            additionDownLeftHolder.sortingOrder = spriteRenderer.sortingOrder + 1;
            additionDownLeftHolder.transform.localRotation = Quaternion.Euler(0, 0, randomRotation);

            additionDownLeftCoverHolder.sprite = additionDownLeftCover;
            additionDownLeftCoverHolder.sortingOrder = spriteRenderer.sortingOrder + 2;
            additionDownLeftCoverHolder.color = additionColor;
            additionDownLeftCoverHolder.transform.localRotation = Quaternion.Euler(0, 0, randomRotation);
            additionDownLeftCoverHolder.enabled = true;
        }
        if (additionDownRight != null)
        {
            float randomRotation = Random.Range(0, 360);

            additionDownRightHolder.sprite = additionDownRight;
            additionDownRightHolder.sortingOrder = spriteRenderer.sortingOrder + 1;
            additionDownRightHolder.transform.localRotation = Quaternion.Euler(0, 0, randomRotation);

            additionDownRightCoverHolder.sprite = additionDownRightCover;
            additionDownRightCoverHolder.sortingOrder = spriteRenderer.sortingOrder + 2;
            additionDownRightCoverHolder.color = additionColor;
            additionDownRightCoverHolder.transform.localRotation = Quaternion.Euler(0, 0, randomRotation);
            additionDownRightCoverHolder.enabled = true;
        }
        if (additionUpLeft != null)
        {
            float randomRotation = Random.Range(0, 360);

            additionUpLeftHolder.sprite = additionUpLeft;
            additionUpLeftHolder.sortingOrder = spriteRenderer.sortingOrder + 1;
            additionUpLeftHolder.transform.localRotation = Quaternion.Euler(0, 0, randomRotation);

            additionUpLeftCoverHolder.sprite = additionUpLeftCover;
            additionUpLeftCoverHolder.sortingOrder = spriteRenderer.sortingOrder + 2;
            additionUpLeftCoverHolder.color = additionColor;
            additionUpLeftCoverHolder.transform.localRotation = Quaternion.Euler(0, 0, randomRotation);
            additionUpLeftCoverHolder.enabled = true;
        }
        if (additionUpRight != null)
        {
            float randomRotation = Random.Range(0, 360);

            additionUpRightHolder.sprite = additionUpRight;
            additionUpRightHolder.sortingOrder = spriteRenderer.sortingOrder + 1;
            additionUpRightHolder.transform.localRotation = Quaternion.Euler(0, 0, randomRotation);

            additionUpRightCoverHolder.sprite = additionUpRightCover;
            additionUpRightCoverHolder.sortingOrder = spriteRenderer.sortingOrder + 2;
            additionUpRightCoverHolder.color = additionColor;
            additionUpRightCoverHolder.transform.localRotation = Quaternion.Euler(0, 0, randomRotation);
            additionUpRightCoverHolder.enabled = true;
        }


        while (true)
        {
            if (animationState == 0 && nextState == 1)
            {
                spriteRenderer.sprite = profileImage;
                animationState = 1;
                nextState = 0;
            }
            else if (animationState == 1 && nextState == 0)
            {
                spriteRenderer.sprite = walk1Image;
                animationState = 0;
                nextState = -1;
            }
            else if (animationState == 0 && nextState == -1)
            {
                spriteRenderer.sprite = profileImage;
                animationState = -1;
                nextState = 0;
            }
            else if (animationState == -1 && nextState == 0)
            {
                spriteRenderer.sprite = walk2Image;
                animationState = 0;
                nextState = 1;
            }

            yield return new WaitForSeconds(0.17f);
        }
    }
}
