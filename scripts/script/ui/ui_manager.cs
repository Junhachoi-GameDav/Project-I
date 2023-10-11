using UnityEngine;
using UnityEngine.UI;

public class ui_manager : MonoBehaviour
{
    public Text p_hp_text;
    public Text heal_text;
    public Image heal_bg;

    [Header("pause_ui_group")]
    //public Image pause_ui_group;

    [Header("boss_ui_group")]
    public GameObject boss_ui_group;

    [Header("data_chip_ui_group")]
    public GameObject data_chip_ui_group;
    public Text data_chip_text;
    public Image data_chip_bg_around;
    public float chip_timer;

    [Header("heal")]
    public Image boss_hp;
    public Image boss_hp2;
    public Image boss_hp_around;
    public Image p_hp;
    public Image p_hp2;
    public Image p_hp_around;


    [Header("stemina")]
    public GameObject p_stemina_group;
    public GameObject enemy_stemina_group;
    public Image p_stemina;
    public Image p_stemina2;
    public Image p_stemina_around;
    public Image enemy_stemina;
    public Image enemy_stemina2;
    public Image enemy_stemina_around;
    
    ingame_manager in_game_mng;


    public float heal_timer;

    private void Awake()
    {
        in_game_mng = FindObjectOfType<ingame_manager>();
    }
    private void Start()
    {
        data_chip_bg_around.color = new Vector4(255, 255, 255, 0);
    }

    private void Update()
    {
        battle_start_ui_sys();
        p_hp_ui_sys();
        boss_hp_ui_sys();
        heal_pack_ui_sys();
        data_chip_collect();
        
        stemina_ui_sys();
    }
    void battle_start_ui_sys()
    {
        if (in_game_mng._enemy.battle_start)
        {
            boss_ui_group.SetActive(true);
        }
        else
        {
            boss_ui_group.SetActive(false);
        }
    }

    void p_hp_text_sys(string text)
    {
        p_hp_text.text = text;
    }

    void p_hp_ui_sys()
    {
        p_hp_text_sys(in_game_mng.player_mng.player_hp.ToString());
        p_hp.fillAmount = in_game_mng.player_mng.player_hp / in_game_mng.player_mng.player_max_hp;

        if(in_game_mng.player_mng.player_hp <= 0) { in_game_mng.player_mng.player_hp = 0; }

        if (p_hp.fillAmount < p_hp2.fillAmount)
        {
            p_hp2.fillAmount -= Time.deltaTime * 0.5f;
        }
        else { p_hp2.fillAmount = p_hp.fillAmount; }

        //경고 색
        if (in_game_mng.player_mng.player_hp <= in_game_mng.player_mng.player_max_hp * 0.3f) { p_hp_around.color = Color.red; }
        else if (in_game_mng.player_mng.player_hp <= in_game_mng.player_mng.player_max_hp * 0.65f) { p_hp_around.color = Color.yellow; }
        else { p_hp_around.color = Color.white; }
    }
    void boss_hp_ui_sys()
    {
        boss_hp.fillAmount = in_game_mng._enemy.en_health / in_game_mng._enemy.max_en_health;
        if (boss_hp.fillAmount < boss_hp2.fillAmount)
        {
            boss_hp2.fillAmount -= Time.deltaTime * 0.1f;
        }
        else { boss_hp2.fillAmount = boss_hp.fillAmount; }

        //경고 색
        if (in_game_mng._enemy.en_health <= in_game_mng._enemy.max_en_health * 0.3f) { boss_hp_around.color = Color.red; }
        else if (in_game_mng._enemy.en_health <= in_game_mng._enemy.max_en_health * 0.65f) { boss_hp_around.color = Color.yellow; }
        else { boss_hp_around.color = Color.white; }
    }

    #region heal
    void heal_pack_ui_sys()
    {
        if(heal_timer > 0 && in_game_mng.player_mng.heal_pack_num > 0)
        {
            heal_timer -= Time.deltaTime / 1.6f;
        }
        heal_bg.fillAmount = heal_timer;
    }

    public void heal_pack_ui_count(string int_num)
    {
        heal_text.text = int_num;
    }
    #endregion

    #region data_collect

    void data_chip_collect()
    {
        data_chip_text.text = in_game_mng.player_mng.player_data_chip.ToString();

        if(chip_timer > 0)
        {
            chip_timer -= Time.deltaTime;
            data_chip_bg_around.color = new Vector4(255, 0, 255, chip_timer);
        }
        
    }
    #endregion

    
    void stemina_ui_sys()
    {
        //플레이어 스테미나 ui
        if(in_game_mng.player_mng.player_stemina <= 0.1f)
        {   
            p_stemina.color = Color.yellow;
            
            p_stemina_group.SetActive(false);
        } 
        else { p_stemina_group.SetActive(true);

            if (in_game_mng.player_mng.player_stemina >= in_game_mng.player_mng.player_max_stemina)
            {
                in_game_mng.player_mng.player_stemina = in_game_mng.player_mng.player_max_stemina;
                p_stemina.color = Color.red;
            }
            else
            {
                if (!in_game_mng.p_movement.run)
                {
                    in_game_mng.player_mng.player_stemina -=
                    in_game_mng.p_movement.stemina_healing ?
                    Time.deltaTime * in_game_mng.player_mng.player_max_stemina * 0.18f : Time.deltaTime * in_game_mng.player_mng.player_max_stemina * 0.03f;
                }
            }

            //게이지 깍이는 모션
            if (p_stemina.fillAmount < p_stemina2.fillAmount)
            {
                p_stemina2.fillAmount -= Time.deltaTime * 0.1f;
            }
            else { p_stemina2.fillAmount = p_stemina.fillAmount;}

            //경고 색
            if (in_game_mng.player_mng.player_stemina >= in_game_mng.player_mng.player_max_stemina * 0.85f) { p_stemina_around.color = Color.red; }
            else if (in_game_mng.player_mng.player_stemina >= in_game_mng.player_mng.player_max_stemina * 0.55f) { p_stemina_around.color = Color.yellow; }
            else if (in_game_mng.player_mng.player_stemina >= in_game_mng.player_mng.player_max_stemina * 0.3f) { p_stemina_around.enabled = true; }
            else { p_stemina_around.color = Color.white; p_stemina_around.enabled = false; }
        }

        //적 스테미나 ui
        if(in_game_mng._enemy.en_stemina <= 0.1f)
        {
            enemy_stemina.color = Color.yellow;
            
            enemy_stemina_group.SetActive(false);
        } 
        else { enemy_stemina_group.SetActive(true); 

            if (in_game_mng._enemy.en_stemina >= in_game_mng._enemy.max_en_stemina)
            {
                in_game_mng._enemy.en_stemina = in_game_mng._enemy.max_en_stemina;
                enemy_stemina.color = Color.red;
            }
            else
            {
                in_game_mng._enemy.en_stemina -= in_game_mng._enemy.is_stemina_heal ? 
                    Time.deltaTime * in_game_mng._enemy.max_en_stemina * 0.1f : Time.deltaTime * in_game_mng._enemy.max_en_stemina * 0.02f;
            }


            //게이지 깍이는 모션
            if (enemy_stemina.fillAmount < enemy_stemina2.fillAmount)
            {
                enemy_stemina2.fillAmount -= Time.deltaTime * 0.1f;
            }
            else { enemy_stemina2.fillAmount = enemy_stemina.fillAmount;}

            //경고 색
            if (in_game_mng._enemy.en_stemina >= in_game_mng._enemy.max_en_stemina * 0.85f) { enemy_stemina_around.color = Color.red; }
            else if (in_game_mng._enemy.en_stemina >= in_game_mng._enemy.max_en_stemina * 0.55f) { enemy_stemina_around.color = Color.yellow; }
            else if (in_game_mng._enemy.en_stemina >= in_game_mng._enemy.max_en_stemina * 0.3f) { enemy_stemina_around.enabled = true; }
            else { enemy_stemina_around.color = Color.white; enemy_stemina_around.enabled = false; }
        }

        // 수치 0~1로 정형화
        p_stemina.fillAmount = in_game_mng.player_mng.player_stemina / in_game_mng.player_mng.player_max_stemina;
        enemy_stemina.fillAmount = in_game_mng._enemy.en_stemina / in_game_mng._enemy.max_en_stemina;

        if(in_game_mng.player_mng.player_stemina <= 0) { p_stemina2.fillAmount = p_stemina.fillAmount; in_game_mng.player_mng.player_stemina = 0; }
        if(in_game_mng._enemy.en_stemina <= 0) { enemy_stemina2.fillAmount = enemy_stemina.fillAmount; in_game_mng._enemy.en_stemina = 0; }

    }

}
