using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine;

public class GhostBossJangpanPattern : MonoBehaviour
{
    [SerializeField]  GameObject Effect;

    private SpriteRenderer FRPRSpriterenderer;
    private SpriteRenderer FRPRStartSpriterenderer;
    


    private WaitForSeconds WaitzerodoteightS = new WaitForSeconds(0.8f);
    private WaitForSeconds WaitzerodotzerooneS = new WaitForSeconds(0.01f);
    private WaitForSeconds WaitfiveS = new WaitForSeconds(5f);
    private WaitForSeconds WaitzerodotfiveS = new WaitForSeconds(0.5f);

    private float ScaleX;
    private float ScaleY;
    public IEnumerator FloorPatternRectangle()
    {
        Poolable FRPR = Managers.Pool.PoolManaging("10.Effects/ghost/FRPR",transform.position, Quaternion.identity);
        Poolable FRPRS = Managers.Pool.PoolManaging("10.Effects/ghost/FRPR",transform.position, Quaternion.identity);
        Poolable FRPRCol = Managers.Pool.PoolManaging("10.Effects/ghost/RecCol", transform.position, Quaternion.identity);

        FRPRSpriterenderer = FRPR.GetComponent<SpriteRenderer>();
        FRPRStartSpriterenderer = FRPRS.GetComponent<SpriteRenderer>();


        FRPR.transform.position = transform.position;
        FRPRS.transform.position = transform.position;
        FRPRCol.transform.position = transform.position;

        FRPR.gameObject.SetActive(true);
        FRPRS.gameObject.SetActive(true);
        
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
        
        

        FRPRSpriterenderer.enabled = false;
        FRPRStartSpriterenderer.enabled = false;
        Poolable clone =  Managers.Pool.PoolManaging("10.Effects/ghost/Smoke", transform.position, Quaternion.identity);
        clone.transform.position = new Vector2(transform.position.x + (-11.26f), transform.position.y + (-12.18f));
        Poolable clone1 = Managers.Pool.PoolManaging("10.Effects/ghost/Smoke", transform.position, Quaternion.identity);
        clone1.transform.position = new Vector2(transform.position.x + (-23.73f), transform.position.y);
        Poolable clone2 = Managers.Pool.PoolManaging("10.Effects/ghost/Smoke", transform.position, Quaternion.identity);
        clone2.transform.position = new Vector2(transform.position.x + (-11.26f), transform.position.y + 12.18f);
        Poolable clone3 = Managers.Pool.PoolManaging("10.Effects/ghost/Smoke", transform.position, Quaternion.identity);
        clone3.transform.position = new Vector2(transform.position.x, transform.position.y + 24);
        Poolable clone4 = Managers.Pool.PoolManaging("10.Effects/ghost/Smoke", transform.position, Quaternion.identity);
        clone4.transform.position = new Vector2(transform.position.x + 11.26f, transform.position.y + 12.18f);
        Poolable clone5 = Managers.Pool.PoolManaging("10.Effects/ghost/Smoke", transform.position, Quaternion.identity);
        clone5.transform.position = new Vector2(transform.position.x + 23.73f, transform.position.y);
        Poolable clone6 = Managers.Pool.PoolManaging("10.Effects/ghost/Smoke", transform.position, Quaternion.identity);
        clone6.transform.position = new Vector2(transform.position.x + 11.26f, transform.position.y + (-12.18f));
        Poolable clone7 = Managers.Pool.PoolManaging("10.Effects/ghost/Smoke", transform.position, Quaternion.identity);
        clone7.transform.position = new Vector2(transform.position.x, transform.position.y + (-23.73f));


        //clone.transform.localScale = 

        yield return WaitzerodoteightS;

        FRPRCol.gameObject.SetActive(true);


        yield return WaitfiveS;

        ScaleX = 0;
        ScaleY = 0;

        FRPR.transform.localScale = Vector2.zero;
        FRPRS.transform.localScale = Vector2.zero;

        FRPRSpriterenderer.enabled = true;
        FRPRStartSpriterenderer.enabled = true;

        FRPR.gameObject.SetActive(false);
        FRPRS.gameObject.SetActive(false);
        FRPRCol.gameObject.SetActive(false);

        Managers.Pool.Push(FRPR);
        Managers.Pool.Push(FRPRS);
        Managers.Pool.Push(FRPRCol);

    }

    public IEnumerator  FloorPatternCircle()
    {

        Poolable FPR = Managers.Pool.PoolManaging("03.Prefabs/BossPattern/Ghost/JangPan/Circle/FPR", transform.position, Quaternion.identity);
        Poolable FPRS = Managers.Pool.PoolManaging("10.Effects/ghost/FPRS", transform.position, Quaternion.identity);


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
