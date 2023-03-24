using StarForce;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Work : MonoBehaviour
{
    [SerializeField] private int workId;
    [SerializeField] private float workTime = 1f;

    private DataWork dataWork;

    // Start is called before the first frame update
    void Start()
    {
        dataWork = GameEntry.Data.GetData<DataWork>();
    }

    public void StarWork()
    {
        GameEntry.UI.OpenUIForm(UIFormId.UIWaitingForm, UIWaitingParams.Create(workId, workTime));
    }
}
