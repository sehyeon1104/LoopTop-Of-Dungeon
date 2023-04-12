using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
// using System.Drawing.Drawing2D;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

// Player Skill Class
public class PlayerSkill : MonoBehaviour
{
    PlayerBase playerBase;
    [Space]
    [Header("ï¿½ï¿½Å³")]
    List<PlayerSkillBase> SkillBase;
    Dictionary<Define.PlayerTransformTypeFlag, PlayerSkillBase> skillData = new Dictionary<Define.PlayerTransformTypeFlag, PlayerSkillBase>();
    List<int> randomSkillNum = new List<int>();
    Rigidbody2D rb;
    int[] slotLevel = new int[2] { 1, 1 };
    Action[] skillEvent = new Action[5];
    PlayerSkillBase[] playerSkillBases = new PlayerSkillBase[2];
    private void Awake()
    {  
        playerSkillBases[0] = GetComponent<PowerSkill>();
        playerSkillBases[1] = GetComponent<GhostSkill>();
        playerBase = GameManager.Instance.Player.playerBase;
        UIManager.Instance.playerUI.transform.Find("RightDown/Btns/Skill1_Btn").GetComponent<Button>().onClick.AddListener(Skill1);
        UIManager.Instance.playerUI.transform.Find("RightDown/Btns/Skill2_Btn").GetComponent<Button>().onClick.AddListener(Skill2);
        UIManager.Instance.playerUI.transform.Find("RightDown/Btns/Dash_Btn").GetComponent<Button>().onClick.AddListener(DashSkill);
        UIManager.Instance.playerUI.transform.Find("RightDown/Btns/UltimateSkill_Btn").GetComponent<Button>().onClick.AddListener(UltimateSkill);
        UIManager.Instance.playerUI.transform.Find("RightDown/Btns/AttackBtn").GetComponent<Button>().onClick.AddListener(Attack);
        skillData.Add(Define.PlayerTransformTypeFlag.Power, playerSkillBases[0]);
        skillData.Add(Define.PlayerTransformTypeFlag.Ghost, playerSkillBases[1]);
    }
    private void Start()
    {
        SkillSelect(playerBase.PlayerTransformTypeFlag);
        for (int i =0; i < playerSkillBases.Length; i++) {
            playerSkillBases[i].enabled = false;    
        }
        SkillShuffle();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            playerSkillBases[(int)playerBase.PlayerTransformTypeFlag].enabled = false;
            playerBase.PlayerTransformTypeFlag = Define.PlayerTransformTypeFlag.Ghost;
            playerBase.PlayerTransformData = playerBase.PlayerTransformDataSOList[(int)playerBase.PlayerTransformTypeFlag];
            
            PlayerVisual.Instance.UpdateVisual(playerBase.PlayerTransformData);
            SkillSelect(playerBase.PlayerTransformTypeFlag);
        }

    }
    void SkillSelect(Define.PlayerTransformTypeFlag playerType)
    {
        PlayerSkillBase playerSkill;
        if (skillData.TryGetValue(playerType, out playerSkill))
        {
            playerSkill.enabled = true;
            skillEvent[0] = () => playerSkill.playerSkills[0](4);
            skillEvent[1] = () => playerSkill.playerSkills[2](1);
            skillEvent[2] = playerSkill.attack;
            skillEvent[3] = playerSkill.ultimateSkill;
            skillEvent[4] = playerSkill.dashSkill;
        }
    }
    void Skill1()
    {
        if (UIManager.Instance.SkillCooltime(playerBase.PlayerTransformData,Define.SkillNum.FirstSkill))
            skillEvent[0]();
    }

    void Skill2()
    {
        if (UIManager.Instance.SkillCooltime(playerBase.PlayerTransformData,Define.SkillNum.SecondSkill))
            skillEvent[1]();
    }
    void Attack()
    {
        skillEvent[2]();
    }
    void UltimateSkill()
    {
        if (UIManager.Instance.SkillCooltime(playerBase.PlayerTransformData,Define.SkillNum.UltimateSkill))
            skillEvent[3]();
    }

    void DashSkill()
    {
        if (UIManager.Instance.SkillCooltime(playerBase.PlayerTransformData, Define.SkillNum.DashSkill))
            skillEvent[4]();        
    }
    #region ½ºÅ³ ¼ÅÇÃ

    public void ListInit()
    {
        randomSkillNum.Clear();
        for (int i = 1; i < 6; i++)
        {
            randomSkillNum.Add(i);
        }
    }
    public void ListShuffle()
    {

        for (int i = 0; i < randomSkillNum.Count; i++)
        {
            int randomAt = Random.Range(1, randomSkillNum.Count + 1);
            int randomAt2 = Random.Range(1, randomSkillNum.Count + 1);
            while (randomAt == randomAt2)
            {
                randomAt2 = Random.Range(1, randomSkillNum.Count + 1);
            }
            int randomAtIndex = randomSkillNum.IndexOf(randomAt);
            int randomAt2Index = randomSkillNum.IndexOf(randomAt2);
            randomSkillNum.Insert(randomAtIndex, randomAt2);
            randomSkillNum.RemoveAt(randomAtIndex + 1);
            randomSkillNum.Insert(randomAt2Index, randomAt);
            randomSkillNum.RemoveAt(randomAt2Index + 1);
        }
    }
    public void ListRemove()
    {
        randomSkillNum.RemoveRange(3, 2);

    }
    public void SkillShuffle()
    {
        ListInit();
        ListShuffle();
        ListRemove();
    }

    #endregion

}
