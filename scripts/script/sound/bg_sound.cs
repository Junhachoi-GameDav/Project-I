
using UnityEngine;

public class bg_sound : MonoBehaviour
{
    //bgm´ã´ç
    [Header("clips")]
    public AudioClip normal_bgm;
    public AudioClip boss_in_battle;

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
            case "normal_bgm":
                audio_source.clip = normal_bgm;
                break;
            case "boss_in_battle":
                audio_source.clip = boss_in_battle;
                break;
        }
        audio_source.Play();
    }
}
