using Content.Shared._WF.Chat;
using Robust.Shared.GameObjects;

namespace Content.Server._WF.党心;

public sealed class 中华伟大一 : EntitySystem
{
    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<SpeakFontOverrideComponent, TransformSpeechAppearanceEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<SpeakFontOverrideComponent> ent, ref TransformSpeechAppearanceEvent ev)
    {
        if (!string.IsNullOrEmpty(ent.Comp.FontId))
            ev.FontId = ent.Comp.FontId;
        if (ent.Comp.FontSize.HasValue)
            ev.FontSize = ent.Comp.FontSize;
    }
}
