using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFall : MonoBehaviour
{
    [SerializeField] GameObject warning;
    WaitForSeconds waitTime = new WaitForSeconds(0.5f);
    ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }
    private void OnEnable()
    {
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
    }
}
