using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class AgentInput : MonoSingleton<AgentInput>,IAgentInput
{
    [SerializeField]
    Joystick _joyStick;
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
       
    }
    public void ButtonClick()
    {
        GameObject clickedObj = EventSystem.current.currentSelectedGameObject;
        if (clickedObj.CompareTag("Attack"))
        {
            Attack.Invoke();
        }
        else if(clickedObj.CompareTag("Skill1"))
        {
            Skill1.Invoke();
        }
        else if(clickedObj.CompareTag("Skill2"))
        {
            skill2.Invoke();
        }
        else
        {
            ultimateSkill.Invoke();
        }
    }
    private void FixedUpdate()
    {
        GetMovementInputMove();
    }
    public void GetMovementInputMove()
    {
        MovementInput.Invoke(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
        MovementInput.Invoke(new Vector2(_joyStick.Horizontal, _joyStick.Vertical));
    }
}
