using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
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
    [SerializeField]
    float dashDistance = 2f;
    PlayerBase playerBase;
    [Space]
    [Header("스킬")]
    List<PlayerSkillBase> SkillBase;
    Dictionary<Define.PlayerTransformTypeFlag, PlayerSkillBase> skillData = new Dictionary<Define.PlayerTransformTypeFlag, PlayerSkillBase>();
    List<int> randomSkillNum = new List<int>();
    Rigidbody2D rb;
    Define.SkillNum[] skillNum = null;
    int[] slotLevel = new int[2] { 1, 1 };
    Action[] skillEvent = new Action[5];
    private void Awake()
    {
        rb = GameManager.Instance.Player.gameObject.GetComponent<Rigidbody2D>();
        playerBase = GameManager.Instance.Player.playerBase;
        UIManager.Instance.playerUI.transform.Find("RightDown/Btns/Skill1_Btn").GetComponent<Button>().onClick.AddListener(Skill1);
        UIManager.Instance.playerUI.transform.Find("RightDown/Btns/Skill2_Btn").GetComponent<Button>().onClick.AddListener(Skill2);
        UIManager.Instance.playerUI.transform.Find("RightDown/Btns/Dash_Btn").GetComponent<Button>().onClick.AddListener(DashSkill);
        UIManager.Instance.playerUI.transform.Find("RightDown/Btns/UltimateSkill_Btn").GetComponent<Button>().onClick.AddListener(UltimateSkill);
        UIManager.Instance.playerUI.transform.Find("RightDown/Btns/AttackBtn").GetComponent<Button>().onClick.AddListener(Attack);
        skillData.Add(Define.PlayerTransformTypeFlag.Ghost, GetComponent<GhostSkill>());
        skillData.Add(Define.PlayerTransformTypeFlag.Power, GetComponent<PowerSkill>());
    }
    private void Start()
    {
        SkillShuffle();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SkillSelect();
        }
    }
    void SkillSelect()
    {
        PlayerSkillBase playerSkill;
        if (skillData.TryGetValue(playerBase.PlayerTransformTypeFlag, out playerSkill))
        {
            
            skillEvent[0] = () => playerSkill.playerSkills[0](0);
            skillEvent[1] = () => playerSkill.playerSkills[1](0);
            skillEvent[2] = playerSkill.attack;
            skillEvent[3] = playerSkill.ultimateSkill;
            skillEvent[4] = playerSkill.dashSkill;
        }
    }
    void Attack()
    {
        skillEvent[2]();
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
    void DashSkill()
    {
        if (UIManager.Instance.SkillCooltime(playerBase.PlayerTransformData,Define.SkillNum.DashSkill))
            transform.parent.position =  rb.position + PlayerMovement.Instance.Direction * dashDistance;
    }
    void UltimateSkill()
    {
        if (UIManager.Instance.SkillCooltime(playerBase.PlayerTransformData,Define.SkillNum.UltimateSkill))
            skillEvent[4]();
    }
    #region 리스트 셔플

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
