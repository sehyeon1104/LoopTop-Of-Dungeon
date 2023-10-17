using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorOfDawn : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Set;

    public override bool isPersitantItem => true;

    public override bool isStackItem => true;

    private float delay = 1f;
    private float abilityDuration = 1f;
    private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    public override void Disabling()
    {
        GameManager.Instance.Player.playerBase.Attack -= GameManager.Instance.Player.playerBase.InitAttack * 0.3f;
        GameManager.Instance.Player.playerBase.AttackRange -= GameManager.Instance.Player.playerBase.InitAttackRange * 0.3f;
        GameManager.Instance.Player.playerBase.AttackSpeed -= GameManager.Instance.Player.playerBase.InitAttackSpeed * 0.3f;
        GameManager.Instance.Player.playerBase.SkillCoolDown -= 20;

        EnemyManager.Instance.EnemyDamagedRelatedItemEffects.RemoveListener(MirrorOfDawnAbility);
    }

    public override void LastingEffect()
    {
        EnemyManager.Instance.EnemyDamagedRelatedItemEffects.RemoveListener(MirrorOfDawnAbility);
        EnemyManager.Instance.EnemyDamagedRelatedItemEffects.AddListener(MirrorOfDawnAbility);
        
        delay = abilityDuration;
        ItemManager.Instance.StartCoroutine(CoolTime());
    }

    public override void Init()
    {
    }

    public override void Use()
    {
        Debug.Log("여명의 거울 준비 완료");

        GameManager.Instance.Player.playerBase.Attack += GameManager.Instance.Player.playerBase.InitAttack * 0.3f;
        GameManager.Instance.Player.playerBase.AttackRange += GameManager.Instance.Player.playerBase.InitAttackRange * 0.3f;
        GameManager.Instance.Player.playerBase.AttackSpeed += GameManager.Instance.Player.playerBase.InitAttackSpeed * 0.3f;
        GameManager.Instance.Player.playerBase.SkillCoolDown += 20;
        LastingEffect();
    }

    public void MirrorOfDawnAbility(Vector3 pos)
    {
        Debug.Log("적 타격");
        if(Random.Range(0,10) == 0 && delay >= abilityDuration)
        {
            delay = 0;
            Managers.Pool.PoolManaging("Assets/10.Effects/player/@Item/Eclipse.prefab", pos, Quaternion.identity);
            ItemManager.Instance.StartCoroutine(StartMirrorAttack(pos));
        }
    }

    public IEnumerator StartMirrorAttack(Vector3 pos)
    {
        for (int i = 0; i < 10; i++)
        {
            CinemachineCameraShaking.Instance.CameraShake(5, 0.1f);
            Collider2D[] cols = Physics2D.OverlapCircleAll(pos, 5.5f, 1 << 9);
            for (int j = 0; j < cols.Length; j++)
            {
                cols[j].GetComponent<IHittable>().OnDamage(5 * (GameManager.Instance.Player.playerBase.Attack * 0.1f));
            }
            yield return new WaitForSeconds(0.1f);
        }
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

    public override void ShowStack()
    {
        base.ShowStack();

        // InventoryUI.Instance.uiInventorySlotDic[this.GetType().Name].UpdateStack(stack);
        InventoryUI.Instance.uiInventorySlotDict[this.GetType().Name].UpdateTimerPanel(abilityDuration);
    }
}
