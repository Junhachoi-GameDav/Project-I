using UnityEngine;

public class ending : MonoBehaviour
{
    ingame_manager ingame_mng;
    private void Awake()
    {
        ingame_mng = FindObjectOfType<ingame_manager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            ingame_mng.p_movement.forward = 0;
            ingame_mng.p_movement.right = 0;
            ingame_mng.load_scene(3);
            game_manager.Instance.save_mng.now_player.is_ending= true;
            ingame_mng.save_data();
        }
    }
}
