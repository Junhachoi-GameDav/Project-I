
using UnityEngine;

public class hit_ef_sound : MonoBehaviour
{
    // 피격 같은 소리 담당
    [Header("clips")]
    public AudioClip hit_katana;
    public AudioClip hit2_katana;
    public AudioClip hit3_katana;
    public AudioClip hit_whip;
    public AudioClip hit2_whip;
    public AudioClip hit_axe;
    public AudioClip hit2_axe;
    public AudioClip hit_kick;

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
            case "hit_katana":
                audio_source.clip = hit_katana;
                break;
            case "hit2_katana":
                audio_source.clip = hit2_katana;
                break;
            case "hit3_katana":
                audio_source.clip = hit3_katana;
                break;
            case "hit_whip":
                audio_source.clip = hit_whip;
                break;
            case "hit2_whip":
                audio_source.clip = hit2_whip;
                break;
            case "hit_axe":
                audio_source.clip = hit_axe;
                break;
            case "hit2_axe":
                audio_source.clip = hit2_axe;
                break;
            case "hit_kick":
                audio_source.clip = hit_kick;
                break;
        }
        audio_source.Play();
    }
}
