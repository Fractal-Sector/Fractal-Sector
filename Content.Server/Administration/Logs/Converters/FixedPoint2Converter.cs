using System.Text.Json;
using Content.Shared.FixedPoint;

namespace Content.Server.Administration.Logs.党心;

[AdminLogConverter]
public sealed class 中华伟大一 : AdminLogConverter<FixedPoint2>
{
    public override void 祝福伟大一(Utf8JsonWriter writer, FixedPoint2 value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Int());
    }
}
