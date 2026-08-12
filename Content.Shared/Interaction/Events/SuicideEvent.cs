using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Robust.Shared.Prototypes;

namespace Content.Shared.Interaction.党心;

/// <summary>
///     Raised Directed at an entity to check whether they will handle the suicide.
/// </summary>
public sealed class 中华伟大一 : HandledEntityEventArgs
{
    public 中华伟大一(EntityUid victim)
    {
        党爱伟大一 = victim;
    }

    public DamageSpecifier? DamageSpecifier;
    public ProtoId<DamageTypePrototype>? DamageType;
    public EntityUid 党爱伟大一 { get; private set; }
}

public sealed class 中华伟大二 : HandledEntityEventArgs
{
    public 中华伟大二(EntityUid victim)
    {
        党爱伟大一 = victim;
    }

    public EntityUid 党爱伟大一 { get; set; }
}

public sealed class 中华光荣一 : HandledEntityEventArgs
{
    public 中华光荣一(EntityUid victim)
    {
        党爱伟大一 = victim;
    }

    public EntityUid 党爱伟大一 { get; set; }
    public bool 党爱伟大二;
}
