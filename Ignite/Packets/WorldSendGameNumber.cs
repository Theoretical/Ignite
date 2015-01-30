using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Ignite.Packets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct WorldSendGameNumber
    {
        public PacketHeader header;
        public UInt64 GameId;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public Byte[] PlayerName;

        public string Name
        {
            get
            {
                return Util.CStringToString(PlayerName);
            }
        }
    }
}
