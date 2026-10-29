using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Leaderboard : MonoBehaviour
{
    public GameObject tranzicija;
    public Animator tranzicijaAnimator;

    public HighScores level1;
    public HighScores level2;
    public HighScores level3;
    public HighScores level4;
    public HighScores level5;

    public GameObject panelTabela;
    public GameObject level1Tabela;
    public GameObject level2Tabela;
    public GameObject level3Tabela;
    public GameObject level4Tabela;
    public GameObject level5Tabela;

    public Dialog leaderboardDialog;

    private void Start()
    {
        if(PlayerPrefs.GetInt("LeaderboardFirstTime",0)==0)
        {
            DialogManager.instance.StartDialogue(leaderboardDialog);

            PlayerPrefs.SetInt("LeaderboardFirstTime", 1);
        }

        if (PlayerPrefs.GetInt("Level") >= 2)
        {
            level1.UploadScore(PlayerPrefs.GetString("Ime"), PlayerPrefs.GetInt("HighScore1Cl"));
        }
        if (PlayerPrefs.GetInt("Level") >= 3)
        {
            level2.UploadScore(PlayerPrefs.GetString("Ime"), PlayerPrefs.GetInt("HighScore2Cl"));
        }
        if (PlayerPrefs.GetInt("Level") >= 4)
        {
            level3.UploadScore(PlayerPrefs.GetString("Ime"), PlayerPrefs.GetInt("HighScore3Cl"));
        }
        if (PlayerPrefs.GetInt("Level") >= 5)
        {
            level4.UploadScore(PlayerPrefs.GetString("Ime"), PlayerPrefs.GetInt("HighScore4Cl"));
        }
        if (PlayerPrefs.GetInt("Level") >= 6)
        {
            level5.UploadScore(PlayerPrefs.GetString("Ime"), PlayerPrefs.GetInt("HighScore5Cl"));
        }

        switch(PlayerPrefs.GetInt("Level"))
        {
            case 2:
                level2Tabela.SetActive(false);
                level3Tabela.SetActive(false);
                level4Tabela.SetActive(false);
                level5Tabela.SetActive(false);
                panelTabela.GetComponent<RectTransform>().offsetMax = new Vector2(1200, 0);
                break;
            case 3:
                level3Tabela.SetActive(false);
                level4Tabela.SetActive(false);
                level5Tabela.SetActive(false);
                panelTabela.GetComponent<RectTransform>().offsetMax = new Vector2(2500, 0);
                break;
            case 4:
                level4Tabela.SetActive(false);
                level5Tabela.SetActive(false);
                panelTabela.GetComponent<RectTransform>().offsetMax = new Vector2(3800, 0);
                break;
            case 5:
                level5Tabela.SetActive(false);
                panelTabela.GetComponent<RectTransform>().offsetMax = new Vector2(4850, 0);
                break;
            case 6:
                break;
            default:
                break;
        }

        StartCoroutine(Pocetak());
    }
    private void Update()
    {
        Tranzicija();
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
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        StartCoroutine(meni());
    }
    public IEnumerator meni()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(0);
    }
}
