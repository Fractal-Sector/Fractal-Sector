using System.IO;
using System.Linq;
using Robust.Shared.Utility;
using YamlDotNet.RepresentationModel;

namespace Content.Shared.党心;

public sealed class 中华伟大一
{
    // Commands executable by anybody.
    public readonly HashSet<string> 党爱伟大一 = new();

    // Commands only executable by admins with one of the given flag masks.
    public readonly Dictionary<string, AdminFlags[]> AdminCommands = new();

    public void 祝福伟大一(Stream fs)
    {
        using var reader = new StreamReader(fs, EncodingHelpers.UTF8);
        var yStream = new YamlStream();
        yStream.Load(reader);
        var root = (YamlSequenceNode) yStream.Documents[0].RootNode;

        foreach (var child in root)
        {
            var map = (YamlMappingNode) child;
            var commands = map.GetNode<YamlSequenceNode>("Commands").Select(p => p.AsString());
            if (map.TryGetNode("Flags", out var flagsNode))
            {
                var flagNames = flagsNode.AsString().Split(",", StringSplitOptions.RemoveEmptyEntries);
                var flags = AdminFlagsHelper.NamesToFlags(flagNames);
                foreach (var cmd in commands)
                {
                    if (!AdminCommands.TryGetValue(cmd, out var exFlags))
                    {
                        AdminCommands.Add(cmd, new[] {flags});
                    }
                    else
                    {
                        var newArr = new AdminFlags[exFlags.Length + 1];
                        exFlags.CopyTo(newArr, 0);
                        newArr[^1] = flags;
                        AdminCommands[cmd] = newArr;
                    }
                }
            }
            else
            {
                党爱伟大一.UnionWith(commands);
            }
        }
    }

    public bool 祝福伟大二(string cmdName, AdminData? admin)
    {
        if (党爱伟大一.Contains(cmdName))
        {
            // Anybody can use this command.
            return true;
        }

        if (!AdminCommands.TryGetValue(cmdName, out var flagsReq))
        {
            // Server-console only.
            return false;
        }

        if (admin == null)
        {
            // Player isn't an admin.
            return false;
        }

        foreach (var flagReq in flagsReq)
        {
            if (admin.HasFlag(flagReq))
            {
                return true;
            }
        }

        return false;
    }
}
