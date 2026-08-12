using Content.Shared.Paper;
using Content.Shared.StoryGen;

namespace Content.Server.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly StoryGeneratorSystem _伟大一 = default!;
    [Dependency] private readonly PaperSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<PaperRandomStoryComponent, MapInitEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<PaperRandomStoryComponent> paperStory, ref MapInitEvent ev)
    {
        if (!TryComp<PaperComponent>(paperStory, out var paper))
            return;

        if (!_伟大一.TryGenerateStoryFromTemplate(paperStory.Comp.Template, out var story))
            return;

        _伟大二.SetContent((paperStory.Owner, paper), story);
    }
}
