using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Meni : MonoBehaviour
{
    public static bool kampanja = false;

    public GameObject tranzicija;
    public Animator tranzicijaAnimator;
    public GameObject campaignLevelMeni;
    public GameObject competitiveLevelMeni;
    public GameObject cityMeni;
    public GameObject citySigns;
    public GameObject seetingsMeni;
    public Text ime;

    //intro animacija
    public Material blur;
    public GameObject introPanel;

    //dialozi
    public Dialog introDialog;
    public Dialog settingsDialog;
    public Dialog meniCampaign1Dialog;
    public Dialog campaignDialog;
    public Dialog meniCity;
    public Dialog cityDialog;
    public Dialog cityAfterHouseDialog;
    public Dialog win1Dialog;
    public Dialog competitiveDialog;

    //highscore campaign
    public TextMeshPro highscoreCam1;
    public TextMeshPro highscoreCam2;
    public TextMeshPro highscoreCam3;
    public TextMeshPro highscoreCam4;
    public TextMeshPro highscoreCam5;

    //highscore competitive
    public TextMeshPro highscoreComp1;
    public TextMeshPro highscoreComp2;
    public TextMeshPro highscoreComp3;
    public TextMeshPro highscoreComp4;
    public TextMeshPro highscoreComp5;

    public static int level = 1;

    public GameObject unosPanel;
    private string imeIgraca;

    //da li moze da se klikne i efekti
    public Button settingsButton;
    public GameObject settingsEffectsHolder;
    public Button bluePortalButton;
    public GameObject bluePortalEffectsHolder;
    public Button cityButton;
    public GameObject cityEffectsHolder;
    public Button redPortalButton;
    public GameObject redPortalEffectsHolder;
    private void Start()
    {
        Camera.main.GetComponent<AudioSource>().volume = PlayerPrefs.GetInt("MusicVolume", 100) * Camera.main.GetComponent<AudioSource>().volume / 100f;

        StartCoroutine(CheckDialogs());

        if (PlayerPrefs.GetInt("FirstTime", 0) == 0)
        {
            Camera.main.GetComponent<AudioSource>().Stop();
            tranzicija.SetActive(false);
            introPanel.SetActive(true);
            PlayerPrefs.SetInt("FirstTime", 1);

            StartCoroutine(IntroAnimacija());
        }

        if (PlayerPrefs.GetInt("OpenCity", 0) == 1)
        {
            cityMeni.SetActive(true);
            citySigns.SetActive(true);
            PlayerPrefs.SetInt("OpenCity", 0);

            if (PlayerPrefs.GetInt("FirstHouse", 0) == 2)
            {
                DialogManager.instance.StartDialogue(cityAfterHouseDialog);
            }
        }

        level = PlayerPrefs.GetInt("Level", 1);

        highscoreCam1.text = "Highscore: \n" + PlayerPrefs.GetInt("HighScore1Cm", 0).ToString();
        highscoreCam2.text = "Highscore: \n" + PlayerPrefs.GetInt("HighScore2Cm", 0).ToString();
        highscoreCam3.text = "Highscore: \n" + PlayerPrefs.GetInt("HighScore3Cm", 0).ToString();
        highscoreCam4.text = "Highscore: \n" + PlayerPrefs.GetInt("HighScore4Cm", 0).ToString();
        highscoreCam5.text = "Highscore: \n" + PlayerPrefs.GetInt("HighScore5Cm", 0).ToString();

        highscoreComp1.text = "Highscore: \n" + PlayerPrefs.GetInt("HighScore1Cl", 0).ToString();
        highscoreComp2.text = "Highscore: \n" + PlayerPrefs.GetInt("HighScore2Cl", 0).ToString();
        highscoreComp3.text = "Highscore: \n" + PlayerPrefs.GetInt("HighScore3Cl", 0).ToString();
        highscoreComp4.text = "Highscore: \n" + PlayerPrefs.GetInt("HighScore4Cl", 0).ToString();
        highscoreComp5.text = "Highscore: \n" + PlayerPrefs.GetInt("HighScore5Cl", 0).ToString();

        GameObject scrollAreaCampaign = campaignLevelMeni.transform.GetChild(campaignLevelMeni.transform.childCount - 1).gameObject;
        GameObject scrollCampaign = scrollAreaCampaign.transform.GetChild(0).gameObject;
        GameObject leveliCampaign = scrollCampaign.transform.GetChild(0).gameObject;
        switch (level)
        {
            case 1:
                leveliCampaign.GetComponent<RectTransform>().offsetMax = new Vector2(1500, 0);
                break;
            case 2:
                leveliCampaign.GetComponent<RectTransform>().offsetMax = new Vector2(2200, 0);
                break;
            case 3:
                leveliCampaign.GetComponent<RectTransform>().offsetMax = new Vector2(2900, 0);
                break;
            default:
                break;
        }
        List<GameObject> leveliCampaignList = new List<GameObject>();
        for (int i = 0; i < leveliCampaign.transform.childCount; i++)
        {
            leveliCampaignList.Add(leveliCampaign.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < leveliCampaignList.Count; i++)
        {
            if (i == level + 1)
            {
                leveliCampaignList[i].GetComponent<Image>().enabled = false;
                GameObject portal = leveliCampaignList[i].transform.GetChild(0).gameObject;
                portal.GetComponent<Image>().color = new Color(0, 0, 0);
                portal.GetComponent<Button>().enabled = false;
                GameObject hsTabla = portal.transform.GetChild(0).gameObject;
                hsTabla.GetComponent<Image>().color = new Color(0, 0, 0);
                GameObject hsTekst = hsTabla.transform.GetChild(0).gameObject;
                hsTekst.GetComponent<TextMeshPro>().enabled = false;
                GameObject imeTabla = portal.transform.GetChild(1).gameObject;
                imeTabla.GetComponent<Image>().color = new Color(0, 0, 0);
                GameObject imeTekst = imeTabla.transform.GetChild(0).gameObject;
                imeTekst.GetComponent<TextMeshPro>().enabled = false;

                if (level + 1 == 3 || level + 1 == 4)
                {
                    GameObject efekat = leveliCampaignList[i].transform.GetChild(1).gameObject;
                    var mainEfekat = efekat.GetComponent<ParticleSystem>().main;
                    mainEfekat.startColor = new Color(0, 0, 0);
                }
            }
            else if (i > level)
            {
                leveliCampaignList[i].SetActive(false);
            }
        }

        GameObject scrollAreaCompetitive = competitiveLevelMeni.transform.GetChild(competitiveLevelMeni.transform.childCount - 1).gameObject;
        GameObject scrollCompetitive = scrollAreaCompetitive.transform.GetChild(0).gameObject;
        GameObject leveliCompetitive = scrollCompetitive.transform.GetChild(0).gameObject;
        switch (level - 1)
        {
            case 0:
                leveliCompetitive.GetComponent<RectTransform>().offsetMax = new Vector2(800, 0);
                break;
            case 1:
                leveliCompetitive.GetComponent<RectTransform>().offsetMax = new Vector2(1500, 0);
                break;
            case 2:
                leveliCompetitive.GetComponent<RectTransform>().offsetMax = new Vector2(2200, 0);
                break;
            case 3:
                leveliCompetitive.GetComponent<RectTransform>().offsetMax = new Vector2(2900, 0);
                break;
            default:
                break;
        }
        List<GameObject> leveliCompetitiveList = new List<GameObject>();
        for (int i = 0; i < leveliCompetitive.transform.childCount; i++)
        {
            leveliCompetitiveList.Add(leveliCompetitive.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < leveliCompetitiveList.Count; i++)
        {
            if (i == level)
            {
                leveliCompetitiveList[i].GetComponent<Image>().enabled = false;
                GameObject portal = leveliCompetitiveList[i].transform.GetChild(0).gameObject;
                portal.GetComponent<Image>().color = new Color(0, 0, 0);
                portal.GetComponent<Button>().enabled = false;
                GameObject hsTabla = portal.transform.GetChild(0).gameObject;
                hsTabla.GetComponent<Image>().color = new Color(0, 0, 0);
                GameObject hsTekst = hsTabla.transform.GetChild(0).gameObject;
                hsTekst.GetComponent<TextMeshPro>().enabled = false;
                GameObject imeTabla = portal.transform.GetChild(1).gameObject;
                imeTabla.GetComponent<Image>().color = new Color(0, 0, 0);
                GameObject imeTekst = imeTabla.transform.GetChild(0).gameObject;
                imeTekst.GetComponent<TextMeshPro>().enabled = false;

                if (level == 3 || level == 4)
                {
                    GameObject efekat = leveliCompetitiveList[i].transform.GetChild(1).gameObject;
                    var mainEfekat = efekat.GetComponent<ParticleSystem>().main;
                    mainEfekat.startColor = new Color(0, 0, 0);
                }
            }
            else if (i > level - 1)
            {
                leveliCompetitiveList[i].SetActive(false);
            }
        }

        StartCoroutine(Pocetak());
        ime.color = new(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        StartCoroutine(PromenaBoje());
    }

    public IEnumerator CheckDialogs()
    {
        while (DialogManager.instance == null)
        {
            yield return new WaitForEndOfFrame();
        }

        if (PlayerPrefs.GetInt("SettingsClickable", 0) == 0)
        {
            settingsButton.enabled = false;

            StartCoroutine(WaitForSettings());
        }
        else if (PlayerPrefs.GetInt("WaitingSettingsFirstClick", 0) == 1)
        {
            settingsEffectsHolder.SetActive(true);
        }
        else if (PlayerPrefs.GetInt("WaitingSettingsFirstBack", 0) == 1)
        {
            DialogManager.instance.StartDialogue(meniCampaign1Dialog);
            PlayerPrefs.SetInt("WaitingSettingsFirstBack", 2);
        }

        if (PlayerPrefs.GetInt("BluePortalClickable", 0) == 0)
        {
            bluePortalButton.enabled = false;

            StartCoroutine(WaitForBluePortal());
        }
        else if (PlayerPrefs.GetInt("WaitingBluePortalFirstClick", 0) == 1)
        {
            bluePortalEffectsHolder.SetActive(true);
        }
        else if (PlayerPrefs.GetInt("DialogueLevel1", 0) == 2)
        {
            DialogManager.instance.StartDialogue(meniCity);

            StartCoroutine(WaitForCity());
        }

        if (PlayerPrefs.GetInt("CityClickable", 0) == 0)
        {
            cityButton.enabled = false;
        }
        else if (PlayerPrefs.GetInt("CityClickable", 0) == 1)
        {
            cityEffectsHolder.SetActive(true);
        }

        if (PlayerPrefs.GetInt("RedPortalFirstClick", 0) == 0)
        {
            redPortalButton.enabled = false;
        }
        else if (PlayerPrefs.GetInt("RedPortalFirstClick", 0) == 1)
        {
            redPortalEffectsHolder.SetActive(true);
        }


        if (PlayerPrefs.GetInt("justFinished", 0) == 1)
        {
            DialogManager.instance.StartDialogue(win1Dialog);
            PlayerPrefs.SetInt("justFinished", 0);

            redPortalEffectsHolder.SetActive(true);
            redPortalButton.enabled = true;
            PlayerPrefs.SetInt("RedPortalFirstClick", 1);
            PlayerPrefs.SetInt("FirstSouvenir", 1);
        }
    }

    public IEnumerator WaitForSettings()
    {
        while (true)
        {
            if (PlayerPrefs.GetInt("SettingsClickable", 0) == 1)
            {
                settingsButton.enabled = true;
                settingsEffectsHolder.SetActive(true);
                PlayerPrefs.SetInt("WaitingSettingsFirstClick", 1);
                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator WaitForBluePortal()
    {
        while (true)
        {
            if (PlayerPrefs.GetInt("BluePortalClickable", 0) == 1)
            {
                bluePortalButton.enabled = true;
                bluePortalEffectsHolder.SetActive(true);
                PlayerPrefs.SetInt("WaitingBluePortalFirstClick", 1);
                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator WaitForCity()
    {
        while (true)
        {
            if (PlayerPrefs.GetInt("CityClickable", 0) == 1)
            {
                cityButton.enabled = true;
                cityEffectsHolder.SetActive(true);
                PlayerPrefs.SetInt("CityClickable", 2);
                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator IntroAnimacija()
    {
        yield return new WaitForSeconds(10f);
        blur.SetFloat("_Intensity", 0.8f);
        blur.SetFloat("_Multiplier", 0.8f);
        yield return new WaitForSeconds(10f);
        blur.SetFloat("_Intensity", 0.6f);
        blur.SetFloat("_Multiplier", 0.6f);
        yield return new WaitForSeconds(8f);
        blur.SetFloat("_Intensity", 0.4f);
        blur.SetFloat("_Multiplier", 0.4f);
        yield return new WaitForSeconds(1.17f);
        blur.SetFloat("_Intensity", 0.2f);
        blur.SetFloat("_Multiplier", 0.2f);
        yield return new WaitForSeconds(0.66f);
        blur.SetFloat("_Intensity", 0f);
        blur.SetFloat("_Multiplier", 0f);
        yield return new WaitForSeconds(8.85f);
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija1", true);
        introPanel.SetActive(false);
        blur.SetFloat("_Intensity", 1f);
        blur.SetFloat("_Multiplier", 1f);

        yield return new WaitForSeconds(0.5f);
        Camera.main.GetComponent<AudioSource>().Play();
        DialogManager.instance.StartDialogue(introDialog);
    }

    private void Update()
    {
        Tranzicija();
    }
    public IEnumerator PromenaBoje()
    {
        yield return new WaitForSeconds(5);
        ime.color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        StartCoroutine(PromenaBoje());
    }
    public IEnumerator Pocetak()
    {
        yield return new WaitForSeconds(0.1f);
        tranzicijaAnimator.SetBool("tranzicija1", true);
        yield return new WaitForSeconds(4f);
    }
    public void Tranzicija()
    {
        if (tranzicijaAnimator.GetCurrentAnimatorStateInfo(0).IsName("kraj"))
        {
            tranzicija.SetActive(false);
        }
    }
    public IEnumerator Ime()
    {
        yield return new WaitForSeconds(1f);
        if (!PlayerPrefs.HasKey("Ime"))
        {
            unosPanel.SetActive(true);
        }
    }
    public void UnosImena(string s)
    {
        imeIgraca = s;
    }
    public void Potvrdi()
    {
        PlayerPrefs.SetString("Ime", imeIgraca);
        unosPanel.GetComponent<Animator>().SetTrigger("kraj");
        StartCoroutine(imeKraj());
    }
    public IEnumerator imeKraj()
    {
        yield return new WaitForSeconds(1f);
        unosPanel.SetActive(false);
    }
    ///Start meni
    public void Quit()
    {
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        SoundManager.Instance.PlayClickSound();
        StartCoroutine(quit());
    }
    public IEnumerator quit()
    {
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }

    public void Shop()
    {
        if (PlayerPrefs.GetInt("CityClickable", 0) == 1 || PlayerPrefs.GetInt("CityClickable", 0) == 2)
        {
            return;
        }

        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        SoundManager.Instance.PlayClickSound();
        StartCoroutine(shop());
    }
    public IEnumerator shop()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(2);
    }

    public void House()
    {
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        SoundManager.Instance.PlayClickSound();
        StartCoroutine(house());
    }
    public IEnumerator house()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(8);
    }

    public void Leaderboard()
    {
        if (PlayerPrefs.GetInt("CityClickable", 0) == 1 || PlayerPrefs.GetInt("CityClickable", 0) == 2 || PlayerPrefs.GetInt("CityClickable", 0) == 3)
        {
            return;
        }

        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        SoundManager.Instance.PlayClickSound();
        StartCoroutine(leaderboard());
    }
    public IEnumerator leaderboard()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(14);
    }

    public void Campaign()
    {
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        SoundManager.Instance.PlayClickSound();
        kampanja = true;
        StartCoroutine(campaign());
    }
    public IEnumerator campaign()
    {
        yield return new WaitForSeconds(0.5f);
        campaignLevelMeni.SetActive(true);

        if (PlayerPrefs.GetInt("WaitingBluePortalFirstClick", 0) == 1)
        {
            DialogManager.instance.StartDialogue(campaignDialog);
            bluePortalEffectsHolder.SetActive(false);
            PlayerPrefs.SetInt("WaitingBluePortalFirstClick", 2);
        }

        tranzicijaAnimator.SetBool("tranzicija1", true);
    }

    public void Competitive()
    {
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        SoundManager.Instance.PlayClickSound();
        kampanja = false;
        StartCoroutine(competitive());
    }
    public IEnumerator competitive()
    {
        yield return new WaitForSeconds(0.5f);
        competitiveLevelMeni.SetActive(true);

        if (PlayerPrefs.GetInt("RedPortalFirstClick", 0) == 1)
        {
            DialogManager.instance.StartDialogue(competitiveDialog);
            PlayerPrefs.SetInt("RedPortalFirstClick", 2);
            redPortalEffectsHolder.SetActive(false);
        }

        tranzicijaAnimator.SetBool("tranzicija1", true);
    }

    public void City()
    {
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        SoundManager.Instance.PlayClickSound();
        StartCoroutine(city());
    }
    public IEnumerator city()
    {
        yield return new WaitForSeconds(0.5f);
        cityMeni.SetActive(true);
        citySigns.SetActive(true);

        if (PlayerPrefs.GetInt("CityClickable", 0) == 1)
        {
            DialogManager.instance.StartDialogue(cityDialog);
            cityEffectsHolder.SetActive(false);
        }
        if (PlayerPrefs.GetInt("FirstHouse", 0) == 2)
        {
            DialogManager.instance.StartDialogue(cityAfterHouseDialog);
        }

        tranzicijaAnimator.SetBool("tranzicija1", true);
    }

    public void Settings()
    {
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        SoundManager.Instance.PlayClickSound();
        StartCoroutine(settings());
    }
    public IEnumerator settings()
    {
        yield return new WaitForSeconds(0.5f);
        seetingsMeni.SetActive(true);

        if (PlayerPrefs.GetInt("WaitingSettingsFirstClick", 0) == 1)
        {
            DialogManager.instance.StartDialogue(settingsDialog);
            settingsEffectsHolder.SetActive(false);
            PlayerPrefs.SetInt("WaitingSettingsFirstClick", 2);
            PlayerPrefs.SetInt("WaitingSettingsFirstBack", 1);
        }

        tranzicijaAnimator.SetBool("tranzicija1", true);
    }

    public void CampaignBack()
    {
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        SoundManager.Instance.PlayClickSound();
        StartCoroutine(campaignBack());
    }
    public IEnumerator campaignBack()
    {
        yield return new WaitForSeconds(0.5f);
        campaignLevelMeni.SetActive(false);
        tranzicijaAnimator.SetBool("tranzicija1", true);
    }

    public void CompetitiveBack()
    {
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        SoundManager.Instance.PlayClickSound();
        StartCoroutine(competitiveBack());
    }
    public IEnumerator competitiveBack()
    {
        yield return new WaitForSeconds(0.5f);
        competitiveLevelMeni.SetActive(false);
        tranzicijaAnimator.SetBool("tranzicija1", true);
    }

    public void CityBack()
    {
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        SoundManager.Instance.PlayClickSound();
        StartCoroutine(cityBack());
    }
    public IEnumerator cityBack()
    {
        yield return new WaitForSeconds(0.5f);
        cityMeni.SetActive(false);
        citySigns.SetActive(false);
        Camera.main.GetComponent<Animator>().Play("New State");
        MeniCity.pozicija = 0;
        tranzicijaAnimator.SetBool("tranzicija1", true);
    }

    public void SettingsBack()
    {
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        SoundManager.Instance.PlayClickSound();
        StartCoroutine(settingsBack());
    }
    public IEnumerator settingsBack()
    {
        yield return new WaitForSeconds(0.5f);
        seetingsMeni.SetActive(false);

        if (PlayerPrefs.GetInt("WaitingSettingsFirstBack", 0) == 1)
        {
            DialogManager.instance.StartDialogue(meniCampaign1Dialog);
            PlayerPrefs.SetInt("WaitingSettingsFirstBack", 2);
        }

        tranzicijaAnimator.SetBool("tranzicija1", true);
    }

    public void Level1()
    {
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        SoundManager.Instance.PlayClickSound();
        StartCoroutine(level1());
    }
    public IEnumerator level1()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(3);
    }

    public void Level2()
    {
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        SoundManager.Instance.PlayClickSound();
        StartCoroutine(level2());
    }
    public IEnumerator level2()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(4);
    }

    public void Level3()
    {
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        SoundManager.Instance.PlayClickSound();
        StartCoroutine(level3());
    }
    public IEnumerator level3()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(5);
    }

    public void Level4()
    {
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        SoundManager.Instance.PlayClickSound();
        StartCoroutine(level4());
    }
    public IEnumerator level4()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(6);
    }

    public void Level5()
    {
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        SoundManager.Instance.PlayClickSound();
        StartCoroutine(level5());
    }
    public IEnumerator level5()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(7);
    }
}
