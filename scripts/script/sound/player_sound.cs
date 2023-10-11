using UnityEngine;

public class player_sound : MonoBehaviour
{
    //플레이어 소리 담당
    [Header("clips")]
    public AudioClip p_rolling;
    public AudioClip p_jump;
    public AudioClip p_landing;
    public AudioClip p_hitted;
    public AudioClip p_hitted2;
    public AudioClip p_hitted3;
    public AudioClip p_grab_downed;
    public AudioClip p_footstep;

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
            case "p_rolling":
                audio_source.clip = p_rolling;
                break;
            case "p_jump":
                audio_source.clip = p_jump;
                break;
            case "p_landing":
                audio_source.clip = p_landing;
                break;
            case "p_hitted":
                audio_source.clip = p_hitted;
                break;
            case "p_hitted2":
                audio_source.clip = p_hitted2;
                break;
            case "p_hitted3":
                audio_source.clip = p_hitted3;
                break;
            case "p_grab_downed":
                audio_source.clip = p_grab_downed;
                break;
            case "p_footstep":
                audio_source.clip = p_footstep;
                break;
        }
        audio_source.Play();
    }
    public void player_sound_play(string pa_s_name)
    {
        play_sounds(pa_s_name);
    }
}
