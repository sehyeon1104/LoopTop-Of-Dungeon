using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodDonationStatue : StatueBase
{
    private int requireHp = 5;

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
        if (requireHp > GameManager.Instance.Player.playerBase.Hp)
        {
            effectTmp.SetText($"체력이 부족합니다! 요구치 : {requireHp}");
            StartCoroutine(IETextAnim());
            return;
        }

        GameManager.Instance.Player.playerBase.Hp -= requireHp;
        if(Random.Range(0, 100) < requireHp)
        {
            var dropItemObj = Managers.Resource.Instantiate("Assets/03.Prefabs/2D/DropItem.prefab");
            dropItemObj.transform.position = transform.position;
            dropItemObj.GetComponent<DropItem>().SetItem(Define.ChestRating.Special);
            dropItemObj.transform.DOJump(new Vector3(transform.position.x, transform.position.y - 3f), 1, 1, 0.4f);
        }

        effectTmp.SetText($"체력 {requireHp * 5}소모");
        StartCoroutine(IETextAnim());

        requireHp *= 2;
    }
}
