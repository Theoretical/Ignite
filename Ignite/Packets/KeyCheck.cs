using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Ignite.Packets
{
    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public struct KeyCheck
    {
        public Byte cmd;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public Byte[] partialKey;
        public UInt32 playerNo;
        public UInt64 userId;
        public UInt32 trash;
        public UInt64 checkId;
        public UInt32 trash2;
        public static KeyCheck Create(UInt64 userId, UInt64 checkId, byte[] key)
        {
            KeyCheck keyCheck;
            keyCheck.cmd = (Byte)PacketCommand.KeyCheck;
            keyCheck.userId = userId;
            keyCheck.checkId = checkId;
            keyCheck.playerNo = 0;
            keyCheck.trash = 41300265;
            keyCheck.trash2 = 51685968;
            keyCheck.partialKey = new byte[3] {key[1], key[2], key[3] };
            return keyCheck;
        }
    }
}
