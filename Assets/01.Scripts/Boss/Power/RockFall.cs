using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFall : MonoBehaviour
{
    [SerializeField] private GameObject warning;
    [SerializeField] private GameObject boxWarning;
    [SerializeField] private SpriteRenderer Rock;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private GameObject TrailLight;

    private BoxCollider2D col;

    WaitForSeconds waitTime = new WaitForSeconds(0.5f);

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();

        Boss.Instance.GetComponent<PowerPattern>()?.RockMoveEvent.RemoveListener(() => StartCoroutine(RockMove()));
        Boss.Instance.GetComponent<PowerPattern>()?.RockMoveEvent.AddListener(() => StartCoroutine(RockMove()));
    }
    private void OnEnable()
    {
        if (Rock != null)
        {
            Rock.color = new Color(1, 1, 1, 0);
            TrailLight.SetActive(false);
            boxWarning.SetActive(false);
            col.enabled = false;
        }
        StartCoroutine(DmgTo());
    }

    private IEnumerator DmgTo()
    {
        warning.SetActive(true);
        yield return waitTime;
        warning.SetActive(false);

        particle.Play();
        CinemachineCameraShaking.Instance.CameraShake(3, 0.2f);
        Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Boss/Power/P_GroundShock.wav");

        Collider2D col = Physics2D.OverlapCircle(transform.position, transform.lossyScale.x * 2.5f, 1 << 8 | 1 << 15);
        if(col != null)
            col.GetComponent<IHittable>().OnDamage(10, 0);

        if (Rock == null)
            yield break;

        Rock.color = new Color(1, 1, 1, 1);
        TrailLight.SetActive(true);
    }

    private IEnumerator RockMove()
    {
        boxWarning.SetActive(true);
        Vector3 dirToBoss = (Boss.Instance.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dirToBoss.y, dirToBoss.x) * Mathf.Rad2Deg;

        boxWarning.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        yield return new WaitForSeconds(1f);

        col.enabled = true;
        transform.DOMove(Boss.Instance.transform.position, 0.2f);

        CinemachineCameraShaking.Instance.CameraShake(8f, 0.1f);
        Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Boss/Power/P_ColumnDestroy.wav");

        yield return new WaitForSeconds(0.2f);
        
        col.enabled = false;
        
        CinemachineCameraShaking.Instance.CameraShake(10f, 0.2f);
        Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Boss/Power/P_ColumnDestroy.wav");

        yield return new WaitForEndOfFrame();
        Managers.Pool.Push(GetComponent<Poolable>());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Rock == null)
            return;
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 15)
            collision.GetComponent<IHittable>().OnDamage(15, 0);

    }
}
