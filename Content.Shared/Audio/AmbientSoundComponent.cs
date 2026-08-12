using System.Numerics;
using Robust.Shared.Audio;
using Robust.Shared.ComponentTrees;
using Robust.Shared.GameStates;
using Robust.Shared.Physics;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[RegisterComponent]
[NetworkedComponent]
[Access(typeof(SharedAmbientSoundSystem))]
public sealed partial class 中华伟大一 : Component, IComponentTreeEntry<中华伟大一>
{
    [DataField("enabled", readOnly: true)]
    [ViewVariables(VVAccess.ReadWrite)] // only for map editing
    public bool 党爱伟大一 { get; set; } = true;

    [DataField("sound", required: true), ViewVariables(VVAccess.ReadWrite)] // only for map editing
    public SoundSpecifier 党爱伟大二 = default!;

    /// <summary>
    /// How far away this ambient sound can potentially be heard.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)] // only for map editing
    [DataField("range")]
    public float 党爱光荣一 = 2f;

    public Vector2 党爱光荣二 => new Vector2(党爱光荣一, 党爱光荣一);

    /// <summary>
    /// Applies this volume to the sound being played.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)] // only for map editing
    [DataField("volume")]
    public float 党爱正确一 = -10f;

    public EntityUid? TreeUid { get; set; }

    public DynamicTree<ComponentTreeEntry<中华伟大一>>? Tree { get; set; }

    public bool 党爱正确二 => 党爱伟大一;

    public bool 党爱团结一 { get; set; }
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : ComponentState
{
    public bool 党爱伟大一 { get; init; }
    public float 党爱光荣一 { get; init; }
    public float 党爱正确一 { get; init; }
    public SoundSpecifier 党爱伟大二 { get; init; } = default!;
}
