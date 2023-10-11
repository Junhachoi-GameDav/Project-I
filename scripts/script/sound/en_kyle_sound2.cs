using UnityEngine;

public class en_kyle_sound2 : MonoBehaviour
{
    //kyle 보이스 당담
    [Header("clips")]
    public AudioClip hit_voice;
    public AudioClip hit_voice2;

    public AudioClip hit_break_voice;

    public AudioClip dead_voice;
    public AudioClip footstep_sound;


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
            case "hit_voice":
                audio_source.clip = hit_voice;
                break;
            case "hit_voice2":
                audio_source.clip = hit_voice2;
                break;
            case "hit_break_voice":
                audio_source.clip = hit_break_voice;
                break;
            case "dead_voice":
                audio_source.clip = dead_voice;
                break;
            case "footstep_sound":
                audio_source.clip = footstep_sound;
                break;
        }
        audio_source.Play();
    }
    public void kyle_sound2_play(string name)
    {
        play_sounds(name);
    }
}
