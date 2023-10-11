using UnityEngine;

public class player_atk_sound : MonoBehaviour
{
    //플레이어 액션 당담
    [Header("clips")]
    public AudioClip swing_katana;
    public AudioClip swing_katana2;
    public AudioClip block_katana;
    public AudioClip block2_katana;
    public AudioClip parry_katana;
    public AudioClip parry2_katana;
    public AudioClip p_eatting_pill;
    public AudioClip p_rolling2;

    [Header("audio")]
    public AudioSource audio_source;

    void Awake()
    {
        audio_source =GetComponent<AudioSource>();
    }

    public void play_sounds(string action)
    {
        switch (action)
        {
            case "swing_katana":
                audio_source.clip = swing_katana;
                break;
            case "swing_katana2":
                audio_source.clip = swing_katana2;
                break;
            case "block_katana":
                audio_source.clip = block_katana;
                break;
            case "block2_katana":
                audio_source.clip = block2_katana;
                break;
            case "parry_katana":
                audio_source.clip = parry_katana;
                break;
            case "parry2_katana":
                audio_source.clip = parry2_katana;
                break;
            case "p_eatting_pill":
                audio_source.clip = p_eatting_pill;
                break;
            case "p_rolling2":
                audio_source.clip = p_rolling2;
                break;
        }
        audio_source.Play();
    }
    public void player_atk_sound_play(string pa_s_name)
    {
        play_sounds(pa_s_name);
    }
}
