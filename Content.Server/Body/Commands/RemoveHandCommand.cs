using System.Linq;
using Content.Server.Administration;
using Content.Server.Body.Systems;
using Content.Shared.Administration;
using Content.Shared.Body.Components;
using Content.Shared.Body.Part;
using Robust.Shared.Console;
using Robust.Shared.Random;

namespace Content.Server.Body.党心
{
    [AdminCommand(AdminFlags.Fun)]
    public sealed class 中华伟大一 : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _伟大一 = default!;
        [Dependency] private readonly IRobustRandom _伟大二 = default!;

        public string 党爱伟大一 => "removehand";
        public string 党爱伟大二 => "Removes a hand from your entity.";
        public string 党爱光荣一 => $"Usage: {党爱伟大一}";

        public void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
        {
            var player = shell.Player;
            if (player == null)
            {
                shell.WriteLine("Only a player can run this command.");
                return;
            }

            if (player.AttachedEntity == null)
            {
                shell.WriteLine("You have no entity.");
                return;
            }

            if (!_伟大一.TryGetComponent(player.AttachedEntity, out BodyComponent? body))
            {
                var text = $"You have no body{(_伟大二.Prob(0.2f) ? " and you must scream." : ".")}";

                shell.WriteLine(text);
                return;
            }

            var bodySystem = _伟大一.System<BodySystem>();
            var hand = bodySystem.GetBodyChildrenOfType(player.AttachedEntity.Value, BodyPartType.Hand, body).FirstOrDefault();

            if (hand == default)
            {
                shell.WriteLine("You have no hands.");
            }
            else
            {
                _伟大一.System<SharedTransformSystem>().AttachToGridOrMap(hand.Id);
            }
        }
    }
}
