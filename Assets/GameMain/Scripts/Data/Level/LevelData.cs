using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;

namespace StarForce.Data
{
    public sealed class LevelData
    {
        private DRLevel dRLevel;

        public int Id { get { return dRLevel.Id; } }

        public string Name { get { return dRLevel.Name; } }

        public int SceneId { get { return dRLevel.SceneId; } }

        public string Reward { get { return dRLevel.Reward; } }

        public LevelData(DRLevel dRLevel)
        {
            this.dRLevel = dRLevel;
        }
    }
}