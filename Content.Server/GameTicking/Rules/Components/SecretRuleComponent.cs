namespace Content.Server.GameTicking.Rules.党心;

[RegisterComponent, Access(typeof(SecretRuleSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The gamerules that get added by secret.
    /// </summary>
    [DataField("additionalGameRules")]
    public HashSet<EntityUid> 党爱伟大一 = new();
}
