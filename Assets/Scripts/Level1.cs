using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level1 : MonoBehaviour
{
    //oblici
    public List<GameObject> kvadrat = new List<GameObject>();
    public List<GameObject> trougao = new List<GameObject>();

    //oblici u igri
    public List<GameObject> klonovi = new List<GameObject>();

    //tekstovi
    public Text poeni;
    public Text highScore;

    //dijalozi
    public Dialog beforeStartDialog;
    public Dialog loseLess;
    public Dialog loseMore;
    public Dialog winDialog;
    public Dialog beforeStartCompetitive;

    //start animatori
    public Animator animator3;
    public Animator animator2;
    public Animator animator1;
    public Animator animatorStart;

    //efekti
    public ParticleSystem plavi;
    public ParticleSystem zuti;
    public ParticleSystem crveni;
    public ParticleSystem narandzasti;
    public ParticleSystem ljubicasti;
    public ParticleSystem zeleni;
    public ParticleSystem beli;
    public ParticleSystem vatromet;

    //animatori
    public Animator tranzicijaAnimator;
    public Animator restartAnimator;
    public Animator meniAnimator;
    public Animator noviNovacAnimator;
    public Animator noviDijamantiAnimator;
    public Animator ukupanNovacAnimator;
    public Animator ukupniDijamantiAnimator;
    public Animator zutoDugme;
    public Animator crvenoDugme;
    public Animator plavoDugme;

    //tekstovi za novac
    public TextMeshProUGUI noviNovac;
    public TextMeshProUGUI noviDijamanti;
    public TextMeshProUGUI ukupanNovac;
    public TextMeshProUGUI ukupniDijamanti;

    //zidovi i transicija
    public GameObject zidL;
    public GameObject zidR;
    public GameObject goodZone;
    public GameObject tranzicija;

    //totemi
    public GameObject tvTotem;

    //zvukovi
    public AudioSource mistakeSound;
    public AudioSource gameOverSound;
    public AudioSource destroySound;
    public AudioSource countdownSound;
    public AudioSource tvSound;
    public AudioSource tvLinesSound;

    public string boja = "";
    public float brzina1 = 1f;
    public float brzina = 1f;
    public int skor;

    private bool game_over = false;
    private bool krajNovac = false;
    private bool plavo = false;
    private bool crveno = false;
    private bool zuto = false;
    private int rot = 0;
    private int novac;
    private int dijamanti;
    private bool noviNovacBio = false;
    private bool skip = false;

    bool tv = false;

    bool mozePadanje = true;
    private void Start()
    {
        Camera.main.GetComponent<AudioSource>().volume = PlayerPrefs.GetInt("MusicVolume", 100) * Camera.main.GetComponent<AudioSource>().volume / 100;
        mistakeSound.volume = PlayerPrefs.GetInt("UIVolume", 100) * mistakeSound.volume / 100;
        gameOverSound.volume = PlayerPrefs.GetInt("UIVolume", 100) * gameOverSound.volume / 100;
        destroySound.volume = PlayerPrefs.GetInt("UIVolume", 100) * destroySound.volume / 100;
        tvSound.volume = PlayerPrefs.GetInt("UIVolume", 100) * tvSound.volume / 100;
        tvLinesSound.volume = PlayerPrefs.GetInt("UIVolume", 100) * tvLinesSound.volume / 100;

        if (Meni.kampanja)
        {
            highScore.text = "High Score - " + PlayerPrefs.GetInt("HighScore1Cm", 0);

            skor = PlayerPrefs.GetInt("startScore", 0);
        }
        else
        {
            highScore.text = "High Score - " + PlayerPrefs.GetInt("HighScore1Cl", 0);

            skor = 0;

            if (PlayerPrefs.GetInt("CompetitiveLevel1", 0) == 0)
            {
                StartCoroutine(WaitFirstTimeCompetitive());

                DialogManager.instance.StartDialogue(beforeStartCompetitive);
            }
        }

        if (PlayerPrefs.GetInt("DialogueLevel1", 0) == 0)
        {
            StartCoroutine(WaitFirstTime());

            DialogManager.instance.StartDialogue(beforeStartDialog);
        }
        else if (Meni.kampanja)
        {
            countdownSound.Play();
            StartCoroutine(MusicLowered());
            animator3.SetTrigger("Start");
            animator2.SetTrigger("Start");
            animator1.SetTrigger("Start");
            animatorStart.SetTrigger("Start");
            StartCoroutine(Pocetak());
        }

        RectTransform kanvasTransform = goodZone.GetComponent<RectTransform>();

        brzina1 = ((0.05f * kanvasTransform.rect.height) / 1870f) * 30;
    }
    public void Update()
    {
        if (noviNovacAnimator.GetCurrentAnimatorStateInfo(0).IsName("New State 0") && !krajNovac)
        {
            StartCoroutine(Novcici());
            krajNovac = true;
        }

        if (noviDijamantiAnimator.GetCurrentAnimatorStateInfo(0).IsName("New State 0") && !krajNovac)
        {
            StartCoroutine(Dijamanti());
            krajNovac = true;
        }

        TrenutnaBoja();

        Brzina();

        SkorTekst();

        if (!tv)
        {
            ProveraPoraza();
        }

        SareneBoje();

        Trougao();

        Padanje();

        Rotacija();

        Tranzicija();

        Tastatura();
    }

    public IEnumerator WaitFirstTime()
    {
        yield return new WaitForSeconds(0.1f);
        tranzicijaAnimator.SetBool("tranzicija1", true);

        while (true)
        {
            if (PlayerPrefs.GetInt("DialogueLevel1", 0) == 1)
            {
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(MusicLowered());
        countdownSound.Play();
        animator3.SetTrigger("Start");
        animator2.SetTrigger("Start");
        animator1.SetTrigger("Start");
        animatorStart.SetTrigger("Start");
        yield return new WaitForSeconds(4f);
        Stvaranje();
    }

    public IEnumerator WaitFirstTimeCompetitive()
    {
        yield return new WaitForSeconds(0.1f);
        tranzicijaAnimator.SetBool("tranzicija1", true);

        while (true)
        {
            if (PlayerPrefs.GetInt("CompetitiveLevel1", 0) == 1)
            {
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(MusicLowered());
        countdownSound.Play();
        animator3.SetTrigger("Start");
        animator2.SetTrigger("Start");
        animator1.SetTrigger("Start");
        animatorStart.SetTrigger("Start");
        yield return new WaitForSeconds(4f);
        Stvaranje();
    }

    public IEnumerator MusicLowered()
    {
        Camera.main.GetComponent<AudioSource>().volume /= 5;
        yield return new WaitForSeconds(4f);
        Camera.main.GetComponent<AudioSource>().volume *= 5;
    }

    public void Brzina()
    {
        if (Meni.kampanja)
        {
            brzina = brzina1 + skor * brzina1 * 0.12f;
        }
        else
        {
            brzina = brzina1 + brzina1 * 2.4f;
        }
    }
    public void Padanje()
    {
        if (klonovi.Count > 0 && mozePadanje)
        {
            klonovi[0].transform.position = new Vector2(klonovi[0].transform.position.x, klonovi[0].transform.position.y - brzina * Time.deltaTime);
        }
    }
    public void Rotacija()
    {
        if (klonovi.Count > 0 && mozePadanje)
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
        ukupniDijamanti.text = PlayerPrefs.GetInt("Dijamanti", 0).ToString() + "<sprite=0>";
    }
    public void ProveraPoraza()
    {
        if (!game_over)
        {
            if (Kraj.kraj == true)
            {
                if (Meni.kampanja)
                {
                    TvTotem();
                }
                else
                {
                    Poraz();
                    game_over = true;
                }
            }
        }
    }
    public void TvTotem()
    {
        tv = true;
        if (PlayerPrefs.GetInt("tv") == 1)
        {
            int r = Random.Range(1, 5);
            if (r == 4)
            {
                tvTotem.GetComponent<Animator>().SetTrigger("tv");

                StartCoroutine(tvPauza());
            }
            else
            {
                Poraz();
                game_over = true;
            }
        }
        else
        {
            Poraz();
            game_over = true;
        }
    }
    public IEnumerator tvPauza()
    {
        mozePadanje = false;
        yield return new WaitForSeconds(2.2f);
        StartCoroutine(vracanjeNazad());
        yield return new WaitForSeconds(2);
        mozePadanje = true;
        Kraj.kraj = false;
        Kraj.uZoni = false;
        Kraj.dno = false;
        tv = false;
        Stvaranje();
    }
    public IEnumerator vracanjeNazad()
    {
        while (klonovi[0].transform.position.y < 8)
        {
            klonovi[0].transform.position = new Vector2(klonovi[0].transform.position.x, klonovi[0].transform.position.y + 50 * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }
    }
    public void Poraz()
    {
        Handheld.Vibrate();
        gameOverSound.Play();

        skip = true;
        mozePadanje = false;
        novac = PlayerPrefs.GetInt("Novcici", 0) + (skor / 10);
        dijamanti = PlayerPrefs.GetInt("Dijamanti", 0) + (skor / 20);

        if (Meni.kampanja)
        {
            if (skor > PlayerPrefs.GetInt("HighScore1Cm", 0))
            {
                PlayerPrefs.SetInt("HighScore1Cm", skor);
                highScore.text = "New High Score !!!";
                StartCoroutine(Vatromet());
            }

            if (PlayerPrefs.GetInt("Level", 1) == 1)
            {
                if (PlayerPrefs.GetInt("dzojstik", 0) == 1 && PlayerPrefs.GetInt("tv", 0) == 1)
                {
                    if (PlayerPrefs.GetInt("trenutniCilj", 0) == 0)
                    {
                        PlayerPrefs.SetInt("trenutniCilj", PlayerPrefs.GetInt("HighScore1Cm", 0) + 10);
                        PlayerPrefs.SetInt("pokusajaKraja", 0);
                    }
                    else
                    {
                        if (skor >= PlayerPrefs.GetInt("trenutniCilj", 0))
                        {
                            PlayerPrefs.SetInt("trenutniCilj", 0);
                            PlayerPrefs.SetInt("pokusajaKraja", 0);

                            PlayerPrefs.SetInt("Level", 2);
                            PlayerPrefs.SetInt("justFinished", 1);

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

                            DialogManager.instance.StartDialogue(winDialog);
                        }
                        else
                        {
                            if (PlayerPrefs.GetInt("pokusajaKraja") >= 10)
                            {
                                if (PlayerPrefs.GetInt("HighScore1Cm", 0) < PlayerPrefs.GetInt("trenutniCilj", 0) - 1)
                                {
                                    PlayerPrefs.SetInt("trenutniCilj", PlayerPrefs.GetInt("trenutniCilj", 0) - 1);
                                }

                                PlayerPrefs.SetInt("pokusajaKraja", 0);
                            }
                            else
                            {
                                PlayerPrefs.SetInt("pokusajaKraja", PlayerPrefs.GetInt("pokusajaKraja", 0) + 1);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            if (skor > PlayerPrefs.GetInt("HighScore1Cl", 0))
            {
                PlayerPrefs.SetInt("HighScore1Cl", skor);
                highScore.text = "New High Score !!!";
                StartCoroutine(Vatromet());
            }
        }

        noviNovac.text = "+" + (skor / 10).ToString() + "<sprite=0>";
        noviDijamanti.text = "+" + (skor / 20).ToString() + "<sprite=0>";

        restartAnimator.SetBool("Kraj", true);
        meniAnimator.SetBool("Kraj", true);

        if (Meni.kampanja)
        {
            ukupanNovacAnimator.SetBool("Kraj", true);
        }
        else
        {
            ukupniDijamantiAnimator.SetBool("Kraj", true);
        }

        if (Meni.kampanja)
        {
            noviNovacAnimator.SetTrigger("kraj");
        }
        else
        {
            noviDijamantiAnimator.SetTrigger("kraj");
        }
        beli.transform.position = klonovi[0].transform.position;
        beli.Play();
        mistakeSound.Play();
        Destroy(klonovi[0]);
        klonovi.RemoveAt(0);
        Kraj.uZoni = false;

        if (PlayerPrefs.GetInt("DialogueLevel1", 0) == 1)
        {
            if (skor < 10)
            {
                DialogManager.instance.StartDialogue(loseLess);
            }
            else
            {
                DialogManager.instance.StartDialogue(loseMore);
            }
        }
    }
    public IEnumerator Novcici()
    {
        if (skor / 10 != 0)
        {
            for (int i = 1; i <= (skor / 10); i++)
            {
                if (!noviNovacBio)
                {
                    PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) + 1);
                    SoundManager.Instance.PlayCoinSound();
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }
    public IEnumerator Dijamanti()
    {
        if (skor / 20 != 0)
        {
            for (int i = 1; i <= (skor / 20); i++)
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
                int random = 0;
                if (PlayerPrefs.GetInt("dzojstik") == 1 && Meni.kampanja)
                {
                    random = Random.Range(0, 2);
                }

                int r;
                if (random == 0)
                {
                    r = Random.Range(0, kvadrat.Count);
                }
                else
                {
                    r = Random.Range(0, 3);
                }
                rot = Random.Range(1, 3);
                float x = Random.Range(zidL.transform.position.x + 0.5f, zidR.transform.position.x - 0.5f);
                float y = Random.Range(6f, 8f);
                GameObject klon = Instantiate(kvadrat[r], new Vector2(x, y), Quaternion.identity);
                klonovi.Add(klon);
                StartCoroutine(Cekanje());
            }
            if (oblik >= 7 && oblik < 10)
            {

                int random = 0;
                if (PlayerPrefs.GetInt("dzojstik") == 1 && Meni.kampanja)
                {
                    random = Random.Range(0, 2);
                }

                int r;
                if (random == 0)
                {
                    r = Random.Range(0, trougao.Count);
                }
                else
                {
                    r = Random.Range(0, 3);
                }
                rot = Random.Range(1, 3);
                float x = Random.Range(zidL.transform.position.x + 0.5f, zidR.transform.position.x - 0.5f);
                float y = Random.Range(6f, 8f);
                GameObject klon = Instantiate(trougao[r], new Vector2(x, y), Quaternion.identity);
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
                    destroySound.Play();
                    klonovi.RemoveAt(0);
                    Kraj.uZoni = false;
                }
                else if ((boja == "narandzasto" || boja == "zeleno") && klonovi.Count > 0 && !Kraj.kraj)
                {

                }
                else
                {
                    Kraj.kraj = true;
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
                    destroySound.Play();
                    klonovi.RemoveAt(0);
                    Kraj.uZoni = false;
                }
                else if ((boja == "narandzasto" || boja == "ljubicasto") && klonovi.Count > 0 && !Kraj.kraj)
                {

                }
                else
                {
                    Kraj.kraj = true;
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
                    destroySound.Play();
                    klonovi.RemoveAt(0);
                    Kraj.uZoni = false;
                }
                else if ((boja == "ljubicasto" || boja == "zeleno") && klonovi.Count > 0 && !Kraj.kraj)
                {

                }
                else
                {
                    Kraj.kraj = true;
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
        plavo = false;
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
        if (klonovi.Count > 0 && Kraj.uZoni)
        {
            if (boja == "narandzasto" && klonovi.Count > 0 && !Kraj.kraj && zuto && crveno && !plavo)
            {
                skor++;
                narandzasti.transform.position = klonovi[0].transform.position;
                narandzasti.Play();
                Destroy(klonovi[0]);
                destroySound.Play();
                klonovi.RemoveAt(0);
                Kraj.uZoni = false;
            }

            if (boja == "ljubicasto" && klonovi.Count > 0 && !Kraj.kraj && !zuto && crveno && plavo)
            {
                skor++;
                ljubicasti.transform.position = klonovi[0].transform.position;
                ljubicasti.Play();
                Destroy(klonovi[0]);
                destroySound.Play();
                klonovi.RemoveAt(0);
                Kraj.uZoni = false;
            }

            if (boja == "zeleno" && klonovi.Count > 0 && !Kraj.kraj && zuto && !crveno && plavo)
            {
                skor++;
                zeleni.transform.position = klonovi[0].transform.position;
                zeleni.Play();
                Destroy(klonovi[0]);
                destroySound.Play();
                klonovi.RemoveAt(0);
                Kraj.uZoni = false;
            }
        }
    }
    public void Trougao()
    {
        if (boja == "trougao" && klonovi.Count > 0)
        {
            if (Kraj.dno && !Kraj.kraj)
            {
                if (klonovi[0].name == "PlaviTrougao1(Clone)")
                {
                    plavi.transform.position = klonovi[0].transform.position;
                    plavi.Play();
                }
                if (klonovi[0].name == "CrveniTrougao1(Clone)")
                {
                    crveni.transform.position = klonovi[0].transform.position;
                    crveni.Play();
                }
                if (klonovi[0].name == "ZutiTrougao1(Clone)")
                {
                    zuti.transform.position = klonovi[0].transform.position;
                    zuti.Play();
                }
                if (klonovi[0].name == "LjubicastiTrougao1(Clone)")
                {
                    ljubicasti.transform.position = klonovi[0].transform.position;
                    ljubicasti.Play();
                }
                if (klonovi[0].name == "ZeleniTrougao1(Clone)")
                {
                    zeleni.transform.position = klonovi[0].transform.position;
                    zeleni.Play();
                }
                if (klonovi[0].name == "NarandzastiTrougao1(Clone)")
                {
                    narandzasti.transform.position = klonovi[0].transform.position;
                    narandzasti.Play();
                }

                Destroy(klonovi[0]);
                destroySound.Play();
                klonovi.RemoveAt(0);
                skor++;
                Kraj.uZoni = false;
                Kraj.dno = false;
            }
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
        if (skip)
        {
            restartAnimator.SetTrigger("skip");
            meniAnimator.SetTrigger("skip");

            if (Meni.kampanja)
            {
                ukupanNovacAnimator.SetTrigger("skip");
            }
            else
            {
                ukupniDijamantiAnimator.SetTrigger("skip");
            }

            if (Meni.kampanja)
            {
                noviNovacAnimator.SetTrigger("skip");
            }
            else
            {
                noviDijamantiAnimator.SetTrigger("skip");
            }
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
        SoundManager.Instance.PlayClickSound();
        StartCoroutine(restart());
    }
    public IEnumerator restart()
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

        SceneManager.LoadScene(3);
    }
    public void Menu()
    {
        if (PlayerPrefs.GetInt("DialogueLevel1", 0) == 1)
        {
            return;
        }

        Kraj.kraj = false;
        Kraj.uZoni = false;
        Kraj.dno = false;
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        SoundManager.Instance.PlayClickSound();
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

        SceneManager.LoadScene(1);
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
        if (Input.GetKeyDown(KeyCode.UpArrow) && klonovi.Count > 0)
        {
            beli.transform.position = klonovi[0].transform.position;
            beli.Play();
            Destroy(klonovi[0]);
            klonovi.RemoveAt(0);
            Kraj.uZoni = false;
        }
    }
}
