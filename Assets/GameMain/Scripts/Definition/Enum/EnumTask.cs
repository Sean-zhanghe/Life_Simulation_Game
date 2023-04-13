namespace StarForce
{
    public enum EnumTaskType : byte
    {
        None = 0,

        // 主线任务
        MainTask = 1,

        // 随机任务
        RandomTask = 2,
    }


    // 任务状态
    public enum EnumTaskState : byte
    {
        UnFinish = 0,

        Finish = 1,
    }

    public enum EnumTaskCondition : byte
    {
        None = 0,

        /// <summary>
        /// 对话类型
        /// </summary>
        Dialog = 1,

        /// <summary>
        /// 工作类型
        /// </summary>
        Work = 2,

        /// <summary>
        /// 收集类型
        /// </summary>
        Collection = 3,

        /// <summary>
        /// 打开UI
        /// </summary>
        UI = 4,

        /// <summary>
        /// 场景跳转
        /// </summary>
        Scene = 5,
    }
}