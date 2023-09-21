using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFountainStatue : StatueBase
{

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

        if (GameManager.Instance.Player.playerBase.Hp <= GameManager.Instance.Player.playerBase.MaxHp * 0.6f)
            GameManager.Instance.Player.playerBase.Hp = GameManager.Instance.Player.playerBase.MaxHp;
        else
        {
            GameManager.Instance.Player.playerBase.MaxHp -= 15;

            var dropItemObj = Managers.Resource.Instantiate("Assets/03.Prefabs/2D/DropItem.prefab");
            dropItemObj.transform.position = transform.position;
            dropItemObj.GetComponent<DropItem>().SetItem(Define.ChestRating.Special);
            dropItemObj.transform.DOJump(new Vector3(transform.position.x, transform.position.y - 3f), 1, 1, 0.4f);
        }

        effectTmp.SetText("당신은 분수에 피를 흘립니다...");
        StartCoroutine(IETextAnim());
        isUseable = false;
        ToggleInteractiveTMP();
    }
}
