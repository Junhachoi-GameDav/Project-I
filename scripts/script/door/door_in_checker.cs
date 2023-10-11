
using UnityEngine;

public class door_in_checker : MonoBehaviour
{
    doors _doors;
    public bool is_player_in;

    private void Awake()
    {
        _doors = GetComponentInParent<doors>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            _doors.open_doors();
            is_player_in = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("player"))
        {
            is_player_in = false;
        }
    }
}
