using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AgentInput : MonoSingleton<AgentInput>,IAgentInput
{
    public UnityEvent Skill1;
    public UnityEvent skill2;
    public UnityEvent ultimateSkill;
    [field:SerializeField] public UnityEvent Attack { get; set; }
    [field:SerializeField] public UnityEvent SkillFirst { get; set; }
    [field:SerializeField] public UnityEvent<Vector2> MovementInput { get; set; }

   
    void Update()   
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
           SkillFirst.Invoke();
        }
        GetMovementInputMove();
    }

    void GetMovementInputMove()
    {
        MovementInput.Invoke(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
    }
}
