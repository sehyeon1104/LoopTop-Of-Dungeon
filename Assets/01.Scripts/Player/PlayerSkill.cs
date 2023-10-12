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

using WindowsInput;

// Player Skill Class
public class PlayerSkill : MonoSingleton<PlayerSkill>
{
    PlayerBase playerBase;
    Button interaction;
    public Dictionary<Define.PlayerTransformTypeFlag, PlayerSkillBase> skillData = new Dictionary<Define.PlayerTransformTypeFlag, PlayerSkillBase>();
    List<int> randomSkillNum = new List<int>();
    int[] slotLevel;
    [HideInInspector]
    public int[] skillIndex;
    Action[] SkillAC = new Action[5];
    Action[] itemAC = new Action[100];
    private float interactionDis = 2f;
    int itemLayer;
    GameObject skillSelectObj;
    Vector2 usuallySize = new Vector2(800, 1100);
    Vector2 enlargementSize;
    private void Awake()
    {
        playerBase = GameManager.Instance.Player.playerBase;
        itemLayer = LayerMask.NameToLayer("Item");
        slotLevel = playerBase.SlotLevel;
        skillIndex = playerBase.PlayerSkillNum;
        skillData.Add(Define.PlayerTransformTypeFlag.Power, GetComponent<PowerSkill>());
        skillData.Add(Define.PlayerTransformTypeFlag.Ghost, GetComponent<GhostSkill>());
        interaction = UIManager.Instance.GetInteractionButton();
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
        // skillSelectObj = UIManager.Instance.shopUI.skillSelect;
        enlargementSize = usuallySize * 1.1f;
        SkillSelect(playerBase.PlayerTransformTypeFlag);
    }
    private void Update()
    {
        if (WinInput.GetKeyDown(KeyCode.Escape))
        {
            if (KeyManager.Instance.keySettingUI.isChangeKey)
                return;

            if (UIManager.Instance.GetPanelStack().Count != 0)
            {
                UIManager.Instance.PopPanel();
            }
            else
            {
                UIManager.Instance.TogglePausePanel();
            }
        }

        if (playerBase.IsPDead || !PlayerMovement.Instance.IsControl || UIManager.Instance.isSetting)
            return;

        if (Input.GetKeyDown(KeySetting.keys[KeyAction.INVENTORY]))
        {
            InventoryUI.Instance.ToggleInventoryUI();
        }
        if (Input.GetKeyDown(KeySetting.keys[KeyAction.SKILL1]))
        {
            Skill1();
        }
        if (Input.GetKeyDown(KeySetting.keys[KeyAction.SKILL2]))
        {
            Skill2();
        }
        if (Input.GetKey(KeySetting.keys[KeyAction.ATTACK]))
        {
              Attack();
        }
        if(Input.GetKeyDown(/*KeyCode.F*/KeySetting.keys[KeyAction.INTERACTION]))
        {
            if (interaction.gameObject.activeSelf)
            {
                interaction.onClick?.Invoke();
                return;
            }
        }
        if (Input.GetKeyDown(/*KeyCode.K*/KeySetting.keys[KeyAction.DASH]))
            DashSkill();
        if (Input.GetKeyDown(/*KeyCode.O*/KeySetting.keys[KeyAction.ULTIMATE]))
            UltimateSkill();
        //if(Input.GetKeyDown(KeyCode.H))
        //{
        //    Collider2D[] enemies;
        //    enemies = Physics2D.OverlapCircleAll(transform.position, 3,1<<8);
        //    enemies[0]?.transform.GetComponent<Rigidbody2D>().AddForce(PlayerMovement.Instance.Direction * 40,ForceMode2D.Impulse);
        //}
    }
    public void SlotUp(int index)
    {
        if (slotLevel[index] >= 5)
            return;
        slotLevel[index]++;
        SkillSelect(playerBase.PlayerTransformTypeFlag);
       
    }
   public void SkillSelect(Define.PlayerTransformTypeFlag playerType)
    {
        ResetProperty();
        PlayerSkillBase playerSkill;
        UIManager.Instance.ResetSkill();
        if (skillData.TryGetValue(playerType, out playerSkill))
        {
            playerSkill.enabled = true;
            PlayerManager.Instance.PostPlayerTransformChangeEffects.AddListener(playerSkill.ToThisForm);
            PlayerManager.Instance.PostPlayerTransformChangeEffects.Invoke();

            SkillAC[0] = () => playerSkill.playerSkills[skillIndex[0]](slotLevel[0]);
            playerSkill.playerSkillUpdate[skillIndex[0]](slotLevel[0]);
            SkillAC[1] = () => playerSkill.playerSkills[skillIndex[1]](slotLevel[1]);
            playerSkill.playerSkillUpdate[skillIndex[1]](slotLevel[1]);
            SkillAC[2] = playerSkill.attack;
            UIManager.Instance.SetSkillIcon(playerBase.PlayerTransformData, 4, 6, 0);
            SkillAC[3] = playerSkill.ultimateSkill;
            UIManager.Instance.SetSkillIcon(playerBase.PlayerTransformData, 2, 7, 0);
            SkillAC[4] = playerSkill.dashSkill;
            UIManager.Instance.SetSkillIcon(playerBase.PlayerTransformData, 3, 8, 0);
            
        }

    }
    void ResetProperty()
    {
        for (int i = 0; i < skillData.Count; i++)
        {
            skillData[(Define.PlayerTransformTypeFlag)i].enabled = false;
            PlayerManager.Instance.PostPlayerTransformChangeEffects.RemoveListener(skillData[(Define.PlayerTransformTypeFlag)i].ToThisForm);
            PlayerManager.Instance.PrePlayerTransformChangeEffects.RemoveListener(skillData[(Define.PlayerTransformTypeFlag)i].ToOtherForm);
        }
    }
    void Skill1()
    {
        if (playerBase.IsPDead)
            return;
        if (PlayerMovement.Instance.IsControl && UIManager.Instance.SkillCooltime(playerBase.PlayerTransformData, skillIndex[0]))
            SkillAC[0]();
    }



    void Skill2()
    {
        if (playerBase.IsPDead)
            return;
        if (PlayerMovement.Instance.IsControl && UIManager.Instance.SkillCooltime(playerBase.PlayerTransformData, skillIndex[1]) )
            SkillAC[1]();
    }
    void Attack()
    {
        if (PlayerMovement.Instance.IsControl)
            SkillAC[2]();

    }
    void UltimateSkill()
    {
        if (PlayerMovement.Instance.IsControl && UIManager.Instance.SkillCooltime(playerBase.PlayerTransformData,7))
            SkillAC[3]();
    }

    void DashSkill()
    {
        if (!PlayerMovement.Instance.IsControl || !PlayerMovement.Instance.IsMove)
            return;

        if (UIManager.Instance.SkillCooltime(playerBase.PlayerTransformData, 8))
            SkillAC[4]();
    }
    #region ��ų ����

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
            int randomAt = Random.Range(1, randomSkillNum.Count +1);
            int randomAt2 = Random.Range(1, randomSkillNum.Count +1);
            while (randomAt == randomAt2)
            {
                randomAt2 = Random.Range(1, randomSkillNum.Count +1);
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
   public IEnumerator SkillShuffle()
    {
        if (!UIManager.Instance.SkillCooltime(playerBase.PlayerTransformData, skillIndex[0],true) || !UIManager.Instance.SkillCooltime(playerBase.PlayerTransformData, skillIndex[1],true))
            yield break;    

        PlayerMovement.Instance.IsControl = false;
        int index = 1;
        List<RectTransform> rectObjs = new List<RectTransform>();
        RectTransform currentObj;
        skillSelectObj.SetActive(true);
        IndexShuffle();
        PlayerSkillInfo[] playerskillInfo = playerBase.PlayerTransformData.skill;
        for (int i = 0; i < randomSkillNum.Count; i++)
        {
            Transform obj = skillSelectObj.transform.GetChild(i);
            obj.name = (randomSkillNum[i]).ToString();
            obj.transform.Find("Image/SkillName").GetComponent<TextMeshProUGUI>().text = playerskillInfo[randomSkillNum[i]].skillName;
            obj.transform.Find("Icon/IconShape/Icon").GetComponent<Image>().sprite = playerskillInfo[randomSkillNum[i]].skillIcon[0];
            obj.transform.Find("SkillExplanation/Background/SkillExplanatioText").GetComponent<TextMeshProUGUI>().text = playerskillInfo[randomSkillNum[i]].skillExplanation;
            obj.transform.Find("SkillExplanation/Background/SkillCool").GetComponent<TextMeshProUGUI>().text = $"{(playerskillInfo[randomSkillNum[i]].skillDelay)}��";
            rectObjs.Add(obj.GetComponent<RectTransform>());
        }
        currentObj = rectObjs[index];
        currentObj.sizeDelta = enlargementSize;
        Time.timeScale = 0;
        while(WaitButton() != 1)
        {
            if(WinInput.GetKeyDown(KeyCode.A)|| WinInput.GetKeyDown(KeyCode.D))
            {
                currentObj.sizeDelta = usuallySize;
                index += (int)Input.GetAxisRaw("Horizontal");
                index = Mathf.Clamp(index, 0, rectObjs.Count-1);
                currentObj = rectObjs[index];
                currentObj.sizeDelta = enlargementSize;
            }
            if(WinInput.GetKeyDown(KeyCode.Return))
            {
                skillIndex[3 - rectObjs.Count] = int.Parse(currentObj.name);
                
                rectObjs.Remove(currentObj);
                currentObj.GetComponent<Button>().onClick.Invoke();
            }
            yield return null;
        }   
        Time.timeScale = 1;
        PlayerMovement.Instance.IsControl = true;
        skillSelectObj.SetActive(false);
        SkillSelect(playerBase.PlayerTransformTypeFlag);
        FinishSelect();
    }
    public int WaitButton()
    {
        int amount = 0;
        for(int i =0; i < skillSelectObj.transform.childCount; i++ )
        {
            if(skillSelectObj.transform.GetChild(i).gameObject.activeSelf)
                amount++;
        }
        return amount;
    }
    public void FinishSelect()
    {
        for (int i = 0; i < skillSelectObj.transform.childCount; i++)
        {
            skillSelectObj.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    public void IndexShuffle()
    {
        
        ListInit();
        ListShuffle();
        ListRemove();
    }

    #endregion

}
