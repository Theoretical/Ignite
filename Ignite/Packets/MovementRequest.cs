using System;
using System.Runtime.InteropServices;

namespace Ignite.Packets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MovementRequest
    {
        public PacketHeader Header;
        public MovementType MoveType;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public Byte[] X;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public Byte[] Y;
        public UInt32 Unknown;
        public Byte WaypointCount;
        public UInt32 NetId;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public Byte[] Waypoints;

        public static MovementRequest CreateCoop(UInt32 netId)
        {
            MovementRequest req;
            req.Header = PacketHeader.Create(PacketCommand.C2S_MoveReq, netId);
            req.MoveType = MovementType.AttackMove;
            req.X = new byte[] { 0xAA, 0xEF, 0x15, 0x46 };
            req.Y = new byte[] { 0x8C, 0xC1, 0x17, 0x46 };
            req.Unknown = 0;
            req.WaypointCount = 10;
            req.NetId = netId;
            req.Waypoints = new byte[] { 0x20, 0x6B, 0xF2, 0x6E, 0xF2, 0xDB, 0xF5, 0x8F, 0xF4, 0x3A, 0xFB, 0x52, 0xFA, 0xE9, 0xFB, 0x7D, 0x16, 0x05, 0xDC, 0x04 };
            return req;
        }

        public static MovementRequest CreateOdin(UInt32 netId, bool toCenter = true)
        {
            MovementRequest req;
            req.Header = PacketHeader.Create(PacketCommand.C2S_MoveReq, netId);
            req.MoveType = MovementType.Move;
            req.Unknown = 0;
            req.NetId = netId;
            
            if (toCenter)
            {
                req.X = new byte[] { 0x38, 0xDE, 0xFE, 0x45 };
                req.Y = new byte[] { 0x69, 0x7B, 0x8E, 0x45 };
                req.WaypointCount = 14;
                req.Waypoints = new byte[] { 0x2C, 0xAC, 0x28, 0xC4, 0xF2, 0x57, 0xF3, 0x19, 0xF4, 0x3E, 0xF5, 0x4B, 0x32, 0x2C, 0xF5, 0x19, 0xC9, 0xF8, 0xE1, 0xF7, 0xF5, 0xF9, 0xF4, 0xF8, 0x64, 0x4B, 0xE9, 0xFB, 0x83, 0x7F, 0xFC, 0x00, 0x73, 0xFE, 0x9D, 0xFA, 0xC4, 0x01, 0x32, 0x46, 0x02, 0xFE };
            }
            else
            {
                req.X = new byte[] { 0xc8, 0xD3, 0x0, 0x42 };
                req.Y = new byte[] { 0x88, 0xA1, 0x82, 0x43 };
                req.WaypointCount = 24;
                req.Waypoints = new byte[] { 0x2C, 0xAC, 0x28, 0xC4, 0xF2, 0x57, 0xF3, 0x19, 0xF4, 0x3E, 0xF5, 0x4B, 0x32, 0x2C, 0xF5, 0x19, 0xC9, 0xF8, 0xE1, 0xF7, 0xF5, 0xF9, 0xF4, 0xF8, 0x64, 0x4B, 0xE9, 0xFB, 0x83, 0x7F, 0xFC, 0x00, 0x73, 0xFE, 0x9D, 0xFA, 0xC4, 0x01, 0x32, 0x46, 0x02, 0xFE };
            }

            return req;
        }
    }
}
