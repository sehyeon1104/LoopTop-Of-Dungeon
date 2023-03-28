using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
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
    [Header("스킬")]
    List<PlayerSkillBase> SkillBase; 
    Dictionary<Define.PlayerTransformTypeFlag, PlayerSkillBase> skillData = new Dictionary<Define.PlayerTransformTypeFlag, PlayerSkillBase>();
    List<int> randomSkillNum = new List<int>();

    int[] slotLevel = new int[2] { 1, 1 };
    public Action<int>[] skillEvent = new Action<int>[2];
    private void Awake()
    { 
        playerBase = GameManager.Instance.Player.playerBase;
        skillData.Add(Define.PlayerTransformTypeFlag.Ghost, GetComponent<GhostSkill>());    
        UIManager.Instance.playerUI.transform.Find("RightDown/Btns/Skill1_Btn").GetComponent<Button>().onClick.AddListener(Skill1);
        UIManager.Instance.playerUI.transform.Find("RightDown/Btns/Skill2_Btn").GetComponent<Button>().onClick.AddListener(Skill2);
    }
    private void Start()
    {
        SkillShuffle();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            SkillSelect();
        }
    }
    void SkillSelect()
    {
        PlayerSkillBase playerSkill;
        if(skillData.TryGetValue(playerBase.PlayerTransformTypeFlag,out playerSkill))
        {
            skillEvent[0] = playerSkill.playerSkills[0];
        }
    }
    public void Skill1()
    {
        skillEvent[0](slotLevel[0]);
    }

    public void Skill2()
    {
        skillEvent[1](slotLevel[1]);
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 1.5f);
    }
}
