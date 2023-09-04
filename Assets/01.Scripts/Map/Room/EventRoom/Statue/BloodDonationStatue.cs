using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodDonationStatue : StatueBase
{
    private int requireHp = 5;
    private Coroutine co = null;

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        button.onClick.RemoveListener(StatueFunc);
    }

    protected override void InteractiveWithPlayer()
    {
        base.InteractiveWithPlayer();
        button.onClick.RemoveListener(StatueFunc);
        button.onClick.AddListener(StatueFunc);
    }

    protected override void StatueFunc()
    {
        if (!isUseable)
            return;

        if(co != null)
            StopCoroutine(co);

        co = null;

        if (requireHp > GameManager.Instance.Player.playerBase.Hp)
        {
            effectTmp.SetText($"체력이 부족합니다! 요구치 : {requireHp}");
            co = StartCoroutine(IETextAnim());
            return;
        }

        Debug.Log(requireHp);

        GameManager.Instance.Player.playerBase.Hp -= requireHp;
        if(Random.Range(0, 100) < requireHp * 4)
        {
            var dropItemObj = Managers.Resource.Instantiate("Assets/03.Prefabs/2D/DropItem.prefab");
            dropItemObj.transform.position = transform.position;
            dropItemObj.GetComponent<DropItem>().SetItem(Define.ChestRating.Rare);
            dropItemObj.transform.DOJump(new Vector3(transform.position.x, transform.position.y - 3f), 1, 1, 0.4f);
            isUseable = false;
        }

        effectTmp.SetText($"체력 {requireHp}소모");
        co = StartCoroutine(IETextAnim());

        requireHp *= 2;
    }
}
