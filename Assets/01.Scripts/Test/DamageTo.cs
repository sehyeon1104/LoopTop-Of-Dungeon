using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTo : MonoBehaviour
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

        Collider2D col = Physics2D.OverlapBox(transform.position + new Vector3(0.25f, -1f), new Vector2(3.5f, 2.5f), 0, 1<<8);
        if(col != null)
            GameManager.Instance.Player.OnDamage(1, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + new Vector3(0.25f, -1.75f), new Vector2(5.5f, 2f));
    }
}
