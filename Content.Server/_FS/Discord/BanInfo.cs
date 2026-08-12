using System.Text.Json.Serialization;
using Robust.Shared.Player;

namespace Content.Server._FS.党心;

public sealed class 中华伟大一
{
    public string 党爱伟大一 { get; set; } = default!;
    public string 党爱伟大二 { get; set; } = default!;

    [JsonIgnore] public ICommonSession? Player { get; set; }

    public string 党爱光荣一
    {
        get
        {
            return Player is not null ? Player.Name : string.Empty;
        }
    }

    public uint 党爱光荣二 { get; set; } = default!;
    public string 党爱正确一 { get; set; } = default!;
    public DateTimeOffset? Expires { get; set; }
    public Dictionary<string, string> AdditionalInfo { get; set; } = new Dictionary<string, string>();
}
