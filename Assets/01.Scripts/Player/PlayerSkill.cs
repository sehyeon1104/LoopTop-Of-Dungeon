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
    [Header("��ų")]
    [Header("��������")]
    Dictionary<int,Action> skillDictionary= new Dictionary<int,Action>();
    Animator animator;
    private GameObject ghostSummonerPrefab = null;
    [SerializeField] GameObject jangPanPrefab;
    [Tooltip("���� ���ӽð�")]
    public float jangPanTime = 3;
    [Tooltip("������ ���ӽð�")]
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
    private int shuffleCount = 20;
    PlayerBase playerBase;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SkillSelecet();
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
        playerBase = GameManager.Instance.Player.playerBase;
        animator = GetComponent<Animator>();
        SkillToButton();
    }
    public float skillCooltime { private set; get; } = 0f;
 
    public void SkillToButton()
    {
        GameObject.FindGameObjectWithTag("Skill1").GetComponent<Button>().onClick.AddListener(Skill1);
        GameObject.FindGameObjectWithTag("Skill2").GetComponent<Button>().onClick.AddListener(Skill2);
        GameObject.FindGameObjectWithTag("UltimateSkill").GetComponent<Button>().onClick.AddListener(UltimateSkill);
    }
    public Action ApplySkill(int skillNum, int slotLevel) => skillNum switch
    {
        // Power
        1 when playerBase.PlayerTransformTypeFlag == Define.PlayerTransformTypeFlag.Power => () => HillaSkill(),
        2 when playerBase.PlayerTransformTypeFlag == Define.PlayerTransformTypeFlag.Power => () => JangPanSkill(),
        3 when playerBase.PlayerTransformTypeFlag == Define.PlayerTransformTypeFlag.Power => () => TeleportSkill(),
        4 when playerBase.PlayerTransformTypeFlag == Define.PlayerTransformTypeFlag.Power => () => ArmStretchSkill(),
        5 when playerBase.PlayerTransformTypeFlag == Define.PlayerTransformTypeFlag.Power => () => RiseUpSkill(),
        // Ghost
        1 when playerBase.PlayerTransformTypeFlag == Define.PlayerTransformTypeFlag.Ghost => () => HillaSkill(),
        2 when playerBase.PlayerTransformTypeFlag == Define.PlayerTransformTypeFlag.Ghost => () => JangPanSkill(),
        3 when playerBase.PlayerTransformTypeFlag == Define.PlayerTransformTypeFlag.Ghost => () => TeleportSkill(),
        4 when playerBase.PlayerTransformTypeFlag == Define.PlayerTransformTypeFlag.Ghost => () => ArmStretchSkill(),
        5 when playerBase.PlayerTransformTypeFlag == Define.PlayerTransformTypeFlag.Ghost => () => RiseUpSkill(),

        _ => () => Debugs(slotLevel),
    };
    public float GetSkillDelay(int i) => playerBase.SkillData.skill[i-1].skillDelay;

    public void SkillSelecet()
    {
        GameObject selectObj = EventSystem.current.currentSelectedGameObject;
        selectObj.SetActive(false);
        int selectNum = int.Parse(selectObj.GetComponentInChildren<TextMeshProUGUI>().text);
        skillNum.Add((Define.SkillNum)selectNum);
        skillEvent[skillSelectNum] = ApplySkill(selectNum, slotLevel[skillSelectNum]);
        skillDictionary.Add(slotLevel[skillSelectNum], ApplySkill(selectNum, slotLevel[skillSelectNum])); 
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
        print($"�����{level}");
    }

    public void Skill1()
    {
        UIManager.Instance.SkillCooltime(playerBase.SkillData, randomSkillNum[0]);
        skillEvent[0]();
    }

    public void Skill2()
    {
        UIManager.Instance.SkillCooltime(playerBase.SkillData, randomSkillNum[1]);
        skillEvent[1]();
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

    #region ���Ʈ ��ų
    //��ų�� �Ű����� �޾Ƽ� ���� �������� �ϱ�
    public void HillaSkill()  //1�� ��ų ���� ��ų
    {

        //if (pBase.PlayerTransformTypeFlag != Define.PlayerTransformTypeFlag.Ghost || isPDead)
        //    return;
        GetSkillDelay(1);
       skillCooltime = playerBase.SkillData.skill[0].skillDelay;
        animator.SetTrigger("Attack");

        for (int i = 0; i < ghostSummonCount; ++i)
        {
            Instantiate(ghostSummonerPrefab, transform.position, Quaternion.identity);
        }
    }

    public void JangPanSkill() //2�� ��ų ���� ��ų �ִϸ��̼� �ʿ�
    {
        //if (pBase.PlayerTransformTypeFlag != Define.PlayerTransformTypeFlag.Ghost || isPDead)
        //    return;
        GetSkillDelay(2);
        StartCoroutine(JangPanSkillCor(jangPanTime));
    }
    public void TeleportSkill() //3�� ��ų �ڷ���Ʈ ����
    {
        GetSkillDelay(1);
        StartCoroutine(TeleportPattern(scratchTime));
    }
    public void ArmStretchSkill() // 4�� ��ų �� ���� ��ų 
    {
        GetSkillDelay(1);
    }
    public void RiseUpSkill() // 5�� ��ų �ھ� ������ ��ų
    {
        GetSkillDelay(1);
    }
    public void UltimateSkill()
    {
        if (playerBase.PlayerTransformTypeFlag != Define.PlayerTransformTypeFlag.Ghost)
            return;
        Debug.Log("�ñر�");

        skillCooltime = playerBase.SkillData.ultiSkillDelay;

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
