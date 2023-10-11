using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class upgrade_manager : MonoBehaviour
{
    public bool is_upgrading;

    ingame_manager ingame_mng;
    weapon _weapon;

    [Header("data_chip")]
    public int needed_datachip;
    public float hp_p;
    public float stemina_p;
    public float attak_p;
    public int heal_p;
    public int needed_datachip_increase;

    [Header("text")]
    public Text[] data_texts;
    public Text[] cur_states_texts;
    public Text[] cur_states_inup_texts;
    public Text[] next_states_inup_texts;

    [Header("btn")]
    public Button[] buttons;


    float hp_num;
    float stemina_num;
    float atk_p_num;
    int heal_p_num;
    int data_chip_num;
    int needed_data_chip_num;
    int minus_data_chip_num;

    private void Awake()
    {
        ingame_mng = GetComponentInParent<ingame_manager>();
        _weapon = ingame_mng.player_mng.GetComponentInChildren<weapon>();
    }
    void Start()
    {
        states_uploading();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && is_upgrading)
        {
            ingame_mng.obj_upgrade_ui.SetActive(false);
            StartCoroutine(co_timer());
        }
        
    }
    
    IEnumerator co_timer()
    {
        yield return null;
        ingame_mng.pause_off();
    }

    public void states_uploading()
    {
        //초기화
        hp_num = ingame_mng.player_mng.player_max_hp;
        stemina_num =ingame_mng.player_mng.player_max_stemina;
        atk_p_num = _weapon.damage;
        heal_p_num = ingame_mng.player_mng.heal_power;
        needed_data_chip_num = needed_datachip;
        data_chip_num = ingame_mng.player_mng.player_data_chip;
        minus_data_chip_num = 0;

        //data chip
        data_texts[0].text = ingame_mng.player_mng.player_data_chip.ToString();
        data_texts[1].text = ingame_mng.player_mng.player_data_chip.ToString();
        data_texts[2].text = needed_datachip.ToString();
        data_texts[3].text = minus_data_chip_num.ToString();

        //states
        cur_states_texts[0].text = ingame_mng.player_mng.player_max_hp.ToString();
        cur_states_texts[1].text = ingame_mng.player_mng.player_max_stemina.ToString();
        cur_states_texts[2].text = _weapon.damage.ToString("F1");
        cur_states_texts[3].text = ingame_mng.player_mng.heal_power.ToString();
        cur_states_texts[4].text = ingame_mng.player_mng.heal_pack_max_num.ToString();

        //stetes in upgrade
        cur_states_inup_texts[0].text = ingame_mng.player_mng.player_max_hp.ToString();
        cur_states_inup_texts[1].text = ingame_mng.player_mng.player_max_stemina.ToString();
        cur_states_inup_texts[2].text = _weapon.damage.ToString("F1");
        cur_states_inup_texts[3].text = ingame_mng.player_mng.heal_power.ToString();

        //next states
        next_states_inup_texts[0].text = ingame_mng.player_mng.player_max_hp.ToString();
        next_states_inup_texts[1].text = ingame_mng.player_mng.player_max_stemina.ToString();
        next_states_inup_texts[2].text = _weapon.damage.ToString("F1");
        next_states_inup_texts[3].text = ingame_mng.player_mng.heal_power.ToString();
        
        //warnning color
        if (ingame_mng.player_mng.player_data_chip <= 0)
        {
            ingame_mng.player_mng.player_data_chip = 0;
            data_texts[1].color = Color.red;
        }
        else if (ingame_mng.player_mng.player_data_chip < needed_datachip)
        {
            data_texts[1].color = Color.red;
        }
        else
        {
            data_texts[1].color = Color.white;
        }


        if (ingame_mng.player_mng.player_data_chip < needed_datachip)
        {
            for(int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = false;
            }
        }
        else
        {
            for (int i = 0; i < buttons.Length-1; i++)
            {
                buttons[i].interactable = true;
            }
            if (ingame_mng.player_mng.player_max_hp <= hp_num) { buttons[0].interactable = false; }
            if (ingame_mng.player_mng.player_max_stemina <= stemina_num) { buttons[2].interactable = false; }
            if (_weapon.damage <= atk_p_num) { buttons[4].interactable = false; }
            if (ingame_mng.player_mng.heal_power <= heal_p_num) { buttons[6].interactable = false; }
            
            if ((hp_num > ingame_mng.player_mng.player_max_hp || stemina_num > ingame_mng.player_mng.player_max_stemina
            || atk_p_num > _weapon.damage || heal_p_num > ingame_mng.player_mng.heal_power))
            {
                buttons[8].interactable = true;
            }
            else
            {
                buttons[8].interactable = false;
            }
        }
    }

    public void state_decision()
    {
        //다시 갱신
        ingame_mng.player_mng.player_max_hp = hp_num;
        ingame_mng.player_mng.player_max_stemina = stemina_num;
        _weapon.damage = atk_p_num;
        ingame_mng.player_mng.heal_power = heal_p_num;
        ingame_mng.player_mng.player_data_chip = data_chip_num;
        needed_datachip = needed_data_chip_num;
        minus_data_chip_num = 0;

        if (ingame_mng.player_mng.player_data_chip <= 0)
        {
            ingame_mng.player_mng.player_data_chip = 0;
            data_texts[1].text = ingame_mng.player_mng.player_data_chip.ToString();
        }
        else if (ingame_mng.player_mng.player_data_chip < needed_datachip)
        {
            data_texts[1].text = ingame_mng.player_mng.player_data_chip.ToString();
        }
        else
        {
            data_texts[1].text = (ingame_mng.player_mng.player_data_chip - needed_datachip).ToString();
        }

        states_uploading();

    }

    void _previous_decision()
    {
        if(data_chip_num <= 0) { data_texts[1].color = Color.red; }
        else if(data_chip_num < needed_data_chip_num) { data_texts[1].color = Color.red; }
        else 
        { 
            data_texts[1].color = Color.white;
        }

        if (data_chip_num < needed_data_chip_num)
        {
            buttons[1].interactable = false;
            buttons[3].interactable = false;
            buttons[5].interactable = false;
            buttons[7].interactable = false;
        }
        else
        {
            buttons[1].interactable = true;
            buttons[3].interactable = true;
            buttons[5].interactable = true;
            buttons[7].interactable = true;
        }


        if ((hp_num > ingame_mng.player_mng.player_max_hp || stemina_num > ingame_mng.player_mng.player_max_stemina 
            || atk_p_num > _weapon.damage || heal_p_num > ingame_mng.player_mng.heal_power))
        {
            buttons[8].interactable = true;
        }
        else
        {
            buttons[8].interactable = false;
        }

        data_texts[1].text = data_chip_num.ToString();
        data_texts[2].text = needed_data_chip_num.ToString();
        data_texts[3].text = "-" + minus_data_chip_num.ToString();
    }




    public void hp_left_btn_click()
    {
        hp_num -= hp_p;
        data_chip_num += needed_data_chip_num;
        minus_data_chip_num -= needed_data_chip_num;
        needed_data_chip_num -= needed_datachip_increase;

        next_states_inup_texts[0].text = hp_num.ToString();
        if(ingame_mng.player_mng.player_max_hp >= hp_num) { buttons[0].interactable = false; }
        
        _previous_decision();
    }
    public void hp_right_btn_click()
    {
        hp_num += hp_p;
        needed_data_chip_num += needed_datachip_increase;
        minus_data_chip_num += needed_data_chip_num;
        data_chip_num -= needed_data_chip_num;

        next_states_inup_texts[0].text = hp_num.ToString();
        if (ingame_mng.player_mng.player_max_hp <= hp_num) { buttons[0].interactable = true; }
        
        _previous_decision();
    }
    public void stemina_left_btn_click()
    {
        stemina_num -= stemina_p;
        data_chip_num += needed_data_chip_num;
        minus_data_chip_num -= needed_data_chip_num;
        needed_data_chip_num -= needed_datachip_increase;

        next_states_inup_texts[1].text = stemina_num.ToString();
        if(ingame_mng.player_mng.player_max_stemina >= stemina_num) { buttons[2].interactable=false; }
        
        _previous_decision();
    }
    public void stemina_right_btn_click()
    {
        stemina_num += stemina_p;
        needed_data_chip_num += needed_datachip_increase;
        minus_data_chip_num += needed_data_chip_num;
        data_chip_num -= needed_data_chip_num;

        next_states_inup_texts[1].text = stemina_num.ToString();
        if (ingame_mng.player_mng.player_max_stemina <= stemina_num) { buttons[2].interactable = true; }

        _previous_decision();
    }
    public void attack_left_btn_click()
    {
        atk_p_num -= attak_p;
        data_chip_num += needed_data_chip_num;
        minus_data_chip_num -= needed_data_chip_num;
        needed_data_chip_num -= needed_datachip_increase;

        next_states_inup_texts[2].text = atk_p_num.ToString("F1");
        if (_weapon.damage >= atk_p_num) { buttons[4].interactable = false; }
        
        _previous_decision();
    }
    public void attack_right_btn_click()
    {
        atk_p_num += attak_p;
        needed_data_chip_num += needed_datachip_increase;
        minus_data_chip_num += needed_data_chip_num;
        data_chip_num -= needed_data_chip_num;

        next_states_inup_texts[2].text = atk_p_num.ToString("F1");
        if (_weapon.damage <= atk_p_num) { buttons[4].interactable = true; }
        
        _previous_decision();
    }
    public void heal_left_btn_click()
    {
        heal_p_num -= heal_p;
        data_chip_num += needed_data_chip_num;
        minus_data_chip_num -= needed_data_chip_num;
        needed_data_chip_num -= needed_datachip_increase;

        next_states_inup_texts[3].text = heal_p_num.ToString();
        if (ingame_mng.player_mng.heal_power >= heal_p_num) { buttons[6].interactable = false; }
        
        _previous_decision();
    }
    public void heal_right_btn_click()
    {
        heal_p_num += heal_p;
        needed_data_chip_num += needed_datachip_increase;
        minus_data_chip_num += needed_data_chip_num;
        data_chip_num -= needed_data_chip_num;

        next_states_inup_texts[3].text = heal_p_num.ToString();
        if (ingame_mng.player_mng.heal_power <= heal_p_num) { buttons[6].interactable = true; }
       
        _previous_decision();
    }
}
