using StarForce;
using StarForce.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class EntityLogicNPC : EntityLogicEx, IPause
    {
        private Transform NPCTips;

        protected EntityDataNPC entityDataNPC;
        private DataTask dataTask;
        private NPCData data;

        private bool isEnter = false;
        protected bool pause = false;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            NPCTips = transform.GetChild(0);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            entityDataNPC = userData as EntityDataNPC;
            if (entityDataNPC == null)
            {
                Log.Error("Entity tower '{0}' tower data invaild.", Id);
                return;
            }
            data = entityDataNPC.data;
            dataTask = GameEntry.Data.GetData<DataTask>();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            if (pause) return;

            if (isEnter)
            {
                if (data.DefDialogId == 0) { return; }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    int dialogId = GetDialogId();
                    GameEntry.Event.Fire(this, OpenDialogEventArgs.Create(data.Name, data.IconId, dialogId));
                }
            }
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            entityDataNPC = null;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (data.DefDialogId == 0) { return; }

            if (collision.CompareTag(Constant.Tag.Player))
            {
                isEnter = true;
                NPCTips.gameObject.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (data.DefDialogId == 0) { return; }

            if (collision.CompareTag(Constant.Tag.Player) && data.DefDialogId != 0)
            {
                isEnter = false;
                NPCTips.gameObject.SetActive(false);
            }
        }

        public void Pause()
        {
            pause = true;
        }

        public void Resume()
        {
            pause = false;
        }

        private int GetDialogId()
        {

            // 判断前置任务是否完成 未完成返回默认对话
            if (data.PreTaskId != 0)
            {
                Task task = GetTask(data.PreTaskId);
                if (task.state == EnumTaskState.UnFinish)
                {
                    return data.DefDialogId;
                }
            }

            // 无任务对话
            if (data.Task == string.Empty)
            {
                return data.DefDialogId;
            }

            string tasks = data.Task;
            string[] singleTaskConfigs = tasks.Split('|');
            foreach (string taskStr in singleTaskConfigs)
            {
                string[] configs = taskStr.Split(':');
                int index = int.Parse(configs[0]);
                int taskId = int.Parse(configs[1]);
                int dialogId = int.Parse(configs[2]);

                Task task = GetTask(taskId);
                if (task.state == EnumTaskState.UnFinish)
                {
                    return dialogId;
                }
            }

            return data.DefDialogId;
        }

        private Task GetTask(int taskId)
        {
            TaskData taskData = dataTask.GetTaskData(taskId);
            Task result = null;
            switch (taskData.TaskType)
            {
                case (int)EnumTaskType.MainTask:
                    result = dataTask.GetMainTask(taskData.Id);
                    break;
                case (int)EnumTaskType.RandomTask:
                    result = dataTask.GetRandomTask(taskData.Id);
                    break;
                default:
                    break;
            }
            return result;
        }

    }

}
