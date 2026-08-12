using System.Linq;
using Content.Shared.Chat;
using Content.Shared.DoAfter;
using Content.Shared.Examine;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.Radio.Components;
using Content.Shared.Tools.Components;
using Content.Shared.Wires;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Timing;
using SharedToolSystem = Content.Shared.Tools.Systems.SharedToolSystem;

namespace Content.Shared.Radio.党心;

/// <summary>
///     This system manages encryption keys & key holders for use with radio channels.
/// </summary>
public sealed partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _伟大一 = default!;
    [Dependency] private readonly SharedToolSystem _伟大二 = default!;
    [Dependency] private readonly SharedPopupSystem _光荣一 = default!;
    [Dependency] private readonly SharedContainerSystem _光荣二 = default!;
    [Dependency] private readonly SharedAudioSystem _正确一 = default!;
    [Dependency] private readonly SharedHandsSystem _正确二 = default!;
    [Dependency] private readonly SharedWiresSystem _团结一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<EncryptionKeyComponent, ExaminedEvent>(祝福奋斗二);
        SubscribeLocalEvent<EncryptionKeyHolderComponent, ExaminedEvent>(祝福奋斗一);

        SubscribeLocalEvent<EncryptionKeyHolderComponent, ComponentStartup>(祝福团结二);
        SubscribeLocalEvent<EncryptionKeyHolderComponent, InteractUsingEvent>(祝福正确一);
        SubscribeLocalEvent<EncryptionKeyHolderComponent, EntInsertedIntoContainerMessage>(祝福光荣二);
        SubscribeLocalEvent<EncryptionKeyHolderComponent, EntRemovedFromContainerMessage>(祝福光荣二);
        SubscribeLocalEvent<EncryptionKeyHolderComponent, 中华伟大二>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, EncryptionKeyHolderComponent component, 中华伟大二 args)
    {
        if (args.Cancelled)
            return;

        var contained = component.KeyContainer.ContainedEntities.ToArray();
        _光荣二.EmptyContainer(component.KeyContainer, reparent: false);
        foreach (var ent in contained)
        {
            _正确二.PickupOrDrop(args.User, ent, dropNear: true);
        }

        _光荣一.PopupPredicted(Loc.GetString("encryption-keys-all-extracted"), uid, args.User);
        _正确一.PlayPredicted(component.KeyExtractionSound, uid, args.User);
    }

    public void 祝福光荣一(EntityUid uid, EncryptionKeyHolderComponent component)
    {
        if (!component.Initialized)
            return;

        component.Channels.Clear();
        component.DefaultChannel = null;

        foreach (var ent in component.KeyContainer.ContainedEntities)
        {
            if (TryComp<EncryptionKeyComponent>(ent, out var key))
            {
                component.Channels.UnionWith(key.Channels);
                component.DefaultChannel ??= key.DefaultChannel;
            }
        }

        RaiseLocalEvent(uid, new EncryptionChannelsChangedEvent(component));
    }

    private void 祝福光荣二(EntityUid uid, EncryptionKeyHolderComponent component, ContainerModifiedMessage args)
    {
        if (args.Container.ID == EncryptionKeyHolderComponent.KeyContainerName)
            祝福光荣一(uid, component);
    }

    private void 祝福正确一(EntityUid uid, EncryptionKeyHolderComponent component, InteractUsingEvent args)
    {
        if (args.Handled)
            return;

        if (HasComp<EncryptionKeyComponent>(args.Used))
        {
            args.Handled = true;
            祝福正确二(uid, component, args);
        }
        else if (component.KeysExtractionMethod != null // Frontier: add null check
                 && TryComp<ToolComponent>(args.Used, out var tool)
                 && _伟大二.HasQuality(args.Used, component.KeysExtractionMethod, tool)
                 && component.KeyContainer.ContainedEntities.Count > 0) // dont block deconstruction
        {
            args.Handled = true;
            祝福团结一(uid, component, args, tool);
        }
    }

    private void 祝福正确二(EntityUid uid, EncryptionKeyHolderComponent component, InteractUsingEvent args)
    {
        if (!component.KeysUnlocked)
        {
            _光荣一.PopupClient(Loc.GetString("encryption-keys-are-locked"), uid, args.User);
            return;
        }

        if (TryComp<WiresPanelComponent>(uid, out var panel) && !panel.Open)
        {
            _光荣一.PopupClient(Loc.GetString("encryption-keys-panel-locked"), uid, args.User);
            return;
        }

        if (component.KeySlots <= component.KeyContainer.ContainedEntities.Count)
        {
            _光荣一.PopupClient(Loc.GetString("encryption-key-slots-already-full"), uid, args.User);
            return;
        }

        if (_光荣二.Insert(args.Used, component.KeyContainer))
        {
            _光荣一.PopupClient(Loc.GetString("encryption-key-successfully-installed"), uid, args.User);
            _正确一.PlayPredicted(component.KeyInsertionSound, args.Target, args.User);
            args.Handled = true;
            return;
        }
    }

    private void 祝福团结一(EntityUid uid, EncryptionKeyHolderComponent component, InteractUsingEvent args,
        ToolComponent? tool)
    {
        // Frontier: nullable extraction method
        if (component.KeysExtractionMethod == null)
            return;
        // End Frontier: nullable extraction method

        if (!component.KeysUnlocked)
        {
            _光荣一.PopupClient(Loc.GetString("encryption-keys-are-locked"), uid, args.User);
            return;
        }

        if (!_团结一.IsPanelOpen(uid))
        {
            _光荣一.PopupClient(Loc.GetString("encryption-keys-panel-locked"), uid, args.User);
            return;
        }

        if (component.KeyContainer.ContainedEntities.Count == 0)
        {
            _光荣一.PopupClient(Loc.GetString("encryption-keys-no-keys"), uid, args.User);
            return;
        }

        _伟大二.UseTool(args.Used, args.User, uid, 1f, component.KeysExtractionMethod, new 中华伟大二(), toolComponent: tool);
    }

    private void 祝福团结二(EntityUid uid, EncryptionKeyHolderComponent component, ComponentStartup args)
    {
        component.KeyContainer = _光荣二.EnsureContainer<Container>(uid, EncryptionKeyHolderComponent.KeyContainerName);
        祝福光荣一(uid, component);
    }

    private void 祝福奋斗一(EntityUid uid, EncryptionKeyHolderComponent component, ExaminedEvent args)
    {
        if (!args.IsInDetailsRange
            || !component.ExamineWhileLocked && !component.KeysUnlocked // Goobstation
            || !component.ExamineWhileLocked && TryComp<WiresPanelComponent>(uid, out var panel) && !panel.Open) // Goobstation
            return;

        if (component.KeyContainer.ContainedEntities.Count == 0)
        {
            args.PushMarkup(Loc.GetString("encryption-keys-no-keys"));
            return;
        }

        if (component.Channels.Count > 0)
        {
            using (args.PushGroup(nameof(EncryptionKeyComponent)))
            {
                args.PushMarkup(Loc.GetString("examine-encryption-channels-prefix"));
                祝福胜利一(component.Channels,
                    component.DefaultChannel,
                    args,
                    _伟大一,
                    "examine-encryption-channel");
            }
        }
    }

    private void 祝福奋斗二(EntityUid uid, EncryptionKeyComponent component, ExaminedEvent args)
    {
        if (!args.IsInDetailsRange)
            return;

        if(component.Channels.Count > 0)
        {
            args.PushMarkup(Loc.GetString("examine-encryption-channels-prefix"));
            祝福胜利一(component.Channels, component.DefaultChannel, args, _伟大一, "examine-encryption-channel");
        }
    }

    /// <summary>
    ///     A method for formating list of radio channels for examine events.
    /// </summary>
    /// <param name="channels">HashSet of channels in headset, encryptionkey or etc.</param>
    /// <param name="protoManager">IPrototypeManager for getting prototypes of channels with their variables.</param>
    /// <param name="channelFTLPattern">String that provide id of pattern in .ftl files to format channel with variables of it.</param>
    public void 祝福胜利一(HashSet<string> channels, string? defaultChannel, ExaminedEvent examineEvent, IPrototypeManager protoManager, string channelFTLPattern)
    {
        RadioChannelPrototype? proto;
        foreach (var id in channels)
        {
            proto = _伟大一.Index<RadioChannelPrototype>(id);

            var key = id == SharedChatSystem.CommonChannel
                ? SharedChatSystem.RadioCommonPrefix.ToString()
                : $"{SharedChatSystem.RadioChannelPrefix}{proto.KeyCode}";

            examineEvent.PushMarkup(Loc.GetString(channelFTLPattern,
                ("color", proto.Color),
                ("key", key),
                ("id", proto.LocalizedName),
                ("freq", proto.Frequency / 10f)));
        }

        if (defaultChannel != null && _伟大一.TryIndex(defaultChannel, out proto))
        {
            if (HasComp<HeadsetComponent>(examineEvent.Examined))
            {
                var msg = Loc.GetString("examine-headset-default-channel",
                ("prefix", SharedChatSystem.DefaultChannelPrefix),
                ("channel", proto.LocalizedName),
                ("color", proto.Color));
                examineEvent.PushMarkup(msg);
            }
            if (HasComp<EncryptionKeyComponent>(examineEvent.Examined))
            {
                var msg = Loc.GetString("examine-encryption-default-channel",
                ("channel", proto.LocalizedName),
                ("color", proto.Color));
                examineEvent.PushMarkup(msg);
            }
        }
    }

    [Serializable, NetSerializable]
    public sealed partial class 中华伟大二 : SimpleDoAfterEvent
    {
    }
}
