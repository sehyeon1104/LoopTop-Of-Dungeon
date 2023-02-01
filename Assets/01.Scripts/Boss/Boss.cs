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
        if (isDamaged) return;

        isDamaged = true;
        Base.Hp -= damage;
        Debug.Log(Base.Hp);
        StartCoroutine(IEHitAction());
        StartCoroutine(CameraShaking.Instance.IECameraShakeOnce());
    }
    
    public IEnumerator IEHitAction()
    {
        // TODO : 피격 애니메이션
        // TODO : 피격시 받은 데미지 표시

        yield return new WaitForSeconds(0.05f);
        isDamaged = false;

        yield break;
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        gameObject.SetActive(false);
    }
}
