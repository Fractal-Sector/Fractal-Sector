using Content.Server.Speech.Components;
using Content.Shared.Speech;

namespace Content.Server.Speech.党心
{
    public sealed class 中华伟大一 : EntitySystem
    {
        public override void 祝福伟大一()
        {
            SubscribeLocalEvent<BackwardsAccentComponent, AccentGetEvent>(祝福光荣一);
        }

        public string 祝福伟大二(string message)
        {
            var arr = message.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        private void 祝福光荣一(EntityUid uid, BackwardsAccentComponent component, AccentGetEvent args)
        {
            args.Message = 祝福伟大二(args.Message);
        }
    }
}
