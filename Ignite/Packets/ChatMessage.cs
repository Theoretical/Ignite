using System;
using System.Runtime.InteropServices;

namespace Ignite.Packets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ChatMessage
    {
        public PacketHeader Header;
        public UInt32 BotId;
        public Byte IsBotMessage;
        public ChatType Type;
        public UInt32 Unknown;
        public UInt32 Length;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public Byte Unknown2;

    }
}
