using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class UIWaitingForm : UGuiForm
    {
        private float m_ElapseSeconds = 0f;

        public int WorkId { get; private set; }

        public float Duration { get; private set; }

#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
        {
            base.OnInit(userData);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

            UIWaitingParams param = (UIWaitingParams)userData;
            if (param == null)
            {
                Log.Warning("WaitingParams is invalid.");
                return;
            }

            WorkId = param.WorkId;
            Duration = Mathf.Max(0.1f, param.Duration);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(bool isShutdown, object userData)
#else
        protected internal override void OnClose(bool isShutdown, object userData)
#endif
        {
            base.OnClose(isShutdown, userData);

        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#else
        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#endif
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            m_ElapseSeconds += elapseSeconds;
            
            if (m_ElapseSeconds > Duration)
            {
                m_ElapseSeconds = 0f;
                GameEntry.Event.Fire(this, WorkFinishEventArgs.Create(WorkId));
                this.Close();
            }
        }
    }
}
