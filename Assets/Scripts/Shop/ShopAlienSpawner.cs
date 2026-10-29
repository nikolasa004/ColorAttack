using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ShopAlienSpawner : MonoBehaviour
{
    public GameObject alienPrefab;
    public GameObject alienHolder;

    public List<Sprite> profileImages;
    public List<Sprite> walk1Images;
    public List<Sprite> walk2Images;

    public List<Sprite> additions;
    public List<Sprite> additionCovers;
    public List<Color> alienColors;

    private float xBorder = 22f;
    private float upBorder = -2.3f;
    private float downBorder = -5f;

    private int startOrder = 4;

    private int numberOfAliens = 0;

    private void OnEnable()
    {
        StartCoroutine(SpawnAliens());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator SpawnAliens()
    {
        while (true)
        {
            int randomSide = Random.Range(0, 2);
            float xStart = 0;

            float currentX=gameObject.transform.position.x;
            if (randomSide == 0)
            {
                xStart = currentX - xBorder;
            }
            else
            {
                xStart = currentX + xBorder;
            }

            int randomRow = Random.Range(0, (int)(Mathf.Abs(downBorder - upBorder) * 10) + 1);
            float yStart = upBorder - (randomRow * 0.1f);

            int order = startOrder + randomRow * 3;

            GameObject alien = Instantiate(alienPrefab, new Vector3(xStart, yStart, 0), Quaternion.identity, alienHolder.transform);
            if (randomSide == 1)
            {
                alien.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }

            int randomProfile = Random.Range(0, profileImages.Count);

            Alien alienScript = alien.GetComponent<Alien>();

            alienScript.profileImage = profileImages[randomProfile];
            alienScript.walk1Image = walk1Images[randomProfile];
            alienScript.walk2Image = walk2Images[randomProfile];

            alienScript.side = randomSide;

            alien.GetComponent<SpriteRenderer>().sortingOrder = order;

            alienScript.additionColor = alienColors[randomProfile];

            randomSide = Random.Range(0, 5);
            if (randomSide != 0)
            {
                alienScript.additionDownLeft = null;
            }
            else
            {
                int randomAddition = Random.Range(0, additions.Count);
                alienScript.additionDownLeft = additions[randomAddition];
                alienScript.additionDownLeftCover = additionCovers[randomAddition];
            }

            randomSide = Random.Range(0, 5);
            if (randomSide != 0)
            {
                alienScript.additionDownRight = null;
            }
            else
            {
                int randomAddition = Random.Range(0, additions.Count);
                alienScript.additionDownRight = additions[randomAddition];
                alienScript.additionDownRightCover = additionCovers[randomAddition];
            }

            randomSide = Random.Range(0, 5);
            if (randomSide != 0)
            {
                alienScript.additionUpLeft = null;
            }
            else
            {
                int randomAddition = Random.Range(0, additions.Count);
                alienScript.additionUpLeft = additions[randomAddition];
                alienScript.additionUpLeftCover = additionCovers[randomAddition];
            }

            randomSide = Random.Range(0, 5);
            if (randomSide != 0)
            {
                alienScript.additionUpRight = null;
            }
            else
            {
                int randomAddition = Random.Range(0, additions.Count);
                alienScript.additionUpRight = additions[randomAddition];
                alienScript.additionUpRightCover = additionCovers[randomAddition];
            }

            numberOfAliens++;

            if (numberOfAliens >= 20)
            {
                float waitTime = Random.Range(0.5f, 3f);
                yield return new WaitForSeconds(waitTime);
            }
            else
            {
                float waitTime = Random.Range(0.5f, 1.5f);
                yield return new WaitForSeconds(waitTime);
            }
        }
    }
}
