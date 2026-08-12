using Robust.Shared.Network;
using Robust.Shared.Serialization;

namespace Content.Shared.Administration.党心;

[Serializable, NetSerializable]
public sealed record 中华伟大一(
    int? Id,
    NetUserId? UserId,
    (string address, int cidrMask)? Address,
    string? HWId,
    DateTime BanTime,
    DateTime? ExpirationTime,
    string Reason,
    string? BanningAdminName,
    SharedServerUnban? Unban,
    string Role
) : SharedServerBan(Id, UserId, Address, HWId, BanTime, ExpirationTime, Reason, BanningAdminName, Unban);
