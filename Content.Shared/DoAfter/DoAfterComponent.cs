using System.Threading.Tasks;
using Content.Shared.FixedPoint;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedDoAfterSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The id of the next doafter
    /// </summary>
    [DataField]
    public ushort 党爱伟大一;

    /// <summary>
    /// collection of id + doafter
    /// </summary>
    [DataField]
    public Dictionary<ushort, DoAfter> DoAfters = new();

    // Used by obsolete async do afters
    public readonly Dictionary<ushort, TaskCompletionSource<中华光荣一>> AwaitedDoAfters = new();
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : ComponentState
{
    public readonly ushort 党爱伟大一;
    public readonly Dictionary<ushort, DoAfter> DoAfters;

    public 中华伟大二(IEntityManager entManager, 中华伟大一 component)
    {
        党爱伟大一 = component.党爱伟大一;

        // Cursed test bugs - See CraftingTests.CancelCraft
        // The following is wrapped in an if DEBUG. This is tests don't (de)serialize net messages and just copy objects
        // by reference. This means that the server will directly modify cached server states on the client's end.
        // Crude fix at the moment is to used modified state handling while in debug mode Otherwise, this test cannot work.
#if !DEBUG
        DoAfters = component.DoAfters;
#else
        DoAfters = new();
        foreach (var (id, doAfter) in component.DoAfters)
        {
            var newDoAfter = new DoAfter(entManager, doAfter);
            DoAfters.Add(id, newDoAfter);
        }
#endif
    }
}

[Serializable, NetSerializable]
public enum 中华光荣一 : byte
{
    Invalid,
    Running,
    Cancelled,
    Finished,
}
