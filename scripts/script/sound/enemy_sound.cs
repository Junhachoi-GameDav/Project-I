using UnityEngine;

public class enemy_sound : MonoBehaviour
{
    //보스 담당
    [Header("clips")]
    public AudioClip swing_whip;
    public AudioClip swing_whip2;
    public AudioClip swing_whip3;
    public AudioClip en_kick;
    public AudioClip en_footstep;
    public AudioClip en_footstep2;

    public AudioClip grab_down;

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
            case "swing_whip":
                audio_source.clip = swing_whip;
                break;
            case "swing_whip2":
                audio_source.clip = swing_whip2;
                break;
            case "swing_whip3":
                audio_source.clip = swing_whip3;
                break;
            case "en_kick":
                audio_source.clip = en_kick;
                break;
            case "grab_down":
                audio_source.clip = grab_down;
                break;
            case "en_footstep":
                audio_source.clip = en_footstep;
                break;
            case "en_footstep2":
                audio_source.clip = en_footstep2;
                break;
        }
        audio_source.Play();
    }
    public void enemy_sound_play(string e_name)
    {
        play_sounds(e_name);
    }
}
