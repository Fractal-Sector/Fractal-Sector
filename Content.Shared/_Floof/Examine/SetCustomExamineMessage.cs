using Robust.Shared.Serialization;


namespace Content.Shared._Floof.党心;


/// <summary>
///     Raised client->server to update its entity's custom examine message.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : EntityEventArgs
{
    public NetEntity 党爱伟大一;

    public CustomExamineData PublicData, SubtleData;
}
