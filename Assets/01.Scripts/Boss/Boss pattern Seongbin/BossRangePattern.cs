using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRangePattern : MonoBehaviour
{
    [SerializeField] private GameObject FPR; //Floor Pattern Range
    [SerializeField] private GameObject FPRS; //Floor Pattern Range Start
    [SerializeField] private GameObject FPRR; //Floor Pattern Range Ractangle 
    [SerializeField] private GameObject FPRRS; //Floor Pattern Range Ractangle Start

    public SpriteRenderer FPRSpriteRenderer;
    public SpriteRenderer FPRSSpriteRenderer;
    public SpriteRenderer FPRRSpriteRenderer;
    public SpriteRenderer FPRRSSpriteRenderer;

    public GameObject FPRSCol;
    public GameObject FPRRSCol;

    private WaitForSeconds WaitForRangeStart = new WaitForSeconds(2f);
    private WaitForSeconds WaitForStart = new WaitForSeconds(0.8f);
    private WaitForSeconds AttackRangeSpeed = new WaitForSeconds(0.01f);
    private WaitForSeconds AttackEnd = new WaitForSeconds(5f);

    private float ScaleX;
    private float ScaleY;

    Vector2 Vec = new Vector2(0,0);

    public static bool isAttackStart { get; set; }

    private void Start()
    {
        StartCoroutine(FloorPatternRactangle());
        //StartCoroutine(FloorPatternCircle());
    }

    IEnumerator FloorPatternRactangle()
    {
        FPRR.transform.position = transform.position;
        FPRRS.transform.position = transform.position;

        FPRR.SetActive(true);
        FPRRS.SetActive(true);

        yield return WaitForRangeStart;

        while (ScaleX < 30f)
        {
            ScaleX += 0.5f;
            ScaleY += 0.5f;

            FPRR.transform.localScale = new Vector2(ScaleX, ScaleY);

            yield return AttackRangeSpeed;
        }

        ScaleX = 0;
        ScaleY = 0;

        yield return WaitForStart;

        while (ScaleX < 30f)
        {
            ScaleX += 0.2f;
            ScaleY += 0.2f;

            FPRRS.transform.localScale = new Vector2(ScaleX, ScaleY);

            yield return AttackRangeSpeed;
        }

        isAttackStart = true;

        
        FPRRSpriteRenderer.enabled = false;
        FPRRSSpriteRenderer.enabled = false;

        yield return AttackEnd;

        isAttackStart = false;

        FPRR.transform.localScale = Vector2.zero;
        FPRRS.transform.localScale = Vector2.zero;

        FPRR.SetActive(false);
        FPRRS.SetActive(false);

    }

    IEnumerator  FloorPatternCircle()
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

            yield return AttackRangeSpeed;
        }

        ScaleX = 0;
        ScaleY = 0;

        yield return WaitForStart;

        while (ScaleX < 17f)
        {
            ScaleX += 0.2f;
            ScaleY += 0.2f;

            FPRS.transform.localScale = new Vector2(ScaleX, ScaleY);

            yield return AttackRangeSpeed;
        }

        

        //for (int i = 0; i < 3; i++)
        //{
        //    Managers.Pool.PoolManaging("10.Effects/ghost/Smoke", new Vector2(transform.position.x + 1, transform.position.y + 7.7f), Quaternion.identity);
        //    Managers.Pool.PoolManaging("10.Effects/ghost/Smoke", new Vector2(transform.position.x + 1, transform.position.y - 7.7f), Quaternion.Euler(new Vector3(0,0,180)));
        //}

        isAttackStart = true;


        FPRSpriteRenderer.enabled = false;
        FPRSSpriteRenderer.enabled = false;

        yield return AttackEnd;

        isAttackStart = false;


        FPR.transform.localScale = Vector2.zero; 
        FPRS.transform.localScale = Vector2.zero;
        

        FPR.gameObject.SetActive(false);
        FPRS.gameObject.SetActive(false);

        


        //effect


    }

    



}
