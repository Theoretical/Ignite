using System;
using System.Runtime.InteropServices;

namespace Ignite.Packets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BuyItem
    {
        public PacketHeader Header;
        public UInt32 ItemId;

        public static BuyItem Create(uint netId, uint itemId)
        {
            BuyItem item;
            item.Header = PacketHeader.Create(PacketCommand.C2S_BuyItemReq, netId);
            item.ItemId = itemId;

            return item;
        }
    }
}
