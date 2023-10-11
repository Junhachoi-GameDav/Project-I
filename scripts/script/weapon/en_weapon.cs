
using UnityEngine;

public class en_weapon : weapon
{
    //public BoxCollider colliders;
    public BoxCollider kick_colliders;
    //public MeshCollider mesh;
    //player_manager p_manager;
    //player_movement p_movement;
    ingame_manager ingame_mng;
    public enemy ene;


    private void Awake()
    {
        ingame_mng =FindObjectOfType<ingame_manager>();
        ene = GetComponentInParent<enemy>();
    }
    private void Start()
    {
        melee_collider.enabled = false;
    }

    public void colliders_on_off(bool swhich)
    {
        melee_collider.enabled = swhich;
    }
    public void kick_colliders_on_off(bool swhich)
    {
        kick_colliders.enabled = swhich;
    }
    //ä�� ������ �ΰ� �־���...
    public void collider_on()
    {
        colliders_on_off(true);
        trail[0].enabled = true;
        
    }
    public void collider_off()
    {
        colliders_on_off(false);
        trail[0].enabled = false;
    }
    //����Ҷ� �ݶ��̴� �ʿ��� ����
    public void en_trail_on()
    {
        trail[0].enabled = true;
    }
    public void en_trail_off()
    {
        trail[0].enabled = false;

    }
    //��� �ִϸ��̼� ����
    public void player_up_anime()
    {
        ingame_mng.p_movement._anime.Play("bound_down_high");
    }
    public void player_push_anime()
    {
        ingame_mng.p_movement._anime.Play("knockdown_up02");
    }

    //
    public void dont_getbool_grab_hit()
    {
        ingame_mng.p_movement._anime.SetBool("grab_hit", false);
    }
    public void done_grab_or_kick_pettern()
    {
        ene.swhich_pettern_num = 0;
    }

    //����Ʈ
    public void grab_hitting_ef()
    {
        ingame_mng.player_mng.push_hit_particle.Play();
    }
    // ���� //����� �ִϸ��̼��� �����ؼ� ��� ��������.
    #region sound
    public void en_swing_whip_sound()
    {
        
    }
    public void hit_katana_sound()
    {
        
    }
    public void en_kick_sound()
    {
        
    }
    public void en_grab_sound()
    {
        
    }
    public void en_grab_whip_sound()
    {

    }
    public void en_guard_break_sound()
    {

    }
    public void en_footstep_sound()
    {
        
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("block_collider"))
        {
            colliders_on_off(false);
            kick_colliders_on_off(false);
        }
    }
}
