
using System.Collections.Generic;
using UnityEngine;

//출처
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
    //gui는 키입력 등의 이벤트가 발동시 호출되는 함수
    //키보드만 해당됨
    private void OnGUI()
    {
        Event key_event = Event.current; //현재 발동되는 이벤트 함수

        if (key_event.isKey || key_event.isMouse)
        {
            key_setting.keys[(key_action)key] = key_event.keyCode;

            if (Input.GetKeyDown(KeyCode.Mouse0)) { key_setting.keys[(key_action)key] = KeyCode.Mouse0; }
            else if(Input.GetKeyDown(KeyCode.Mouse1)) { key_setting.keys[(key_action)key] = KeyCode.Mouse1; }
            else if(Input.GetKeyDown(KeyCode.Mouse2)) { key_setting.keys[(key_action)key] = KeyCode.Mouse2; }

            if (key != -1) { setting_mng.txt[key].text = key_setting.keys[(key_action)key].ToString(); }

            key = -1; //바꾼뒤 아무것도 없는것으로 초기화
        }
    }
    
    public int key = -1; //인트 변수에 위에 키인덱스를 저장

    //public void change_key(int num)
    //{
    //    key = num;
    //}
}
