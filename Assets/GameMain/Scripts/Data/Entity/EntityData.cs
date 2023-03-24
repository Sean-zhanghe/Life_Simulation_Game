
namespace StarForce.Data
{
    public sealed class EntityData
    {
        private DREntity dREntity;
        private EntityGroupData entityGroupData;

        public int Id
        {
            get
            {
                return dREntity.Id;
            }
        }
        public string Name
        {
            get
            {
                return dREntity.AssetName;
            }
        }

        public EntityGroupData EntityGroupData
        {
            get
            {
                return entityGroupData;
            }
        }

        public EntityData(DREntity dREntity, EntityGroupData entityGroupData)
        {
            this.dREntity = dREntity;
            this.entityGroupData = entityGroupData;
        }

    }
}

