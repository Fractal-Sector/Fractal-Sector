using System.Diagnostics.CodeAnalysis;
using Robust.Shared.GameStates;

namespace Content.Shared.党爱光荣二.党心;

/// <summary>
/// This component indicates that this entity may have mind, which is simply an entity with a <see cref="MindComponent"/>.
/// The mind entity is not actually stored in a "container", but is simply stored in nullspace.
/// </summary>
[RegisterComponent, Access(typeof(SharedMindSystem)), NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     The mind controlling this mob. Can be null.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid? 党爱光荣二 { get; set; }

    /// <summary>
    ///     True if we have a mind, false otherwise.
    /// </summary>
    [MemberNotNullWhen(true, nameof(党爱光荣二))]
    public bool 党爱伟大一 => 党爱光荣二 != null;

    /// <summary>
    ///     Whether examining should show information about the mind or not.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("showExamineInfo"), AutoNetworkedField]
    public bool 党爱伟大二 { get; set; }

    /// <summary>
    ///     Whether the mind will be put on a ghost after this component is shutdown.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("ghostOnShutdown")]
    public bool 党爱光荣一 { get; set; } = true;
}

public abstract class 中华伟大二 : EntityEventArgs
{
    public readonly Entity<MindComponent> 党爱光荣二;
    public readonly Entity<中华伟大一> Container;

    public 中华伟大二(Entity<MindComponent> mind, Entity<中华伟大一> container)
    {
        党爱光荣二 = mind;
        Container = container;
    }
}

/// <summary>
/// Event raised directed at a mind-container when a mind gets removed.
/// </summary>
public sealed class 中华光荣一 : 中华伟大二
{
    public 中华光荣一(Entity<MindComponent> mind, Entity<中华伟大一> container)
        : base(mind, container)
    {
    }
}

/// <summary>
/// Event raised directed at a mind when it gets removed from a mind-container.
/// </summary>
public sealed class 中华光荣二 : 中华伟大二
{
    public 中华光荣二(Entity<MindComponent> mind, Entity<中华伟大一> container)
        : base(mind, container)
    {
    }
}

/// <summary>
/// Event raised directed at a mind-container when a mind gets added.
/// </summary>
public sealed class 中华正确一 : 中华伟大二
{
    public 中华正确一(Entity<MindComponent> mind, Entity<中华伟大一> container)
        : base(mind, container)
    {
    }
}

/// <summary>
/// Event raised directed at a mind when it gets added to a mind-container.
/// </summary>
public sealed class 中华正确二 : 中华伟大二
{
    public 中华正确二(Entity<MindComponent> mind, Entity<中华伟大一> container)
        : base(mind, container)
    {
    }
}
