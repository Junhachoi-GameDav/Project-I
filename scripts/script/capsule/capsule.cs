using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class capsule : MonoBehaviour
{
    public int capsule_number;
    public int capsule_fade_count;
    ingame_manager ingame_mng;
    enemy_target_checker target_ck;

    public GameObject en_guider_obj;
    public GameObject cam;
    public GameObject capsule_menu_obj;
    public GameObject heal_info_obj;
    public Button tele_button;
    public Button heal_pack_button;

    [Header("interaction")]
    public GameObject inter_icon_obj;
    public Image interection_icon_img;
    public Transform inter_icon_pos;
    public ParticleSystem inter_ef;

    [Header("bool")]
    public bool is_player_in;
    public bool is_seconds_in;

    private void Awake()
    {
        ingame_mng = FindObjectOfType<ingame_manager>();
        target_ck = GetComponent<enemy_target_checker>();
    }
    
    private void Update()
    {
        if (target_ck.is_target_checked() && Input.GetKeyDown(key_setting.keys[key_action.INTERECTION]) &&!is_player_in)
        {
            cam.SetActive(true);
            capsule_menu_obj.SetActive(true);
            ingame_mng.player_mng.heal_particle2.Play();
            game_manager.Instance.sound_mng.sm_normal_ef_sound_play("heal_impact");
            ingame_mng.ui_mng.heal_pack_ui_count(ingame_mng.player_mng.heal_pack_max_num.ToString());
            inter_ef.Play();
            respawn_eneies();
            if (is_seconds_in) { tele_button.interactable = true; }
            if (ingame_mng.player_mng.pill_part_num > 0) { heal_pack_button.interactable = true; }
            is_player_in = true;
        }
        check();
    }
    void check()
    {
        if (target_ck.is_target_checked() && !is_player_in)
        {
            inter_icon_obj.SetActive(true);
            interection_icon_img.rectTransform.position = Camera.main.WorldToScreenPoint(inter_icon_pos.position);
        }
        else { inter_icon_obj.SetActive(false); }
    }

    public void is_player_out()
    {
        is_player_in = false;
    }

    public void respawn_eneies()
    {
        ingame_mng.respawn_enemies();
    }

    public void tele_capsule(int num)
    {
        StartCoroutine(co_tele_capsule(num));
    }
    IEnumerator co_tele_capsule(int num)
    {
        game_manager.Instance.fade_mng.Fade();
        yield return yield_cache.WaitForSeconds(1f);
        ingame_mng.is_game_starting = true;
        ingame_mng.p_movement._anime.applyRootMotion = false;
        ingame_mng.player_mng.transform.position = ingame_mng.player_respawn_pos[num].position;
        ingame_mng.player_mng.transform.rotation = ingame_mng.player_respawn_pos[num].rotation;

        en_guider_obj.transform.position = ingame_mng.player_respawn_pos[num + 2].position;
        en_guider_obj.transform.rotation = ingame_mng.player_respawn_pos[num + 2].rotation;

        transform.position = ingame_mng.player_respawn_pos[num + 4].position;
        transform.rotation = ingame_mng.player_respawn_pos[num + 4].rotation;

        capsule_number = num;

        yield return null;
        ingame_mng.is_game_starting = false;
    }

    public void heal_pack_add()
    {
        if(ingame_mng.player_mng.pill_part_num > 0) { ingame_mng.player_mng.pill_part_num--; }
        else { ingame_mng.player_mng.pill_part_num = 0; }
        ingame_mng.player_mng.heal_pack_num++;
        ingame_mng.player_mng.heal_pack_max_num++;
        ingame_mng.ui_mng.heal_pack_ui_count(ingame_mng.player_mng.heal_pack_num.ToString());

        if (ingame_mng.player_mng.pill_part_num <= 0) { heal_pack_button.interactable = false; }
        Invoke("set_false", 2f);
    }
    void set_false()
    {
        heal_info_obj.SetActive(false);
    }
}
