using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    readonly int attackHash = Animator.StringToHash("Attack");
    Animator animator;
    Rigidbody2D rigid;
    [SerializeField]
    float speed = 3f;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        transform.Translate(new Vector3(horizontal * Time.deltaTime * speed, 0, 0));
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigid.AddForce(Vector2.up*10,ForceMode2D.Impulse);
        }
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger(attackHash);
        }
    }
}
