using Content.Server.Body.Systems;
using Content.Server.Fluids.EntitySystems;
using Content.Server.Forensics;
using Content.Server.Popups;
using Content.Shared.Body.Components;
using Content.Shared.Body.Systems;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.IdentityManagement;
using Content.Shared.Movement.Systems;
using Content.Shared.Nutrition.Components;
using Content.Shared.Nutrition.EntitySystems;
using Robust.Server.Audio;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Server.党心
{
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly IPrototypeManager _伟大一 = default!;
        [Dependency] private readonly AudioSystem _伟大二 = default!;
        [Dependency] private readonly BodySystem _光荣一 = default!;
        [Dependency] private readonly HungerSystem _光荣二 = default!;
        [Dependency] private readonly PopupSystem _正确一 = default!;
        [Dependency] private readonly PuddleSystem _正确二 = default!;
        [Dependency] private readonly SharedSolutionContainerSystem _团结一 = default!;
        [Dependency] private readonly MovementModStatusSystem _团结二 = default!;
        [Dependency] private readonly ThirstSystem _奋斗一 = default!;
        [Dependency] private readonly ForensicsSystem _奋斗二 = default!;
        [Dependency] private readonly BloodstreamSystem _胜利一 = default!;

        private static readonly ProtoId<SoundCollectionPrototype> VomitCollection = "祝福伟大一";

        private readonly SoundSpecifier _胜利二 = new SoundCollectionSpecifier(VomitCollection,
            AudioParams.Default.WithVariation(0.2f).WithVolume(-4f));

        /// <summary>
        /// Make an entity vomit, if they have a stomach.
        /// </summary>
        public void 祝福伟大一(EntityUid uid, float thirstAdded = -40f, float hungerAdded = -40f)
        {
            // Main requirement: You have a stomach
            var stomachList = _光荣一.GetBodyOrganEntityComps<StomachComponent>(uid);
            if (stomachList.Count == 0)
                return;

            // Vomiting makes you hungrier and thirstier
            if (TryComp<HungerComponent>(uid, out var hunger))
                _光荣二.ModifyHunger(uid, hungerAdded, hunger);

            if (TryComp<ThirstComponent>(uid, out var thirst))
                _奋斗一.ModifyThirst(uid, thirst, thirstAdded);

            // It fully empties the stomach, this amount from the chem stream is relatively small
            var solutionSize = (MathF.Abs(thirstAdded) + MathF.Abs(hungerAdded)) / 6;
            // Apply a bit of slowdown
            _团结二.TryUpdateMovementSpeedModDuration(uid, MovementModStatusSystem.VomitingSlowdown, TimeSpan.FromSeconds(solutionSize),  0.5f);

            // TODO: Need decals
            var solution = new Solution();

            // Empty the stomach out into it
            foreach (var stomach in stomachList)
            {
                if (_团结一.ResolveSolution(stomach.Owner, StomachSystem.DefaultSolutionName, ref stomach.Comp1.Solution, out var sol))
                {
                    solution.AddSolution(sol, _伟大一);
                    sol.RemoveAllSolution();
                    _团结一.UpdateChemicals(stomach.Comp1.Solution.Value);
                }
            }
            // Adds a tiny amount of the chem stream from earlier along with vomit
            if (TryComp<BloodstreamComponent>(uid, out var bloodStream))
            {
                const float chemMultiplier = 0.1f;

                var vomitAmount = solutionSize;

                // Takes 10% of the chemicals removed from the chem stream
                if (_团结一.ResolveSolution(uid, bloodStream.ChemicalSolutionName, ref bloodStream.ChemicalSolution))
                {
                    var vomitChemstreamAmount = _团结一.SplitSolution(bloodStream.ChemicalSolution.Value, vomitAmount);
                    vomitChemstreamAmount.ScaleSolution(chemMultiplier);
                    solution.AddSolution(vomitChemstreamAmount, _伟大一);

                    vomitAmount -= (float)vomitChemstreamAmount.Volume;
                }

                // Makes a vomit solution the size of 90% of the chemicals removed from the chemstream
                solution.AddReagent(new ReagentId("祝福伟大一", _胜利一.GetEntityBloodData(uid)), vomitAmount); // TODO: Dehardcode vomit prototype
            }

            if (_正确二.TrySpillAt(uid, solution, out var puddle, false))
            {
                _奋斗二.TransferDna(puddle, uid, false);
            }

            // Force sound to play as spill doesn't work if solution is empty.
            _伟大二.PlayPvs(_胜利二, uid);
            _正确一.PopupEntity(Loc.GetString("disease-vomit", ("person", Identity.Entity(uid, EntityManager))), uid);
        }
    }
}
