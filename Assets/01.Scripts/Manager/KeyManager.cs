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
    public KeySettingUI keySettingUI { get; private set; } = null;
    Event keyEvent = null;

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
        keySettingUI = FindObjectOfType<KeySettingUI>()
;
        for(int i = 0; i < (int)KeyAction.KeyCount; ++i)
        {
            KeySetting.keys.Add((KeyAction)i, defaultKeys[i]);
        }
    }

    private void OnGUI()
    {
        if (!keySettingUI.isChangeKey)
            return;

        keyEvent = Event.current;
        if (keyEvent.isKey)
        {
            ExceptionCheck();

            KeySetting.keys[(KeyAction)key] = keyEvent.keyCode;
            key = -1;
            keySettingUI.UpdateKeyTmp();
        }
    }

    int key = -1;
    public void ChangeKey(int num)
    {
        key = num;
    }

    public void ExceptionCheck()
    {
        foreach (var keyCode in KeySetting.keys.Values)
        {
            if (keyEvent.keyCode == keyCode)
            {
                keySettingUI.SetIsChangeKey(false);
                return;
            }
        }
        for (int i = 0; i < keySettingUI.exceptionKeys.Length; ++i)
        {
            if (keyEvent.keyCode == keySettingUI.exceptionKeys[i])
                return;
        }
    }

    //public void ChangeKey(string keyAction, KeyCode key)
    //{
    //    KeySetting.keys[(KeyAction)Enum.Parse(typeof(KeyAction), keyAction)] = key;
    //}
}
