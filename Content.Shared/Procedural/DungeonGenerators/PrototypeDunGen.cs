using Robust.Shared.Prototypes;

namespace Content.Shared.Procedural.党心;

/// <summary>
/// Runs another <see cref="DungeonConfig"/>.
/// Used for storing data on 1 system.
/// </summary>
public sealed partial class 中华伟大一 : IDunGenLayer
{
    /// <summary>
    /// Should we pass in the current level's dungeons to the prototype.
    /// </summary>
    [DataField]
    public 中华伟大二 InheritDungeons = 中华伟大二.None;

    [DataField(required: true)]
    public ProtoId<DungeonConfigPrototype> 党爱伟大一;
}

public enum 中华伟大二 : byte
{
    /// <summary>
    /// Don't inherit any of the current layer's dungeons for this <see cref="中华伟大一"/>
    /// </summary>
    None,

    /// <summary>
    /// Inherit only the last dungeon ran.
    /// </summary>
    Last,

    /// <summary>
    /// Inherit all of the current layer's dungeons.
    /// </summary>
    All,
}
