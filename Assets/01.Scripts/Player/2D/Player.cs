using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoSingleton<Player>
{
    public PlayerBase pBase;

    private bool isPDamaged = false;
    private bool isPDead = false;

    [SerializeField]
    private float InvincibleTime = 0.2f;    // �����ð�

    private void Awake()
    {
        pBase = new PlayerBase();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Boss.Instance.isBDead)
            {
                PlayerTransformation.Instance.TransformGhost();
                Boss.Instance.gameObject.SetActive(false);
                UIManager.Instance.pressF.gameObject.SetActive(false);
            }
        }
    }

    // TODO : ���� �÷��̾��� �Ÿ��� ���� �ǰ�����

    public void Damaged(int damage)
    {
        if (isPDamaged) 
            return;

        isPDamaged = true;

        // TODO : �ǰ� �ִϸ��̼� ���

        pBase.Hp -= damage;
        StartCoroutine(IEDamaged());
    }

    public IEnumerator IEDamaged()
    {
        yield return new WaitForSeconds(InvincibleTime);

        isPDamaged = false;

        yield break;
    }
}
