using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Kopanje : MonoBehaviour
{
    public GameObject tranzicija;
    public Animator tranzicijaAnimator;

    public TextMeshProUGUI skorText;
    public TextMeshProUGUI highScoreText;

    public ParticleSystem noviHSEfekat;

    public GameObject pozadinaBlokova;

    public GameObject brod;
    public GameObject laser;
    private Animator laserAnimator;

    public GameObject[,] blokoviObjekti = new GameObject[20, 5];//blokovi
    private bool[,] blokoviMoze = new bool[20, 5];//da li moze da se selektuje
    private bool[,] blokoviSelekt = new bool[20, 5];//da li je selektovan
    private bool[,] blokoviX = new bool[20, 5];//da li je stavljenX
    private Dictionary<Tuple<int,int>,List<Tuple<int,int>>> blokoviOblasti= new Dictionary<Tuple<int, int>, List<Tuple<int, int>>>();
    public List<Image> blokovi;
    public List<Sprite> blokoviSlike;
    public List<Sprite> blokoviSlikeSelekt;
    public List<Sprite> blokoviSlikeX;

    public List<GameObject> blokoviLava=new List<GameObject>();
    public List<GameObject> blokoviKristali=new List<GameObject>();
    private List<GameObject> nedodeljeniKristali = new List<GameObject>();
    private List<Tuple<int,int>> blokoviPutanje = new List<Tuple<int, int>>();
    private List<Tuple<int, int>> zauzetePozicijeKristala = new List<Tuple<int, int>>();    
    private Tuple<int, int> pozicijaCrveni;
    private Tuple<int, int> pozicijaPlavi;
    private Tuple<int, int> pozicijaZuti;
    private int verovatnocaLave = 4;
    public List<int> verovatnocaKristala;

    private List<Tuple<int, int>> pozicijeLave = new List<Tuple<int, int>>();

    public int skor = 0;

    private bool kraj = false;
    private bool poraz = false;
    private Vector2 pocetnaPozicijaBroda;

    public float brzina=10f;
    private Tuple<int, int> ciljniBlok;

    public GameObject crveniKristal;
    public GameObject plaviKristal;
    public GameObject zutiKristal;
    private bool crveni = false;
    private bool plavi = false;
    private bool zuti = false;
    private int brojKristala = 0;

    private static int opstiSkor=0;
    void Start()
    {
        laserAnimator = laser.GetComponent<Animator>();
        laserAnimator.speed = 2f;
        pocetnaPozicijaBroda = brod.GetComponent<RectTransform>().anchoredPosition;
        skor=opstiSkor;

        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if(i == 0)
                {
                    blokoviMoze[i, j] = true;
                }
                else
                {
                    blokoviMoze[i, j] = false;
                }

                blokoviSelekt[i, j] = false;
            }
        }

        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                blokoviObjekti[i, j] = pozadinaBlokova.transform.GetChild(3+i*5+j).gameObject;
            }
        }

        blokoviOblasti.Add(new Tuple<int, int>(0,0), new List<Tuple<int, int>>());
        blokoviOblasti.Add(new Tuple<int, int>(0, 1), new List<Tuple<int, int>>());
        blokoviOblasti.Add(new Tuple<int, int>(0, 2), new List<Tuple<int, int>>());
        blokoviOblasti.Add(new Tuple<int, int>(0, 3), new List<Tuple<int, int>>());
        blokoviOblasti.Add(new Tuple<int, int>(0, 4), new List<Tuple<int, int>>());

        for (int i = 0; i < blokoviKristali.Count; i++)
        {
            nedodeljeniKristali.Add(blokoviKristali[i]);
        }

        for (int i = 0; i < blokoviKristali.Count; i++)
        {
            napraviPutanju();
        }

        postavljanjeLave();

        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                int brojLave = 0;
                if(i!=0)
                {
                    if (pozicijeLave.Contains(new Tuple<int, int>(i - 1, j)))
                    {
                        brojLave++;
                    }
                    if (j != 0)
                    {
                        if (pozicijeLave.Contains(new Tuple<int, int>(i - 1, j - 1)))
                        {
                            brojLave++;
                        }
                    }
                    if (j != 4)
                    {
                        if (pozicijeLave.Contains(new Tuple<int, int>(i - 1, j + 1)))
                        {
                            brojLave++;
                        }
                    }
                }
                if (i != 19)
                {
                    if (pozicijeLave.Contains(new Tuple<int, int>(i + 1, j)))
                    {
                        brojLave++;
                    }
                    if (j != 0)
                    {
                        if (pozicijeLave.Contains(new Tuple<int, int>(i + 1, j - 1)))
                        {
                            brojLave++;
                        }
                    }
                    if (j != 4)
                    {
                        if (pozicijeLave.Contains(new Tuple<int, int>(i + 1, j + 1)))
                        {
                            brojLave++;
                        }
                    }
                }
                if (j != 0)
                {
                    if (pozicijeLave.Contains(new Tuple<int, int>(i, j - 1)))
                    {
                        brojLave++;
                    }
                }
                if (j != 4)
                {
                    if (pozicijeLave.Contains(new Tuple<int, int>(i, j + 1)))
                    {
                        brojLave++;
                    }
                }

                blokoviObjekti[i, j].GetComponent<Image>().sprite = blokoviSlike[brojLave];
            }
        }

        StartCoroutine(Pocetak());
    }
    public void napraviPutanju()
    {
        int random = UnityEngine.Random.Range(0, 5);
        Tuple<int, int> trenutni = new Tuple<int, int>(0, random);

        int randomZaNivo = UnityEngine.Random.Range(0, 100);
        int nivo=0;
        for (int i = 0; i < verovatnocaKristala.Count; i++)
        {
            if (randomZaNivo < verovatnocaKristala[i])
            {
                nivo = i;
                break;
            }
        }

        while (true)
        {
            blokoviPutanje.Add(trenutni);
            
            if (trenutni.Item1==nivo && !zauzetePozicijeKristala.Contains(trenutni)) 
            {
                int randomZaKristal = UnityEngine.Random.Range(0, nedodeljeniKristali.Count);
                nedodeljeniKristali[randomZaKristal].transform.position=  new Vector2(blokoviObjekti[trenutni.Item1,trenutni.Item2].transform.position.x, blokoviObjekti[trenutni.Item1, trenutni.Item2].transform.position.y);
                if (nedodeljeniKristali[randomZaKristal].name=="CrveniKristal")
                {
                    pozicijaCrveni = trenutni;
                }
                else if (nedodeljeniKristali[randomZaKristal].name == "PlaviKristal")
                {
                    pozicijaPlavi = trenutni;
                }
                else
                {
                    pozicijaZuti = trenutni;
                }
                nedodeljeniKristali.RemoveAt(randomZaKristal);
                zauzetePozicijeKristala.Add(trenutni);
                break;
            }
            else if(trenutni.Item1==nivo)
            {
                if(trenutni.Item2!=4)
                {
                    trenutni= new Tuple<int, int>(trenutni.Item1, trenutni.Item2 + 1);
                }
                else
                {
                    trenutni = new Tuple<int, int>(trenutni.Item1, 0);
                }
                continue;
            }

            if(trenutni.Item2!=0 && trenutni.Item2!=4)
            {
                int randomZaSmer = UnityEngine.Random.Range(0, 3);
                if(randomZaSmer==0)
                {
                    trenutni = new Tuple<int, int>(trenutni.Item1 + 1, trenutni.Item2);
                }
                else if(randomZaSmer==1)
                {
                    trenutni = new Tuple<int, int>(trenutni.Item1, trenutni.Item2 + 1);
                }
                else
                {
                    trenutni = new Tuple<int, int>(trenutni.Item1, trenutni.Item2 - 1);
                }

            }
            else if(trenutni.Item2==0)
            {
                int randomZaSmer = UnityEngine.Random.Range(0, 2);
                if (randomZaSmer == 0)
                {
                    trenutni = new Tuple<int, int>(trenutni.Item1 + 1, trenutni.Item2);
                }
                else
                {
                    trenutni = new Tuple<int, int>(trenutni.Item1, trenutni.Item2 + 1);
                }
            }
            else
            {
                int randomZaSmer = UnityEngine.Random.Range(0, 2);
                if (randomZaSmer == 0)
                {
                    trenutni = new Tuple<int, int>(trenutni.Item1 + 1, trenutni.Item2);
                }
                else
                {
                    trenutni = new Tuple<int, int>(trenutni.Item1, trenutni.Item2 - 1);
                }
            }
        }
    }
    public void postavljanjeLave()
    {
        for(int i=0;i<20;i++)
        {
            for(int j=0;j<5;j++)
            {
                if(blokoviPutanje.Contains(new Tuple<int, int>(i,j))==false)
                {
                    int random = UnityEngine.Random.Range(0, 100);
                    if(random<verovatnocaLave)
                    {
                        GameObject klon= Instantiate(blokoviLava[UnityEngine.Random.Range(0, blokoviLava.Count)], blokoviObjekti[i, j].transform.position, Quaternion.identity,pozadinaBlokova.transform);
                        klon.transform.SetSiblingIndex(0);
                        pozicijeLave.Add(new Tuple<int, int>(i, j));
                    }
                }
            }
            verovatnocaLave+=4;
        }
    }
    public void klikBrod()
    {
        brod.GetComponent<Button>().enabled = false;
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                blokoviObjekti[i,j].GetComponent<Button>().enabled=false;
            }
        }
        List<Tuple<int, int>> zaPosetu = new List<Tuple<int, int>>();

        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (blokoviSelekt[i, j] == true)
                {
                    zaPosetu.Add(new Tuple<int, int>(i, j));
                }
            }
        }

        if (zaPosetu.Count == 0)
        {
            Kraj(false);
            return;
        }

        StartCoroutine(PomerajBrod(zaPosetu));
    }

    private IEnumerator PomerajBrod(List<Tuple<int, int>> zaPosetu)
    {
        RectTransform rectTransform = brod.GetComponent<RectTransform>();

        if (!kraj)
        {
            ciljniBlok = zaPosetu[0];
            zaPosetu.RemoveAt(0);
        }

        Vector2 ciljnaPozicija;
        if (kraj ==false)
        {
            ciljnaPozicija = new Vector2(blokoviObjekti[ciljniBlok.Item1, ciljniBlok.Item2].GetComponent<RectTransform>().anchoredPosition.x, blokoviObjekti[ciljniBlok.Item1, ciljniBlok.Item2].GetComponent<RectTransform>().anchoredPosition.y + 220);
        }
        else
        {
            ciljnaPozicija = pocetnaPozicijaBroda;
        }

        while (Vector2.Distance(rectTransform.anchoredPosition, ciljnaPozicija) > 0.1f)
        {
            rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, ciljnaPozicija, brzina * Time.deltaTime);
            yield return null;
        }

        if (kraj == false)
        {
            StartCoroutine(Unistavanje(zaPosetu, ciljniBlok));
        }
        else
        {
            Kraj(poraz);
        }
    }
    public IEnumerator Unistavanje(List<Tuple<int, int>> zaPosetu,Tuple<int,int> trenutniBlok)
    {
        laserAnimator.SetBool("Laser", true);
        yield return new WaitForSeconds(0.1f);
        blokoviObjekti[trenutniBlok.Item1, trenutniBlok.Item2].GetComponent<Image>().enabled=false;
        blokoviObjekti[trenutniBlok.Item1, trenutniBlok.Item2].GetComponentInChildren<ParticleSystem>().Play();
        //yield return new WaitForSeconds(0.1f);
        if (pozicijeLave.Contains(trenutniBlok))
        {
            kraj = true;
            poraz = true;
        }
        else if (zauzetePozicijeKristala.Contains(trenutniBlok))
        {
            skor++;

            if (trenutniBlok.Equals(pozicijaCrveni))
            {
                crveni = true;
            }
            else if (trenutniBlok.Equals(pozicijaPlavi))
            {
                plavi = true;
            }
            else
            {
                zuti = true;
            }
        }

        laserAnimator.SetBool("Laser", false);
        yield return new WaitForSeconds(0.5f);
        if (zaPosetu.Count != 0)
        {
            StartCoroutine(PomerajBrod(zaPosetu));
        }
        else
        {
            kraj = true;
            poraz = false;
            StartCoroutine(PomerajBrod(zaPosetu));
        }
    }
    public void klik()
    {
        string ime = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;

        bool ima = false;
        for(int i=0;i<20;i++)
        {
            for(int j=0;j<5;j++)
            {
                if(ime == blokoviObjekti[i, j].name)
                {
                    ima = true;
                    if (blokoviMoze[i, j] == true)
                    {
                        if (blokoviSelekt[i, j] == false && blokoviX[i, j] == false)
                        {
                            int indeks = 0;
                            for (int k = 0; k < blokoviSlikeSelekt.Count; k++)
                            {
                                if (blokoviObjekti[i, j].GetComponent<Image>().sprite == blokoviSlike[k])
                                {
                                    indeks = k;
                                    break;
                                }
                            }
                            blokoviObjekti[i, j].GetComponent<Image>().sprite = blokoviSlikeSelekt[indeks];

                            if (i != 19)
                            {
                                blokoviMoze[i + 1, j] = true;
                            }
                            if (i != 0)
                            {
                                blokoviMoze[i - 1, j] = true;
                            }
                            if (j != 4)
                            {
                                blokoviMoze[i, j + 1] = true;
                            }
                            if (j != 0)
                            {
                                blokoviMoze[i, j - 1] = true;
                            }

                            blokoviSelekt[i, j] = !blokoviSelekt[i, j];
                        }
                        else if (blokoviX[i,j]==true)
                        {
                            int indeks = 0;
                            for (int k = 0; k < blokoviSlikeX.Count; k++)
                            {
                                if (blokoviObjekti[i, j].GetComponent<Image>().sprite == blokoviSlikeX[k])
                                {
                                    indeks = k;
                                    break;
                                }
                            }
                            blokoviObjekti[i, j].GetComponent<Image>().sprite = blokoviSlike[indeks];
                            blokoviX[i, j] = false;
                        }
                        else
                        {
                            int indeks = 0;
                            for (int k = 0; k < blokoviSlikeSelekt.Count; k++)
                            {
                                if (blokoviObjekti[i, j].GetComponent<Image>().sprite == blokoviSlikeSelekt[k])
                                {
                                    indeks = k;
                                    break;
                                }
                            }
                            blokoviObjekti[i, j].GetComponent<Image>().sprite = blokoviSlikeX[indeks];

                            blokoviSelekt[i, j] = !blokoviSelekt[i, j];
                            blokoviX[i, j] = true;

                            odselektuj();
                        }

                    }
                    else if (blokoviX[i, j] == true)
                    {
                        int indeks = 0;
                        for (int k = 0; k < blokoviSlikeX.Count; k++)
                        {
                            if (blokoviObjekti[i, j].GetComponent<Image>().sprite == blokoviSlikeX[k])
                            {
                                indeks = k;
                                break;
                            }
                        }
                        blokoviObjekti[i, j].GetComponent<Image>().sprite = blokoviSlike[indeks];
                        blokoviX[i, j] = false;
                    }
                    break;
                }
            }
            if(ima == true)
            {
                break;
            }
        }
    }
    public void odselektuj()
    { 
        foreach(var kljuc in blokoviOblasti.Keys)
        {
            blokoviOblasti[kljuc].Clear();
            if (blokoviSelekt[kljuc.Item1, kljuc.Item2] == true)
            {
                napraviOblast(kljuc,kljuc.Item1,kljuc.Item2);
            }
        }

        for (int i = 1;i<20;i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (blokoviSelekt[i, j] == true)
                {
                    bool ima = false;
                    foreach (var kljuc in blokoviOblasti.Keys)
                    {
                        if (blokoviOblasti[kljuc].Contains(new Tuple<int, int>(i, j)))
                        {
                            ima = true;
                            break;
                        }
                    }
                    if (ima == false)
                    {
                        blokoviMoze[i, j] = false;
                        blokoviSelekt[i, j] = false;

                        int indeks = 0;
                        for (int k = 0; k < blokoviSlikeSelekt.Count; k++)
                        {
                            if (blokoviObjekti[i, j].GetComponent<Image>().sprite == blokoviSlikeSelekt[k])
                            {
                                indeks = k;
                                break;
                            }
                        }
                        blokoviObjekti[i, j].GetComponent<Image>().sprite = blokoviSlike[indeks];
                    }
                }
            }
        }   

        for (int i = 1; i < 20; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (blokoviSelekt[i, j] == false && blokoviMoze[i,j]==true)
                {
                    bool ima = false;
                    if(i!=19)
                    {
                        if (blokoviSelekt[i + 1, j] == true)
                        {
                            ima = true;
                        }
                    }
                    if (i != 0)
                    {
                        if (blokoviSelekt[i - 1, j] == true)
                        {
                            ima = true;
                        }
                    }
                    if (j != 4)
                    {
                        if (blokoviSelekt[i, j + 1] == true)
                        {
                            ima = true;
                        }
                    }
                    if (j != 0)
                    {
                        if (blokoviSelekt[i, j - 1] == true)
                        {
                            ima = true;
                        }
                    }
                    if (ima == false)
                    {
                        blokoviMoze[i, j] = false;
                    }
                }
            }
        }
    }
    public void napraviOblast(Tuple<int,int> kljuc,int i,int j)
    {
        if (i != 0)
        {
            if (blokoviSelekt[i - 1, j] == true && !blokoviOblasti[kljuc].Contains(new Tuple<int,int>(i-1,j)))
            {
                blokoviOblasti[kljuc].Add(new Tuple<int, int>(i - 1, j));
                napraviOblast(kljuc,i-1,j);
            }
        }
        if (i != 19)
        {
            if (blokoviSelekt[i + 1, j] == true && !blokoviOblasti[kljuc].Contains(new Tuple<int, int>(i + 1, j)))
            {
                blokoviOblasti[kljuc].Add(new Tuple<int, int>(i + 1, j));
                napraviOblast(kljuc,i+1,j);
            }
        }
        if (j != 0)
        {
            if (blokoviSelekt[i, j - 1] == true && !blokoviOblasti[kljuc].Contains(new Tuple<int, int>(i, j-1)))
            {
                blokoviOblasti[kljuc].Add(new Tuple<int, int>(i, j - 1));
                napraviOblast(kljuc,i,j-1);
            }
        }
        if (j != 4)
        {
            if (blokoviSelekt[i, j + 1] == true && !blokoviOblasti[kljuc].Contains(new Tuple<int, int>(i, j + 1)))
            {
                blokoviOblasti[kljuc].Add(new Tuple<int, int>(i,j + 1));
                napraviOblast(kljuc,i,j+1);
            }
        }
    }
    void Update()
    {
        skorText.text = "Score: " + skor;
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScoreKopanje", 0);

        if(crveni)
        {
            crveniKristal.SetActive(true);
            crveni = false;
            brojKristala++;
        }
        if(plavi)
        {
            plaviKristal.SetActive(true);
            plavi = false;
            brojKristala++;
        }
        if(zuti)
        {
            zutiKristal.SetActive(true);
            zuti = false;
            brojKristala++;
        }
        if(brojKristala==3)
        {
            skor += 10;
            brojKristala = 0;
        }

        Tranzicija();
    }
    public void Kraj(bool poraz)
    {
        if (skor > PlayerPrefs.GetInt("HighScoreKopanje", 0) && poraz)
        {
            PlayerPrefs.SetInt("HighScoreKopanje", skor);
            noviHSEfekat.Play();
        }

        if(!poraz)
        {
            opstiSkor = skor;
        }
        else
        {
            opstiSkor = 0;
        }

        StartCoroutine(kopanje());
    }
    public IEnumerator kopanje()
    {
        yield return new WaitForSecondsRealtime(2f);
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene(12);
    }
    public IEnumerator Pocetak()
    {
        yield return new WaitForSeconds(0.1f);
        tranzicijaAnimator.SetBool("tranzicija1", true);

        yield return new WaitForSeconds(1.5f);
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
