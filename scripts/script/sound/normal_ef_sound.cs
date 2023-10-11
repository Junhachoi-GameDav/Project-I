using UnityEngine;

public class normal_ef_sound : MonoBehaviour
{
    // 효과음 담당
    [Header("clips")]
    
    public AudioClip en_grab;
    public AudioClip break_sound;
    public AudioClip parry_impact;
    public AudioClip parry_impact2;
    public AudioClip heal_impact;
    public AudioClip go_game_impact;
    public AudioClip game_over_impact;

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
            case "en_grab":
                audio_source.clip = en_grab;
                break;
            case "break_sound":
                audio_source.clip = break_sound;
                break;
            case "parry_impact":
                audio_source.clip = parry_impact;
                break;
            case "parry_impact2":
                audio_source.clip = parry_impact2;
                break;
            case "heal_impact":
                audio_source.clip = heal_impact;
                break;
            case "go_game_impact":
                audio_source.clip = go_game_impact;
                break;
            case "game_over_impact":
                audio_source.clip = game_over_impact;
                break;
        }
        audio_source.Play();
    }
}
