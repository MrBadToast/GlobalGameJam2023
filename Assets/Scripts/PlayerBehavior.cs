using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerBehavior : SerializedMonoBehaviour
{
    [Title("Properties")]

    [SerializeField] private float MoveSpeed;
    [SerializeField] private float JumpPower;

    [Title("Raycasts")]

    [SerializeField] Transform RCO_FootR;
    [SerializeField] Transform RCO_FootL;
    [SerializeField] float GroundedLength = 0.2f;
    [SerializeField] LayerMask groundLayer;
    
    [Title("Controls")]

    public KeyCode Key_Right;
    public KeyCode Key_Left;
    public KeyCode Key_Up;
    public KeyCode Key_Down;

    public KeyCode Key_Jump;
    public KeyCode Key_Plant;
    public KeyCode Key_ShiftControl;

    public enum ControlMode
    {
        Normal,
        Root
    }

    public ControlMode currentControlmode;
    public Rigidbody2D rbody;

    private Animator anim;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }

    float moveLerpT = 0.25f;

    private void FixedUpdate()
    {
        anim.SetBool("Grounded", IsGrounded());
        anim.SetFloat("VertSpeed", rbody.velocity.y);

        if (currentControlmode == ControlMode.Normal)
        {
            if(Input.GetKey(Key_Right))
            {
                rbody.velocity = new Vector2(Mathf.Lerp(rbody.velocity.x, MoveSpeed, moveLerpT), rbody.velocity.y);
                transform.right = Vector2.right;
                anim.SetBool("HorInput", true);
            }
            else if(Input.GetKey(Key_Left))
            {
                rbody.velocity = new Vector2(Mathf.Lerp(rbody.velocity.x, -MoveSpeed, moveLerpT), rbody.velocity.y);
                transform.right = Vector2.left;
                anim.SetBool("HorInput", true);
            }
        }

        if(!Input.GetKey(Key_Right)&&!Input.GetKey(Key_Left))
        {
            rbody.velocity = new Vector2(Vector2.Lerp(rbody.velocity, Vector2.zero, moveLerpT).x, rbody.velocity.y);
            anim.SetBool("HorInput", false);
        }
        
    }

    private void Update()
    {
        if(currentControlmode == ControlMode.Normal)
        {
            if(Input.GetKeyDown(Key_Jump) && IsGrounded())
            {
                rbody.velocity = new Vector2(rbody.velocity.x, JumpPower);
            }
        }
    }

    public bool IsGrounded()
    {
        bool footR = Physics2D.Raycast(RCO_FootR.position, Vector2.down, GroundedLength, groundLayer);
        bool footL = Physics2D.Raycast(RCO_FootL.position, Vector2.down, GroundedLength, groundLayer);

        Debug.Log("FOOTR : " + footR + " / FOOTL : " + footL);
        Debug.DrawRay(RCO_FootR.position, Vector3.down);
        Debug.DrawRay(RCO_FootL.position, Vector3.down);

        return footR || footL;
    }

}
