using System;
using System.Runtime.InteropServices;

namespace Ignite.Packets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CapturePoint
    {
        public PacketHeader Header;
        public UInt32 CapNetId;

        public static CapturePoint Create(uint netId, uint capNetId)
        {
            CapturePoint point;
            point.Header = PacketHeader.Create(PacketCommand.C2S_CapturePoint, netId);
            point.CapNetId = capNetId;

            return point;
        }
    }
}
