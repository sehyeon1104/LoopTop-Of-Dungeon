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
    int[] slotLevel;
    Action[] skillEvent = new Action[5];
    private float interactionDis = 2f;
    private void Awake()
    {
        playerBase = GameManager.Instance.Player.playerBase;
        slotLevel = playerBase.slotLevel;
        skillData.Add(Define.PlayerTransformTypeFlag.Power, GetComponent<PowerSkill>());
        skillData.Add(Define.PlayerTransformTypeFlag.Ghost, GetComponent<GhostSkill>());
        if (UIManager.Instance.skill1Button != null)
        {
            UIManager.Instance.skill1Button.GetComponent<Button>().onClick.AddListener(Skill1);
            UIManager.Instance.skill2Button.GetComponent<Button>().onClick.AddListener(Skill2);
            UIManager.Instance.dashButton.GetComponent<Button>().onClick.AddListener(DashSkill);
            UIManager.Instance.ultButton.GetComponent<Button>().onClick.AddListener(UltimateSkill);
            UIManager.Instance.playerUI.transform.Find("RightDown/Btns/AttackBtn").GetComponent<Button>().onClick.AddListener(Attack);
        }
    }
    private void Start()
    {
        SkillSelect(playerBase.PlayerTransformTypeFlag);

        SkillShuffle();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            slotLevel[0]++;
            playerBase.PlayerTransformTypeFlag = Define.PlayerTransformTypeFlag.Ghost;
            playerBase.PlayerTransformData = playerBase.PlayerTransformDataSOList[(int)playerBase.PlayerTransformTypeFlag];
            PlayerVisual.Instance.UpdateVisual(playerBase.PlayerTransformData);
            SkillSelect(playerBase.PlayerTransformTypeFlag);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            Skill1();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            Skill2();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (UIManager.Instance.GetInteractionButton().gameObject.activeSelf == true)
            {
                UIManager.Instance.GetInteractionButton().onClick.Invoke();
            }
            else
            {
                Attack();
            }
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            DashSkill();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            UltimateSkill();
        }
        if(Input.GetKeyDown(KeyCode.O))
        {
            Collider2D[] itemDis = Physics2D.OverlapCircleAll(transform.position, interactionDis);
           
        }
    }

    void SkillSelect(Define.PlayerTransformTypeFlag playerType)
    {
        ReserProperty();
        PlayerSkillBase playerSkill;
        UIManager.Instance.ResetSkill();
        if (skillData.TryGetValue(playerType, out playerSkill))
        {
            playerSkill.enabled = true;
            skillEvent[0] = () => playerSkill.playerSkills[4](slotLevel[0]);
            playerSkill.playerSkillUpdate[4](slotLevel[0]);
            skillEvent[1] = () => playerSkill.playerSkills[5](slotLevel[0]);
            playerSkill.playerSkillUpdate[5](slotLevel[0]);
            skillEvent[2] = playerSkill.attack;
            skillEvent[3] = playerSkill.ultimateSkill;
            UIManager.Instance.SetSkillIcon(playerBase.PlayerTransformData, 2, 6, 0);
            skillEvent[4] = playerSkill.dashSkill;
            UIManager.Instance.SetSkillIcon(playerBase.PlayerTransformData, 3, 7, 0);
        }
    }
    void ReserProperty()
    {
        for (int i = 0; i < skillData.Count; i++)
        {
            skillData[(Define.PlayerTransformTypeFlag)i].enabled = false;
        }
    }
    void Skill1()
    {
        if (UIManager.Instance.SkillCooltime(playerBase.PlayerTransformData, Define.SkillNum.FirstSkill) && PlayerMovement.Instance.IsControl)
            skillEvent[0]();
    }



    void Skill2()
    {
        if (UIManager.Instance.SkillCooltime(playerBase.PlayerTransformData, Define.SkillNum.SecondSkill) && PlayerMovement.Instance.IsControl)
            skillEvent[1]();
    }
    void Attack()
    {
        if (PlayerMovement.Instance.IsControl)
            skillEvent[2]();
    }
    void UltimateSkill()
    {
        if (UIManager.Instance.SkillCooltime(playerBase.PlayerTransformData, Define.SkillNum.UltimateSkill) && PlayerMovement.Instance.IsControl)
            skillEvent[3]();
    }

    void DashSkill()
    {
        if (!PlayerMovement.Instance.IsControl || !PlayerMovement.Instance.IsMove)
            return;

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
