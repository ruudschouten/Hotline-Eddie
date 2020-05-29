using System;
using UnityEngine;

public class CursorTextureSetter : MonoBehaviour
{
    [SerializeField] private Texture2D texture;
    [SerializeField] private Vector2 crosshairOffset;

    private void Awake()
    {
        Cursor.SetCursor(texture, crosshairOffset, CursorMode.Auto);
    }
}