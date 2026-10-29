using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level3 : MonoBehaviour
{
    public List<GameObject> ledeniKvadrat= new List<GameObject>();
    public List<GameObject> snezniKvadrat=new List<GameObject>();
    public List<GameObject> kvadrat = new List<GameObject>();
    public List<GameObject> ledeniTrougao = new List<GameObject>();
    public List<GameObject> snezniTrougao = new List<GameObject>();
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
    public ParticleSystem snezni;
    public ParticleSystem ledeni;
    public ParticleSystem snezniNaEkranu;
    public ParticleSystem ledeniNaEkranu;

    public GameObject zidL;
    public GameObject zidR;
    public GameObject ekstraZivot;
    public GameObject goodZone;
    public GameObject tranzicija;
    public GameObject snegEkran;
    public GameObject lavina;
    public GameObject sanke;
    public GameObject skip;
    public GameObject grudva;
    public GameObject ukupniNovcici;
    public GameObject ukupniDijamanti;

    public Animator tranzicijaAnimator;
    public Animator zaradjeniNovciciAnimator;
    public Animator zutoDugme;
    public Animator crvenoDugme;
    public Animator plavoDugme;
    public Animator pozadina;
    public Animator restartAnimator;
    public Animator meniAnimator;

    public Text noviNovac;
    public TextMeshProUGUI ukupanNovac;
    public TextMeshProUGUI ukupniDijamant;

    public Sprite narandzast;
    public Sprite snegNarandzast;
    public Sprite ljubicast;
    public Sprite snegLjubicast;
    public Sprite zelen;
    public Sprite snegZelen;
    public Sprite plav;
    public Sprite snegPlav;
    public Sprite zut;
    public Sprite snegZut;
    public Sprite crven;
    public Sprite snegCrven;
    public Sprite crvenTrougao;
    public Sprite snegCrvenTrougao;
    public Sprite plavTrougao;
    public Sprite snegPlavTrougao;
    public Sprite zutTrougao;
    public Sprite snegZutTrougao;
    public Sprite zelenTrougao;
    public Sprite snegZelenTrougao;
    public Sprite narandzastTrougao;
    public Sprite snegNarandzastTrougao;
    public Sprite ljubicastTrougao;
    public Sprite snegLjubicastTrougao;

    public Sprite nista;
    public Sprite sneg1;
    public Sprite sneg2;
    public Sprite sneg3;
    public Sprite sneg4;
    public Sprite sneg5;
    public Sprite sneg6;
    public Sprite sneg7;
    public Sprite sneg8;

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
    private int rot = 0;
    private bool sneg = false;
    private int snegNaEkranu = 0;
    private int slikaSnegaNaEkranu = 0;
    private bool noviNovacBio=false;

    private int bonusNovcici = 0;
    private int skorZaNovcic = 10;

    private bool duplo= false;
    public Animator sankeZnakAnimator;

    private void Start()
    {
        if (Meni.kampanja)
        {
            if (PlayerPrefs.GetInt("scoreForCoin", 10) < 1)
            {
                bonusNovcici = -PlayerPrefs.GetInt("scoreForCoin", 10) + 1;
                skorZaNovcic = 1;
            }
            else
            {
                skorZaNovcic = PlayerPrefs.GetInt("scoreForCoin", 10);
            }
        }

        if (Meni.kampanja)
        {
            highScore.text = "High Score - " + PlayerPrefs.GetInt("HighScore3Cm", 0);
            skor = PlayerPrefs.GetInt("startScore", 0);

            ukupniDijamanti.SetActive(false);
        }
        else
        {
            highScore.text = "High Score - " + PlayerPrefs.GetInt("HighScore3Cl", 0);

            ukupniNovcici.SetActive(false);
        }

        zutoDugme.transform.position = new Vector2((zidL.transform.position.x + crvenoDugme.transform.position.x) / 2 - 0.2f, zutoDugme.transform.position.y);
        plavoDugme.transform.position = new Vector2((zidR.transform.position.x + crvenoDugme.transform.position.x) / 2 + 0.2f, plavoDugme.transform.position.y);

        StartCoroutine(Pocetak());
        StartCoroutine(PromenaSnegaNaEkranu());

        RectTransform kanvasTransform = goodZone.GetComponent<RectTransform>();

        brzina1 = ((0.05f * kanvasTransform.rect.height) / 1870f) * 30;
    }
    public void Update()
    {
        TrenutnaBoja();

        Brzina();

        SkorTekst();

        ProveraPoraza();

        SareneBoje();

        Sneg();

        Led();

        SnegTrougao();

        LedTrougao();

        Trougao();

        TrougaoSneg();

        TrougaoLed();

        Krug();

        Padanje();

        Rotacija();

        Tranzicija();

        Tastatura();

        SlikaSnega();

        if(snegEkran.GetComponent<Image>().sprite==nista)
        {
            snegEkran.SetActive(false);
        }
        else
        {
            snegEkran.SetActive(true);
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
        if (klonovi.Count > 0)
        {
            klonovi[0].transform.position = new Vector2(klonovi[0].transform.position.x, klonovi[0].transform.position.y - brzina * Time.deltaTime);
        }
    }
    public void Rotacija()
    {
        if (klonovi.Count > 0)
        {
            if (rot == 1)
            {
                klonovi[0].transform.Rotate(new Vector3(0, 0, brzina * 30 * Time.deltaTime));
            }
            else
            {
                klonovi[0].transform.Rotate(new Vector3(0, 0, -brzina * 30 * Time.deltaTime));
            }
        }
    }
    public void TrenutnaBoja()
    {
        if (klonovi.Count > 0)
        {
            boja = klonovi[0].tag;
        }
    }
    public void SkorTekst()
    {
        poeni.text = "Score:" + skor.ToString();

        ukupanNovac.text = PlayerPrefs.GetInt("Novcici", 0).ToString() + "<sprite=0>";
        ukupniDijamant.text = PlayerPrefs.GetInt("Dijamanti", 0).ToString() + "<sprite=0>";
    }
    public void ProveraPoraza()
    {
        if (!game_over)
        {
            if (Kraj.kraj == true && ekstra == 0)
            {
                Poraz();
                game_over = true;
            }
            else
            {
                EkstraZivot();
            }
        }
    }
    public void Poraz()
    {
        skip.SetActive(true);

        if (Meni.kampanja)
        {
            if (skor > PlayerPrefs.GetInt("HighScore3Cm", 0))
            {
                PlayerPrefs.SetInt("HighScore3Cm", skor);
                highScore.text = "New High Score !!!";
                StartCoroutine(Vatromet());
            }

            if (skor >= 100 && PlayerPrefs.GetInt("Level", 1) == 3)
            {
                PlayerPrefs.SetInt("Level", 4);

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
            if (skor > PlayerPrefs.GetInt("HighScore3Cl", 0))
            {
                PlayerPrefs.SetInt("HighScore3Cl", skor);
                highScore.text = "New High Score !!!";
                StartCoroutine(Vatromet());
            }
        }

        if (Meni.kampanja)
        {
            noviNovac.text = "+" + ((skor / skorZaNovcic) + bonusNovcici);
        }
        else
        {
            noviNovac.text = "+"+(skor/20);
        }

        crni.transform.position = klonovi[0].transform.position;
        crni.Play();
        Destroy(klonovi[0]);
        klonovi.RemoveAt(0);
        Kraj.uZoni = false;
        lavina.GetComponent<Animator>().SetBool("kraj", true);
        pozadina.SetBool("kraj", true);
        sanke.GetComponent<Animator>().SetBool("kraj", true);
        restartAnimator.SetBool("kraj", true);
        meniAnimator.SetBool("kraj", true);

        if (Meni.kampanja)
        {
            StartCoroutine(Novcici());
        }
        else
        {
            StartCoroutine(Dijamanti());
        }
    }
    public IEnumerator Novcici()
    {
        yield return new WaitForSeconds(2.5f);
        zaradjeniNovciciAnimator.SetBool("kraj", true);
        yield return new WaitForSeconds(2f);
        if (!noviNovacBio)
        {
            PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) + (skor / skorZaNovcic)+bonusNovcici);
            noviNovacBio= true;
        }
    }
    public IEnumerator Dijamanti()
    {
        yield return new WaitForSeconds(2.5f);
        zaradjeniNovciciAnimator.SetBool("kraj", true);
        yield return new WaitForSeconds(2f);
        if (!noviNovacBio)
        {
            PlayerPrefs.SetInt("Dijamanti", PlayerPrefs.GetInt("Dijamanti", 0) + (skor / 20));
            noviNovacBio = true;
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
            if(PlayerPrefs.GetInt("poklon",0)==1 && duplo==false && Meni.kampanja)
            {
                int randomZaDuplo = Random.Range(0, 10);
                if(randomZaDuplo==0)
                {
                    StartCoroutine(Duplo());
                }
            }

            int oblik = Random.Range(0, 10);

            if (oblik < 6)
            {
                rot = Random.Range(1, 3);
                float x = Random.Range(zidL.transform.position.x + 0.5f, zidR.transform.position.x - 0.5f);
                float y = Random.Range(6f, 8f);

                int kv = 0;
                if(PlayerPrefs.GetInt("kramp", 0)==0 || !Meni.kampanja)
                {
                    kv = Random.Range(0, 9);
                }
                else
                {
                    if(PlayerPrefs.GetInt("lopata",0)==0)
                    {
                        kv = Random.Range(0, 7);
                    }
                    else
                    {
                        kv = Random.Range(0, 4);
                    }
                }
                int r;

                GameObject klon;

                if (kv < 4)
                {
                    r = Random.Range(0, kvadrat.Count);
                    klon = Instantiate(kvadrat[r], new Vector2(x, y), Quaternion.identity);
                }
                else if(kv<7)
                {
                    r = Random.Range(0, snezniKvadrat.Count);
                    klon = Instantiate(snezniKvadrat[r], new Vector2(x, y), Quaternion.identity);
                }
                else
                {
                    r = Random.Range(0, ledeniKvadrat.Count);
                    klon = Instantiate(ledeniKvadrat[r], new Vector2(x, y), Quaternion.identity);
                }

                klonovi.Add(klon);
                StartCoroutine(Cekanje());
            }
            if (oblik >= 6 && oblik < 9)
            {
                rot = Random.Range(1, 3);
                float x = Random.Range(zidL.transform.position.x + 0.5f, zidR.transform.position.x - 0.5f);
                float y = Random.Range(6f, 8f);

                int kv = 0;
                if (PlayerPrefs.GetInt("kramp", 0) == 0 || !Meni.kampanja)
                {
                    kv = Random.Range(0, 9);
                }
                else
                {
                    if (PlayerPrefs.GetInt("lopata", 0) == 0)
                    {
                        kv = Random.Range(0, 7);
                    }
                    else
                    {
                        kv = Random.Range(0, 4);
                    }
                }
                int r;

                GameObject klon;

                if (kv < 4)
                {
                    r = Random.Range(0, trougao.Count);
                    klon = Instantiate(trougao[r], new Vector2(x, y), Quaternion.identity);
                }
                else if (kv < 7)
                {
                    r = Random.Range(0, snezniTrougao.Count);
                    klon = Instantiate(snezniTrougao[r], new Vector2(x, y), Quaternion.identity);
                }
                else
                {
                    r = Random.Range(0, ledeniTrougao.Count);
                    klon = Instantiate(ledeniTrougao[r], new Vector2(x, y), Quaternion.identity);
                }

                klonovi.Add(klon);
                StartCoroutine(Cekanje());
            }
            if (oblik == 9)
            {
                int r = Random.Range(0, krug.Count);
                rot = Random.Range(1, 3);
                float x = Random.Range(zidL.transform.position.x + 0.5f, zidR.transform.position.x - 0.5f);
                float y = Random.Range(6f, 8f);
                GameObject klon = Instantiate(krug[r], new Vector2(x, y), Quaternion.identity);
                klonovi.Add(klon);
                StartCoroutine(Cekanje());
            }
        }
    }
    public IEnumerator Duplo()
    {
        sankeZnakAnimator.SetTrigger("sanke");
        duplo= true;
        yield return new WaitForSeconds(10f);
        sankeZnakAnimator.SetTrigger("kraj");
        duplo= false;
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
        if (!sneg && klonovi.Count > 0)
        {
            if (Kraj.uZoni)
            {
                if (boja == "zuto" && klonovi.Count > 0 && !Kraj.kraj)
                {
                    if(duplo)
                    {
                        skor++;
                    }
                    skor++;
                    zuti.transform.position = klonovi[0].transform.position;
                    zuti.Play();
                    Destroy(klonovi[0]);
                    klonovi.RemoveAt(0);
                    Kraj.uZoni = false;
                }
                else if ((boja == "narandzasto" || boja == "zeleno" || boja == "krug" || boja== "snegZuto" || boja == "ledZuto" || boja == "snegNarandzasto" || boja == "ledNarandzasto" || boja == "snegZeleno" || boja == "ledZeleno" || boja == "snegZutoTrougao" || boja == "ledZutoTrougao" || boja == "snegNarandzastoTrougao" || boja == "ledNarandzastoTrougao" || boja == "snegZelenoTrougao" || boja == "ledZelenoTrougao") && klonovi.Count > 0 && !Kraj.kraj)
                {

                }
                else
                {
                    if (PlayerPrefs.GetInt("snesko", 0) == 0 || klonovi[0].tag != "trougao" || !Meni.kampanja)
                    {
                        Kraj.kraj = true;
                    }
                    else
                    {
                        crni.transform.position = klonovi[0].transform.position;
                        crni.Play();
                        Destroy(klonovi[0]);
                        klonovi.RemoveAt(0);
                        Kraj.uZoni = false;
                        Kraj.dno = false;
                    }
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
        if (!sneg && klonovi.Count > 0)
        {
            if (Kraj.uZoni)
            {
                if (boja == "crveno" && klonovi.Count > 0 && !Kraj.kraj)
                {
                    if(duplo)
                    {
                        skor++;
                    }
                    skor++;
                    crveni.transform.position = klonovi[0].transform.position;
                    crveni.Play();
                    Destroy(klonovi[0]);
                    klonovi.RemoveAt(0);
                    Kraj.uZoni = false;
                }
                else if ((boja == "narandzasto" || boja == "ljubicasto" || boja == "krug" || boja == "snegCrveno" || boja == "ledCrveno" || boja == "snegNarandzasto" || boja == "ledNarandzasto" || boja == "snegLjubicasto" || boja == "ledLjubicasto" || boja == "snegCrvenoTrougao" || boja == "ledCrvenoTrougao" || boja == "snegNarandzastoTrougao" || boja == "ledNarandzastoTrougao" || boja == "snegLjubicastoTrougao" || boja == "ledLjubicastoTrougao") && klonovi.Count > 0 && !Kraj.kraj)
                {

                }
                else
                {
                    if (PlayerPrefs.GetInt("snesko", 0) == 0 || klonovi[0].tag!="trougao" || !Meni.kampanja)
                    {
                        Kraj.kraj = true;
                    }
                    else
                    {
                        crni.transform.position = klonovi[0].transform.position;
                        crni.Play();
                        Destroy(klonovi[0]);
                        klonovi.RemoveAt(0);
                        Kraj.uZoni = false;
                        Kraj.dno = false;
                    }
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
        if (!sneg && klonovi.Count > 0)
        {
            if (Kraj.uZoni)
            {
                if (boja == "plavo" && klonovi.Count > 0 && !Kraj.kraj)
                {
                    if(duplo)
                    {
                        skor++;
                    }
                    skor++;
                    plavi.transform.position = klonovi[0].transform.position;
                    plavi.Play();
                    Destroy(klonovi[0]);
                    klonovi.RemoveAt(0);
                    Kraj.uZoni = false;
                }
                else if ((boja == "ljubicasto" || boja == "zeleno" || boja == "krug" || boja == "snegPlavo" || boja == "ledPlavo" || boja == "snegLjubicasto" || boja == "ledLjubicasto" || boja == "snegZeleno" || boja == "ledZeleno" || boja == "snegPlavoTrougao" || boja == "ledPlavoTrougao" || boja == "snegLjubicastoTrougao" || boja == "ledLjubicastoTrougao" || boja == "snegZelenoTrougao" || boja == "ledZelenoTrougao") && klonovi.Count > 0 && !Kraj.kraj)
                {

                }
                else
                {
                    if (PlayerPrefs.GetInt("snesko", 0) == 0 || klonovi[0].tag != "trougao" || !Meni.kampanja)
                    {
                        Kraj.kraj = true;
                    }
                    else
                    {
                        crni.transform.position = klonovi[0].transform.position;
                        crni.Play();
                        Destroy(klonovi[0]);
                        klonovi.RemoveAt(0);
                        Kraj.uZoni = false;
                        Kraj.dno = false;
                    }
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
        sneg= false;
        plavo = true;
        plavoDugme.SetBool("plavo", true);
    }
    public void PlavoUp()
    {
        plavo = false;
        plavoDugme.SetBool("plavo", false);
    }
    public void CrvenoDown()
    {
        sneg= false;
        crveno = true;
        crvenoDugme.SetBool("crveno", true);
    }
    public void CrvenoUp()
    {
        crveno = false;
        crvenoDugme.SetBool("crveno", false);
    }
    public void ZutoDown()
    {
        sneg= false;
        zuto = true;
        zutoDugme.SetBool("zuto", true);
    }
    public void ZutoUp()
    {
        zuto = false;
        zutoDugme.SetBool("zuto", false);
    }
    public void Sneg()
    {
        if (boja == "snegNarandzasto" && klonovi.Count > 0 && !Kraj.kraj && zuto && crveno && !plavo)
        {
            sneg= true;
            klonovi[0].GetComponent<SpriteRenderer>().sprite = narandzast;
            zuto= false;
            crveno= false;
            plavo= false;
            klonovi[0].tag = "narandzasto";
            snezni.transform.position = klonovi[0].transform.position;
            snezni.Play();
        }
        if (boja == "snegLjubicasto" && klonovi.Count > 0 && !Kraj.kraj && !zuto && crveno && plavo)
        {
            sneg= true;
            klonovi[0].GetComponent<SpriteRenderer>().sprite = ljubicast;
            zuto = false;
            crveno = false;
            plavo = false;
            klonovi[0].tag = "ljubicasto";
            snezni.transform.position = klonovi[0].transform.position;
            snezni.Play();
        }
        if (boja == "snegZeleno" && klonovi.Count > 0 && !Kraj.kraj && zuto && !crveno && plavo)
        {
            sneg= true;
            klonovi[0].GetComponent<SpriteRenderer>().sprite = zelen;
            zuto = false;
            crveno = false;
            plavo = false;
            klonovi[0].tag = "zeleno";
            snezni.transform.position = klonovi[0].transform.position;
            snezni.Play();
        }
        if (boja == "snegCrveno" && klonovi.Count > 0 && !Kraj.kraj && !zuto && crveno && !plavo)
        {
            sneg= true;
            klonovi[0].GetComponent<SpriteRenderer>().sprite = crven;
            zuto = false;
            crveno = false;
            plavo = false;
            klonovi[0].tag = "crveno";
            snezni.transform.position = klonovi[0].transform.position;
            snezni.Play();
        }
        if (boja == "snegPlavo" && klonovi.Count > 0 && !Kraj.kraj && !zuto && !crveno && plavo)
        {
            sneg= true;
            klonovi[0].GetComponent<SpriteRenderer>().sprite = plav;
            zuto = false;
            crveno = false;
            plavo = false;
            klonovi[0].tag = "plavo";
            snezni.transform.position = klonovi[0].transform.position;
            snezni.Play();
        }
        if (boja == "snegZuto" && klonovi.Count > 0 && !Kraj.kraj && zuto && !crveno && !plavo)
        {
            sneg = true;
            klonovi[0].GetComponent<SpriteRenderer>().sprite = zut;
            zuto = false;
            crveno = false;
            plavo = false;
            klonovi[0].tag = "zuto";
            snezni.transform.position = klonovi[0].transform.position;
            snezni.Play();
        }
    }
    public void Led()
    {
        if (boja == "ledZuto" && klonovi.Count > 0 && !Kraj.kraj && zuto && !crveno && !plavo)
        {
            klonovi[0].GetComponent<SpriteRenderer>().sprite = snegZut;
            zuto = false;
            crveno = false;
            plavo = false;
            klonovi[0].tag = "snegZuto";
            ledeni.transform.position = klonovi[0].transform.position;
            ledeni.Play();
        }
        if (boja == "ledCrveno" && klonovi.Count > 0 && !Kraj.kraj && !zuto && crveno && !plavo)
        {
            klonovi[0].GetComponent<SpriteRenderer>().sprite = snegCrven;
            zuto = false;
            crveno = false;
            plavo = false;
            klonovi[0].tag = "snegCrveno";
            ledeni.transform.position = klonovi[0].transform.position;
            ledeni.Play();
        }
        if (boja == "ledPlavo" && klonovi.Count > 0 && !Kraj.kraj && !zuto && !crveno && plavo)
        {
            klonovi[0].GetComponent<SpriteRenderer>().sprite = snegPlav;
            zuto = false;
            crveno = false;
            plavo = false;
            klonovi[0].tag = "snegPlavo";
            ledeni.transform.position = klonovi[0].transform.position;
            ledeni.Play();
        }
        if (boja == "ledZeleno" && klonovi.Count > 0 && !Kraj.kraj && zuto && !crveno && plavo)
        {
            klonovi[0].GetComponent<SpriteRenderer>().sprite = snegZelen;
            zuto = false;
            crveno = false;
            plavo = false;
            klonovi[0].tag = "snegZeleno";
            ledeni.transform.position = klonovi[0].transform.position;
            ledeni.Play();
        }
        if (boja == "ledNarandzasto" && klonovi.Count > 0 && !Kraj.kraj && zuto && crveno && !plavo)
        {
            klonovi[0].GetComponent<SpriteRenderer>().sprite = snegNarandzast;
            zuto = false;
            crveno = false;
            plavo = false;
            klonovi[0].tag = "snegNarandzasto";
            ledeni.transform.position = klonovi[0].transform.position;
            ledeni.Play();
        }
        if (boja == "ledLjubicasto" && klonovi.Count > 0 && !Kraj.kraj && !zuto && crveno && plavo)
        {
            klonovi[0].GetComponent<SpriteRenderer>().sprite = snegLjubicast;
            zuto = false;
            crveno = false;
            plavo = false;
            klonovi[0].tag = "snegLjubicasto";
            ledeni.transform.position = klonovi[0].transform.position;
            ledeni.Play();
        }
    }
    public void SnegTrougao()
    {
        if (boja == "snegNarandzastoTrougao" && klonovi.Count > 0 && !Kraj.kraj && zuto && crveno && !plavo)
        {
            sneg = true;
            klonovi[0].GetComponent<SpriteRenderer>().sprite = narandzastTrougao;
            zuto = false;
            crveno = false;
            plavo = false;
            klonovi[0].tag = "trougao";
            snezni.transform.position = klonovi[0].transform.position;
            snezni.Play();
        }
        if (boja == "snegLjubicastoTrougao" && klonovi.Count > 0 && !Kraj.kraj && !zuto && crveno && plavo)
        {
            sneg = true;
            klonovi[0].GetComponent<SpriteRenderer>().sprite = ljubicastTrougao;
            zuto = false;
            crveno = false;
            plavo = false;
            klonovi[0].tag = "trougao";
            snezni.transform.position = klonovi[0].transform.position;
            snezni.Play();
        }
        if (boja == "snegZelenoTrougao" && klonovi.Count > 0 && !Kraj.kraj && zuto && !crveno && plavo)
        {
            sneg = true;
            klonovi[0].GetComponent<SpriteRenderer>().sprite = zelenTrougao;
            zuto = false;
            crveno = false;
            plavo = false;
            klonovi[0].tag = "trougao";
            snezni.transform.position = klonovi[0].transform.position;
            snezni.Play();
        }
        if (boja == "snegCrvenoTrougao" && klonovi.Count > 0 && !Kraj.kraj && !zuto && crveno && !plavo)
        {
            sneg = true;
            klonovi[0].GetComponent<SpriteRenderer>().sprite = crvenTrougao;
            zuto = false;
            crveno = false;
            plavo = false;
            klonovi[0].tag = "trougao";
            snezni.transform.position = klonovi[0].transform.position;
            snezni.Play();
        }
        if (boja == "snegPlavoTrougao" && klonovi.Count > 0 && !Kraj.kraj && !zuto && !crveno && plavo)
        {
            sneg = true;
            klonovi[0].GetComponent<SpriteRenderer>().sprite = plavTrougao;
            zuto = false;
            crveno = false;
            plavo = false;
            klonovi[0].tag = "trougao";
            snezni.transform.position = klonovi[0].transform.position;
            snezni.Play();
        }
        if (boja == "snegZutoTrougao" && klonovi.Count > 0 && !Kraj.kraj && zuto && !crveno && !plavo)
        {
            sneg = true;
            klonovi[0].GetComponent<SpriteRenderer>().sprite = zutTrougao;
            zuto = false;
            crveno = false;
            plavo = false;
            klonovi[0].tag = "trougao";
            snezni.transform.position = klonovi[0].transform.position;
            snezni.Play();
        }
    }
    public void LedTrougao()
    {
        if (boja == "ledZutoTrougao" && klonovi.Count > 0 && !Kraj.kraj && zuto && !crveno && !plavo)
        {
            klonovi[0].GetComponent<SpriteRenderer>().sprite = snegZutTrougao;
            zuto = false;
            crveno = false;
            plavo = false;
            klonovi[0].tag = "snegZutoTrougao";
            ledeni.transform.position = klonovi[0].transform.position;
            ledeni.Play();
        }
        if (boja == "ledCrvenoTrougao" && klonovi.Count > 0 && !Kraj.kraj && !zuto && crveno && !plavo)
        {
            klonovi[0].GetComponent<SpriteRenderer>().sprite = snegCrvenTrougao;
            zuto = false;
            crveno = false;
            plavo = false;
            klonovi[0].tag = "snegCrvenoTrougao";
            ledeni.transform.position = klonovi[0].transform.position;
            ledeni.Play();
        }
        if (boja == "ledPlavoTrougao" && klonovi.Count > 0 && !Kraj.kraj && !zuto && !crveno && plavo)
        {
            klonovi[0].GetComponent<SpriteRenderer>().sprite = snegPlavTrougao;
            zuto = false;
            crveno = false;
            plavo = false;
            klonovi[0].tag = "snegPlavoTrougao";
            ledeni.transform.position = klonovi[0].transform.position;
            ledeni.Play();
        }
        if (boja == "ledZelenoTrougao" && klonovi.Count > 0 && !Kraj.kraj && zuto && !crveno && plavo)
        {
            klonovi[0].GetComponent<SpriteRenderer>().sprite = snegZelenTrougao;
            zuto = false;
            crveno = false;
            plavo = false;
            klonovi[0].tag = "snegZelenoTrougao";
            ledeni.transform.position = klonovi[0].transform.position;
            ledeni.Play();
        }
        if (boja == "ledNarandzastoTrougao" && klonovi.Count > 0 && !Kraj.kraj && zuto && crveno && !plavo)
        {
            klonovi[0].GetComponent<SpriteRenderer>().sprite = snegNarandzastTrougao;
            zuto = false;
            crveno = false;
            plavo = false;
            klonovi[0].tag = "snegNarandzastoTrougao";
            ledeni.transform.position = klonovi[0].transform.position;
            ledeni.Play();
        }
        if (boja == "ledLjubicastoTrougao" && klonovi.Count > 0 && !Kraj.kraj && !zuto && crveno && plavo)
        {
            klonovi[0].GetComponent<SpriteRenderer>().sprite = snegLjubicastTrougao;
            zuto = false;
            crveno = false;
            plavo = false;
            klonovi[0].tag = "snegLjubicastoTrougao";
            ledeni.transform.position = klonovi[0].transform.position;
            ledeni.Play();
        }
    }
    public void SareneBoje()
    {
        if (klonovi.Count > 0 && Kraj.uZoni)
        {
            if (boja == "narandzasto" && klonovi.Count > 0 && !Kraj.kraj && zuto && crveno && !plavo)
            {
                if(duplo)
                {
                    skor++;
                }
                skor++;
                narandzasti.transform.position = klonovi[0].transform.position;
                narandzasti.Play();
                Destroy(klonovi[0]);
                klonovi.RemoveAt(0);
                Kraj.uZoni = false;
            }

            if (boja == "ljubicasto" && klonovi.Count > 0 && !Kraj.kraj && !zuto && crveno && plavo)
            {
                if (duplo)
                {
                    skor++;
                }
                skor++;
                ljubicasti.transform.position = klonovi[0].transform.position;
                ljubicasti.Play();
                Destroy(klonovi[0]);
                klonovi.RemoveAt(0);
                Kraj.uZoni = false;
            }

            if (boja == "zeleno" && klonovi.Count > 0 && !Kraj.kraj && zuto && !crveno && plavo)
            {
                if (duplo)
                {
                    skor++;
                }
                skor++;
                zeleni.transform.position = klonovi[0].transform.position;
                zeleni.Play();
                Destroy(klonovi[0]);
                klonovi.RemoveAt(0);
                Kraj.uZoni = false;
            }

            if (boja == "krug" && klonovi.Count > 0 && !Kraj.kraj && moc == 3)
            {

                if (klonovi[0].name == "EkstraZivot(Clone)")
                {
                    ekstra++;
                    crveni.transform.position = klonovi[0].transform.position;
                    crveni.Play();
                }

                Destroy(klonovi[0]);
                klonovi.RemoveAt(0);
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
                if (klonovi[0].GetComponent<SpriteRenderer>().sprite==plavTrougao)
                {
                    plavi.transform.position = klonovi[0].transform.position;
                    plavi.Play();
                }
                if (klonovi[0].GetComponent<SpriteRenderer>().sprite == crvenTrougao)
                {
                    crveni.transform.position = klonovi[0].transform.position;
                    crveni.Play();
                }
                if (klonovi[0].GetComponent<SpriteRenderer>().sprite == zutTrougao)
                {
                    zuti.transform.position = klonovi[0].transform.position;
                    zuti.Play();
                }
                if (klonovi[0].GetComponent<SpriteRenderer>().sprite == ljubicastTrougao)
                {
                    ljubicasti.transform.position = klonovi[0].transform.position;
                    ljubicasti.Play();
                }
                if (klonovi[0].GetComponent<SpriteRenderer>().sprite == zelenTrougao)
                {
                    zeleni.transform.position = klonovi[0].transform.position;
                    zeleni.Play();
                }
                if (klonovi[0].GetComponent<SpriteRenderer>().sprite == narandzastTrougao)
                {
                    narandzasti.transform.position = klonovi[0].transform.position;
                    narandzasti.Play();
                }

                Destroy(klonovi[0]);
                klonovi.RemoveAt(0);
                if(duplo)
                {
                    skor++;
                }
                skor++;
                Kraj.uZoni = false;
                Kraj.dno = false;
            }
        }
    }
    public void TrougaoSneg()
    {
        if ((boja == "snegPlavoTrougao" || boja == "snegCrvenoTrougao" || boja == "snegZutoTrougao" || boja == "snegLjubicastoTrougao" || boja == "snegNarandzastoTrougao" || boja == "snegZelenoTrougao") && klonovi.Count > 0)
        {
            if (Kraj.dno && !Kraj.kraj)
            {
                if (boja == "snegPlavoTrougao")
                {
                    plavi.transform.position = klonovi[0].transform.position;
                    plavi.Play();
                }
                if (boja == "snegCrvenoTrougao")
                {
                    crveni.transform.position = klonovi[0].transform.position;
                    crveni.Play();
                }
                if (boja == "snegZutoTrougao")
                {
                    zuti.transform.position = klonovi[0].transform.position;
                    zuti.Play();
                }
                if (boja == "snegLjubicastoTrougao")
                {
                    ljubicasti.transform.position = klonovi[0].transform.position;
                    ljubicasti.Play();
                }
                if (boja == "snegZelenoTrougao")
                {
                    zeleni.transform.position = klonovi[0].transform.position;
                    zeleni.Play();
                }
                if (boja == "snegNarandzastoTrougao")
                {
                    narandzasti.transform.position = klonovi[0].transform.position;
                    narandzasti.Play();
                }

                snezniNaEkranu.transform.position = klonovi[0].transform.position;
                snezniNaEkranu.Play();
                if (snegNaEkranu + 4 <= 8)
                {
                    snegNaEkranu += 4;
                }
                else
                {
                    snegNaEkranu = 8;
                }
                Destroy(klonovi[0]);
                klonovi.RemoveAt(0);
                if(duplo)
                {
                    skor++;
                }
                skor++;
                Kraj.uZoni = false;
                Kraj.dno = false;
            }
        }
    }
    public void TrougaoLed()
    {
        if ((boja == "ledPlavoTrougao" || boja == "ledCrvenoTrougao" || boja == "ledZutoTrougao" || boja == "ledLjubicastoTrougao" || boja == "ledNarandzastoTrougao" || boja == "ledZelenoTrougao") && klonovi.Count > 0)
        {
            if (Kraj.dno && !Kraj.kraj)
            {
                if (boja == "ledPlavoTrougao")
                {
                    plavi.transform.position = klonovi[0].transform.position;
                    plavi.Play();
                }
                if (boja == "ledCrvenoTrougao")
                {
                    crveni.transform.position = klonovi[0].transform.position;
                    crveni.Play();
                }
                if (boja == "ledZutoTrougao")
                {
                    zuti.transform.position = klonovi[0].transform.position;
                    zuti.Play();
                }
                if (boja == "ledLjubicastoTrougao")
                {
                    ljubicasti.transform.position = klonovi[0].transform.position;
                    ljubicasti.Play();
                }
                if (boja == "ledZelenoTrougao")
                {
                    zeleni.transform.position = klonovi[0].transform.position;
                    zeleni.Play();
                }
                if (boja == "ledNarandzastoTrougao")
                {
                    narandzasti.transform.position = klonovi[0].transform.position;
                    narandzasti.Play();
                }

                snezniNaEkranu.transform.position = klonovi[0].transform.position;
                snezniNaEkranu.Play();
                ledeniNaEkranu.transform.position = klonovi[0].transform.position;
                ledeniNaEkranu.Play();
                snegNaEkranu = 8;
                Destroy(klonovi[0]);
                klonovi.RemoveAt(0);
                if(duplo)
                {
                    skor++;
                }
                skor++;
                Kraj.uZoni = false;
                Kraj.dno = false;
            }
        }
    }
    public void Krug()
    {
        if (boja == "krug" && klonovi.Count > 0)
        {
            if (Kraj.dno)
            {
                if (klonovi[0].name == "EkstraZivot(Clone)")
                {
                    crni.transform.position = klonovi[0].transform.position;
                    crni.Play();
                }

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
    public IEnumerator PromenaSnegaNaEkranu()
    {
        if(slikaSnegaNaEkranu<snegNaEkranu)
        {
            yield return new WaitForSeconds(0.05f);
            slikaSnegaNaEkranu++;
        }
        else if(slikaSnegaNaEkranu>0 && slikaSnegaNaEkranu==snegNaEkranu && !game_over)
        {
            yield return new WaitForSeconds(1f);
            slikaSnegaNaEkranu--;
            snegNaEkranu--;
        }
        else if(slikaSnegaNaEkranu > 0 && slikaSnegaNaEkranu == snegNaEkranu && game_over)
        {
            yield return new WaitForSeconds(0.05f);
            slikaSnegaNaEkranu--;
            snegNaEkranu--;
        }
        yield return new WaitForEndOfFrame();
        StartCoroutine(PromenaSnegaNaEkranu());
    }
    public void SlikaSnega()
    {
        if(slikaSnegaNaEkranu==0)
        {
            snegEkran.GetComponent<Image>().sprite = nista;
        }
        if (slikaSnegaNaEkranu == 1)
        {
            snegEkran.GetComponent<Image>().sprite = sneg1;
        }
        if (slikaSnegaNaEkranu == 2)
        {
            snegEkran.GetComponent<Image>().sprite = sneg2;
        }
        if (slikaSnegaNaEkranu == 3)
        {
            snegEkran.GetComponent<Image>().sprite = sneg3;
        }
        if (slikaSnegaNaEkranu == 4)
        {
            snegEkran.GetComponent<Image>().sprite = sneg4;
        }
        if (slikaSnegaNaEkranu == 5)
        {
            snegEkran.GetComponent<Image>().sprite = sneg5;
        }
        if (slikaSnegaNaEkranu == 6)
        {
            snegEkran.GetComponent<Image>().sprite = sneg6;
        }
        if (slikaSnegaNaEkranu == 7)
        {
            snegEkran.GetComponent<Image>().sprite = sneg7;
        }
        if (slikaSnegaNaEkranu == 8)
        {
            snegEkran.GetComponent<Image>().sprite = sneg8;
        }
    }
    public IEnumerator Vatromet()
    {
        yield return new WaitForSeconds(2f);
        float x = Random.Range(zidL.transform.position.x + 0.5f, zidR.transform.position.x - 0.5f);
        float y = Random.Range(-2f, 4f);
        vatromet.transform.position = new Vector2(x, y);
        vatromet.Play();
        StartCoroutine(Vatromet());
    }
    public void Skip()
    {
        grudva.SetActive(false);
        sanke.SetActive(false);
        lavina.GetComponent<Animator>().SetTrigger("skip");
        restartAnimator.SetTrigger("skip");
        meniAnimator.SetTrigger("skip");
        pozadina.SetTrigger("skip");
        if (!noviNovacBio)
        {
            if (Meni.kampanja)
            {
                PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) + (skor / skorZaNovcic) + bonusNovcici);
            }
            else
            {
                PlayerPrefs.SetInt("Dijamanti", PlayerPrefs.GetInt("Dijamanti", 0) + (skor / 20));
            }
            noviNovacBio = true;
        }
    }
    public void Tranzicija()
    {
        if (tranzicijaAnimator.GetCurrentAnimatorStateInfo(0).IsName("kraj"))
        {
            tranzicija.SetActive(false);
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
        SceneManager.LoadScene(4);
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
            sneg = false;
            klikZuto();
            zuto = true;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            sneg = false;
            klikCrveno();
            crveno = true;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            sneg = false;
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
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            crni.transform.position = klonovi[0].transform.position;
            crni.Play();
            Destroy(klonovi[0]);
            klonovi.RemoveAt(0);
            Kraj.uZoni = false;
        }
    }
}
