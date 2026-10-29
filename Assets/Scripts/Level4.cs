using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level4 : MonoBehaviour
{
    public List<GameObject> kvadrat = new List<GameObject>();
    public List<GameObject> trougao = new List<GameObject>();
    public List<GameObject> krug = new List<GameObject>();

    public List<GameObject> klonovi = new List<GameObject>();

    public Text poeni;
    public Text highScore;

    public ParticleSystem plavi;
    public ParticleSystem zuti;
    public ParticleSystem crveni;
    public ParticleSystem narandzasti;
    public ParticleSystem ljubicasti;
    public ParticleSystem zeleni;
    public ParticleSystem crni;
    public ParticleSystem vatromet;
    public ParticleSystem mehuriSkoljkaPlavo;
    public ParticleSystem mehuriSkoljkaCrveno;
    public ParticleSystem mehuriSkoljkaZuto;
    public ParticleSystem ljubicasteRibice;
    public ParticleSystem crveneRibice;
    public ParticleSystem plaveRibice;
    public ParticleSystem zuteRibice;
    public ParticleSystem narandzasteRibice;
    public ParticleSystem zeleneRibice;
    public ParticleSystem ljubicasteTrougaoRibice;
    public ParticleSystem crveneTrougaoRibice;
    public ParticleSystem plaveTrougaoRibice;
    public ParticleSystem zuteTrougaoRibice;
    public ParticleSystem narandzasteTrougaoRibice;
    public ParticleSystem zeleneTrougaoRibice;

    public GameObject zidL;
    public GameObject zidR;
    public GameObject zidU;
    public GameObject zidD;
    public GameObject ekstraZivot;
    public GameObject goodZone;
    public GameObject tranzicija;
    public GameObject udica;
    public GameObject ribaNaUdici;
    public GameObject kraba;
    public GameObject krabinNovcic;
    public GameObject velikiKrabinNovcic;
    public GameObject novcic;
    public GameObject strujaPozadina;
    public GameObject plavoPozadina;
    public GameObject strujaNoviNovac;
    public GameObject skip;
    public GameObject hobotnica;
    public GameObject mehuriPodmornica;
    public GameObject mehuriStart;
    public GameObject ajkula;
    public GameObject crvenoDugmeObjekat;
    public GameObject plavoDugmeObjekat;
    public GameObject zutoDugmeObjekat;
    public GameObject ukupniNovcici;
    public GameObject ukupniDijamanti;

    public Animator tranzicijaAnimator;
    private Animator zutoDugme;
    private Animator crvenoDugme;
    private Animator plavoDugme;
    public Animator strujaMeduza;
    public Animator mastilo;
    public Animator podmornica;

    public TextMeshProUGUI ukupanNovac;
    public TextMeshProUGUI ukupniDijamant;

    public Sprite nista;

    public string boja = "";
    public float brzina1 = 1f;
    public float brzina = 1f;
    public int skor = 0;

    private bool game_over = false;
    private bool plavo = false;
    private bool crveno = false;
    private bool zuto = false;
    private int ekstra = 0;
    private int moc = 0;
    private bool moze=false;
    private Vector3 vektorUdica;
    private bool kretanje=true;
    private Sprite trenutnaRiba;
    private bool partiklaJednom=false;
    private bool krabaKlik=false;
    private bool noviNovac=false;

    private int bonusNovcici = 0;
    private int skorZaNovcic = 10;
    private int mnozilacNovcica = 1;

    private bool jednobojno = false;
    private int bojaJednobojna = 0;
    public Sprite crvenaSkoljka;
    public Sprite plavaSkoljka;
    public Sprite zutaSkoljka;
    public RuntimeAnimatorController crvenoDugmeAnimator;
    public RuntimeAnimatorController plavoDugmeAnimator;
    public RuntimeAnimatorController zutoDugmeAnimator;

    public Animator zlatnaRibica;
    private bool goldRush = false;

    private float brojOdbrane = 0;
    private List<int> indeksiOdbrane = new List<int>();
    private bool odbrana = false;

    private bool bezi=false;

    public Animator lobanja;
    private bool lobanjaTotem= false;

    private void Start()
    {
        crvenoDugme = crvenoDugmeObjekat.GetComponent<Animator>();
        plavoDugme = plavoDugmeObjekat.GetComponent<Animator>();
        zutoDugme = zutoDugmeObjekat.GetComponent<Animator>();

        if (Meni.kampanja)
        {
            if(PlayerPrefs.GetInt("kasica",0) == 1)
            {
                mnozilacNovcica = 2;
            }

            if (PlayerPrefs.GetInt("scoreForCoin", 10) < 1)
            {
                bonusNovcici = -PlayerPrefs.GetInt("scoreForCoin", 10) + 1;
                bonusNovcici *= mnozilacNovcica;
                skorZaNovcic = 1;
            }
            else
            {
                skorZaNovcic = PlayerPrefs.GetInt("scoreForCoin", 10);
            }

            if(PlayerPrefs.GetInt("colorShield",0)<12)
            {
                brojOdbrane = PlayerPrefs.GetInt("colorShield", 0);
            }
            else
            {
                brojOdbrane = 11;
            }

            for (int i = 0; i < brojOdbrane; i++)
            {
                while (true)
                {
                    int rand = UnityEngine.Random.Range(0, 12);
                    if (!indeksiOdbrane.Contains(rand))
                    {
                        indeksiOdbrane.Add(rand);
                        break;
                    }
                }
            }
        }

        if (Meni.kampanja)
        {
            highScore.text = "High Score - " + PlayerPrefs.GetInt("HighScore4Cm", 0);
            skor = PlayerPrefs.GetInt("startScore", 0);

            ukupniDijamanti.SetActive(false);
        }
        else
        {
            highScore.text = "High Score - " + PlayerPrefs.GetInt("HighScore4Cl", 0);

            ukupniNovcici.SetActive(false);
        }

        zutoDugme.transform.position = new Vector2((zidL.transform.position.x + crvenoDugme.transform.position.x) / 2 - 0.2f, zutoDugme.transform.position.y);
        plavoDugme.transform.position = new Vector2((zidR.transform.position.x + crvenoDugme.transform.position.x) / 2 + 0.2f, plavoDugme.transform.position.y);

        StartCoroutine(Pocetak());

        RectTransform kanvasTransform = goodZone.GetComponent<RectTransform>();

        brzina1 = ((0.05f * kanvasTransform.rect.height) / 1870f) * 30;

        vektorUdica=udica.transform.position;

        mehuriSkoljkaCrveno.transform.position = new Vector2(crvenoDugme.GetComponent<RectTransform>().transform.position.x, mehuriSkoljkaCrveno.transform.position.y);
        mehuriSkoljkaPlavo.transform.position = new Vector2(plavoDugme.GetComponent<RectTransform>().transform.position.x, mehuriSkoljkaPlavo.transform.position.y);
        mehuriSkoljkaZuto.transform.position = new Vector2(zutoDugme.GetComponent<RectTransform>().transform.position.x, mehuriSkoljkaZuto.transform.position.y);

        kraba.GetComponent<Animator>().speed = (zidD.transform.position.x - zidL.transform.position.x)/2;
    }
    public void Update()
    {

        TrenutnaBoja();

        Brzina();

        SkorTekst();

        if (!lobanjaTotem)
        {
            ProveraPoraza();
        }

        SareneBoje();

        Trougao();

        Krug();

        Padanje();

        Tranzicija();

        Tastatura();

        UspehUdica();

        if(podmornica.GetCurrentAnimatorStateInfo(0).IsName("Podmornica"))
        {
            mehuriPodmornica.SetActive(true);
            mehuriPodmornica.transform.position=mehuriStart.transform.position;
        }
        else
        {
            mehuriPodmornica.SetActive(false);
        }
    }

    public void Brzina()
    {
        if (Meni.kampanja)
        {
            brzina = brzina1 + skor * brzina1 * PlayerPrefs.GetFloat("speedChange", 6) * Time.deltaTime;
        }
        else
        {
            brzina = brzina1 + brzina1 * 120 * Time.deltaTime;
        }
    }
    public void Padanje()
    {
        if (kretanje)
        {
            if (klonovi.Count > 0)
            {
                klonovi[0].transform.position = new Vector2(klonovi[0].transform.position.x, klonovi[0].transform.position.y - brzina * Time.deltaTime);
            }
        }
    }
    public void TrenutnaBoja()
    {
        if (klonovi.Count > 0)
        {
            boja = klonovi[0].tag;
            trenutnaRiba = klonovi[0].GetComponent<SpriteRenderer>().sprite;
        }
    }
    public void SkorTekst()
    {
        poeni.text = "Score:" + skor.ToString();
        ukupanNovac.text = PlayerPrefs.GetInt("Novcici", 0).ToString() + "<sprite=0>";
        ukupniDijamant.text = PlayerPrefs.GetInt("Dijamanti", 0).ToString() + "<sprite=0>";

        if (Meni.kampanja)
        {
            if (strujaNoviNovac.GetComponent<Image>().sprite != nista && !noviNovac)
            {
                PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) + ((skor / skorZaNovcic) + bonusNovcici) * mnozilacNovcica);
                noviNovac = true;
            }
        }
        else
        {
            if (strujaNoviNovac.GetComponent<Image>().sprite != nista && !noviNovac)
            {
                PlayerPrefs.SetInt("Dijamanti", PlayerPrefs.GetInt("Dijamanti", 0) + (skor / 20));
                noviNovac = true;
            }
        }
    }
    public void ProveraPoraza()
    {
        if (!game_over)
        {
            if (Kraj.kraj == true && ekstra == 0 && !odbrana)
            {
                crni.transform.position = klonovi[0].transform.position;
                crni.Play();
                Destroy(klonovi[0]);
                klonovi.RemoveAt(0);
                Kraj.uZoni = false;

                if (Meni.kampanja)
                {
                    LobanjaTotem();
                }
                else
                {
                    Poraz();
                    game_over = true;
                }
            }
            else if(Kraj.kraj && odbrana)
            {
                crni.transform.position = klonovi[0].transform.position;
                crni.Play();
                Destroy(klonovi[0]);
                klonovi.RemoveAt(0);
                Kraj.uZoni = false;
                Kraj.kraj = false;
                Kraj.dno = false;
            }
            else
            {
                EkstraZivot();
            }
        }
    }
    public void LobanjaTotem()
    {
        lobanjaTotem = true;
        if (PlayerPrefs.GetInt("lobanja") == 1)
        {
            int r = UnityEngine.Random.Range(0, 5);
            if (r == 4)
            {
                lobanja.GetComponent<Animator>().SetTrigger("lobanja");

                StartCoroutine(lobanjaPauza());
            }
            else
            {
                lobanja.GetComponent<Animator>().SetTrigger("lobanjaNe");

                StartCoroutine(lobanjaPauzaNe());
            }
        }
        else
        {
            Poraz();
            game_over = true;
        }
    }
    public IEnumerator lobanjaPauza()
    {
        yield return new WaitForSeconds(2);
        crveni.transform.position=new Vector2(0,0);
        crveni.Play();
        yield return new WaitForSeconds(1.5f);
        Kraj.kraj = false;
        Kraj.uZoni = false;
        Kraj.dno = false;
        lobanjaTotem = false;
        Stvaranje();
    }
    public IEnumerator lobanjaPauzaNe()
    {
        yield return new WaitForSeconds(3f);
        Poraz();
        game_over = true;
    }
    public void Poraz()
    {
        skip.SetActive(true);
        hobotnica.GetComponent<Animator>().SetTrigger("kraj");
        podmornica.SetTrigger("kraj");
        mastilo.SetTrigger("kraj");
        strujaNoviNovac.GetComponent<Animator>().SetTrigger("struja");
        strujaPozadina.SetActive(true);
        plavoPozadina.SetActive(true);
        strujaPozadina.GetComponent<Animator>().SetTrigger("struja");
        plavoPozadina.GetComponent<Animator>().SetTrigger("struja");

        if (Meni.kampanja)
        {
            if (skor > PlayerPrefs.GetInt("HighScore4Cm", 0))
            {
                PlayerPrefs.SetInt("HighScore4Cm", skor);
                highScore.text = "New High Score !!!";
                StartCoroutine(Vatromet());
            }

            if (skor >= 100 && PlayerPrefs.GetInt("Level", 1) == 4)
            {
                PlayerPrefs.SetInt("Level", 5);

                PlayerPrefs.SetInt("plavi", 0);
                PlayerPrefs.SetInt("ljubicasti", 0);
                PlayerPrefs.SetInt("crveni", 0);
                PlayerPrefs.SetInt("zuti", 0);
                PlayerPrefs.SetInt("narandzasti", 0);
                PlayerPrefs.SetInt("zeleni", 0);

                PlayerPrefs.SetInt("startScore", 0);
                PlayerPrefs.SetFloat("speedChange", 6);
                PlayerPrefs.SetInt("scoreForCoin", 10);
                PlayerPrefs.SetInt("colorShield", 0);
                PlayerPrefs.SetInt("hotStreak", 15);

                PlayerPrefs.SetInt("Novcici", 0);
            }
        }
        else
        {
            if (skor > PlayerPrefs.GetInt("HighScore4Cl", 0))
            {
                PlayerPrefs.SetInt("HighScore4Cl", skor);
                highScore.text = "New High Score !!!";
                StartCoroutine(Vatromet());
            }
        }
    }
    public IEnumerator Pocetak()
    {
        yield return new WaitForSeconds(0.1f);
        tranzicijaAnimator.SetBool("tranzicija1", true);
        yield return new WaitForSeconds(4f);
        Stvaranje();
    }
    public void Stvaranje()
    {
        if (!Kraj.kraj)
        {
            odbrana = false;

            if (PlayerPrefs.GetInt("hobotnica", 0) == 1 && jednobojno == false && Meni.kampanja)
            {
                int randomZaJednobojno = UnityEngine.Random.Range(0, 10);
                if (randomZaJednobojno == 0)
                {
                    StartCoroutine(Jednobojno());
                }
            }

            if (PlayerPrefs.GetInt("zlatnaRibica", 0) == 1 && goldRush == false && Meni.kampanja)
            {
                int randomZaGoldRush = UnityEngine.Random.Range(0, 15);
                if (randomZaGoldRush == 0)
                {
                    StartCoroutine(GoldRush());
                }
            }

            int oblik = UnityEngine.Random.Range(0, 10);

            if (oblik < 7)
            {
                int r; 
                if(jednobojno)
                {
                    r= bojaJednobojna;
                }
                else
                {
                    r = UnityEngine.Random.Range(0, kvadrat.Count);
                }

                if(Meni.kampanja)
                {
                    if (indeksiOdbrane.Contains(r))
                    {
                        odbrana = true;
                    }
                }

                float x = UnityEngine.Random.Range(zidL.transform.position.x + 0.5f, zidR.transform.position.x - 0.5f);
                float y = UnityEngine.Random.Range(6f, 8f);
                GameObject klon = Instantiate(kvadrat[r], new Vector2(x, y), Quaternion.identity);
                klonovi.Add(klon);
                if(goldRush)
                {
                    GameObject ivica=klon.transform.GetChild(0).gameObject;
                    ivica.GetComponent<SpriteRenderer>().enabled = true;
                }
                StartCoroutine(Cekanje());
            }
            if (oblik >= 7 && oblik < 9)
            {
                int r;
                if (jednobojno)
                {
                    r = bojaJednobojna;
                }
                else
                {
                    r = UnityEngine.Random.Range(0, trougao.Count);
                }

                if (Meni.kampanja)
                {
                    if (indeksiOdbrane.Contains(r))
                    {
                        odbrana = true;
                    }
                }

                float x = UnityEngine.Random.Range(zidL.transform.position.x + 0.5f, zidR.transform.position.x - 0.5f);
                float y = UnityEngine.Random.Range(6f, 8f);
                GameObject klon = Instantiate(trougao[r], new Vector2(x, y), Quaternion.identity);
                klonovi.Add(klon);
                if (goldRush)
                {
                    GameObject ivica = klon.transform.GetChild(0).gameObject;
                    ivica.GetComponent<SpriteRenderer>().enabled = true;
                }
                StartCoroutine(Cekanje());
            }
            if (oblik == 9)
            {
                int r = UnityEngine.Random.Range(0, krug.Count);
                float x = UnityEngine.Random.Range(zidL.transform.position.x + 0.5f, zidR.transform.position.x - 0.5f);
                float y = UnityEngine.Random.Range(6f, 8f);
                GameObject klon = Instantiate(krug[r], new Vector2(x, y), Quaternion.identity);
                klonovi.Add(klon);
                StartCoroutine(Cekanje());
            }
            klonovi[0].transform.Rotate(0, 0, 90);
            EfekatRibice();

            if (klonovi.Count > 0)
            {
                udica.transform.position = new Vector2(klonovi[0].transform.position.x, udica.transform.position.y);
            }

            vektorUdica = udica.transform.position;

            int k = UnityEngine.Random.Range(0, 15);
            if (k == 0 && Meni.kampanja && kraba.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("New State"))
            {
                if(PlayerPrefs.GetInt("kraba",0)==1)
                {
                    krabaKlik = false;
                    kraba.GetComponent<Animator>().speed=1;
                    krabinNovcic.GetComponent<Image>().enabled = true;

                    int randomZaKrabu = UnityEngine.Random.Range(0, 7);
                    if (randomZaKrabu == 0)
                    {
                        velikiKrabinNovcic.SetActive(true);
                        bezi=true;
                        kraba.GetComponent<Animator>().SetTrigger("bezi");
                        krabinNovcic.GetComponent<Image>().enabled=false;
                    }
                    else
                    {
                        velikiKrabinNovcic.SetActive(false);
                        bezi=false;
                        kraba.GetComponent<Animator>().SetTrigger("kraba");
                    }
                }
                else
                {
                    kraba.GetComponent<Animator>().SetTrigger("kraba");
                }
            }

            int p = UnityEngine.Random.Range(0, 5);
            if (p == 2)
            {
                strujaMeduza.GetComponent<Animator>().SetTrigger("struja");
            }

            int j = UnityEngine.Random.Range(0, 30);
            if (j == 8)
            {
                ajkula.GetComponent<Animator>().SetTrigger("ajkula");
            }
        }
    }
    public IEnumerator Jednobojno()
    {
        bojaJednobojna=UnityEngine.Random.Range(0, 3);
        switch (bojaJednobojna)
        {
            case 0:
                plavoDugmeObjekat.GetComponent<Image>().sprite = crvenaSkoljka;
                plavoDugmeObjekat.GetComponent<Animator>().runtimeAnimatorController=crvenoDugmeAnimator;
                zutoDugmeObjekat.GetComponent<Image>().sprite = crvenaSkoljka;
                zutoDugmeObjekat.GetComponent<Animator>().runtimeAnimatorController=crvenoDugmeAnimator;
                break;
            case 1:
                crvenoDugmeObjekat.GetComponent<Image>().sprite = plavaSkoljka;
                crvenoDugmeObjekat.GetComponent<Animator>().runtimeAnimatorController=plavoDugmeAnimator;
                zutoDugmeObjekat.GetComponent<Image>().sprite = plavaSkoljka;
                zutoDugmeObjekat.GetComponent<Animator>().runtimeAnimatorController= plavoDugmeAnimator;
                break;
            case 2:
                crvenoDugmeObjekat.GetComponent<Image>().sprite = zutaSkoljka;
                crvenoDugmeObjekat.GetComponent<Animator>().runtimeAnimatorController= zutoDugmeAnimator;
                plavoDugmeObjekat.GetComponent<Image>().sprite = zutaSkoljka;
                plavoDugmeObjekat.GetComponent<Animator>().runtimeAnimatorController= zutoDugmeAnimator;
                break;
        }
        jednobojno = true;

        yield return new WaitForSeconds(10f);

        crvenoDugmeObjekat.GetComponent<Image>().sprite = crvenaSkoljka;
        crvenoDugmeObjekat.GetComponent<Animator>().runtimeAnimatorController=crvenoDugmeAnimator;
        plavoDugmeObjekat.GetComponent<Image>().sprite = plavaSkoljka;
        plavoDugmeObjekat.GetComponent<Animator>().runtimeAnimatorController=plavoDugmeAnimator;
        zutoDugmeObjekat.GetComponent<Image>().sprite = zutaSkoljka;
        zutoDugmeObjekat.GetComponent<Animator>().runtimeAnimatorController= zutoDugmeAnimator;

        jednobojno = false;
    }
    public IEnumerator GoldRush()
    {
        zlatnaRibica.SetTrigger("zlato");
        goldRush = true;
        yield return new WaitForSeconds(10f);
        goldRush = false;
    }
    public IEnumerator Cekanje()
    {
        yield return new WaitForSeconds(0.5f);
        if (klonovi.Count == 0)
        {
            yield return new WaitForSeconds(1f);
            Stvaranje();
        }
        else
            StartCoroutine(Cekanje());
    }
    public void klikZuto()
    {
        int r = UnityEngine.Random.Range(0, 9);
        if(r==8)
        {
            mehuriSkoljkaZuto.Play();
        }

        if (klonovi.Count > 0)
        {
            if (Kraj.uZoni)
            {
                if (boja == "zuto" && klonovi.Count > 0 && !Kraj.kraj)
                {
                    moze = true;
                    kretanje = false;

                    if (!partiklaJednom)
                    {
                        zuti.transform.position = klonovi[0].transform.position;
                        zuti.Play();
                        partiklaJednom= true;
                        if(goldRush)
                        {
                            PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) + 1);
                        }
                        skor++;
                    }
                }
                else if ((boja == "narandzasto" || boja == "zeleno" || boja == "krug") && klonovi.Count > 0 && !Kraj.kraj)
                {

                }
                else
                {
                    Kraj.kraj = true;
                }

                if (moc == 0 && boja == "krug")
                {
                    moc++;
                }
            }
        }
    }
    public void klikCrveno()
    {
        int r = UnityEngine.Random.Range(0, 9);
        if (r == 8)
        {
            mehuriSkoljkaCrveno.Play();
        }

        if (klonovi.Count > 0)
        {
            if (Kraj.uZoni)
            {
                if (boja == "crveno" && klonovi.Count > 0 && !Kraj.kraj)
                {
                    moze = true;
                    kretanje = false;

                    if (!partiklaJednom)
                    {
                        crveni.transform.position = klonovi[0].transform.position;
                        crveni.Play();
                        partiklaJednom= true;
                        if(goldRush)
                        {
                            PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) + 1);
                        }
                        skor++;
                    }
                }
                else if ((boja == "narandzasto" || boja == "ljubicasto" || boja == "krug") && klonovi.Count > 0 && !Kraj.kraj)
                {

                }
                else
                {
                    Kraj.kraj = true;
                }

                if (moc == 1 && boja == "krug")
                {
                    moc++;
                }
            }
        }
    }
    public void klikPlavo()
    {
        int r = UnityEngine.Random.Range(0, 9);
        if (r == 8)
        {
            mehuriSkoljkaPlavo.Play();
        }

        if (klonovi.Count > 0)
        {
            if (Kraj.uZoni)
            {
                if (boja == "plavo" && klonovi.Count > 0 && !Kraj.kraj)
                {
                    moze = true;
                    kretanje = false;

                    if (!partiklaJednom)
                    {
                        plavi.transform.position = klonovi[0].transform.position;
                        plavi.Play();
                        partiklaJednom = true;
                        if (goldRush)
                        {
                            PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) + 1);
                        }
                        skor++;
                    }
                }
                else if ((boja == "ljubicasto" || boja == "zeleno" || boja == "krug") && klonovi.Count > 0 && !Kraj.kraj)
                {

                }
                else
                {
                    Kraj.kraj = true;
                }

                if (moc == 2 && boja == "krug")
                {
                    moc++;
                }
            }
        }
    }
    public void PlavoDown()
    {
        plavo = true;
        plavoDugme.SetBool("plavo", true);
        plavoDugme.SetBool("zatvori",true);
    }
    public void PlavoUp()
    {
        plavo = false;
        plavoDugme.SetBool("plavo", false);
        plavoDugme.SetBool("zatvori", false);
    }
    public void CrvenoDown()
    {
        crveno = true;
        crvenoDugme.SetBool("crveno", true);
        crvenoDugme.SetBool("zatvori", true);
    }
    public void CrvenoUp()
    {
        crveno = false;
        crvenoDugme.SetBool("crveno", false);
        crvenoDugme.SetBool("zatvori", false);
    }
    public void ZutoDown()
    {
        zuto = true;
        zutoDugme.SetBool("zuto", true);
        zutoDugme.SetBool("zatvori", true);
    }
    public void ZutoUp()
    {
        zuto = false;
        zutoDugme.SetBool("zuto", false);
        zutoDugme.SetBool("zatvori", false);
    }
    public void SareneBoje()
    {
        if (klonovi.Count > 0 && Kraj.uZoni)
        {
            if (boja == "narandzasto" && klonovi.Count > 0 && !Kraj.kraj && zuto && crveno && !plavo)
            {
                moze= true;
                kretanje = false;

                if (!partiklaJednom)
                {
                    narandzasti.transform.position = klonovi[0].transform.position;
                    narandzasti.Play();
                    partiklaJednom= true;
                    if(goldRush)
                    {
                        PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) + 1);
                    }
                    skor++;
                }
            }

            if (boja == "ljubicasto" && klonovi.Count > 0 && !Kraj.kraj && !zuto && crveno && plavo)
            {
                moze = true;
                kretanje = false;

                if (!partiklaJednom)
                {
                    ljubicasti.transform.position = klonovi[0].transform.position;
                    ljubicasti.Play();
                    partiklaJednom= true;
                    if(goldRush)
                    {
                        PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) + 1);
                    }
                    skor++;
                }
            }

            if (boja == "zeleno" && klonovi.Count > 0 && !Kraj.kraj && zuto && !crveno && plavo)
            {
                moze = true;
                kretanje = false;

                if (!partiklaJednom)
                {
                    zeleni.transform.position = klonovi[0].transform.position;
                    zeleni.Play();
                    partiklaJednom= true;
                    if(goldRush)
                    {
                        PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) + 1);
                    }
                    skor++;
                }
            }

            if (boja == "krug" && klonovi.Count > 0 && !Kraj.kraj && moc == 3)
            {
                ekstra++;
                moze= true;
                kretanje = false;

                if (!partiklaJednom)
                {
                    crveni.transform.position = klonovi[0].transform.position;
                    crveni.Play();
                    partiklaJednom= true;
                }
                moc = 0;
            }
        }
    }
    public void Trougao()
    {
        if (boja == "trougao" && klonovi.Count > 0)
        {
            if (Kraj.dno && !Kraj.kraj)
            {
                moze = true;
                kretanje = false;

                if (!partiklaJednom)
                {
                    if (klonovi[0].name == "PlavaTrougaoRiba(Clone)")
                    {
                        plavi.transform.position = klonovi[0].transform.position;
                        plavi.Play();
                    }
                    if (klonovi[0].name == "CrvenaTrougaoRiba(Clone)")
                    {
                        crveni.transform.position = klonovi[0].transform.position;
                        crveni.Play();
                    }
                    if (klonovi[0].name == "ZutaTrougaoRiba(Clone)")
                    {
                        zuti.transform.position = klonovi[0].transform.position;
                        zuti.Play();
                    }
                    if (klonovi[0].name == "LjubicastaTrougaoRiba(Clone)" || klonovi[0].name == "CrvenoPlavaTrougaoRiba(Clone)" || klonovi[0].name == "PlavoCrvenaTrougaoRiba(Clone)")
                    {
                        ljubicasti.transform.position = klonovi[0].transform.position;
                        ljubicasti.Play();
                    }
                    if (klonovi[0].name == "ZelenaTrougaoRiba(Clone)" || klonovi[0].name == "ZutoPlavaTrougaoRiba(Clone)" || klonovi[0].name == "PlavoZutaTrougaoRiba(Clone)")
                    {
                        zeleni.transform.position = klonovi[0].transform.position;
                        zeleni.Play();
                    }
                    if (klonovi[0].name == "NarandzastaTrougaoRiba(Clone)" || klonovi[0].name == "ZutoCrvenaTrougaoRiba(Clone)" || klonovi[0].name == "CrvenoZutaTrougaoRiba(Clone)")
                    {
                        narandzasti.transform.position = klonovi[0].transform.position;
                        narandzasti.Play();
                    }
                    partiklaJednom = true;
                    if (goldRush)
                    {
                        PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) + 1);
                    }
                    skor++;
                }
            }
        }
    }
    public void Krug()
    {
        if (boja == "krug" && klonovi.Count > 0)
        {
            if (Kraj.dno)
            {
                crni.transform.position = klonovi[0].transform.position;
                crni.Play();
  
                Destroy(klonovi[0]);
                klonovi.RemoveAt(0);
                Kraj.uZoni = false;
                Kraj.dno = false;
                moc = 0;
            }
        }
    }
    public void EkstraZivot()
    {
        ekstraZivot.GetComponentInChildren<Text>().text = ekstra.ToString();
        if (ekstra > 0)
        {
            ekstraZivot.SetActive(true);
        }
        else
        {
            ekstraZivot.SetActive(false);
        }
        if (Kraj.kraj && ekstra > 0)
        {
            crni.transform.position = klonovi[0].transform.position;
            crni.Play();

            Destroy(klonovi[0]);
            klonovi.RemoveAt(0);
            Kraj.kraj = false;
            Kraj.uZoni = false;
            Kraj.dno = false;
            ekstra--;
        }
    }
    public void UspehUdica()
    {
        if (moze)
        {
            if (!Udica.udica && klonovi.Count>0)
            {
                udica.transform.position = Vector3.MoveTowards(udica.transform.position,klonovi[0].transform.position,25f * Time.deltaTime);
            }
            else
            {
                udica.transform.position = Vector3.MoveTowards(udica.transform.position, vektorUdica, 25f*Time.deltaTime);
                ribaNaUdici.GetComponent<SpriteRenderer>().sprite = trenutnaRiba;

                if (klonovi.Count > 0)
                {
                    Destroy(klonovi[0]);
                    klonovi.RemoveAt(0);
                }
                Kraj.uZoni = false;
                Kraj.dno = false;
                Kraj.kraj = false;
            }
        }
        
        if(udica.transform.position.y>zidU.transform.position.y+1 && Udica.udica)
        {
            kretanje = true;
            moze= false;
            Udica.udica = false;
            partiklaJednom = false;
            udica.transform.position = vektorUdica;
            ribaNaUdici.GetComponent<SpriteRenderer>().sprite = nista;
        }
    }
    public void EfekatRibice()
    {
        int p=UnityEngine.Random.Range(0, 9);

        if (p == 8)
        {
            float r = UnityEngine.Random.Range(zidD.transform.position.y, zidU.transform.position.y);
            int k=UnityEngine.Random.Range(0, 12);

            if (k==0)
            {
                crveneRibice.transform.position = new Vector2(zidR.transform.position.x + 0.5f, r);
                crveneRibice.Play();
            }
            if (k==1)
            {
                plaveRibice.transform.position = new Vector2(zidR.transform.position.x + 0.5f, r);
                plaveRibice.Play();
            }
            if (k==2)
            {
                zuteRibice.transform.position = new Vector2(zidR.transform.position.x + 0.5f, r);
                zuteRibice.Play();
            }
            if (k==3)
            {
                narandzasteRibice.transform.position = new Vector2(zidR.transform.position.x + 0.5f, r);
                narandzasteRibice.Play();
            }
            if (k==4)
            {
                ljubicasteRibice.transform.position = new Vector2(zidR.transform.position.x + 0.5f, r);
                ljubicasteRibice.Play();
            }
            if (k==5)
            {
                zeleneRibice.transform.position = new Vector2(zidR.transform.position.x + 0.5f, r);
                zeleneRibice.Play();
            }
            if (k == 6)
            {
                crveneTrougaoRibice.transform.position = new Vector2(zidR.transform.position.x + 0.5f, r);
                crveneTrougaoRibice.Play();
            }
            if (k == 7)
            {
                plaveTrougaoRibice.transform.position = new Vector2(zidR.transform.position.x + 0.5f, r);
                plaveTrougaoRibice.Play();
            }
            if (k == 8)
            {
                zuteTrougaoRibice.transform.position = new Vector2(zidR.transform.position.x + 0.5f, r);
                zuteTrougaoRibice.Play();
            }
            if (k == 9)
            {
                narandzasteTrougaoRibice.transform.position = new Vector2(zidR.transform.position.x + 0.5f, r);
                narandzasteTrougaoRibice.Play();
            }
            if (k == 10)
            {
                ljubicasteTrougaoRibice.transform.position = new Vector2(zidR.transform.position.x + 0.5f, r);
                ljubicasteTrougaoRibice.Play();
            }
            if (k == 11)
            {
                zeleneTrougaoRibice.transform.position = new Vector2(zidR.transform.position.x + 0.5f, r);
                zeleneTrougaoRibice.Play();
            }
        }
    }
    public void KrabaKlik()
    {
        if (!krabaKlik && !bezi)
        {
            kraba.GetComponent<Animator>().speed = 2.5f;
            krabinNovcic.GetComponent<Image>().enabled=false;
            novcic.transform.position = kraba.transform.position;
            novcic.GetComponent<Animator>().SetTrigger("novcic");
            PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) + 1);
            krabaKlik = true;
        }
    }
    public IEnumerator Vatromet()
    {
        yield return new WaitForSeconds(2f);
        float x = UnityEngine.Random.Range(zidL.transform.position.x + 0.5f, zidR.transform.position.x - 0.5f);
        float y = UnityEngine.Random.Range(-2f, 4f);
        vatromet.transform.position = new Vector2(x, y);
        vatromet.Play();
        StartCoroutine(Vatromet());
    }

    public void Tranzicija()
    {
        if (tranzicijaAnimator.GetCurrentAnimatorStateInfo(0).IsName("kraj"))
        {
            tranzicija.SetActive(false);
        }
    }
    public void Skip()
    {
        hobotnica.SetActive(false);
        podmornica.SetTrigger("skip");
        plavoPozadina.SetActive(false);
        strujaPozadina.SetActive(false);
        strujaNoviNovac.SetActive(false);
        mastilo.SetTrigger("skip");
        if (!noviNovac)
        {
            if (Meni.kampanja)
            {
                PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) + ((skor / skorZaNovcic) + bonusNovcici) * mnozilacNovcica);
            }
            else
            {
                PlayerPrefs.SetInt("Dijamanti", PlayerPrefs.GetInt("Dijamanti", 0) + (skor / 20));
            }
            noviNovac = true;
        }
    }
    public void Restart()
    {
        Kraj.kraj = false;
        Kraj.uZoni = false;
        Kraj.dno = false;
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        StartCoroutine(restart());
    }
    public IEnumerator restart()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(5);
    }
    public void Menu()
    {
        Kraj.kraj = false;
        Kraj.uZoni = false;
        Kraj.dno = false;
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        StartCoroutine(meni());
    }
    public IEnumerator meni()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(0);
    }


    /////////////////////////////////////



    public void Tastatura()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            klikZuto();
            zuto = true;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            klikCrveno();
            crveno = true;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            klikPlavo();
            plavo = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            zuto = false;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            crveno = false;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            plavo = false;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            crni.transform.position = klonovi[0].transform.position;
            crni.Play();
            Destroy(klonovi[0]);
            klonovi.RemoveAt(0);
            Kraj.uZoni = false;
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
           
        }
    }
}
