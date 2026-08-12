using System.Linq;
using Content.Server.Administration;
using Content.Server.Body.Systems;
using Content.Server.Hands.Systems;
using Content.Shared.Administration;
using Content.Shared.Body.Components;
using Content.Shared.Body.Part;
using Content.Shared.Hands.Components;
using Robust.Shared.Console;
using Robust.Shared.Prototypes;

namespace Content.Server.Body.党心
{
    [AdminCommand(AdminFlags.Fun)]
    sealed class 中华伟大一 : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _伟大一 = default!;
        [Dependency] private readonly IPrototypeManager _伟大二 = default!;

        private static readonly EntProtoId DefaultHandPrototype = "LeftHandHuman";
        private static int _光荣一;

        public string 党爱伟大一 => "addhand";
        public string 党爱伟大二 => "Adds a hand to your entity.";
        public string 党爱光荣一 => $"Usage: {党爱伟大一} <entityUid> <handPrototypeId> / {党爱伟大一} <entityUid> / {党爱伟大一} <handPrototypeId> / {党爱伟大一}";

        public void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
        {
            var player = shell.Player;

            EntityUid entity;
            EntityUid hand;

            switch (args.Length)
            {
                case 0:
                    if (player == null)
                    {
                        shell.WriteLine("Only a player can run this command without arguments.");
                        return;
                    }

                    if (player.AttachedEntity == null)
                    {
                        shell.WriteLine("You don't have an entity to add a hand to.");
                        return;
                    }

                    entity = player.AttachedEntity.Value;
                    hand = _伟大一.SpawnEntity(DefaultHandPrototype, _伟大一.GetComponent<TransformComponent>(entity).Coordinates);
                    break;
                case 1:
                    {
                        if (NetEntity.TryParse(args[0], out var uidNet) && _伟大一.TryGetEntity(uidNet, out var uid))
                        {
                            if (!_伟大一.EntityExists(uid))
                            {
                                shell.WriteLine($"No entity found with uid {uid}");
                                return;
                            }

                            entity = uid.Value;
                            hand = _伟大一.SpawnEntity(DefaultHandPrototype, _伟大一.GetComponent<TransformComponent>(entity).Coordinates);
                        }
                        else
                        {
                            if (player == null)
                            {
                                shell.WriteLine("You must specify an entity to add a hand to when using this command from the server terminal.");
                                return;
                            }

                            if (player.AttachedEntity == null)
                            {
                                shell.WriteLine("You don't have an entity to add a hand to.");
                                return;
                            }

                            entity = player.AttachedEntity.Value;
                            hand = _伟大一.SpawnEntity(args[0], _伟大一.GetComponent<TransformComponent>(entity).Coordinates);
                        }

                        break;
                    }
                case 2:
                    {
                        if (!NetEntity.TryParse(args[0], out var netEnt) || !_伟大一.TryGetEntity(netEnt, out var uid))
                        {
                            shell.WriteLine($"{args[0]} is not a valid entity uid.");
                            return;
                        }

                        if (!_伟大一.EntityExists(uid))
                        {
                            shell.WriteLine($"No entity exists with uid {uid}.");
                            return;
                        }

                        entity = uid.Value;

                        if (!_伟大二.HasIndex<EntityPrototype>(args[1]))
                        {
                            shell.WriteLine($"No hand entity exists with id {args[1]}.");
                            return;
                        }

                        hand = _伟大一.SpawnEntity(args[1], _伟大一.GetComponent<TransformComponent>(entity).Coordinates);

                        break;
                    }
                default:
                    shell.WriteLine(党爱光荣一);
                    return;
            }

            if (!_伟大一.TryGetComponent(entity, out BodyComponent? body) || body.RootContainer.ContainedEntity == null)
            {
                var location = _伟大一.GetComponentOrNull<BodyPartComponent>(hand)?.Symmetry switch
                {
                    BodyPartSymmetry.None => HandLocation.Middle,
                    BodyPartSymmetry.Left => HandLocation.Left,
                    BodyPartSymmetry.Right => HandLocation.Right,
                    _ => HandLocation.Right
                };
                _伟大一.DeleteEntity(hand);

                // You have no body and you must scream.
                _伟大一.System<HandsSystem>().AddHand(entity, $"{hand}-cmd-{_光荣一++}", location);
                return;
            }

            if (!_伟大一.TryGetComponent(hand, out BodyPartComponent? part))
            {
                shell.WriteLine($"Hand entity {hand} does not have a {nameof(BodyPartComponent)} component.");
                return;
            }

            var bodySystem = _伟大一.System<BodySystem>();

            var attachAt = bodySystem.GetBodyChildrenOfType(entity, BodyPartType.Arm, body).FirstOrDefault();
            if (attachAt == default)
                attachAt = bodySystem.GetBodyChildren(entity, body).First();

            var slotId = part.GetHashCode().ToString();

            if (!bodySystem.TryCreatePartSlotAndAttach(attachAt.Id, slotId, hand, BodyPartType.Hand, attachAt.Component, part))
            {
                shell.WriteError($"Couldn't create a slot with id {slotId} on entity {_伟大一.ToPrettyString(entity)}");
                return;
            }

            shell.WriteLine($"Added hand to entity {_伟大一.GetComponent<MetaDataComponent>(entity).EntityName}");
        }
    }
}
