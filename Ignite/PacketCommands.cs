using System;


namespace Ignite 
{
    public enum PacketCommand : ushort
    {
        KeyCheck = 0x00,

        C2S_InGame = 0x08,
        S2C_EndSpawn = 0x11,
        C2S_QueryStatusReq = 0x14,
        S2C_SkillUp = 0x15,
        C2S_Ping_Load_Info = 0x16,
        S2C_AutoAttack = 0x1A,

        S2C_AddGold = 0x22,
        S2C_CreateRegion = 0x23,
        C2S_Mute = 0x27,
        S2C_PlayerInfo = 0x2A,

        S2C_ViewAns = 0x2C,
        C2S_ViewReq = 0x2E,
        
        S2C_RemoveVisionBuff = 0x33,
        Char_SpawnPet = 0x37,
        C2S_SkillUp = 0x39,
        C2S_CapturePoint = 0x3A,
        S2C_FireSpellMissile = 0x3B,
        S2C_AttentionPing = 0x40,

        S2C_Emotion = 0x42,
        C2S_unkstart = 0x47,
        C2S_Emotion = 0x48,
        S2C_HeroSpawn = 0x4C,
        S2C_Announce = 0x4D,

        C2S_StartGame = 0x52,
        S2C_SynchVersion = 0x54,
        C2S_ScoreBoard = 0x56,
        C2S_AttentionPing = 0x57,
        S2C_SpellSet = 0x5A,
        S2C_StartGame = 0x5C,

        S2C_MoveAns = 0x61,
        S2C_StartSpawn = 0x62,
        S2C_CreateNeutral = 0x63,
        C2S_ClientReady = 0x64,
        S2C_LoadHero = 0x65,
        S2C_LoadName = 0x66,
        S2C_LoadScreenInfo = 0x67,
        ChatBoxMessage = 0x68,
        S2C_SetTarget = 0x6A,
        S2C_BuyItemAns = 0x6F,

        S2C_SetSpellData = 0x70,
        C2S_MoveReq = 0x72,
        C2S_MoveConfirm = 0x77,
        S2C_SpawnMinion = 0x7C,

        C2S_LockCamera = 0x81,
        C2S_BuyItemReq = 0x82,
        S2C_QueryStatusAns = 0x88,
        S2C_QuestComplete = 0x8C,
        C2S_Exit = 0x8F,

        World_SendGameNumber = 0x92,
        S2C_Ping_Load_Info = 0x95,
        S2C_ReconnectInfo = 0x98,
        C2S_CastSpell = 0x9A,
        S2C_TurretSpawn = 0x9D,

        S2C_Pause = 0xA1,
        C2S_Surrender = 0xA4,
        C2S_StatsConfirm = 0xA8,
        S2C_GainVision = 0xAD,
        S2C_SetHealth = 0xAE,
        C2S_Click = 0xAF,

        S2C_SpellAnimation = 0xB0,
        S2C_Tutorial = 0xB3,
        S2C_CastSpellAns = 0xB5,
        S2C_AfkWarning = 0xB8,
        S2C_MinionSpawn = 0xBA,
        C2S_SynchVersion = 0xBD,
        C2S_CharLoaded = 0xBE,

        S2C_GameTimer = 0xC1,
        S2C_GameTimerUpdate = 0xC2,
        S2C_CharStats = 0xC4,
        S2C_EndGame = 0xC6,
        S2C_SpawnBot = 0xCF,

        S2C_LevelPropSpawn = 0xD0,
        S2C_UpdateLevelProp = 0xD1,
        S2C_HandleCapturePointUpdate = 0xD3,

        S2C_GoldGain = 0xE4,
        S2C_ActivateMinionCamp = 0xE4,

        Batch = 0xFF
    }
}