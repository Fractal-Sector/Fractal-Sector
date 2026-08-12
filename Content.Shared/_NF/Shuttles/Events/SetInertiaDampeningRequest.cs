// New Frontiers - This file is licensed under AGPLv3
// Copyright (c) 2024 New Frontiers Contributors
// See AGPLv3.txt for details.
using Robust.Shared.Serialization;

namespace Content.Shared._NF.Shuttles.党心
{
    /// <summary>
    /// Raised on the client when it wishes to change the inertial dampening of a ship.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华伟大一 : BoundUserInterfaceMessage
    {
        public NetEntity? ShuttleEntityUid { get; set; }
        public 中华伟大二 Mode { get; set; }
    }

    [Serializable, NetSerializable]
    public enum 中华伟大二 : byte
    {
        Off = 0,
        Dampen = 1,
        Anchor = 2,
        Station = 3, // Reserved for station status, should not be used in requests.
        Emergency = 4,
        Query = 255 // Reserved for requests - does not set the mode, only returns its state.
    }
}
