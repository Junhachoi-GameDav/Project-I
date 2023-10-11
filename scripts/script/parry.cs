using UnityEngine;


public class parry : MonoBehaviour
{
    public ParticleSystem particle;
    player_movement p_movement;
    player_manager p_manager;
    
    int ran_num;
    int show_num;

    private void Awake()
    {
        p_movement = GetComponentInParent<player_movement>();
        p_manager = GetComponentInParent<player_manager>();
    }
    private void Start()
    {
        this.gameObject.SetActive(false);
    }
    void time_root()
    {
        Time.timeScale = 1.0f;
        p_movement.parry = false;
        p_movement.gameObject.layer = 3;
        show_num = 0;
        ran_num = Random.Range(0, 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("en_melee"))
        {
            enemy_manager en_mng = other.GetComponentInParent<enemy_manager>();
            p_movement.parry = true;
            p_movement.gameObject.layer = 12;
            p_manager.parry_katana_sound();
 
            en_mng.en_stemina += p_movement.p_weapon.damage * 3;
            p_manager.player_stemina -= p_manager.player_max_stemina * 0.15f;
            p_movement._anime.SetBool("pushed_hit", false);
            p_movement._anime.ResetTrigger("hit_trigger");
            p_movement._anime.ResetTrigger("guard_hit");
            p_movement._anime.ResetTrigger("grab_hit_trigger");

            if(show_num <= 0) { particle.Play(); show_num = 1; }
            if(ran_num > 0 ) { p_movement._anime.Play("parry"); } else { p_movement._anime.Play("parry2 1"); }
            
            Time.timeScale = 0.2f;
            p_movement.block_collider_off();
            Invoke("time_root",0.12f);
        }
    }
}
