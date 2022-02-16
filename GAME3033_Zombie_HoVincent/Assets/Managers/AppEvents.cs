using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppEvents
{
    public delegate void OnMouseCursorEnable(bool enabled);

    public static event OnMouseCursorEnable MouseCursorEnabled;

    public static void InvokeOnMouseCursorEnable(bool enabled)
    {
        MouseCursorEnabled?.Invoke(enabled);
    }
}
