using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StvaranjeOblika : MonoBehaviour
{
    public List<GameObject> kvadrat = new List<GameObject>();
    public List<GameObject> trougao = new List<GameObject>();
    public List<GameObject> krug = new List<GameObject>();

    public List<GameObject> klonovi= new List<GameObject>();

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

    public GameObject zidL;
    public GameObject zidR;
    public GameObject ekstraZivot;
    public GameObject goodZone;
    public GameObject tranzicija;
    public GameObject noviNovcici;
    public GameObject noviDijamanti;
    public GameObject ukupniNovcici;
    public GameObject ukupniDijamanti;
    public GameObject skip;
    public GameObject tabla;
    public GameObject restartBaner;
    public GameObject meniBaner;

    public Animator restartAnimator;
    public Animator meniAnimator;
    public Animator tranzicijaAnimator;
    public Animator zaradjeniNovciciAnimator;
    public Animator zaradjeniDijamantiAnimator;
    public Animator zutoDugme;
    public Animator crvenoDugme;
    public Animator plavoDugme;
    public Animator bob;
    public Animator pacov;
    public Animator zbun;

    public TextMeshProUGUI noviNovac;
    public TextMeshProUGUI noviDijamant;
    public TextMeshProUGUI ukupanNovac;
    public TextMeshProUGUI ukupniDijamant;

    public string boja = "";
    public float brzina1 = 1f;
    public float brzina=1f;
    public int skor = 0;

    private bool game_over = false;
    private bool plavo=false;
    private bool crveno=false;
    private bool zuto=false;
    private int ekstra = 0;
    private int moc = 0;
    private int rot = 0;
    private bool krajNovac=false;
    private int novac;
    private int dijamanti;
    public bool noviNovacBio=false;
    private void Start()
    {
        if (Meni.kampanja)
        {
            highScore.text = "High Score - " + PlayerPrefs.GetInt("HighScore2Cm", 0);
            skor = PlayerPrefs.GetInt("startScore", 0);

            ukupniDijamanti.SetActive(false);
        }
        else
        {
            highScore.text = "High Score - " + PlayerPrefs.GetInt("HighScore2Cl", 0);
            skor = 0;

            ukupniNovcici.SetActive(false);
        }

        tabla.transform.position=new Vector2(zidR.transform.position.x-2.2f,tabla.transform.position.y);
        zutoDugme.transform.position = new Vector2((zidL.transform.position.x + crvenoDugme.transform.position.x) / 2-0.2f, zutoDugme.transform.position.y);
        plavoDugme.transform.position = new Vector2((zidR.transform.position.x + crvenoDugme.transform.position.x) / 2+0.2f,plavoDugme.transform.position.y);

        StartCoroutine(Pocetak());

        RectTransform kanvasTransform=goodZone.GetComponent<RectTransform>();

        brzina1 = ((0.05f * kanvasTransform.rect.height) / 1870f)*30;
    }
    public void Update()
    {
        if (zaradjeniNovciciAnimator.GetCurrentAnimatorStateInfo(0).IsName("New State 1") && !krajNovac)
        {
            StartCoroutine(Novcici());
            krajNovac = true;
        }
        if(zaradjeniDijamantiAnimator.GetCurrentAnimatorStateInfo(0).IsName("New State 1") && !krajNovac)
        {
            StartCoroutine(Dijamanti());
            krajNovac = true;
        }


        TrenutnaBoja();

        Brzina();

        SkorTekst();

        ProveraPoraza();

        SareneBoje();

        Trougao();

        Krug();

        Padanje();

        Rotacija();

        Tranzicija();

        Tastatura();
    }
    public void Brzina()
    {
        if (Meni.kampanja)
        {
            brzina = brzina1 + skor * brzina1 * PlayerPrefs.GetFloat("speedChange",6) * Time.deltaTime;
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
            klonovi[0].transform.position = new Vector2(klonovi[0].transform.position.x, klonovi[0].transform.position.y - brzina*Time.deltaTime);
        }
    }
    public void Rotacija()
    {
        if(klonovi.Count>0)
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
        poeni.text="Score:"+skor.ToString();

        ukupanNovac.text = PlayerPrefs.GetInt("Novcici", 0).ToString() + "<sprite=0>";
        ukupniDijamant.text = PlayerPrefs.GetInt("Dijamanti", 0).ToString() + "<sprite=0>";
    }
    public void ProveraPoraza()
    {
        if (!game_over)
        {
            if (Kraj.kraj == true && ekstra==0)
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
        novac = PlayerPrefs.GetInt("Novcici", 0) + (skor / 10);
        dijamanti = PlayerPrefs.GetInt("Dijamanti", 0) + (skor / 20);
        restartBaner.SetActive(true);
        restartAnimator.SetTrigger("kraj");
        meniBaner.SetActive(true);
        meniAnimator.SetTrigger("kraj");
        bob.SetTrigger("kraj");
        pacov.SetTrigger("kraj");
        zbun.SetTrigger("kraj");

        if (Meni.kampanja)
        {
            zaradjeniNovciciAnimator.SetTrigger("kraj");
        }
        else
        {
            zaradjeniDijamantiAnimator.SetTrigger("kraj");
        }

        if (Meni.kampanja)
        {
            if (skor > PlayerPrefs.GetInt("HighScore2Cm", 0))
            {
                PlayerPrefs.SetInt("HighScore2Cm", skor);
                highScore.text = "New High Score !!!";
                StartCoroutine(Vatromet());
            }

            if (skor >= 100 && PlayerPrefs.GetInt("Level", 1) == 2)
            {
                PlayerPrefs.SetInt("Level", 3);

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
            if (skor > PlayerPrefs.GetInt("HighScore2Cl", 0))
            {
                PlayerPrefs.SetInt("HighScore2Cl", skor);
                highScore.text = "New High Score !!!";
                StartCoroutine(Vatromet());
            }
        }

        noviNovac.text = "+" + (skor / 10).ToString() + "<sprite=0>";
        noviDijamant.text = "+" + (skor / 20).ToString() + "<sprite=0>";

        if (Meni.kampanja)
        {
            noviNovcici.SetActive(true);
        }
        else
        {
            noviDijamanti.SetActive(true);
        }

        if (Meni.kampanja)
        {
            zaradjeniNovciciAnimator.SetBool("Kraj", true);
        }
        else
        {
            zaradjeniDijamantiAnimator.SetBool("Kraj", true);
        }

        crni.transform.position = klonovi[0].transform.position;
        crni.Play();
        Destroy(klonovi[0]);
        klonovi.RemoveAt(0);
        Kraj.uZoni = false;
    }
    public IEnumerator Novcici()
    {
        if (skor / 10!= 0)
        {
            for (int i = 1; i <= skor / 10; i++)
            {
                if (!noviNovacBio)
                {
                    PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) + 1);
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }
    public IEnumerator Dijamanti()
    {
        if (skor / 20 != 0)
        {
            for (int i = 1; i <= skor / 20; i++)
            {
                if (!noviNovacBio)
                {
                    PlayerPrefs.SetInt("Dijamanti", PlayerPrefs.GetInt("Dijamanti", 0) + 1);
                    yield return new WaitForSeconds(0.1f);
                }
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
            int oblik = Random.Range(0, 10);

            if (oblik < 7)
            {
                int r = Random.Range(0, kvadrat.Count);
                if(r>2 && PlayerPrefs.GetInt("tocak",0)==1 && Meni.kampanja)
                {
                    pacov.GetComponent<Animator>().SetTrigger("znak");
                }
                rot = Random.Range(1, 3);
                float x = Random.Range(zidL.transform.position.x + 0.5f, zidR.transform.position.x - 0.5f);
                float y = Random.Range(6f, 8f);
                GameObject klon = Instantiate(kvadrat[r], new Vector2(x, y), Quaternion.identity);
                klonovi.Add(klon);
                StartCoroutine(Cekanje());
            }
            if (oblik >= 7 && oblik < 9)
            {
                int r = Random.Range(0, trougao.Count);
                if (r > 2 && PlayerPrefs.GetInt("tocak", 0) == 1 && Meni.kampanja)
                {
                    pacov.GetComponent<Animator>().SetTrigger("znak");
                }
                rot = Random.Range(1, 3);
                float x = Random.Range(zidL.transform.position.x + 0.5f, zidR.transform.position.x - 0.5f);
                float y = Random.Range(6f, 8f);
                GameObject klon = Instantiate(trougao[r], new Vector2(x, y), Quaternion.identity);
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
        if (klonovi.Count > 0)
        {
            if (Kraj.uZoni)
            {
                if (boja == "zuto" && klonovi.Count > 0 && !Kraj.kraj)
                {
                    skor++;
                    zuti.transform.position = klonovi[0].transform.position;
                    zuti.Play();
                    Destroy(klonovi[0]);
                    klonovi.RemoveAt(0);
                    Kraj.uZoni = false;     
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
        if (klonovi.Count > 0)
        {
            if (Kraj.uZoni)
            {
                if (boja == "crveno" && klonovi.Count > 0 && !Kraj.kraj)
                {
                    skor++;
                    crveni.transform.position = klonovi[0].transform.position;
                    crveni.Play();
                    Destroy(klonovi[0]);
                    klonovi.RemoveAt(0);
                    Kraj.uZoni = false;
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
        if (klonovi.Count > 0)
        {
            if (Kraj.uZoni)
            {
                if (boja == "plavo" && klonovi.Count > 0 && !Kraj.kraj)
                {
                    skor++;
                    plavi.transform.position = klonovi[0].transform.position;
                    plavi.Play();
                    Destroy(klonovi[0]);
                    klonovi.RemoveAt(0);
                    Kraj.uZoni = false;
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
    }
    public void PlavoUp()
    {
        plavo= false;
        plavoDugme.SetBool("plavo", false);
    }
    public void CrvenoDown()
    {
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
        zuto = true;
        zutoDugme.SetBool("zuto", true);
    }
    public void ZutoUp()
    {
        zuto = false;
        zutoDugme.SetBool("zuto", false);
    }
    public void SareneBoje()
    {
        if (klonovi.Count>0 && Kraj.uZoni)
        {
            if (boja == "narandzasto" && klonovi.Count > 0 && !Kraj.kraj && zuto && crveno && !plavo)
            {
                if(Meni.kampanja && PlayerPrefs.GetInt("kapibara",0)==1)
                {
                    skor += 3;
                }
                else
                {
                    skor++;
                }
                narandzasti.transform.position = klonovi[0].transform.position;
                narandzasti.Play();
                Destroy(klonovi[0]);
                klonovi.RemoveAt(0);
                Kraj.uZoni = false;
            }

            if (boja == "ljubicasto" && klonovi.Count > 0 && !Kraj.kraj && !zuto && crveno && plavo)
            {
                if (Meni.kampanja && PlayerPrefs.GetInt("kapibara", 0) == 1)
                {
                    skor += 3;
                }
                else
                {
                    skor++;
                }
                ljubicasti.transform.position = klonovi[0].transform.position;
                ljubicasti.Play();
                Destroy(klonovi[0]);
                klonovi.RemoveAt(0);
                Kraj.uZoni = false;
            }

            if (boja == "zeleno" && klonovi.Count > 0 && !Kraj.kraj && zuto && !crveno && plavo)
            {
                if (Meni.kampanja && PlayerPrefs.GetInt("kapibara", 0) == 1)
                {
                    skor += 3;
                }
                else
                {
                    skor++;
                }
                zeleni.transform.position = klonovi[0].transform.position;
                zeleni.Play();
                Destroy(klonovi[0]);
                klonovi.RemoveAt(0);
                Kraj.uZoni = false;
            }

            if (boja == "krug" && klonovi.Count > 0 && !Kraj.kraj && moc==3)
            {

                if (klonovi[0].name=="EkstraZivot(Clone)")
                {
                    if(Meni.kampanja && PlayerPrefs.GetInt("srce", 0) == 1)
                    {
                        ekstra++;
                    }
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
        if(boja=="trougao" && klonovi.Count>0)
        {
            if (Kraj.dno && !Kraj.kraj)
            {
                if (klonovi[0].name=="PlaviTrougao(Clone)")
                {
                    plavi.transform.position = klonovi[0].transform.position;
                    plavi.Play();
                }
                if (klonovi[0].name == "CrveniTrougao(Clone)")
                {
                    crveni.transform.position = klonovi[0].transform.position;
                    crveni.Play();
                }
                if (klonovi[0].name == "ZutiTrougao(Clone)")
                {
                    zuti.transform.position = klonovi[0].transform.position;
                    zuti.Play();
                }
                if (klonovi[0].name == "LjubicastiTrougao(Clone)")
                {
                    ljubicasti.transform.position = klonovi[0].transform.position;
                    ljubicasti.Play();
                }
                if (klonovi[0].name == "ZeleniTrougao(Clone)")
                {
                    zeleni.transform.position = klonovi[0].transform.position;
                    zeleni.Play();
                }
                if (klonovi[0].name == "NarandzastiTrougao(Clone)")
                {
                    narandzasti.transform.position = klonovi[0].transform.position;
                    narandzasti.Play();
                }

                Destroy(klonovi[0]);
                klonovi.RemoveAt(0);
                skor++;
                Kraj.uZoni=false;
                Kraj.dno = false;
            }
        }
    }
    public void Krug()
    {
        if(boja=="krug" && klonovi.Count> 0)
        {
            if (Kraj.dno)
            {
                if (klonovi[0].name=="EkstraZivot(Clone)")
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
        if(ekstra>0)
        {
            ekstraZivot.SetActive(true);
        }
        else
        {
            ekstraZivot.SetActive(false);
        }
        if(Kraj.kraj && ekstra>0)
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
    public IEnumerator Vatromet()
    {
        yield return new WaitForSeconds(2f);
        float x = Random.Range(zidL.transform.position.x + 0.5f, zidR.transform.position.x - 0.5f);
        float y = Random.Range(-2f,4f);
        vatromet.transform.position=new Vector2(x, y);
        vatromet.Play();
        StartCoroutine(Vatromet());
    }
    public void Skip()
    {
        restartAnimator.SetTrigger("skip");
        meniAnimator.SetTrigger("skip");
        bob.SetTrigger("skip");
        pacov.SetTrigger("skip");
        zbun.SetTrigger("skip");
        noviNovac.enabled=false;
        noviDijamant.enabled = false;
        if (!noviNovacBio)
        {
            if (Meni.kampanja)
            {
                PlayerPrefs.SetInt("Novcici", novac);
            }
            else
            {
                PlayerPrefs.SetInt("Dijamanti", dijamanti);
            }
            noviNovacBio = true;
        }
    }
    public void Tranzicija()
    {
        if(tranzicijaAnimator.GetCurrentAnimatorStateInfo(0).IsName("kraj"))
        {
            tranzicija.SetActive(false);
        }
    }
    public void Restart()
    {
        Kraj.kraj = false;
        Kraj.uZoni= false;
        Kraj.dno = false;
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        StartCoroutine(restart());
    }
    public IEnumerator restart()
    {
        yield return new WaitForSeconds(0.5f);
        if(Meni.kampanja)
        {
            PlayerPrefs.SetInt("Novcici", novac);
        }
        else
        {
            PlayerPrefs.SetInt("Dijamanti", dijamanti);
        }
        
        SceneManager.LoadScene(3);
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
        if (Meni.kampanja)
        {
            PlayerPrefs.SetInt("Novcici", novac);
        }
        else
        {
            PlayerPrefs.SetInt("Dijamanti", dijamanti);
        }

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
    }
}
