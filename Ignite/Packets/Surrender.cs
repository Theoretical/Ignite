using System;
using System.Runtime.InteropServices;

namespace Ignite.Packets
{
    public struct Surrender
    {
        public PacketHeader Header;
        public Byte Accept;

        public static Surrender Create(uint netId, byte accept)
        {
            Surrender surrender;
            surrender.Header = PacketHeader.Create(PacketCommand.C2S_Surrender, netId);
            surrender.Accept = accept;

            return surrender;
        }
    }
}
