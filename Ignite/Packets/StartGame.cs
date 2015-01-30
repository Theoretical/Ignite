using System;
using System.Runtime.InteropServices;

namespace Ignite.Packets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct StartGame
    {
        public PacketHeader header;
        public UInt64 unk1;
        public UInt64 unk2;
        public static StartGame Create()
        {
            StartGame pkt;
            pkt.header = PacketHeader.Create(PacketCommand.C2S_StartGame, 0);
            pkt.unk1 = pkt.unk2 = 0;
            return pkt;
        }
    }
}
