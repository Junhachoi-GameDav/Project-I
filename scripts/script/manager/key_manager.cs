
using System.Collections.Generic;
using UnityEngine;

//��ó
//https://www.youtube.com/watch?v=ODB3IOFqmrE

public enum key_action
{
    UP,
    DOWN,
    RIGHT,
    LEFT,
    ATTACK,
    BLOCK,
    TARGETTING,
    ROLLING,
    RUN,
    INTERECTION,
    HEAL,
    key_count
}


public static class key_setting { public static Dictionary<key_action,KeyCode> keys = new Dictionary<key_action, KeyCode>();}



public class key_manager : MonoBehaviour
{
    setting_manager setting_mng;

    public KeyCode[] default_keys = new KeyCode[]
    {
        KeyCode.W,
        KeyCode.S,
        KeyCode.D,
        KeyCode.A,
        KeyCode.Mouse0,
        KeyCode.Mouse1,
        KeyCode.Mouse2,
        KeyCode.Space,
        KeyCode.LeftShift,
        KeyCode.E,
        KeyCode.R
    };
    
    private void Awake()
    {
        key_setting.keys.Clear();
        for (int i = 0; i < (int)key_action.key_count; i++)
        {
            key_setting.keys.Add((key_action)i, default_keys[i]);
        }
        
    }
    private void Start()
    {
        setting_mng = FindObjectOfType<setting_manager>();
    }
    //gui�� Ű�Է� ���� �̺�Ʈ�� �ߵ��� ȣ��Ǵ� �Լ�
    //Ű���常 �ش��
    private void OnGUI()
    {
        Event key_event = Event.current; //���� �ߵ��Ǵ� �̺�Ʈ �Լ�

        if (key_event.isKey || key_event.isMouse)
        {
            key_setting.keys[(key_action)key] = key_event.keyCode;

            if (Input.GetKeyDown(KeyCode.Mouse0)) { key_setting.keys[(key_action)key] = KeyCode.Mouse0; }
            else if(Input.GetKeyDown(KeyCode.Mouse1)) { key_setting.keys[(key_action)key] = KeyCode.Mouse1; }
            else if(Input.GetKeyDown(KeyCode.Mouse2)) { key_setting.keys[(key_action)key] = KeyCode.Mouse2; }

            if (key != -1) { setting_mng.txt[key].text = key_setting.keys[(key_action)key].ToString(); }

            key = -1; //�ٲ۵� �ƹ��͵� ���°����� �ʱ�ȭ
        }
    }
    
    public int key = -1; //��Ʈ ������ ���� Ű�ε����� ����

    //public void change_key(int num)
    //{
    //    key = num;
    //}
}
