using System;
using System.IO;
using System.Runtime.InteropServices;
namespace Ignite.Packets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketHeader
    {
        public UInt16 cmd;
        public UInt32 netId;
        public static PacketHeader Create(PacketCommand cmd, UInt32 netId)
        {
            PacketHeader pktHeader;
            pktHeader.cmd = (UInt16) cmd;
            pktHeader.netId = netId;
            return pktHeader;
        }
    }
}
