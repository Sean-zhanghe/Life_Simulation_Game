using StarForce;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

public class Teleport : MonoBehaviour
{
    [SerializeField] private EnumScene to;
    [SerializeField] private int PreTaskId;

    private DataTask dataTask;

    private void Start()
    {
        dataTask = StarForce.GameEntry.Data.GetData<DataTask>();
    }

    public void TeleportToScene()
    {
        // 无前置任务直接跳转
        if (PreTaskId == -1)
        {
            StarForce.GameEntry.Event.Fire(this, LoadGameSceneEventArgs.Create((int)to));
        }

        Task preTask = dataTask.GetTask(PreTaskId);
        if (preTask == null)
        {
            Log.Error("Can not get pretask");
            return;
        }

        if (preTask.state != EnumTaskState.Finish)
        {
            string message = string.Format(StarForce.GameEntry.Localization.GetString(Constant.Localization.TipsPreTaskUnfinish), preTask.Description);
            StarForce.GameEntry.UI.OpenTips(new DialogParams()
            {
                Mode = 1,
                Title = StarForce.GameEntry.Localization.GetString(Constant.Localization.TipsTaskTitle),
                Message = message,
                UserData = null
            });
            return;
        }
        StarForce.GameEntry.Event.Fire(this, LoadGameSceneEventArgs.Create((int)to));
    }
}
