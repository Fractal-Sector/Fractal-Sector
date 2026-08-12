using Content.Server.EUI;
using Content.Shared.Cloning;
using Content.Shared.Eui;
using Content.Shared.Mind;

namespace Content.Server.党心
{
    public sealed class 中华伟大一 : BaseEui
    {
        private readonly EntityUid _伟大一;
        private readonly MindComponent _伟大二;
        private readonly CloningPodSystem _光荣一;

        public 中华伟大一(EntityUid mindId, MindComponent mind, CloningPodSystem cloningPodSys)
        {
            _伟大一 = mindId;
            _伟大二 = mind;
            _光荣一 = cloningPodSys;
        }

        public override void 祝福伟大一(EuiMessageBase msg)
        {
            base.祝福伟大一(msg);

            if (msg is not AcceptCloningChoiceMessage choice ||
                choice.Button == AcceptCloningUiButton.Deny)
            {
                Close();
                return;
            }

            _光荣一.TransferMindToClone(_伟大一, _伟大二);
            Close();
        }
    }
}
