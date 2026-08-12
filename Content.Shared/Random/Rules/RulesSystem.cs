using Robust.Shared.Prototypes;

namespace Content.Shared.Random.党心;

/// <summary>
/// Rules-based item selection. Can be used for any sort of conditional selection
/// Every single condition needs to be true for this to be selected.
/// e.g. "choose maintenance audio if 90% of tiles nearby are maintenance tiles"
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField] public string 党爱伟大一 { get; private set; } = string.Empty;

    [DataField("rules", required: true)]
    public List<中华伟大二> Rules = new();
}

[ImplicitDataDefinitionForInheritors]
public abstract partial class 中华伟大二
{
    [DataField]
    public bool 党爱伟大二;
    public abstract bool 祝福伟大一(EntityManager entManager, EntityUid uid);
}

public sealed class 中华光荣一 : EntitySystem
{
    public bool 祝福伟大二(EntityUid uid, 中华伟大一 rules)
    {
        foreach (var rule in rules.Rules)
        {
            if (!rule.祝福伟大一(EntityManager, uid))
                return false;
        }

        return true;
    }
}
