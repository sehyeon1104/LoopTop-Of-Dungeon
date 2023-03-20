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
using Random = UnityEngine.Random;

// Player Skill Class
public class PlayerSkill : MonoSingleton<PlayerSkill>
{
    [Space]
    [Header("스킬")]
    [Header("힐라패턴")]
    
    Animator animator;
    private GameObject ghostSummonerPrefab = null;
    [SerializeField] GameObject jangPanPrefab;
    [Tooltip("장판 지속시간")]
    public float jangPanTime = 3;
    [Tooltip("할퀴기 지속시간")]
    public float scratchTime = 3;
    public GameObject skillSelect;
    [SerializeField] Transform Skill1Trans;
    [SerializeField] float JangPanPersDamage = 10;
    int skillSelectNum = 0;
    private int ghostSummonCount = 1;
    List<int> randomSkillNum = new List<int>();
    public List<Define.SkillNum> skillNum = new List<Define.SkillNum>();
    int[] slotLevel = new int[2] { 1, 1};
    public Action[] skillEvent = new Action[2];
    [SerializeField]
    private int shuffleCount = 100;
    private int[] randomSkillNumArr = new int[5];
    private int randomSkilltemp = 0;
    private int randomSkilltemp2 = 0;
    PlayerBase playerBase = null;
   public PlayerSkillData skillData = null;
    private void Start()
    {
        SkillShuffle();
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerBase = new PlayerBase();
    }
    public float skillCooltime { private set; get; } = 0f;
    public void ListInit()
    {
        randomSkillNum.Clear();
        for (int i = 1; i < 6; i++)
        {
            randomSkillNum.Add(i);
        }
        // 배열 ver
        //for(int i = 0; i < randomSkillNumArr.Length; ++i)
        //{
        //    randomSkillNumArr[i] = i + 1;
        //}
    }
    public Action ApplySkill(int skillNum, int slotLevel) => skillNum switch
    {
        // Power
        1 when playerBase.PlayerTransformTypeFlag == Define.PlayerTransformTypeFlag.Power => () => HillaSkill(slotLevel),
        2 when playerBase.PlayerTransformTypeFlag == Define.PlayerTransformTypeFlag.Power => () => JangPanSkill(slotLevel),
        3 when playerBase.PlayerTransformTypeFlag == Define.PlayerTransformTypeFlag.Power => () => TeleportSkill(slotLevel),
        4 when playerBase.PlayerTransformTypeFlag == Define.PlayerTransformTypeFlag.Power => () => ArmStretchSkill(slotLevel),
        5 when playerBase.PlayerTransformTypeFlag == Define.PlayerTransformTypeFlag.Power => () => RiseUpSkill(slotLevel),
        // Ghost
        1 when playerBase.PlayerTransformTypeFlag == Define.PlayerTransformTypeFlag.Ghost => () => HillaSkill(slotLevel),
        2 when playerBase.PlayerTransformTypeFlag == Define.PlayerTransformTypeFlag.Ghost => () => JangPanSkillCor(slotLevel),
        3 when playerBase.PlayerTransformTypeFlag == Define.PlayerTransformTypeFlag.Ghost => () => TeleportSkill(slotLevel),
        4 when playerBase.PlayerTransformTypeFlag == Define.PlayerTransformTypeFlag.Ghost => () => ArmStretchSkill(slotLevel),
        5 when playerBase.PlayerTransformTypeFlag == Define.PlayerTransformTypeFlag.Ghost => () => RiseUpSkill(slotLevel),

        _ => () => Debugs(slotLevel),
    };
    public void SkillSelecet()
    {
        GameObject selectObj = EventSystem.current.currentSelectedGameObject;
        selectObj.SetActive(false);
        int selectNum = int.Parse(selectObj.GetComponentInChildren<TextMeshProUGUI>().text);
        skillNum.Add((Define.SkillNum)selectNum);
        skillEvent[skillSelectNum] = ApplySkill(selectNum, slotLevel[skillSelectNum]);
        skillSelectNum++;
        if (skillSelectNum > 1)
        {
            Time.timeScale = 1;
            skillSelectNum = 0;
            skillSelect.SetActive(false);
        }
    }
    public void Debugs(int level)
    {
        print($"디버깅{level}");
    }

    #region 리스트 셔플
    public void ListShuffle()
    {
        for (int i = 0; i < shuffleCount; i++)
        {
            int randomAt = Random.Range(1, randomSkillNum.Count);
            randomSkillNum.Remove(randomAt);
            randomSkillNum.Insert(Random.Range(0, randomSkillNum.Count + 1), randomAt);
        }

        // 배열 ver
        //for(int i = 0; i < shuffleCount; ++i)
        //{
        //    randomSkilltemp = Random.Range(0, randomSkillNumArr.Length);
        //    randomSkilltemp2 = randomSkillNumArr[i % 5];
        //    randomSkillNumArr[i % 5] = randomSkillNumArr[randomSkilltemp];
        //    randomSkillNumArr[randomSkilltemp] = randomSkilltemp2;
        //}
    }
    public void ListRemove()
    {
        int num1 = Random.Range(1, randomSkillNum.Count);
        int num2 = Random.Range(1, randomSkillNum.Count);
        while (num2 == num1)
        {
            num2 = Random.Range(1, randomSkillNum.Count);
        }
        randomSkillNum.Remove(num1);
        randomSkillNum.Remove(num2);
    }
    public void SkillShuffle()
    {
        ListInit();
        ListShuffle();
        ListRemove();
    }
    #endregion
    public void Skill1()
    {
        //if (isPDead)
        //      return;

        skillEvent[0]();
    }

    public void Skill2()
    {
        //if (isPDead)
        //    return;

        skillEvent[1]();
    }


    #region 고스트 스킬
    //스킬에 매개변수 달아서 슬롯 레벨맞춰 하기
    public void HillaSkill(int slotLevel)  //1번 스킬 힐라 스킬
    {

        //if (pBase.PlayerTransformTypeFlag != Define.PlayerTransformTypeFlag.Ghost || isPDead)
        //    return;

       skillCooltime = skillData.skill[0].skillDelay;
        animator.SetTrigger("Attack");

        for (int i = 0; i < ghostSummonCount; ++i)
        {
            Instantiate(ghostSummonerPrefab, transform.position, Quaternion.identity);
        }
    }

    public void JangPanSkill(int slotLEvel) //2번 스킬 장판 스킬 애니메이션 필요
    {
        //if (pBase.PlayerTransformTypeFlag != Define.PlayerTransformTypeFlag.Ghost || isPDead)
        //    return;

        skillCooltime = skillData.skill[1].skillDelay;
        StartCoroutine(JangPanSkillCor(jangPanTime));
    }
    public void TeleportSkill(int slotLEvel) //3번 스킬 텔레포트 패턴
    {
        skillCooltime = skillData.skill[2].skillDelay;
        StartCoroutine(TeleportPattern(scratchTime));
    }
    public void ArmStretchSkill(int slotLEvel) // 4번 스킬 팔 뻗기 스킬 
    {
        skillCooltime = skillData.skill[1].skillDelay;
    }
    public void RiseUpSkill(int slotLevel) // 5번 스킬 솟아 오르기 스킬
    {
        skillCooltime = skillData.skill[1].skillDelay;
    }
    public void UltimateSkill()
    {
        if (PlayerBase.Instance.PlayerTransformTypeFlag != Define.PlayerTransformTypeFlag.Ghost)
            return;
        Debug.Log("궁극기");

        skillCooltime = skillData.ultiSkillDelay;

    }

    IEnumerator JangPanSkillCor(float skillTime)
    {
        float timer = 0;
        float timerA = 0;
        Instantiate(jangPanPrefab, transform.position, Quaternion.identity, Skill1Trans);
        do
        {
            Collider2D[] attachObjs;

            timer += Time.deltaTime;
            timerA += Time.deltaTime;
            if (timerA > 0.1f)
            {
                attachObjs = Physics2D.OverlapCircleAll(transform.position, 2.5f);
                foreach (Collider2D c in attachObjs)
                {
                    if (c.CompareTag("Enemy") || c.CompareTag("Boss"))
                    {
                        c.GetComponent<IHittable>().OnDamage(1, gameObject, 0);
                    }
                }
                timerA = 0;

            }
            if (timer > skillTime)
            {
                Destroy(Skill1Trans.GetChild(0).gameObject);
            }
            yield return null;
        } while (timer < skillTime);

    }
    IEnumerator TeleportPattern(float skillTime)
    {
        float timer = 0;
        float timerA = 0;
        do
        {
            timer += Time.deltaTime;
            timerA += Time.deltaTime;
            if (timerA > 0.05f)
            {
                RaycastHit2D[] enemys = Physics2D.BoxCastAll(transform.position, new Vector2(2, 2), 0, Vector2.up, 2);
                foreach (RaycastHit2D c in enemys)
                {
                    if (c.collider.CompareTag("Enemy") || c.collider.CompareTag("Boss"))
                    {
                        c.collider.GetComponent<IHittable>().OnDamage(1, gameObject, 0);
                    }
                }
                timerA = 0;
            }
            yield return null;
        } while (timer < skillTime);
    }
    #endregion
}
