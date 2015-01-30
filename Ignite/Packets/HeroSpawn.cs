using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Ignite.Packets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct HeroSpawn
    {
        public PacketHeader Header;
        public UInt32 NetId;
        public UInt32 PlayerId;
        public Byte NodeId;
        public Byte SkillLevel;
        public Byte TeamIsOrder;
        public Byte IsBot;
        public Byte SpawnPos;
        public UInt32 SkinNumber;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public Byte[] PlayerName;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        public Byte[] HeroName;
        public float DeathDurationRemaining;
        public float TimeSinceDeath;
        public Byte BitField;

        public string Name
        {
            get
            {
                return Util.CStringToString(PlayerName);
            }
        }

        public string Hero
        {
            get
            {
                return Util.CStringToString(HeroName);
            }
        }
    }
}
