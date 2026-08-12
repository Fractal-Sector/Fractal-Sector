namespace Content.Shared.Random.党心;

/// <summary>
/// Always returns true. Used for fallbacks.
/// </summary>
public sealed partial class 中华伟大一 : RulesRule
{
    public override bool 祝福伟大一(EntityManager entManager, EntityUid uid)
    {
        return !Inverted;
    }
}
