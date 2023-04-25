namespace StarForce
{

    // 工作状态
    public enum EnumWorkState : byte
    {
        None = 0,

        // 申请
        Apply = 1,

        // 已申请
        Applied = 2,

        // 工作中
        Working = 3,

        // 完成
        Finish = 4,
    }

    public enum EnumWorkType : byte
    {
        None = 0,

        // 游戏闯关
        Game = 1,

        // 收集（垃圾，砍树...）
        Collect = 2,
    }

}