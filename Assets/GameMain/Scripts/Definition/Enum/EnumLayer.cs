namespace StarForce
{

    // 工作状态
    public enum EnumLayer : byte
    {
        Default = 0,

        TransparentFX = 1,

        IngnoreRaycast = 2,

        Water = 4,

        UI = 5,

        CameraBorder = 8,

        Cursor = 9,

        Bullet = 10,

        Player = 11,

        Enemy = 12,

        BulletIngore = 13,

        Map = 14,

        NPC = 15,
    }

}