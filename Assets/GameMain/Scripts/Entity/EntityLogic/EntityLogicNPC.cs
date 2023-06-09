﻿using StarForce;
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
        private Animator animator;

        protected EntityDataNPC entityDataNPC;
        private DataTask dataTask;
        private DataEvent dataEvent;
        private DataRecruit dataRecruit;
        private NPCData data;

        private bool isEnter = false;
        protected bool pause = false;

        private Task task = null;
        private Data.Event m_Event = null;
        private int dialogId = 0;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            NPCTips = transform.GetChild(0);
            animator = GetComponent<Animator>();
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
            dataEvent = GameEntry.Data.GetData<DataEvent>();
            dataRecruit = GameEntry.Data.GetData<DataRecruit>();

            task = null;
            m_Event = null;
            dialogId = 0;
    }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            if (pause) return;

            if (isEnter)
            {
                if (dialogId == 0) { return; }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (m_Event != null)
                    {
                        // 触发事件
                        dataEvent.TriggerEvent(m_Event.Id);
                    }
                    GameEntry.Event.Fire(this, OpenDialogEventArgs.Create(data.Name, data.IconId, dialogId));
                }
            }
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            entityDataNPC = null;

            task = null;
            m_Event = null;
            dialogId = 0;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(Constant.Tag.Player))
            {
                GetTaskAndDialogId(out task, out m_Event, out dialogId);

                // 无对话
                if (dialogId == 0) return;
                // 有任务 有对话
                if (task != null && dialogId != 0)
                {
                    // 任务强制执行
                    if (task.IsForce)
                    {
                        GameEntry.Event.Fire(this, OpenDialogEventArgs.Create(data.Name, data.IconId, dialogId));
                        return;
                    }
                }
                else if (m_Event != null && dialogId != 0)
                {
                    if (m_Event.IsForce)
                    {
                        dataEvent.TriggerEvent(m_Event.Id);
                        GameEntry.Event.Fire(this, OpenDialogEventArgs.Create(data.Name, data.IconId, dialogId));
                        return;
                    }
                }

                isEnter = true;
                NPCTips.gameObject.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(Constant.Tag.Player))
            {
                isEnter = false;
                NPCTips.gameObject.SetActive(false);
            }
        }

        public void Pause()
        {
            pause = true;

            animator.speed = 0;
        }

        public void Resume()
        {
            pause = false;

            animator.speed = 1;
        }

        private void GetTaskAndDialogId(out Task task, out Data.Event m_Event, out int dialog)
        {
            // 判断前置任务是否完成 未完成返回默认对话
            if (data.PreTaskId != 0)
            {
                Task preTask = dataTask.GetTask(data.PreTaskId);
                if (preTask.state == EnumTaskState.UnFinish)
                {
                    task = null;
                    m_Event = null;
                    dialog = data.DefDialogId;
                    return;
                }
            }

            // 无任务对话
            if (data.Task == string.Empty)
            {
                task = null;
                m_Event = null;
                dialog = data.DefDialogId;
                return;
            }

            string tasks = data.Task;
            string[] singleTaskConfigs = tasks.Split('|');
            foreach (string taskStr in singleTaskConfigs)
            {
                string[] configs = taskStr.Split(':');
                int type = int.Parse(configs[0]);
                int id = int.Parse(configs[1]);

                if (type == (int)EnumTriggerType.Task)
                {
                    Task curTask = dataTask.GetTask(id);

                    if (curTask.state == EnumTaskState.UnFinish)
                    {
                        task = curTask;
                        m_Event = null;
                        dialog = 0;
                        string value = dataTask.GetTaskConditionValue(task.Parameter, Constant.Parameter.Dialog);
                        if (value != string.Empty)
                            dialog = int.Parse(value);
                        return;
                    }
                }

                if (type == (int)EnumTriggerType.Event)
                {
                    Data.Event curEvent = dataEvent.GetEvent(id);
                    if (curEvent.state == EnumEventState.UnFinish || curEvent.IsRepeat)
                    {
                        task = null;
                        m_Event = curEvent;
                        dialog = 0;
                        string value = dataEvent.GetEventConditionValue(m_Event.Parameter, Constant.Parameter.Dialog);
                        if (value != string.Empty)
                            dialog = int.Parse(value);
                        return;
                    }
                }

                if (type == (int)EnumTriggerType.Recruit)
                {
                    Recruit recuruit = dataRecruit.GetRecruit(id);
                    if (recuruit.state == EnumWorkState.Applied)
                    {
                        task = null;
                        dialog = GetEventDialogId(recuruit.Apply, out m_Event);
                        return;
                    }
                    if (recuruit.state == EnumWorkState.Working)
                    {
                        task = null;
                        dialog = GetEventDialogId(recuruit.Finish, out m_Event);
                        return;
                    }
                }
            }

            task = null;
            m_Event = null;
            dialog = data.DefDialogId;
            return;
        }

        private int GetEventDialogId(string condition, out Data.Event m_Event)
        {
            int dialog = 0;
            m_Event = null;
            if (condition == string.Empty) return dialog;
            string[] conds = condition.Split('&');
            foreach (var cond in conds)
            {
                string type = cond.Split('=')[0];
                if (type != Constant.Parameter.Event) continue;

                int eventId = int.Parse(cond.Split('=')[1]);
                Data.Event applyEvent = dataEvent.GetEvent(eventId);
                if (applyEvent.EventType != (int)EnumEventType.Dialog) continue;

                string value = dataEvent.GetEventConditionValue(applyEvent.Parameter, Constant.Parameter.Dialog);
                if (value != string.Empty)
                    dialog = int.Parse(value);
                m_Event = applyEvent;
                return dialog;
            }
            return dialog;
        }
    }

}
