using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeySettingUI : MonoBehaviour
{
    [SerializeField]
    private GameObject keySettingPanel = null;

    [SerializeField]
    private TextMeshProUGUI[] buttonTmp;

    public bool isChangeKey { get; private set; } = false;

    private void Start()
    {
        keySettingPanel.SetActive(false);

        for(int i = 0; i < buttonTmp.Length; ++i)
        {
            buttonTmp[i].text = KeySetting.keys[(KeyAction)i].ToString().ToUpper();
        }
    }

    public void ToggleKeySettingPanel()
    {
        UIManager.Instance.PushPanel(keySettingPanel);
    }

    public void UpdateKeyTmp()
    {
        for (int i = 0; i < buttonTmp.Length; ++i)
        {
            buttonTmp[i].text = KeySetting.keys[(KeyAction)i].ToString().ToUpper();
        }
    }

    public void ChangeKey(int num)
    {
        isChangeKey = true;
        KeyManager.Instance.ChangeKey(num);
    }

    public void SetIsChangeKey(bool _isChangeKey)
    {
        isChangeKey = _isChangeKey;
    }
}
