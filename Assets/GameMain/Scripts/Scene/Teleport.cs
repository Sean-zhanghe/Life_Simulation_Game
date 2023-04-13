using StarForce;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

public class Teleport : MonoBehaviour
{
    [SerializeField] private EnumScene to;
    [SerializeField] private int preTaskId;
    [SerializeField] private int propertyId;

    private DataTask dataTask;
    private DataBag dataBag;
    private DataProperty dataProperty;

    private void Start()
    {
        dataTask = StarForce.GameEntry.Data.GetData<DataTask>();
        dataBag = StarForce.GameEntry.Data.GetData<DataBag>();
        dataProperty = StarForce.GameEntry.Data.GetData<DataProperty>();
    }

    public void TeleportToScene()
    {
        // 无前置任务和物品直接跳转
        if (preTaskId == 0 && propertyId == 0)
        {
            StarForce.GameEntry.Event.Fire(this, LoadGameSceneEventArgs.Create((int)to));
            return;
        }
        if (preTaskId != 0)
        {
            // 前置任务
            Task preTask = dataTask.GetTask(preTaskId);
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
        }

        if (propertyId != 0)
        {
            // 检查背包
            if (!dataBag.FindItemIsInBag(EnumBag.Property, propertyId))
            {
                PropertyData property = dataProperty.GetPropertyDataById(propertyId);
                if (property == null) return;
                string message = string.Format(StarForce.GameEntry.Localization.GetString(Constant.Localization.BagNoItem), property.Name);
                StarForce.GameEntry.UI.OpenTips(new DialogParams()
                {
                    Mode = 1,
                    Title = StarForce.GameEntry.Localization.GetString(Constant.Localization.BagTitle),
                    Message = message,
                    UserData = null
                });
                return;
            }

        }

        StarForce.GameEntry.Event.Fire(this, LoadGameSceneEventArgs.Create((int)to));
    }
}
