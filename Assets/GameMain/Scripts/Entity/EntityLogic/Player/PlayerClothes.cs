using GameFramework.Event;
using StarForce;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerClothes : MonoBehaviour
{

    [SerializeField] private Animator animatorHead;
    [SerializeField] private Animator animatorArm;
    [SerializeField] private Animator animatorBody;
    [SerializeField] private Animator animatorLeg;

    [SerializeField] private AnimatorOverrideController[] headAnimControllers;
    [SerializeField] private AnimatorOverrideController[] armAnimControllers;
    [SerializeField] private AnimatorOverrideController[] bodyAnimControllers;
    [SerializeField] private AnimatorOverrideController[] legAnimControllers;

    public bool IsPause { get; private set; }
    public bool IsDead { get; private set; }

    private DataBag dataBag;

    public void OnInit(object userData)
    {
        dataBag = GameEntry.Data.GetData<DataBag>();
    }

    public void OnShow(object userData)
    {
        RefreshPlayerClothes();

        GameEntry.Event.Subscribe(ChangeClothesEventArgs.EventId, OnChangeClothes);
    }

    public void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        if (IsPause) return;

        if (IsDead) return;
    }

    public void OnHide(bool isShutdown, object userData)
    {
        GameEntry.Event.Unsubscribe(ChangeClothesEventArgs.EventId, OnChangeClothes);
    }

    public void OnPause()
    {
        IsPause = true;
    }

    public void OnResume()
    {
        IsPause = false;
    }

    public void OnDead()
    {
        IsDead = true;
    }

    private void OnChangeClothes(object sender, GameEventArgs e)
    {
        ChangeClothesEventArgs ne = (ChangeClothesEventArgs)e;
        if (ne == null)
        {
            return;
        }

        RefreshPlayerClothes();
    }

    private void RefreshPlayerClothes()
    {
        int clothes = 0;
        if (dataBag.clothes.clothesData != null)
            clothes = dataBag.clothes.clothesData.Clothes;

        ChangeArm(clothes);
        ChangeBody(clothes);
        ChangeHead(clothes);
        ChangeLeg(clothes);
    }

    private void ChangeArm(int arm)
    {
        if (arm < 0 || arm >= armAnimControllers.Length) return;

        animatorArm.runtimeAnimatorController = armAnimControllers[arm];
    }
    private void ChangeHead(int head)
    {
        if (head < 0 || head >= headAnimControllers.Length) return;

        animatorHead.runtimeAnimatorController = headAnimControllers[head];
    }
    private void ChangeBody(int body)
    {
        if (body < 0 || body >= bodyAnimControllers.Length) return;

        animatorBody.runtimeAnimatorController = bodyAnimControllers[body];
    }
    private void ChangeLeg(int leg)
    {
        if (leg < 0 || leg >= legAnimControllers.Length) return;

        animatorLeg.runtimeAnimatorController = legAnimControllers[leg];
    }
}