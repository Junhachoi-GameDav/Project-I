using UnityEngine;

public class ui_sound : MonoBehaviour
{
    [Header("clips")]
    public AudioClip button1;
    public AudioClip button2_click;
    public AudioClip button3_click;
    public AudioClip button4_close;
    public AudioClip button5_desicion;
    public AudioClip get_pill;
    public AudioClip tutorial_window;
    public AudioClip tank_found;
    public AudioClip beat_clone;

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
            case "button1":
                audio_source.clip = button1;
                break;
            case "button2_click":
                audio_source.clip = button2_click;
                break;
            case "button3_click":
                audio_source.clip = button3_click;
                break;
            case "button4_close":
                audio_source.clip = button4_close;
                break;
            case "button5_desicion":
                audio_source.clip = button5_desicion;
                break;
            case "get_pill":
                audio_source.clip = get_pill;
                break;
            case "tutorial_window":
                audio_source.clip = tutorial_window;
                break;
            case "tank_found":
                audio_source.clip = tank_found;
                break;
            case "beat_clone":
                audio_source.clip = beat_clone;
                break;
        }
        audio_source.Play();
    }

}
