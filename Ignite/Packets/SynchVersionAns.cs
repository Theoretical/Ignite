using System;
using System.Runtime.InteropServices;

namespace Ignite.Packets
{

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SynchBlock
    {
        public UInt64 userId;
        public UInt16 netId;
        public UInt32 skill1;
        public UInt32 skill2;
        public Byte bBot;
        public UInt32 teamId;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public Byte[] name;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public Byte[] type;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
        public Byte[] rank;

        public static SynchBlock Create()
        {
            SynchBlock synch = new SynchBlock();
            synch.userId = 0xFFFFFFFFFFFFFFFF;
            synch.netId = 0x1E;
            synch.teamId = 0x64;
            synch.bBot = 0; //1 for bot?
            return synch;
        }
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SynchVersionAns
    {
        public PacketHeader header;
        public Byte ok;
        public UInt32 mapId;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public SynchBlock[] players;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public Byte[] version;      //Ending zero so size 26+0x00
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public Byte[] gameMode;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
        public Byte[] unk1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 245)]
        public Byte[] unk2;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public Byte[] ekg1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public Byte[] msg1;
        public UInt16 unk3;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public Byte[] ekg2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public Byte[] msg2;

        public UInt16 unk4;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public Byte[] ekg3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public Byte[] msg3;

        public UInt16 unk5;
        public UInt32 unk6;
        public UInt32 options;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x100)]
        public Byte[] unk7;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
        public Byte[] ukn8;

        public string GameMode
        {
            get
            {
                return Util.CStringToString(gameMode);
            }
        }
    }
}
