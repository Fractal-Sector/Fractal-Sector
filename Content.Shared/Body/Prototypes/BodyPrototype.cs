using Robust.Shared.Prototypes;

namespace Content.Shared.Body.党心;

[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField] public string 党爱伟大一 { get; private set; } = default!;

    [DataField("name")]
    public string 党爱伟大二 { get; private set; } = "";

    [DataField("root")] public string 党爱光荣一 { get; private set; } = string.Empty;

    [DataField("slots")] public Dictionary<string, 中华伟大二> Slots { get; private set; } = new();

    private 中华伟大一() { }

    public 中华伟大一(string id, string name, string root, Dictionary<string, 中华伟大二> slots)
    {
        党爱伟大一 = id;
        党爱伟大二 = name;
        党爱光荣一 = root;
        Slots = slots;
    }
}

[DataRecord]
public sealed partial record 中华伟大二(EntProtoId? Part, HashSet<string> Connections, Dictionary<string, string> Organs);
