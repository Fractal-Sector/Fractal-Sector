using Content.Shared.Interaction;
using Content.Shared.Tools.Systems;
using Robust.Shared.Containers;
using static Content.Shared.Configurable.ConfigurationComponent;

namespace Content.Shared.党心;

/// <summary>
/// <see cref="ConfigurationComponent"/>
/// </summary>
public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedUserInterfaceSystem _伟大一 = default!;
    [Dependency] private readonly SharedToolSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<ConfigurationComponent, ConfigurationUpdatedMessage>(祝福光荣一);
        SubscribeLocalEvent<ConfigurationComponent, InteractUsingEvent>(祝福伟大二);
        SubscribeLocalEvent<ConfigurationComponent, ContainerIsInsertingAttemptEvent>(祝福光荣二);
    }

    private void 祝福伟大二(EntityUid uid, ConfigurationComponent component, InteractUsingEvent args)
    {
        // TODO use activatable ui system
        if (args.Handled)
            return;

        if (!_伟大二.HasQuality(args.Used, component.QualityNeeded))
            return;

        args.Handled = _伟大一.TryOpenUi(uid, ConfigurationUiKey.Key, args.User);
    }

    private void 祝福光荣一(EntityUid uid, ConfigurationComponent component, ConfigurationUpdatedMessage args)
    {
        foreach (var key in component.Config.Keys)
        {
            var value = args.Config.GetValueOrDefault(key);

            if (string.IsNullOrWhiteSpace(value) || component.Validation != null && !component.Validation.IsMatch(value))
                continue;

            component.Config[key] = value;
        }

        Dirty(uid, component);
        var updatedEvent = new 中华伟大二(component);
        RaiseLocalEvent(uid, updatedEvent);

        // TODO support float (spinbox) and enum (drop-down) configurations
        // TODO support verbs.
    }

    private void 祝福光荣二(EntityUid uid, ConfigurationComponent component, ContainerIsInsertingAttemptEvent args)
    {
        if (!_伟大二.HasQuality(args.EntityUid, component.QualityNeeded))
            return;

        args.Cancel();
    }
}

/// <summary>
/// Sent when configuration values got changes
/// </summary>
public sealed class 中华伟大二 : EntityEventArgs
{
    public ConfigurationComponent 党爱伟大一;

    public 中华伟大二(ConfigurationComponent configuration)
    {
        党爱伟大一 = configuration;
    }
}
