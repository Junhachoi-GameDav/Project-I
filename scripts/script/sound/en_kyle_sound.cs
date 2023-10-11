using UnityEngine;

public class en_kyle_sound : MonoBehaviour
{
    //kyle ´ç´ã
    [Header("clips")]
    public AudioClip swing_weapon;
    public AudioClip swing_weapon2;
    public AudioClip hit_sound; 
    public AudioClip hit_sound2;
    public AudioClip hit_break;
    public AudioClip dead_sound;
    public AudioClip dead_sound2;

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
            case "swing_weapon":
                audio_source.clip = swing_weapon;
                break;
            case "swing_weapon2":
                audio_source.clip = swing_weapon2;
                break;
            case "hit_sound":
                audio_source.clip = hit_sound;
                break;
            case "hit_sound2":
                audio_source.clip = hit_sound2;
                break;
            case "hit_break":
                audio_source.clip = hit_break;
                break;
            case "dead_sound":
                audio_source.clip = dead_sound;
                break;
            case "dead_sound2":
                audio_source.clip = dead_sound2;
                break;
        }
        audio_source.Play();
    }
    public void kyle_sound1_play(string name)
    {
        play_sounds(name);
    }
}
