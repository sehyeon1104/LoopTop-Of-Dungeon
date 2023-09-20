using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GachaStatue : StatueBase
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

    int rand = 0;
    protected override void StatueFunc()
    {
        // TODO : 33% 확률로 각각 재화획득, 아이템 획득, 데미지 입음
        if (!isUseable)
            return;
        isUseable = false;
        UIManager.Instance.RotateAttackButton();

        if (GameManager.Instance.Player.playerBase.Hp <= 2)
        {
            Debug.Log("체력 부족");
            // TODO : 체력 부족 UI 표시 및 사운드 실행
        }

        rand = 1;//Random.Range(0, 3);

        // 재화 획득
        if(rand == 0)
        {
            effectTmp.text = "재화 획득!";
            FragmentCollectManager.Instance.DropFragmentByCircle(GameManager.Instance.Player.gameObject, 8);
        }
        // 아이템 상자 드랍
        else if(rand == 1)
        {
            effectTmp.text = "아이템상자 드랍!";
            GameObject chestObj = Managers.Resource.Instantiate("Assets/03.Prefabs/Chest.prefab");
            chestObj.transform.position = new Vector3(transform.position.x, transform.position.y - 5f);
            CinemachineCameraShaking.Instance.CameraShake(3, 0.1f);
            Chest chest = chestObj.GetComponent<Chest>();

            int chestRand = Random.Range(0, 10);

            // common : 40%
            if (chestRand < 4)
                chest.SetChestRating(Define.ChestRating.Common);
            // rare : 30%
            else if (chestRand >= 4 && chestRand < 7)
                chest.SetChestRating(Define.ChestRating.Rare);
            // epic : 20%
            else if (chestRand >= 7 && chestRand < 9)
                chest.SetChestRating(Define.ChestRating.Epic);
            // legendary : 10%
            else if (chestRand == 9)
                chest.SetChestRating(Define.ChestRating.Legendary);
        }
        // 데미지 입음
        else if(rand == 2)
        {
            effectTmp.text = "2의 피해!";
            GameManager.Instance.Player.OnDamage(2, 0);
        }

        StartCoroutine(IETextAnim());
        ToggleInteractiveTMP();
    }
}
