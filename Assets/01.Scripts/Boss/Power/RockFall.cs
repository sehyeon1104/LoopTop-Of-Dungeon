using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFall : MonoBehaviour
{
    [SerializeField] GameObject warning;
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

        Collider2D col = Physics2D.OverlapCircle(transform.position, 2.5f, 1 << 8 | 1 << 15);
        if(col != null)
            col.GetComponent<IHittable>().OnDamage(10, 0);

        if (Rock == null)
            yield break;

        Rock.color = new Color(1, 1, 1, 1);
        TrailLight.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Rock == null)
            return;
    }
}
