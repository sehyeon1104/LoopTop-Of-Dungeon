using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoSingleton<Boss>
{
    public BossBase Base;

    public bool isDamaged { private set; get; } = false;
    public bool isDead { private set; get; } = false;

    private SpriteRenderer spriteRenderer = null;

    private void Awake()
    {
        Base = new BossBase();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Hit(int damage)
    {
        if (isDead) return;
        if (isDamaged) return;


        isDamaged = true;
        Base.Hp -= damage;
        Debug.Log(Base.Hp);
        StartCoroutine(IEHitAction());
        StartCoroutine(CameraShaking.Instance.IECameraShakeOnce());

        if(Base.Hp <= 0)
        {
            Die();
            return;
        }
    }
    
    public IEnumerator IEHitAction()
    {
        // TODO : �ǰ� �ִϸ��̼�
        // TODO : �ǰݽ� ���� ������ ǥ��

        spriteRenderer.color = Color.black;
        yield return new WaitForSeconds(0.01f);
        spriteRenderer.color = Color.white;

        yield return new WaitForSeconds(0.05f);
        isDamaged = false;

        yield break;
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("Died!");
        gameObject.SetActive(false);
    }
}
