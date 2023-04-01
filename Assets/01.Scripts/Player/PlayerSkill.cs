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
    float dashDistance = 3f;
    PlayerBase playerBase;
    [Space]
    [Header("��ų")]
    List<PlayerSkillBase> SkillBase;
    Dictionary<Define.PlayerTransformTypeFlag, PlayerSkillBase> skillData = new Dictionary<Define.PlayerTransformTypeFlag, PlayerSkillBase>();
    List<int> randomSkillNum = new List<int>();
    Rigidbody2D rb;
    int[] slotLevel = new int[2] { 1, 1 };
    Action<int>[] skillEvent = new Action<int>[2];
    Action ultimateSkill;
    private void Awake()
    {
        rb = GameManager.Instance.Player.gameObject.GetComponent<Rigidbody2D>();
        playerBase = GameManager.Instance.Player.playerBase;
        UIManager.Instance.playerUI.transform.Find("RightDown/Btns/Skill1_Btn").GetComponent<Button>().onClick.AddListener(Skill1);
        UIManager.Instance.playerUI.transform.Find("RightDown/Btns/Skill2_Btn").GetComponent<Button>().onClick.AddListener(Skill2);
        UIManager.Instance.playerUI.transform.Find("RightDown/Btns/Dash_Btn").GetComponent<Button>().onClick.AddListener(DashSkill);
        UIManager.Instance.playerUI.transform.Find("RightDown/Btns/UltimateSkill_Btn").GetComponent<Button>().onClick.AddListener(UltimateSkill);
    }
    private void Start()
    {
        skillData.Add(Define.PlayerTransformTypeFlag.Ghost, GetComponent<GhostSkill>());
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
            skillEvent[0] = playerSkill.playerSkills[0];
            skillEvent[1] = playerSkill.playerSkills[1];
            ultimateSkill = playerSkill.UltimateSkill;
        }
    }
    void Skill1()
    {
        if (UIManager.Instance.SkillCooltime(playerBase.PlayerTransformData, 1))
            skillEvent[0](slotLevel[0]);
    }

    void Skill2()
    {
        if (UIManager.Instance.SkillCooltime(playerBase.PlayerTransformData, 2))
            skillEvent[1](slotLevel[1]);
    }
    void DashSkill()
    {
        PlayerMovement.Instance.Rb.isKinematic=false;
        rb.MovePosition(rb.position + PlayerMovement.Instance.Direction * dashDistance);
        PlayerMovement.Instance.Rb.isKinematic = true;
    }
    void UltimateSkill()
    {
        ultimateSkill();
    }
    #region ����Ʈ ����

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
