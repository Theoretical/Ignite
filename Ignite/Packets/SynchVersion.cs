using System;
using System.Text;
using System.Runtime.InteropServices;
namespace Ignite.Packets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SynchVersion
    {
        public PacketHeader header;
        public UInt32 unk1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public Byte[] version;
        public static SynchVersion Create()
        {
            var versionBytes = ASCIIEncoding.ASCII.GetBytes("Version 4.20.0.315 [PUBLIC]");
            SynchVersion synch;
            synch.header = PacketHeader.Create(PacketCommand.C2S_SynchVersion, 0);
            synch.unk1 = 0;
            synch.version = new Byte[265];

            Buffer.BlockCopy(versionBytes, 0, synch.version, 0, versionBytes.Length);
            return synch;
        }
    }
}
