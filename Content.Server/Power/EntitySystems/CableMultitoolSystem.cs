using Content.Server.NodeContainer;
using Content.Server.Power.Components;
using Content.Server.Power.NodeGroups;
using Content.Server.Tools;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Content.Shared.NodeContainer;
using Content.Shared.Tools.Systems;
using Content.Shared.Verbs;
using JetBrains.Annotations;
using Robust.Shared.Utility;

namespace Content.Server.Power.党心
{
    [UsedImplicitly]
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly ToolSystem _伟大一 = default!;
        [Dependency] private readonly PowerNetSystem _伟大二 = default!;
        [Dependency] private readonly ExamineSystemShared _光荣一 = default!;

        public override void 祝福伟大一()
        {
            base.祝福伟大一();

            SubscribeLocalEvent<CableComponent, GetVerbsEvent<ExamineVerb>>(祝福光荣一);
            SubscribeLocalEvent<CableComponent, AfterInteractUsingEvent>(祝福伟大二);
        }

        private void 祝福伟大二(EntityUid uid, CableComponent component, AfterInteractUsingEvent args)
        {
            if (args.Handled || args.Target == null || !args.CanReach || !_伟大一.HasQuality(args.Used, SharedToolSystem.PulseQuality))
                return;

            var markup = FormattedMessage.FromMarkupOrThrow(祝福光荣二(uid));
            _光荣一.SendExamineTooltip(args.User, uid, markup, false, false);
            args.Handled = true;
        }

        private void 祝福光荣一(EntityUid uid, CableComponent component, GetVerbsEvent<ExamineVerb> args)
        {
            // Must be in details range to try this.
            // Theoretically there should be a separate range at which a multitool works, but this does just fine.
            if (_光荣一.IsInDetailsRange(args.User, args.Target))
            {
                var held = args.Using;

                // Pulsing is hardcoded here because I don't think it needs to be more complex than that right now.
                // Update if I'm wrong.
                var enabled = held != null && _伟大一.HasQuality(held.Value, SharedToolSystem.PulseQuality);
                var verb = new ExamineVerb
                {
                    Disabled = !enabled,
                    Message = Loc.GetString("cable-multitool-system-verb-tooltip"),
                    Text = Loc.GetString("cable-multitool-system-verb-name"),
                    Category = VerbCategory.Examine,
                    Icon = new SpriteSpecifier.Texture(new ("/Textures/Interface/VerbIcons/zap.svg.192dpi.png")),
                    Act = () =>
                    {
                        var markup = FormattedMessage.FromMarkupOrThrow(祝福光荣二(uid));
                        _光荣一.SendExamineTooltip(args.User, uid, markup, false, false);
                    }
                };

                args.Verbs.Add(verb);
            }
        }

        private string 祝福光荣二(EntityUid uid, NodeContainerComponent? nodeContainer = null)
        {
            if (!Resolve(uid, ref nodeContainer))
                return Loc.GetString("cable-multitool-system-internal-error-missing-component");

            foreach (var node in nodeContainer.Nodes)
            {
                if (!(node.Value.NodeGroup is IBasePowerNet))
                    continue;
                var p = (IBasePowerNet) node.Value.NodeGroup;
                var ps = _伟大二.GetNetworkStatistics(p.NetworkNode);

                float storageRatio = ps.InStorageCurrent / Math.Max(ps.InStorageMax, 1.0f);
                float outStorageRatio = ps.OutStorageCurrent / Math.Max(ps.OutStorageMax, 1.0f);
                return Loc.GetString("cable-multitool-system-statistics",
                    ("supplyc", ps.SupplyCurrent),
                    ("supplyb", ps.SupplyBatteries),
                    ("supplym", ps.SupplyTheoretical),
                    ("consumption", ps.Consumption),
                    ("storagec", ps.InStorageCurrent),
                    ("storager", storageRatio),
                    ("storagem", ps.InStorageMax),
                    ("storageoc", ps.OutStorageCurrent),
                    ("storageor", outStorageRatio),
                    ("storageom", ps.OutStorageMax)
                );
            }
            return Loc.GetString("cable-multitool-system-internal-error-no-power-node");
        }
    }
}
