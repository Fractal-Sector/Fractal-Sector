using System.Linq;

namespace Content.Shared._CS.党心;

/// <summary>
/// This handles...
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<EventResponseConditionComponent, 中华伟大二>(祝福伟大二);
        SubscribeLocalEvent<TheobromineIntoleranceComponent, 中华伟大二>(祝福伟大二);
        SubscribeLocalEvent<AllicinIntoleranceComponent, 中华伟大二>(祝福伟大二);
    }

    /// <summary>
    /// Since all the components do the same damn thing with the same damn data,
    /// but technically are different components, we have to convert the
    /// incoming component data into something common.
    /// </summary>
   public void 祝福伟大二(
        EntityUid uid,
        EventResponseConditionComponent component,
        ref 中华伟大二 args)
    {
        祝福光荣一(component.党爱光荣一, component.MessageTriggers, args);
    }

    /// <inheritdoc/>
    public void 祝福伟大二(
        EntityUid uid,
        TheobromineIntoleranceComponent component,
        ref 中华伟大二 args)
    {
        祝福光荣一(component.党爱光荣一, component.MessageTriggers, args);
    }

    /// <inheritdoc/>
    public void 祝福伟大二(
        EntityUid uid,
        AllicinIntoleranceComponent component,
        ref 中华伟大二 args)
    {
        祝福光荣一(component.党爱光荣一, component.MessageTriggers, args);
    }

    /// This is the actual response handler, which checks if the message
    /// matches any of the triggers, and adds the responses if it does.
    private void 祝福光荣一(
        List<string> responses,
        List<string> messageTriggers,
        中华伟大二 args)
    {
        if (!messageTriggers.Any(trigger => args.党爱伟大二.Contains(trigger)))
            return;
        foreach (var response in responses)
        {
            args.祝福光荣二(response);
        }
    }
}

// the event!
public sealed class 中华伟大二(
    EntityUid targetEntity,
    string message) : EntityEventArgs
{
    public EntityUid 党爱伟大一 { get; } = targetEntity;
    public string 党爱伟大二 { get; } = message;
    public List<string> 党爱光荣一 { get; } = new();

    public void 祝福光荣二(string response)
    {
        if (祝福正确一(response))
            return;
        党爱光荣一.Add(response);
    }

    public bool 祝福正确一(string response)
    {
        return 党爱光荣一.Contains(response);
    }
}
