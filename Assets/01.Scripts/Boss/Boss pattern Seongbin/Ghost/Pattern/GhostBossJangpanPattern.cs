using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBossJangpanPattern : MonoBehaviour
{
    [SerializeField] GameObject Effect;
    [SerializeField]  GameObject FPR; //Floor Pattern Range
    [SerializeField]  GameObject FPRS; //Floor Pattern Range Start
    [SerializeField]  GameObject FRPR; //Floor Pattern Range Rectangle 
    [SerializeField]  GameObject FRPRS; //Floor Pattern Range Rectangle Start
    [SerializeField]  GameObject FPRSCol;
    [SerializeField]  GameObject FPRRSCol;
    
    public SpriteRenderer FPRSpriteRenderer;
    public SpriteRenderer FPRSSpriteRenderer;
    public SpriteRenderer FPRRSpriteRenderer;
    public SpriteRenderer FPRRSSpriteRenderer;

    private WaitForSeconds WaitForRangeStart = new WaitForSeconds(2f);
    private WaitForSeconds WaitForStart = new WaitForSeconds(0.8f);
    private WaitForSeconds AttackRangeSpeed = new WaitForSeconds(0.01f);
    private WaitForSeconds AttackEnd = new WaitForSeconds(5f);

    private float ScaleX;
    private float ScaleY;
    private void Awake()
    {
        FPR = Effect.transform.Find("FPR").gameObject;
        FRPR =Effect.transform.Find("FRPR").gameObject;
        FPRS = Effect.transform.Find("FPRS").gameObject;
        FRPRS = Effect.transform.Find("FRPRS").gameObject;
        FPRSCol = Effect.transform.Find("CircleCol").gameObject;
        FPRRSCol = Effect.transform.Find("RacCol").gameObject;
        FPRSpriteRenderer = FPR.GetComponent<SpriteRenderer>();
        FPRSSpriteRenderer = FPRS.GetComponent<SpriteRenderer>();
        FPRRSpriteRenderer = FRPR.GetComponent<SpriteRenderer>();
        FPRRSSpriteRenderer = FRPRS.GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
       
        print(FPRRSCol.name);
    }
    public IEnumerator FloorPatternRectangle()
    {
        
        FRPR.transform.position = transform.position;
        FRPRS.transform.position = transform.position;
        FPRRSCol.transform.position = transform.position;

        FRPR.SetActive(true);
        FRPRS.SetActive(true);
        
        yield return WaitForRangeStart;

        while (ScaleX < 30f)
        {
            ScaleX += 0.5f;
            ScaleY += 0.5f;

            FRPR.transform.localScale = new Vector2(ScaleX, ScaleY);

            yield return AttackRangeSpeed;
        }

        ScaleX = 0;
        ScaleY = 0;

        yield return WaitForStart;

        while (ScaleX < 30f)
        {
            ScaleX += 0.2f;
            ScaleY += 0.2f;

            FRPRS.transform.localScale = new Vector2(ScaleX, ScaleY);

            yield return AttackRangeSpeed;
        }
        ScaleX = 0;
        ScaleY = 0;

        
        FPRRSpriteRenderer.enabled = false;
        FPRRSSpriteRenderer.enabled = false;

        FPRRSCol.SetActive(true);


        yield return AttackEnd;

        ScaleX = 0;
        ScaleY = 0;

        FRPR.transform.localScale = Vector2.zero;
        FRPRS.transform.localScale = Vector2.zero;

        FPRRSpriteRenderer.enabled = true;
        FPRRSSpriteRenderer.enabled = true;

        FRPR.SetActive(false);
        FRPRS.SetActive(false);
        FPRRSCol.SetActive(false);
    }

    public IEnumerator  FloorPatternCircle()
    {

        FPR.transform.position = transform.position;
        FPRS.transform.position = transform.position;
        FPRSCol.transform.position = transform.position;

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
               
        ScaleX = 0;
        ScaleY = 0;

        FPRSpriteRenderer.enabled = false;
        FPRSSpriteRenderer.enabled = false;

        Managers.Pool.PoolManaging("10.Effects/ghost/Smoke", transform.position, Quaternion.identity);
        FPRSCol.SetActive(true);

        yield return AttackEnd;


        FPR.transform.localScale = Vector2.zero; 
        FPRS.transform.localScale = Vector2.zero;

        FPRSpriteRenderer.enabled = true;
        FPRSSpriteRenderer.enabled = true;


        FPR.gameObject.SetActive(false);
        FPRS.gameObject.SetActive(false);
        FPRSCol.gameObject.SetActive(false);

        //effect


    }

    



}
