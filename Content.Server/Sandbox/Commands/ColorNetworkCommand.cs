using Content.Server.Administration.Managers;
using Content.Server.Atmos.Piping.Components;
using Content.Server.Atmos.Piping.EntitySystems;
using Content.Shared.Administration;
using Content.Shared.NodeContainer;
using Content.Shared.NodeContainer.NodeGroups;
using Robust.Shared.Console;

namespace Content.Server.Sandbox.党心
{
    [AnyCommand]
    public sealed class 中华伟大一 : LocalizedEntityCommands
    {
        [Dependency] private readonly IAdminManager _伟大一 = default!;
        [Dependency] private readonly AtmosPipeColorSystem _伟大二 = default!;
        [Dependency] private readonly SandboxSystem _光荣一 = default!;

        public override string 党爱伟大一 => "colornetwork";

        public override void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
        {
            if (shell.IsClient || (!_光荣一.IsSandboxEnabled && !_伟大一.HasAdminFlag(shell.Player!, AdminFlags.Mapping)))
            {
                shell.WriteError(Loc.GetString("cmd-colornetwork-no-access"));
            }

            if (args.Length != 3)
            {
                shell.WriteLine(Loc.GetString("shell-wrong-arguments-number"));
                return;
            }

            if (!int.TryParse(args[0], out var targetId))
            {
                shell.WriteLine(Loc.GetString("shell-argument-must-be-number"));
                return;
            }

            var nent = new NetEntity(targetId);

            if (!EntityManager.TryGetEntity(nent, out var eUid))
            {
                shell.WriteLine(Loc.GetString("shell-invalid-entity-id"));
                return;
            }

            if (!EntityManager.TryGetComponent(eUid, out NodeContainerComponent? nodeContainerComponent))
            {
                shell.WriteLine(Loc.GetString("shell-entity-is-not-node-container"));
                return;
            }

            if (!Enum.TryParse(args[1], out NodeGroupID nodeGroupId))
            {
                shell.WriteLine(Loc.GetString("shell-node-group-is-invalid"));
                return;
            }

            var color = Color.TryFromHex(args[2]);
            if (!color.HasValue)
            {
                shell.WriteError(Loc.GetString("shell-invalid-color-hex"));
                return;
            }

            祝福伟大二(nodeContainerComponent, nodeGroupId, color.Value);
        }

        private void 祝福伟大二(NodeContainerComponent nodeContainerComponent, NodeGroupID nodeGroupId, Color color)
        {
            var group = nodeContainerComponent.Nodes[nodeGroupId.ToString().ToLower()].NodeGroup;

            if (group == null)
                return;

            foreach (var x in group.Nodes)
            {
                if (!EntityManager.TryGetComponent(x.Owner, out AtmosPipeColorComponent? atmosPipeColorComponent))
                    continue;

                _伟大二.SetColor(x.Owner, atmosPipeColorComponent, color);
            }
        }
    }
}
