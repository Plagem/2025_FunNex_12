using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [Header("Cursor Textures")]
    public Texture2D defaultCursor;
    public Texture2D clickCursor;

    [Header("Hotspot")]
    public Vector2 hotspot = Vector2.zero;

    private void Start()
    {
        SetDefaultCursor();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetClickCursor();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            SetDefaultCursor();
        }
    }

    public void SetDefaultCursor()
    {
        Cursor.SetCursor(defaultCursor, hotspot, CursorMode.Auto);
    }

    public void SetClickCursor()
    {
        Cursor.SetCursor(clickCursor, hotspot, CursorMode.Auto);
    }
}
