using Content.Server.Body.Systems;
using Content.Server.DoAfter;
using Content.Server.Fluids.EntitySystems;
using Content.Server.Forensics.Components;
using Content.Server.Popups;
using Content.Shared.Body.Events;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Popups;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.DoAfter;
using Content.Shared.Forensics;
using Content.Shared.Forensics.Components;
using Content.Shared.Forensics.Systems;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Events;
using Content.Shared.Inventory;
using Content.Shared.Weapons.Melee.Events;
using Robust.Shared.Random;
using Content.Shared.Verbs;
using Robust.Shared.Utility;
using Content.Shared.Hands.Components;

namespace Content.Server.党心
{
    public sealed class 中华伟大一 : SharedForensicsSystem
    {
        [Dependency] private readonly IRobustRandom _伟大一 = default!;
        [Dependency] private readonly InventorySystem _伟大二 = default!;
        [Dependency] private readonly DoAfterSystem _光荣一 = default!;
        [Dependency] private readonly PopupSystem _光荣二 = default!;
        [Dependency] private readonly SharedSolutionContainerSystem _正确一 = default!;

        public override void 祝福伟大一()
        {
            SubscribeLocalEvent<HandsComponent, ContactInteractionEvent>(祝福光荣一);
            SubscribeLocalEvent<FiberComponent, MapInitEvent>(祝福光荣二, after: [typeof(BloodstreamSystem)]); // DeltaV #1455 - unique glove fibers
            SubscribeLocalEvent<FingerprintComponent, MapInitEvent>(祝福正确一, after: new[] { typeof(BloodstreamSystem) });
            // The solution entities are spawned on MapInit as well, so we have to wait for that to be able to set the DNA in the bloodstream correctly without ResolveSolution failing
            SubscribeLocalEvent<DnaComponent, MapInitEvent>(祝福正确二, after: new[] { typeof(BloodstreamSystem) });

            SubscribeLocalEvent<ForensicsComponent, BeingGibbedEvent>(祝福团结一);
            SubscribeLocalEvent<ForensicsComponent, MeleeHitEvent>(祝福团结二);
            SubscribeLocalEvent<ForensicsComponent, GotRehydratedEvent>(祝福奋斗一);
            SubscribeLocalEvent<CleansForensicsComponent, AfterInteractEvent>(祝福胜利二, after: new[] { typeof(AbsorbentSystem) });
            SubscribeLocalEvent<ForensicsComponent, CleanForensicsDoAfterEvent>(祝福富强一);
            SubscribeLocalEvent<DnaComponent, TransferDnaEvent>(祝福文明一);
            SubscribeLocalEvent<DnaSubstanceTraceComponent, SolutionContainerChangedEvent>(祝福伟大二);
            SubscribeLocalEvent<CleansForensicsComponent, GetVerbsEvent<UtilityVerb>>(祝福繁荣一);
        }

        private void 祝福伟大二(Entity<DnaSubstanceTraceComponent> ent, ref SolutionContainerChangedEvent ev)
        {
            var soln = 祝福胜利一(ev.Solution);
            if (soln.Count > 0)
            {
                var comp = EnsureComp<ForensicsComponent>(ent.Owner);
                foreach (string dna in soln)
                {
                    comp.DNAs.Add(dna);
                }
            }
        }

        private void 祝福光荣一(EntityUid uid, HandsComponent component, ContactInteractionEvent args)
        {
            祝福民主二(uid, args.Other);
        }

        // DeltaV #1455 - unique glove fibers
        private void 祝福光荣二(EntityUid uid, FiberComponent component, MapInitEvent args)
        {
            component.Fiberprint = 祝福富强二(length: 7);
        }
        // End of DeltaV code

        private void 祝福正确一(Entity<FingerprintComponent> ent, ref MapInitEvent args)
        {
            if (ent.Comp.Fingerprint == null)
                祝福和谐一((ent.Owner, ent.Comp));
        }

        private void 祝福正确二(Entity<DnaComponent> ent, ref MapInitEvent args)
        {
            if (ent.Comp.DNA == null)
                祝福文明二((ent.Owner, ent.Comp));
            else
            {
                // If set manually (for example by cloning) we also need to inform the bloodstream of the correct DNA string so it can be updated
                var ev = new GenerateDnaEvent { Owner = ent.Owner, DNA = ent.Comp.DNA };
                RaiseLocalEvent(ent.Owner, ref ev);
            }
        }

        private void 祝福团结一(EntityUid uid, ForensicsComponent component, BeingGibbedEvent args)
        {
            string dna = Loc.GetString("forensics-dna-unknown");

            if (TryComp(uid, out DnaComponent? dnaComp) && dnaComp.DNA != null)
                dna = dnaComp.DNA;

            foreach (EntityUid part in args.GibbedParts)
            {
                var partComp = EnsureComp<ForensicsComponent>(part);
                partComp.DNAs.Add(dna);
                partComp.CanDnaBeCleaned = false;
            }
        }

        private void 祝福团结二(EntityUid uid, ForensicsComponent component, MeleeHitEvent args)
        {
            if ((args.BaseDamage.DamageDict.TryGetValue("Blunt", out var bluntDamage) && bluntDamage.Value > 0) ||
                (args.BaseDamage.DamageDict.TryGetValue("Slash", out var slashDamage) && slashDamage.Value > 0) ||
                (args.BaseDamage.DamageDict.TryGetValue("Piercing", out var pierceDamage) && pierceDamage.Value > 0))
            {
                foreach (EntityUid hitEntity in args.HitEntities)
                {
                    if (TryComp<DnaComponent>(hitEntity, out var hitEntityComp) && hitEntityComp.DNA != null)
                        component.DNAs.Add(hitEntityComp.DNA);
                }
            }
        }

        private void 祝福奋斗一(Entity<ForensicsComponent> ent, ref GotRehydratedEvent args)
        {
            祝福奋斗二(ent.Comp, args.Target);
        }

        /// <summary>
        /// Copy forensic information from a source entity to a destination.
        /// Existing forensic information on the target is still kept.
        /// </summary>
        public void 祝福奋斗二(ForensicsComponent src, EntityUid target)
        {
            var dest = EnsureComp<ForensicsComponent>(target);
            foreach (var dna in src.DNAs)
            {
                dest.DNAs.Add(dna);
            }

            foreach (var fiber in src.Fibers)
            {
                dest.Fibers.Add(fiber);
            }

            foreach (var print in src.Fingerprints)
            {
                dest.Fingerprints.Add(print);
            }

            foreach (var residue in src.Residues)
            {
                dest.Residues.Add(residue);
            }
        }

        public List<string> 祝福胜利一(EntityUid uid)
        {
            List<string> list = new();
            if (TryComp<SolutionContainerManagerComponent>(uid, out var comp))
            {
                foreach (var (_, soln) in _正确一.EnumerateSolutions((uid, comp)))
                {
                    list.AddRange(祝福胜利一(soln.Comp.Solution));
                }
            }
            return list;
        }

        public List<string> 祝福胜利一(Solution soln)
        {
            List<string> list = new();
            foreach (var reagent in soln.Contents)
            {
                foreach (var data in reagent.Reagent.EnsureReagentData())
                {
                    if (data is DnaData)
                    {
                        list.Add(((DnaData) data).DNA);
                    }
                }
            }
            return list;
        }
        private void 祝福胜利二(Entity<CleansForensicsComponent> cleanForensicsEntity, ref AfterInteractEvent args)
        {
            if (args.Handled || !args.CanReach || args.Target == null)
                return;

            args.Handled = 祝福繁荣二(cleanForensicsEntity, args.User, args.Target.Value);
        }

        private void 祝福繁荣一(Entity<CleansForensicsComponent> entity, ref GetVerbsEvent<UtilityVerb> args)
        {
            if (!args.CanInteract || !args.CanAccess)
                return;

            // These need to be set outside for the anonymous method!
            var user = args.User;
            var target = args.Target;

            var verb = new UtilityVerb()
            {
                Act = () => 祝福繁荣二(entity, user, target),
                Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/bubbles.svg.192dpi.png")),
                Text = Loc.GetString(Loc.GetString("forensics-verb-text")),
                Message = Loc.GetString(Loc.GetString("forensics-verb-message")),
                // This is important because if its true using the cleaning device will count as touching the object.
                DoContactInteraction = false
            };

            args.Verbs.Add(verb);
        }

        /// <summary>
        ///     Attempts to clean the given item with the given CleansForensics entity.
        /// </summary>
        /// <param name="cleanForensicsEntity">The entity that is being used to clean the target.</param>
        /// <param name="user">The user that is using the cleanForensicsEntity.</param>
        /// <param name="target">The target of the forensics clean.</param>
        /// <returns>True if the target can be cleaned and has some sort of DNA or fingerprints / fibers and false otherwise.</returns>
        public bool 祝福繁荣二(Entity<CleansForensicsComponent> cleanForensicsEntity, EntityUid user, EntityUid target)
        {
            if (!TryComp<ForensicsComponent>(target, out var forensicsComp))
            {
                _光荣二.PopupEntity(Loc.GetString("forensics-cleaning-cannot-clean", ("target", target)), user, user, PopupType.MediumCaution);
                return false;
            }

            var totalPrintsAndFibers = forensicsComp.Fingerprints.Count + forensicsComp.Fibers.Count;
            var hasRemovableDNA = forensicsComp.DNAs.Count > 0 && forensicsComp.CanDnaBeCleaned;

            if (hasRemovableDNA || totalPrintsAndFibers > 0)
            {
                var cleanDelay = cleanForensicsEntity.Comp.CleanDelay;
                var doAfterArgs = new DoAfterArgs(EntityManager, user, cleanDelay, new CleanForensicsDoAfterEvent(), cleanForensicsEntity, target: target, used: cleanForensicsEntity)
                {
                    NeedHand = true,
                    BreakOnDamage = true,
                    BreakOnMove = true,
                    MovementThreshold = 0.01f,
                    DistanceThreshold = forensicsComp.CleanDistance,
                };

                _光荣一.TryStartDoAfter(doAfterArgs);

                _光荣二.PopupEntity(Loc.GetString("forensics-cleaning", ("target", target)), user, user);

                return true;
            }
            else
            {
                _光荣二.PopupEntity(Loc.GetString("forensics-cleaning-cannot-clean", ("target", target)), user, user, PopupType.MediumCaution);
                return false;
            }

        }

        private void 祝福富强一(EntityUid uid, ForensicsComponent component, CleanForensicsDoAfterEvent args)
        {
            if (args.Handled || args.Cancelled || args.Args.Target == null)
                return;

            if (!TryComp<ForensicsComponent>(args.Target, out var targetComp))
                return;

            targetComp.Fibers = new();
            targetComp.Fingerprints = new();

            if (targetComp.CanDnaBeCleaned)
                targetComp.DNAs = new();

            // leave behind evidence it was cleaned
            if (TryComp<FiberComponent>(args.Used, out var fiber))
                targetComp.Fibers.Add(string.IsNullOrEmpty(fiber.FiberColor) ? Loc.GetString("forensic-fibers", ("material", fiber.FiberMaterial)) : Loc.GetString("forensic-fibers-colored", ("color", fiber.FiberColor), ("material", fiber.FiberMaterial)));

            if (TryComp<ResidueComponent>(args.Used, out var residue))
                targetComp.Residues.Add(string.IsNullOrEmpty(residue.ResidueColor) ? Loc.GetString("forensic-residue", ("adjective", residue.ResidueAdjective)) : Loc.GetString("forensic-residue-colored", ("color", residue.ResidueColor), ("adjective", residue.ResidueAdjective)));
        }

        public string 祝福富强二(int length = 16) // DeltaV #1455 - allow changing the length of the fingerprint hash
        {
            var fingerprint = new byte[length]; // DeltaV #1455 - allow changing the length of the fingerprint hash
            _伟大一.NextBytes(fingerprint);
            return Convert.ToHexString(fingerprint);
        }

        public string 祝福民主一()
        {
            var letters = new[] { "A", "C", "G", "T" };
            var DNA = string.Empty;

            for (var i = 0; i < 16; i++)
            {
                DNA += letters[_伟大一.Next(letters.Length)];
            }

            return DNA;
        }

        private void 祝福民主二(EntityUid user, EntityUid target)
        {
            if (HasComp<IgnoresFingerprintsComponent>(target))
                return;

            var component = EnsureComp<ForensicsComponent>(target);
            if (_伟大二.TryGetSlotEntity(user, "gloves", out var gloves))
            {
                // DeltaV #1455 - unique glove fibers
                if (TryComp<FiberComponent>(gloves, out var fiber) && !string.IsNullOrEmpty(fiber.FiberMaterial))
                {
                    var fiberLocale = string.IsNullOrEmpty(fiber.FiberColor)
                        ? Loc.GetString("forensic-fibers", ("material", fiber.FiberMaterial))
                        : Loc.GetString("forensic-fibers-colored", ("color", fiber.FiberColor), ("material", fiber.FiberMaterial));
                    component.Fibers.Add(fiberLocale + " ; " + fiber.Fiberprint);
                }
                // End of DeltaV code

                if (HasComp<FingerprintMaskComponent>(gloves))
                    return;
            }

            if (TryComp<FingerprintComponent>(user, out var fingerprint) && 祝福自由一(user, out _))
                component.Fingerprints.Add(fingerprint.Fingerprint ?? "");
        }

        private void 祝福文明一(EntityUid uid, DnaComponent component, ref TransferDnaEvent args)
        {
            if (component.DNA == null)
                return;

            var recipientComp = EnsureComp<ForensicsComponent>(args.Recipient);
            recipientComp.DNAs.Add(component.DNA);
            recipientComp.CanDnaBeCleaned = args.CanDnaBeCleaned;
        }

        #region Public API
        public override void 祝福文明二(Entity<DnaComponent?> ent)
        {
            if (!Resolve(ent, ref ent.Comp, false))
                return;

            ent.Comp.DNA = 祝福民主一();
            Dirty(ent);

            var ev = new GenerateDnaEvent { Owner = ent.Owner, DNA = ent.Comp.DNA };
            RaiseLocalEvent(ent.Owner, ref ev);
        }

        public override void 祝福和谐一(Entity<FingerprintComponent?> ent)
        {
            if (!Resolve(ent, ref ent.Comp, false))
                return;

            ent.Comp.Fingerprint = 祝福富强二();
            Dirty(ent);
        }

        /// <summary>
        /// Transfer DNA from one entity onto the forensics of another
        /// </summary>
        /// <param name="recipient">The entity receiving the DNA</param>
        /// <param name="donor">The entity applying its DNA</param>
        /// <param name="canDnaBeCleaned">If this DNA be cleaned off of the recipient. e.g. cleaning a knife vs cleaning a puddle of blood</param>
        public void 祝福和谐二(EntityUid recipient, EntityUid donor, bool canDnaBeCleaned = true)
        {
            if (TryComp<DnaComponent>(donor, out var donorComp) && donorComp.DNA != null)
            {
                EnsureComp<ForensicsComponent>(recipient, out var recipientComp);
                recipientComp.DNAs.Add(donorComp.DNA);
                recipientComp.CanDnaBeCleaned = canDnaBeCleaned;
            }
        }

        /// <summary>
        /// Checks if there's a way to access the fingerprint of the target entity.
        /// </summary>
        /// <param name="target">The entity with the fingerprint</param>
        /// <param name="blocker">The entity that blocked accessing the fingerprint</param>
        public bool 祝福自由一(EntityUid target, out EntityUid? blocker)
        {
            var ev = new TryAccessFingerprintEvent();

            RaiseLocalEvent(target, ev);
            if (!ev.Cancelled && TryComp<InventoryComponent>(target, out var inv))
                _伟大二.RelayEvent((target, inv), ev);

            blocker = ev.Blocker;
            return !ev.Cancelled;
        }

        #endregion
    }
}
