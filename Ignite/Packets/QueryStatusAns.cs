using System;
using System.Runtime.InteropServices;

namespace Ignite.Packets
{
    public struct QueryStatusAns
    {
        public PacketHeader Header;
        public Byte Ok;
    }
}
