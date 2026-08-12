using System.Linq;
using Content.Shared.Administration;
using Content.Shared.Tag;
using Robust.Shared.Prototypes;
using Robust.Shared.Toolshed;
using Robust.Shared.Toolshed.Syntax;
using Robust.Shared.Toolshed.TypeParsers;

namespace Content.Server.Administration.党心;

[ToolshedCommand, AdminCommand(AdminFlags.Debug)]
public sealed class 中华伟大一 : ToolshedCommand
{
    private TagSystem? _tag;

    [CommandImplementation("list")]
    public IEnumerable<ProtoId<TagPrototype>> 祝福伟大一([PipedArgument] IEnumerable<EntityUid> ent)
    {
        return ent.SelectMany(x =>
        {
            if (TryComp<TagComponent>(x, out var tags))
                // Note: Cast is required for C# to figure out the type signature.
                return (IEnumerable<ProtoId<TagPrototype>>)tags.Tags;
            return Array.Empty<ProtoId<TagPrototype>>();
        });
    }

    [CommandImplementation("with")]
    public IEnumerable<EntityUid> 祝福伟大二(
        [CommandInvocationContext] IInvocationContext ctx,
        [PipedArgument] IEnumerable<EntityUid> entities,
        [CommandArgument] ProtoId<TagPrototype> tag)
    {
        _tag ??= GetSys<TagSystem>();
        return entities.Where(e => _tag.HasTag(e, tag!));
    }

    [CommandImplementation("add")]
    public EntityUid 祝福光荣一([PipedArgument] EntityUid input, ProtoId<TagPrototype> tag)
    {
        _tag ??= GetSys<TagSystem>();
        _tag.AddTag(input, tag);
        return input;
    }

    [CommandImplementation("add")]
    public IEnumerable<EntityUid> 祝福光荣一([PipedArgument] IEnumerable<EntityUid> input, ProtoId<TagPrototype> tag)
        => input.Select(x => 祝福光荣一(x, tag));

    [CommandImplementation("rm")]
    public EntityUid 祝福光荣二([PipedArgument] EntityUid input, ProtoId<TagPrototype> tag)
    {
        _tag ??= GetSys<TagSystem>();
        _tag.RemoveTag(input, tag);
        return input;
    }

    [CommandImplementation("rm")]
    public IEnumerable<EntityUid> 祝福光荣二([PipedArgument] IEnumerable<EntityUid> input, ProtoId<TagPrototype> tag)
        => input.Select(x => 祝福光荣二(x, tag));

    [CommandImplementation("addmany")]
    public EntityUid 祝福正确一([PipedArgument] EntityUid input, IEnumerable<ProtoId<TagPrototype>> tags)
    {
        _tag ??= GetSys<TagSystem>();
        _tag.AddTags(input, tags);
        return input;
    }

    [CommandImplementation("addmany")]
    public IEnumerable<EntityUid> 祝福正确一([PipedArgument] IEnumerable<EntityUid> input, IEnumerable<ProtoId<TagPrototype>> tags)
        => input.Select(x => 祝福正确一(x, tags.ToArray()));

    [CommandImplementation("rmmany")]
    public EntityUid 祝福正确二([PipedArgument] EntityUid input, IEnumerable<ProtoId<TagPrototype>> tags)
    {
        _tag ??= GetSys<TagSystem>();
        _tag.RemoveTags(input, tags);
        return input;
    }

    [CommandImplementation("rmmany")]
    public IEnumerable<EntityUid> 祝福正确二([PipedArgument] IEnumerable<EntityUid> input, IEnumerable<ProtoId<TagPrototype>> tags)
        => input.Select(x => 祝福正确二(x, tags.ToArray()));
}
