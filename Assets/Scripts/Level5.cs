using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level5 : MonoBehaviour
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
    public ParticleSystem kristalZuti;
    public ParticleSystem kristalCrveni;
    public ParticleSystem kristalPlavi;
    public ParticleSystem vatromet;
    public ParticleSystem brodici;

    [HideInInspector]
    public static ParticleSystem plaviEfekat;
    public static ParticleSystem zutiEfekat;
    public static ParticleSystem crveniEfekat;
    public static ParticleSystem narandzastiEfekat;
    public static ParticleSystem ljubicastiEfekat;
    public static ParticleSystem zeleniEfekat;
    public static ParticleSystem crniEfekat;

    public GameObject zidL;
    public GameObject zidR;
    public GameObject zidU;
    public GameObject zidD;
    public GameObject ekstraZivot;
    public GameObject goodZone;
    public GameObject laser;
    public GameObject tranzicija;
    public GameObject noviNovcici;
    public GameObject noviDijamanti;
    public GameObject skip;
    public GameObject zutoDugme;
    public GameObject crvenoDugme;
    public GameObject plavoDugme;
    public GameObject zutiMetakStart;
    public GameObject crveniMetakStart;
    public GameObject plaviMetakStart;
    public GameObject zutiMetak;
    public GameObject crveniMetak;
    public GameObject plaviMetak;
    public GameObject plaviTop;
    public GameObject zutiTop;
    public GameObject crveniTop;
    public GameObject prviKristal;
    public GameObject drugiKristal;
    public GameObject treciKristal;
    public GameObject ukupniNovcici;
    public GameObject ukupniDijamanti;

    public Animator brodAnimator;
    public Animator laserAnimator;
    public Animator restartAnimator;
    public Animator meniAnimator;
    public Animator tranzicijaAnimator;
    public Animator zaradjeniNovciciAnimator;
    public Animator zaradjeniDijamantiAnimator;

    public Sprite crveniKvadrat;
    public Sprite plaviKvadrat;
    public Sprite zutiKvadrat;
    public Sprite crveniTrougao;
    public Sprite plaviTrougao;
    public Sprite zutiTrougao;
    public Sprite crveniKristal;
    public Sprite plaviKristal;
    public Sprite zutiKristal;

    public TextMeshProUGUI noviNovac;
    public TextMeshProUGUI noviDijamant;
    public TextMeshProUGUI ukupanNovac;
    public TextMeshProUGUI ukupanDijamant;

    public string boja = "";
    public float brzina1 = 1f;
    public float brzina = 1f;
    public static int skor = 0;

    private bool game_over = false;
    private bool plavo = false;
    private bool crveno = false;
    private bool zuto = false;
    private int ekstra = 0;
    private int rot = 0;
    private bool krajNovac = false;
    private int novac;
    private int dijamanti;
    private bool noviNovacBio = false;
    private bool zutiMetakMoze=false;
    private bool crveniMetakMoze=false;
    private bool plaviMetakMoze = false;
    private bool kretanje=true;
    private bool sareniMetak=false;
    private bool ispaljenMetak = false;
    private bool bioZuti=false;
    private bool bioCrveni = false;
    private bool bioPlavi = false;
    private int kristal = 0;

    private int bonusNovcici = 0;
    private int skorZaNovcic = 10;

    private float brojOdbrane = 0;
    private List<int> indeksiOdbrane = new List<int>();
    private bool odbrana = false;

    private int potrebanStreak = 0;
    private int streak = 0;
    private int mnozilac = 1;

    private int brojTrouglova = 0;

    public GameObject stit;
    public GameObject stit1;
    public GameObject stit2;
    public GameObject stit3;
    private bool stiti = false;

    public static bool kisaMeteora = false;
    public static bool pritisnuoCrveno=false;
    public static bool pritisnuoPlavo=false;
    public static bool pritisnuoZuto=false;

    public GameObject efektiHolder;
    public static GameObject efektiHolderStatik;

    private void Start()
    {
        kisaMeteora = false;
        pritisnuoCrveno = false;
        pritisnuoPlavo = false;
        pritisnuoZuto = false;

        efektiHolderStatik = efektiHolder;

        plaviEfekat = plavi;
        zutiEfekat = zuti;
        crveniEfekat = crveni;
        narandzastiEfekat = narandzasti;
        ljubicastiEfekat = ljubicasti;
        zeleniEfekat = zeleni;
        crniEfekat = crni;

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

            if (PlayerPrefs.GetInt("colorShield", 0) < 6)
            {
                brojOdbrane = PlayerPrefs.GetInt("colorShield", 0);
            }
            else
            {
                brojOdbrane = 5;
            }

            if(PlayerPrefs.GetInt("hotStreak",15)==15)
            {
                potrebanStreak = 10000;
            }
            else if (PlayerPrefs.GetInt("hotStreak",15)>=5)
            {
                potrebanStreak = PlayerPrefs.GetInt("hotStreak", 15);
            }
            else
            {
                potrebanStreak = 5;
            }

            for (int i = 0; i < brojOdbrane; i++)
            {
                while (true)
                {
                    int rand =Random.Range(0, 6);
                    if (!indeksiOdbrane.Contains(rand))
                    {
                        indeksiOdbrane.Add(rand);
                        break;
                    }
                }
            }

            if (PlayerPrefs.GetInt("mafin",0)==1)
            {
                int randomZaZivote = Random.Range(0, 6);
                ekstra = randomZaZivote;
            }
        }

        if (Meni.kampanja)
        {
            highScore.text = "High Score - " + PlayerPrefs.GetInt("HighScore5Cm", 0);
            skor = PlayerPrefs.GetInt("startScore", 0);

            ukupniDijamanti.SetActive(false);
        }
        else
        {
            highScore.text = "High Score - " + PlayerPrefs.GetInt("HighScore5Cl", 0);
            skor = 0;

            ukupniNovcici.SetActive(false);
        }

        zutoDugme.transform.position = new Vector2((zidL.transform.position.x + crvenoDugme.transform.position.x) / 2 - 0.2f, zutoDugme.transform.position.y);
        plavoDugme.transform.position = new Vector2((zidR.transform.position.x + crvenoDugme.transform.position.x) / 2 + 0.2f, plavoDugme.transform.position.y);
        crvenoDugme.transform.position = new Vector3(crvenoDugme.transform.position.x,crvenoDugme.transform.position.y,plavoDugme.transform.position.z);

        zutiMetak.transform.position = zutiMetakStart.transform.position;
        crveniMetak.transform.position = crveniMetakStart.transform.position;
        plaviMetak.transform.position = plaviMetakStart.transform.position;

        StartCoroutine(Pocetak());

        RectTransform kanvasTransform = goodZone.GetComponent<RectTransform>();

        brzina1 = ((0.05f * kanvasTransform.rect.height) / 1870f) * 30;
    }
    public void Update()
    {
        if (Meni.kampanja)
        {
            if (zaradjeniNovciciAnimator.GetCurrentAnimatorStateInfo(0).IsName("ZaradjeniNovciciStoji") && !krajNovac)
            {
                StartCoroutine(Novcici());
                krajNovac = true;
            }
        }
        else
        {
            if (zaradjeniDijamantiAnimator.GetCurrentAnimatorStateInfo(0).IsName("ZaradjeniNovciciStoji") && !krajNovac)
            {
                StartCoroutine(Dijamanti());
                krajNovac = true;
            }
        }

        TrenutnaBoja();

        Brzina();

        SkorTekst();

        if (!kisaMeteora)
        {
            ProveraPoraza();
        }

        SareneBoje();

        if (!kisaMeteora)
        {
            Trougao();
        }

        Krug();

        Padanje();

        if (!kisaMeteora)
        {
            Rotacija();
        }

        RotacijaTopova();

        if (!kisaMeteora)
        {
            Shoot();
        }

        AnimacijeKraj();

        Tranzicija();

        Tastatura();

        if(brojTrouglova==10)
        {
            brojTrouglova = 0;
            skor+=5*mnozilac;
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
        if (klonovi.Count > 0 && kretanje && !kisaMeteora)
        {
            for (int i = 0; i < klonovi.Count; i++)
            {
                klonovi[i].transform.position = new Vector2(klonovi[i].transform.position.x, klonovi[i].transform.position.y - brzina * Time.deltaTime);
            }
        }
    }
    public void Rotacija()
    {
        if (klonovi.Count > 0)
        {
            for (int i = 0; i < klonovi.Count; i++)
            {
                if (rot == 1)
                {
                    klonovi[i].transform.Rotate(new Vector3(0, 0, brzina * 30 * Time.deltaTime));
                }
                else
                {
                    klonovi[i].transform.Rotate(new Vector3(0, 0, -brzina * 30 * Time.deltaTime));
                }
            }
        }
    }
    public void RotacijaTopova()
    {
        if(klonovi.Count>0 && !kisaMeteora)
        {
            float plaviUgao = Mathf.Atan2(klonovi[0].transform.position.y - plaviTop.transform.position.y, klonovi[0].transform.position.x - plaviTop.transform.position.x)*Mathf.Rad2Deg-90f;
            Quaternion plavaRotacija = Quaternion.Euler(new Vector3(0f, 0f, plaviUgao));
            plaviTop.transform.rotation = Quaternion.RotateTowards(plaviTop.transform.rotation, plavaRotacija, 200f * Time.deltaTime);

            float crveniUgao = Mathf.Atan2(klonovi[0].transform.position.y - crveniTop.transform.position.y, klonovi[0].transform.position.x - crveniTop.transform.position.x) * Mathf.Rad2Deg - 90f;
            Quaternion crvenaRotacija = Quaternion.Euler(new Vector3(0f, 0f, crveniUgao));
            crveniTop.transform.rotation = Quaternion.RotateTowards(crveniTop.transform.rotation, crvenaRotacija, 200f * Time.deltaTime);

            float zutiUgao = Mathf.Atan2(klonovi[0].transform.position.y - zutiTop.transform.position.y, klonovi[0].transform.position.x - zutiTop.transform.position.x) * Mathf.Rad2Deg - 90f;
            Quaternion zutaRotacija = Quaternion.Euler(new Vector3(0f, 0f, zutiUgao));
            zutiTop.transform.rotation = Quaternion.RotateTowards(zutiTop.transform.rotation, zutaRotacija, 200f * Time.deltaTime);
        }
        else
        {
            Quaternion plavaRotacija = Quaternion.Euler(new Vector3(0f, 0f, 0));
            plaviTop.transform.rotation = Quaternion.RotateTowards(plaviTop.transform.rotation, plavaRotacija, 200f * Time.deltaTime);

            Quaternion crvenaRotacija = Quaternion.Euler(new Vector3(0f, 0f, 0));
            crveniTop.transform.rotation = Quaternion.RotateTowards(crveniTop.transform.rotation, crvenaRotacija, 200f * Time.deltaTime);

            Quaternion zutaRotacija = Quaternion.Euler(new Vector3(0f, 0f, 0));
            zutiTop.transform.rotation = Quaternion.RotateTowards(zutiTop.transform.rotation, zutaRotacija, 200f * Time.deltaTime);
        }
    }
    public void TrenutnaBoja()
    {
        if (klonovi.Count > 0 && !kisaMeteora)
        {
            boja = klonovi[0].tag;
        }
    }
    public void SkorTekst()
    {
        poeni.text = "Score:" + skor.ToString();

        ukupanNovac.text = PlayerPrefs.GetInt("Novcici", 0).ToString() + "<sprite=0>";
        ukupanDijamant.text = PlayerPrefs.GetInt("Dijamanti", 0).ToString() + "<sprite=0>";
    }
    public void ProveraPoraza()
    {
        if (!game_over)
        {
            if (Kraj.kraj == true && ekstra == 0 && !odbrana && (!stiti || (stiti && !Kraj.dno)) && !kisaMeteora)
            {
                streak = 0;
                mnozilac = 1;
                Poraz();
                game_over = true;
            }
            else if (Kraj.kraj && (odbrana || (stiti && Kraj.dno) || kisaMeteora))
            {
                if (!kisaMeteora)
                {
                    streak = 0;
                    mnozilac = 1;
                }
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
    public void Poraz()
    {
        skip.SetActive(true);
        if (kristal == 3)
        {
            if (Meni.kampanja)
            {
                novac = PlayerPrefs.GetInt("Novcici", 0) + (skor / skorZaNovcic +bonusNovcici) * 2;
            }
            else
            {
                dijamanti = PlayerPrefs.GetInt("Dijamanti", 0) + (skor / 20) * 2;
            }
        }
        else
        {
            if (Meni.kampanja)
            {
                novac = PlayerPrefs.GetInt("Novcici", 0) + (skor / skorZaNovcic + bonusNovcici);
            }
            else
            {
                dijamanti = PlayerPrefs.GetInt("Dijamanti", 0) + (skor / 20);
            }
        }
        brodAnimator.SetBool("Kraj", true);

        if (Meni.kampanja)
        {
            if (skor > PlayerPrefs.GetInt("HighScore5Cm", 0))
            {
                PlayerPrefs.SetInt("HighScore5Cm", skor);
                highScore.text = "New High Score !!!";
                StartCoroutine(Vatromet());
            }

            if (skor >= 100 && PlayerPrefs.GetInt("Level", 1) == 5)
            {
                PlayerPrefs.SetInt("Level", 6);

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
            if (skor > PlayerPrefs.GetInt("HighScore5Cl", 0))
            {
                PlayerPrefs.SetInt("HighScore5Cl", skor);
                highScore.text = "New High Score !!!";
                StartCoroutine(Vatromet());
            }
        }

        if (kristal == 3)
        {
            if (Meni.kampanja)
            {
                noviNovac.text = "+" + ((skor / skorZaNovcic + bonusNovcici) * 2).ToString() + "<sprite=0>";
            }
            else
            {
                noviDijamant.text = "+" + ((skor / 20) * 2).ToString() + "<sprite=0>";
            }
        }
        else
        {
            if (Meni.kampanja)
            {
                noviNovac.text = "+" + (skor / skorZaNovcic + bonusNovcici).ToString() + "<sprite=0>";
            }
            else
            {
                noviDijamant.text = "+" + (skor / 20).ToString() + "<sprite=0>";
            }
        }

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
        if ((skor / skorZaNovcic +bonusNovcici) != 0)
        {
            if (kristal == 3)
            {
                for (int i = 1; i <= (skor / skorZaNovcic + bonusNovcici)*2; i++)
                {
                    if (!noviNovacBio)
                    {
                        PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) + 1);
                        yield return new WaitForSeconds(0.1f);
                    }
                }
            }
            else
            {
                for (int i = 1; i <= skor / skorZaNovcic +bonusNovcici; i++)
                {
                    if (!noviNovacBio)
                    {
                        PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) + 1);
                        yield return new WaitForSeconds(0.1f);
                    }
                }
            }
        }
    }
    public IEnumerator Dijamanti()
    {
        if ((skor / 20) != 0)
        {
            if (kristal == 3)
            {
                for (int i = 1; i <= (skor / 20) * 2; i++)
                {
                    if (!noviNovacBio)
                    {
                        PlayerPrefs.SetInt("Dijamanti", PlayerPrefs.GetInt("Dijamanti", 0) + 1);
                        yield return new WaitForSeconds(0.1f);
                    }
                }
            }
            else
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
    }
    public void AnimacijeKraj()
    {
        if (brodAnimator.GetCurrentAnimatorStateInfo(0).IsName("Stajanje1"))
        {
            laser.transform.localScale = new Vector2(550, 900);
            laserAnimator.SetBool("Laser", true);
        }

        if (laserAnimator.GetCurrentAnimatorStateInfo(0).IsName("StajanjeLasera"))
        {
            restartAnimator.SetBool("Restart", true);
        }

        if (restartAnimator.GetCurrentAnimatorStateInfo(0).IsName("StajanjeRestart"))
        {
            laserAnimator.SetBool("Laser", false);
        }

        if (laserAnimator.GetCurrentAnimatorStateInfo(0).IsName("New State") && restartAnimator.GetCurrentAnimatorStateInfo(0).IsName("StajanjeRestart"))
        {
            laser.transform.localScale = new Vector2(0, 0);
            brodAnimator.SetBool("Dalje1", true);
        }

        if (brodAnimator.GetCurrentAnimatorStateInfo(0).IsName("Stajanje2"))
        {
            laser.transform.localScale = new Vector2(550, 900);
            laserAnimator.SetBool("Laser", true);
        }

        if (laserAnimator.GetCurrentAnimatorStateInfo(0).IsName("StajanjeLasera") && brodAnimator.GetCurrentAnimatorStateInfo(0).IsName("Stajanje2"))
        {
            meniAnimator.SetBool("Meni", true);
        }

        if (meniAnimator.GetCurrentAnimatorStateInfo(0).IsName("StajanjeMeni"))
        {
            laserAnimator.SetBool("Laser", false);
        }

        if (laserAnimator.GetCurrentAnimatorStateInfo(0).IsName("New State") && meniAnimator.GetCurrentAnimatorStateInfo(0).IsName("StajanjeMeni"))
        {
            laser.transform.localScale = new Vector2(0, 0);
            brodAnimator.SetBool("Dalje2", true);
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
        if (!Kraj.kraj || kisaMeteora)
        {
            Kraj.dno = false;
            odbrana = false;

            int oblik = 0;
            if (!kisaMeteora)
            {
                oblik = Random.Range(0, 10);
            }
            else
            {
                oblik = Random.Range(0, 8);
            }

            if (oblik < 5)
            {
                int r = Random.Range(0, kvadrat.Count);

                if (Meni.kampanja)
                {
                    if (indeksiOdbrane.Contains(r))
                    {
                        odbrana = true;
                    }
                }

                rot = Random.Range(1, 3);
                float x = Random.Range(zidL.transform.position.x + 0.5f, zidR.transform.position.x - 0.5f);

                float y = 8;
                if (!kisaMeteora)
                {
                    y = Random.Range(6f, 8f);
                }

                GameObject klon = Instantiate(kvadrat[r], new Vector2(x, y), Quaternion.identity);
                klonovi.Add(klon);
                if (!kisaMeteora)
                {
                    StartCoroutine(Cekanje());
                }
            }
            if (oblik>=5 && oblik<8)
            {
                int r = Random.Range(0, trougao.Count);

                if (Meni.kampanja)
                {
                    if (indeksiOdbrane.Contains(r))
                    {
                        odbrana = true;
                    }
                }
                rot = Random.Range(1, 3);
                float x = Random.Range(zidL.transform.position.x + 0.5f, zidR.transform.position.x - 0.5f);
                float y = Random.Range(6f, 8f);
                GameObject klon = Instantiate(trougao[r], new Vector2(x, y), Quaternion.identity);
                klonovi.Add(klon);
                if (!kisaMeteora)
                {
                    StartCoroutine(Cekanje());
                }
            }
            if (oblik >=8)
            {
                int r = Random.Range(0, krug.Count);
                rot = Random.Range(1, 3);
                float x = Random.Range(zidL.transform.position.x + 0.5f, zidR.transform.position.x - 0.5f);
                float y = Random.Range(6f, 8f);
                GameObject klon = Instantiate(krug[r], new Vector2(x, y), Quaternion.identity);
                klonovi.Add(klon);
                StartCoroutine(Cekanje());
            }

            if (!kisaMeteora)
            {
                EfekatBrod();
            }

            if (Meni.kampanja && PlayerPrefs.GetInt("antena", 0) == 1 && !stiti && !kisaMeteora)
            {
                int randomZaStit = Random.Range(0, 15);
                if (randomZaStit == 8)
                {
                    StartCoroutine(Stit());
                }
            }

            if (Meni.kampanja && PlayerPrefs.GetInt("satelit", 0) == 1 && !kisaMeteora)
            {
                int randomZaMeteor = Random.Range(0, 25);
                if (randomZaMeteor == 0)
                {
                    for (int i = 0; i < klonovi.Count; i++)
                    {
                        Destroy(klonovi[i]);
                    }
                    klonovi.Clear();
                    StartCoroutine(KisaMeteora());
                }
            }
        }
    }
    public IEnumerator Stit()
    {
        stiti = true;
        stit.SetActive(true);
        stit.GetComponent<Animator>().SetBool("stit",true);
        yield return new WaitForSeconds(0.25f);
        stit1.SetActive(true);
        stit2.SetActive(true);
        stit3.SetActive(true);
        yield return new WaitForSeconds(10f);
        stit.GetComponent<Animator>().SetBool("stit",false);
        yield return new WaitForSeconds(0.0834f);
        stit1.SetActive(false);
        stit2.SetActive(false);
        stit3.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        stit.SetActive(false);
        stiti = false;
    }
    public IEnumerator KisaMeteora()
    {
        kisaMeteora = true;
        StartCoroutine(Kisa());
        yield return new WaitForSeconds(10f);
        Kraj.uZoni = false;
        Kraj.kraj = false;
        Kraj.dno = false;
        for(int i=0;i<klonovi.Count;i++)
        {
            Destroy(klonovi[i]);
        }
        klonovi.Clear();
        crni.transform.position = new Vector2(0, 0);
        crni.Play();
        for (int i = 0; i < klonovi.Count; i++)
        {
            Destroy(klonovi[i]);
        }
        klonovi.Clear();
        kisaMeteora = false;

        int n = efektiHolderStatik.transform.childCount;
        for (int i=n-1;i>=0;i--)
        {
            Destroy(efektiHolderStatik.transform.GetChild(i).gameObject);
        }

        yield return new WaitForSeconds(1f);
        Stvaranje();
    }
    public IEnumerator Kisa()
    {
        if(kisaMeteora)
        {
            Stvaranje();
        }
        yield return new WaitForSeconds(0.25f);
        if (kisaMeteora)
        {
            StartCoroutine(Kisa());
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
        else if (!kisaMeteora)
        {
            StartCoroutine(Cekanje());
        }
    }
    public void klikZuto()
    {
        if (klonovi.Count > 0 && !kisaMeteora)
        {
            if (Kraj.uZoni)
            {
                if (boja == "zuto" || boja == "zutaRuda" && klonovi.Count > 0 && !Kraj.kraj)
                {
                    if(boja=="zuto")
                        kretanje = false;

                    if (!ispaljenMetak)
                    {
                        zutiMetak.transform.rotation = zutiTop.transform.rotation;
                        zutiTop.GetComponent<Animator>().SetTrigger("top");
                        zutiMetak.SetActive(true);
                        zutiMetakMoze = true;
                    }

                }
                else if (boja == "krug")
                {
                    if (klonovi[0].name == "ZutaRuda(Clone)" && !ispaljenMetak)
                    {
                        kretanje = false;
                        zutiMetak.transform.rotation = zutiTop.transform.rotation;
                        zutiTop.GetComponent<Animator>().SetTrigger("top");
                        zutiMetak.SetActive(true);
                        zutiMetakMoze = true;
                    }
                }
                else if ((boja == "narandzasto" || boja == "zeleno" || boja == "crvenaRuda" || boja == "plavaRuda") && klonovi.Count > 0 && !Kraj.kraj)
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
        if (klonovi.Count > 0 && !kisaMeteora)
        {
            if (Kraj.uZoni)
            {
                if (boja == "crveno" || boja == "crvenaRuda" && klonovi.Count > 0 && !Kraj.kraj)
                {
                    if(boja=="crveno")
                        kretanje= false;

                    if (!ispaljenMetak)
                    {
                        crveniMetak.transform.rotation = crveniTop.transform.rotation;
                        crveniTop.GetComponent<Animator>().SetTrigger("top");
                        crveniMetak.SetActive(true);
                        crveniMetakMoze = true;
                    }
                }
                else if (boja == "krug")
                {
                    if (klonovi[0].name == "CrvenaRuda(Clone)" && !ispaljenMetak)
                    {
                        kretanje = false;
                        crveniMetak.transform.rotation = crveniTop.transform.rotation;
                        crveniTop.GetComponent<Animator>().SetTrigger("top");
                        crveniMetak.SetActive(true);
                        crveniMetakMoze = true;
                    }
                }
                else if ((boja == "narandzasto" || boja == "ljubicasto" || boja == "plavaRuda" || boja == "zutaRuda") && klonovi.Count > 0 && !Kraj.kraj)
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
        if (klonovi.Count > 0 && !kisaMeteora)
        {
            if (Kraj.uZoni)
            {
                if (boja == "plavo" || boja == "plavaRuda" && klonovi.Count > 0 && !Kraj.kraj)
                {
                    if(boja=="plavo")
                        kretanje= false;

                    if (!ispaljenMetak)
                    {
                        plaviMetak.transform.rotation = plaviTop.transform.rotation;
                        plaviTop.GetComponent<Animator>().SetTrigger("top");
                        plaviMetak.SetActive(true);
                        plaviMetakMoze = true;
                    }
                }
                else if(boja=="krug")
                {
                    if (klonovi[0].name=="PlavaRuda(Clone)" && !ispaljenMetak)
                    {
                        kretanje = false;
                        plaviMetak.transform.rotation = plaviTop.transform.rotation;
                        plaviTop.GetComponent<Animator>().SetTrigger("top");
                        plaviMetak.SetActive(true);
                        plaviMetakMoze = true;
                    }
                }
                else if ((boja == "ljubicasto" || boja == "zeleno" || boja=="crvenaRuda" || boja=="zutaRuda") && klonovi.Count > 0 && !Kraj.kraj)
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
        plavoDugme.GetComponent<Animator>().SetBool("plavo", true);
        pritisnuoPlavo = true;
    }
    public void PlavoUp()
    {
        plavo = false;
        plavoDugme.GetComponent<Animator>().SetBool("plavo", false);
        pritisnuoPlavo = false;
    }
    public void CrvenoDown()
    {
        crveno = true;
        crvenoDugme.GetComponent<Animator>().SetBool("crveno", true);
        pritisnuoCrveno = true;
    }
    public void CrvenoUp()
    {
        crveno = false;
        crvenoDugme.GetComponent<Animator>().SetBool("crveno", false);
        pritisnuoCrveno = false;
    }
    public void ZutoDown()
    {
        zuto = true;
        zutoDugme.GetComponent<Animator>().SetBool("zuto", true);
        pritisnuoZuto = true;
    }
    public void ZutoUp()
    {
        zuto = false;
        zutoDugme.GetComponent<Animator>().SetBool("zuto", false);
        pritisnuoZuto = false;
    }
    public void SareneBoje()
    {
        if (klonovi.Count > 0 && Kraj.uZoni && !sareniMetak && !kisaMeteora)
        {
            if (boja == "narandzasto" && klonovi.Count > 0 && !Kraj.kraj && zuto && crveno && !plavo)
            {
                kretanje = false;
                zutiMetak.transform.rotation = zutiTop.transform.rotation;
                crveniMetak.transform.rotation = crveniTop.transform.rotation;
                zutiTop.GetComponent<Animator>().SetTrigger("top");
                crveniTop.GetComponent<Animator>().SetTrigger("top");
                zutiMetak.SetActive(true);
                crveniMetak.SetActive(true);
                zutiMetakMoze = true;
                crveniMetakMoze = true;
                sareniMetak = true;
            }

            if (boja == "ljubicasto" && klonovi.Count > 0 && !Kraj.kraj && !zuto && crveno && plavo)
            {
                kretanje = false;
                plaviMetak.transform.rotation = plaviTop.transform.rotation;
                crveniMetak.transform.rotation= crveniTop.transform.rotation;
                crveniTop.GetComponent<Animator>().SetTrigger("top");
                plaviTop.GetComponent<Animator>().SetTrigger("top");
                plaviMetak.SetActive(true);
                crveniMetak.SetActive(true);
                plaviMetakMoze = true;
                crveniMetakMoze = true;
                sareniMetak = true;
            }

            if (boja == "zeleno" && klonovi.Count > 0 && !Kraj.kraj && zuto && !crveno && plavo)
            {
                kretanje = false;
                plaviMetak.transform.rotation = plaviTop.transform.rotation;
                zutiMetak.transform.rotation = zutiTop.transform.rotation;
                zutiTop.GetComponent<Animator>().SetTrigger("top");
                plaviTop.GetComponent<Animator>().SetTrigger("top");
                plaviMetak.SetActive(true);
                zutiMetak.SetActive(true);
                plaviMetakMoze = true;
                zutiMetakMoze = true;
                sareniMetak = true;
            }
        }
    }
    public void Trougao()
    {
        if (boja == "trougao" && klonovi.Count > 0)
        {
            if (Kraj.dno && !Kraj.kraj)
            {
                if (klonovi[0].name == "PlaviTrougaoMeteor(Clone)" || klonovi[0].name == "PlavaRuda(Clone)")
                {
                    plavi.transform.position = klonovi[0].transform.position;
                    plavi.Play();
                }
                if (klonovi[0].name == "CrveniTrougaoMeteor(Clone)" || klonovi[0].name == "CrvenaRuda(Clone)")
                {
                    crveni.transform.position = klonovi[0].transform.position;
                    crveni.Play();
                }
                if (klonovi[0].name == "ZutiTrougaoMeteor(Clone)" || klonovi[0].name == "ZutaRuda(Clone)")
                {
                    zuti.transform.position = klonovi[0].transform.position;
                    zuti.Play();
                }
                if (klonovi[0].name == "LjubicastiTrougaoMeteor(Clone)")
                {
                    ljubicasti.transform.position = klonovi[0].transform.position;
                    ljubicasti.Play();
                }
                if (klonovi[0].name == "ZeleniTrougaoMeteor(Clone)")
                {
                    zeleni.transform.position = klonovi[0].transform.position;
                    zeleni.Play();
                }
                if (klonovi[0].name == "NarandzastiTrougaoMeteor(Clone)")
                {
                    narandzasti.transform.position = klonovi[0].transform.position;
                    narandzasti.Play();
                }

                Destroy(klonovi[0]);
                klonovi.RemoveAt(0);

                if(Meni.kampanja)
                {
                    streak++;
                    if (streak%potrebanStreak==0 && streak!=0)
                    {
                        mnozilac++;
                    }

                    brojTrouglova++;
                }

                skor+=mnozilac;
                Kraj.uZoni = false;
                Kraj.dno = false;
            }
        }
    }
    public void Krug()
    {
        if ((boja == "crvenaRuda" || boja=="plavaRuda" || boja=="zutaRuda" || boja=="krug") && klonovi.Count > 0)
        {
            if (Kraj.dno)
            {
                crni.transform.position = klonovi[0].transform.position;
                crni.Play();

                Destroy(klonovi[0]);
                klonovi.RemoveAt(0);
                Kraj.uZoni = false;
                Kraj.dno = false;
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
            streak = 0;
            mnozilac = 1;
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
    public void Shoot()
    {
        if (klonovi.Count > 0)
        {
            if (zutiMetakMoze && zutiTop.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("moze"))
            {
                zutiMetak.transform.position = Vector2.MoveTowards(zutiMetak.transform.position, klonovi[0].transform.position, 50f * Time.deltaTime);
                ispaljenMetak = true;
            }
            if (crveniMetakMoze && crveniTop.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("moze"))
            {
                crveniMetak.transform.position = Vector2.MoveTowards(crveniMetak.transform.position, klonovi[0].transform.position, 50f * Time.deltaTime);
                ispaljenMetak = true;
            }
            if (plaviMetakMoze && plaviTop.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("moze"))
            {
                plaviMetak.transform.position = Vector2.MoveTowards(plaviMetak.transform.position, klonovi[0].transform.position, 50f * Time.deltaTime);
                ispaljenMetak = true;
            }

            if (Metak.metak)
            {
                if (boja == "plavo" || boja=="plavaRuda")
                {
                    plavi.transform.position = klonovi[0].transform.position;
                    plavi.Play();
                }
                if (boja == "crveno" || boja == "crvenaRuda")
                {
                    crveni.transform.position = klonovi[0].transform.position;
                    crveni.Play();
                }
                if (boja == "zuto" || boja == "zutaRuda")
                {
                    zuti.transform.position = klonovi[0].transform.position;
                    zuti.Play();
                }
                if (boja == "narandzasto")
                {
                    narandzasti.transform.position = klonovi[0].transform.position;
                    narandzasti.Play();
                }
                if (boja == "ljubicasto")
                {
                    ljubicasti.transform.position = klonovi[0].transform.position;
                    ljubicasti.Play();
                }
                if (boja == "zeleno")
                {
                    zeleni.transform.position = klonovi[0].transform.position;
                    zeleni.Play();
                }
                if(boja == "krug" && klonovi[0].name == "ZutaRuda(Clone)")
                {
                    kristalZuti.transform.position = klonovi[0].transform.position;
                    kristalZuti.Play();
                }
                if (boja == "krug" && klonovi[0].name == "CrvenaRuda(Clone)")
                {
                    kristalCrveni.transform.position = klonovi[0].transform.position;
                    kristalCrveni.Play();
                }
                if (boja == "krug" && klonovi[0].name == "PlavaRuda(Clone)")
                {
                    kristalPlavi.transform.position = klonovi[0].transform.position;
                    kristalPlavi.Play();
                }

                if (boja != "crvenaRuda" && boja != "zutaRuda" && boja != "plavaRuda" && boja!="krug")
                {
                    if (Meni.kampanja)
                    {
                        streak++;
                        if (streak % potrebanStreak == 0 && streak != 0)
                        {
                            mnozilac++;
                        }
                    }

                    skor += mnozilac;

                    Destroy(klonovi[0]);
                    klonovi.RemoveAt(0);
                    Kraj.uZoni = false;
                }
                else if (boja == "krug" && klonovi[0].name == "ZutaRuda(Clone)")
                {
                    if (Meni.kampanja)
                    {
                        PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) + 1);
                    }
                    else
                    {
                        PlayerPrefs.SetInt("Dijamanti", PlayerPrefs.GetInt("Dijamanti", 0) + 1);
                    }

                    if (!bioZuti)
                    {
                        bioZuti = true;

                        if(kristal==0)
                        {
                            prviKristal.GetComponent<Image>().sprite = zutiKristal;
                            kristal++;
                        }
                        else if(kristal==1)
                        {
                            drugiKristal.GetComponent<Image>().sprite = zutiKristal;
                            kristal++;
                        }
                        else
                        {
                            treciKristal.GetComponent<Image>().sprite = zutiKristal;
                            kristal++;
                        }
                    }

                    Destroy(klonovi[0]);
                    klonovi.RemoveAt(0);
                    Kraj.uZoni = false;
                }
                else if (boja == "krug" && klonovi[0].name == "CrvenaRuda(Clone)")
                {
                    ekstra++;

                    if (!bioCrveni)
                    {
                        bioCrveni = true;

                        if (kristal == 0)
                        {
                            prviKristal.GetComponent<Image>().sprite = crveniKristal;
                            kristal++;
                        }
                        else if (kristal == 1)
                        {
                            drugiKristal.GetComponent<Image>().sprite = crveniKristal;
                            kristal++;
                        }
                        else
                        {
                            treciKristal.GetComponent<Image>().sprite = crveniKristal;
                            kristal++;
                        }
                    }

                    Destroy(klonovi[0]);
                    klonovi.RemoveAt(0);
                    Kraj.uZoni = false;
                }
                else if (boja == "krug" && klonovi[0].name == "PlavaRuda(Clone)")
                {
                    if (Meni.kampanja)
                    {
                        streak++;
                        if (streak % potrebanStreak == 0 && streak != 0)
                        {
                            mnozilac++;
                        }
                    }

                    skor += 5*mnozilac;

                    if (!bioPlavi)
                    {
                        bioPlavi = true;

                        if (kristal == 0)
                        {
                            prviKristal.GetComponent<Image>().sprite = plaviKristal;
                            kristal++;
                        }
                        else if (kristal == 1)
                        {
                            drugiKristal.GetComponent<Image>().sprite = plaviKristal;
                            kristal++;
                        }
                        else
                        {
                            treciKristal.GetComponent<Image>().sprite = plaviKristal;
                            kristal++;
                        }
                    }

                    Destroy(klonovi[0]);
                    klonovi.RemoveAt(0);
                    Kraj.uZoni = false;
                }
                else
                {
                    klonovi[0].transform.localScale = new Vector2(3, 3);
                }
                

                if(boja=="crvenaRuda")
                {
                    int r;
                    if (Meni.kampanja)
                    {
                        if(PlayerPrefs.GetInt("pistoljKvadrat",0)==1 && PlayerPrefs.GetInt("pistoljTrougao", 0) == 1)
                        {
                            r = 10;
                        }
                        else if (PlayerPrefs.GetInt("pistoljKvadrat", 0) == 1)
                        {
                            int p = Random.Range(0, 8);
                            if (p < 3)
                            {
                                r = 0;
                            }
                            else
                            {
                                r = 10;
                            }
                        }
                        else if (PlayerPrefs.GetInt("pistoljTrougao", 0) == 1)
                        {
                            r = Random.Range(3, 10);
                        }
                        else
                        {
                            r = Random.Range(0, 10);
                        }
                    }
                    else
                    {
                        r = Random.Range(0, 10);
                    }

                    if(r<3)
                    {
                        klonovi[0].tag = "trougao";
                        klonovi[0].GetComponent<SpriteRenderer>().sprite = crveniTrougao;
                    }
                    if (r>=3 && r<5)
                    {
                        klonovi[0].tag = "crveno";
                        klonovi[0].GetComponent<SpriteRenderer>().sprite = crveniKvadrat;
                        klonovi[0].GetComponent<Kraj>().kvadrat = true;
                    }
                    if (r >=5)
                    {
                        klonovi[0].tag = "krug";
                        klonovi[0].GetComponent<SpriteRenderer>().sprite = crveniKristal;
                    }
                }
                if (boja == "plavaRuda")
                {
                    int r;
                    if (Meni.kampanja)
                    {
                        if (PlayerPrefs.GetInt("pistoljKvadrat", 0) == 1 && PlayerPrefs.GetInt("pistoljTrougao", 0) == 1)
                        {
                            r = 10;
                        }
                        else if (PlayerPrefs.GetInt("pistoljKvadrat", 0) == 1)
                        {
                            int p = Random.Range(0, 8);
                            if (p < 3)
                            {
                                r = 0;
                            }
                            else
                            {
                                r = 10;
                            }
                        }
                        else if (PlayerPrefs.GetInt("pistoljTrougao", 0) == 1)
                        {
                            r = Random.Range(3, 10);
                        }
                        else
                        {
                            r = Random.Range(0, 10);
                        }
                    }
                    else
                    {
                        r = Random.Range(0, 10);
                    }

                    if (r <3)
                    {
                        klonovi[0].tag = "trougao";
                        klonovi[0].GetComponent<SpriteRenderer>().sprite = plaviTrougao;
                    }
                    if (r>=3 && r<5)
                    {
                        klonovi[0].tag = "plavo";
                        klonovi[0].GetComponent<SpriteRenderer>().sprite = plaviKvadrat;
                        klonovi[0].GetComponent<Kraj>().kvadrat = true;
                    }
                    if (r >= 5)
                    {
                        klonovi[0].tag = "krug";
                        klonovi[0].GetComponent<SpriteRenderer>().sprite = plaviKristal;
                    }
                }
                if (boja == "zutaRuda")
                {
                    int r;
                    if (Meni.kampanja)
                    {
                        if (PlayerPrefs.GetInt("pistoljKvadrat", 0) == 1 && PlayerPrefs.GetInt("pistoljTrougao", 0) == 1)
                        {
                            r = 10;
                        }
                        else if (PlayerPrefs.GetInt("pistoljKvadrat", 0) == 1)
                        {
                            int p = Random.Range(0, 8);
                            if (p < 3)
                            {
                                r = 0;
                            }
                            else
                            {
                                r = 10;
                            }
                        }
                        else if (PlayerPrefs.GetInt("pistoljTrougao", 0) == 1)
                        {
                            r = Random.Range(3, 10);
                        }
                        else
                        {
                            r = Random.Range(0, 10);
                        }
                    }
                    else
                    {
                        r = Random.Range(0, 10);
                    }

                    if (r < 3)
                    {
                        klonovi[0].tag = "trougao";
                        klonovi[0].GetComponent<SpriteRenderer>().sprite = zutiTrougao;
                    }
                    if (r >= 3 && r < 5)
                    {
                        klonovi[0].tag = "zuto";
                        klonovi[0].GetComponent<SpriteRenderer>().sprite = zutiKvadrat;
                        klonovi[0].GetComponent<Kraj>().kvadrat = true;
                    }
                    if (r >= 5)
                    {
                        klonovi[0].tag = "krug";
                        klonovi[0].GetComponent<SpriteRenderer>().sprite = zutiKristal;
                    }
                }

                zutiMetak.SetActive(false);
                crveniMetak.SetActive(false);
                plaviMetak.SetActive(false);
                zutiMetak.transform.position = zutiMetakStart.transform.position;
                crveniMetak.transform.position = crveniMetakStart.transform.position;
                plaviMetak.transform.position = plaviMetakStart.transform.position;
                zutiMetak.GetComponent<TrailRenderer>().Clear();
                crveniMetak.GetComponent<TrailRenderer>().Clear();
                plaviMetak.GetComponent<TrailRenderer>().Clear();
                zutiMetakMoze = false;
                crveniMetakMoze = false;
                plaviMetakMoze = false;
                kretanje = true;
                Metak.metak = false;
                sareniMetak=false;
                ispaljenMetak = false;
            }
        }
    }
    public void EfekatBrod()
    {
        int p = Random.Range(0, 9);

        if (p == 8)
        {
            float r = Random.Range(zidD.transform.position.y, zidU.transform.position.y);

            brodici.transform.position = new Vector2(zidR.transform.position.x + 1, r);
            brodici.Play();
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
        laser.SetActive(false);
        brodAnimator.SetTrigger("skip");
        restartAnimator.SetTrigger("skip");
        meniAnimator.SetTrigger("skip");
        noviNovac.enabled = false;
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
        if (Meni.kampanja)
        {
            PlayerPrefs.SetInt("Novcici", novac);
        }
        else
        {
            PlayerPrefs.SetInt("Dijamanti", dijamanti);
        }

        SceneManager.LoadScene(6);
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
            pritisnuoZuto = true;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            klikCrveno();
            crveno = true;
            pritisnuoCrveno = true;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            klikPlavo();
            plavo = true;
            pritisnuoPlavo = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            zuto = false;
            pritisnuoZuto = false;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            crveno = false;
            pritisnuoCrveno = false;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            plavo = false;
            pritisnuoPlavo = false;
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
