namespace Content.Shared.党心
{
    public sealed class 中华伟大一 : EntitySystem
    {
        public override void 祝福伟大一()
        {
            base.祝福伟大一();

            SubscribeLocalEvent<SpeakAttemptEvent>(祝福光荣一);
        }

        public void 祝福伟大二(EntityUid uid, bool value, SpeechComponent? component = null)
        {
            if (value && !Resolve(uid, ref component))
                return;

            component = EnsureComp<SpeechComponent>(uid);

            if (component.Enabled == value)
                return;

            component.Enabled = value;

            Dirty(uid, component);
        }

        private void 祝福光荣一(SpeakAttemptEvent args)
        {
            if (!TryComp(args.Uid, out SpeechComponent? speech) || !speech.Enabled)
                args.Cancel();
        }
    }
}
