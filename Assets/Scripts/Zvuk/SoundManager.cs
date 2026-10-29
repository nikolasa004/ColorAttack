using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private AudioSource[] audioSources;
    private float[] originalVolumes;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSources = GetComponents<AudioSource>();
            originalVolumes = new float[audioSources.Length];
            for (int i = 0; i < audioSources.Length; i++)
            {
                originalVolumes[i] = audioSources[i].volume;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayClickSound()
    {
        audioSources[0].volume = PlayerPrefs.GetInt("UIVolume", 100) * originalVolumes[0] / 100f;
        audioSources[0].Play();
    }

    public void PlayCoinSound()
    {
        audioSources[1].volume = PlayerPrefs.GetInt("UIVolume", 100) * originalVolumes[1] / 100f;
        audioSources[1].Play();
    }
}
