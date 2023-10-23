using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotoursAxe : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Rare;

    public override bool isPersitantItem => true;
    public override bool isStackItem => true;

    private Coroutine coCooltime = null;
    private Coroutine coAbility = null;
    private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    private bool isEquip = false;

    private float abilityDuration = 5f;
    private float delay = 5f;

    // 중첩수
    private int stack = 0;

    // 주변 적 수
    private int nearByEnemyCount = 0;
    // 적 체크 거리
    private float dis = 5f;

    public override void Disabling()
    {
        isEquip = false;
        GameManager.Instance.Player.playerBase.AttackRange -= GameManager.Instance.Player.playerBase.InitAttackRange * 0.1f;
    }

    public override void Init()
    {
        coCooltime = null;
        coAbility = null;
    }

    public override void Use()
    {
        GameManager.Instance.Player.playerBase.AttackRange += GameManager.Instance.Player.playerBase.InitAttackRange * 0.1f;
        LastingEffect();
    }

    public override void LastingEffect()
    {
        isEquip = true;
        delay = abilityDuration;
        coCooltime = ItemManager.Instance.StartCoroutine(CoolTime());
        coAbility = ItemManager.Instance.StartCoroutine(MinotoursAxeAbility());
    }

    public IEnumerator MinotoursAxeAbility()
    {
        while (isEquip)
        {
            if (delay >= abilityDuration)
            {
                GameManager.Instance.Player.playerBase.AttackRange -= GameManager.Instance.Player.playerBase.InitAttackRange * (0.04f * stack);

                stack = CheckNearByEnemyCount();
                stack = Mathf.Clamp(stack, 0, 5);
                delay = 0f;

                GameManager.Instance.Player.playerBase.AttackRange += GameManager.Instance.Player.playerBase.InitAttackRange * (0.04f * stack);
                UpdateStackAndTimerPanel();
            }
            yield return waitForEndOfFrame;
        }
    }

    public int CheckNearByEnemyCount()
    {
        Debug.Log("CheckNearByEnemyCount");

        if (GameManager.Instance.sceneType == Define.Scene.Center)
            return 0;

        nearByEnemyCount = 0;

        foreach(var enemy in EnemySpawnManager.Instance.curEnemies)
        {
            if (Vector3.Distance(GameManager.Instance.Player.transform.position, enemy.transform.position) < dis)
                nearByEnemyCount++;

            Debug.Log(Vector3.Distance(GameManager.Instance.Player.transform.position, enemy.transform.position));
        }

        return nearByEnemyCount;
    }

    private IEnumerator CoolTime()
    {
        while (true)
        {
            if (delay < abilityDuration)
            {
                delay += Time.deltaTime;
            }

            yield return waitForEndOfFrame;
        }
    }

    public override void UpdateStackAndTimerPanel()
    {
        base.UpdateStackAndTimerPanel();

        InventoryUI.Instance.uiInventorySlotDict[this.GetType().Name].UpdateStack(stack);
        InventoryUI.Instance.uiInventorySlotDict[this.GetType().Name].UpdateTimerPanel(abilityDuration);
    }
}
