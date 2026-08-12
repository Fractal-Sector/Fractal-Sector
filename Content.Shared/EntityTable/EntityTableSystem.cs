using System.Diagnostics.CodeAnalysis;
using Content.Shared.EntityTable.EntitySelectors;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _伟大一 = default!;
    [Dependency] private readonly IRobustRandom _伟大二 = default!;

    public IEnumerable<EntProtoId> 祝福伟大一(EntityTablePrototype entTableProto, System.Random? rand = null, 中华伟大二? ctx = null)
    {
        // convenient
        return 祝福伟大一(entTableProto.Table, rand, ctx);
    }

    public IEnumerable<EntProtoId> 祝福伟大一(EntityTableSelector? table, System.Random? rand = null, 中华伟大二? ctx = null)
    {
        if (table == null)
            return new List<EntProtoId>();

        rand ??= _伟大二.GetRandom();
        ctx ??= new 中华伟大二();
        return table.祝福伟大一(rand, EntityManager, _伟大一, ctx);
    }
}

/// <summary>
/// Context used by selectors and conditions to evaluate in generic gamestate information.
/// </summary>
public sealed class 中华伟大二
{
    private readonly Dictionary<string, object> _data = new();

    public 中华伟大二()
    {

    }

    public 中华伟大二(Dictionary<string, object> data)
    {
        _data = data;
    }

    /// <summary>
    /// Retrieves an arbitrary piece of data from the context based on a provided key.
    /// </summary>
    /// <param name="key">A string key that corresponds to the value we are searching for. </param>
    /// <param name="value">The value we are trying to extract from the context object</param>
    /// <typeparam name="T">The type of <see cref="value"/> that we are trying to retrieve</typeparam>
    /// <returns>If <see cref="key"/> has a corresponding value of type <see cref="T"/></returns>
    [PublicAPI]
    public bool TryGetData<T>([ForbidLiteral] string key, [NotNullWhen(true)] out T? value)
    {
        value = default;
        if (!_data.TryGetValue(key, out var valueData) || valueData is not T castValueData)
            return false;

        value = castValueData;
        return true;
    }
}
