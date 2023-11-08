using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class setting_manager : MonoBehaviour
{
    List<Resolution> resolutions = new List<Resolution>();
    FullScreenMode screenMode;
    public Dropdown resolution_dropdown;
    public Dropdown screenmode_dropdown;
    public Dropdown language_dropdown;

    public Slider mouse_sensitivity_slider;

    public GameObject setting_obj;
    public bool is_setting;

    public int resolution_num;
    public int screenmode_num;
    public int language_num;

    public Text[] txt;
    key_manager key_mng;

    bool isChanging;

    void Awake()
    {
        key_mng = FindObjectOfType<key_manager>();
    }
    private void Start()
    {
        resoultion_ui();
        Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow);
        key_text();
        StartCoroutine(ChangeRoutine(0));
    }
    public void is_setting_on_off(bool value)
    {
        is_setting = value;
    }

    void resoultion_ui()
    {
        for(int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRate == 60)
            {
                if( (Screen.resolutions[i].width == 1920 && Screen.resolutions[i].height == 1080) ||
                    (Screen.resolutions[i].width == 1600 && Screen.resolutions[i].height == 900) ||
                    (Screen.resolutions[i].width == 1366 && Screen.resolutions[i].height == 768) ||
                    (Screen.resolutions[i].width == 1280 && Screen.resolutions[i].height == 720) ||
                    (Screen.resolutions[i].width == 960 && Screen.resolutions[i].height == 540) ||
                    (Screen.resolutions[i].width == 640 && Screen.resolutions[i].height == 360))
                {
                    resolutions.Add(Screen.resolutions[i]);
                }
            }
        }
        resolution_dropdown.options.Clear(); //안에 기본으로 있던것 다 제거

        int option_num = 0;

        foreach(Resolution item in resolutions)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = item.width + " X " + item.height + "  " + item.refreshRate + "hz";
            resolution_dropdown.options.Add(option);

            if(item.width == Screen.width && item.height == Screen.height)
            {
                resolution_dropdown.value = option_num;
            }
            option_num++;
        }
        resolution_dropdown.RefreshShownValue(); //새로고침
    }

    public void dropbox_option_change(int num)
    {
        resolution_num = num;
    }

    public void apply_btn_click()
    {
        screenmode_num = screenmode_dropdown.value;
        language_num = language_dropdown.value;
        switch (screenmode_num)
        {
            case 0:
                screenMode = FullScreenMode.FullScreenWindow;
                break;
            case 1:
                screenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 2:
                screenMode = FullScreenMode.Windowed;
                break;
        }

        Screen.SetResolution(resolutions[resolution_num].width, resolutions[resolution_num].height, screenMode);
        
        if (isChanging) { return; }
        StartCoroutine(ChangeRoutine(language_num));
    }

    public void default_btn_click()
    {
        resolution_dropdown.value = Screen.resolutions.Length;
        screenmode_dropdown.value = 0;
        screenMode = FullScreenMode.FullScreenWindow;
        resolution_num = resolution_dropdown.value;
        Screen.SetResolution(resolutions[resolution_num].width, resolutions[resolution_num].height, screenMode);
    }

    public void change_key(int num)
    {
        key_mng.key = num;
        //txt[num].text = key_setting.keys[(key_action)num].ToString();
    }

    public void key_text()
    {
        for (int i = 0; i < txt.Length; i++)
        {
            txt[i].text = key_setting.keys[(key_action)i].ToString();
        }
    }

    public void default_btn_key()
    {
        for(int i = 0; i < (int)key_action.key_count; i++)
        {
            key_setting.keys[(key_action)i] = key_mng.default_keys[i];
        }
        key_text();
        mouse_sensitivity_slider.value = 0.3f;
    }

    public void Change_language(int index)
    {
        if (isChanging) { return; }
        StartCoroutine(ChangeRoutine(index));
        language_num = index;
        language_dropdown.value = language_num;
    }
    IEnumerator ChangeRoutine(int index)
    {
        isChanging = true;

        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];

        isChanging = false;
    }
}
