using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class House : MonoBehaviour
{
    public GameObject tranzicija;
    public Animator tranzicijaAnimator;

    public List<Sprite> skinovi = new List<Sprite>();
    public Sprite obicanSkin;
    private int brojSkinova = 0;

    public Animator ormaric;

    public GameObject skrol;
    public GameObject papiri;
    public GameObject blokPapira;
    public GameObject rolnaDesno;
    public GameObject rolnaLevo;
    private float levoX = 0;
    private float desnoX = 0;
    private float pomerajRolne = 520;
    private float pomerajBloka = 799;

    public GameObject diploma;
    public GameObject pacov;
    public GameObject sanke;
    public GameObject akvarijum;
    public GameObject kristali;

    public Dialog firstTime;
    public Dialog cupboardFirst;
    public Dialog souvenirFirst;

    public List<Dialog> baseDialogs;
    public List<Dialog> level1Dialogs;
    public List<Dialog> level2Dialogs;
    public List<Dialog> level3Dialogs;
    public List<Dialog> level4Dialogs;
    public List<Dialog> level5Dialogs;

    private List<int> indeksiSkinova = new List<int>();

    public static bool zatvori= false;
    private int indeksTrenutnogSkina = 0;
    public BlokIzborSkin blokIzborSkin;
    public Animator bobAnimator;

    void Start()
    {
        Camera.main.GetComponent<AudioSource>().volume = PlayerPrefs.GetInt("MusicVolume", 100)*Camera.main.GetComponent<AudioSource>().volume/100f;

        zatvori = false;

        if (PlayerPrefs.GetInt("FirstHouse", 0) == 0)
        {
            DialogManager.instance.StartDialogue(firstTime);
        }

        if (PlayerPrefs.GetInt("FirstSouvenir", 0) == 1)
        {
            DialogManager.instance.StartDialogue(souvenirFirst);
            PlayerPrefs.SetInt("FirstSouvenir", 2);
        }

        indeksTrenutnogSkina=PlayerPrefs.GetInt("TrenutniSkin",-1);
        if(indeksTrenutnogSkina!=-1)
        {
            bobAnimator.runtimeAnimatorController = blokIzborSkin.animatoriSkinova[indeksTrenutnogSkina];
        }

        brojSkinova=PlayerPrefs.GetInt("BrojSkinova",0);

        for (int i = 0; i < skinovi.Count; i++)
        {
            if(PlayerPrefs.GetInt("Skin"+i.ToString(),0)==1)
            {
                indeksiSkinova.Add(i);
            }
        }

        switch (PlayerPrefs.GetInt("Level"))
        {
            case 2:
                diploma.SetActive(true);
                break;
            case 3:
                diploma.SetActive(true);
                pacov.SetActive(true);
                break;
            case 4:
                diploma.SetActive(true);
                pacov.SetActive(true);
                sanke.SetActive(true);
                break;
            case 5:
                diploma.SetActive(true);
                pacov.SetActive(true);
                sanke.SetActive(true);
                akvarijum.SetActive(true);
                break;
            case 6:
                diploma.SetActive(true);
                pacov.SetActive(true);
                sanke.SetActive(true);
                akvarijum.SetActive(true);
                kristali.SetActive(true);
                break;
        }

        StartCoroutine(Pocetak());
    }

    void Update()
    {
        Tranzicija();

        if (zatvori)
        {
            zatvori = false;
            Skrol();
        }
    }
    //za igre
    public void Memorija()
    {
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        SoundManager.Instance.PlayClickSound();
        StartCoroutine(memorija());
    }
    public IEnumerator memorija()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(9);
    }

    public void SirNinja()
    {
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        SoundManager.Instance.PlayClickSound();
        StartCoroutine(sirNinja());
    }
    public IEnumerator sirNinja()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(10);
    }

    public void Sankanje()
    {
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        SoundManager.Instance.PlayClickSound();
        StartCoroutine(sankanje());
    }
    public IEnumerator sankanje()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(11);
    }

    public void Pecanje()
    {
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        SoundManager.Instance.PlayClickSound();
        StartCoroutine(pecanje());
    }
    public IEnumerator pecanje()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(12);
    }

    public void Kopanje()
    {
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        SoundManager.Instance.PlayClickSound();
        StartCoroutine(kopanje());
    }
    public IEnumerator kopanje()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(13);
    }
    //za kucu
    public void Ormaric()
    {
        SoundManager.Instance.PlayClickSound();

        if(PlayerPrefs.GetInt("FirstHouse", 0) == 1)
        {
            DialogManager.instance.StartDialogue(cupboardFirst);
        }

        if (skrol.activeSelf == false)
        {
            StartCoroutine(namestanjeSkinova());
        }
    }
    public IEnumerator namestanjeSkinova()
    {
        ormaric.SetBool("Ormaric", true);

        int brojPotrebnihBlokova = brojSkinova / 4;
        if(brojSkinova % 4 != 0)
        {
            brojPotrebnihBlokova++;
        }

        int mnozitelj = 1;
        bool neparanBrojBlokova = false;

        if(PlayerPrefs.GetInt("BrojSkinova",0)==0)
        {
            brojPotrebnihBlokova = 0;
        }

        if (brojPotrebnihBlokova%2!=0)
        {
            neparanBrojBlokova = true;
            brojPotrebnihBlokova++;
        }
        int brojSkina = 0;
        int i;
        bool ubacenObican = false;

        GameObject noviBlok;
        for (i = 1; i <= brojPotrebnihBlokova; i++)
        {
            if (i % 2 == 0)
            {
                noviBlok = Instantiate(blokPapira, new Vector2(0, 0), Quaternion.identity, papiri.transform);
                noviBlok.GetComponent<RectTransform>().anchoredPosition = new Vector2(pomerajBloka * mnozitelj, 0);
                noviBlok.SetActive(true);

                desnoX = noviBlok.GetComponent<RectTransform>().anchoredPosition.x;
                mnozitelj++;
            }
            else
            {
                noviBlok = Instantiate(blokPapira, new Vector2(0, 0), Quaternion.identity, papiri.transform);
                noviBlok.GetComponent<RectTransform>().anchoredPosition = new Vector2(-pomerajBloka * mnozitelj, 0);
                noviBlok.SetActive(true);

                levoX = noviBlok.GetComponent<RectTransform>().anchoredPosition.x;
            }

            for (int j = 0; j < 4; j++)
            {
                if (brojSkina < brojSkinova)
                {
                    GameObject polje = noviBlok.transform.GetChild(j).gameObject; 
                    polje.SetActive(true);
                    GameObject skin= polje.transform.GetChild(0).gameObject;

                    skin.GetComponent<Image>().sprite = skinovi[indeksiSkinova[brojSkina]];
                    polje.GetComponent<BlokIzborSkin>().indeksSkina = indeksiSkinova[brojSkina];
                    brojSkina++;
                }
                else
                {
                    if (ubacenObican)
                    {
                        GameObject polje = noviBlok.transform.GetChild(j).gameObject;
                        polje.SetActive(false);
                    }
                    else if(brojSkinova%4!=0 || neparanBrojBlokova)
                    {
                        GameObject polje = noviBlok.transform.GetChild(j).gameObject;
                        polje.SetActive(true);
                        GameObject skin = polje.transform.GetChild(0).gameObject;
                        skin.GetComponent<Image>().sprite = obicanSkin;
                        polje.GetComponent<BlokIzborSkin>().indeksSkina = -1;
                        ubacenObican = true;
                        brojSkina++;
                    }
                }
            }
        }

        //ako nema skinova
        if(!ubacenObican)
        {
            noviBlok = Instantiate(blokPapira, new Vector2(0, 0), Quaternion.identity, papiri.transform);
            noviBlok.GetComponent<RectTransform>().anchoredPosition = new Vector2(pomerajBloka * mnozitelj, 0);
            noviBlok.SetActive(true);

            desnoX = noviBlok.GetComponent<RectTransform>().anchoredPosition.x;

            for (int j = 0; j < 4; j++)
            {
                GameObject polje = noviBlok.transform.GetChild(j).gameObject;
                polje.SetActive(false);
            }

            noviBlok = Instantiate(blokPapira, new Vector2(0, 0), Quaternion.identity, papiri.transform);
            noviBlok.GetComponent<RectTransform>().anchoredPosition = new Vector2(-pomerajBloka * mnozitelj, 0);
            noviBlok.SetActive(true);

            levoX = noviBlok.GetComponent<RectTransform>().anchoredPosition.x;
            mnozitelj++;

            for (int j = 0; j < 4; j++)
            {
                if (j == 0)
                {
                    GameObject polje = noviBlok.transform.GetChild(j).gameObject;
                    polje.SetActive(true);
                    GameObject skin = polje.transform.GetChild(0).gameObject;
                    skin.GetComponent<Image>().sprite = obicanSkin;
                    polje.GetComponent<BlokIzborSkin>().indeksSkina = -1;
                    brojSkina++;
                }
                else
                {
                    GameObject polje = noviBlok.transform.GetChild(j).gameObject;
                    polje.SetActive(false);
                }
            }
        }

        rolnaDesno.GetComponent<RectTransform>().anchoredPosition = new Vector2(desnoX + pomerajRolne, 0);
        rolnaLevo.GetComponent<RectTransform>().anchoredPosition = new Vector2(levoX - pomerajRolne, 0);

        papiri.GetComponent<RectTransform>().sizeDelta=new Vector2(mnozitelj*pomerajBloka*2, papiri.GetComponent<RectTransform>().sizeDelta.y); 

        yield return new WaitForSeconds(0.5f);

        skrol.SetActive(true);
    }
    public void Skrol()
    {
        StartCoroutine(zatvaranje());
    }
    public IEnumerator zatvaranje()
    {
        skrol.GetComponent<Animator>().SetTrigger("Skrol");
        yield return new WaitForSeconds(0.5f);
        skrol.SetActive(false);
        ormaric.SetBool("Ormaric", false);
        if (papiri.transform.childCount > 3)
        {
            for (int i = 4; i < papiri.transform.childCount; i++)
            {
                Destroy(papiri.transform.GetChild(i).gameObject);
            }
        }
    }

    public void StolicaClick()
    {
        SoundManager.Instance.PlayClickSound();
        int random = Random.Range(0, 2);
        int randomIndex = 0;
        switch (PlayerPrefs.GetInt("Level", 1))
        {
            case 1:
                randomIndex= Random.Range(0, baseDialogs.Count);
                DialogManager.instance.StartDialogue(baseDialogs[randomIndex]);
                break;
            case 2:
                if(random==0)
                {
                    randomIndex = Random.Range(0, level1Dialogs.Count);
                    DialogManager.instance.StartDialogue(level1Dialogs[randomIndex]);
                }
                else
                {
                    randomIndex = Random.Range(0, baseDialogs.Count);
                    DialogManager.instance.StartDialogue(baseDialogs[randomIndex]);
                }
                break;
            case 3:
                if (random == 0)
                {
                    randomIndex = Random.Range(0, level2Dialogs.Count);
                    DialogManager.instance.StartDialogue(level2Dialogs[randomIndex]);
                }
                else
                {
                    int random1 = Random.Range(0, 2);
                    if (random1 == 0)
                    {
                        randomIndex = Random.Range(0, level1Dialogs.Count);
                        DialogManager.instance.StartDialogue(level1Dialogs[randomIndex]);
                    }
                    else
                    {
                        randomIndex = Random.Range(0, baseDialogs.Count);
                        DialogManager.instance.StartDialogue(baseDialogs[randomIndex]);
                    }
                }
                break;
            case 4:
                if (random == 0)
                {
                    randomIndex = Random.Range(0, level3Dialogs.Count);
                    DialogManager.instance.StartDialogue(level3Dialogs[randomIndex]);
                }
                else
                {
                    int random1 = Random.Range(0, 3);
                    if (random1 == 0)
                    {
                        randomIndex = Random.Range(0, level2Dialogs.Count);
                        DialogManager.instance.StartDialogue(level2Dialogs[randomIndex]);
                    }
                    else if (random1 == 1)
                    {
                        randomIndex = Random.Range(0, level1Dialogs.Count);
                        DialogManager.instance.StartDialogue(level1Dialogs[randomIndex]);
                    }
                    else
                    {
                        randomIndex = Random.Range(0, baseDialogs.Count);
                        DialogManager.instance.StartDialogue(baseDialogs[randomIndex]);
                    }
                }
                break;
            case 5:
                if (random == 0)
                {
                    randomIndex = Random.Range(0, level4Dialogs.Count);
                    DialogManager.instance.StartDialogue(level4Dialogs[randomIndex]);
                }
                else
                {
                    int random1 = Random.Range(0, 4);
                    if (random1 == 0)
                    {
                        randomIndex = Random.Range(0, level3Dialogs.Count);
                        DialogManager.instance.StartDialogue(level3Dialogs[randomIndex]);
                    }
                    else if (random1 == 1)
                    {
                        randomIndex = Random.Range(0, level2Dialogs.Count);
                        DialogManager.instance.StartDialogue(level2Dialogs[randomIndex]);
                    }
                    else if (random1 == 2)
                    {
                        randomIndex = Random.Range(0, level1Dialogs.Count);
                        DialogManager.instance.StartDialogue(level1Dialogs[randomIndex]);
                    }
                    else
                    {
                        randomIndex = Random.Range(0, baseDialogs.Count);
                        DialogManager.instance.StartDialogue(baseDialogs[randomIndex]);
                    }
                }
                break;
            case 6:
                if (random == 0)
                {
                    randomIndex = Random.Range(0, level5Dialogs.Count);
                    DialogManager.instance.StartDialogue(level5Dialogs[randomIndex]);
                }
                else
                {
                    int random1 = Random.Range(0, 5);
                    if (random1 == 0)
                    {
                        randomIndex = Random.Range(0, level4Dialogs.Count);
                        DialogManager.instance.StartDialogue(level4Dialogs[randomIndex]);
                    }
                    else if (random1 == 1)
                    {
                        randomIndex = Random.Range(0, level3Dialogs.Count);
                        DialogManager.instance.StartDialogue(level3Dialogs[randomIndex]);
                    }
                    else if (random1 == 2)
                    {
                        randomIndex = Random.Range(0, level2Dialogs.Count);
                        DialogManager.instance.StartDialogue(level2Dialogs[randomIndex]);
                    }
                    else if (random1 == 3)
                    {
                        randomIndex = Random.Range(0, level1Dialogs.Count);
                        DialogManager.instance.StartDialogue(level1Dialogs[randomIndex]);
                    }
                    else
                    {
                        randomIndex = Random.Range(0, baseDialogs.Count);
                        DialogManager.instance.StartDialogue(baseDialogs[randomIndex]);
                    }
                }
                break;
        }
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
        if(PlayerPrefs.GetInt("FirstHouse", 0) == 0 || PlayerPrefs.GetInt("FirstHouse", 0) == 1)
        {
            return;
        }

        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        SoundManager.Instance.PlayClickSound();
        StartCoroutine(meni());
    }
    public IEnumerator meni()
    {
        yield return new WaitForSeconds(0.5f);
        PlayerPrefs.SetInt("OpenCity", 1);
        SceneManager.LoadScene(1);
    }
}
