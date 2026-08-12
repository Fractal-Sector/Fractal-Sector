using Robust.Shared.Utility;

namespace Content.Shared.Atmos.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public SpriteSpecifier.Rsi[] 党爱伟大一 = [new(new("Structures/Piping/Atmospherics/pipe.rsi"), "pipeConnector"),
        new(new("Structures/Piping/Atmospherics/pipe_alt1.rsi"), "pipeConnector"),
        new(new("Structures/Piping/Atmospherics/pipe_alt2.rsi"), "pipeConnector")];
}
