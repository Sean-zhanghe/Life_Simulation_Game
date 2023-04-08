using Cinemachine;
using StarForce;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using StarForce.Data;
using static StarForce.Constant;
using UnityEngine;
using Animator = UnityEngine.Animator;

public class PlayerMovement : MonoBehaviour
{
    public float speed;

    private Rigidbody2D rb2;

    [SerializeField] private Animator animatorHead;
    [SerializeField] private Animator animatorArm;
    [SerializeField] private Animator animatorBody;
    [SerializeField] private Animator animatorLeg;

    private float inputX, inputY;

    private float stopX, stopY = -1;

    private PolygonCollider2D polygon;

    private CinemachineVirtualCamera virtualCamera;

    private CinemachineConfiner virtualConfig;

    private Vector3 tempVelocity;

    public bool IsPause { get; private set; }
    public bool IsDead { get; private set; }


    public void OnInit(object userData)
    {
        rb2 = GetComponent<Rigidbody2D>();

        IsPause = false;
        IsDead = false;
    }

    public void OnShow(object userData)
    {
        polygon = GameObject.FindGameObjectWithTag(Constant.Tag.CameraBorder).GetComponent<PolygonCollider2D>();
        virtualCamera = GameObject.FindGameObjectWithTag(Constant.Tag.VirtualCamera).GetComponent<CinemachineVirtualCamera>(); ;
        virtualConfig = GameObject.FindGameObjectWithTag(Constant.Tag.VirtualCamera).GetComponent<CinemachineConfiner>(); ;

        if (virtualCamera != null)
            virtualCamera.Follow = transform;

        if (virtualConfig != null && polygon)
            virtualConfig.m_BoundingShape2D = polygon;
    }

    public void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        if (IsPause) return;

        if (IsDead) return;

        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        Vector2 input = (transform.right * inputX + transform.up * inputY).normalized;
        rb2.velocity = input * speed;

        SetAnimatorBool(Constant.Animator.IsMoving, input != Vector2.zero);
        if (input != Vector2.zero)
        {
            stopX = inputX;
            stopY = inputY;
        }

        SetAnimatorFloat(Constant.Animator.InputX, stopX);
        SetAnimatorFloat(Constant.Animator.InputY, stopY);
    }

    public void OnHide(bool isShutdown, object userData)
    {

    }

    public void OnPause()
    {
        IsPause = true;
        SetAnimatorSpeed(0);
        tempVelocity = rb2.velocity;
        rb2.velocity = Vector3.zero;
    }

    public void OnResume()
    {
        IsPause = false;
        SetAnimatorSpeed(1);
        rb2.velocity = tempVelocity;
    }

    public void OnDead()
    {
        IsDead = true;
    }

    private void SetAnimatorSpeed(int value)
    {
        animatorHead.speed = value;
        animatorArm.speed = value;
        animatorBody.speed = value;
        animatorLeg.speed = value;
    }

    private void SetAnimatorBool(string parameters, bool value)
    {
        animatorHead.SetBool(parameters, value);
        animatorArm.SetBool(parameters, value);
        animatorBody.SetBool(parameters, value);
        animatorLeg.SetBool(parameters, value);
    }

    private void SetAnimatorFloat(string parameters, float value)
    {
        animatorHead.SetFloat(parameters, value);
        animatorArm.SetFloat(parameters, value);
        animatorBody.SetFloat(parameters, value);
        animatorLeg.SetFloat(parameters, value);
    }
}
