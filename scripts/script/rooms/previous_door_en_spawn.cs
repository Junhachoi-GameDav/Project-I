using UnityEngine;

public class previous_door_en_spawn : MonoBehaviour
{
    public GameObject[] area_n_enemies;
    //public GameObject area_n_pill;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            settings_area1();
        }
    }

    void settings_area1()
    {
        for (int i = 0; i < area_n_enemies.Length; i++)
        {
            area_n_enemies[i].SetActive(false);
        }
    }
}
