using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class sound_manager : MonoBehaviour
{
    hit_ef_sound _hit_ef_sound;
    normal_ef_sound _normal_ef_sound;
    bg_sound _bg_sound;
    ui_sound _ui_sound;
    enviroments_sound _enviroments_sound;

    public bool is_audio_setting;

    public AudioMixer audioMixer;
    public Slider master_volume_slider;
    public Slider bgm_volume_slider;
    public Slider vfx_volume_slider;

    private void Awake()
    {
        master_volume_slider.onValueChanged.AddListener(set_master_volume);
        bgm_volume_slider.onValueChanged.AddListener(set_bgm_volume);
        vfx_volume_slider.onValueChanged.AddListener(set_vfx_volume);
        _hit_ef_sound = GetComponentInChildren<hit_ef_sound>();
        _normal_ef_sound = GetComponentInChildren<normal_ef_sound>();
        _bg_sound = GetComponentInChildren<bg_sound>();
        _ui_sound = GetComponentInChildren<ui_sound>();
        _enviroments_sound = GetComponentInChildren<enviroments_sound>();
    }


    public void sm_hit_ef_sound_play(string hit_ef_name)
    {
        _hit_ef_sound.play_sounds(hit_ef_name);
    }
    public void sm_normal_ef_sound_play(string normal_ef_name)
    {
        _normal_ef_sound.play_sounds(normal_ef_name);
    }
    public void sm_bg_sound_play(string bg_name)
    {
        _bg_sound.play_sounds(bg_name);
    }
    public void sm_ui_sound_play(string bg_name)
    {
        _ui_sound.play_sounds(bg_name);
    }
    public void sm_enviroment_sound_play(string bg_name)
    {
        _enviroments_sound.play_sounds(bg_name);
    }

    //슬라이더
    // min Value 에 0으로 두면 안되고 0.0001로 두어야함
    public void set_master_volume(float value)
    {
        // 오디오 믹스 안에 넣을 값이 이런가 봄.
        // -80 ~ 20까지 존재  -40이하는 안들리고  0이상은 깨지는 소리들린다고함
        audioMixer.SetFloat("Master", Mathf.Log10(value) * 20);
    }
    public void set_bgm_volume(float value)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(value) * 20);
    }
    public void set_vfx_volume(float value)
    {
        audioMixer.SetFloat("VFX", Mathf.Log10(value) * 20);
    }

    public void default_btn_click()
    {
        master_volume_slider.value = 0.5f;
        bgm_volume_slider.value = 0.5f;
        vfx_volume_slider.value = 0.5f;
    }
}
