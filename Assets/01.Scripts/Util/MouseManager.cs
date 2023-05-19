using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MouseManager
{
    public static void Show(bool isShow)
    {
        Cursor.visible = isShow;
    }

    public static void Lock(bool isLock)
    {
        Cursor.lockState = isLock ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
