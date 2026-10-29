using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SirNinja : MonoBehaviour
{
    public GameObject tranzicija;
    public Animator tranzicijaAnimator;

    public TextMeshProUGUI skorText;
    public TextMeshProUGUI highScoreText;

    public ParticleSystem noviHSEfekat;

    public GameObject sir;
    public GameObject sirLevo;
    public GameObject sirDesno;
    public GameObject misolovka;

    public Sprite zatvorenaMisolovka;

    public TrailRenderer trail;
    public GameObject trailObjekat;

    public TextMeshProUGUI comboText;

    public float minX=-1.5f;
    public float maxX=1.5f;
    public float minY=-7;
    public float maxY=-6;
    public float spawnInterval = 3f;

    private int skor = 0;
    private float combo = 0;
    private int dodatakSkor = 1;

    private Vector2 startPos;
    private bool swiping = false;
    private bool vremeZaPromenu = false;
    private bool kraj = false;
    private bool vremeZaSpawn = false;

    private GameObject misolovkaPritisnuta;

    private bool isTouching = false;
    private GameObject currentTrail;
    void Start()
    {
        StartCoroutine(Pocetak());
    }
    void Update()
    {
        skorText.text = "Score: " + skor;
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScoreSirNinja", 0);
        comboText.text = "Combo: " + combo;
        if (combo <= 25)
        {
            comboText.faceColor = new Color(1, (255 - combo * 10.2f) / 255, (255 - combo * 10.2f) / 255, 1);
        }
        else
        {
            comboText.faceColor = new Color(1, 0, 0, 1);
        }

        switch (combo)
        {
            case < 10:
                dodatakSkor = 1;
                break;
            case < 15:
                dodatakSkor = 2;
                break;
            case < 20:
                dodatakSkor = 3;
                break;
            case < 25:
                dodatakSkor = 4;
                break;
            case >= 25:
                dodatakSkor = 5;
                break;
            default:
                break;
        }

        Tranzicija();
        ProveraSeca();
        promenaBrzine();
        if (!kraj)
        {
            pokreniSpawn();
        }
        else
        {
            ZaKraj();
        }
        Trail();
        zatvaranjeMisolovke();
    }
    public void promenaBrzine()
    {
        if(vremeZaPromenu)
        {
            StartCoroutine(cekajPromenu());
            vremeZaPromenu = false;
        }
    }
    public IEnumerator cekajPromenu()
    {
        yield return new WaitForSeconds(3f);
        spawnInterval -= 0.1f;
        if (spawnInterval < 0.5f)
        {
            spawnInterval = 0.5f;
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
    public void ProveraSeca()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startPos = touch.position;
                swiping = true;
            }
            else if (touch.phase == TouchPhase.Moved && swiping)
            {
                Vector2 currentPos = touch.position;
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(currentPos), Vector2.zero);

                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("sir"))
                    {
                        float rotacija = hit.collider.transform.eulerAngles.z;
                        hit.collider.GetComponent<Sir>().Pogodak();

                        // Instanciraj levi deo sira
                        GameObject sirL = Instantiate(sirLevo, hit.collider.transform.position, Quaternion.Euler(new Vector3(0, 0, rotacija)));
                        sirL.GetComponent<Rigidbody2D>().AddForce(-sirL.transform.right * 2f, ForceMode2D.Impulse);

                        // Instanciraj desni deo sira
                        GameObject sirD = Instantiate(sirDesno, hit.collider.transform.position, Quaternion.Euler(new Vector3(0, 0, rotacija)));
                        sirD.GetComponent<Rigidbody2D>().AddForce(sirD.transform.right * 2f, ForceMode2D.Impulse);

                        skor+=dodatakSkor;
                        combo++;
                    }
                    else if (hit.collider.CompareTag("misolovka"))
                    {
                        misolovkaPritisnuta = hit.collider.gameObject;
                        misolovkaPritisnuta.GetComponent<Rigidbody2D>().gravityScale = 0;
                        misolovkaPritisnuta.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
                        misolovkaPritisnuta.GetComponent<Sir>().kraj = true;
                        Kraj();
                        kraj = true;
                    }
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                swiping = false;
                combo = 0;
            }
        }
    }
    public void zatvaranjeMisolovke()
    {
        if(kraj && misolovkaPritisnuta.transform.position.x == 0 && misolovkaPritisnuta.transform.position.y == 0)
        {
            misolovkaPritisnuta.GetComponent<SpriteRenderer>().sprite = zatvorenaMisolovka;
        }
    }
    public void Trail()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10f));
            touchPosition.z = 0; // Ensure z-position is zero to stay in 2D space

            if (touch.phase == TouchPhase.Began)
            {
                // Create a new TrailRenderer object
                if (currentTrail != null)
                {
                    Destroy(currentTrail);
                }
                currentTrail = Instantiate(trailObjekat, touchPosition, Quaternion.identity);
                currentTrail.transform.SetParent(trailObjekat.transform); // Optional: parent it to this object
                isTouching = true;
            }
            else if (touch.phase == TouchPhase.Moved && isTouching)
            {
                // Move the current TrailRenderer object to follow the touch
                currentTrail.transform.position = touchPosition;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isTouching = false;
                if (currentTrail != null)
                {
                    Destroy(currentTrail); // Destroy the TrailRenderer object when touch ends
                }
            }
        }
        else if (!isTouching)
        {
            if (currentTrail != null)
            {
                Destroy(currentTrail); // Ensure trail object is destroyed when not touching
            }
        }
    }
    public void ZaKraj()
    {
        misolovkaPritisnuta.transform.position = Vector2.MoveTowards(misolovkaPritisnuta.transform.position, new Vector2(0, 0), 3f * Time.deltaTime);
        misolovkaPritisnuta.transform.localScale = Vector2.Lerp(misolovkaPritisnuta.transform.localScale, new Vector2(5, 5), 3f * Time.deltaTime);
    }
    public void Spawn()
    {
        int random = Random.Range(0, 4);

        if (random > 0)
        {
            Instantiate(sir, new Vector3(Random.Range(minX, maxX), Random.Range(minY,maxY), 0), Quaternion.identity);
        }
        else
        {
            Instantiate(misolovka, new Vector3(Random.Range(minX, maxX), Random.Range(minY,maxY), 0), Quaternion.identity);
        }
    }
    public void Kraj()
    {
        if (skor > PlayerPrefs.GetInt("HighScoreSirNinja", 0))
        {
            PlayerPrefs.SetInt("HighScoreSirNinja", skor);
            noviHSEfekat.Play();
        }
            StartCoroutine(sirNinja(4));
    }
    public IEnumerator sirNinja(float proslava)
    {
        yield return new WaitForSeconds(proslava);
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(9);
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
