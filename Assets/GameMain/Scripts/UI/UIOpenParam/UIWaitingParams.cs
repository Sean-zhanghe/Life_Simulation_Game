//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;

namespace StarForce
{
    /// <summary>
    /// 对话框显示数据。
    /// </summary>
    public class UIWaitingParams : IReference
    {
        public int WorkId { get; private set; }

        public float Duration { get; private set; }

        public UIWaitingParams()
        {
            WorkId = 0;
            Duration = 1;
        }

        public static UIWaitingParams Create(int workId, float duration)
        {
            UIWaitingParams uIWaitingParams = ReferencePool.Acquire<UIWaitingParams>();
            uIWaitingParams.WorkId = workId;
            uIWaitingParams.Duration = duration;
            return uIWaitingParams;
        }

        public void Clear()
        {
            WorkId = 0;
            Duration = 1;
        }
    }
}
