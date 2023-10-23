using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGroup : MonoBehaviour
{
    [SerializeField] private GameObject[] warning;
    private float waitTime = 4.5f;
    private WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();

    private void OnEnable()
    {
        transform.localScale = Vector3.one;
        StartCoroutine(OnShoot());
        if (waitTime != 4.5f && Boss.Instance.bossPattern.NowPhase == 1)
            waitTime = 4.5f;
        else if (waitTime != 0.5f && Boss.Instance.bossPattern.NowPhase == 2)
            waitTime = 0.5f;
    }

    private IEnumerator OnShoot()
    {
        float timer = 0f;
        if (waitTime == 0.5f)
        {
            Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Boss/Ghost/G_Warning.wav", Define.Sound.Effect, 0.75f, 0.5f);
            while (timer < waitTime)
            {
                transform.Rotate(Vector3.forward * Time.deltaTime * 300);
                timer += Time.deltaTime;
                yield return endOfFrame;
            }
        }
        else
            yield return new WaitForSeconds(waitTime);

        Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Boss/Ghost/G_Warning.wav", Define.Sound.Effect, 0.75f, 0.5f);
        for (int i = 0; i < warning.Length; i++)
            warning[i].SetActive(true);

        transform.localScale += Vector3.right * 0.1f;

        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < warning.Length; i++)
            warning[i].SetActive(false);

        Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Boss/Ghost/G_SoulBullet.wav", Define.Sound.Effect, 1.5f, 0.5f);
        while (transform.localScale.x >= -10)
        {
            transform.localScale -= Vector3.right * Time.deltaTime * 10f;
            yield return endOfFrame;
        }
        yield return null;
    }
}
