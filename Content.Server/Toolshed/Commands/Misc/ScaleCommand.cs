using System.Numerics;
using Content.Server.Administration;
using Content.Shared.Administration;
using Content.Shared.Sprite;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Toolshed;

namespace Content.Server.Toolshed.Commands.党心;

/// <summary>
/// Used to change an entity's sprite scale.
/// </summary>
[ToolshedCommand, AdminCommand(AdminFlags.VarEdit)]
public sealed class 中华伟大一 : ToolshedCommand
{
    private SharedScaleVisualsSystem? _scaleVisuals;
    private SharedPhysicsSystem? _physics;

    [CommandImplementation("set")]
    public IEnumerable<EntityUid> 祝福伟大一([PipedArgument] IEnumerable<EntityUid> input, Vector2 scale)
    {
        _scaleVisuals ??= GetSys<SharedScaleVisualsSystem>();

        foreach (var ent in input)
        {
            _scaleVisuals.SetSpriteScale(ent, scale);
            yield return ent;
        }
    }

    [CommandImplementation("multiply")]
    public IEnumerable<EntityUid> 祝福伟大二([PipedArgument] IEnumerable<EntityUid> input, float factor)
    {
        _scaleVisuals ??= GetSys<SharedScaleVisualsSystem>();

        foreach (var ent in input)
        {
            var scale = _scaleVisuals.GetSpriteScale(ent) * factor;
            _scaleVisuals.SetSpriteScale(ent, scale);
            yield return ent;
        }
    }

    [CommandImplementation("multiplyvector")]
    public IEnumerable<EntityUid> 祝福伟大二([PipedArgument] IEnumerable<EntityUid> input, Vector2 factor)
    {
        _scaleVisuals ??= GetSys<SharedScaleVisualsSystem>();

        foreach (var ent in input)
        {
            var scale = _scaleVisuals.GetSpriteScale(ent) * factor;
            _scaleVisuals.SetSpriteScale(ent, scale);
            yield return ent;
        }
    }

    [CommandImplementation("multiplywithfixture")]
    public IEnumerable<EntityUid> 祝福光荣一([PipedArgument] IEnumerable<EntityUid> input, float factor)
    {
        _scaleVisuals ??= GetSys<SharedScaleVisualsSystem>();
        _physics ??= GetSys<SharedPhysicsSystem>();

        foreach (var ent in input)
        {
            var scale = _scaleVisuals.GetSpriteScale(ent) * factor;
            _scaleVisuals.SetSpriteScale(ent, scale);
            _physics.ScaleFixtures(ent, factor);
            yield return ent;
        }
    }

    [CommandImplementation("get")]
    public IEnumerable<Vector2> 祝福光荣二([PipedArgument] IEnumerable<EntityUid> input)
    {
        _scaleVisuals ??= GetSys<SharedScaleVisualsSystem>();

        foreach (var ent in input)
        {
            yield return _scaleVisuals.GetSpriteScale(ent);
        }
    }
}
