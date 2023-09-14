using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

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

    INVENTORY,      // 인벤토리

    KeyCount,
}

public static class KeySetting
{
    public static Dictionary<KeyAction, KeyCode> keys = new Dictionary<KeyAction, KeyCode>();
    public static Dictionary<string, Sprite> keySprite = new Dictionary<string, Sprite>();
}

public class KeyManager : MonoSingleton<KeyManager>
{
    private Sprite[] keySprite = null;

    [SerializeField]
    private Sprite[] specKey = null;
    private GameObject keyCap = null;
    private GameObject mouseClick = null;

    public KeySettingUI keySettingUI { get; private set; } = null;
    private Event keyEvent = null;

    KeyCode[] defaultKeys = new KeyCode[]
    {
        KeyCode.Mouse0,
        KeyCode.F,
        KeyCode.Space,

        KeyCode.Mouse1,
        KeyCode.Q,
        KeyCode.E,

        KeyCode.Tab,
    };

    KeyCode[] exceptionKeys = new KeyCode[]
    {
        KeyCode.W,
        KeyCode.A,
        KeyCode.S,
        KeyCode.D,

        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow,

        KeyCode.Escape,
        KeyCode.Return,
        KeyCode.Backspace,
    };


    private void Awake()
    {
        keySettingUI = FindObjectOfType<KeySettingUI>();

        keyCap = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/Tutorial/KeyCap.prefab");
        mouseClick = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/Tutorial/MouseClick.prefab");

        KeySpriteInit();
    }

    public void InitKey()
    {
        KeySetting.keys.Clear();

        for (int i = 0; i < (int)KeyAction.KeyCount; ++i)
        {
            if (!KeySetting.keys.ContainsValue(defaultKeys[i]))
                KeySetting.keys.Add((KeyAction)i, defaultKeys[i]);
        }
        GameManager.Instance.SaveKeyData();
    }

    public void InitKeySetting()
    {
        KeySetting.keys.Clear();

        for (int i = 0; i < (int)KeyAction.KeyCount; ++i)
        {
            if (!KeySetting.keys.ContainsValue(defaultKeys[i]))
                KeySetting.keys.Add((KeyAction)i, defaultKeys[i]);
        }
        GameManager.Instance.SaveKeyData();
        keySettingUI.UpdateKeyTmp();
    }

    public void KeySpriteInit()
    {
        foreach(var keySpr in specKey)
        {
            string s = keySpr.name.Substring(9, keySpr.name.Length - 9);
            if(!KeySetting.keySprite.ContainsKey(s))
                KeySetting.keySprite.Add(s, keySpr);
        }
    }

    private void OnGUI()
    {
        if (!keySettingUI.isChangeKey)
            return;

        keyEvent = Event.current;

        if (keyEvent.type == EventType.KeyDown || keyEvent.isMouse)
        {
            if (!ExceptionCheck() || keyEvent.keyCode == KeyCode.None && !keyEvent.isMouse)
                return;

            if (keyEvent.isMouse)
                KeySetting.keys[(KeyAction)key] = (KeyCode)(keyEvent.button + 323);
            else
                KeySetting.keys[(KeyAction)key] = keyEvent.keyCode;

            key = -1;
            keySettingUI.ToggleChangeKeyPanel();
            keySettingUI.UpdateKeyTmp();
            keySettingUI.SetIsChangeKey(false);
        }
    }

    int key = -1;
    public void ChangeKey(int num)
    {
        key = num;
    }

    public bool ExceptionCheck()
    {
        // 현재 지닌 키
        foreach (var keyCode in KeySetting.keys.Values)
        {
            if (keyEvent.keyCode == keyCode)
                return false;

            if (keyEvent.isMouse)
            {
                if ((KeyCode)(keyEvent.button + 323) == keyCode)
                    return false;
            }
        }
        // 예외 키
        for (int i = 0; i < exceptionKeys.Length; ++i)
        {
            if (keyEvent.keyCode == exceptionKeys[i])
                return false;
        }

        return true;
    }

    public GameObject InstantiateKey(KeyCode keyCode, Transform parent)
    {
        GameObject obj = null;

        if(keyCode.ToString().Length == 1)
        {
            obj = Instantiate(keyCap);
            obj.GetComponentInChildren<TextMeshPro>().SetText(keyCode.ToString());
        }
        else if(keyCode == KeyCode.Mouse0 || keyCode == KeyCode.Mouse1)
        {
            obj = Instantiate(mouseClick);
            obj.GetComponent<SpriteRenderer>().flipX = (int)keyCode % 2 == 0 ? true : false;
        }
        else
        {
            obj = Instantiate(keyCap);
            obj.GetComponent<SpriteRenderer>().sprite = KeySetting.keySprite[((int)keyCode).ToString()];
        }

        obj.transform.SetParent(parent);
        obj.transform.position = parent.position;
        return obj;
    }
}