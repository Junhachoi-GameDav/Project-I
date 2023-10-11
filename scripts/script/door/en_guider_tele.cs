
using UnityEngine;

public class en_guider_tele : MonoBehaviour
{
    public GameObject en_guider_obj;
    public GameObject capsule_obj;
    ingame_manager ingame_mng;

    public int num;

    private void Awake()
    {
        ingame_mng = FindObjectOfType<ingame_manager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            en_guider_obj.transform.position = ingame_mng.player_respawn_pos[num].position;
            en_guider_obj.transform.rotation = ingame_mng.player_respawn_pos[num].rotation;
            capsule_obj.transform.position = ingame_mng.player_respawn_pos[num+2].position;
            capsule_obj.transform.rotation = ingame_mng.player_respawn_pos[num+2].rotation;
        }
    }
}
