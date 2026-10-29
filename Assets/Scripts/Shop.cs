using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject tranzicija;
    public Animator tranzicijaAnimator;
    public GameObject kanvas;
    public GameObject panel;

    public GameObject rupa;

    //diajlozi
    public Dialog firstTime;
    public Dialog nikorax;
    public Dialog potions;
    public Dialog filipikus;
    public Dialog totems;
    public Dialog adrianida;
    public Dialog skin;
    public Dialog pizza;
    public Dialog afterPizza;

    public static string clickedStand = "";

    //objekti vezani za napitke
    public GameObject blurNapitci;
    public GameObject napitciPanel;
    public GameObject policaNapitci;
    public TextMeshPro novciciPolicaNapitci;
    public TextMeshPro brojNapitaka;

    //objekti vezani za toteme
    public GameObject blurTotemi;
    public GameObject totemiPanel;
    public GameObject policaTotemi;
    public TextMeshPro novciciPolicaTotemi;
    public GameObject level1Panel;
    public GameObject level2Panel;
    public GameObject level3Panel;
    public GameObject level4Panel;
    public GameObject level5Panel;

    //objekti vezani za skinove
    public GameObject skinPanel;
    public GameObject bob;
    public TextMeshProUGUI dijamantiSkinovi;
    public List<Sprite> skinovi = new List<Sprite>();

    //napitci
    public GameObject plavi;
    public GameObject ljubicasti;
    public GameObject crveni;
    public GameObject zuti;
    public GameObject narandzasti;
    public GameObject zeleni;

    public float poskupljenjeNapitaka = 1.5f;

    //totemi
    //level1
    public GameObject dzojstik;
    public GameObject tv;

    //level2
    public GameObject kapibara;
    public GameObject tocak;
    public GameObject srce;


    //level3
    public GameObject kramp;
    public GameObject lopata;
    public GameObject snesko;
    public GameObject poklon;

    //level4
    public GameObject kasica;
    public GameObject hobotnica;
    public GameObject zlatnaRibica;
    public GameObject lobanja;
    public GameObject kraba;

    //level5
    public GameObject satelit;
    public GameObject antena;
    public GameObject mafin;
    public GameObject vanzemaljac;
    public GameObject pistoljKvadrat;
    public GameObject pistoljTrougao;

    //tekstovi za napitke na blur-u
    public Text imeNapitci;
    public Text opisNapitci;
    public TextMeshProUGUI cenaNapitci;

    //tekstovi za toteme na blur-u
    public Text imeTotemi;
    public Text opisTotemi;
    public TextMeshProUGUI cenaTotemi;

    //tekstovi za skinove
    public TextMeshProUGUI cenaSkinaTekst;

    //tekstovi za shop
    public TextMeshProUGUI novciciShop;
    public TextMeshProUGUI dijamantiShop;

    private bool aktivanBlur = false;
    private string proizvod;

    //cena napitaka
    private int cenaPlaviPocetna = 10;
    private int cenaPlavi = 10;

    private int cenaCrveniPocetna = 15;
    private int cenaCrveni = 15;

    private int cenaLjubicasti = 5;

    private int cenaZutiPocetna = 20;
    private int cenaZuti = 20;

    private int cenaNarandzastiPocetna = 25;
    private int cenaNarandzasti = 25;

    private int cenaZeleniPocetna = 30;
    private int cenaZeleni = 30;

    //cena skinova
    private int cenaSkina = 5;

    //cena totema
    //level1
    private int cenaDzojstik = 50;
    private int cenaTv = 75;
    //level2
    private int cenaKapibara = 75;
    private int cenaTocak = 50;
    private int cenaSrce = 100;
    //level3
    private int cenaKramp = 100;
    private int cenaLopata = 150;
    private int cenaSnesko = 75;
    private int cenaPoklon = 50;
    //level4
    private int cenaKasica = 40;
    private int cenaHobotnica = 30;
    private int cenaZlatnaRibica = 100;
    private int cenaLobanja = 50;
    private int cenaKraba = 20;
    //level5
    private int cenaSatelit = 100;
    private int cenaAntena = 50;
    private int cenaMafin = 75;
    private int cenaVanzemaljac = 50;
    private int cenaPistoljKvadrat = 100;
    private int cenaPistoljTrougao = 150;

    //namestanje totema
    private bool dzojstikAktivan = true;
    private bool tvAktivan = true;
    private bool kapibaraAktivan = true;
    private bool tocakAktivan = true;
    private bool srceAktivan = true;
    private bool krampAktivan = true;
    private bool lopataAktivan = true;
    private bool sneskoAktivan = true;
    private bool poklonAktivan = true;
    private bool kasicaAktivan = true;
    private bool hobotnicaAktivan = true;
    private bool zlatnaRibicaAktivan = true;
    private bool lobanjaAktivan = true;
    private bool krabaAktivan = true;
    private bool satelitAktivan = true;
    private bool antenaAktivan = true;
    private bool mafinAktivan = true;
    private bool vanzemaljacAktivan = true;
    private bool pistoljKvadratAktivan = true;
    private bool pistoljTrougaoAktivan = true;

    private List<bool> kupljeniSkinovi = new List<bool>();
    private List<int> indeksiKupovnaSkinova = new List<int>();

    private int indeksTrenutnogSkina;
    public Animator bobAnimator;
    public List<RuntimeAnimatorController> animatoriSkinova;

    private void Start()
    {
        clickedStand = "";

        Camera.main.GetComponent<AudioSource>().volume = PlayerPrefs.GetInt("MusicVolume", 100) * Camera.main.GetComponent<AudioSource>().volume / 100f;

        if (PlayerPrefs.GetInt("Nikorax", 0) == 2 && PlayerPrefs.GetInt("Filipikus", 0) == 2 && PlayerPrefs.GetInt("Adrianida", 0) == 2 && PlayerPrefs.GetInt("Pizza", 0) == 0)
        {
            DialogManager.instance.StartDialogue(pizza);
        }

        if (PlayerPrefs.GetInt("Pizza", 0) == 2)
        {
            DialogManager.instance.StartDialogue(afterPizza);
        }

        indeksTrenutnogSkina = PlayerPrefs.GetInt("TrenutniSkin", -1);
        if (indeksTrenutnogSkina != -1)
        {
            bobAnimator.runtimeAnimatorController = animatoriSkinova[indeksTrenutnogSkina];
        }

        if (PlayerPrefs.GetInt("FirstShop", 0) == 0)
        {
            DialogManager.instance.StartDialogue(firstTime);
        }

        if (PlayerPrefs.GetInt("BrojSkinova", 0) == 0)
        {
            for (int i = 0; i < skinovi.Count; i++)
            {
                PlayerPrefs.SetInt("Skin" + i.ToString(), 0);
            }
        }
        for (int i = 0; i < skinovi.Count; i++)
        {
            if (PlayerPrefs.GetInt("Skin" + i.ToString(), 0) == 1)
            {
                kupljeniSkinovi.Add(true);
            }
            else
            {
                kupljeniSkinovi.Add(false);
                indeksiKupovnaSkinova.Add(i);
            }
        }

        if (PlayerPrefs.GetString("DailyDatum", "") == "")
        {
            rupa.SetActive(true);
        }
        else
        {
            DateTime zadnjaIgra = DateTime.Parse(PlayerPrefs.GetString("DailyDatum", ""));

            DateTime sada = DateTime.Now;

            double razlika = (sada - zadnjaIgra).TotalMinutes;
            //double razlika = (sada - zadnjaIgra).TotalDays;

            if (razlika >= 1)
            {
                rupa.SetActive(true);
            }
        }

        //namestanje cena napitaka
        if (PlayerPrefs.GetInt("plavi", 0) != 0)
        {
            cenaPlavi = (int)(cenaPlaviPocetna * Mathf.Pow(poskupljenjeNapitaka, PlayerPrefs.GetInt("plavi", 0)));
            cenaPlavi -= cenaPlavi % 5;
        }
        if (PlayerPrefs.GetInt("crveni", 0) != 0)
        {
            cenaCrveni = (int)(cenaCrveniPocetna * Mathf.Pow(poskupljenjeNapitaka, PlayerPrefs.GetInt("crveni", 0)));
            cenaCrveni -= cenaCrveni % 5;
        }
        if (PlayerPrefs.GetInt("zuti", 0) != 0)
        {
            cenaZuti = (int)(cenaZutiPocetna * Mathf.Pow(poskupljenjeNapitaka, PlayerPrefs.GetInt("zuti", 0)));
            cenaZuti -= cenaZuti % 5;
        }
        if (PlayerPrefs.GetInt("narandzasti", 0) != 0)
        {
            cenaNarandzasti = (int)(cenaNarandzastiPocetna * Mathf.Pow(poskupljenjeNapitaka, PlayerPrefs.GetInt("narandzasti", 0)));
            cenaNarandzasti -= cenaNarandzasti % 5;
        }
        if (PlayerPrefs.GetInt("zeleni", 0) != 0)
        {
            cenaZeleni = (int)(cenaZeleniPocetna * Mathf.Pow(poskupljenjeNapitaka, PlayerPrefs.GetInt("zeleni", 0)));
            cenaZeleni -= cenaZeleni % 5;
        }

        cenaSkina = PlayerPrefs.GetInt("BrojSkinova", 0) * 5 + 5;

        blurNapitci.transform.SetParent(policaNapitci.transform);

        switch (Meni.level)
        {
            case 1:
                level1Panel.SetActive(true);
                blurTotemi.transform.SetParent(level1Panel.transform);
                break;
            case 2:
                level2Panel.SetActive(true);
                blurTotemi.transform.SetParent(level2Panel.transform);
                break;
            case 3:
                level3Panel.SetActive(true);
                blurTotemi.transform.SetParent(level3Panel.transform);
                break;
            case 4:
                level4Panel.SetActive(true);
                blurTotemi.transform.SetParent(level4Panel.transform);
                break;
            case 5:
                level5Panel.SetActive(true);
                blurTotemi.transform.SetParent(level5Panel.transform);
                break;
            default:
                break;
        }

        if (Meni.level > 1)
        {
            crveni.SetActive(true);
        }
        if (Meni.level > 2)
        {
            zuti.SetActive(true);
        }
        if (Meni.level > 3)
        {
            narandzasti.SetActive(true);
        }
        if (Meni.level > 4)
        {
            zeleni.SetActive(true);
        }

        namestanjeTotema();

        StartCoroutine(Pocetak());

        //PlayerPrefs.SetInt("Novcici", 1000);//////
        //PlayerPrefs.SetInt("Dijamanti", 100);//////
        /////////daily je na minut a ne na dan////////////
    }

    private void Update()
    {
        if (blurNapitci.activeInHierarchy)
        {
            if (blurNapitci.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("New State") && !aktivanBlur)
            {
                blurNapitci.SetActive(false);
            }

            novciciPolicaNapitci.text = PlayerPrefs.GetInt("Novcici", 0).ToString() + "<sprite=0>";
        }

        if (blurTotemi.activeInHierarchy)
        {
            if (blurTotemi.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("New State") && !aktivanBlur)
            {
                blurTotemi.SetActive(false);
            }

            novciciPolicaTotemi.text = PlayerPrefs.GetInt("Novcici", 0).ToString() + "<sprite=0>";
        }

        if (skinPanel.activeInHierarchy)
        {
            dijamantiSkinovi.text = PlayerPrefs.GetInt("Dijamanti", 0).ToString() + "<sprite=0>";
            cenaSkinaTekst.text = "  Buy\n" + cenaSkina + "<sprite=0>";
            cenaSkina = PlayerPrefs.GetInt("BrojSkinova", 0) * 5 + 5;
        }

        novciciShop.text = PlayerPrefs.GetInt("Novcici", 0).ToString() + "<sprite=0>";
        dijamantiShop.text = PlayerPrefs.GetInt("Dijamanti", 0).ToString() + "<sprite=0>";

        Tranzicija();

        namestanjeTotema();

        BojaZaBuy();
    }

    ///Napici
    public void plaviNapitak()
    {
        SoundManager.Instance.PlayClickSound();
        brojNapitaka.text = PlayerPrefs.GetInt("plavi", 0).ToString() + "<sprite=0>";
        if (!plavi.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("PlaviNapitakStoji"))
        {
            proizvod = "plavi";
            aktivanBlur = true;
            imeNapitci.text = "Potion of Birth";
            opisNapitci.text = "Start with +5 score. Skip the slow start, but beware, too much will make you too fast!";
            cenaNapitci.text = "buy " + cenaPlavi.ToString() + "<sprite=0>";
            blurNapitci.transform.SetAsLastSibling();
            plavi.transform.SetAsLastSibling();
            plavi.GetComponent<Animator>().SetBool("plavi", true);
            blurNapitci.SetActive(true);
            blurNapitci.GetComponent<Animator>().SetBool("blur", true);
        }
        else
        {
            aktivanBlur = false;
            plavi.GetComponent<Animator>().SetBool("plavi", false);
            blurNapitci.GetComponent<Animator>().SetBool("blur", false);
        }
    }
    public void ljubicastiNapitak()
    {
        SoundManager.Instance.PlayClickSound();
        brojNapitaka.text = PlayerPrefs.GetInt("ljubicasti", 0).ToString() + "<sprite=1>";
        if (!ljubicasti.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("LjubicastiNapitakStoji"))
        {
            proizvod = "ljubicasti";
            aktivanBlur = true;
            imeNapitci.text = "Potion of Forgetfulness";
            opisNapitci.text = "Lose all potion effects. Drink it to reset. But beware, you'll have to buy potions again!";
            cenaNapitci.text = "buy " + cenaLjubicasti.ToString() + "<sprite=0>";
            blurNapitci.transform.SetAsLastSibling();
            ljubicasti.transform.SetAsLastSibling();
            ljubicasti.GetComponent<Animator>().SetBool("ljubicasti", true);
            blurNapitci.SetActive(true);
            blurNapitci.GetComponent<Animator>().SetBool("blur", true);
        }
        else
        {
            aktivanBlur = false;
            ljubicasti.GetComponent<Animator>().SetBool("ljubicasti", false);
            blurNapitci.GetComponent<Animator>().SetBool("blur", false);
        }
    }
    public void crveniNapitak()
    {
        SoundManager.Instance.PlayClickSound();
        brojNapitaka.text = PlayerPrefs.GetInt("crveni", 0).ToString() + "<sprite=2>";
        if (!crveni.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("CrveniNapitakStoji"))
        {
            proizvod = "crveni";
            aktivanBlur = true;
            imeNapitci.text = "Potion of Slowness";
            opisNapitci.text = "Decrease speed growth. Slow down to score higher. But beware, too much will make you too slow!";
            cenaNapitci.text = "buy " + cenaCrveni.ToString() + "<sprite=0>";
            blurNapitci.transform.SetAsLastSibling();
            crveni.transform.SetAsLastSibling();
            crveni.GetComponent<Animator>().SetBool("crveni", true);
            blurNapitci.SetActive(true);
            blurNapitci.GetComponent<Animator>().SetBool("blur", true);
        }
        else
        {
            aktivanBlur = false;
            crveni.GetComponent<Animator>().SetBool("crveni", false);
            blurNapitci.GetComponent<Animator>().SetBool("blur", false);
        }
    }
    public void zutiNapitak()
    {
        SoundManager.Instance.PlayClickSound();
        brojNapitaka.text = PlayerPrefs.GetInt("zuti", 0).ToString() + "<sprite=3>";
        if (!zuti.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("ZutiStoji"))
        {
            proizvod = "zuti";
            aktivanBlur = true;
            imeNapitci.text = "Potion of Wealthiness";
            opisNapitci.text = "Reduce points needed for coin by 1. If you go negative, you'll start with that amount of coins. Money, money, money!";
            cenaNapitci.text = "buy " + cenaZuti.ToString() + "<sprite=0>";
            blurNapitci.transform.SetAsLastSibling();
            zuti.transform.SetAsLastSibling();
            zuti.GetComponent<Animator>().SetBool("zuti", true);
            blurNapitci.SetActive(true);
            blurNapitci.GetComponent<Animator>().SetBool("blur", true);
        }
        else
        {
            aktivanBlur = false;
            zuti.GetComponent<Animator>().SetBool("zuti", false);
            blurNapitci.GetComponent<Animator>().SetBool("blur", false);
        }
    }
    public void narandzastiNapitak()
    {
        SoundManager.Instance.PlayClickSound();
        brojNapitaka.text = PlayerPrefs.GetInt("narandzasti", 0).ToString() + "<sprite=4>";
        if (!narandzasti.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("NarandzastiNapitakStoji"))
        {
            proizvod = "narandzasti";
            aktivanBlur = true;
            imeNapitci.text = "Potion of Guardians";
            if (PlayerPrefs.GetInt("Level", 1) == 4)
            {
                opisNapitci.text = "Gives immunity to one random color, changing each time. More potions grant immunity to more colors. After 11 potions, no more effects.";
            }
            else
            {
                opisNapitci.text = "Gives immunity to one random color, changing each time. More potions grant immunity to more colors. After 5 potions, no more effects.";
            }
            cenaNapitci.text = "buy " + cenaNarandzasti.ToString() + "<sprite=0>";
            blurNapitci.transform.SetAsLastSibling();
            narandzasti.transform.SetAsLastSibling();
            narandzasti.GetComponent<Animator>().SetBool("narandzasti", true);
            blurNapitci.SetActive(true);
            blurNapitci.GetComponent<Animator>().SetBool("blur", true);
        }
        else
        {
            aktivanBlur = false;
            narandzasti.GetComponent<Animator>().SetBool("narandzasti", false);
            blurNapitci.GetComponent<Animator>().SetBool("blur", false);
        }
    }
    public void zeleniNapitak()
    {
        SoundManager.Instance.PlayClickSound();
        brojNapitaka.text = PlayerPrefs.GetInt("zeleni", 0).ToString() + "<sprite=5>";
        if (!zeleni.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("ZeleniNapitakStoji"))
        {
            proizvod = "zeleni";
            aktivanBlur = true;
            imeNapitci.text = "Potion of Enhances";
            opisNapitci.text = "After a streak without losing life, points double, then triple... Lose life and effect is lost. More potions reduce points needed. No effect after 10 drinks.";
            cenaNapitci.text = "buy " + cenaZeleni.ToString() + "<sprite=0>";
            blurNapitci.transform.SetAsLastSibling();
            zeleni.transform.SetAsLastSibling();
            zeleni.GetComponent<Animator>().SetBool("zeleni", true);
            blurNapitci.SetActive(true);
            blurNapitci.GetComponent<Animator>().SetBool("blur", true);
        }
        else
        {
            aktivanBlur = false;
            zeleni.GetComponent<Animator>().SetBool("zeleni", false);
            blurNapitci.GetComponent<Animator>().SetBool("blur", false);
        }
    }
    //Totemi
    //level1
    public void dzojstikTotem()
    {
        SoundManager.Instance.PlayClickSound();
        if (!dzojstik.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("PlaviNapitakStoji"))
        {
            proizvod = "dzojstik";
            aktivanBlur = true;
            imeTotemi.text = "Totem of Simplicity";
            opisTotemi.text = "Makes mixed colors appear more rarely. Confused by mixed colors? Take newer controls, lay back and relax.";
            cenaTotemi.text = cenaDzojstik.ToString() + "<sprite=0>";
            blurTotemi.transform.SetAsLastSibling();
            dzojstik.transform.SetAsLastSibling();
            dzojstik.GetComponent<Animator>().SetBool("plavi", true);
            blurTotemi.SetActive(true);
            blurTotemi.GetComponent<Animator>().SetBool("blur", true);
        }
        else
        {
            aktivanBlur = false;
            dzojstik.GetComponent<Animator>().SetBool("plavi", false);
            blurTotemi.GetComponent<Animator>().SetBool("blur", false);
        }
    }
    public void tvTotem()
    {
        SoundManager.Instance.PlayClickSound();
        if (!tv.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("LjubicastiNapitakStoji"))
        {
            proizvod = "tv";
            aktivanBlur = true;
            imeTotemi.text = "Totem of Rewind";
            opisTotemi.text = "Gives you a 25% chance to rewind a move if you lost. Made a fatal mistake? Don't worry, this old box has your back. But be careful, don't embarrass yourself... again.";
            cenaTotemi.text = cenaTv.ToString() + "<sprite=0>";
            blurTotemi.transform.SetAsLastSibling();
            tv.transform.SetAsLastSibling();
            tv.GetComponent<Animator>().SetBool("ljubicasti", true);
            blurTotemi.SetActive(true);
            blurTotemi.GetComponent<Animator>().SetBool("blur", true);
        }
        else
        {
            aktivanBlur = false;
            tv.GetComponent<Animator>().SetBool("ljubicasti", false);
            blurTotemi.GetComponent<Animator>().SetBool("blur", false);
        }
    }
    //level2
    public void kapibaraTotem()
    {
        SoundManager.Instance.PlayClickSound();
        if (!kapibara.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("PlaviNapitakStoji"))
        {
            proizvod = "kapibara";
            aktivanBlur = true;
            imeTotemi.text = "Totem of Holy capybara";
            opisTotemi.text = "Destroying orange, green, and purple squares gives +3 score. Make that color mixing knowledge worth something. Remember, holy capybara is proud of you.";
            cenaTotemi.text = cenaKapibara.ToString() + "<sprite=0>";
            blurTotemi.transform.SetAsLastSibling();
            kapibara.transform.SetAsLastSibling();
            kapibara.GetComponent<Animator>().SetBool("plavi", true);
            blurTotemi.SetActive(true);
            blurTotemi.GetComponent<Animator>().SetBool("blur", true);
        }
        else
        {
            aktivanBlur = false;
            kapibara.GetComponent<Animator>().SetBool("plavi", false);
            blurTotemi.GetComponent<Animator>().SetBool("blur", false);
        }
    }
    public void tocakTotem()
    {
        SoundManager.Instance.PlayClickSound();
        if (!tocak.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("LjubicastiNapitakStoji"))
        {
            proizvod = "tocak";
            aktivanBlur = true;
            imeTotemi.text = "Totem of Hamster wheel";
            opisTotemi.text = "Gives you a sign when mixed color is coming. Animals are amazing, they have senses that we can not even imagine... For example, Floridian mouses can feel mixed colors for some reason.";
            cenaTotemi.text = cenaTocak.ToString() + "<sprite=0>";
            blurTotemi.transform.SetAsLastSibling();
            tocak.transform.SetAsLastSibling();
            tocak.GetComponent<Animator>().SetBool("ljubicasti", true);
            blurTotemi.SetActive(true);
            blurTotemi.GetComponent<Animator>().SetBool("blur", true);
        }
        else
        {
            aktivanBlur = false;
            tocak.GetComponent<Animator>().SetBool("ljubicasti", false);
            blurTotemi.GetComponent<Animator>().SetBool("blur", false);
        }
    }
    public void srceTotem()
    {
        SoundManager.Instance.PlayClickSound();
        if (!srce.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("CrveniNapitakStoji"))
        {
            proizvod = "srce";
            aktivanBlur = true;
            imeTotemi.text = "Totem of Twin hearts";
            opisTotemi.text = "Every heart that you get now gives 2 extra lives. Remember, many people will tell you that you have a good heart, but only doctors know the truth. 9 out of 10 doctors recommend this totem.";
            cenaTotemi.text = cenaSrce.ToString() + "<sprite=0>";
            blurTotemi.transform.SetAsLastSibling();
            srce.transform.SetAsLastSibling();
            srce.GetComponent<Animator>().SetBool("crveni", true);
            blurTotemi.SetActive(true);
            blurTotemi.GetComponent<Animator>().SetBool("blur", true);
        }
        else
        {
            aktivanBlur = false;
            srce.GetComponent<Animator>().SetBool("crveni", false);
            blurTotemi.GetComponent<Animator>().SetBool("blur", false);
        }
    }
    //level3
    public void krampTotem()
    {
        SoundManager.Instance.PlayClickSound();
        if (!kramp.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("PlaviNapitakStoji"))
        {
            proizvod = "kramp";
            aktivanBlur = true;
            imeTotemi.text = "Totem of Ice breaker";
            opisTotemi.text = "Removes all ice squares. Have you ever had trouble starting your car or struggled to walk because of ice? Don't worry, this totem will solve your issues... You just need to... get it out of the ice.";
            cenaTotemi.text = cenaKramp.ToString() + "<sprite=0>";
            blurTotemi.transform.SetAsLastSibling();
            kramp.transform.SetAsLastSibling();
            kramp.GetComponent<Animator>().SetBool("plavi", true);
            blurTotemi.SetActive(true);
            blurTotemi.GetComponent<Animator>().SetBool("blur", true);
        }
        else
        {
            aktivanBlur = false;
            kramp.GetComponent<Animator>().SetBool("plavi", false);
            blurTotemi.GetComponent<Animator>().SetBool("blur", false);
        }
    }
    public void lopataTotem()
    {
        SoundManager.Instance.PlayClickSound();
        if (!lopata.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("LjubicastiNapitakStoji"))
        {
            proizvod = "lopata";
            aktivanBlur = true;
            imeTotemi.text = "Totem of Snow breaker";
            opisTotemi.text = "Removes all snow squares. Requires the Totem of Ice Breaker to be bought, because you obviously need a pickaxe to use a shovel, you know... I'm not the one making the rules.";
            cenaTotemi.text = cenaLopata.ToString() + "<sprite=0>";
            blurTotemi.transform.SetAsLastSibling();
            lopata.transform.SetAsLastSibling();
            lopata.GetComponent<Animator>().SetBool("ljubicasti", true);
            blurTotemi.SetActive(true);
            blurTotemi.GetComponent<Animator>().SetBool("blur", true);
        }
        else
        {
            aktivanBlur = false;
            lopata.GetComponent<Animator>().SetBool("ljubicasti", false);
            blurTotemi.GetComponent<Animator>().SetBool("blur", false);
        }
    }
    public void sneskoTotem()
    {
        SoundManager.Instance.PlayClickSound();
        if (!snesko.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("CrveniNapitakStoji"))
        {
            proizvod = "snesko";
            aktivanBlur = true;
            imeTotemi.text = "Totem of The snowman";
            opisTotemi.text = "Makes it so you cannot lose because of triangles. Ever seen a triangle snowman? Yeah, me neither. The quicker you buy it, the quicker it will disappear. Now, buy it!";
            cenaTotemi.text = cenaSnesko.ToString() + "<sprite=0>";
            blurTotemi.transform.SetAsLastSibling();
            snesko.transform.SetAsLastSibling();
            snesko.GetComponent<Animator>().SetBool("crveni", true);
            blurTotemi.SetActive(true);
            blurTotemi.GetComponent<Animator>().SetBool("blur", true);
        }
        else
        {
            aktivanBlur = false;
            snesko.GetComponent<Animator>().SetBool("crveni", false);
            blurTotemi.GetComponent<Animator>().SetBool("blur", false);
        }
    }
    public void poklonTotem()
    {
        SoundManager.Instance.PlayClickSound();
        if (!poklon.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("ZutiStoji"))
        {
            proizvod = "poklon";
            aktivanBlur = true;
            imeTotemi.text = "Totem of The santa";
            opisTotemi.text = "Randomly activates and gives double points for a short time. Is it a bird? Is it a plane? No, it's a non-motor snow vehicle! ...Sleds, I'm talking about sleds, they're the sign for double points.";
            cenaTotemi.text = cenaPoklon.ToString() + "<sprite=0>";
            blurTotemi.transform.SetAsLastSibling();
            poklon.transform.SetAsLastSibling();
            poklon.GetComponent<Animator>().SetBool("zuti", true);
            blurTotemi.SetActive(true);
            blurTotemi.GetComponent<Animator>().SetBool("blur", true);
        }
        else
        {
            aktivanBlur = false;
            poklon.GetComponent<Animator>().SetBool("zuti", false);
            blurTotemi.GetComponent<Animator>().SetBool("blur", false);
        }
    }
    //level4
    public void kasicaTotem()
    {
        SoundManager.Instance.PlayClickSound();
        if (!kasica.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("PlaviNapitakStoji"))
        {
            proizvod = "kasica";
            aktivanBlur = true;
            imeTotemi.text = "Totem of Bling bling";
            opisTotemi.text = "Instead of 1, you get 2 coins for the needed score. Speed up the upgrade process by earning twice as much money as before. But watch out, money doesn't grow on trees.";
            cenaTotemi.text = cenaKasica.ToString() + "<sprite=0>";
            blurTotemi.transform.SetAsLastSibling();
            kasica.transform.SetAsLastSibling();
            kasica.GetComponent<Animator>().SetBool("plavi", true);
            blurTotemi.SetActive(true);
            blurTotemi.GetComponent<Animator>().SetBool("blur", true);
        }
        else
        {
            aktivanBlur = false;
            kasica.GetComponent<Animator>().SetBool("plavi", false);
            blurTotemi.GetComponent<Animator>().SetBool("blur", false);
        }
    }
    public void hobotnicaTotem()
    {
        SoundManager.Instance.PlayClickSound();
        if (!hobotnica.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("LjubicastiNapitakStoji"))
        {
            proizvod = "hobotnica";
            aktivanBlur = true;
            imeTotemi.text = "Totem of Camouflage";
            opisTotemi.text = "Randomly makes a few next shapes the same color. Like an octopus with 3 hearts, you'll get 3 matching shapes when this totem activates.";
            cenaTotemi.text = cenaHobotnica.ToString() + "<sprite=0>";
            blurTotemi.transform.SetAsLastSibling();
            hobotnica.transform.SetAsLastSibling();
            hobotnica.GetComponent<Animator>().SetBool("ljubicasti", true);
            blurTotemi.SetActive(true);
            blurTotemi.GetComponent<Animator>().SetBool("blur", true);
        }
        else
        {
            aktivanBlur = false;
            hobotnica.GetComponent<Animator>().SetBool("ljubicasti", false);
            blurTotemi.GetComponent<Animator>().SetBool("blur", false);
        }
    }
    public void zlatnaRibicaTotem()
    {
        SoundManager.Instance.PlayClickSound();
        if (!zlatnaRibica.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("CrveniNapitakStoji"))
        {
            proizvod = "zlatnaRibica";
            aktivanBlur = true;
            imeTotemi.text = "Totem of Golden goldfish";
            opisTotemi.text = "Activates Gold Rush, making a few next fishes worth 1 coin each. There's a myth that goldfish have a 3-second memory... Let's hope this one remembers the way to you!";
            cenaTotemi.text = cenaZlatnaRibica.ToString() + "<sprite=0>";
            blurTotemi.transform.SetAsLastSibling();
            zlatnaRibica.transform.SetAsLastSibling();
            zlatnaRibica.GetComponent<Animator>().SetBool("crveni", true);
            blurTotemi.SetActive(true);
            blurTotemi.GetComponent<Animator>().SetBool("blur", true);
        }
        else
        {
            aktivanBlur = false;
            zlatnaRibica.GetComponent<Animator>().SetBool("crveni", false);
            blurTotemi.GetComponent<Animator>().SetBool("blur", false);
        }
    }
    public void lobanjaTotem()
    {
        SoundManager.Instance.PlayClickSound();
        if (!lobanja.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("ZutiStoji"))
        {
            proizvod = "lobanja";
            aktivanBlur = true;
            imeTotemi.text = "Totem of Resurrection";
            opisTotemi.text = "20% chance to be resurrected when you lose. Arghh! Made a fatal mistake? Don't worry, the ghosts of dead pirates are with you. Wait... pirates? Ghosts?";
            cenaTotemi.text = cenaLobanja.ToString() + "<sprite=0>";
            blurTotemi.transform.SetAsLastSibling();
            lobanja.transform.SetAsLastSibling();
            lobanja.GetComponent<Animator>().SetBool("zuti", true);
            blurTotemi.SetActive(true);
            blurTotemi.GetComponent<Animator>().SetBool("blur", true);
        }
        else
        {
            aktivanBlur = false;
            lobanja.GetComponent<Animator>().SetBool("zuti", false);
            blurTotemi.GetComponent<Animator>().SetBool("blur", false);
        }
    }
    public void krabaTotem()
    {
        SoundManager.Instance.PlayClickSound();
        if (!kraba.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("NarandzastiNapitakStoji"))
        {
            proizvod = "kraba";
            aktivanBlur = true;
            imeTotemi.text = "Totem of Sir crab";
            opisTotemi.text = "Crabs can appear more often and even infinitely! This crab loves money, but is it ready for all that fortune? Maybe with this totem, the money will start chasing him!?";
            cenaTotemi.text = cenaKraba.ToString() + "<sprite=0>";
            blurTotemi.transform.SetAsLastSibling();
            kraba.transform.SetAsLastSibling();
            kraba.GetComponent<Animator>().SetBool("narandzasti", true);
            blurTotemi.SetActive(true);
            blurTotemi.GetComponent<Animator>().SetBool("blur", true);
        }
        else
        {
            aktivanBlur = false;
            kraba.GetComponent<Animator>().SetBool("narandzasti", false);
            blurTotemi.GetComponent<Animator>().SetBool("blur", false);
        }
    }
    //level5
    public void satelitTotem()
    {
        SoundManager.Instance.PlayClickSound();
        if (!satelit.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("PlaviNapitakStoji"))
        {
            proizvod = "satelit";
            aktivanBlur = true;
            imeTotemi.text = "Totem of Rain";
            opisTotemi.text = "Randomly activates Crazy Rush, making meteor rain! You can't lose while it's active, but you can rack up a ton of points. So what are you waiting for? Go crazy, go stupid!!!";
            cenaTotemi.text = cenaSatelit.ToString() + "<sprite=0>";
            blurTotemi.transform.SetAsLastSibling();
            satelit.transform.SetAsLastSibling();
            satelit.GetComponent<Animator>().SetBool("plavi", true);
            blurTotemi.SetActive(true);
            blurTotemi.GetComponent<Animator>().SetBool("blur", true);
        }
        else
        {
            aktivanBlur = false;
            satelit.GetComponent<Animator>().SetBool("plavi", false);
            blurTotemi.GetComponent<Animator>().SetBool("blur", false);
        }
    }
    public void antenaTotem()
    {
        SoundManager.Instance.PlayClickSound();
        if (!antena.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("LjubicastiNapitakStoji"))
        {
            proizvod = "antena";
            aktivanBlur = true;
            imeTotemi.text = "Totem of Shield";
            opisTotemi.text = "Randomly activates a shield, stopping every meteor and preventing you from losing. Do you know what warriors used shields for? Well, for sledding, obviously...";
            cenaTotemi.text = cenaAntena.ToString() + "<sprite=0>";
            blurTotemi.transform.SetAsLastSibling();
            antena.transform.SetAsLastSibling();
            antena.GetComponent<Animator>().SetBool("ljubicasti", true);
            blurTotemi.SetActive(true);
            blurTotemi.GetComponent<Animator>().SetBool("blur", true);
        }
        else
        {
            aktivanBlur = false;
            antena.GetComponent<Animator>().SetBool("ljubicasti", false);
            blurTotemi.GetComponent<Animator>().SetBool("blur", false);
        }
    }
    public void mafinTotem()
    {
        SoundManager.Instance.PlayClickSound();
        if (!mafin.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("CrveniNapitakStoji"))
        {
            proizvod = "mafin";
            aktivanBlur = true;
            imeTotemi.text = "Totem of The muffin";
            opisTotemi.text = "Gives you a few extra lives to start, anywhere from 0 to 5. There are many things you might not like to eat, but are alien muffins one of them? You can't know until you try.";
            cenaTotemi.text = cenaMafin.ToString() + "<sprite=0>";
            blurTotemi.transform.SetAsLastSibling();
            mafin.transform.SetAsLastSibling();
            mafin.GetComponent<Animator>().SetBool("crveni", true);
            blurTotemi.SetActive(true);
            blurTotemi.GetComponent<Animator>().SetBool("blur", true);
        }
        else
        {
            aktivanBlur = false;
            mafin.GetComponent<Animator>().SetBool("crveni", false);
            blurTotemi.GetComponent<Animator>().SetBool("blur", false);
        }
    }
    public void vanzemaljacTotem()
    {
        SoundManager.Instance.PlayClickSound();
        if (!vanzemaljac.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("ZutiStoji"))
        {
            proizvod = "vanzemaljac";
            aktivanBlur = true;
            imeTotemi.text = "Totem of The alien";
            opisTotemi.text = "Gives you 5 points for every 10 triangles you break. Aren't cartoons weird? Some animals are human-like, while others are pets. Anyway, this alien is a totem.";
            cenaTotemi.text = cenaVanzemaljac.ToString() + "<sprite=0>";
            blurTotemi.transform.SetAsLastSibling();
            vanzemaljac.transform.SetAsLastSibling();
            vanzemaljac.GetComponent<Animator>().SetBool("zuti", true);
            blurTotemi.SetActive(true);
            blurTotemi.GetComponent<Animator>().SetBool("blur", true);
        }
        else
        {
            aktivanBlur = false;
            vanzemaljac.GetComponent<Animator>().SetBool("zuti", false);
            blurTotemi.GetComponent<Animator>().SetBool("blur", false);
        }
    }
    public void pistoljKvadratTotem()
    {
        SoundManager.Instance.PlayClickSound();
        if (!pistoljKvadrat.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("NarandzastiNapitakStoji"))
        {
            proizvod = "pistoljKvadrat";
            aktivanBlur = true;
            imeTotemi.text = "Totem of Square gun";
            opisTotemi.text = "Prevents squares from appearing inside circles. Been fooled by squares in circles too many times? Make them vanish and get closer to those shiny crystals. I know you want them.";
            cenaTotemi.text = cenaPistoljKvadrat.ToString() + "<sprite=0>";
            blurTotemi.transform.SetAsLastSibling();
            pistoljKvadrat.transform.SetAsLastSibling();
            pistoljKvadrat.GetComponent<Animator>().SetBool("narandzasti", true);
            blurTotemi.SetActive(true);
            blurTotemi.GetComponent<Animator>().SetBool("blur", true);
        }
        else
        {
            aktivanBlur = false;
            pistoljKvadrat.GetComponent<Animator>().SetBool("narandzasti", false);
            blurTotemi.GetComponent<Animator>().SetBool("blur", false);
        }
    }
    public void pistoljTrougaoTotem()
    {
        SoundManager.Instance.PlayClickSound();
        if (!pistoljTrougao.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("ZeleniNapitakStoji"))
        {
            proizvod = "pistoljTrougao";
            aktivanBlur = true;
            imeTotemi.text = "Totem of Triangle gun";
            opisTotemi.text = "Prevents triangles from appearing inside circles. Been fooled by triangles in circles too many times? Make them vanish... wait, this sounds familiar. Anyway, go grab those crystals!";
            cenaTotemi.text = cenaPistoljTrougao.ToString() + "<sprite=0>";
            blurTotemi.transform.SetAsLastSibling();
            pistoljTrougao.transform.SetAsLastSibling();
            pistoljTrougao.GetComponent<Animator>().SetBool("zeleni", true);
            blurTotemi.SetActive(true);
            blurTotemi.GetComponent<Animator>().SetBool("blur", true);
        }
        else
        {
            aktivanBlur = false;
            pistoljTrougao.GetComponent<Animator>().SetBool("zeleni", false);
            blurTotemi.GetComponent<Animator>().SetBool("blur", false);
        }
    }

    public void namestanjeTotema()
    {
        if (Meni.level == 1)
        {
            if (PlayerPrefs.GetInt("dzojstik", 0) == 1 && dzojstikAktivan)
            {
                if (blurTotemi.activeInHierarchy)
                {
                    aktivanBlur = false;
                    blurTotemi.GetComponent<Animator>().SetBool("blur", false);
                }
                dzojstik.SetActive(false);
                dzojstikAktivan = false;
            }
            if (PlayerPrefs.GetInt("tv", 0) == 1 && tvAktivan)
            {
                if (blurTotemi.activeInHierarchy)
                {
                    aktivanBlur = false;
                    blurTotemi.GetComponent<Animator>().SetBool("blur", false);
                }
                tv.SetActive(false);
                tvAktivan = false;
            }
        }
        if (Meni.level == 2)
        {
            if (PlayerPrefs.GetInt("kapibara", 0) == 1 && kapibaraAktivan)
            {
                if (blurTotemi.activeInHierarchy)
                {
                    aktivanBlur = false;
                    blurTotemi.GetComponent<Animator>().SetBool("blur", false);
                }
                kapibara.SetActive(false);
                kapibaraAktivan = false;
            }
            if (PlayerPrefs.GetInt("tocak", 0) == 1 && tocakAktivan)
            {
                if (blurTotemi.activeInHierarchy)
                {
                    aktivanBlur = false;
                    blurTotemi.GetComponent<Animator>().SetBool("blur", false);
                }
                tocak.SetActive(false);
                tocakAktivan = false;
            }
            if (PlayerPrefs.GetInt("srce", 0) == 1 && srceAktivan)
            {
                if (blurTotemi.activeInHierarchy)
                {
                    aktivanBlur = false;
                    blurTotemi.GetComponent<Animator>().SetBool("blur", false);
                }
                srce.SetActive(false);
                srceAktivan = false;
            }
        }
        if (Meni.level == 3)
        {
            if (PlayerPrefs.GetInt("kramp", 0) == 1 && krampAktivan)
            {
                if (blurTotemi.activeInHierarchy)
                {
                    aktivanBlur = false;
                    blurTotemi.GetComponent<Animator>().SetBool("blur", false);
                }
                kramp.SetActive(false);
                krampAktivan = false;
            }
            if (PlayerPrefs.GetInt("lopata", 0) == 1 && lopataAktivan)
            {
                if (blurTotemi.activeInHierarchy)
                {
                    aktivanBlur = false;
                    blurTotemi.GetComponent<Animator>().SetBool("blur", false);
                }
                lopata.SetActive(false);
                lopataAktivan = false;
            }
            if (PlayerPrefs.GetInt("snesko", 0) == 1 && sneskoAktivan)
            {
                if (blurTotemi.activeInHierarchy)
                {
                    aktivanBlur = false;
                    blurTotemi.GetComponent<Animator>().SetBool("blur", false);
                }
                snesko.SetActive(false);
                sneskoAktivan = false;
            }
            if (PlayerPrefs.GetInt("poklon", 0) == 1 && poklonAktivan)
            {
                if (blurTotemi.activeInHierarchy)
                {
                    aktivanBlur = false;
                    blurTotemi.GetComponent<Animator>().SetBool("blur", false);
                }
                poklon.SetActive(false);
                poklonAktivan = false;
            }
        }
        else if (Meni.level == 4)
        {
            if (PlayerPrefs.GetInt("kasica", 0) == 1 && kasicaAktivan)
            {
                if (blurTotemi.activeInHierarchy)
                {
                    aktivanBlur = false;
                    blurTotemi.GetComponent<Animator>().SetBool("blur", false);
                }
                kasica.SetActive(false);
                kasicaAktivan = false;
            }
            if (PlayerPrefs.GetInt("hobotnica", 0) == 1 && hobotnicaAktivan)
            {
                if (blurTotemi.activeInHierarchy)
                {
                    aktivanBlur = false;
                    blurTotemi.GetComponent<Animator>().SetBool("blur", false);
                }
                hobotnica.SetActive(false);
                hobotnicaAktivan = false;
            }
            if (PlayerPrefs.GetInt("zlatnaRibica", 0) == 1 && zlatnaRibicaAktivan)
            {
                if (blurTotemi.activeInHierarchy)
                {
                    aktivanBlur = false;
                    blurTotemi.GetComponent<Animator>().SetBool("blur", false);
                }
                zlatnaRibica.SetActive(false);
                zlatnaRibicaAktivan = false;
            }
            if (PlayerPrefs.GetInt("lobanja", 0) == 1 && lobanjaAktivan)
            {
                if (blurTotemi.activeInHierarchy)
                {
                    aktivanBlur = false;
                    blurTotemi.GetComponent<Animator>().SetBool("blur", false);
                }
                lobanja.SetActive(false);
                lobanjaAktivan = false;
            }
            if (PlayerPrefs.GetInt("kraba", 0) == 1 && krabaAktivan)
            {
                if (blurTotemi.activeInHierarchy)
                {
                    aktivanBlur = false;
                    blurTotemi.GetComponent<Animator>().SetBool("blur", false);
                }
                kraba.SetActive(false);
                krabaAktivan = false;
            }
        }
        else if (Meni.level == 5)
        {
            if (PlayerPrefs.GetInt("satelit", 0) == 1 && satelitAktivan)
            {
                if (blurTotemi.activeInHierarchy)
                {
                    aktivanBlur = false;
                    blurTotemi.GetComponent<Animator>().SetBool("blur", false);
                }
                satelit.SetActive(false);
                satelitAktivan = false;
            }
            if (PlayerPrefs.GetInt("antena", 0) == 1 && antenaAktivan)
            {
                if (blurTotemi.activeInHierarchy)
                {
                    aktivanBlur = false;
                    blurTotemi.GetComponent<Animator>().SetBool("blur", false);
                }
                antena.SetActive(false);
                antenaAktivan = false;
            }
            if (PlayerPrefs.GetInt("mafin", 0) == 1 && mafinAktivan)
            {
                if (blurTotemi.activeInHierarchy)
                {
                    aktivanBlur = false;
                    blurTotemi.GetComponent<Animator>().SetBool("blur", false);
                }
                mafin.SetActive(false);
                mafinAktivan = false;
            }
            if (PlayerPrefs.GetInt("vanzemaljac", 0) == 1 && vanzemaljacAktivan)
            {
                if (blurTotemi.activeInHierarchy)
                {
                    aktivanBlur = false;
                    blurTotemi.GetComponent<Animator>().SetBool("blur", false);
                }
                vanzemaljac.SetActive(false);
                vanzemaljacAktivan = false;
            }
            if (PlayerPrefs.GetInt("pistoljKvadrat", 0) == 1 && pistoljKvadratAktivan)
            {
                if (blurTotemi.activeInHierarchy)
                {
                    aktivanBlur = false;
                    blurTotemi.GetComponent<Animator>().SetBool("blur", false);
                }
                pistoljKvadrat.SetActive(false);
                pistoljKvadratAktivan = false;
            }
            if (PlayerPrefs.GetInt("pistoljTrougao", 0) == 1 && pistoljTrougaoAktivan)
            {
                if (blurTotemi.activeInHierarchy)
                {
                    aktivanBlur = false;
                    blurTotemi.GetComponent<Animator>().SetBool("blur", false);
                }
                pistoljTrougao.SetActive(false);
                pistoljTrougaoAktivan = false;
            }
        }
    }
    public void BojaZaBuy()
    {
        if (proizvod == "plavi" && cenaPlavi <= PlayerPrefs.GetInt("Novcici", 0))
        {
            cenaNapitci.faceColor = Color.white;
        }
        else if (proizvod == "crveni" && cenaCrveni <= PlayerPrefs.GetInt("Novcici", 0))
        {
            cenaNapitci.faceColor = Color.white;
        }
        else if (proizvod == "ljubicasti" && cenaLjubicasti <= PlayerPrefs.GetInt("Novcici", 0))
        {
            cenaNapitci.faceColor = Color.white;
        }
        else if (proizvod == "zuti" && cenaZuti <= PlayerPrefs.GetInt("Novcici", 0))
        {
            cenaNapitci.faceColor = Color.white;
        }
        else if (proizvod == "narandzasti" && cenaNarandzasti <= PlayerPrefs.GetInt("Novcici", 0))
        {
            cenaNapitci.faceColor = Color.white;
        }
        else if (proizvod == "zeleni" && cenaZeleni <= PlayerPrefs.GetInt("Novcici", 0))
        {
            cenaNapitci.faceColor = Color.white;
        }
        else if (proizvod == "dzojstik" && cenaDzojstik <= PlayerPrefs.GetInt("Novcici", 0))
        {
            cenaTotemi.faceColor = Color.white;
        }
        else if (proizvod == "tv" && cenaTv <= PlayerPrefs.GetInt("Novcici", 0))
        {
            cenaTotemi.faceColor = Color.white;
        }
        else if (proizvod == "kapibara" && cenaKapibara <= PlayerPrefs.GetInt("Novcici", 0))
        {
            cenaTotemi.faceColor = Color.white;
        }
        else if (proizvod == "tocak" && cenaTocak <= PlayerPrefs.GetInt("Novcici", 0))
        {
            cenaTotemi.faceColor = Color.white;
        }
        else if (proizvod == "srce" && cenaSrce <= PlayerPrefs.GetInt("Novcici", 0))
        {
            cenaTotemi.faceColor = Color.white;
        }
        else if (proizvod == "kramp" && cenaKramp <= PlayerPrefs.GetInt("Novcici", 0))
        {
            cenaTotemi.faceColor = Color.white;
        }
        else if (proizvod == "lopata" && cenaLopata <= PlayerPrefs.GetInt("Novcici", 0) && PlayerPrefs.GetInt("kramp", 0) == 1)
        {
            cenaTotemi.faceColor = Color.white;
        }
        else if (proizvod == "snesko" && cenaSnesko <= PlayerPrefs.GetInt("Novcici", 0))
        {
            cenaTotemi.faceColor = Color.white;
        }
        else if (proizvod == "poklon" && cenaPoklon <= PlayerPrefs.GetInt("Novcici", 0))
        {
            cenaTotemi.faceColor = Color.white;
        }
        else if (proizvod == "kasica" && cenaKasica <= PlayerPrefs.GetInt("Novcici", 0))
        {
            cenaTotemi.faceColor = Color.white;
        }
        else if (proizvod == "hobotnica" && cenaHobotnica <= PlayerPrefs.GetInt("Novcici", 0))
        {
            cenaTotemi.faceColor = Color.white;
        }
        else if (proizvod == "zlatnaRibica" && cenaZlatnaRibica <= PlayerPrefs.GetInt("Novcici", 0))
        {
            cenaTotemi.faceColor = Color.white;
        }
        else if (proizvod == "lobanja" && cenaLobanja <= PlayerPrefs.GetInt("Novcici", 0))
        {
            cenaTotemi.faceColor = Color.white;
        }
        else if (proizvod == "kraba" && cenaKraba <= PlayerPrefs.GetInt("Novcici", 0))
        {
            cenaTotemi.faceColor = Color.white;
        }
        else if (proizvod == "satelit" && cenaSatelit <= PlayerPrefs.GetInt("Novcici", 0))
        {
            cenaTotemi.faceColor = Color.white;
        }
        else if (proizvod == "antena" && cenaAntena <= PlayerPrefs.GetInt("Novcici", 0))
        {
            cenaTotemi.faceColor = Color.white;
        }
        else if (proizvod == "mafin" && cenaMafin <= PlayerPrefs.GetInt("Novcici", 0))
        {
            cenaTotemi.faceColor = Color.white;
        }
        else if (proizvod == "vanzemaljac" && cenaVanzemaljac <= PlayerPrefs.GetInt("Novcici", 0))
        {
            cenaTotemi.faceColor = Color.white;
        }
        else if (proizvod == "pistoljKvadrat" && cenaPistoljKvadrat <= PlayerPrefs.GetInt("Novcici", 0))
        {
            cenaTotemi.faceColor = Color.white;
        }
        else if (proizvod == "pistoljTrougao" && cenaPistoljTrougao <= PlayerPrefs.GetInt("Novcici", 0))
        {
            cenaTotemi.faceColor = Color.white;
        }
        else if (skinPanel.activeInHierarchy && cenaSkina <= PlayerPrefs.GetInt("Dijamanti", 0) && indeksiKupovnaSkinova.Count > 0)
        {
            cenaSkinaTekst.faceColor = Color.white;
        }
        else
        {
            cenaNapitci.faceColor = Color.red;
            cenaTotemi.faceColor = Color.red;
            cenaSkinaTekst.faceColor = Color.red;
        }
    }
    ///Ostalo 
    public void Buy()
    {
        if (proizvod == "plavi" && cenaPlavi <= PlayerPrefs.GetInt("Novcici", 0))
        {
            PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) - cenaPlavi);
            PlayerPrefs.SetInt("plavi", PlayerPrefs.GetInt("plavi", 0) + 1);

            brojNapitaka.text = PlayerPrefs.GetInt("plavi", 0).ToString() + "<sprite=0>";
            PlayerPrefs.SetInt("startScore", PlayerPrefs.GetInt("startScore", 0) + 5);

            cenaPlavi = (int)(cenaPlaviPocetna * Mathf.Pow(poskupljenjeNapitaka, PlayerPrefs.GetInt("plavi", 0)));
            cenaPlavi -= cenaPlavi % 5;
            cenaNapitci.text = "buy " + cenaPlavi.ToString() + "<sprite=0>";

            SoundManager.Instance.PlayClickSound();
            SoundManager.Instance.PlayCoinSound();
        }
        if (proizvod == "crveni" && cenaCrveni <= PlayerPrefs.GetInt("Novcici", 0))
        {
            PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) - cenaCrveni);
            PlayerPrefs.SetInt("crveni", PlayerPrefs.GetInt("crveni", 0) + 1);

            brojNapitaka.text = PlayerPrefs.GetInt("crveni", 0).ToString() + "<sprite=2>";
            PlayerPrefs.SetFloat("speedChange", PlayerPrefs.GetFloat("speedChange", 6) / 1.1f);

            cenaCrveni = (int)(cenaCrveniPocetna * Mathf.Pow(poskupljenjeNapitaka, PlayerPrefs.GetInt("crveni", 0)));
            cenaCrveni -= cenaCrveni % 5;
            cenaNapitci.text = "buy " + cenaCrveni.ToString() + "<sprite=0>";

            SoundManager.Instance.PlayClickSound();
            SoundManager.Instance.PlayCoinSound();
        }
        if (proizvod == "ljubicasti" && cenaLjubicasti <= PlayerPrefs.GetInt("Novcici", 0))
        {
            PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) - cenaLjubicasti);
            PlayerPrefs.SetInt("ljubicasti", PlayerPrefs.GetInt("ljubicasti", 0) + 1);

            brojNapitaka.text = PlayerPrefs.GetInt("ljubicasti", 0).ToString() + "<sprite=1>";
            PlayerPrefs.SetInt("plavi", 0);
            PlayerPrefs.SetInt("crveni", 0);
            PlayerPrefs.SetInt("zuti", 0);
            PlayerPrefs.SetInt("narandzasti", 0);
            PlayerPrefs.SetInt("zeleni", 0);

            PlayerPrefs.SetInt("startScore", 0);
            PlayerPrefs.SetFloat("speedChange", 6);
            PlayerPrefs.SetInt("scoreForCoin", 10);
            PlayerPrefs.SetInt("colorShield", 0);
            PlayerPrefs.SetInt("hotStreak", 15);

            cenaPlavi = cenaPlaviPocetna;
            cenaCrveni = cenaCrveniPocetna;
            cenaZuti = cenaZutiPocetna;
            cenaNarandzasti = cenaNarandzastiPocetna;
            cenaZeleni = cenaZeleniPocetna;

            SoundManager.Instance.PlayClickSound();
            SoundManager.Instance.PlayCoinSound();
        }
        if (proizvod == "zuti" && cenaZuti <= PlayerPrefs.GetInt("Novcici", 0))
        {
            PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) - cenaZuti);
            PlayerPrefs.SetInt("zuti", PlayerPrefs.GetInt("zuti", 0) + 1);

            brojNapitaka.text = PlayerPrefs.GetInt("zuti", 0).ToString() + "<sprite=3>";
            PlayerPrefs.SetInt("scoreForCoin", PlayerPrefs.GetInt("scoreForCoin", 10) - 1);

            cenaZuti = (int)(cenaZutiPocetna * Mathf.Pow(poskupljenjeNapitaka, PlayerPrefs.GetInt("zuti", 0)));
            cenaZuti -= cenaZuti % 5;
            cenaNapitci.text = "buy " + cenaZuti.ToString() + "<sprite=0>";

            SoundManager.Instance.PlayClickSound();
            SoundManager.Instance.PlayCoinSound();
        }
        if (proizvod == "narandzasti" && cenaNarandzasti <= PlayerPrefs.GetInt("Novcici", 0))
        {
            PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) - cenaNarandzasti);
            PlayerPrefs.SetInt("narandzasti", PlayerPrefs.GetInt("narandzasti", 0) + 1);

            brojNapitaka.text = PlayerPrefs.GetInt("narandzasti", 0).ToString() + "<sprite=4>";
            PlayerPrefs.SetInt("colorShield", PlayerPrefs.GetInt("colorShield", 0) + 1);

            cenaNarandzasti = (int)(cenaNarandzastiPocetna * Mathf.Pow(poskupljenjeNapitaka, PlayerPrefs.GetInt("narandzasti", 0)));
            cenaNarandzasti -= cenaNarandzasti % 5;
            cenaNapitci.text = "buy " + cenaNarandzasti.ToString() + "<sprite=0>";

            SoundManager.Instance.PlayClickSound();
            SoundManager.Instance.PlayCoinSound();
        }
        if (proizvod == "zeleni" && cenaZeleni <= PlayerPrefs.GetInt("Novcici", 0))
        {
            PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) - cenaZeleni);
            PlayerPrefs.SetInt("zeleni", PlayerPrefs.GetInt("zeleni", 0) + 1);

            brojNapitaka.text = PlayerPrefs.GetInt("zeleni", 0).ToString() + "<sprite=5>";
            PlayerPrefs.SetInt("hotStreak", PlayerPrefs.GetInt("hotStreak", 15) - 1);

            cenaZeleni = (int)(cenaZeleniPocetna * Mathf.Pow(poskupljenjeNapitaka, PlayerPrefs.GetInt("zeleni", 0)));
            cenaZeleni -= cenaZeleni % 5;
            cenaNapitci.text = "buy " + cenaZeleni.ToString() + "<sprite=0>";

            SoundManager.Instance.PlayClickSound();
            SoundManager.Instance.PlayCoinSound();
        }
        if (proizvod == "dzojstik" && cenaDzojstik <= PlayerPrefs.GetInt("Novcici", 0))
        {
            PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) - cenaDzojstik);

            PlayerPrefs.SetInt("dzojstik", 1);

            SoundManager.Instance.PlayClickSound();
            SoundManager.Instance.PlayCoinSound();
        }
        if (proizvod == "tv" && cenaTv <= PlayerPrefs.GetInt("Novcici", 0))
        {
            PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) - cenaTv);

            PlayerPrefs.SetInt("tv", 1);

            SoundManager.Instance.PlayClickSound();
            SoundManager.Instance.PlayCoinSound();
        }
        if (proizvod == "kapibara" && cenaKapibara <= PlayerPrefs.GetInt("Novcici", 0))
        {
            PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) - cenaKapibara);

            PlayerPrefs.SetInt("kapibara", 1);

            SoundManager.Instance.PlayClickSound();
            SoundManager.Instance.PlayCoinSound();
        }
        if (proizvod == "tocak" && cenaTocak <= PlayerPrefs.GetInt("Novcici", 0))
        {
            PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) - cenaTocak);

            PlayerPrefs.SetInt("tocak", 1);

            SoundManager.Instance.PlayClickSound();
            SoundManager.Instance.PlayCoinSound();
        }
        if (proizvod == "srce" && cenaSrce <= PlayerPrefs.GetInt("Novcici", 0))
        {
            PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) - cenaSrce);

            PlayerPrefs.SetInt("srce", 1);

            SoundManager.Instance.PlayClickSound();
            SoundManager.Instance.PlayCoinSound();
        }
        if (proizvod == "kramp" && cenaKramp <= PlayerPrefs.GetInt("Novcici", 0))
        {
            PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) - cenaKramp);

            PlayerPrefs.SetInt("kramp", 1);

            SoundManager.Instance.PlayClickSound();
            SoundManager.Instance.PlayCoinSound();
        }
        if (proizvod == "lopata" && cenaLopata <= PlayerPrefs.GetInt("Novcici", 0) && PlayerPrefs.GetInt("kramp", 0) == 1)
        {
            PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) - cenaLopata);

            PlayerPrefs.SetInt("lopata", 1);

            SoundManager.Instance.PlayClickSound();
            SoundManager.Instance.PlayCoinSound();
        }
        if (proizvod == "snesko" && cenaSnesko <= PlayerPrefs.GetInt("Novcici", 0))
        {
            PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) - cenaSnesko);

            PlayerPrefs.SetInt("snesko", 1);

            SoundManager.Instance.PlayClickSound();
            SoundManager.Instance.PlayCoinSound();
        }
        if (proizvod == "poklon" && cenaPoklon <= PlayerPrefs.GetInt("Novcici", 0))
        {
            PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) - cenaPoklon);

            PlayerPrefs.SetInt("poklon", 1);

            SoundManager.Instance.PlayClickSound();
            SoundManager.Instance.PlayCoinSound();
        }
        if (proizvod == "kasica" && cenaKasica <= PlayerPrefs.GetInt("Novcici", 0))
        {
            PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) - cenaKasica);

            PlayerPrefs.SetInt("kasica", 1);

            SoundManager.Instance.PlayClickSound();
            SoundManager.Instance.PlayCoinSound();
        }
        if (proizvod == "hobotnica" && cenaHobotnica <= PlayerPrefs.GetInt("Novcici", 0))
        {
            PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) - cenaHobotnica);

            PlayerPrefs.SetInt("hobotnica", 1);

            SoundManager.Instance.PlayClickSound();
            SoundManager.Instance.PlayCoinSound();
        }
        if (proizvod == "zlatnaRibica" && cenaZlatnaRibica <= PlayerPrefs.GetInt("Novcici", 0))
        {
            PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) - cenaZlatnaRibica);

            PlayerPrefs.SetInt("zlatnaRibica", 1);

            SoundManager.Instance.PlayClickSound();
            SoundManager.Instance.PlayCoinSound();
        }
        if (proizvod == "lobanja" && cenaLobanja <= PlayerPrefs.GetInt("Novcici", 0))
        {
            PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) - cenaLobanja);

            PlayerPrefs.SetInt("lobanja", 1);

            SoundManager.Instance.PlayClickSound();
            SoundManager.Instance.PlayCoinSound();
        }
        if (proizvod == "kraba" && cenaKraba <= PlayerPrefs.GetInt("Novcici", 0))
        {
            PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) - cenaKraba);

            PlayerPrefs.SetInt("kraba", 1);

            SoundManager.Instance.PlayClickSound();
            SoundManager.Instance.PlayCoinSound();
        }
        if (proizvod == "satelit" && cenaSatelit <= PlayerPrefs.GetInt("Novcici", 0))
        {
            PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) - cenaSatelit);

            PlayerPrefs.SetInt("satelit", 1);

            SoundManager.Instance.PlayClickSound();
            SoundManager.Instance.PlayCoinSound();
        }
        if (proizvod == "antena" && cenaAntena <= PlayerPrefs.GetInt("Novcici", 0))
        {
            PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) - cenaAntena);

            PlayerPrefs.SetInt("antena", 1);

            SoundManager.Instance.PlayClickSound();
            SoundManager.Instance.PlayCoinSound();
        }
        if (proizvod == "mafin" && cenaMafin <= PlayerPrefs.GetInt("Novcici", 0))
        {
            PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) - cenaMafin);

            PlayerPrefs.SetInt("mafin", 1);

            SoundManager.Instance.PlayClickSound();
            SoundManager.Instance.PlayCoinSound();
        }
        if (proizvod == "vanzemaljac" && cenaVanzemaljac <= PlayerPrefs.GetInt("Novcici", 0))
        {
            PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) - cenaVanzemaljac);

            PlayerPrefs.SetInt("vanzemaljac", 1);

            SoundManager.Instance.PlayClickSound();
            SoundManager.Instance.PlayCoinSound();
        }
        if (proizvod == "pistoljKvadrat" && cenaPistoljKvadrat <= PlayerPrefs.GetInt("Novcici", 0))
        {
            PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) - cenaPistoljKvadrat);

            PlayerPrefs.SetInt("pistoljKvadrat", 1);

            SoundManager.Instance.PlayClickSound();
            SoundManager.Instance.PlayCoinSound();
        }
        if (proizvod == "pistoljTrougao" && cenaPistoljTrougao <= PlayerPrefs.GetInt("Novcici", 0))
        {
            PlayerPrefs.SetInt("Novcici", PlayerPrefs.GetInt("Novcici", 0) - cenaPistoljTrougao);

            PlayerPrefs.SetInt("pistoljTrougao", 1);

            SoundManager.Instance.PlayClickSound();
            SoundManager.Instance.PlayCoinSound();
        }
    }
    public void BuySkin()
    {
        if (cenaSkina <= PlayerPrefs.GetInt("Dijamanti", 0) && indeksiKupovnaSkinova.Count > 0)
        {
            PlayerPrefs.SetInt("Dijamanti", PlayerPrefs.GetInt("Dijamanti", 0) - cenaSkina);
            PlayerPrefs.SetInt("BrojSkinova", PlayerPrefs.GetInt("BrojSkinova", 0) + 1);

            skinPanel.GetComponent<Animator>().SetTrigger("Zatvaranje");
            bob.GetComponent<Animator>().SetTrigger("Zatvaranje");
            StartCoroutine(namestanjeSkina());

            SoundManager.Instance.PlayClickSound();
            SoundManager.Instance.PlayCoinSound();
        }
    }
    public IEnumerator namestanjeSkina()
    {
        Sprite bobTrenutni = bob.GetComponent<Image>().sprite;
        yield return new WaitForSeconds(1.5f);
        bob.GetComponent<Animator>().enabled = false;
        int randomBr = UnityEngine.Random.Range(0, indeksiKupovnaSkinova.Count);
        int random = indeksiKupovnaSkinova[randomBr];
        indeksiKupovnaSkinova.Remove(random);
        PlayerPrefs.SetInt("Skin" + random.ToString(), 1);
        Sprite skin = skinovi[random];
        bob.GetComponent<Image>().sprite = skin;

        yield return new WaitForSeconds(1.5f);

        Image bobImage = bob.GetComponent<Image>();

        DialogManager.instance.StartDialogue(random, bobImage);

        kupljeniSkinovi[random] = true;

        StartCoroutine(WaitForEndOfSkinDialog(bobTrenutni));
    }

    public IEnumerator WaitForEndOfSkinDialog(Sprite bobTrenutni)
    {
        while (DialogManager.instance.doingDialogue)
        {
            yield return new WaitForSeconds(0.1f);
        }

        skinPanel.GetComponent<Animator>().SetTrigger("Zatvaranje");

        yield return new WaitForSeconds(1.5f);

        bob.GetComponent<Image>().sprite = bobTrenutni;
        bob.GetComponent<Animator>().enabled = true;
        bob.GetComponent<Animator>().SetTrigger("Zatvaranje");
    }

    public void Napitci()
    {
        clickedStand = "n";

        if (PlayerPrefs.GetInt("Nikorax", 0) == 0)
        {
            DialogManager.instance.StartDialogue(nikorax);
            return;
        }

        if (PlayerPrefs.GetInt("Nikorax", 0) == 1)
        {
            DialogManager.instance.StartDialogue(potions);
        }

        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        SoundManager.Instance.PlayClickSound();
        StartCoroutine(napitci());
    }
    public IEnumerator napitci()
    {
        yield return new WaitForSeconds(0.5f);
        napitciPanel.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija1", true);
    }

    public void NapitciBack()
    {
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        SoundManager.Instance.PlayClickSound();
        StartCoroutine(napitciBack());
    }
    public IEnumerator napitciBack()
    {
        yield return new WaitForSeconds(0.5f);
        napitciPanel.SetActive(false);
        tranzicijaAnimator.SetBool("tranzicija1", true);

        if (PlayerPrefs.GetInt("Nikorax", 0) == 2 && PlayerPrefs.GetInt("Filipikus", 0) == 2 && PlayerPrefs.GetInt("Adrianida", 0) == 2 && PlayerPrefs.GetInt("Pizza", 0) == 0)
        {
            DialogManager.instance.StartDialogue(pizza);
        }
    }

    public void Totemi()
    {
        clickedStand = "t";

        if (PlayerPrefs.GetInt("Filipikus", 0) == 0)
        {
            DialogManager.instance.StartDialogue(filipikus);
            return;
        }

        if (PlayerPrefs.GetInt("Filipikus", 0) == 1)
        {
            DialogManager.instance.StartDialogue(totems);
        }

        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        SoundManager.Instance.PlayClickSound();
        StartCoroutine(totemi());
    }
    public IEnumerator totemi()
    {
        yield return new WaitForSeconds(0.5f);
        totemiPanel.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija1", true);
    }

    public void TotemiBack()
    {
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        SoundManager.Instance.PlayClickSound();
        StartCoroutine(totemiBack());
    }
    public IEnumerator totemiBack()
    {
        yield return new WaitForSeconds(0.5f);
        totemiPanel.SetActive(false);
        tranzicijaAnimator.SetBool("tranzicija1", true);

        if (PlayerPrefs.GetInt("Nikorax", 0) == 2 && PlayerPrefs.GetInt("Filipikus", 0) == 2 && PlayerPrefs.GetInt("Adrianida", 0) == 2 && PlayerPrefs.GetInt("Pizza", 0) == 0)
        {
            DialogManager.instance.StartDialogue(pizza);
        }
    }

    public void Skins()
    {
        clickedStand = "s";

        if (PlayerPrefs.GetInt("Adrianida", 0) == 0)
        {
            DialogManager.instance.StartDialogue(adrianida);
            return;
        }

        if (PlayerPrefs.GetInt("Adrianida", 0) == 1)
        {
            DialogManager.instance.StartDialogue(skin);
        }

        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        SoundManager.Instance.PlayClickSound();
        StartCoroutine(skins());
    }
    public IEnumerator skins()
    {
        yield return new WaitForSeconds(0.5f);
        skinPanel.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija1", true);
    }

    public void SkinsBack()
    {
        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        SoundManager.Instance.PlayClickSound();
        StartCoroutine(skinsBack());
    }
    public IEnumerator skinsBack()
    {
        yield return new WaitForSeconds(0.5f);
        skinPanel.SetActive(false);
        tranzicijaAnimator.SetBool("tranzicija1", true);

        if (PlayerPrefs.GetInt("Nikorax", 0) == 2 && PlayerPrefs.GetInt("Filipikus", 0) == 2 && PlayerPrefs.GetInt("Adrianida", 0) == 2 && PlayerPrefs.GetInt("Pizza", 0) == 0)
        {
            DialogManager.instance.StartDialogue(pizza);
        }
    }

    public void PicaSoba()
    {
        if (PlayerPrefs.GetInt("Nikorax", 0) != 2 || PlayerPrefs.GetInt("Filipikus", 0) != 2 || PlayerPrefs.GetInt("Adrianida", 0) != 2 || PlayerPrefs.GetInt("Pizza", 0) == 0)
        {
            return;
        }

        tranzicija.SetActive(true);
        tranzicijaAnimator.SetBool("tranzicija2", true);
        SoundManager.Instance.PlayClickSound();
        StartCoroutine(picaSoba());
    }
    public IEnumerator picaSoba()
    {
        DateTime sada = DateTime.Now;
        PlayerPrefs.SetString("DailyDatum", sada.ToString());
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(15);
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
        if (PlayerPrefs.GetInt("Nikorax", 0) != 2 || PlayerPrefs.GetInt("Filipikus", 0) != 2 || PlayerPrefs.GetInt("Adrianida", 0) != 2 || PlayerPrefs.GetInt("Pizza", 0) < 2)
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
        SceneManager.LoadScene(1);
    }
}
