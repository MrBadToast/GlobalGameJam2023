using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using UnityEngine.Events;

public class PlayerBehavior : SerializedMonoBehaviour
{
    [Title("Properties")]

    [SerializeField] private float MoveSpeed;
    [SerializeField] private float JumpPower;
    [SerializeField] private float StepInterval = 0.5f;

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

    [Title("Prefabs")]
    public GameObject StepParticle;

    public enum ControlMode
    {
        Normal,
        Root
    }

    [HideInInspector] public ControlMode currentControlmode;
    [HideInInspector] public Rigidbody2D rbody;
    [HideInInspector] public UnityAction OnShiftToRoot;
    [HideInInspector] public UnityAction OnShiftToNormal;

    private Animator anim;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    float moveLerpT = 0.25f;
    float stepT = 0f;

    private void FixedUpdate()
    {
        anim.SetBool("Grounded", IsGrounded());
        anim.SetFloat("VertSpeed", rbody.velocity.y);
        stepT -= Time.fixedDeltaTime;

        if (currentControlmode == ControlMode.Normal)
        {
            if (Input.GetKey(Key_Right))
            {
                rbody.velocity = new Vector2(Mathf.Lerp(rbody.velocity.x, MoveSpeed, moveLerpT), rbody.velocity.y);
                transform.right = Vector2.right;
                anim.SetBool("HorInput", true);
                TryStep();
            }
            else if(Input.GetKey(Key_Left))
            {
                rbody.velocity = new Vector2(Mathf.Lerp(rbody.velocity.x, -MoveSpeed, moveLerpT), rbody.velocity.y);
                transform.right = Vector2.left;
                anim.SetBool("HorInput", true);
                TryStep();
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
            if(Input.GetKeyDown(Key_ShiftControl))
            {
                OnShiftToRoot.Invoke();
                currentControlmode = ControlMode.Root;
                rbody.isKinematic = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(Key_ShiftControl))
            {
                OnShiftToNormal.Invoke();
                currentControlmode = ControlMode.Normal;
                rbody.isKinematic = false;
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

    private void TryStep()
    {
        if (stepT < 0f)
        {
            stepT = StepInterval;
            Instantiate(StepParticle, RCO_FootL, true);
        }
    }
}
