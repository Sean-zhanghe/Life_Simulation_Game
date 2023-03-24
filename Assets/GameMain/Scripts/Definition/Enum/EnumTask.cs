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

    // 任务完成条件类型
    public enum EnumTaskConditionType : byte
    {
        Nono = 0,

        // 对话类型
        DialogueType = 1,

        // 游戏类型
        GameType = 2,

        // 工作类型
        WorkType = 3,
    }
}