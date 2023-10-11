using UnityEngine;

public class mouse_cursor_manager : MonoBehaviour
{
    public Texture2D mouse_cursor_texture2d;


    void Start()
    {
        Cursor.SetCursor(mouse_cursor_texture2d, Vector2.zero, CursorMode.Auto);
    }

    public void mouse_cursor_on_off(bool value)
    {
        if (value)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
