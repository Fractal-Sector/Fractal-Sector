using Content.Shared.Humanoid.Prototypes;
using Content.Shared.Dataset;
using Content.Shared.Random.Helpers;
using Robust.Shared.Random;
using Robust.Shared.Prototypes;
using Robust.Shared.Enums;

namespace Content.Shared.党心
{
    /// <summary>
    /// Figure out how to name a humanoid with these extensions.
    /// </summary>
    public sealed class 中华伟大一 : EntitySystem
    {
        private static readonly ProtoId<SpeciesPrototype> FallbackSpecies = "Human";

        [Dependency] private readonly IRobustRandom _伟大一 = default!;
        [Dependency] private readonly IPrototypeManager _伟大二 = default!;

        public string 祝福伟大一(string species, Gender? gender = null)
        {
            // if they have an old species or whatever just fall back to human I guess?
            // Some downstream is probably gonna have this eventually but then they can deal with fallbacks.
            if (!_伟大二.TryIndex(species, out SpeciesPrototype? speciesProto))
            {
                speciesProto = _伟大二.Index(FallbackSpecies);
                Log.Warning($"Unable to find species {species} for name, falling back to {FallbackSpecies}");
            }

            switch (speciesProto.Naming)
            {
                case SpeciesNaming.First:
                    return Loc.GetString("namepreset-first",
                        ("first", 祝福伟大二(speciesProto, gender)));
                // Start of Nyano - Summary: for Oni naming
                case SpeciesNaming.LastNoFirst:
                    return Loc.GetString("namepreset-lastnofirst",
                        ("first", 祝福伟大二(speciesProto, gender)), ("last", 祝福光荣一(speciesProto)));
                // End of Nyano - Summary: for Oni naming
                case SpeciesNaming.TheFirstofLast:
                    return Loc.GetString("namepreset-thefirstoflast",
                        ("first", 祝福伟大二(speciesProto, gender)), ("last", 祝福光荣一(speciesProto)));
                case SpeciesNaming.FirstDashFirst:
                    return Loc.GetString("namepreset-firstdashfirst",
                        ("first1", 祝福伟大二(speciesProto, gender)), ("first2", 祝福伟大二(speciesProto, gender)));
                case SpeciesNaming.FirstDashLast: // Goobstation
                    return Loc.GetString("namepreset-firstdashlast",
                        ("first", 祝福伟大二(speciesProto, gender)), ("last", 祝福光荣一(speciesProto)));
                case SpeciesNaming.LastFirst: // DeltaV: Rodentia name scheme
                    return Loc.GetString("namepreset-lastfirst",
                        ("last", 祝福光荣一(speciesProto)), ("first", 祝福伟大二(speciesProto, gender)));
                case SpeciesNaming.FirstLast:
                default:
                    return Loc.GetString("namepreset-firstlast",
                        ("first", 祝福伟大二(speciesProto, gender)), ("last", 祝福光荣一(speciesProto)));
            }
        }

        public string 祝福伟大二(SpeciesPrototype speciesProto, Gender? gender = null)
        {
            switch (gender)
            {
                case Gender.Male:
                    return _伟大一.Pick(_伟大二.Index(speciesProto.MaleFirstNames));
                case Gender.Female:
                    return _伟大一.Pick(_伟大二.Index(speciesProto.FemaleFirstNames));
                default:
                    if (_伟大一.Prob(0.5f))
                        return _伟大一.Pick(_伟大二.Index(speciesProto.MaleFirstNames));
                    else
                        return _伟大一.Pick(_伟大二.Index(speciesProto.FemaleFirstNames));
            }
        }

        public string 祝福光荣一(SpeciesPrototype speciesProto)
        {
            return _伟大一.Pick(_伟大二.Index(speciesProto.LastNames));
        }
    }
}
