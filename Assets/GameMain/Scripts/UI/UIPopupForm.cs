using DG.Tweening;
using GameFramework;
using GameFramework.Event;
using GameFramework.ObjectPool;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class TipsObject : ObjectBase
    {
        public static TipsObject Create(object target)
        {
            TipsObject tipsObject = ReferencePool.Acquire<TipsObject>();
            tipsObject.Initialize(target);
            return tipsObject;
        }

        protected override void Release(bool isShutdown)
        {
            GameObject target = (GameObject)Target;
            if (target == null)
            {
                return;
            }
            Object.Destroy(target);
        }

        protected override void OnUnspawn()
        {
            base.OnUnspawn();
            GameObject target = (GameObject)Target;
            if (target == null)
            {
                return;
            }
            target.SetActive(false);
            target.transform.localPosition = Vector3.zero;
        }
    }

    public class UIPopupForm : UGuiForm
    {
        [SerializeField]
        private int m_InstancePoolCapacity = 16;

        [SerializeField]
        private GameObject tipsTemplete;

        [SerializeField]
        private Transform tipsRoot;

        private IObjectPool<TipsObject> m_TipsObjectPool = null;
        private Dictionary<string, GameObject> dicTips = null;
        private int waitCount = 0;  // 等待显示提示数量

#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
        {
            base.OnInit(userData);

            m_TipsObjectPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<TipsObject>("Tips", m_InstancePoolCapacity);
            dicTips = new Dictionary<string, GameObject>();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

            GameEntry.Event.Subscribe(ShowTipsEventArgs.EventId, OnShowTips);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(bool isShutdown, object userData)
#else
        protected internal override void OnClose(bool isShutdown, object userData)
#endif
        {
            base.OnClose(isShutdown, userData);

            GameEntry.Event.Unsubscribe(ShowTipsEventArgs.EventId, OnShowTips);

            foreach (string key in dicTips.Keys)
            {
                if (DOTween.IsTweening(key))
                {
                    DOTween.Kill(key, true);
                    m_TipsObjectPool.Unspawn(dicTips[key]);
                }
            }
            dicTips.Clear();
            waitCount = 0;
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#else
        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#endif
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        private void OnShowTips(object sender, GameEventArgs e)
        {
            ShowTipsEventArgs ne = (ShowTipsEventArgs)e;
            if (ne == null)
            {
                return;
            }

            float timer = 0;
            Tween t = DOTween.To(() => timer, x => timer = x, 1, waitCount * 0.5f)
                      .OnStepComplete(() =>
                      {
                          waitCount -= 1;
                          if (waitCount < 0)
                          {
                              waitCount = 0;
                          }

                          GameObject tips = null;
                          TipsObject tipsOjbect = m_TipsObjectPool.Spawn();
                          if (tipsOjbect != null)
                          {
                              tips = (GameObject)tipsOjbect.Target;
                              tips.SetActive(true);
                          }
                          else
                          {
                              tips = Instantiate(tipsTemplete);
                              tips.transform.SetParent(tipsRoot);
                              tips.transform.localPosition = Vector3.zero;
                              tips.name = "Tips_" + m_TipsObjectPool.Count;
                              m_TipsObjectPool.Register(TipsObject.Create(tips), true);
                          }

                          Text content = tips.GetComponentInChildren<Text>();
                          content.text = ne.Content;
                          string tweenId = tips.name;

                          tips.transform.DOLocalMoveY(600, 2).SetId(tweenId).OnComplete(
                              () => {
                                  dicTips.Remove(tweenId);
                                  m_TipsObjectPool.Unspawn(tips);
                              });
                          dicTips.Add(tweenId, tips);
                      });
            waitCount++;
        }
    }
}
