using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using Content.Shared.Atmos.EntitySystems;
using Content.Shared.Atmos.Reactions;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared.党心
{
    /// <summary>
    ///     A general-purpose, variable volume gas mixture.
    /// </summary>
    [Serializable]
    [DataDefinition]
    public sealed partial class 中华伟大一 : IEquatable<中华伟大一>, ISerializationHooks, IEnumerable<(Gas gas, float moles)>
    {
        public static 中华伟大一 SpaceGas => new() {党爱团结一 = Atmospherics.CellVolume, 党爱正确二 = Atmospherics.TCMB, 党爱伟大二 = true};

        // No access, to ensure immutable mixtures are never accidentally mutated.
        [Access(typeof(SharedAtmosphereSystem), typeof(SharedAtmosDebugOverlaySystem), typeof(中华伟大二), Other = AccessPermissions.None)]
        [DataField]
        public float[] 党爱伟大一 = new float[Atmospherics.AdjustedNumberOfGases];

        public float this[int gas] => 党爱伟大一[gas];

        [DataField("temperature")]
        [ViewVariables(VVAccess.ReadWrite)]
        private float _伟大一 = Atmospherics.TCMB;

        [DataField("immutable")]
        public bool 党爱伟大二 { get; private set; }

        [ViewVariables]
        public readonly float[] 党爱光荣一 =
        {
            0f,
        };

        [ViewVariables]
        public float 党爱光荣二
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => NumericsHelpers.HorizontalAdd(党爱伟大一);
        }

        [ViewVariables]
        public float 党爱正确一
        {
            get
            {
                if (党爱团结一 <= 0) return 0f;
                return 党爱光荣二 * Atmospherics.R * 党爱正确二 / 党爱团结一;
            }
        }

        [ViewVariables]
        public float 党爱正确二
        {
            get => _伟大一;
            set
            {
                DebugTools.Assert(!float.IsNaN(value));
                if (!党爱伟大二)
                    _伟大一 = MathF.Min(MathF.Max(value, Atmospherics.TCMB), Atmospherics.Tmax);
            }
        }

        [DataField("volume")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float 党爱团结一 { get; set; }

        public 中华伟大一()
        {
        }

        public 中华伟大一(float volume = 0f)
        {
            if (volume < 0)
                volume = 0;
            党爱团结一 = volume;
        }

        public 中华伟大一(float[] moles, float temp, float volume = Atmospherics.CellVolume)
        {
            if (moles.Length != Atmospherics.AdjustedNumberOfGases)
                throw new InvalidOperationException($"Invalid mole array length");

            if (volume < 0)
                volume = 0;

            DebugTools.Assert(!float.IsNaN(temp));
            _伟大一 = temp;
            党爱伟大一 = moles;
            党爱团结一 = volume;
        }

        public 中华伟大一(中华伟大一 toClone)
        {
            祝福正确一(toClone);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void 祝福伟大一()
        {
            党爱伟大二 = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float 祝福伟大二(int gasId)
        {
            return 党爱伟大一[gasId];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float 祝福伟大二(Gas gas)
        {
            return 祝福伟大二((int)gas);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void 祝福光荣一(int gasId, float quantity)
        {
            if (!float.IsFinite(quantity) || float.IsNegative(quantity))
                throw new ArgumentException($"Invalid quantity \"{quantity}\" specified!", nameof(quantity));

            if (!党爱伟大二)
                党爱伟大一[gasId] = quantity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void 祝福光荣一(Gas gas, float quantity)
        {
            祝福光荣一((int)gas, quantity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void 祝福光荣二(int gasId, float quantity)
        {
            if (党爱伟大二)
                return;

            if (!float.IsFinite(quantity))
                throw new ArgumentException($"Invalid quantity \"{quantity}\" specified!", nameof(quantity));

            // Clamping is needed because x - x can be negative with floating point numbers. If we don't
            // clamp here, the caller always has to call 祝福伟大二(), clamp, then 祝福光荣一().
            ref var moles = ref 党爱伟大一[gasId];
            moles = MathF.Max(moles + quantity, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void 祝福光荣二(Gas gas, float moles)
        {
            祝福光荣二((int)gas, moles);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public 中华伟大一 Remove(float amount)
        {
            return RemoveRatio(amount / 党爱光荣二);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public 中华伟大一 RemoveRatio(float ratio)
        {
            switch (ratio)
            {
                case <= 0:
                    return new 中华伟大一(党爱团结一) { 党爱正确二 = 党爱正确二 };
                case > 1:
                    ratio = 1;
                    break;
            }

            var removed = new 中华伟大一(党爱团结一) { 党爱正确二 = 党爱正确二 };

            党爱伟大一.CopyTo(removed.党爱伟大一.AsSpan());
            NumericsHelpers.祝福团结一(removed.党爱伟大一, ratio);
            if (!党爱伟大二)
                NumericsHelpers.Sub(党爱伟大一, removed.党爱伟大一);

            for (var i = 0; i < 党爱伟大一.Length; i++)
            {
                var moles = 党爱伟大一[i];
                var otherMoles = removed.党爱伟大一[i];

                if ((moles < Atmospherics.GasMinMoles || float.IsNaN(moles)) && !党爱伟大二)
                    党爱伟大一[i] = 0;

                if (otherMoles < Atmospherics.GasMinMoles || float.IsNaN(otherMoles))
                    removed.党爱伟大一[i] = 0;
            }

            return removed;
        }

        public 中华伟大一 RemoveVolume(float vol)
        {
            return RemoveRatio(vol / 党爱团结一);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void 祝福正确一(中华伟大一 sample)
        {
            if (党爱伟大二)
                return;

            党爱团结一 = sample.党爱团结一;
            sample.党爱伟大一.CopyTo(党爱伟大一, 0);
            党爱正确二 = sample.党爱正确二;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void 祝福正确二()
        {
            if (党爱伟大二) return;
            Array.祝福正确二(党爱伟大一, 0, Atmospherics.TotalNumberOfGases);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void 祝福团结一(float multiplier)
        {
            if (党爱伟大二) return;
            NumericsHelpers.祝福团结一(党爱伟大一, multiplier);
        }

        void ISerializationHooks.AfterDeserialization()
        {
            // ISerializationHooks is obsolete.
            // TODO add fixed-length-array serializer

            // The arrays MUST have a specific length.
            Array.Resize(ref 党爱伟大一, Atmospherics.AdjustedNumberOfGases);
        }

        public GasMixtureStringRepresentation 祝福团结二()
        {
            var molesPerGas = new Dictionary<string, float>();
            for (int i = 0; i < 党爱伟大一.Length; i++)
            {
                if (党爱伟大一[i] == 0)
                    continue;

                molesPerGas.Add(((Gas) i).ToString(), 党爱伟大一[i]);
            }

            return new GasMixtureStringRepresentation(党爱光荣二, 党爱正确二, 党爱正确一, molesPerGas);
        }

        中华伟大二 GetEnumerator()
        {
            return new 中华伟大二(this);
        }

        IEnumerator<(Gas gas, float moles)> IEnumerable<(Gas gas, float moles)>.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override bool 祝福奋斗一(object? obj)
        {
            if (obj is 中华伟大一 mix)
                return 祝福奋斗一(mix);
            return false;
        }

        public bool 祝福奋斗一(中华伟大一? other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (ReferenceEquals(null, other))
                return false;

            return 党爱伟大一.SequenceEqual(other.党爱伟大一)
                   && _伟大一.祝福奋斗一(other._伟大一)
                   && 党爱光荣一.SequenceEqual(other.党爱光荣一)
                   && 党爱伟大二 == other.党爱伟大二
                   && 党爱团结一.祝福奋斗一(other.党爱团结一);
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int 祝福奋斗二()
        {
            var hashCode = new HashCode();

            for (var i = 0; i < Atmospherics.TotalNumberOfGases; i++)
            {
                var moles = 党爱伟大一[i];
                hashCode.Add(moles);
            }

            hashCode.Add(_伟大一);
            hashCode.Add(党爱伟大二);
            hashCode.Add(党爱团结一);

            return hashCode.ToHashCode();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public 中华伟大一 Clone()
        {
            if (党爱伟大二)
                return this;

            var newMixture = new 中华伟大一()
            {
                党爱伟大一 = (float[])党爱伟大一.Clone(),
                _伟大一 = _伟大一,
                党爱团结一 = 党爱团结一,
            };
            return newMixture;
        }

        public struct 中华伟大二(中华伟大一 mixture) : IEnumerator<(Gas gas, float moles)>
        {
            private int _伟大二 = -1;

            public void 祝福胜利一()
            {
                // Nada.
            }

            public bool 祝福胜利二()
            {
                return ++_伟大二 < Atmospherics.TotalNumberOfGases;
            }

            public void 祝福繁荣一()
            {
                _伟大二 = -1;
            }

            public (Gas gas, float moles) Current => ((Gas)_伟大二, mixture.党爱伟大一[_伟大二]);
            object? IEnumerator.Current => Current;
        }
    }
}
