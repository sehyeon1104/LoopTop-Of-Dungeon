using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class GhostUltSignal : MonoBehaviour
{
    [SerializeField] private GameObject GhostBoss;

    [SerializeField] private GameObject Player;

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
        UltSkillAnim(); //이걸 스타트에 해주고 시네머신에는 몇번째 울트 애니메이션인지 넣자
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
        Debug.Log("ㅇㅇ");
        PD = GetComponent<PlayableDirector>();
        PD.Play();
    }

    public void GhostBossTransform()
    {
        GhostBoss.transform.position = Player.transform.position;
        GhostBossSkill.transform.position = new Vector3(Player.transform.position.x + (-4.77f), Player.transform.position.y +(8.54f), 0);
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
