using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class Pecanje : MonoBehaviour
{
    public GameObject tranzicija;
    public Animator tranzicijaAnimator;

    public TextMeshProUGUI skorText;
    public TextMeshProUGUI highScoreText;

    public ParticleSystem noviHSEfekat;

    public Animator ajkula;

    public GameObject igla;

    public int skor = 0;

    public GameObject nivo;
    public GameObject dno;
    public GameObject udica;

    public List<GameObject> ribe=new List<GameObject>();
    private float sirinaEkrana;
    private float pozicija1X;
    private float pozicija2X=0;
    private float pozicija3X;

    private float konstantaPomerajaRibaY=5f;

    public float konstantaPomerajaKamereY=5f;

    private int brojTrenutnogNivoa = 0;
    private int trenutniProcenat = 0;

    private bool dobijenUgao = false;
    private int brojNivoa = 0;
    public float brzinaRotacije = 200f;
    public float visina=-6;

    public float brzinaNagore = 0.5f;
    public float brzinaNadole = 10f;
    public float pozicijaGoreY=5;
    public float pozicijaDoleY=0;
    public float pocetnaPozicijaY = 4;
    public float brzinaPecanja=30f;

    private Camera mainCamera;

    private int zelena = 106;
    private int plava=186;

    private bool peca = false;

    public bool kraj = false;
    private bool zaustavio = false;

    void Start()
    {
        mainCamera = Camera.main;
        sirinaEkrana = mainCamera.orthographicSize * mainCamera.aspect;
        pozicija1X=-sirinaEkrana/2;
        pozicija3X=sirinaEkrana/2;
        udica.GetComponent<Collider2D>().enabled = false;
        StartCoroutine(Pocetak());
        StartCoroutine(Ajkula());
    }
    void Update()
    {
        skorText.text = "Score: " + skor;
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScorePecanje", 0);

        if(!dobijenUgao)
        {
            float rotationZ = Mathf.PingPong(Time.time * brzinaRotacije, 180) - 90;
            igla.transform.rotation = Quaternion.Euler(0, 0, rotationZ);
        }

        if (Input.touchCount>0 && !dobijenUgao)
        {
            dobijenUgao = true;
            float ugao=igla.transform.eulerAngles.z;
            if(ugao>90)
            {
                ugao=360-ugao;
            }

            brojNivoa = Mathf.RoundToInt(90-Mathf.Abs(ugao));
            PravljenjeNivoa();
        }

        Tranzicija();

        Pecaj();
    }
    public void PravljenjeNivoa()
    {
        for (int i = 0; i < brojNivoa; i++)
        {
            brojTrenutnogNivoa++;
            switch(brojTrenutnogNivoa)
            {
                case < 5:
                    trenutniProcenat = 5;
                    break;
                case < 10:
                    trenutniProcenat = 10;
                    break;
                case < 15:
                    trenutniProcenat = 15;
                    break;
                case < 20:
                    trenutniProcenat = 20;
                    break;
                case < 25:
                    trenutniProcenat = 25;
                    break;
                case < 30:
                    trenutniProcenat = 30;
                    break;
                case < 35:
                    trenutniProcenat = 35;
                    break;
                case < 40:
                    trenutniProcenat = 40;
                    break;
                case < 45:
                    trenutniProcenat = 45;
                    break;
                case < 50:
                    trenutniProcenat = 50;
                    break;
                case < 55:
                    trenutniProcenat = 55;
                    break;
                case < 60:
                    trenutniProcenat = 60;
                    break;
                case < 65:
                    trenutniProcenat = 65;
                    break;
                case < 70:
                    trenutniProcenat = 70;
                    break;
                case < 75:
                    trenutniProcenat = 75;
                    break;
                case < 80:
                    trenutniProcenat = 80;
                    break;
                case < 85:
                    trenutniProcenat = 85;
                    break;
                case < 90:
                    trenutniProcenat = 90;
                    break;
                default:
                    break;
            }
            SpremiNivo();
            
            if(i!=brojNivoa-1)
            {
                visina -= nivo.GetComponent<SpriteRenderer>().bounds.size.y;
            }
        }

        visina-=dno.GetComponent<SpriteRenderer>().bounds.size.y;

        dno.transform.position = new Vector2(0, visina);
        float zelenaNormalized = Mathf.Clamp(zelena / 255f, 0f, 1f);
        float plavaNormalized = Mathf.Clamp(plava / 255f, 0f, 1f);

        dno.GetComponent<SpriteRenderer>().color = new Color(0f, zelenaNormalized, plavaNormalized);

        pozicijaDoleY = visina;

        StartCoroutine(PadanjeUdice());
    }
    public void SpremiNivo()
    {
        GameObject nivoKlon= Instantiate(nivo, new Vector2(0, visina), Quaternion.identity);

        float zelenaNormalized = Mathf.Clamp(zelena / 255f, 0f, 1f);
        float plavaNormalized = Mathf.Clamp(plava / 255f, 0f, 1f);

        nivoKlon.GetComponent<SpriteRenderer>().color = new Color(0f, zelenaNormalized, plavaNormalized);

        if (brojTrenutnogNivoa != brojNivoa)
        {
            zelena -= 2;
            plava -= 2;
        }

        if (zelena < 0) zelena = 0;
        if (plava < 0) plava = 0;

        int random1=Random.Range(0,100);
        int random2=Random.Range(0,100);
        int random3=Random.Range(0,100);

        if (random1<trenutniProcenat)
        {
            int randomRiba = Random.Range(0, ribe.Count);
            GameObject riba=Instantiate(ribe[randomRiba], new Vector2(pozicija1X, Random.Range(visina-konstantaPomerajaRibaY,visina+konstantaPomerajaRibaY)), Quaternion.identity);
            riba.GetComponent<SpriteRenderer>().sortingOrder = 3;
        }
        if (random2<trenutniProcenat)
        {
            int randomRiba = Random.Range(0, ribe.Count);
            GameObject riba=Instantiate(ribe[randomRiba], new Vector2(pozicija2X, Random.Range(visina - konstantaPomerajaRibaY, visina + konstantaPomerajaRibaY)), Quaternion.identity);
            riba.GetComponent<SpriteRenderer>().sortingOrder = 3;
        }
        if (random3 < trenutniProcenat)
        {
            int randomRiba = Random.Range(0, ribe.Count);
            GameObject riba= Instantiate(ribe[randomRiba], new Vector2(pozicija3X, Random.Range(visina - konstantaPomerajaRibaY, visina + konstantaPomerajaRibaY)), Quaternion.identity);
            riba.GetComponent<SpriteRenderer>().sortingOrder = 3;
        }
    }
    public IEnumerator PadanjeUdice()
    {
        yield return new WaitForSeconds(0.5f);

        while (udica.transform.position.y < pozicijaGoreY)
        {
            udica.transform.position = new Vector2(udica.transform.position.x, udica.transform.position.y +brzinaNagore * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        bool stavljen = false;
        while (udica.transform.position.y > pozicijaDoleY)
        {
            if(udica.transform.position.y <-konstantaPomerajaKamereY)
            {
                mainCamera.transform.SetParent(udica.transform);
                stavljen = true;
            }
            if (stavljen)
            {
                udica.transform.position = new Vector2(udica.transform.position.x, udica.transform.position.y - brzinaNadole * Time.deltaTime);
            }
            else
            {
                udica.transform.position = new Vector2(udica.transform.position.x, udica.transform.position.y - brzinaPecanja * Time.deltaTime);
            }
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        mainCamera.transform.SetParent(null);
        while (udica.transform.position.y > pozicijaDoleY-0.5)
        {
            udica.transform.position = new Vector2(udica.transform.position.x, udica.transform.position.y - brzinaNagore * Time.deltaTime);
            yield return null;
        }

        peca = true;
        udica.GetComponent<Collider2D>().enabled = true;
    }
    public void Pecaj()
    {
        if (peca && !kraj)
        {
            if(udica.transform.position.y >pozicijaDoleY && udica.transform.position.y<-konstantaPomerajaKamereY)
            {
                mainCamera.transform.position = new Vector3(0, udica.transform.position.y+konstantaPomerajaKamereY, -10);
            }

            if (udica.transform.position.y >=-konstantaPomerajaKamereY)
            {
                mainCamera.transform.position = new Vector3(0, 0, -10);
                Kraj();
            }

            float newY = Mathf.MoveTowards(udica.transform.position.y, pocetnaPozicijaY, brzinaPecanja * Time.deltaTime);
            udica.transform.position = new Vector2(udica.transform.position.x, newY);

            if (udica.transform.position.y == pocetnaPozicijaY)
            {
                peca = false;
            }

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                udica.transform.position = new Vector2(touchPosition.x, udica.transform.position.y);
            }
        }

        if (kraj)
        {
            if (zaustavio)
            {
                if (udica.transform.position.y > visina && udica.transform.position.y < 0)
                {
                    mainCamera.transform.SetParent(udica.transform);
                }

                bool stavljen = true;
                if(udica.transform.position.y >=-konstantaPomerajaKamereY)
                {
                    stavljen = false;
                }
                if (udica.transform.position.y >= -konstantaPomerajaKamereY)
                {

                    mainCamera.transform.SetParent(null);
                    mainCamera.transform.position = new Vector3(0, 0, -10);
                    Kraj();
                }

                float newY;
                if (stavljen)
                {
                    newY = Mathf.MoveTowards(udica.transform.position.y, pocetnaPozicijaY, brzinaNadole * Time.deltaTime);
                }
                else
                {
                    newY = Mathf.MoveTowards(udica.transform.position.y, pocetnaPozicijaY, brzinaPecanja * Time.deltaTime);
                }

                udica.transform.position = new Vector2(udica.transform.position.x, newY);
            }
            else
            {
                StartCoroutine(KrajPecanja());
            }
        }
    }
    public IEnumerator KrajPecanja()
    {
        yield return new WaitForSeconds(1f);
        zaustavio = true;
    }
    public IEnumerator Ajkula()
    {
        while (true)
        {
            float random = Random.Range(10, 60);
            yield return new WaitForSeconds(random);
            ajkula.SetTrigger("ajkula");
        }
    }
    public void Kraj()
    {
        if (skor > PlayerPrefs.GetInt("HighScorePecanje", 0))
        {
            PlayerPrefs.SetInt("HighScorePecanje", skor);
            noviHSEfekat.Play();
        }

        StartCoroutine(pecanje());
    }
    public IEnumerator pecanje()
    {
        yield return new WaitForSecondsRealtime(4f);
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene(11);
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
