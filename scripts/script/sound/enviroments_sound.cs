using UnityEngine;

public class enviroments_sound : MonoBehaviour
{
    // 피격 같은 소리 담당
    [Header("clips")]
    public AudioClip open_door;

    [Header("audio")]
    public AudioSource audio_source;

    void Awake()
    {
        audio_source = GetComponent<AudioSource>();
    }

    public void play_sounds(string action)
    {
        switch (action)
        {
            case "open_door":
                audio_source.clip = open_door;
                break;
        }
        audio_source.Play();
    }
}
