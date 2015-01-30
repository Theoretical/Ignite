using System;
using System.Text;
using System.Runtime.InteropServices;

namespace Ignite.Packets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MinionSpawn
    {
        public PacketHeader Header;
        public UInt32 Unknown;
        public Byte SpawnType;
        public UInt32 NetId;
        public UInt32 NetId2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)]
        public Byte[] Uknown3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public Byte[] MinionName;

        public string Name
        {
            get
            {
                return Util.CStringToString(MinionName);
            }
        }
    }
}
