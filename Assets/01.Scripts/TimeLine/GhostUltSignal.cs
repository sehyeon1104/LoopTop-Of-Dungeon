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

    [SerializeField] private Animation GhostUltAnim;

    [SerializeField] private SpriteRenderer panel1, panel2;
     

    PlayableDirector PD;

    List<string> animArray;

    WaitForSeconds zerodotzeroone = new WaitForSeconds(0.01f);

    internal int index = 0;

    bool isArrayed = false;

    float alpha = 0;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        UltSkillAnim(); 
    }

    public void AnimationArray()
    {
        foreach (AnimationState states in GhostUltAnim)
        {
            animArray.Add(states.name);
            Debug.Log(states.name);
        }
        isArrayed = true;
    }


    public void UltSkillAnim()
    {
        
        if (isArrayed == false)
        {
            animArray = new List<string>();
            AnimationArray();
        }
    }

    
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
        PlayerMovement.Instance.IsMove = true;
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
    }

    public void GhostBossTransform()
    {
        PlayerMovement.Instance.IsMove = false;
        GhostBoss.transform.position = player.transform.position;
        GhostBossSkill.transform.position = new Vector3(player.transform.position.x + (-4.77f), player.transform.position.y +(8.54f), 0);
    }

    public void Ult1()
    {
        GhostUltAnim.Play(animArray[0]);
    }

    public void Ult2()
    {
        GhostUltAnim.Play(animArray[1]);
    }

    public void Ult3()
    {
        GhostUltAnim.Play(animArray[2]);
    }


}
