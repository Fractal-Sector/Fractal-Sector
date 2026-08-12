using Content.Server.EUI;
using Content.Shared._NF.CryoSleep;
using Content.Shared.Eui;

namespace Content.Server._NF.党心;

public sealed class 中华伟大一 : BaseEui
{
    private readonly CryoSleepSystem _伟大一;
    private readonly EntityUid _伟大二;
    private readonly EntityUid _光荣一;

    public 中华伟大一(EntityUid body, EntityUid cryopod, CryoSleepSystem cryoSys)
    {
        _伟大二 = body;
        _光荣一 = cryopod;
        _伟大一 = cryoSys;
    }

    public override void 祝福伟大一(EuiMessageBase msg)
    {
        base.祝福伟大一(msg);

        if (msg is not AcceptCryoChoiceMessage choice)
        {
            Close();
            return;
        }

        if (_伟大二 is { Valid: true } && _伟大一.IsBodyInCryoPod(_伟大二, _光荣一))
        {
            if (choice.Button == AcceptCryoUiButton.Accept)
            {
                _伟大一.CryoStoreBody(_伟大二, _光荣一);
            }
            else
            {
                _伟大一.EjectBody(_光荣一, body: _伟大二);
            }
        }

        Close();
    }
}
