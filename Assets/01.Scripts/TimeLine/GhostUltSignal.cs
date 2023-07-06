using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class GhostUltSignal : MonoBehaviour
{
    [SerializeField] private GameObject GhostBoss;

    [SerializeField] private GameObject player;

    [SerializeField] private GameObject GhostBossSkill;
    //[SerializeField] private Animation GhostUltAnim;

    [SerializeField] Animator ghostUltAnim;

    [SerializeField] private SpriteRenderer panel1, panel2;

    Player playerScript;
    PlayableDirector PD;
    CanvasGroup playerPCUI;
    CanvasGroup playerPPUI;

    List<string> animArray;
    
    WaitForSecondsRealtime zerodotzeroone = new WaitForSecondsRealtime(0.01f);

    internal int index = 0;

    bool isArrayed = false;

    int ult1 = Animator.StringToHash("Ult1");
    int ult2 = Animator.StringToHash("Ult2");
    int ult3 = Animator.StringToHash("Ult3");
    float alpha = 0;
    int enemyLayer;
    private void Awake()
    {
        enemyLayer = LayerMask.NameToLayer("Enemy");
        player = GameObject.FindGameObjectWithTag("Player");
        playerPCUI = GameObject.Find("PCPlayerUI").transform.Find("UltFade").GetComponent<CanvasGroup>();
        playerPPUI = GameObject.Find("PPPlayerUI").GetComponent<CanvasGroup>();
        playerScript = player.GetComponent<Player>();
        //  UltSkillAnim(); 
    }

    //public void AnimationArray()
    //{
    //    foreach (AnimationState states in GhostUltAnim)
    //    {
    //        animArray.Add(states.name);
    //        Debug.Log(states.name);
    //    }
    //    isArrayed = true;
    //}

    public void AttackEnemy()
    {
        float ySize = Camera.main.orthographicSize * 2;
        Vector2 CamSize = new Vector2(ySize * Camera.main.aspect, ySize);
        Collider2D[] attachEnemises = Physics2D.OverlapBoxAll(transform.position, CamSize, 0, 1 << enemyLayer);
        for (int i = 0; i < attachEnemises.Length; i++)
        {
            attachEnemises[i].GetComponent<IHittable>().OnDamage(50, 0);
        }
        playerPCUI.alpha = 1;
        playerPPUI.alpha = 1;
        PlayerMovement.Instance.IsMove = true;
        PlayerMovement.Instance.IsControl = true;
        Time.timeScale = 1;
    }
    //public void UltSkillAnim()
    //{
    //    if (isArrayed == false)
    //    {
    //        animArray = new List<string>();
    //        AnimationArray();
    //    }
    //}

    
    public void ScreenDark()
    {
        StartCoroutine(ScreenDarkCor());
    }

    public void ScreenWhite()
    {
        Color color = panel1.color;
        color.a = 0;
        panel1.color = color;
        panel2.color = color;
   
    }

    public IEnumerator ScreenDarkCor()
    {
        alpha = 0f;
        Color color = panel1.color;

        while(alpha <= 1.1f)
        {
            color.a = alpha;
            yield return zerodotzeroone;
            panel1.color = color;
            panel2.color = color;
            if (alpha > 0.7f)
            {
                alpha += 0.07f;
            }
            else
            {
                alpha += 0.01f;
            }

        }

    }
    public void UltSkillCast()
    {

        PD = GetComponent<PlayableDirector>();
        PD.Play();
        Time.timeScale = 0f;
        playerPCUI.alpha = 0f;
        playerPPUI.alpha = 0f;
        PlayerMovement.Instance.IsControl = false;
        PlayerMovement.Instance.IsMove = false;
    }

    public void GhostBossTransform()
    {
        GhostBoss.transform.position = player.transform.position;
        GhostBossSkill.transform.position = new Vector3(player.transform.position.x + (-4.77f), player.transform.position.y +(8.54f), 0);
    }

    public void Ult1()
    {
        ghostUltAnim.SetTrigger(ult1);
    }

    public void Ult2()
    {
        ghostUltAnim.SetTrigger(ult2);
    }

    public void Ult3()
    {
        ghostUltAnim.SetTrigger(ult3);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(transform.position,new Vector2( Camera.main.rect.size.x + 10, Camera.main.rect.size.y + 10));
    }
}
