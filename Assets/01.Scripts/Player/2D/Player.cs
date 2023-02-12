using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoSingleton<Player> , IHittable , IAgent
{
    public PlayerBase pBase;
    int hp=3;
    private bool isPDamaged = false;
    private bool isPDead = false;

    [SerializeField]
    private float InvincibleTime = 0.2f;    // 무적시간

    public Vector3 hitPoint { get; private set; }

    public int Hp { get=>hp; 
         set=> hp=value; }
   [field:SerializeField] public UnityEvent GetHit { get; set; }
   [field:SerializeField] public UnityEvent OnDie { get; set; }

    private void Awake()
    {
        pBase = new PlayerBase();
    }
    private void Start()
    {
        hp = 3;
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

    // TODO : 적과 플레이어의 거리에 따라 피격판정

    public void Damaged(int damage)
    {
        if (isPDamaged) 
            return;

        isPDamaged = true;

        // TODO : 피격 애니메이션 재생

        pBase.Hp -= damage;
        StartCoroutine(IEDamaged());
    }

    public IEnumerator IEDamaged()
    {
        yield return new WaitForSeconds(InvincibleTime);

        isPDamaged = false;

        yield break;
    }

    public void OnDamage(int damage)
    {
       Hp-= damage;
        GetHit.Invoke();
    }
}
