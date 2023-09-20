using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column : MonoBehaviour, IHittable
{
    [SerializeField] private SpriteRenderer sprite;

    private bool isDie = false;

    private GameObject warning;
    private GameObject shockWave;
    private Animator columnAnim;

    private WaitForSeconds waitTime = new WaitForSeconds(2.5f);
    private WaitForSeconds delay = new WaitForSeconds(1.5f);

    private int hashDisappear = Animator.StringToHash("Disappear");

    private static int nowBossPhase = 1;

    public Vector3 hitPoint => throw new System.NotImplementedException();
    private Material hitMat;
    private Material defaultMat;

    private void Awake()
    {
        warning = transform.Find("Warning").gameObject;
        shockWave = transform.Find("ShockWave").gameObject;
        columnAnim = GetComponentInChildren<Animator>();

        hitMat = new Material(Managers.Resource.Load<Material>("Assets/12.ShaderGraph/Mat/HitMat.mat"));
        defaultMat = sprite.material;
    }

    private void OnEnable()
    {
        StopCoroutine(Attack());

        isDie = false;
        nowBossPhase = Boss.Instance.bossPattern.NowPhase;

        warning.SetActive(false);
        shockWave.SetActive(false);

        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        for(int i = 0; i < 3; i++)
        {
            yield return waitTime;
            shockWave.SetActive(false);
            warning.SetActive(true);

            yield return delay;
            warning.SetActive(false);

            shockWave.SetActive(true);
            Collider2D col = Physics2D.OverlapCircle(transform.position, 4.5f, 1 << 8);
            if (col != null) GameManager.Instance.Player.OnDamage(10, 0);
        }
        yield return waitTime;

        columnAnim.SetTrigger(hashDisappear);

        yield return delay;
        Managers.Pool.Push(GetComponent<Poolable>());
    }

    private IEnumerator StartDestroy()
    {
        yield return null;

        hitMat.SetTexture("_Texture2D", sprite.sprite.texture);
        sprite.material = hitMat;
        warning.SetActive(false);

        switch (nowBossPhase)
        {
            case 1:
                yield return new WaitForSeconds(0.5f);

                Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Boss/Power/P_ColumnDestroy.wav");
                CinemachineCameraShaking.Instance.CameraShake(6, 0.2f);
                Managers.Pool.PoolManaging("Assets/10.Effects/power/ColumnShock.prefab", transform.position, Quaternion.identity);

                Collider2D col = Physics2D.OverlapCircle(transform.position, 4.5f, 1 << 8 | 1 << 15);
                if (col != null && col.gameObject != this) col.GetComponent<IHittable>().OnDamage(10, 0);
                break;
            case 2:
                int randomAngle = Random.Range(0, 2);
                int angle = randomAngle * 45;

                CinemachineCameraShaking.Instance.CameraShake(8, 0.1f);
                for (int i = 0; i < 2; i++)
                {
                    Managers.Pool.PoolManaging("Assets/10.Effects/power/ColumnPieceWarning.prefab", transform.position, Quaternion.Euler(Vector3.forward * (angle + 90 * i)));
                }
                Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Boss/Power/P_ColumnDestroy.wav");
                yield return new WaitForSeconds(0.5f);
                CinemachineCameraShaking.Instance.CameraShake(6, 0.2f);
                for (int i = 0; i < 4; i++)
                {
                    Managers.Pool.PoolManaging("Assets/10.Effects/power/ColumnPiece.prefab", transform.position, Quaternion.Euler(Vector3.forward * (angle + 90 * i)));
                }
                break;
        }
        yield return new WaitForSeconds(0.2f);
        sprite.material = defaultMat;
        Managers.Pool.Push(GetComponent<Poolable>());

    }

    public void OnDamage(float damage, float critChance = 0, Poolable hitEffect = null)
    {
        if (isDie) return;
        
        isDie = true;
        StopCoroutine(Attack());
        StartCoroutine(StartDestroy());
    }
}
