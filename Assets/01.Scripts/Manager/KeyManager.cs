using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum KeyAction
{
    // 플레이어 베이스 스킬
    ATTACK,         // 공격
    INTERACTION,    // 상호작용
    DASH,           // 대쉬

    // 플레이어 스킬
    SKILL1,         // 스킬1
    SKILL2,         // 스킬2
    ULTIMATE,       // 궁극기

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
        KeyCode.Mouse0,
        KeyCode.F,
        KeyCode.Space,

        KeyCode.Mouse1,
        KeyCode.Q,
        KeyCode.E,
    };

    KeyCode[] exceptionKeys = new KeyCode[]
    {
        KeyCode.W,
        KeyCode.A,
        KeyCode.S,
        KeyCode.D,
        KeyCode.Escape,
        KeyCode.Return,
        KeyCode.Backspace,
    };

    private void Awake()
    {
        keySettingUI = FindObjectOfType<KeySettingUI>()
;
        for(int i = 0; i < (int)KeyAction.KeyCount; ++i)
        {
            if(!KeySetting.keys.ContainsValue(defaultKeys[i]))
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
            if (!ExceptionCheck())
                return;

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

    public bool ExceptionCheck()
    {
        foreach (var keyCode in KeySetting.keys.Values)
        {
            if (keyEvent.keyCode == keyCode)
            {
                keySettingUI.SetIsChangeKey(false);
                return false;
            }
        }
        for (int i = 0; i < exceptionKeys.Length; ++i)
        {
            if (keyEvent.keyCode == exceptionKeys[i])
                return false;
        }

        return true;
    }

    //public void ChangeKey(string keyAction, KeyCode key)
    //{
    //    KeySetting.keys[(KeyAction)Enum.Parse(typeof(KeyAction), keyAction)] = key;
    //}
}
