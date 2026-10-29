using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;

    public Image characterImage;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueText;

    public List<Sprite> bobSkinsNormalFace = new List<Sprite>();
    public List<Sprite> bobSkinsOpenMouthFace = new List<Sprite>();
    public List<Sprite> bobSkinsClosedEyesFace = new List<Sprite>();

    public List<Sprite> capricciosaSkinsNormalFace = new List<Sprite>();
    public List<Sprite> capricciosaSkinsOpenMouthFace = new List<Sprite>();

    public List<Dialog> skinDialogs = new List<Dialog>();

    private bool isTyping=false;
    private bool isDialogueActive=false;
    private bool isNormalFace = true;
    private bool isBlinking = false;
    private bool loweredMusic = false;

    [HideInInspector]
    public bool doingDialogue = false;

    private float typingSpeed = 0.05f;
    private float comaPause = 0.25f;
    private float periodPause = 0.5f;

    private string fullText="";
    private Dialog currentDialog;

    private int currentIndeks = -1;
    private Image currentImage = null;

    private Animator dialogAnimator;
    private AudioSource audioSource;

    private Sprite currentNormalFace;
    private Sprite currentOpenMouthFace;
    private Sprite currentClosedEyesFace;

    private List<Coroutine> coroutines = new List<Coroutine>();
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            dialogAnimator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartDialogue(int indeks,Image bobImage=null)
    {
        doingDialogue = true;
        currentIndeks = indeks;

        Dialog dialog;
        if (currentDialog == null)
        {
            dialog = skinDialogs[indeks];
        }
        else
        {
            dialog=currentDialog.nextDialog;
        }

        if (currentImage==null)
        {
            currentImage=bobImage;
            StartCoroutine(BobAnim());
        }

        if (dialog.character.name == "Bob")
        {
            currentNormalFace = bobSkinsNormalFace[indeks];
            currentOpenMouthFace = bobSkinsOpenMouthFace[indeks];
            currentClosedEyesFace = bobSkinsClosedEyesFace[indeks];
        }
        else if (dialog.character.name == "Kapricoza")
        {
            currentNormalFace = capricciosaSkinsNormalFace[indeks];
            currentOpenMouthFace = capricciosaSkinsOpenMouthFace[indeks];
            currentClosedEyesFace = capricciosaSkinsNormalFace[indeks];
        }

        characterImage.sprite = currentNormalFace;
        characterName.text = dialog.character.characterName;
        characterName.fontSharedMaterial = dialog.character.nameMaterial;
        fullText = dialog.text;
        audioSource.clip = dialog.character.sound;
        audioSource.volume = PlayerPrefs.GetInt("VoicesVolume", 100) / 100f;
        currentDialog = dialog;

        if (!dialog.onRightSide)
        {
            dialogAnimator.SetTrigger("Gore");
        }
        else
        {
            dialogAnimator.SetTrigger("GoreObrnuto");
        }

        if (loweredMusic == false)
        {
            Camera.main.GetComponent<AudioSource>().volume = Camera.main.GetComponent<AudioSource>().volume / 10;
            loweredMusic = true;
        }

        Coroutine cor = StartCoroutine(TypeText(true));
        coroutines.Add(cor);
    }

    private IEnumerator BobAnim()
    {
        while (doingDialogue)
        {
            if (currentIndeks == -1)
                yield break;
            currentImage.sprite = bobSkinsNormalFace[currentIndeks];
            yield return new WaitForSeconds(2.5f);
            if (doingDialogue == false)
                yield break;
            currentImage.sprite = bobSkinsOpenMouthFace[currentIndeks];
            yield return new WaitForSeconds(1f);
            if (doingDialogue == false)
                yield break;
            currentImage.sprite = bobSkinsNormalFace[currentIndeks];
            yield return new WaitForSeconds(0.5f);
            if (doingDialogue == false)
                yield break;
            currentImage.sprite = bobSkinsClosedEyesFace[currentIndeks];
            yield return new WaitForSeconds(0.15f);
        }
    }

    public void StartDialogue(Dialog dialog)
    {
        doingDialogue = true;

        if (dialog.character.name == "Bob" && PlayerPrefs.GetInt("TrenutniSkin", -1) != -1)
        {
            int skinIndex = PlayerPrefs.GetInt("TrenutniSkin");
            currentNormalFace = bobSkinsNormalFace[skinIndex];
            currentOpenMouthFace = bobSkinsOpenMouthFace[skinIndex];
            currentClosedEyesFace = bobSkinsClosedEyesFace[skinIndex];
        }
        else if (dialog.character.name == "Kapricoza" && PlayerPrefs.GetInt("TrenutniSkin", -1) != -1)
        {
            int skinIndex = PlayerPrefs.GetInt("TrenutniSkin");
            currentNormalFace = capricciosaSkinsNormalFace[skinIndex];
            currentOpenMouthFace = capricciosaSkinsOpenMouthFace[skinIndex];
            currentClosedEyesFace = capricciosaSkinsNormalFace[skinIndex];
        }
        else
        {
            currentNormalFace = dialog.character.normalFace;
            currentOpenMouthFace = dialog.character.openMouthFace;
            currentClosedEyesFace = dialog.character.closedEyesFace;
        }
        characterImage.sprite = currentNormalFace;
        characterName.text = dialog.character.characterName;
        characterName.fontSharedMaterial = dialog.character.nameMaterial;
        fullText = dialog.text;
        audioSource.clip = dialog.character.sound;
        audioSource.volume = PlayerPrefs.GetInt("VoicesVolume", 100) / 100f;
        currentDialog = dialog;

        if(!dialog.onRightSide)
        {
            dialogAnimator.SetTrigger("Gore");
        }
        else
        {
            dialogAnimator.SetTrigger("GoreObrnuto");
        }

        if (loweredMusic == false)
        {
            Camera.main.GetComponent<AudioSource>().volume = Camera.main.GetComponent<AudioSource>().volume / 10;
            loweredMusic = true;
        }

        Coroutine cor= StartCoroutine(TypeText(true));
        coroutines.Add(cor);
    }

    private IEnumerator TypeText(bool wait=false)
    {
        if (wait)
        {
            yield return new WaitForSeconds(1f);
        }

        isDialogueActive = true;
        isTyping = true;
        int i = 0;
        float blinkTime = 0;
        foreach (char letter in fullText)
        {
            dialogueText.text += letter;

            if (letter == ',')
            {
                blinkTime+=comaPause;

                if(isBlinking)
                {
                    float time= typingSpeed*(3-i);
                    isBlinking = false;
                    yield return new WaitForSeconds(time);
                    characterImage.sprite = currentNormalFace;
                    yield return new WaitForSeconds(comaPause-time);
                }
                else if(isNormalFace==false)
                {
                    float time= typingSpeed*(3-i);
                    isNormalFace = true;
                    yield return new WaitForSeconds(time);
                    characterImage.sprite = currentNormalFace;
                    yield return new WaitForSeconds(comaPause-time);
                }
                else
                {
                    yield return new WaitForSeconds(comaPause);
                }
            }
            else if (letter == '.' || letter == '!' || letter == '?')
            {
                blinkTime+=periodPause;

                if(isBlinking)
                {
                    isBlinking = false;
                    yield return new WaitForSeconds(typingSpeed);
                    characterImage.sprite = currentNormalFace;
                    yield return new WaitForSeconds(periodPause-typingSpeed);
                }
                else if(isNormalFace==false)
                {
                    isNormalFace = true;
                    yield return new WaitForSeconds(typingSpeed);
                    characterImage.sprite = currentNormalFace;
                    yield return new WaitForSeconds(periodPause-typingSpeed);
                }
                else
                {
                    yield return new WaitForSeconds(periodPause);
                }
            }
            blinkTime+=typingSpeed;
            yield return new WaitForSeconds(typingSpeed);

            i++;
            if(i==3)
            {
                i = 0;

                if (audioSource.isPlaying == false)
                {
                    audioSource.pitch = Random.Range(0.5f, 1.5f);
                    audioSource.Play();
                }

                if(isNormalFace)
                {
                    characterImage.sprite = currentOpenMouthFace;
                    isNormalFace = false;

                    if(isBlinking)
                    {
                        isBlinking = false;
                    }
                }
                else
                {
                    if(blinkTime>3)
                    {
                        characterImage.sprite = currentClosedEyesFace;
                        blinkTime=0;
                        isBlinking = true;
                    }
                    else
                    {
                        characterImage.sprite = currentNormalFace;
                    }
                    isNormalFace = true;
                }
            }
        }
        isTyping = false;
        audioSource.Stop();

        if(isNormalFace==false)
        {
            characterImage.sprite = currentNormalFace;
            isNormalFace = true;
        }
        else if(isBlinking)
        {
            characterImage.sprite = currentNormalFace;
            isBlinking = false;
        }

        Coroutine cor= StartCoroutine(WaitForNextDialog());
        coroutines.Add(cor);
    }

    private IEnumerator WaitForNextDialog()
    {
        while(true)
        {
            if(isBlinking)
            {
                yield return new WaitForSeconds(typingSpeed*3);
                isBlinking = false;
                characterImage.sprite = currentNormalFace;
            }
            else
            {
                yield return new WaitForSeconds(3);
                isBlinking = true;
                characterImage.sprite = currentClosedEyesFace;
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping && isDialogueActive)
            {
                SoundManager.Instance.PlayClickSound();

                foreach (Coroutine cor1 in coroutines)
                {
                    StopCoroutine(cor1);
                }
                coroutines.Clear();

                dialogueText.text = fullText;
                isTyping = false;

                if(isNormalFace==false)
                {
                    characterImage.sprite = currentNormalFace;
                    isNormalFace = true;
                }
                else if(isBlinking)
                {
                    characterImage.sprite = currentNormalFace;
                    isBlinking = false;
                }

                Coroutine cor = StartCoroutine(WaitForNextDialog());
                coroutines.Add(cor);
            }
            else if(isDialogueActive)
            {
                SoundManager.Instance.PlayClickSound();

                foreach (Coroutine cor in coroutines)
                {
                    StopCoroutine(cor);
                }
                coroutines.Clear();

                isDialogueActive = false;

                if(isBlinking)
                {
                    characterImage.sprite = currentNormalFace;
                    isBlinking = false;
                }

                if(currentDialog.nextDialog != null)
                {
                    if(currentDialog.character==currentDialog.nextDialog.character)
                    {
                        dialogueText.text = "";
                        currentDialog = currentDialog.nextDialog;
                        fullText = currentDialog.text;

                        Coroutine cor= StartCoroutine(TypeText());
                        coroutines.Add(cor);
                    }
                    else
                    {
                        if(currentDialog.onRightSide)
                        {
                            dialogAnimator.SetTrigger("GoreObrnuto");
                        }
                        else
                        {
                            dialogAnimator.SetTrigger("Gore");
                        }

                        Coroutine cor = StartCoroutine(ChangeCharacter());
                        coroutines.Add(cor);
                    }
                }
                else
                {
                    doingDialogue = false;
                    currentIndeks = -1;

                    if(currentDialog.onRightSide)
                    {
                        dialogAnimator.SetTrigger("GoreObrnuto");
                    }
                    else
                    {
                        dialogAnimator.SetTrigger("Gore");
                    }

                    if (loweredMusic)
                    {
                        Camera.main.GetComponent<AudioSource>().volume = Camera.main.GetComponent<AudioSource>().volume * 10;
                        loweredMusic = false;
                    }

                    if(currentDialog.hasEffect)
                    {
                        MakeEffect();
                    }

                    dialogueText.text = "";
                    currentDialog = null;
                    currentImage = null;
                }
            }
        }
    }

    private IEnumerator ChangeCharacter()
    {
        yield return new WaitForSeconds(0.5f);
        dialogueText.text = "";
        if (currentIndeks == -1)
        {
            StartDialogue(currentDialog.nextDialog);
        }
        else
        {
            StartDialogue(currentIndeks);
        }
    }

    public void MakeEffect()
    {
        if(PlayerPrefs.GetInt("SettingsClickable",0)==0)
        {
            PlayerPrefs.SetInt("SettingsClickable",1);
        }
        else if (PlayerPrefs.GetInt("GearCanRotate",0)==0)
        {
            PlayerPrefs.SetInt("GearCanRotate", 1);
        }
        else if(PlayerPrefs.GetInt("BluePortalClickable",0)==0)
        {
            PlayerPrefs.SetInt("BluePortalClickable",1);
        }
        else if(PlayerPrefs.GetInt("DialogueLevel1", 0) == 0)
        {
            PlayerPrefs.SetInt("DialogueLevel1", 1);
        }
        else if(PlayerPrefs.GetInt("DialogueLevel1", 0) == 1)
        {
            PlayerPrefs.SetInt("DialogueLevel1", 2);
        }
        else if(PlayerPrefs.GetInt("DialogueLevel1", 0) == 2)
        {
            PlayerPrefs.SetInt("DialogueLevel1", 3);
            PlayerPrefs.SetInt("CityClickable", 1);
        }
        else if(PlayerPrefs.GetInt("CityClickable",0)==1)
        {
            PlayerPrefs.SetInt("CityClickable", 2);
        }
        else if(PlayerPrefs.GetInt("FirstHouse",0)==0)
        {
            PlayerPrefs.SetInt("FirstHouse", 1);
        }
        else if(PlayerPrefs.GetInt("FirstHouse",0)==1)
        {
            PlayerPrefs.SetInt("FirstHouse", 2);
        }
        else if (PlayerPrefs.GetInt("CityClickable", 0) == 2)
        {
            PlayerPrefs.SetInt("CityClickable", 3);
            PlayerPrefs.SetInt("FirstHouse", 3);
        }
        else if(PlayerPrefs.GetInt("FirstShop",0)==0)
        {
            PlayerPrefs.SetInt("FirstShop", 1);
        }
        else if(PlayerPrefs.GetInt("Nikorax",0)<2 || PlayerPrefs.GetInt("Filipikus",0)<2 || PlayerPrefs.GetInt("Adrianida",0)<2)
        {
            if(Shop.clickedStand=="n")
            {
                PlayerPrefs.SetInt("Nikorax", PlayerPrefs.GetInt("Nikorax", 0) + 1);
            }
            else if(Shop.clickedStand=="t")
            {
                PlayerPrefs.SetInt("Filipikus", PlayerPrefs.GetInt("Filipikus", 0) + 1);
            }
            else if(Shop.clickedStand=="s")
            {
                PlayerPrefs.SetInt("Adrianida", PlayerPrefs.GetInt("Adrianida", 0) + 1);
            }
        }
        else if(PlayerPrefs.GetInt("Pizza",0)==0)
        {
            PlayerPrefs.SetInt("Pizza", 1);
        }
        else if(PlayerPrefs.GetInt("Pizza",0)==1)
        {
            PlayerPrefs.SetInt("Pizza", 2);
        }
        else if(PlayerPrefs.GetInt("Pizza",0)==2)
        {
            PlayerPrefs.SetInt("Pizza", 3);
        }
        else if(PlayerPrefs.GetInt("CompetitiveLevel1",0)==0)
        {
            PlayerPrefs.SetInt("CompetitiveLevel1", 1);
        }
    }
}
