using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum KeyAction
{
    // �÷��̾� ���̽� ��ų
    ATTACK,         // ����
    INTERACTION,    // ��ȣ�ۿ�
    DASH,           // �뽬

    // �÷��̾� ��ų
    SKILL1,         // ��ų1
    SKILL2,         // ��ų2
    ULTIMATE,       // �ñر�

    KeyCount,
}

public static class KeySetting
{
    public static Dictionary<KeyAction, KeyCode> keys = new Dictionary<KeyAction, KeyCode>();
}

public class KeyManager : MonoSingleton<KeyManager>
{
    KeyCode[] defaultKeys = new KeyCode[]
    {
        KeyCode.J,
        KeyCode.F,
        KeyCode.K,

        KeyCode.U,
        KeyCode.I,
        KeyCode.O,
    };

    private void Awake()
    {
        for(int i = 0; i < (int)KeyAction.KeyCount; ++i)
        {
            KeySetting.keys.Add((KeyAction)i, defaultKeys[i]);
        }
    }

    public void ChangeKey(string keyAction, KeyCode key)
    {
        KeySetting.keys[(KeyAction)Enum.Parse(typeof(KeyAction), keyAction)] = key;
    }
}
