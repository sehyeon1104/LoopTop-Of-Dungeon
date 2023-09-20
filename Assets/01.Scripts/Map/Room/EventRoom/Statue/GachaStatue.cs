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
        // TODO : 33% Ȯ���� ���� ��ȭȹ��, ������ ȹ��, ������ ����
        if (!isUseable)
            return;
        isUseable = false;
        UIManager.Instance.RotateAttackButton();

        if (GameManager.Instance.Player.playerBase.Hp <= 2)
        {
            Debug.Log("ü�� ����");
            // TODO : ü�� ���� UI ǥ�� �� ���� ����
        }

        rand = 1;//Random.Range(0, 3);

        // ��ȭ ȹ��
        if(rand == 0)
        {
            effectTmp.text = "��ȭ ȹ��!";
            FragmentCollectManager.Instance.DropFragmentByCircle(GameManager.Instance.Player.gameObject, 8);
        }
        // ������ ���� ���
        else if(rand == 1)
        {
            effectTmp.text = "�����ۻ��� ���!";
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
        // ������ ����
        else if(rand == 2)
        {
            effectTmp.text = "2�� ����!";
            GameManager.Instance.Player.OnDamage(2, 0);
        }

        StartCoroutine(IETextAnim());
        ToggleInteractiveTMP();
    }
}
