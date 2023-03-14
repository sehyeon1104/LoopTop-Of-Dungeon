using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRangePattern : MonoBehaviour
{
    [SerializeField] private GameObject FPR; //Floor Pattern Range
    [SerializeField] private GameObject FPRS; //Floor Pattern Range Start
    [SerializeField] private GameObject Smoke;

    public SpriteRenderer FPRSpriteRenderer;
    public SpriteRenderer FPRSSpriteRenderer;

    private WaitForSeconds WaitForRangeStart = new WaitForSeconds(2f);
    private WaitForSeconds WaitForStart = new WaitForSeconds(0.8f);
    private WaitForSeconds FPRSpeed = new WaitForSeconds(0.01f);
    private WaitForSeconds FPREnd = new WaitForSeconds(1f);

    private float ScaleX;
    private float ScaleY;

    Vector2 Vec = new Vector2(0,0);

    public static bool isAttackStart { get; set; }

    private void Start()
    {
        
        StartCoroutine(FloorPattern());
    }

    IEnumerator  FloorPattern()
    {
       

        FPR.transform.position = transform.position;
        FPRS.transform.position = transform.position;

        FPR.SetActive(true);
        FPRS.SetActive(true);

        
        yield return WaitForRangeStart; //패턴 시전 대기


        while(ScaleX < 17f)
        {
            ScaleX += 0.5f;
            ScaleY += 0.5f;

            FPR.transform.localScale = new Vector2(ScaleX, ScaleY);

            yield return FPRSpeed;
        }

        ScaleX = 0;
        ScaleY = 0;

        yield return WaitForStart;

        while (ScaleX < 17f)
        {
            ScaleX += 0.2f;
            ScaleY += 0.2f;

            FPRS.transform.localScale = new Vector2(ScaleX, ScaleY);

            yield return FPRSpeed;
        }

        for (int i = 0; i < 3; i++)
        {
            Managers.Pool.PoolManaging("10.Effects/ghost/Smoke", new Vector2(transform.position.x + 1, transform.position.y + 7.7f), Quaternion.identity);
            Managers.Pool.PoolManaging("10.Effects/ghost/Smoke", new Vector2(transform.position.x + 1, transform.position.y - 7.7f), Quaternion.Euler(new Vector3(0,0,180)));
        }

        isAttackStart = true;

        FPRSpriteRenderer.enabled = false;
        FPRSSpriteRenderer.enabled = false;

        yield return FPREnd;

        FPR.transform.localScale = Vector2.zero; 
        FPRS.transform.localScale = Vector2.zero;

        FPR.gameObject.SetActive(false);
        FPRS.gameObject.SetActive(false);

        //effect


    }

    



}
