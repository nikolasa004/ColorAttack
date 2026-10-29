using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sankanje : MonoBehaviour
{
    public GameObject tranzicija;
    public Animator tranzicijaAnimator;

    public TextMeshProUGUI skorText;
    public TextMeshProUGUI highScoreText;

    public ParticleSystem noviHSEfekat;

    public GameObject drvo;
    public GameObject sanke;

    public Animator podloga;
    public GameObject snegPoEkranu;
    public ParticleSystem snegPoEkranuEfekat;
    public ParticleSystem snegEfekat;

    public float pozicijaSirina=1.5f;
    public float pozicijaVisina=0.4f;
    public float spawnInterval = 3f;

    public int skor = 0;

    private bool vremeZaPromenu = false;
    private bool vremeZaSpawn = false;
    private Vector2 startPos, endPos;
    private int pozicija = 0;

    public static int orderInLayer = 10000;

    void Start()
    {
        Time.timeScale = 1;
        orderInLayer = 10000;
        StartCoroutine(Pocetak());
    }
    void Update()
    {
        skorText.text = "Score: " + skor;
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScoreSankanje", 0);

        podloga.speed = 2/spawnInterval;
        var main = snegEfekat.main;
        main.startSpeed=new ParticleSystem.MinMaxCurve(5/spawnInterval);
        var emission = snegEfekat.emission;
        emission.rateOverTime = 80/spawnInterval;
        sanke.GetComponent<Animator>().speed = 2/spawnInterval;
        
        Tranzicija();
        promenaBrzine();
        pokreniSpawn();
        Slide();
    }
    public void Slide()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startPos = touch.position;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                endPos = touch.position;

                if (endPos.x < startPos.x && pozicija != 1)
                {
                    Desno();
                }
                else if (endPos.x > startPos.x && pozicija != -1)
                {
                    Levo();
                }
            }
        }
    }
    public void Levo()
    {
        pozicija--;
        sanke.GetComponent<Animator>().SetTrigger("Desno");
    }
    public void Desno()
    {
        pozicija++;
        sanke.GetComponent<Animator>().SetTrigger("Levo");
    }
    public void promenaBrzine()
    {
        if (vremeZaPromenu)
        {
            StartCoroutine(cekajPromenu());
            vremeZaPromenu = false;
        }
    }
    public IEnumerator cekajPromenu()
    {
        yield return new WaitForSeconds(3f);
        spawnInterval -= 0.1f;
        if (spawnInterval < 1f)
        {
            spawnInterval = 1f;
        }
        vremeZaPromenu = true;
    }
    public void pokreniSpawn()
    {
        if (vremeZaSpawn)
        {
            StartCoroutine(cekajSpawn());
            vremeZaSpawn = false;
        }
    }
    public IEnumerator cekajSpawn()
    {
        Spawn();
        yield return new WaitForSeconds(spawnInterval);
        vremeZaSpawn = true;
    }
    public void Spawn()
    {
        int rand=Random.Range(0, 6);
        
        if(orderInLayer==3)
        {
            orderInLayer = 10000;
        }
        else
        {
            orderInLayer--;
        }

        switch (rand)
        {
            case 0:
                Instantiate(drvo, new Vector3(-pozicijaSirina, pozicijaVisina, 0), Quaternion.identity);
                break;
            case 1:
                Instantiate(drvo, new Vector3(pozicijaSirina, pozicijaVisina, 0), Quaternion.identity);
                break;
            case 2:
                Instantiate(drvo, new Vector3(0, pozicijaVisina, 0), Quaternion.identity);
                break;
            case 3:
                Instantiate(drvo, new Vector3(-pozicijaSirina, pozicijaVisina, 0), Quaternion.identity);
                Instantiate(drvo, new Vector3(pozicijaSirina, pozicijaVisina, 0), Quaternion.identity);
                break;
            case 4:
                Instantiate(drvo, new Vector3(-pozicijaSirina, pozicijaVisina, 0), Quaternion.identity);
                Instantiate(drvo, new Vector3(0, pozicijaVisina, 0), Quaternion.identity);
                break;
            case 5:
                Instantiate(drvo, new Vector3(pozicijaSirina, pozicijaVisina, 0), Quaternion.identity);
                Instantiate(drvo, new Vector3(0, pozicijaVisina, 0), Quaternion.identity);
                break;
            default:
                break;
        }
    }
    public void Kraj()
    {
        Time.timeScale = 0;
        sanke.GetComponent<SpriteRenderer>().sortingOrder = 10001;
        if (skor > PlayerPrefs.GetInt("HighScoreSankanje", 0))
        {
            PlayerPrefs.SetInt("HighScoreSankanje", skor);
            noviHSEfekat.Play();
        }

        snegPoEkranu.SetActive(true);
        snegPoEkranuEfekat.Play();

        StartCoroutine(sankanje());
    }
    public IEnumerator sankanje()
    {
        yield return new WaitForSecondsRealtime(3f);
        snegPoEkranu.SetActive(false);
        yield return new WaitForSecondsRealtime(1f);
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene(10);
    }
    public IEnumerator Pocetak()
    {
        yield return new WaitForSeconds(0.1f);
        tranzicijaAnimator.SetBool("tranzicija1", true);

        yield return new WaitForSeconds(1.5f);
        vremeZaPromenu = true;
        vremeZaSpawn = true;
    }
    public void Tranzicija()
    {
        if (tranzicijaAnimator.GetCurrentAnimatorStateInfo(0).IsName("kraj"))
        {
            tranzicija.SetActive(false);
        }
    }
    public void Menu()
    {
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        StartCoroutine(meni());
    }
    public IEnumerator meni()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(7);
    }
}