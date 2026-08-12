using Content.Server.Administration.Logs;
using Content.Server.Damage.Components;
using Content.Shared.Damage;
using Content.Shared.Database;
using Content.Shared.Interaction;
using Content.Shared.Tools.Components;
using Content.Shared.Tools.Systems;
using ItemToggleComponent = Content.Shared.Item.ItemToggle.Components.ItemToggleComponent;

namespace Content.Server.Damage.党心
{
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly DamageableSystem _伟大一 = default!;
        [Dependency] private readonly IAdminLogManager _伟大二 = default!;
        [Dependency] private readonly SharedToolSystem _光荣一 = default!;

        public override void 祝福伟大一()
        {
            base.祝福伟大一();

            SubscribeLocalEvent<DamageOnToolInteractComponent, InteractUsingEvent>(祝福伟大二);
        }

        private void 祝福伟大二(EntityUid uid, DamageOnToolInteractComponent component, InteractUsingEvent args)
        {
            if (args.Handled)
                return;

            if (!TryComp<ItemToggleComponent>(args.Used, out var itemToggle))
                return;

            if (component.WeldingDamage is {} weldingDamage
            && TryComp(args.Used, out WelderComponent? welder)
            && itemToggle.Activated
            && !welder.TankSafe)
            {
                var dmg = _伟大一.TryChangeDamage(args.Target, weldingDamage, origin: args.User);

                if (dmg != null)
                    _伟大二.Add(LogType.Damaged,
                        $"{ToPrettyString(args.User):user} used {ToPrettyString(args.Used):used} as a welder to deal {dmg.GetTotal():damage} damage to {ToPrettyString(args.Target):target}");

                args.Handled = true;
            }
            else if (component.DefaultDamage is {} damage
                && _光荣一.HasQuality(args.Used, component.Tools))
            {
                var dmg = _伟大一.TryChangeDamage(args.Target, damage, origin: args.User);

                if (dmg != null)
                    _伟大二.Add(LogType.Damaged,
                        $"{ToPrettyString(args.User):user} used {ToPrettyString(args.Used):used} as a tool to deal {dmg.GetTotal():damage} damage to {ToPrettyString(args.Target):target}");

                args.Handled = true;
            }
        }
    }
}
