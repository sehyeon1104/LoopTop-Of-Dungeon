using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoSingleton<Boss>, IHittable
{
    public BossBase Base;
    public MultiGage.TargetGageValue TargetGage;

    public bool isBDamaged { set; get; } = false;
    public bool isBDead { private set; get; } = false;

    public Vector3 hitPoint { get; }

    private SpriteRenderer spriteRenderer = null;

    private void Awake()
    {
        Base = new BossBase();
        TargetGage = new MultiGage.TargetGageValue(Base.Hp);
        MultiGage.Instance.ObserveStart(TargetGage);

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        UpdateBossHP();
    }

    public IEnumerator IEHitAction()
    {
        // TODO : 피격 애니메이션
        // TODO : 피격시 받은 데미지 표시

        //StartCoroutine(CameraShaking.Instance.IECameraShakeOnce());

        spriteRenderer.color = Color.black;
        yield return new WaitForSeconds(0.01f);
        spriteRenderer.color = Color.white;

        yield return new WaitForSeconds(0.05f);
        isBDamaged = false;

        yield break;
    }

    public void Die()
    {
        if (isBDead) return;

        // StartCoroutine(CameraShaking.Instance.IECameraShakeMultiple(2f));
        MultiGage.Instance.ObserveEnd();
        UIManager.Instance.TransformUITest();

        isBDead = true;
        Debug.Log("Died!");
        //gameObject.SetActive(false);
    }

    public void OnDamage(float damage, GameObject damageDealer, float critChance)
    {
        if (isBDead) return;
        if (isBDamaged) return;

        isBDamaged = true;
        Base.Hp -= (int)damage;
        UpdateBossHP();
        Debug.Log(Base.Hp);
        StartCoroutine(IEHitAction());

        if (Base.Hp <= 0)
        {
            Die();
            return;
        }
    }

    public void UpdateBossHP()
    {
        TargetGage.value = Base.Hp;
    }
}
