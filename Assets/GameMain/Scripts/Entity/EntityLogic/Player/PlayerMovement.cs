using Cinemachine;
using StarForce;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using StarForce.Data;

public class PlayerMovement : MonoBehaviour
{
    public float speed;

    private Rigidbody2D rb2;

    private Animator animator;

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
        animator = GetComponent<Animator>();

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

        animator.SetBool(Constant.Animator.IsMoving, input != Vector2.zero);
        if (input != Vector2.zero)
        {
            stopX = inputX;
            stopY = inputY;
        }

        animator.SetFloat(Constant.Animator.InputX, stopX);
        animator.SetFloat(Constant.Animator.InputY, stopY);
    }

    public void OnHide(bool isShutdown, object userData)
    {

    }

    public void OnPause()
    {
        IsPause = true;
        animator.speed = 0;
        tempVelocity = rb2.velocity;
        rb2.velocity = Vector3.zero;
    }

    public void OnResume()
    {
        IsPause = false;
        animator.speed = 1;
        rb2.velocity = tempVelocity;
    }

    public void OnDead()
    {
        IsDead = true;
    }
}
