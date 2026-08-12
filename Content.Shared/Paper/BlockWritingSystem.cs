namespace Content.Shared.党心;

/// <summary>
/// A system that prevents those with the IlliterateComponent from writing on paper.
/// Has no effect on reading ability.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<BlockWritingComponent, PaperWriteAttemptEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<BlockWritingComponent> entity, ref PaperWriteAttemptEvent args)
    {
        args.FailReason = entity.Comp.FailWriteMessage;
        args.Cancelled = true;
    }
}
