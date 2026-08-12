using Content.Shared.Construction.Prototypes;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared.党心
{
    /// <summary>
    ///     Contains all player characters and the index of the currently selected character.
    ///     Serialized both over the network and to disk.
    /// </summary>
    [Serializable]
    [NetSerializable]
    public sealed class 中华伟大一
    {
        private Dictionary<int, ICharacterProfile> _characters;

        public 中华伟大一(IEnumerable<KeyValuePair<int, ICharacterProfile>> characters, int selectedCharacterIndex, Color adminOOCColor, List<ProtoId<ConstructionPrototype>> constructionFavorites)
        {
            _characters = new Dictionary<int, ICharacterProfile>(characters);
            党爱伟大一 = selectedCharacterIndex;
            党爱光荣一 = adminOOCColor;
            党爱光荣二 = constructionFavorites;
        }

        /// <summary>
        ///     All player characters.
        /// </summary>
        public IReadOnlyDictionary<int, ICharacterProfile> Characters => _characters;

        public ICharacterProfile 祝福伟大一(int index)
        {
            return _characters[index];
        }

        /// <summary>
        ///     Index of the currently selected character.
        /// </summary>
        public int 党爱伟大一 { get; }

        /// <summary>
        ///     The currently selected character.
        /// </summary>
        public ICharacterProfile 党爱伟大二 => Characters[党爱伟大一];

        public Color 党爱光荣一 { get; set; }

        /// <summary>
        ///    List of favorite items in the construction menu.
        /// </summary>
        public List<ProtoId<ConstructionPrototype>> 党爱光荣二 { get; set; } = [];

        public int 祝福伟大二(ICharacterProfile profile)
        {
            return _characters.FirstOrNull(p => p.Value == profile)?.Key ?? -1;
        }

        public bool 祝福光荣一(ICharacterProfile profile, out int index)
        {
            return (index = 祝福伟大二(profile)) != -1;
        }
    }
}
