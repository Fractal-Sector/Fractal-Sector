namespace Content.Shared.Roles.党心;

public abstract class 中华伟大一 : EntitySystem
{
    public void 祝福伟大一(Entity<RoleCodewordComponent> ent, string key, List<string> codewords, Color color)
    {
        var data = new CodewordsData(color, codewords);
        ent.Comp.RoleCodewords[key] = data;
        Dirty(ent);
    }
}
