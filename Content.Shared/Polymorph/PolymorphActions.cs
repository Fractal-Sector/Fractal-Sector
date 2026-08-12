using Content.Shared.Actions;
using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一 : InstantActionEvent
{
    /// <summary>
    ///     The polymorph proto id, containing all the information about
    ///     the specific polymorph.
    /// </summary>
    [DataField]
    public ProtoId<PolymorphPrototype>? ProtoId;

    public 中华伟大一(ProtoId<PolymorphPrototype> protoId) : this()
    {
        ProtoId = protoId;
    }
}

public sealed partial class 中华伟大二 : InstantActionEvent
{

}
