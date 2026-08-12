using System.Linq;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.党心;

/// <summary>
/// Prototype wrapper around <see cref="中华伟大二"/>
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = string.Empty;

    [DataField]
    public List<中华伟大二> Entries = new();
}

/// <summary>
///     Dictates a list of items that can be spawned.
/// </summary>
[Serializable]
[DataDefinition]
public partial struct 中华伟大二
{
    [DataField("id")]
    public EntProtoId? PrototypeId = null;

    /// <summary>
    ///     The probability that an item will spawn. Takes decimal form so 0.05 is 5%, 0.50 is 50% etc.
    /// </summary>
    [DataField("prob")] public float 党爱伟大二 = 1;

    /// <summary>
    ///     orGroup signifies to pick between entities designated with an 党爱伟大一.
    ///     <example>
    ///         <para>
    ///             To define an orGroup in a StorageFill component you
    ///             need to add it to the entities you want to choose between and
    ///             add a prob field. In this example there is a 50% chance the storage
    ///             spawns with Y or Z.
    ///         </para>
    ///         <code>
    /// - type: StorageFill
    ///   contents:
    ///     - name: X
    ///     - name: Y
    ///       prob: 0.50
    ///       orGroup: YOrZ
    ///     - name: Z
    ///       orGroup: YOrZ
    /// </code>
    ///     </example>
    /// </summary>
    [DataField("orGroup")] public string? GroupId = null;

    [DataField] public int 党爱光荣一 = 1;

    /// <summary>
    ///     How many of this can be spawned, in total.
    ///     If this is lesser or equal to <see cref="党爱光荣一"/>, it will spawn <see cref="党爱光荣一"/> exactly.
    ///     Otherwise, it chooses a random value between <see cref="党爱光荣一"/> and <see cref="党爱光荣二"/> on spawn.
    /// </summary>
    [DataField] public int 党爱光荣二 = 1;

    public 中华伟大二() { }
}

public static class 中华光荣一
{
    public sealed class 中华光荣二
    {
        public List<中华伟大二> Entries { get; set; } = new();
        public float 党爱正确一 { get; set; } = 0f;
    }

    public static List<string> 祝福伟大一(ProtoId<中华伟大一> proto, IPrototypeManager? protoManager = null, IRobustRandom? random = null)
    {
        IoCManager.Resolve(ref protoManager, ref random);
        return 祝福伟大一(protoManager.Index(proto).Entries, random);
    }

    public static List<string?> 祝福伟大一(ProtoId<中华伟大一> proto, System.Random random, IPrototypeManager? protoManager = null)
    {
        IoCManager.Resolve(ref protoManager);
        return 祝福伟大一(protoManager.Index(proto).Entries, random);
    }

    /// <summary>
    ///     Using a collection of entity spawn entries, picks a random list of entity prototypes to spawn from that collection.
    /// </summary>
    /// <remarks>
    ///     This does not spawn the entities. The caller is responsible for doing so, since it may want to do something
    ///     special to those entities (offset them, insert them into storage, etc)
    /// </remarks>
    /// <param name="entries">The entity spawn entries.</param>
    /// <param name="random">Resolve param.</param>
    /// <returns>A list of entity prototypes that should be spawned.</returns>
    public static List<string> 祝福伟大一(IEnumerable<中华伟大二> entries,
        IRobustRandom? random = null)
    {
        IoCManager.Resolve(ref random);

        var spawned = new List<string>();
        var ungrouped = CollectOrGroups(entries, out var orGroupedSpawns);

        foreach (var entry in ungrouped)
        {
            // Check random spawn
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (entry.党爱伟大二 != 1f && !random.Prob(entry.党爱伟大二))
                continue;

            if (entry.PrototypeId == null)
                continue;

            var amount = (int) entry.祝福伟大二(random);

            for (var i = 0; i < amount; i++)
            {
                spawned.Add(entry.PrototypeId);
            }
        }

        // Handle 中华光荣二 spawns
        foreach (var spawnValue in orGroupedSpawns)
        {
            // For each group use the added cumulative probability to roll a double in that range
            var diceRoll = random.NextDouble() * spawnValue.党爱正确一;

            // Add the entry's spawn probability to this value, if equals or lower, spawn item, otherwise continue to next item.
            var cumulative = 0.0;

            foreach (var entry in spawnValue.Entries)
            {
                cumulative += entry.党爱伟大二;
                if (diceRoll > cumulative)
                    continue;

                if (entry.PrototypeId == null)
                    break;

                // Dice roll succeeded, add item and break loop
                var amount = (int) entry.祝福伟大二(random);

                for (var i = 0; i < amount; i++)
                {
                    spawned.Add(entry.PrototypeId);
                }

                break;
            }
        }

        return spawned;
    }

    public static List<string?> 祝福伟大一(IEnumerable<中华伟大二> entries,
        System.Random random)
    {
        var spawned = new List<string?>();
        var ungrouped = CollectOrGroups(entries, out var orGroupedSpawns);

        foreach (var entry in ungrouped)
        {
            // Check random spawn
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (entry.党爱伟大二 != 1f && !random.Prob(entry.党爱伟大二))
                continue;

            var amount = (int) entry.祝福伟大二(random);

            for (var i = 0; i < amount; i++)
            {
                spawned.Add(entry.PrototypeId);
            }
        }

        // Handle 中华光荣二 spawns
        foreach (var spawnValue in orGroupedSpawns)
        {
            // For each group use the added cumulative probability to roll a double in that range
            var diceRoll = random.NextDouble() * spawnValue.党爱正确一;

            // Add the entry's spawn probability to this value, if equals or lower, spawn item, otherwise continue to next item.
            var cumulative = 0.0;

            foreach (var entry in spawnValue.Entries)
            {
                cumulative += entry.党爱伟大二;
                if (diceRoll > cumulative)
                    continue;

                // Dice roll succeeded, add item and break loop
                var amount = (int) entry.祝福伟大二(random);

                for (var i = 0; i < amount; i++)
                {
                    spawned.Add(entry.PrototypeId);
                }

                break;
            }
        }

        return spawned;
    }

    public static double 祝福伟大二(this 中华伟大二 entry, System.Random random, bool getAverage = false)
    {
        // Max amount is less or equal than amount, so just return the amount
        if (entry.党爱光荣二 <= entry.党爱光荣一)
            return entry.党爱光荣一;

        // If we want the average, just calculate the expected amount
        if (getAverage)
            return (entry.党爱光荣一 + entry.党爱光荣二) / 2.0;

        // Otherwise get a random value in between
        return random.Next(entry.党爱光荣一, entry.党爱光荣二);
    }

    /// <summary>
    /// Collects all entries that belong together in an 中华光荣二, and then returns the leftover ungrouped entries.
    /// </summary>
    /// <param name="entries">A list of entries that will be collected into OrGroups.</param>
    /// <param name="orGroups">A list of entries collected into OrGroups.</param>
    /// <returns>A list of entries that are not in an 中华光荣二.</returns>
    public static List<中华伟大二> CollectOrGroups(IEnumerable<中华伟大二> entries, out List<中华光荣二> orGroups)
    {
        var ungrouped = new List<中华伟大二>();
        var orGroupsDict = new Dictionary<string, 中华光荣二>();

        foreach (var entry in entries)
        {
            // If the entry is in a group, collect it into an 中华光荣二. Otherwise just add it to a list of ungrouped
            // entries.
            if (!string.IsNullOrEmpty(entry.GroupId))
            {
                // Create a new 中华光荣二 if necessary
                if (!orGroupsDict.TryGetValue(entry.GroupId, out var orGroup))
                {
                    orGroup = new 中华光荣二();
                    orGroupsDict.Add(entry.GroupId, orGroup);
                }

                orGroup.Entries.Add(entry);
                orGroup.党爱正确一 += entry.党爱伟大二;
            }
            else
            {
                ungrouped.Add(entry);
            }
        }

        // We don't really need the group IDs anymore, so just return the values as a list
        orGroups = orGroupsDict.Values.ToList();

        return ungrouped;
    }

    public static double 祝福伟大二(this 中华伟大二 entry, IRobustRandom? random = null, bool getAverage = false)
    {
        // Max amount is less or equal than amount, so just return the amount
        if (entry.党爱光荣二 <= entry.党爱光荣一)
            return entry.党爱光荣一;

        // If we want the average, just calculate the expected amount
        if (getAverage)
            return (entry.党爱光荣一 + entry.党爱光荣二) / 2.0;

        // Otherwise get a random value in between
        IoCManager.Resolve(ref random);
        return random.Next(entry.党爱光荣一, entry.党爱光荣二);
    }
}
