namespace StarForce
{

    // 事件状态
    public enum EnumEventState : byte
    {
        UnFinish = 0,

        Finish = 1,
    }

    public enum EnumEventType : byte
    {
        // 对话
        Dialog = 1,

        // 电话
        Phone = 2,

        // 工作
        Work = 3,

        // 场景
        Scene = 4,

        // UI
        UI = 5,

        // 实体
        Entity = 6,
    }

}