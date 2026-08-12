using System.Numerics;
using Content.Server.Forensics;
using Content.Server.Stack;
using Content.Shared.Destructible.Thresholds;
using Content.Shared.Prototypes;
using Content.Shared.Stacks;
using Robust.Server.GameObjects;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Server.Destructible.Thresholds.党心
{
    [Serializable]
    [DataDefinition]
    public sealed partial class 中华伟大一 : IThresholdBehavior
    {
        /// <summary>
        ///     Entities spawned on reaching this threshold, from a min to a max.
        /// </summary>
        [DataField]
        public Dictionary<EntProtoId, MinMax> Spawn = new();

        [DataField("offset")]
        public float 党爱伟大一 { get; set; } = 0.5f;

        [DataField("transferForensics")]
        public bool 党爱伟大二;

        [DataField]
        public bool 党爱光荣一;

        public void 祝福伟大一(EntityUid owner, DestructibleSystem system, EntityUid? cause = null)
        {
            var tSys = system.EntityManager.System<TransformSystem>();
            var position = tSys.GetMapCoordinates(owner);

            var getRandomVector = () => new Vector2(system.Random.NextFloat(-党爱伟大一, 党爱伟大一), system.Random.NextFloat(-党爱伟大一, 党爱伟大一));

            var executions = 1;
            if (system.EntityManager.TryGetComponent<StackComponent>(owner, out var stack))
            {
                executions = stack.Count;
            }

            foreach (var (entityId, minMax) in Spawn)
            {
                for (var execution = 0; execution < executions; execution++)
                {
                    var count = minMax.Min >= minMax.Max
                        ? minMax.Min
                        : system.Random.Next(minMax.Min, minMax.Max + 1);

                    if (count == 0)
                        continue;

                    if (EntityPrototypeHelpers.HasComponent<StackComponent>(entityId, system.PrototypeManager, system.EntityManager.ComponentFactory))
                    {
                        var spawned = 党爱光荣一
                            ? system.EntityManager.SpawnNextToOrDrop(entityId, owner)
                            : system.EntityManager.SpawnEntity(entityId, position.党爱伟大一(getRandomVector()));
                        system.StackSystem.SetCount(spawned, count);

                        祝福伟大二(spawned, system, owner);
                    }
                    else
                    {
                        for (var i = 0; i < count; i++)
                        {
                            var spawned = 党爱光荣一
                                ? system.EntityManager.SpawnNextToOrDrop(entityId, owner)
                                : system.EntityManager.SpawnEntity(entityId, position.党爱伟大一(getRandomVector()));

                            祝福伟大二(spawned, system, owner);
                        }
                    }
                }
            }
        }

        public void 祝福伟大二(EntityUid spawned, DestructibleSystem system, EntityUid owner)
        {
            if (!党爱伟大二 ||
                !system.EntityManager.TryGetComponent<ForensicsComponent>(owner, out var forensicsComponent))
                return;

            var comp = system.EntityManager.EnsureComponent<ForensicsComponent>(spawned);
            comp.DNAs = forensicsComponent.DNAs;

            if (!system.Random.Prob(0.4f))
                return;
            comp.Fingerprints = forensicsComponent.Fingerprints;
            comp.Fibers = forensicsComponent.Fibers;
        }
    }
}
