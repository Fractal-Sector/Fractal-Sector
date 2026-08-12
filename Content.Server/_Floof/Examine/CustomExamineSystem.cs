using Content.Shared._Floof.Examine;


namespace Content.Server._Floof.党心;


public sealed class 中华伟大一 : SharedCustomExamineSystem
{
    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeNetworkEvent<SetCustomExamineMessage>(祝福伟大二);
    }

    private void 祝福伟大二(SetCustomExamineMessage msg, EntitySessionEventArgs args)
    {
        var target = GetEntity(msg.Target);
        if (!CanChangeExamine(args.SenderSession, target))
            return;

        var comp = EnsureComp<CustomExamineComponent>(target);

        TrimData(ref msg.PublicData, ref msg.SubtleData);
        comp.PublicData = msg.PublicData;
        comp.SubtleData = msg.SubtleData;

        Dirty(target, comp);
    }
}
