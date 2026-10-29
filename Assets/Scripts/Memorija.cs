using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Memorija : MonoBehaviour
{
    public GameObject tranzicija;
    public Animator tranzicijaAnimator;

    public TextMeshProUGUI skorText;
    public TextMeshProUGUI highScoreText;

    public ParticleSystem noviHSEfekat;

    public List<GameObject> karte= new List<GameObject>();
    public List<Sprite> slike= new List<Sprite>();

    public Dialog memoryDialog;

    private List<Sprite> pomocneSlike = new List<Sprite>();

    private bool prvaKarta = false;
    private bool drugaKarta = false;

    private string prvaKartaIme;
    private GameObject prvaKartaObjekat;
    private GameObject drugaKartaObjekat;

    private int brojKarata = 24;

    private int skor = 0;
    void Start()
    {
        if(PlayerPrefs.GetInt("MemoryFirstTime", 0) == 0)
        {
            DialogManager.instance.StartDialogue(memoryDialog);

            PlayerPrefs.SetInt("MemoryFirstTime", 1);
        }

        for (int i=0;i<slike.Count;i++)
        {
            pomocneSlike.Add(slike[i]);
        }

        for(int i=0;i<slike.Count;i++)
        {
            int random = Random.Range(0, pomocneSlike.Count);

            karte[i].GetComponent<Image>().sprite = pomocneSlike[random];
            pomocneSlike.RemoveAt(random);
        }

        StartCoroutine(Pocetak());
    }
    void Update()
    {
        skorText.text = "Score: " + skor;
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScoreMemory",1000);

        if(brojKarata==0)
        {
            Kraj();
        }

        Tranzicija();
    }

    public void pritisakKarte()
    {
        if (!prvaKarta)
        {
            prvaKarta = true;
            prvaKartaObjekat = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            prvaKartaIme = prvaKartaObjekat.transform.GetChild(0).GetComponent<Image>().sprite.name;
            prvaKartaObjekat.GetComponent<Animator>().SetBool("Otkrivanje",true);
            prvaKartaObjekat.GetComponent<Button>().enabled = false;
        }
        else if (!drugaKarta)
        {
            drugaKarta = true;
            drugaKartaObjekat = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            drugaKartaObjekat.GetComponent<Animator>().SetBool("Otkrivanje", true);
            drugaKartaObjekat.GetComponent<Button>().enabled = false;
            StartCoroutine(provera());
        }
    }
    public IEnumerator provera()
    {
        yield return new WaitForSeconds(0.5f);
        if (prvaKartaIme == drugaKartaObjekat.transform.GetChild(0).GetComponent<Image>().sprite.name)
        {
            prvaKartaObjekat.GetComponent<Animator>().SetTrigger("Pogodak");
            drugaKartaObjekat.GetComponent<Animator>().SetTrigger("Pogodak");
            prvaKartaObjekat.GetComponentInChildren<ParticleSystem>().Play();
            drugaKartaObjekat.GetComponentInChildren<ParticleSystem>().Play();

            brojKarata -= 2;

            yield return new WaitForSeconds(0.2f);
            prvaKarta = false;
            drugaKarta = false;
        }
        else
        {
            prvaKartaObjekat.GetComponent<Animator>().SetBool("Otkrivanje", false);
            drugaKartaObjekat.GetComponent<Animator>().SetBool("Otkrivanje", false);
            skor += 1;

            yield return new WaitForSeconds(0.2f);
            prvaKartaObjekat.GetComponent<Button>().enabled = true;
            drugaKartaObjekat.GetComponent<Button>().enabled = true;
            prvaKarta = false;
            drugaKarta = false;
        }
    }   

    public void Kraj()
    {
        if (skor < PlayerPrefs.GetInt("HighScoreMemory",1000))
        {
            PlayerPrefs.SetInt("HighScoreMemory", skor);
            noviHSEfekat.Play();
            StartCoroutine(memorija(5));
        }
        else
        {
            StartCoroutine(memorija(3));
        }
    }
    public IEnumerator memorija(float proslava)
    {
        yield return new WaitForSeconds(proslava);
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(8);
    }
    public IEnumerator Pocetak()
    {
        yield return new WaitForSeconds(0.1f);
        tranzicijaAnimator.SetBool("tranzicija1", true);
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
