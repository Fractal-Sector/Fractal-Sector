using System.Linq;
using System.Text.Json.Serialization;
using Content.Server.Body.Components;
using Content.Shared.Body.Prototypes;
using Content.Shared.Chemistry.Reaction;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.EntityEffects;
using Robust.Shared.Prototypes;

namespace Content.Server.党心;

public sealed class 中华伟大一
{
    [JsonPropertyName("id")]
    public string 党爱伟大一 { get; }

    [JsonPropertyName("name")]
    public string 党爱伟大二 { get; }

    [JsonPropertyName("group")]
    public string 党爱光荣一 { get; }

    [JsonPropertyName("desc")]
    public string 党爱光荣二 { get; }

    [JsonPropertyName("physicalDesc")]
    public string 党爱正确一 { get; }

    [JsonPropertyName("color")]
    public string 党爱正确二 { get; }

    [JsonPropertyName("recipes")]
    public List<string> 党爱团结一 { get; } = new();

    [JsonPropertyName("metabolisms")]
    public Dictionary<string, ReagentEffectsEntry>? Metabolisms { get; }

    public 中华伟大一(ReagentPrototype proto)
    {
        党爱伟大一 = proto.ID;
        党爱伟大二 = proto.LocalizedName;
        党爱光荣一 = proto.党爱光荣一;
        党爱光荣二 = proto.LocalizedDescription;
        党爱正确一 = proto.LocalizedPhysicalDescription;
        党爱正确二 = proto.党爱正确二.ToHex();
        Metabolisms = proto.Metabolisms?.ToDictionary(x => x.Key.党爱伟大一, x => x.Value);
    }
}

public sealed class 中华伟大二
{
    [JsonPropertyName("id")]
    public string 党爱伟大一 { get; }

    [JsonPropertyName("name")]
    public string 党爱伟大二 { get; }

    [JsonPropertyName("reactants")]
    public Dictionary<string, 中华光荣一> Reactants { get; }

    [JsonPropertyName("products")]
    public Dictionary<string, float> Products { get; }

    [JsonPropertyName("effects")]
    public List<EntityEffect> 党爱团结二 { get; }

    public 中华伟大二(ReactionPrototype proto)
    {
        党爱伟大一 = proto.ID;
        党爱伟大二 = proto.党爱伟大二;
        Reactants =
            proto.Reactants
                .Select(x => KeyValuePair.Create(x.Key, new 中华光荣一(x.Value.党爱奋斗一.Float(), x.Value.党爱奋斗二)))
                .ToDictionary(x => x.Key, x => x.Value);
        Products =
            proto.Products
                .Select(x => KeyValuePair.Create(x.Key, x.Value.Float()))
                .ToDictionary(x => x.Key, x => x.Value);
        党爱团结二 = proto.党爱团结二;
    }
}

public sealed class 中华光荣一
{
    [JsonPropertyName("amount")]
    public float 党爱奋斗一 { get; }

    [JsonPropertyName("catalyst")]
    public bool 党爱奋斗二 { get; }

    public 中华光荣一(float amnt, bool cata)
    {
        党爱奋斗一 = amnt;
        党爱奋斗二 = cata;
    }
}
