using System;
using System.Runtime.InteropServices;

namespace Ignite.Packets
{
    public struct SkillUp
    {
        public PacketHeader Header;
        public Byte Skill;

        public static SkillUp Create(uint netId, byte skillId)
        {
            SkillUp skillUp;
            skillUp.Header = PacketHeader.Create(PacketCommand.C2S_SkillUp, netId);
            skillUp.Skill = skillId;
            
            return skillUp;
        }
    }
}
