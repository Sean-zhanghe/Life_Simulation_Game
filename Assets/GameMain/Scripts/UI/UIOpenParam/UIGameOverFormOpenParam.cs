//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using StarForce.Data;

namespace StarForce
{

    public class UIGameOverFormOpenParam : IReference
    {
        public LevelData LevelData
        {
            get;
            private set;
        }

        public EnumGameOverType EnumGameOverType
        {
            get;
            private set;
        }

        public UIGameOverFormOpenParam()
        {
            LevelData = null;
            EnumGameOverType = EnumGameOverType.Fail;
        }

        public static UIGameOverFormOpenParam Create(LevelData levelData, EnumGameOverType enumGameOverType)
        {
            UIGameOverFormOpenParam uIGameOverFormOpenParam = ReferencePool.Acquire<UIGameOverFormOpenParam>();
            uIGameOverFormOpenParam.LevelData = levelData;
            uIGameOverFormOpenParam.EnumGameOverType = enumGameOverType;
            return uIGameOverFormOpenParam;
        }

        public void Clear()
        {
            LevelData = null;
            EnumGameOverType = EnumGameOverType.Fail;
        }
    }
}
