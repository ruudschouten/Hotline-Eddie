using System;
using UnityEngine;

public class CursorTextureSetter : MonoBehaviour
{
    [SerializeField] private Texture2D texture;
    [SerializeField] private Vector2 crosshairOffset;

    private bool _resetTexture;

    private void Awake()
    {
        SetCursor();
    }

    private void Update()
    {
        if (_resetTexture)
        {
            SetCursor();
            _resetTexture = false;
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        _resetTexture = !hasFocus;
    }

    private void SetCursor()
    {
        Cursor.SetCursor(texture, crosshairOffset, CursorMode.Auto);
    }
}