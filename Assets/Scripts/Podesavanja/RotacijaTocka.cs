using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class RotacijaTocka : MonoBehaviour
{
    public GameObject holder;
    public NamestanjePodesavanja settingsManager;

    private float rotationAmount = 0f;
    public float rotationThreshold = 30f;

    private Vector2 centerScreenPos;
    private float previousAngle;
    private bool isTouchingGear = false;

    [SerializeField] private GraphicRaycaster raycaster;
    [SerializeField] private EventSystem eventSystem;

    private AudioSource audioSource;

    void Start()
    {
        centerScreenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetInt("UIVolume", 100)*audioSource.volume/100;
    }

    void Update()
    {
        if (Input.touchCount > 0 && PlayerPrefs.GetInt("GearCanRotate", 0) == 1)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = touch.position;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (IsTouchingThisUIElement(touch))
                    {
                        isTouchingGear = true;
                        Vector2 dirStart = touchPos - centerScreenPos;
                        previousAngle = Mathf.Atan2(dirStart.y, dirStart.x) * Mathf.Rad2Deg;
                        rotationAmount = 0f;
                    }
                    else
                    {
                        isTouchingGear = false;
                    }
                    break;

                case TouchPhase.Moved:
                    if (!isTouchingGear) return;

                    Vector2 dir = touchPos - centerScreenPos;
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    float deltaAngle = Mathf.DeltaAngle(previousAngle, angle);

                    transform.Rotate(0, 0, deltaAngle / 2);
                    rotationAmount += deltaAngle;

                    if (Mathf.Abs(rotationAmount) > rotationThreshold)
                    {
                        if (rotationAmount < 0)
                            OnRotateRight();
                        else
                            OnRotateLeft();

                        rotationAmount = 0f;

                        audioSource.Play();
                    }

                    previousAngle = angle;
                    break;

                case TouchPhase.Ended:
                    isTouchingGear = false;
                    rotationAmount = 0f;
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
            if (result.gameObject == holder)
                return true;
        }

        return false;
    }

    void OnRotateRight()
    {
        settingsManager.VolumeUp();
    }

    void OnRotateLeft()
    {
        settingsManager.VolumeDown();
    }
}
