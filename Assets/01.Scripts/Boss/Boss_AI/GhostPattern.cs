using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPattern : BossPattern
{
    [SerializeField] private GameObject warning;
    [SerializeField] private GameObject bossMonster;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject bullet_guided;

    [SerializeField] private ParticleSystem thornFx;
    [SerializeField] private ParticleSystem SummonFx;

    private void Update()
    {
        if (Boss.Instance.Base.Hp <= Boss.Instance.Base.MaxHp * 0.4f) 
            isCanUseSpecialPattern = true; 
    }

    public override int GetRandomCount(int choisedPattern)
    {
        switch (choisedPattern)
        {
            case 0:
                return Random.Range(3, 6);
            case 1:
                return Random.Range(20, 30);
            case 2:
                break;
            case 3:
                return 10;
            case 4:
                break;
            default:
                break;
        }
        return 0;

    }

    public override IEnumerator Pattern1(int count = 0) //가시 소환 패턴
    {
        for (int i = 0; i < count; i++)
        {
            //보스 애니메이션 
            attackAnim.Play(animArray[1]);

            GameObject clone = Instantiate(warning, player.position, Quaternion.Euler(Vector3.zero));
            yield return new WaitForSeconds(1f);

            thornFx.transform.position = clone.transform.position;
            thornFx.Play();
            Managers.Sound.Play("SoundEffects/Test.wav");

            Destroy(clone);
        }

        attackCoroutine = null;
    }

    public override IEnumerator Pattern2(int count = 0) //탄막 발사 패턴
    {
        float angle = 360 / count;

        //애니메이션 적용
        attackAnim.Play(animArray[0]);

        for (int i = 0; i < count; i++)
        {
            Instantiate(bullet, transform.position, Quaternion.Euler(Vector3.forward * angle * i));
            yield return new WaitForSeconds(0.1f);
        }

        attackCoroutine = null;
    }

    public override IEnumerator Pattern3(int count = 0) //텔레포트 패턴
    {
        moveSpeed *= 0.5f;
        float timer = 0f;
        Vector3 dir;


        while (timer <= 3f)
        {
            timer += Time.deltaTime;
            yield return null;

            dir = player.position - transform.position;
            transform.Translate(dir.normalized * Time.deltaTime * moveSpeed);
        }

        transform.position = player.forward + player.position;
        moveSpeed *= 2f; 

        yield return new WaitForSeconds(1f);

        dir = player.position - transform.position;
        float rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float angle = 7.2f;

        //애니메이션 추가
        attackAnim.Play(animArray[2]);

        for (int i = -4; i < 4; i++)
        {
            Instantiate(bullet_guided, transform.position, Quaternion.Euler(Vector3.forward * (angle * i + rot * 0.5f)));
        }

        attackCoroutine = null;
    }

    public override IEnumerator Pattern4(int count = 0) //힐라 패턴
    {
        int finalCount = 0;
        List<GameObject> mobList = new List<GameObject>();

        for (int i = 0; i < count; i++)
        {
            GameObject clone = Instantiate(bossMonster, new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)), Quaternion.Euler(Vector3.zero));
            GameObject pattern33 = Instantiate(SummonFx.gameObject, clone.transform.position, Quaternion.Euler(Vector3.zero));

            ParticleSystem particle = pattern33.GetComponent<ParticleSystem>();

            particle.Play();

            mobList.Add(clone);
        }

        yield return new WaitForSeconds(10f);

        foreach (var mob in mobList)
        {
            if (mob != null)
            {
                finalCount++;
                Destroy(mob);
            }

        }
        Boss.Instance.Base.Hp += finalCount * 10;
        mobList.Clear();

        attackCoroutine = null;
    }
}
