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

        Event keyEvent = Event.current;
        if (keyEvent.isKey)
        {
            if (keyEvent.keyCode == KeyCode.W
                || keyEvent.keyCode == KeyCode.A
                || keyEvent.keyCode == KeyCode.S
                || keyEvent.keyCode == KeyCode.D)
            {
                keySettingUI.SetIsChangeKey(false);
                return;
            }

            foreach (var keyCode in KeySetting.keys.Values)
            {
                if (keyEvent.keyCode == keyCode)
                {
                    keySettingUI.SetIsChangeKey(false);
                    return;
                }
            }

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

    //public void ChangeKey(string keyAction, KeyCode key)
    //{
    //    KeySetting.keys[(KeyAction)Enum.Parse(typeof(KeyAction), keyAction)] = key;
    //}
}
