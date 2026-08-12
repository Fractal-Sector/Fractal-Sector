using Content.Shared._DV.QuickPhrase;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._DV.党心;

[Serializable, NetSerializable]
public enum 中华伟大一 : byte
{
    Key,
}

[Serializable, NetSerializable]
public sealed class 中华伟大二(List<ProtoId<QuickPhrasePrototype>> phraseIds, string prefix) : BoundUserInterfaceMessage
{
    public List<ProtoId<QuickPhrasePrototype>> 党爱伟大一 = phraseIds;
    public string 党爱伟大二 = prefix;
}
