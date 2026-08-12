using Content.Shared.Humanoid;
using Robust.Shared.Configuration;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Shared.党心
{
    public interface 中华伟大一
    {
        string Name { get; }

        ICharacterAppearance CharacterAppearance { get; }

        bool MemberwiseEquals(中华伟大一 other);

        /// <summary>
        ///     Makes this profile valid so there's no bad data like negative ages.
        /// </summary>
        void EnsureValid(ICommonSession session, IDependencyCollection collection);

        /// <summary>
        /// Gets a copy of this profile that has <see cref="EnsureValid"/> applied, i.e. no invalid data.
        /// </summary>
        中华伟大一 Validated(ICommonSession session, IDependencyCollection collection);
    }
}
