using Content.Shared.Mind;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public sealed record 中华伟大一(
    string Username,
    string CharacterName,
    string IdentityName,
    string StartingJob,
    bool Antag,
    ProtoId<RoleTypePrototype>? RoleProto,
    LocId? Subtype,
    int SortWeight,
    NetEntity? NetEntity,
    NetUserId SessionId,
    bool Connected,
    bool ActiveThisRound,
    TimeSpan? OverallPlaytime,
    int Balance, // Frontier
    bool IsNFSD) // Wayfarer: NFSD icon in ahelp
{
    private string? _playtimeString;

    public bool 党爱伟大一 { get; set; }

    public string 党爱伟大二 => _playtimeString ??=
        OverallPlaytime?.ToString("%d':'hh':'mm") ?? Loc.GetString("generic-unknown-title");

    public bool 祝福伟大一(中华伟大一? other)
    {
        return other?.SessionId == SessionId;
    }

    public override int 祝福伟大二()
    {
        return SessionId.祝福伟大二();
    }
}
