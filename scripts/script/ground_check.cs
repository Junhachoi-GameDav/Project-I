
using UnityEngine;

public class ground_check : MonoBehaviour
{
    [Header("box Property")]
    [SerializeField] private Vector3 box_size;
    [SerializeField] private float maxditance;

    [Header("LayerMask")]
    [SerializeField] private LayerMask ground_layer;
    [SerializeField] private LayerMask enemy_layer;

    [Header("ray Property")]
    [SerializeField] private Transform chliff_check_pos;
    [SerializeField] private Transform wall_check_pos;
    [SerializeField] private Transform foot_check_pos_l;
    [SerializeField] private Transform foot_check_pos_r;
    [SerializeField] private float ray_distance;
    [SerializeField] private float ray_distance2;
    [SerializeField] private float ray_distance3_wall;
    [SerializeField] private float ray_distance_on_foot;


    [Header("Debug")]
    [SerializeField] private bool drawGizmo;

    public bool can_jump;

    private void OnDrawGizmos()
    {
        if (!drawGizmo)
        {
            return;
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position - transform.up * maxditance, box_size);
        Gizmos.DrawRay(transform.position, -transform.up * ray_distance);
        Gizmos.DrawRay(chliff_check_pos.position, -transform.up * ray_distance2);
        Gizmos.DrawRay(wall_check_pos.position, transform.forward * ray_distance3_wall);
        Gizmos.DrawRay(foot_check_pos_l.position, -transform.up * ray_distance_on_foot);
        Gizmos.DrawRay(foot_check_pos_r.position, -transform.up * ray_distance_on_foot);
    }

    public bool is_ground()
    {
        RaycastHit hit_info;
        return Physics.BoxCast(transform.position, box_size, -transform.up, out hit_info, transform.rotation, maxditance, ground_layer);
    }
    public bool is_stair()
    {
        RaycastHit hit_info;
        return Physics.Raycast(transform.position, -transform.up, out hit_info, ray_distance, ground_layer);
    }

    public bool is_cliff()
    {
        RaycastHit hit_info;
        return Physics.Raycast(chliff_check_pos.position, -transform.up, out hit_info, ray_distance2, ground_layer);
    }
    public bool is_wall()
    {
        RaycastHit hit_info;
        return Physics.Raycast(wall_check_pos.position, transform.forward, out hit_info, ray_distance3_wall, ground_layer);
    }

    public bool is_enemy()
    {
        RaycastHit hit_info;
        return Physics.Raycast(wall_check_pos.position, transform.forward, out hit_info, ray_distance3_wall - 0.1f, enemy_layer);
    }

    public bool is_l_foot_on_floor()
    {
        RaycastHit hit_info;
        return Physics.Raycast(foot_check_pos_l.position, -transform.up, out hit_info, ray_distance_on_foot, ground_layer);
    }
    public bool is_r_foot_on_floor()
    {
        RaycastHit hit_info;
        return Physics.Raycast(foot_check_pos_r.position, -transform.up, out hit_info, ray_distance_on_foot, ground_layer);
    }
}
