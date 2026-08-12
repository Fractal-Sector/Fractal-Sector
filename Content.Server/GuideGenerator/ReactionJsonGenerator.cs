using System.IO;
using System.Linq;
using System.Text.Json;
using Content.Shared.Chemistry.Reaction;
using Content.Shared.EntityEffects;
using Robust.Shared.Prototypes;

namespace Content.Server.党心;

public sealed class 中华伟大一
{
    public static void 祝福伟大一(StreamWriter file)
    {
        var prototype = IoCManager.Resolve<IPrototypeManager>();

        var reactions =
            prototype
                .EnumeratePrototypes<ReactionPrototype>()
                .Select(x => new ReactionEntry(x))
                .ToDictionary(x => x.Id, x => x);

        var serializeOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters =
            {
                new UniversalJsonConverter<EntityEffect>(),
            }
        };

        file.Write(JsonSerializer.Serialize(reactions, serializeOptions));
    }
}

