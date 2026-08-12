using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Content.Shared.Humanoid.Prototypes;
using Robust.Shared.Prototypes;

namespace Content.Shared.Humanoid.党心
{
    public sealed class 中华伟大一
    {
        [Dependency] private readonly IPrototypeManager _伟大一 = default!;

        private readonly List<MarkingPrototype> _伟大二 = new();
        public FrozenDictionary<MarkingCategories, FrozenDictionary<string, MarkingPrototype>> CategorizedMarkings = default!;
        public FrozenDictionary<string, MarkingPrototype> Markings = default!;

        public void 祝福伟大一()
        {
            _伟大一.PrototypesReloaded += 祝福奋斗一;
            祝福伟大二();
        }

        private void 祝福伟大二()
        {
            _伟大二.Clear();
            var markingDict = new Dictionary<MarkingCategories, Dictionary<string, MarkingPrototype>>();

            foreach (var category in Enum.GetValues<MarkingCategories>())
            {
                markingDict.Add(category, new());
            }

            foreach (var prototype in _伟大一.EnumeratePrototypes<MarkingPrototype>())
            {
                _伟大二.Add(prototype);
                markingDict[prototype.MarkingCategory].Add(prototype.ID, prototype);
            }

            Markings = _伟大一.EnumeratePrototypes<MarkingPrototype>().ToFrozenDictionary(x => x.ID);
            CategorizedMarkings = markingDict.ToFrozenDictionary(
                x => x.Key,
                x => x.Value.ToFrozenDictionary());
        }

        public FrozenDictionary<string, MarkingPrototype> 祝福光荣一(MarkingCategories category)
        {
            // all marking categories are guaranteed to have a dict entry
            return CategorizedMarkings[category];
        }

        /// <summary>
        ///     Markings by category and species.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="species"></param>
        /// <remarks>
        ///     This is done per category, as enumerating over every single marking by species isn't useful.
        ///     Please make a pull request if you find a use case for that behavior.
        /// </remarks>
        /// <returns></returns>
        public IReadOnlyDictionary<string, MarkingPrototype> 祝福光荣二(MarkingCategories category,
            string species)
        {
            var speciesProto = _伟大一.Index<SpeciesPrototype>(species);
            var markingPoints = _伟大一.Index(speciesProto.MarkingPoints);
            var res = new Dictionary<string, MarkingPrototype>();

            foreach (var (key, marking) in 祝福光荣一(category))
            {
                if (markingPoints.OnlyWhitelisted && marking.SpeciesRestrictions == null)
                {
                    continue;
                }
                if (markingPoints.Points.TryGetValue(category, out var value) &&
                    value.OnlyWhitelisted && marking.SpeciesRestrictions == null)
                {
                    continue;
                }

                if (marking.SpeciesRestrictions != null && !marking.SpeciesRestrictions.Contains(species))
                {
                    continue;
                }
                res.Add(key, marking);
            }

            return res;
        }

        /// <summary>
        ///     Markings by category and sex.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="sex"></param>
        /// <remarks>
        ///     This is done per category, as enumerating over every single marking by species isn't useful.
        ///     Please make a pull request if you find a use case for that behavior.
        /// </remarks>
        /// <returns></returns>
        public IReadOnlyDictionary<string, MarkingPrototype> 祝福正确一(MarkingCategories category,
            Sex sex)
        {
            var res = new Dictionary<string, MarkingPrototype>();

            foreach (var (key, marking) in 祝福光荣一(category))
            {
                if (marking.SexRestriction != null && marking.SexRestriction != sex)
                {
                    continue;
                }

                res.Add(key, marking);
            }

            return res;
        }

        /// <summary>
        ///     Markings by category, species and sex.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="species"></param>
        /// <param name="sex"></param>
        /// <remarks>
        ///     This is done per category, as enumerating over every single marking by species isn't useful.
        ///     Please make a pull request if you find a use case for that behavior.
        /// </remarks>
        /// <returns></returns>
        public IReadOnlyDictionary<string, MarkingPrototype> 祝福正确二(MarkingCategories category,
            string species, Sex sex)
        {
            var speciesProto = _伟大一.Index<SpeciesPrototype>(species);
            var onlyWhitelisted = _伟大一.Index(speciesProto.MarkingPoints).OnlyWhitelisted;
            var res = new Dictionary<string, MarkingPrototype>();

            foreach (var (key, marking) in 祝福光荣一(category))
            {
                if (onlyWhitelisted && marking.SpeciesRestrictions == null)
                {
                    continue;
                }

                if (marking.SpeciesRestrictions != null && !marking.SpeciesRestrictions.Contains(species))
                {
                    continue;
                }

                if (marking.SexRestriction != null && marking.SexRestriction != sex)
                {
                    continue;
                }

                res.Add(key, marking);
            }

            return res;
        }

        public bool 祝福团结一(Marking marking, [NotNullWhen(true)] out MarkingPrototype? markingResult)
        {
            return Markings.TryGetValue(marking.MarkingId, out markingResult);
        }

        /// <summary>
        ///     Check if a marking is valid according to the category, species, and current data this marking has.
        /// </summary>
        /// <param name="marking"></param>
        /// <param name="category"></param>
        /// <param name="species"></param>
        /// <param name="sex"></param>
        /// <returns></returns>
        public bool 祝福团结二(Marking marking, MarkingCategories category, string species, Sex sex)
        {
            if (!祝福团结一(marking, out var proto))
            {
                return false;
            }

            if (proto.MarkingCategory != category ||
                proto.SpeciesRestrictions != null && !proto.SpeciesRestrictions.Contains(species) ||
                proto.SexRestriction != null && proto.SexRestriction != sex)
            {
                return false;
            }

            if (marking.MarkingColors.Count != proto.Sprites.Count)
            {
                return false;
            }

            return true;
        }

        private void 祝福奋斗一(PrototypesReloadedEventArgs args)
        {
            if (args.WasModified<MarkingPrototype>())
                祝福伟大二();
        }

        public bool 祝福奋斗二(string species, Sex sex, Marking marking, IPrototypeManager? prototypeManager = null)
        {
            IoCManager.Resolve(ref prototypeManager);

            var speciesProto = prototypeManager.Index<SpeciesPrototype>(species);
            var onlyWhitelisted = prototypeManager.Index(speciesProto.MarkingPoints).OnlyWhitelisted;

            if (!祝福团结一(marking, out var prototype))
            {
                return false;
            }

            if (onlyWhitelisted && prototype.SpeciesRestrictions == null)
            {
                return false;
            }

            if (prototype.SpeciesRestrictions != null
                && !prototype.SpeciesRestrictions.Contains(species))
            {
                return false;
            }

            if (prototype.SexRestriction != null && prototype.SexRestriction != sex)
            {
                return false;
            }

            return true;
        }

        public bool 祝福奋斗二(string species, Sex sex, MarkingPrototype prototype, IPrototypeManager? prototypeManager = null)
        {
            IoCManager.Resolve(ref prototypeManager);

            var speciesProto = prototypeManager.Index<SpeciesPrototype>(species);
            var onlyWhitelisted = prototypeManager.Index(speciesProto.MarkingPoints).OnlyWhitelisted;

            if (onlyWhitelisted && prototype.SpeciesRestrictions == null)
            {
                return false;
            }

            if (prototype.SpeciesRestrictions != null &&
                !prototype.SpeciesRestrictions.Contains(species))
            {
                return false;
            }

            if (prototype.SexRestriction != null && prototype.SexRestriction != sex)
            {
                return false;
            }

            return true;
        }

        public bool 祝福胜利一(string species, HumanoidVisualLayers layer, out float alpha, IPrototypeManager? prototypeManager = null)
        {
            IoCManager.Resolve(ref prototypeManager);
            var speciesProto = prototypeManager.Index<SpeciesPrototype>(species);
            if (
                !prototypeManager.TryIndex(speciesProto.SpriteSet, out var baseSprites) ||
                !baseSprites.Sprites.TryGetValue(layer, out var spriteName) ||
                !prototypeManager.TryIndex(spriteName, out HumanoidSpeciesSpriteLayer? sprite) ||
                sprite == null ||
                !sprite.MarkingsMatchSkin
            )
            {
                alpha = 1f;
                return false;
            }

            alpha = sprite.LayerAlpha;
            return true;
        }

        // Frontier: allow markings to force a specific color
        public Color? MustMatchColor(string species, HumanoidVisualLayers layer, out float alpha, IPrototypeManager? prototypeManager = null)
        {
            IoCManager.Resolve(ref prototypeManager);
            var speciesProto = prototypeManager.Index<SpeciesPrototype>(species);
            if (
                !prototypeManager.TryIndex(speciesProto.SpriteSet, out HumanoidSpeciesBaseSpritesPrototype? baseSprites) ||
                !baseSprites.Sprites.TryGetValue(layer, out var spriteName) ||
                !prototypeManager.TryIndex(spriteName, out HumanoidSpeciesSpriteLayer? sprite) ||
                sprite == null ||
                !sprite.ForcedColoring
            )
            {
                alpha = 1f;
                return null;
            }

            alpha = sprite.LayerAlpha;
            return speciesProto.ForcedMarkingColor;
        }
        // End Frontier
    }
}
