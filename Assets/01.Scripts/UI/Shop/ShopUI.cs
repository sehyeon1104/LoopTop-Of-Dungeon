using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    #region SkillBook_Val

    [SerializeField]
    private GameObject skillBookPanel = null;
    [SerializeField]
    private GameObject skillShufflePanel = null;
    [SerializeField]
    private GameObject changeCharacterPanel = null;
    public bool isSkillBookPanelActive { get; private set; } = false;
    public bool isSkillShufflePanelActive { get; private set; } = false;
    public bool isChangeCharacterPanelActive { get; private set; } = false;

    public int slotNum { get; private set; } = 0;
    private int price = 0;

    #endregion

    #region SkillShuffle_Val


    PlayerSkillInfo[] playerskillInfo;
    //public GameObject skillSelect = null;

    #endregion

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        //SkillSelectButtonInit();

        playerskillInfo = GameManager.Instance.Player.playerBase.PlayerTransformData.skill;
    }

    #region SkillBook_Func

    public void ToggleSkillBookPanel()
    {
        isSkillBookPanelActive = !skillBookPanel.activeSelf;
        if (isSkillBookPanelActive)
        {
            UIManager.Instance.PushPanel(skillBookPanel);
            MouseManager.Lock(!skillBookPanel.activeSelf);
            MouseManager.Show(skillBookPanel.activeSelf);
        }
        else
        {
            UIManager.Instance.PopPanel();
            MouseManager.Lock(!skillBookPanel.activeSelf);
            MouseManager.Show(skillBookPanel.activeSelf);
        }
    }

    #endregion

    #region SkillShuffle_Func

    public void ToggleSkillShufflePanel()
    {
        isSkillShufflePanelActive = !skillShufflePanel.activeSelf;
        if (isSkillShufflePanelActive)
        {
            UIManager.Instance.PushPanel(skillShufflePanel);
            MouseManager.Lock(!skillShufflePanel.activeSelf);
            MouseManager.Show(skillShufflePanel.activeSelf);
        }
        else
        {
            UIManager.Instance.PopPanel();
            MouseManager.Lock(!skillShufflePanel.activeSelf);
            MouseManager.Show(skillShufflePanel.activeSelf);
        }
    }
    #endregion

    public void ToggleChangeCharacterPanel()
    {
        isChangeCharacterPanelActive = !changeCharacterPanel.activeSelf;

        if (isChangeCharacterPanelActive)
        {
            UIManager.Instance.PushPanel(changeCharacterPanel);
            MouseManager.Lock(!changeCharacterPanel.activeSelf);
            MouseManager.Show(changeCharacterPanel.activeSelf);
        }
        else
        {
            UIManager.Instance.PopPanel();
            MouseManager.Lock(!changeCharacterPanel.activeSelf);
            MouseManager.Show(changeCharacterPanel.activeSelf);
        }
    }
}
