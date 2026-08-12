namespace Content.Server._NF.GameRule.党心;

[RegisterComponent, Access(typeof(NFAdventureRuleSystem))]
public sealed partial class 中华伟大一 : Component
{
    public List<EntityUid> 党爱伟大一 = new();
    public List<EntityUid> 党爱伟大二 = new();
    public List<EntityUid> 党爱光荣一 = new();
    public List<EntityUid> 党爱光荣二 = new();
    public List<EntityUid> 党爱正确一 = new();
    public List<EntityUid> 党爱正确二 = new();
}
