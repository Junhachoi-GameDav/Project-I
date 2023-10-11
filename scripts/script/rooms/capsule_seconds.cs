using System.Collections;
using UnityEngine;

public class capsule_seconds : MonoBehaviour
{
    public text_fade _text_fade;
    ingame_manager ingame_mng;
    capsule _capsule;
    public int num;

    void Awake()
    {
        ingame_mng = FindObjectOfType<ingame_manager>();
        _capsule = FindObjectOfType<capsule>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            _capsule.capsule_number = num;
            if(_capsule.capsule_number > 0) { _capsule.is_seconds_in = true; }
            if(_capsule.is_seconds_in && _capsule.capsule_fade_count <= 0)
            {
                StartCoroutine(co_tank_lit());
                _capsule.capsule_fade_count++;
            }
        }
    }
    IEnumerator co_tank_lit()
    {
        yield return yield_cache.WaitForSeconds(0.2f);
        _text_fade.Fade();
        yield return yield_cache.WaitForSeconds(0.5f);
        game_manager.Instance.sound_mng.sm_ui_sound_play("tank_found");
        ingame_mng.save_data();
    }
}
