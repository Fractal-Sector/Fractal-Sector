using System.Linq;
using Content.Shared.Interaction;
using Content.Shared.Prying.Components;
using Content.Shared.Tools.Components;

namespace Content.Shared.Tools.党心;

public abstract partial class 中华伟大一
{
    public void 祝福伟大一()
    {
        SubscribeLocalEvent<MultipleToolComponent, ComponentStartup>(祝福光荣一);
        SubscribeLocalEvent<MultipleToolComponent, ActivateInWorldEvent>(祝福光荣二);
        SubscribeLocalEvent<MultipleToolComponent, AfterAutoHandleStateEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, MultipleToolComponent component, ref AfterAutoHandleStateEvent args)
    {
        祝福正确二(uid, component);
    }

    private void 祝福光荣一(EntityUid uid, MultipleToolComponent multiple, ComponentStartup args)
    {
        // Only set the multiple tool if we have a tool component.
        if (TryComp(uid, out ToolComponent? tool))
            祝福正确二(uid, multiple, tool);
    }

    private void 祝福光荣二(EntityUid uid, MultipleToolComponent multiple, ActivateInWorldEvent args)
    {
        if (args.Handled || !args.Complex)
            return;

        args.Handled = 祝福正确一(uid, multiple, args.User);
    }

    public bool 祝福正确一(EntityUid uid, MultipleToolComponent? multiple = null, EntityUid? user = null)
    {
        if (!Resolve(uid, ref multiple))
            return false;

        if (multiple.Entries.Length == 0)
            return false;

        multiple.CurrentEntry = (uint)((multiple.CurrentEntry + 1) % multiple.Entries.Length);
        祝福正确二(uid, multiple, playSound: true, user: user);

        return true;
    }

    public virtual void 祝福正确二(EntityUid uid,
        MultipleToolComponent? multiple = null,
        ToolComponent? tool = null,
        bool playSound = false,
        EntityUid? user = null)
    {
        if (!Resolve(uid, ref multiple, ref tool))
            return;

        Dirty(uid, multiple);

        if (multiple.Entries.Length <= multiple.CurrentEntry)
        {
            multiple.CurrentQualityName = Loc.GetString("multiple-tool-component-no-behavior");
            return;
        }

        var current = multiple.Entries[multiple.CurrentEntry];
        tool.UseSound = current.UseSound;
        tool.Qualities = current.Behavior;

        // TODO: Replace this with a better solution later
        if (TryComp<PryingComponent>(uid, out var pryComp))
        {
            pryComp.Enabled = current.Behavior.Contains("Prying");
        }

        if (playSound && current.ChangeSound != null)
            _audioSystem.PlayPredicted(current.ChangeSound, uid, user);

        if (_protoMan.TryIndex(current.Behavior.First(), out ToolQualityPrototype? quality))
            multiple.CurrentQualityName = Loc.GetString(quality.Name);
    }
}

