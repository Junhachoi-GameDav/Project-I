
using UnityEngine;

public class line_checker : MonoBehaviour
{
    public en_kyle[] en_kyles;

    public bool line_check;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            if(line_check) { player_line_over_check(); }
            else { player_line_over_uncheck(); }
        }
    }

    void player_line_over_check()
    {
        for (int i = 0; i < en_kyles.Length; i++)
        {
            en_kyles[i].is_player_line_over = true;
        }
    }

    void player_line_over_uncheck()
    {
        for (int i = 0; i < en_kyles.Length; i++)
        {
            en_kyles[i].is_player_line_over = false;
        }
    }
}
