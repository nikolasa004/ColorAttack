using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class NamestanjePodesavanja : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private GraphicRaycaster raycaster;
    [SerializeField] private EventSystem eventSystem;

    [SerializeField] private GameObject voicesScale;
    [SerializeField] private GameObject musicScale;
    [SerializeField] private GameObject uiScale;

    [SerializeField] private TextMeshProUGUI voicesText;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI uiText;

    [SerializeField] private List<AudioSource> uiAudioSources;

    private Vector2 voicesPosition = new Vector2(0, 1000);
    private Vector2 musicPosition = new Vector2(-500, 500);
    private Vector2 uiPosition = new Vector2(-1000, 0);

    private Coroutine positionCoroutine;
    private bool isTouchingGear = false;

    private string currentScale = "m";
    private List<float> uiOriginalVolumes = new List<float>();

    private void Start()
    {
        foreach (AudioSource audioSource in uiAudioSources)
        {
            uiOriginalVolumes.Add(audioSource.volume);
        }
        for (int i = 0; i < uiAudioSources.Count; i++)
        {
            uiAudioSources[i].volume = uiOriginalVolumes[i] * PlayerPrefs.GetInt("UIVolume", 100) / 100f;
        }

        voicesText.text = PlayerPrefs.GetInt("VoicesVolume", 100) + "%";
        musicText.text = PlayerPrefs.GetInt("MusicVolume", 100) + "%";
        uiText.text = PlayerPrefs.GetInt("UIVolume", 100) + "%";

        voicesScale.GetComponent<RectTransform>().sizeDelta = new Vector2(voicesScale.GetComponent<RectTransform>().sizeDelta.x, PlayerPrefs.GetInt("VoicesVolume", 100) * 225 / 100);
        musicScale.GetComponent<RectTransform>().sizeDelta = new Vector2(musicScale.GetComponent<RectTransform>().sizeDelta.x, PlayerPrefs.GetInt("MusicVolume", 100) * 225 / 100);
        uiScale.GetComponent<RectTransform>().sizeDelta = new Vector2(uiScale.GetComponent<RectTransform>().sizeDelta.x, PlayerPrefs.GetInt("UIVolume", 100) * 225 / 100);
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (IsTouchingThisUIElement(touch))
                    {
                        if (positionCoroutine != null)
                        {
                            StopCoroutine(positionCoroutine);
                        }

                        isTouchingGear = true;
                    }
                    break;


                case TouchPhase.Ended:

                    if (isTouchingGear)
                    {
                        positionCoroutine = StartCoroutine(PositionSettings());
                        isTouchingGear = false;
                    }

                    break;
            }
        }
    }

    bool IsTouchingThisUIElement(Touch touch)
    {
        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = touch.position
        };

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        foreach (var result in results)
        {
            if (result.gameObject == gameObject)
                return true;
        }

        return false;
    }

    private IEnumerator PositionSettings()
    {
        yield return new WaitForSeconds(0.5f);

        gameObject.GetComponent<ScrollRect>().velocity = Vector2.zero;

        Vector2 currentPosition = new Vector2(content.GetComponent<RectTransform>().offsetMin.x, content.GetComponent<RectTransform>().offsetMax.x);

        float offsetYMin = content.GetComponent<RectTransform>().offsetMin.y;
        float offsetYMax = content.GetComponent<RectTransform>().offsetMax.y;
        if (currentPosition.y > 750)
        {
            content.GetComponent<RectTransform>().offsetMin = new Vector2(voicesPosition.x, offsetYMin);
            content.GetComponent<RectTransform>().offsetMax = new Vector2(voicesPosition.y, offsetYMax);

            currentScale = "v";
        }
        else if (currentPosition.x < -750)
        {
            content.GetComponent<RectTransform>().offsetMin = new Vector2(uiPosition.x, offsetYMin);
            content.GetComponent<RectTransform>().offsetMax = new Vector2(uiPosition.y, offsetYMax);

            currentScale = "u";
        }
        else
        {
            content.GetComponent<RectTransform>().offsetMin = new Vector2(musicPosition.x, offsetYMin);
            content.GetComponent<RectTransform>().offsetMax = new Vector2(musicPosition.y, offsetYMax);

            currentScale = "m";
        }
    }

    public void VolumeUp()
    {
        if (currentScale == "v")
        {
            if (PlayerPrefs.GetInt("VoicesVolume", 100) < 100)
            {
                PlayerPrefs.SetInt("VoicesVolume", PlayerPrefs.GetInt("VoicesVolume", 100) + 1);
                voicesText.text = PlayerPrefs.GetInt("VoicesVolume", 100).ToString() + "%";

                voicesScale.GetComponent<RectTransform>().sizeDelta = new Vector2(voicesScale.GetComponent<RectTransform>().sizeDelta.x, PlayerPrefs.GetInt("VoicesVolume", 100) * 225 / 100);
            }
        }
        else if (currentScale == "m")
        {
            if (PlayerPrefs.GetInt("MusicVolume", 100) < 100)
            {
                PlayerPrefs.SetInt("MusicVolume", PlayerPrefs.GetInt("MusicVolume", 100) + 1);
                musicText.text = PlayerPrefs.GetInt("MusicVolume", 100).ToString() + "%";

                musicScale.GetComponent<RectTransform>().sizeDelta = new Vector2(musicScale.GetComponent<RectTransform>().sizeDelta.x, PlayerPrefs.GetInt("MusicVolume", 100) * 225 / 100);

                Camera.main.GetComponent<AudioSource>().volume += 0.01f;
            }
        }
        else if (currentScale == "u")
        {
            if (PlayerPrefs.GetInt("UIVolume", 100) < 100)
            {
                PlayerPrefs.SetInt("UIVolume", PlayerPrefs.GetInt("UIVolume", 100) + 1);
                uiText.text = PlayerPrefs.GetInt("UIVolume", 100).ToString() + "%";

                uiScale.GetComponent<RectTransform>().sizeDelta = new Vector2(uiScale.GetComponent<RectTransform>().sizeDelta.x, PlayerPrefs.GetInt("UIVolume", 100) * 225 / 100);

                for (int i = 0; i < uiAudioSources.Count; i++)
                {
                    uiAudioSources[i].volume = uiOriginalVolumes[i] * PlayerPrefs.GetInt("UIVolume", 100) / 100f;
                }
            }
        }
    }

    public void VolumeDown()
    {
        if (currentScale == "v")
        {
            if (PlayerPrefs.GetInt("VoicesVolume", 100) > 0)
            {
                PlayerPrefs.SetInt("VoicesVolume", PlayerPrefs.GetInt("VoicesVolume", 100) - 1);
                voicesText.text = PlayerPrefs.GetInt("VoicesVolume", 100).ToString() + "%";

                voicesScale.GetComponent<RectTransform>().sizeDelta = new Vector2(voicesScale.GetComponent<RectTransform>().sizeDelta.x, PlayerPrefs.GetInt("VoicesVolume", 100) * 225 / 100);
            }
        }
        else if (currentScale == "m")
        {
            if (PlayerPrefs.GetInt("MusicVolume", 100) > 0)
            {
                PlayerPrefs.SetInt("MusicVolume", PlayerPrefs.GetInt("MusicVolume", 100) - 1);
                musicText.text = PlayerPrefs.GetInt("MusicVolume", 100).ToString() + "%";

                musicScale.GetComponent<RectTransform>().sizeDelta = new Vector2(musicScale.GetComponent<RectTransform>().sizeDelta.x, PlayerPrefs.GetInt("MusicVolume", 100) * 225 / 100);

                Camera.main.GetComponent<AudioSource>().volume -= 0.01f;
            }
        }
        else if (currentScale == "u")
        {
            if (PlayerPrefs.GetInt("UIVolume", 100) > 0)
            {
                PlayerPrefs.SetInt("UIVolume", PlayerPrefs.GetInt("UIVolume", 100) - 1);
                uiText.text = PlayerPrefs.GetInt("UIVolume", 100).ToString() + "%";

                uiScale.GetComponent<RectTransform>().sizeDelta = new Vector2(uiScale.GetComponent<RectTransform>().sizeDelta.x, PlayerPrefs.GetInt("UIVolume", 100) * 225 / 100);

                for (int i = 0; i < uiAudioSources.Count; i++)
                {
                    uiAudioSources[i].volume = uiOriginalVolumes[i] * PlayerPrefs.GetInt("UIVolume", 100) / 100f;
                }
            }
        }
    }
}
