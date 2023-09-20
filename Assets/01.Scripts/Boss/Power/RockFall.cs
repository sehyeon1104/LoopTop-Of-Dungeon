using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFall : MonoBehaviour
{
    [SerializeField] GameObject warning;
    [SerializeField] GameObject boxWarning;
    [SerializeField] SpriteRenderer Rock;
    [SerializeField] ParticleSystem particle;
    [SerializeField] GameObject TrailLight;

    WaitForSeconds waitTime = new WaitForSeconds(0.5f);

    private void OnEnable()
    {
        if (Rock != null)
        {
            Rock.color = new Color(1, 1, 1, 0);
            TrailLight.SetActive(false);
            boxWarning.SetActive(false);
            GetComponent<BoxCollider2D>().enabled = false;
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

        yield return new WaitUntil(() => GetComponent<BoxCollider2D>().enabled == true);

        boxWarning.SetActive(true);
        Vector3 dirToBoss = (Boss.Instance.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dirToBoss.y, dirToBoss.x) * Mathf.Rad2Deg;

        boxWarning.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Rock == null)
            return;
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 15)
            collision.GetComponent<IHittable>().OnDamage(15, 0);

    }
}
