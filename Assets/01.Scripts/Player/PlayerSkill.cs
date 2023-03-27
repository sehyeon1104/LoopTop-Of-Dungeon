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
    [Space]
    [Header("스킬")]

    Dictionary<PlayerSkillBase,Action> skillDictionary= new Dictionary<PlayerSkillBase, Action>();

    Animator animator; 
    public GameObject skillSelect;
    [SerializeField] Transform Skill1Trans;
    int skillSelectNum = 0;
    private int ghostSummonCount = 1;
    List<int> randomSkillNum = new List<int>();
    public List<Define.SkillNum> skillNum = new List<Define.SkillNum>();
    int[] slotLevel = new int[2] { 1, 1};
    public Action[] skillEvent = new Action[2];
    private void Update()
     {
         if (Input.GetKeyDown(KeyCode.O))
        {
            //SkillSelecet();
        }
     }
    private void Start()
    {
        SkillShuffle();
        foreach(int i  in randomSkillNum)
        {
            print(i);
        }
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        //SkillToButton();
    }
 
    public void Skill1()
    {
        UIManager.Instance.SkillCooltime(PlayerTransformation.Instance.GetPlayerData(GameManager.Instance.Player.playerBase.PlayerTransformTypeFlag), randomSkillNum[0]);
        skillEvent[0]();
    }

    public void Skill2()
    {
        UIManager.Instance.SkillCooltime(PlayerTransformation.Instance.GetPlayerData(GameManager.Instance.Player.playerBase.PlayerTransformTypeFlag), randomSkillNum[1]);
        skillEvent[1]();
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
            int randomAt = Random.Range(1, randomSkillNum.Count+1);
            int randomAt2 = Random.Range(1, randomSkillNum.Count+1);
            while(randomAt == randomAt2)
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
        randomSkillNum.RemoveRange(3,2);
    }
    public void SkillShuffle()
    {
        ListInit();
        ListShuffle();
        ListRemove();
    }

    #endregion
}
