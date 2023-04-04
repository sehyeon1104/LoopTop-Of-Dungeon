using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine;

public class GhostBossJangpanPattern : MonoBehaviour
{
    [SerializeField]  GameObject Effect;

    [HideInInspector] GameObject FPR; //Floor Pattern Range
    [HideInInspector] GameObject FPRS; //Floor Pattern Range Start
    [HideInInspector] GameObject FRPR; //Floor Pattern Range Rectangle 
    [HideInInspector] GameObject FRPRS; //Floor Pattern Range Rectangle Start
    [HideInInspector] GameObject FPRSCol;
    [HideInInspector] GameObject FPRRSCol;
    
    [HideInInspector] public SpriteRenderer FPRSpriteRenderer;
    [HideInInspector] public SpriteRenderer FPRSSpriteRenderer;
    [HideInInspector] public SpriteRenderer FPRRSpriteRenderer;
    [HideInInspector] public SpriteRenderer FPRRSSpriteRenderer;


    private WaitForSeconds WaitzerodoteightS = new WaitForSeconds(0.8f);
    private WaitForSeconds WaitzerodotzerooneS = new WaitForSeconds(0.01f);
    private WaitForSeconds WaitfiveS = new WaitForSeconds(5f);
    private WaitForSeconds WaitzerodotfiveS = new WaitForSeconds(0.5f);

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
        
        while (ScaleX < 30f)
        {
            ScaleX += 0.5f;
            ScaleY += 0.5f;

            FRPR.transform.localScale = new Vector2(ScaleX, ScaleY);

            yield return WaitzerodotzerooneS;
        }

        yield return WaitzerodotfiveS;

        ScaleX = 0;
        ScaleY = 0;

        while (ScaleX < 30f)
        {
            ScaleX += 0.2f;
            ScaleY += 0.2f;

            FRPRS.transform.localScale = new Vector2(ScaleX, ScaleY);

            yield return WaitzerodotzerooneS;
        }

        ScaleX = 0;
        ScaleY = 0;
                
        FPRRSpriteRenderer.enabled = false;
        FPRRSSpriteRenderer.enabled = false;

        Poolable clone =  Managers.Pool.PoolManaging("10.Effects/ghost/Smoke", transform.position, Quaternion.identity);
        clone.transform.position = new Vector2(3.31f, (-6.98f));
        Poolable clone1 = Managers.Pool.PoolManaging("10.Effects/ghost/Smoke", transform.position, Quaternion.identity);
        clone1.transform.position = new Vector2((-9.27f),6.45f);
        Poolable clone2 = Managers.Pool.PoolManaging("10.Effects/ghost/Smoke", transform.position, Quaternion.identity);
        clone2.transform.position = new Vector2(2.85f, 19.42f);
        Poolable clone3 = Managers.Pool.PoolManaging("10.Effects/ghost/Smoke", transform.position, Quaternion.identity);
        clone3.transform.position = new Vector2(15.36f, 29.75f);
        Poolable clone4 = Managers.Pool.PoolManaging("10.Effects/ghost/Smoke", transform.position, Quaternion.identity);
        clone4.transform.position = new Vector2(27.06f, 18.4f);
        Poolable clone5 = Managers.Pool.PoolManaging("10.Effects/ghost/Smoke", transform.position, Quaternion.identity);
        clone5.transform.position = new Vector2(38.35f,6.91f);
        Poolable clone6 = Managers.Pool.PoolManaging("10.Effects/ghost/Smoke", transform.position, Quaternion.identity);
        clone6.transform.position = new Vector2(28.04f,(-6.74f));
        Poolable clone7 = Managers.Pool.PoolManaging("10.Effects/ghost/Smoke", transform.position, Quaternion.identity);
        clone7.transform.position = new Vector2(14.72f, (-17.88f));


        //clone.transform.localScale = 

        yield return WaitzerodoteightS;

        FPRRSCol.SetActive(true);


        yield return WaitfiveS;

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
       
        while(ScaleX < 17f)
        {
            ScaleX += 0.5f;
            ScaleY += 0.5f;

            FPR.transform.localScale = new Vector2(ScaleX, ScaleY);

            yield return WaitzerodotzerooneS;
        }

        ScaleX = 0;
        ScaleY = 0;
        yield return WaitzerodotfiveS;


        while (ScaleX < 17f)
        {
            ScaleX += 0.2f;
            ScaleY += 0.2f;

            FPRS.transform.localScale = new Vector2(ScaleX, ScaleY);

            yield return WaitzerodotzerooneS;
        }
               
        ScaleX = 0;
        ScaleY = 0;

        

        FPRSpriteRenderer.enabled = false;
        FPRSSpriteRenderer.enabled = false;

        Managers.Pool.PoolManaging("10.Effects/ghost/Smoke", transform.position, Quaternion.identity);

        yield return WaitzerodoteightS;
        FPRSCol.SetActive(true);

        yield return WaitfiveS;

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
