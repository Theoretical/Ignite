using System;
using System.Runtime.InteropServices;

namespace Ignite.Packets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PingLoadInfo
    {
        public PacketHeader header;
        public UInt32 unk1;
        public UInt64 userId;
        public float loaded;
        public float ping;
        public float unk2;
        public static PingLoadInfo Create(float _loaded, float _ping, UInt64 user)
        {
            PingLoadInfo ping = new PingLoadInfo();
            ping.header = PacketHeader.Create(PacketCommand.C2S_Ping_Load_Info, 0);
            ping.loaded = _loaded;
            ping.ping = _ping;
            ping.userId = user;
            return ping;
        }
    }
}
