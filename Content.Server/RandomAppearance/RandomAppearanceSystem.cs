using Robust.Server.GameObjects;
using Robust.Shared.Random;

namespace Content.Server.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;
    [Dependency] private readonly SharedAppearanceSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<RandomAppearanceComponent, ComponentInit>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, RandomAppearanceComponent component, ComponentInit args)
    {
        if (TryComp(uid, out AppearanceComponent? appearance) && component.EnumKey != null)
        {
            _伟大二.SetData(uid, component.EnumKey, _伟大一.Pick(component.SpriteStates), appearance);
        }
    }
}
