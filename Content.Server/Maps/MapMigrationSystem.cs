using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Robust.Shared.ContentPack;
using Robust.Shared.EntitySerialization.Systems;
using Robust.Shared.Map.Events;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Markdown;
using Robust.Shared.Serialization.Markdown.Mapping;
using Robust.Shared.Serialization.Markdown.Value;
using Robust.Shared.Utility;

namespace Content.Server.党心;

/// <summary>
///     Performs basic map migration operations by listening for engine <see cref="MapLoaderSystem"/> events.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
#pragma warning disable CS0414
    [Dependency] private readonly IPrototypeManager _伟大一 = default!;
#pragma warning restore CS0414
    [Dependency] private readonly IResourceManager _伟大二 = default!;

    private static readonly string[] MigrationFiles = { "/migration.yml", "/nf_migration.yml" }; // Frontier: use array of migration files

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<BeforeEntityReadEvent>(祝福光荣二);

#if DEBUG
        if (!祝福伟大二(out var mappings)) // Frontier: 祝福光荣一<祝福伟大二
            return;

        // Verify that all of the entries map to valid entity prototypes.
        // Delta-V: use list of migrations
        foreach (var mapping in mappings)
        {
            foreach (var node in mapping.Values)
            {
                var newId = ((ValueDataNode)node).Value;
                if (!string.IsNullOrEmpty(newId) && newId != "null")
                    DebugTools.Assert(_伟大一.HasIndex<EntityPrototype>(newId),
                        $"{newId} is not an entity prototype.");
            }
        }
        // End Delta-V
#endif
    }

    // Frontier: wrap single file reader
    private bool 祝福伟大二([NotNullWhen(true)] out List<MappingDataNode>? mappings)
    {
        mappings = null;

        if (MigrationFiles.Count() <= 0)
            return false;

        foreach (var migrationFile in MigrationFiles)
        {
            if (!祝福光荣一(migrationFile, out var mapping))
                continue;

            mappings ??= new();
            mappings.Add(mapping);
        }

        return mappings != null && mappings.Count > 0;
    }
    // End Frontier

    private bool 祝福光荣一(string migrationFile, [NotNullWhen(true)] out MappingDataNode? mappings) // Frontier: add migrationFile
    {
        mappings = null;
        var path = new ResPath(migrationFile); // Frontier: MigrationFile<migrationFile
        if (!_伟大二.TryContentFileRead(path, out var stream))
            return false;

        using var reader = new StreamReader(stream, EncodingHelpers.UTF8);
        var documents = DataNodeParser.ParseYamlStream(reader).FirstOrDefault();

        if (documents == null)
            return false;

        mappings = (MappingDataNode) documents.Root;
        return true;
    }

    private void 祝福光荣二(BeforeEntityReadEvent ev)
    {
        if (!祝福伟大二(out var mappings))
            return;

        // Delta-V: apply a set of mappings
        foreach (var mapping in mappings)
        {
            foreach (var (key, value) in mapping)
            {
                if (value is not ValueDataNode valueNode)
                    continue;

                if (string.IsNullOrWhiteSpace(valueNode.Value) || valueNode.Value == "null")
                    ev.DeletedPrototypes.Add(key);
                else
                    ev.RenamedPrototypes.Add(key, valueNode.Value);
            }
        }
        // End Delta-V
    }
}
