using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

//[System.Serializable]
//public class _dialogue
//{
//    //[TextArea]//�ν�����â�� ���� �̻� �����ְ� �������
//    public string dialogue;  //��
//    public bool is_question;
//    //public Text dialoge_text;
//}

public class talk_manager : MonoBehaviour
{
    [Header("object")]
    public GameObject interection_icon;
    public Image interection_icon_img;
    public Text dialog_talk;
    public GameObject t_panel;
    public GameObject question_panel;
    public GameObject talk_icon_obj;

    [Header("info")]
    public float dialog_speed;
    public int count = 0; //���° �������
    public int upgrade_index; //���° ��翡�� ��ȭ ����
    public bool is_talk;
    public bool is_typing;
    public bool is_question1;
    bool text_finish;

    //[Header("dialogues")]
    //public _dialogue[] en_guider_dialogues;

    mouse_cursor_manager mouse_cursor_mng;
    ingame_manager ingame_mng;
    en_guider _en_guider;


    private void Awake()
    {
        mouse_cursor_mng = FindObjectOfType<mouse_cursor_manager>();
        ingame_mng = FindObjectOfType<ingame_manager>();
        _en_guider = FindObjectOfType<en_guider>();
    }

    private void Update()
    {
        if (ingame_mng.upgrade_mng.is_upgrading) { talk_reset(); return; }
        check();
        is_talking_start();
    }
    void check()
    {
        if (_en_guider.is_ready_talk && !is_talk)
        {
            interection_icon.SetActive(true);
            interection_icon_img.rectTransform.position = Camera.main.WorldToScreenPoint(_en_guider.talk_icon_pos.position);
        }
        else { interection_icon.SetActive(false); }

    }

    void is_talking_start()
    {
        if (_en_guider.is_ready_talk)
        {
            if (!ingame_mng.is_pause) { t_panel.SetActive(is_talk); }
            if (is_question1)
            {
                question_panel.SetActive(true);
                if ((Input.GetKeyDown(key_setting.keys[key_action.ATTACK]) || Input.GetKeyDown(key_setting.keys[key_action.INTERECTION])))
                {
                    text_finish = true;
                    Locale cur_locale = LocalizationSettings.SelectedLocale;
                    dialog_talk.text = LocalizationSettings.StringDatabase.GetLocalizedString("TALK", "I-ROBOTO" + (count - 1).ToString(), cur_locale);
                    is_typing = false;
                }
            }
            else if (is_talk)
            {
                mouse_cursor_mng.mouse_cursor_on_off(is_talk);

                if (!is_typing && (Input.GetKeyDown(key_setting.keys[key_action.ATTACK]) || Input.GetKeyDown(key_setting.keys[key_action.INTERECTION])))
                {
                    if (count < LocalizationSettings.StringDatabase.GetTable("TALK").Count) 
                    { 
                        text_finish = false; 
                        next_dialogue();
                    }
                    else
                    {
                        text_finish = false; 
                        is_talk = false;
                    }
                }
                else if (is_typing && (Input.GetKeyDown(key_setting.keys[key_action.ATTACK]) || Input.GetKeyDown(key_setting.keys[key_action.INTERECTION])))
                {
                    text_finish = true;
                    Locale cur_locale = LocalizationSettings.SelectedLocale;
                    dialog_talk.text = LocalizationSettings.StringDatabase.GetLocalizedString("TALK", "I-ROBOTO" + (count - 1).ToString(), cur_locale);
                    talk_icon_obj.SetActive(true);
                    is_typing = false;
                }
                else if (!is_typing)
                {
                    talk_icon_obj.SetActive(true);
                }
            }
            else
            {
                dialog_talk.text = "...";
                t_panel.SetActive(false);
                question_panel.SetActive(false);
                count = 0;
            }
        }
        else
        {
            talk_reset();
        }
    }
    public void talk_reset()
    {
        dialog_talk.text = "...";
        t_panel.SetActive(false);
        count = 0;
        is_talk = false;
        is_typing = false;
        is_question1 = false;
        question_panel.SetActive(false);
    }
    public void is_t_start_botton()
    {
        is_talk = true;
    }
    public void is_question_yes_no(bool value)
    {
        is_question1 = value;
    }
    public void click_no()
    {
        dialog_talk.text = "??";
    }
    public void next_dialogue()
    {
        Locale cur_locale = LocalizationSettings.SelectedLocale;
        dialog_talk.text = "";
        talk_icon_obj.SetActive(false);
        if (count == upgrade_index) { is_question1 = true; } //��ȭ���� ����
        else { is_question1 = false; }
        
        StartCoroutine(typing(LocalizationSettings.StringDatabase.GetLocalizedString("TALK", "I-ROBOTO" + count.ToString(), cur_locale)));
        count++;//����
    }
    
    IEnumerator typing(string text)
    {
        is_typing = true;
        foreach (char letter in text.ToCharArray())//���ڿ��� �� ���ھ� �ɰ��� �̸� charŸ���� �迭�� ����־���
        {
            if (text_finish)
            {
                break;
            }
            //is_typing = true;

            dialog_talk.text += letter;
            yield return _yield_cache.WaitForSeconds(dialog_speed);
            
        }
        is_typing = false;
    }
}
