using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PicaDrop : MonoBehaviour
{
    public GameObject tranzicija;
    public Animator tranzicijaAnimator;

    public GameObject pecnica1;
    public GameObject pecnica2;
    public GameObject pecnica3;

    public GameObject linija1;
    public GameObject linija2;
    public GameObject linija3;

    public List<GameObject> pice = new List<GameObject>();

    public Dialog pizza;

    public GameObject soundHolder;
    private AudioSource fire;
    private AudioSource firing;
    private AudioSource joining;
    private AudioSource end;
    private AudioSource complete;

    private bool soundPlayed = false;

    public GameObject completeEffect;

    private GameObject izabranaPecnica;
    private GameObject izabranaLinija;

    public static Transform pocetakPucanja;
    public static int brojPecnice = 0;

    public static bool mozePucati = true;

    private bool izabranaPecnicaBool = false;

    public static bool cekanje=false;

    public static bool gasi=false;

    public static Tuple<int,Vector2> stvoriBroj;

    public TextMeshProUGUI skorTekst;
    public TextMeshProUGUI dijamanti;

    public static bool krajIgre = false;

    public static int skor = 0;

    private bool dodelioDijamante = false;

    private List<GameObject> allPizzas = new List<GameObject>();
    void Start()
    {
        Camera.main.GetComponent<AudioSource>().volume = PlayerPrefs.GetInt("MusicVolume", 100) * Camera.main.GetComponent<AudioSource>().volume / 100f;

        AudioSource[] zvuci = soundHolder.GetComponents<AudioSource>();
        fire = zvuci[0];
        fire.volume = PlayerPrefs.GetInt("UIVolume", 100) * fire.volume / 100f;
        firing = zvuci[1];
        firing.volume = PlayerPrefs.GetInt("UIVolume", 100) * firing.volume / 100f;
        joining = zvuci[2];
        joining.volume = PlayerPrefs.GetInt("UIVolume", 100) * joining.volume / 100f;
        end = zvuci[3];
        end.volume = PlayerPrefs.GetInt("UIVolume", 100) * end.volume / 100f;
        complete = zvuci[4];
        complete.volume = PlayerPrefs.GetInt("UIVolume", 100) * complete.volume / 100f;

        if(PlayerPrefs.GetInt("Pizza", 0) == 1)
        {
            DialogManager.instance.StartDialogue(pizza);
        }

        mozePucati = true;
        cekanje = false;
        gasi = false;
        stvoriBroj = new Tuple<int, Vector2>(100,new Vector2(0,0));
        krajIgre = false;
        skor = 0;

        StartCoroutine(Pocetak());

        StartCoroutine(ResolveOverlaping());
    }

    void Update()
    {
        Tranzicija();

        Tekstovi();

        if (PlayerPrefs.GetInt("Pizza",0)<2)
        {
            return;
        }


        ProveraKraja();

        if(izabranaPecnicaBool && !krajIgre)
        {
            Pucanje();
        }

        if (!cekanje && !krajIgre)
        {
            StartCoroutine(cekanjeZaPecnicu());
            cekanje = true;
        }

        if(gasi || krajIgre)
        {
            if(gasi)
            {
                firing.Play();
            }
            else if(soundPlayed==false)
            {
                end.Play();
                soundPlayed=true;
            }

            gasi = false;
            izabranaPecnica.GetComponent<Image>().color = new Color32(120, 120, 120, 255);
            izabranaPecnica.GetComponent<Shadow>().effectColor = new Color32(120, 120, 120, 255);
            izabranaPecnica.GetComponent<Animator>().SetBool("pecnica", false);
            izabranaLinija.SetActive(false);
            fire.enabled = false;
        }

        if(stvoriBroj.Item1 != 100 && !krajIgre)
        {
            if(stvoriBroj.Item1!=10)
            {
                //random rotacija
                float randomRotacija = UnityEngine.Random.Range(0, 360);

                GameObject klon= Instantiate(pice[stvoriBroj.Item1], stvoriBroj.Item2, Quaternion.Euler(0,0,randomRotacija));
                klon.GetComponent<PicaMetak>().stvoren = true;
                klon.GetComponentInChildren<ParticleSystem>().Play();

                allPizzas.Add(klon);

                joining.Play();

                stvoriBroj = new Tuple<int, Vector2>(100, new Vector2(0, 0));
            }
            else
            {
                GameObject createdEffect= Instantiate(completeEffect, stvoriBroj.Item2, Quaternion.identity);
                createdEffect.GetComponent<ParticleSystem>().Play();

                complete.Play();

                stvoriBroj = new Tuple<int, Vector2>(100, new Vector2(0, 0));

                StartCoroutine(DestroyEffect(createdEffect));
            }
        }
    }

    public IEnumerator DestroyEffect(GameObject effect)
    {
        yield return new WaitForSeconds(3f);
        Destroy(effect);
    }

    public void ProveraKraja()
    {
        if(krajIgre)
        {
            StartCoroutine(KrajIgre());
        }
    }

    public void Tekstovi()
    {
        skorTekst.text = "Score:\n" + skor;
        dijamanti.text = PlayerPrefs.GetInt("Dijamanti", 0) + "<sprite=0>";
    }

    public IEnumerator Pocetak()
    {
        yield return new WaitForSeconds(0.1f);
        tranzicijaAnimator.SetBool("tranzicija1", true);
    }

    public void AktiviranjePecnice()
    {
        int random= UnityEngine.Random.Range(0, 3);

        if(random==0)
        {
            izabranaPecnica = pecnica1;
            izabranaLinija = linija1;
            brojPecnice = 1;
        }
        else if(random==1)
        {
            izabranaPecnica = pecnica2;
            izabranaLinija = linija2;
            brojPecnice = 2;
        }
        else if(random==2)
        {
            izabranaPecnica = pecnica3;
            izabranaLinija = linija3;
            brojPecnice = 3;
        }

        pocetakPucanja = izabranaPecnica.transform;
        izabranaPecnica.GetComponent<Image>().color= new Color32(255, 255, 255, 255);
        izabranaPecnica.GetComponent<Shadow>().effectColor = new Color32(0, 0, 0, 255); 
        izabranaPecnica.GetComponent<Animator>().SetBool("pecnica", true);
        izabranaPecnicaBool = true;
        fire.enabled = true;

        GameObject pica = BiranjePice();
        Instantiate(pica, izabranaPecnica.transform.position, Quaternion.identity);

        allPizzas.Add(pica);
    }

    public IEnumerator cekanjeZaPecnicu()
    {
        yield return new WaitForSeconds(2f);
        AktiviranjePecnice();
    }

    public void Pucanje()
    {
        if(Input.touchCount>0 && mozePucati)
        {
            mozePucati = false;
            izabranaLinija.SetActive(true);
            izabranaPecnicaBool = false;
        }
    }
    public GameObject BiranjePice()
    {
        GameObject pica;

        int random = UnityEngine.Random.Range(0, 100);

        //ima 10 vrsta pica
        if(random<25)
        {
            pica = pice[0];
        }
        else if(random<45)
        {
            pica = pice[1];
        }
        else if(random<60)
        {
            pica = pice[2];
        }
        else if(random<70)
        {
            pica = pice[3];
        }
        else if(random<78)
        {
            pica = pice[4];
        }
        else if(random<85)
        {
            pica = pice[5];
        }
        else if(random<91)
        {
            pica = pice[6];
        }
        else if(random<96)
        {
            pica = pice[7];
        }
        else if(random<100)
        {
            pica = pice[8];
        }
        else
        {
            pica = pice[9];
        }

        return pica;
    }
    public void Tranzicija()
    {
        if (tranzicijaAnimator.GetCurrentAnimatorStateInfo(0).IsName("kraj"))
        {
            tranzicija.SetActive(false);
        }
    }

    public IEnumerator KrajIgre()
    {
        if (!dodelioDijamante)
        {
            dodelioDijamante = true;
            PlayerPrefs.SetInt("Dijamanti", PlayerPrefs.GetInt("Dijamanti", 0) + skor / 250);
            SoundManager.Instance.PlayCoinSound();
        }
        yield return new WaitForSeconds(2f);
        Menu();
    }
    public void Menu()
    {
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        SoundManager.Instance.PlayClickSound();
        StartCoroutine(meni());
    }
    public IEnumerator meni()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(2);
    }

    private IEnumerator ResolveOverlaping()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            ResolveOverlaps();
        }
    }

    void ResolveOverlaps()
    {
        List<Collider2D> pizzas = new List<Collider2D>();
        foreach (var pizza in allPizzas)
        {
            if (pizza != null)
            {
                pizzas.Add(pizza.GetComponent<Collider2D>());
            }
            else
            {
                Debug.Log("Pica je null");
            }   
        }

        foreach (var smallPizza in pizzas)
        {
            foreach (var bigPizza in pizzas)
            {
                if (smallPizza == null || bigPizza == null || smallPizza == bigPizza)
                    continue;

                // Proveravamo da li je svaki ugao (bound) male pice unutar velike pice
                Bounds smallBounds = smallPizza.bounds;
                Bounds bigBounds = bigPizza.bounds;

                // Provera da li sve tacke od smallBounds su unutar bigBounds
                if (IsBoundsInside(smallBounds, bigBounds))
                {
                    Debug.Log($"Pica {smallPizza.name} je unutar {bigPizza.name}!");
                }
            }
        }
    }

    bool IsBoundsInside(Bounds small, Bounds big)
    {
        // Provera da li su sve 8 granicne tacke unutra
        Vector3[] corners = new Vector3[8];
        corners[0] = new Vector3(small.min.x, small.min.y, small.min.z);
        corners[1] = new Vector3(small.min.x, small.min.y, small.max.z);
        corners[2] = new Vector3(small.min.x, small.max.y, small.min.z);
        corners[3] = new Vector3(small.min.x, small.max.y, small.max.z);
        corners[4] = new Vector3(small.max.x, small.min.y, small.min.z);
        corners[5] = new Vector3(small.max.x, small.min.y, small.max.z);
        corners[6] = new Vector3(small.max.x, small.max.y, small.min.z);
        corners[7] = new Vector3(small.max.x, small.max.y, small.max.z);

        foreach (var corner in corners)
        {
            if (!big.Contains(corner))
                return false; // Ako bilo koji ugao nije unutra, nije cela pica unutar
        }

        return true; // Sve tacke su unutra
    }
}
