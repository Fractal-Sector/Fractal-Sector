using Content.Server.CharacterAppearance.Components;
using Content.Shared.Humanoid;
using Content.Shared.Preferences;

namespace Content.Server.Humanoid.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly HumanoidAppearanceSystem _伟大一 = default!;
    [Dependency] private readonly MetaDataSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<RandomHumanoidAppearanceComponent, MapInitEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, RandomHumanoidAppearanceComponent component, MapInitEvent args)
    {
        // If we have an initial profile/base layer set, do not randomize this humanoid.
        if (!TryComp(uid, out HumanoidAppearanceComponent? humanoid) || !string.IsNullOrEmpty(humanoid.Initial))
        {
            return;
        }

        var profile = HumanoidCharacterProfile.RandomWithSpecies(humanoid.Species);
        //If we have a specified hair style, change it to this
        if(component.Hair != null)
            profile = profile.WithCharacterAppearance(profile.Appearance.WithHairStyleName(component.Hair));

        _伟大一.LoadProfile(uid, profile, humanoid);

        if (component.RandomizeName)
            _伟大二.SetEntityName(uid, profile.Name);
    }
}
