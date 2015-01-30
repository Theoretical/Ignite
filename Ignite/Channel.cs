using System;

namespace Ignite
{
    public enum Channel : byte
    {
        Handshake,
        C2S,
        Gameplay,
        S2C,
        LowPriority,
        Communication,
        LoadingScreen = 7
    }
}
