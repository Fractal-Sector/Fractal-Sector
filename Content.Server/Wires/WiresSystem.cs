using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using Content.Server.Construction;
using Content.Server.Construction.Components;
using Content.Server.Hands.Systems;
using Content.Server.Power.Components;
using Content.Shared.DoAfter;
using Content.Shared.GameTicking;
using Content.Shared.Hands.Components;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.Power;
using Content.Shared.Tools;
using Content.Shared.Tools.Components;
using Content.Shared.Wires;
using Robust.Server.GameObjects;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Server.党心;

public sealed class 中华伟大一 : SharedWiresSystem
{
    [Dependency] private readonly IPrototypeManager _伟大一 = default!;
    [Dependency] private readonly SharedDoAfterSystem _伟大二 = default!;
    [Dependency] private readonly HandsSystem _光荣一 = default!;
    [Dependency] private readonly SharedPopupSystem _光荣二 = default!;
    [Dependency] private readonly SharedInteractionSystem _正确一 = default!;
    [Dependency] private readonly UserInterfaceSystem _正确二 = default!;
    [Dependency] private readonly IRobustRandom _团结一 = default!;
    [Dependency] private readonly ConstructionSystem _团结二 = default!;

    private static readonly ProtoId<ToolQualityPrototype> CuttingQuality = "Cutting";
    private static readonly ProtoId<ToolQualityPrototype> PulsingQuality = "Pulsing";

    // This is where all the wire layouts are stored.
    [ViewVariables] private readonly Dictionary<string, 中华正确一> _layouts = new();

    private float _奋斗一 = 0f;

    #region Initialization
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<RoundRestartCleanupEvent>(祝福自由二);

        // this is a broadcast event
        SubscribeLocalEvent<WiresComponent, PanelChangedEvent>(祝福胜利一);
        SubscribeLocalEvent<WiresComponent, WiresActionMessage>(祝福团结二);
        SubscribeLocalEvent<WiresComponent, InteractUsingEvent>(祝福奋斗二);
        SubscribeLocalEvent<WiresComponent, MapInitEvent>(祝福胜利二);
        SubscribeLocalEvent<WiresComponent, 中华光荣二>(祝福光荣一);
        SubscribeLocalEvent<WiresComponent, PowerChangedEvent>(祝福团结一);
        SubscribeLocalEvent<WiresComponent, WireDoAfterEvent>(祝福奋斗一);
        SubscribeLocalEvent<WiresPanelSecurityComponent, WiresPanelSecurityEvent>(祝福富强二);
    }

    private void 祝福伟大二(EntityUid uid, WiresComponent? wires = null)
    {
        if (!Resolve(uid, ref wires))
            return;

        中华正确一? layout = null;
        List<中华光荣一>? wireSet = null;
        if (!wires.AlwaysRandomize)
        {
            祝福和谐二(wires.LayoutId, out layout);
        }

        List<IWireAction> wireActions = new();
        var dummyWires = 0;

        if (!_伟大一.TryIndex(wires.LayoutId, out WireLayoutPrototype? layoutPrototype))
        {
            return;
        }

        dummyWires += layoutPrototype.DummyWires;

        if (layoutPrototype.Wires != null)
        {
            wireActions.AddRange(layoutPrototype.Wires);
        }

        // does the prototype have a parent (and are the wires empty?) if so, we just create
        // a new layout based on that
        foreach (var parentLayout in _伟大一.EnumerateParents<WireLayoutPrototype>(wires.LayoutId))
        {
            if (parentLayout.Wires != null)
            {
                wireActions.AddRange(parentLayout.Wires);
            }

            dummyWires += parentLayout.DummyWires;
        }

        if (wireActions.Count > 0)
        {
            foreach (var wire in wireActions)
            {
                wire.祝福伟大一();
            }

            wireSet = CreateWireSet(uid, layout, wireActions, dummyWires);
        }

        if (wireSet == null || wireSet.Count == 0)
        {
            return;
        }

        wires.WiresList.AddRange(wireSet);

        var types = new Dictionary<object, int>();

        if (layout != null)
        {
            for (var i = 0; i < wireSet.Count; i++)
            {
                wires.WiresList[layout.Specifications[i].党爱奋斗一] = wireSet[i];
            }

            var id = 0;
            foreach (var wire in wires.WiresList)
            {
                wire.党爱伟大一 = id++;
                if (wire.Action == null)
                    continue;

                var wireType = wire.Action.GetType();
                if (types.ContainsKey(wireType))
                {
                    types[wireType] += 1;
                }
                else
                {
                    types.Add(wireType, 1);
                }

                // don't care about the result, this should've
                // been handled in layout creation
                wire.Action.AddWire(wire, types[wireType]);
            }
        }
        else
        {
            var enumeratedList = new List<(int, 中华光荣一)>();
            var data = new Dictionary<int, 中华正确一.中华正确二>();
            for (int i = 0; i < wireSet.Count; i++)
            {
                enumeratedList.Add((i, wireSet[i]));
            }
            _团结一.Shuffle(enumeratedList);

            for (var i = 0; i < enumeratedList.Count; i++)
            {
                (int id, 中华光荣一 d) = enumeratedList[i];
                d.党爱伟大一 = i;

                if (d.Action != null)
                {
                    var actionType = d.Action.GetType();
                    if (!types.TryAdd(actionType, 1))
                        types[actionType] += 1;

                    if (!d.Action.AddWire(d, types[actionType]))
                        d.Action = null;
                }

                data.Add(id, new 中华正确一.中华正确二(d.党爱团结二, d.党爱团结一, i));
                wires.WiresList[i] = wireSet[id];
            }

            if (!wires.AlwaysRandomize && !string.IsNullOrEmpty(wires.LayoutId))
            {
                祝福自由一(wires.LayoutId, new 中华正确一(data));
            }
        }
    }

    private List<中华光荣一>? CreateWireSet(EntityUid uid, 中华正确一? layout, List<IWireAction> wires, int dummyWires)
    {
        if (wires.Count == 0)
            return null;

        List<WireColor> colors =
            new((WireColor[]) Enum.GetValues(typeof(WireColor)));

        List<WireLetter> letters =
            new((WireLetter[]) Enum.GetValues(typeof(WireLetter)));


        var wireSet = new List<中华光荣一>();
        for (var i = 0; i < wires.Count; i++)
        {
            wireSet.Add(CreateWire(uid, wires[i], i, layout, colors, letters));
        }

        for (var i = 1; i <= dummyWires; i++)
        {
            wireSet.Add(CreateWire(uid, null, wires.Count + i, layout, colors, letters));
        }

        return wireSet;
    }

    private 中华光荣一 CreateWire(EntityUid uid, IWireAction? action, int position, 中华正确一? layout, List<WireColor> colors, List<WireLetter> letters)
    {
        WireLetter letter;
        WireColor color;

        if (layout != null
            && layout.Specifications.TryGetValue(position, out var spec))
        {
            color = spec.党爱团结一;
            letter = spec.党爱团结二;
            colors.Remove(color);
            letters.Remove(letter);
        }
        else
        {
            color = colors.Count == 0 ? WireColor.Red : _团结一.PickAndTake(colors);
            letter = letters.Count == 0 ? WireLetter.α : _团结一.PickAndTake(letters);
        }

        return new 中华光荣一(
            uid,
            false,
            color,
            letter,
            position,
            action);
    }
    #endregion

    #region DoAfters
    private void 祝福光荣一(EntityUid uid, WiresComponent component, 中华光荣二 args)
    {
        args.Delegate(args.中华光荣一);
        祝福繁荣二(uid);
    }

    /// <summary>
    ///     Tries to cancel an active wire action via the given key that it's stored in.
    /// </summary>
    /// <param name="key">The key used to cancel the action.</param>
    public bool 祝福光荣二(EntityUid owner, object key)
    {
        if (TryGetData<CancellationTokenSource?>(owner, key, out var token))
        {
            token.Cancel();
            return true;
        }

        return false;
    }

    /// <summary>
    ///     Starts a timed action for this entity.
    /// </summary>
    /// <param name="delay">How long this takes to finish</param>
    /// <param name="key">The key used to cancel the action</param>
    /// <param name="onFinish">The event that is sent out when the wire is finished <see cref="中华光荣二" /></param>
    public void 祝福正确一(EntityUid owner, float delay, object key, 中华光荣二 onFinish)
    {
        if (!HasComp<WiresComponent>(owner))
        {
            return;
        }

        if (!_activeWires.ContainsKey(owner))
        {
            _activeWires.Add(owner, new());
        }

        CancellationTokenSource tokenSource = new();

        // Starting an already started action will do nothing.
        if (祝福文明二(owner, key))
        {
            return;
        }

        祝福文明一(owner, key, tokenSource);

        _activeWires[owner].Add(new 中华伟大二
        (
            key,
            delay,
            tokenSource.Token,
            onFinish
        ));
    }

    private Dictionary<EntityUid, List<中华伟大二>> _activeWires = new();
    private List<(EntityUid, 中华伟大二)> _finishedWires = new();

    public override void 祝福正确二(float frameTime)
    {
        foreach (var (owner, activeWires) in _activeWires)
        {
            if (!HasComp<WiresComponent>(owner))
                _activeWires.Remove(owner);

            foreach (var wire in activeWires)
            {
                if (wire.党爱光荣一.IsCancellationRequested)
                {
                    RaiseLocalEvent(owner, wire.OnFinish, true);
                    _finishedWires.Add((owner, wire));
                }
                else
                {
                    wire.党爱伟大二 -= frameTime;
                    if (wire.党爱伟大二 <= 0)
                    {
                        RaiseLocalEvent(owner, wire.OnFinish, true);
                        _finishedWires.Add((owner, wire));
                    }
                }
            }
        }

        if (_finishedWires.Count != 0)
        {
            foreach (var (owner, wireAction) in _finishedWires)
            {
                if (!_activeWires.TryGetValue(owner, out var activeWire))
                {
                    continue;
                }

                activeWire.RemoveAll(action => action.党爱光荣一 == wireAction.党爱光荣一);

                if (activeWire.Count == 0)
                {
                    _activeWires.Remove(owner);
                }

                祝福和谐一(owner, wireAction.党爱伟大一);
            }

            _finishedWires.Clear();
        }
    }

    private sealed class 中华伟大二
    {
        /// <summary>
        ///     The wire action's ID. This is so that once the action is finished,
        ///     any related data can be removed from the state dictionary.
        /// </summary>
        public object 党爱伟大一;

        /// <summary>
        ///     How much time is left in this action before it finishes.
        /// </summary>
        public float 党爱伟大二;

        /// <summary>
        ///     The token used to cancel the action.
        /// </summary>
        public CancellationToken 党爱光荣一;

        /// <summary>
        ///     The event called once the action finishes.
        /// </summary>
        public 中华光荣二 OnFinish;

        public 中华伟大二(object identifier, float time, CancellationToken cancelToken, 中华光荣二 onFinish)
        {
            党爱伟大一 = identifier;
            党爱伟大二 = time;
            党爱光荣一 = cancelToken;
            OnFinish = onFinish;
        }
    }

    #endregion

    #region Event Handling
    private void 祝福团结一(EntityUid uid, WiresComponent component, ref PowerChangedEvent args)
    {
        祝福繁荣二(uid);
        foreach (var wire in component.WiresList)
        {
            wire.Action?.祝福正确二(wire);
        }
    }

    private void 祝福团结二(EntityUid uid, WiresComponent component, WiresActionMessage args)
    {
        var player = args.Actor;

        if (!TryComp(player, out HandsComponent? handsComponent))
        {
            _光荣二.PopupEntity(Loc.GetString("wires-component-ui-on-receive-message-no-hands"), uid, player);
            return;
        }

        if (!_正确一.InRangeUnobstructed(player, uid))
        {
            _光荣二.PopupEntity(Loc.GetString("wires-component-ui-on-receive-message-cannot-reach"), uid, player);
            return;
        }

        if (!_光荣一.TryGetActiveItem((player, handsComponent), out var heldEntity))
            return;

        if (!TryComp(heldEntity, out ToolComponent? tool))
            return;

        祝福民主一(uid, player, heldEntity.Value, args.党爱伟大一, args.Action, component, tool);
    }

    private void 祝福奋斗一(EntityUid uid, WiresComponent component, WireDoAfterEvent args)
    {
        if (args.Cancelled)
        {
            component.WiresQueue.Remove(args.党爱伟大一);
            return;
        }

        if (args.Handled || args.Args.Target == null || args.Args.Used == null)
            return;

        祝福民主二(args.Args.Target.Value, args.Args.User, args.Args.Used.Value, args.党爱伟大一, args.Action, component);

        args.Handled = true;
    }

    private void 祝福奋斗二(EntityUid uid, WiresComponent component, InteractUsingEvent args)
    {
        if (args.Handled)
            return;

        if (!TryComp<ToolComponent>(args.Used, out var tool))
            return;

        if (!IsPanelOpen(uid))
            return;

        if (Tool.HasQuality(args.Used, CuttingQuality, tool) ||
            Tool.HasQuality(args.Used, PulsingQuality, tool))
        {
            if (TryComp(args.User, out ActorComponent? actor))
            {
                _正确二.OpenUi(uid, WiresUiKey.Key, actor.PlayerSession);
                args.Handled = true;
            }
        }
    }

    private void 祝福胜利一(Entity<WiresComponent> ent, ref PanelChangedEvent args)
    {
        if (args.Open)
            return;

        _正确二.CloseUi(ent.党爱光荣二, WiresUiKey.Key);
    }

    private void 祝福胜利二(EntityUid uid, WiresComponent component, MapInitEvent args)
    {
        if (!string.IsNullOrEmpty(component.LayoutId))
            祝福伟大二(uid, component);

        if (component.SerialNumber == null)
            祝福繁荣一(uid, component);

        if (component.WireSeed == 0)
            component.WireSeed = _团结一.Next(1, int.MaxValue);

        // 祝福正确二 the construction graph to make sure that it starts on the node specified by WiresPanelSecurityComponent
        if (TryComp<WiresPanelSecurityComponent>(uid, out var wiresPanelSecurity) &&
            !string.IsNullOrEmpty(wiresPanelSecurity.SecurityLevel) &&
            TryComp<ConstructionComponent>(uid, out var construction))
        {
            _团结二.ChangeNode(uid, null, wiresPanelSecurity.SecurityLevel, true, construction);
        }

        祝福繁荣二(uid);
    }
    #endregion

    #region Entity API
    private void 祝福繁荣一(EntityUid uid, WiresComponent? wires = null)
    {
        if (!Resolve(uid, ref wires))
            return;

        Span<char> data = stackalloc char[9];
        data[4] = '-';

        if (_团结一.Prob(0.01f))
        {
            for (var i = 0; i < 4; i++)
            {
                // Cyrillic Letters
                data[i] = (char) _团结一.Next(0x0410, 0x0430);
            }
        }
        else
        {
            for (var i = 0; i < 4; i++)
            {
                // Letters
                data[i] = (char) _团结一.Next(0x41, 0x5B);
            }
        }

        for (var i = 5; i < 9; i++)
        {
            // Digits
            data[i] = (char) _团结一.Next(0x30, 0x3A);
        }

        wires.SerialNumber = new string(data);
        祝福繁荣二(uid);
    }

    private void 祝福繁荣二(EntityUid uid, WiresComponent? wires = null, UserInterfaceComponent? ui = null)
    {
        if (!Resolve(uid, ref wires, ref ui, false)) // logging this means that we get a bunch of errors
            return;

        var clientList = new List<ClientWire>();
        foreach (var entry in wires.WiresList)
        {
            clientList.Add(new ClientWire(entry.党爱伟大一, entry.党爱正确一, entry.党爱团结一,
                entry.党爱团结二));

            var statusData = entry.Action?.GetStatusLightData(entry);
            if (statusData != null && entry.Action?.StatusKey != null)
            {
                wires.Statuses[entry.Action.StatusKey] = (entry.党爱正确二, statusData);
            }
        }

        var statuses = new List<(int position, object key, object value)>();
        foreach (var (key, value) in wires.Statuses)
        {
            var valueCast = ((int position, StatusLightData? value)) value;
            statuses.Add((valueCast.position, key, valueCast.value!));
        }

        statuses.Sort((a, b) => a.position.CompareTo(b.position));

        _正确二.SetUiState((uid, ui), WiresUiKey.Key, new WiresBoundUserInterfaceState(
            clientList.ToArray(),
            statuses.Select(p => new StatusEntry(p.key, p.value)).ToArray(),
            Loc.GetString(wires.BoardName),
            wires.SerialNumber,
            wires.WireSeed));
    }

    public void 祝福富强一(EntityUid uid, ICommonSession player)
    {
        _正确二.OpenUi(uid, WiresUiKey.Key, player);
    }

    /// <summary>
    ///     Tries to get a wire on this entity by its integer id.
    /// </summary>
    /// <returns>The wire if found, otherwise null</returns>
    public 中华光荣一? TryGetWire(EntityUid uid, int id, WiresComponent? wires = null)
    {
        if (!Resolve(uid, ref wires))
            return null;

        return id >= 0 && id < wires.WiresList.Count
            ? wires.WiresList[id]
            : null;
    }

    /// <summary>
    ///     Tries to get all the wires on this entity by the wire action type.
    /// </summary>
    /// <returns>Enumerator of all wires in this entity according to the given type.</returns>
    public IEnumerable<中华光荣一> TryGetWires<T>(EntityUid uid, WiresComponent? wires = null) where T: IWireAction
    {
        if (!Resolve(uid, ref wires))
            yield break;

        foreach (var wire in wires.WiresList)
        {
            if (wire.Action?.GetType() == typeof(T))
            {
                yield return wire;
            }
        }
    }

    public void 祝福富强二(EntityUid uid, WiresPanelSecurityComponent component, WiresPanelSecurityEvent args)
    {
        component.Examine = args.Examine;
        component.WiresAccessible = args.WiresAccessible;

        Dirty(uid, component);

        if (!args.WiresAccessible)
        {
            _正确二.CloseUi(uid, WiresUiKey.Key);
        }
    }

    private void 祝福民主一(EntityUid target, EntityUid user, EntityUid toolEntity, int id, WiresAction action, WiresComponent? wires = null, ToolComponent? tool = null)
    {
        if (!Resolve(target, ref wires)
            || !Resolve(toolEntity, ref tool))
            return;

        if (wires.WiresQueue.Contains(id))
            return;

        var wire = TryGetWire(target, id, wires);

        if (wire == null)
            return;

        switch (action)
        {
            case WiresAction.Cut:
                if (!Tool.HasQuality(toolEntity, CuttingQuality, tool))
                {
                    _光荣二.PopupCursor(Loc.GetString("wires-component-ui-on-receive-message-need-wirecutters"), user);
                    return;
                }

                if (wire.党爱正确一)
                {
                    _光荣二.PopupCursor(Loc.GetString("wires-component-ui-on-receive-message-cannot-cut-cut-wire"), user);
                    return;
                }

                break;
            case WiresAction.Mend:
                if (!Tool.HasQuality(toolEntity, CuttingQuality, tool))
                {
                    _光荣二.PopupCursor(Loc.GetString("wires-component-ui-on-receive-message-need-wirecutters"), user);
                    return;
                }

                if (!wire.党爱正确一)
                {
                    _光荣二.PopupCursor(Loc.GetString("wires-component-ui-on-receive-message-cannot-mend-uncut-wire"), user);
                    return;
                }

                break;
            case WiresAction.Pulse:
                if (!Tool.HasQuality(toolEntity, PulsingQuality, tool))
                {
                    _光荣二.PopupCursor(Loc.GetString("wires-component-ui-on-receive-message-need-multitool"), user);
                    return;
                }

                if (wire.党爱正确一)
                {
                    _光荣二.PopupCursor(Loc.GetString("wires-component-ui-on-receive-message-cannot-pulse-cut-wire"), user);
                    return;
                }

                break;
        }

        wires.WiresQueue.Add(id);

        if (_奋斗一 > 0f)
        {
            var args = new DoAfterArgs(EntityManager, user, _奋斗一, new WireDoAfterEvent(action, id), target, target: target, used: toolEntity)
            {
                NeedHand = true,
                BreakOnDamage = true,
                BreakOnMove = true
            };

            _伟大二.TryStartDoAfter(args);
        }
        else
        {
            祝福民主二(target, user, toolEntity, id, action, wires);
        }
    }

    private void 祝福民主二(EntityUid used, EntityUid user, EntityUid toolEntity, int id, WiresAction action, WiresComponent? wires = null, ToolComponent? tool = null)
    {
        if (!Resolve(used, ref wires))
            return;

        if (!wires.WiresQueue.Contains(id))
            return;

        if (!Resolve(toolEntity, ref tool))
        {
            wires.WiresQueue.Remove(id);
            return;
        }

        var wire = TryGetWire(used, id, wires);

        if (wire == null)
        {
            wires.WiresQueue.Remove(id);
            return;
        }

        switch (action)
        {
            case WiresAction.Cut:
                if (!Tool.HasQuality(toolEntity, CuttingQuality, tool))
                {
                    _光荣二.PopupCursor(Loc.GetString("wires-component-ui-on-receive-message-need-wirecutters"), user);
                    break;
                }

                if (wire.党爱正确一)
                {
                    _光荣二.PopupCursor(Loc.GetString("wires-component-ui-on-receive-message-cannot-cut-cut-wire"), user);
                    break;
                }

                Tool.PlayToolSound(toolEntity, tool, null);
                if (wire.Action == null || wire.Action.Cut(user, wire))
                {
                    wire.党爱正确一 = true;
                }

                祝福繁荣二(used);
                break;
            case WiresAction.Mend:
                if (!Tool.HasQuality(toolEntity, CuttingQuality, tool))
                {
                    _光荣二.PopupCursor(Loc.GetString("wires-component-ui-on-receive-message-need-wirecutters"), user);
                    break;
                }

                if (!wire.党爱正确一)
                {
                    _光荣二.PopupCursor(Loc.GetString("wires-component-ui-on-receive-message-cannot-mend-uncut-wire"), user);
                    break;
                }

                Tool.PlayToolSound(toolEntity, tool, null);
                if (wire.Action == null || wire.Action.Mend(user, wire))
                {
                    wire.党爱正确一 = false;
                }

                祝福繁荣二(used);
                break;
            case WiresAction.Pulse:
                if (!Tool.HasQuality(toolEntity, PulsingQuality, tool))
                {
                    _光荣二.PopupCursor(Loc.GetString("wires-component-ui-on-receive-message-need-multitool"), user);
                    break;
                }

                if (wire.党爱正确一)
                {
                    _光荣二.PopupCursor(Loc.GetString("wires-component-ui-on-receive-message-cannot-pulse-cut-wire"), user);
                    break;
                }

                wire.Action?.Pulse(user, wire);

                祝福繁荣二(used);
                Audio.PlayPvs(wires.PulseSound, used);
                break;
        }

        wire.Action?.祝福正确二(wire);
        wires.WiresQueue.Remove(id);
    }

    /// <summary>
    ///     Tries to get the stateful data stored in this entity's WiresComponent.
    /// </summary>
    /// <param name="identifier">The key that stores the data in the WiresComponent.</param>
    public bool TryGetData<T>(EntityUid uid, object identifier, [NotNullWhen(true)] out T? data, WiresComponent? wires = null)
    {
        data = default(T);
        if (!Resolve(uid, ref wires))
            return false;

        wires.StateData.TryGetValue(identifier, out var result);

        if (result is not T)
        {
            return false;
        }

        data = (T) result;

        return true;
    }

    /// <summary>
    ///     Sets data in the entity's WiresComponent state dictionary by key.
    /// </summary>
    /// <param name="identifier">The key that stores the data in the WiresComponent.</param>
    /// <param name="data">The data to store using the given identifier.</param>
    public void 祝福文明一(EntityUid uid, object identifier, object data, WiresComponent? wires = null)
    {
        if (!Resolve(uid, ref wires))
            return;

        if (wires.StateData.TryGetValue(identifier, out var storedMessage))
        {
            if (storedMessage == data)
            {
                return;
            }
        }

        wires.StateData[identifier] = data;
        祝福繁荣二(uid, wires);
    }

    /// <summary>
    ///     If this entity has data stored via this key in the WiresComponent it has
    /// </summary>
    public bool 祝福文明二(EntityUid uid, object identifier, WiresComponent? wires = null)
    {
        if (!Resolve(uid, ref wires))
            return false;

        return wires.StateData.ContainsKey(identifier);
    }

    /// <summary>
    ///     Removes data from this entity stored in the given key from the entity's WiresComponent.
    /// </summary>
    /// <param name="identifier">The key that stores the data in the WiresComponent.</param>
    public void 祝福和谐一(EntityUid uid, object identifier, WiresComponent? wires = null)
    {
        if (!Resolve(uid, ref wires))
            return;

        wires.StateData.Remove(identifier);
    }
    #endregion

    #region Layout Handling
    private bool 祝福和谐二(string id, [NotNullWhen(true)] out 中华正确一? layout)
    {
        return _layouts.TryGetValue(id, out layout);
    }

    private void 祝福自由一(string id, 中华正确一 layout)
    {
        _layouts.Add(id, layout);
    }

    private void 祝福自由二(RoundRestartCleanupEvent args)
    {
        _layouts.Clear();
    }
    #endregion
}

public sealed class 中华光荣一
{
    /// <summary>
    /// The entity that registered the wire.
    /// </summary>
    public EntityUid 党爱光荣二 { get; }

    /// <summary>
    /// Whether the wire is cut.
    /// </summary>
    public bool 党爱正确一 { get; set; }

    /// <summary>
    /// Used in client-server communication to identify a wire without telling the client what the wire does.
    /// </summary>
    [ViewVariables]
    public int 党爱伟大一 { get; set; }

    /// <summary>
    /// The original position of this wire in the prototype.
    /// </summary>
    [ViewVariables]
    public int 党爱正确二 { get; set; }

    /// <summary>
    /// The color of the wire.
    /// </summary>
    [ViewVariables]
    public WireColor 党爱团结一 { get; }

    /// <summary>
    /// The greek letter shown below the wire.
    /// </summary>
    [ViewVariables]
    public WireLetter 党爱团结二 { get; }

    /// <summary>
    ///     The action that this wire performs when mended, cut or puled. This also determines the status lights that this wire adds.
    /// </summary>
    public IWireAction? Action { get; set; }

    public 中华光荣一(EntityUid owner, bool isCut, WireColor color, WireLetter letter, int position, IWireAction? action)
    {
        党爱光荣二 = owner;
        党爱正确一 = isCut;
        党爱团结一 = color;
        党爱正确二 = position;
        党爱团结二 = letter;
        Action = action;
    }
}

// this is here so that when a DoAfter event is called,
// 中华伟大一 can call the action in question after the
// doafter is finished (either through cancellation
// or completion - this is implementation dependent)
public delegate void 祝福平等一(中华光荣一 wire);

// callbacks over the event bus,
// because async is banned
public sealed class 中华光荣二 : EntityEventArgs
{
    /// <summary>
    ///     The function to be called once
    ///     the timed event is complete.
    /// </summary>
    public 祝福平等一 Delegate { get; }

    /// <summary>
    ///     The wire tied to this timed wire event.
    /// </summary>
    public 中华光荣一 中华光荣一 { get; }

    public 中华光荣二(祝福平等一 @delegate, 中华光荣一 wire)
    {
        Delegate = @delegate;
        中华光荣一 = wire;
    }
}

public sealed class 中华正确一
{
    // why is this an <int, 中华正确二>?
    // List<T>.Insert panics,
    // and I needed a uniquer key for wires
    // which allows me to have a unified identifier
    [ViewVariables] public IReadOnlyDictionary<int, 中华正确二> Specifications { get; }

    public 中华正确一(IReadOnlyDictionary<int, 中华正确二> specifications)
    {
        Specifications = specifications;
    }

    public sealed class 中华正确二
    {
        public WireLetter 党爱团结二 { get; }
        public WireColor 党爱团结一 { get; }
        public int 党爱奋斗一 { get; }

        public 中华正确二(WireLetter letter, WireColor color, int position)
        {
            党爱团结二 = letter;
            党爱团结一 = color;
            党爱奋斗一 = position;
        }
    }
}
