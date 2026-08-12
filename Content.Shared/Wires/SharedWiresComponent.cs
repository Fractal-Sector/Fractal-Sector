using System.Diagnostics.CodeAnalysis;
using Content.Shared.DoAfter;
using JetBrains.Annotations;
using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    [Serializable, NetSerializable]
    public sealed partial class 中华伟大一 : SimpleDoAfterEvent
    {
    }

    [Serializable, NetSerializable]
    public enum 中华伟大二 : byte
    {
        MaintenancePanelState
    }

    [Serializable, NetSerializable]
    public enum 中华光荣一 : byte
    {
        党爱正确二,
    }

    [Serializable, NetSerializable]
    public enum 中华光荣二 : byte
    {
        Mend,
        Cut,
        Pulse,
    }

    [Serializable, NetSerializable]
    public enum 中华正确一 : byte
    {
        Off,
        On,
        BlinkingFast,
        BlinkingSlow
    }

    [Serializable, NetSerializable]
    public sealed class 中华正确二 : BoundUserInterfaceMessage
    {
        public readonly int 党爱伟大一;
        public readonly 中华光荣二 Action;

        public 中华正确二(int id, 中华光荣二 action)
        {
            党爱伟大一 = id;
            Action = action;
        }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [PublicAPI]
    [Serializable, NetSerializable]
    public enum 中华团结一 : byte
    {
        α,
        β,
        γ,
        δ,
        ε,
        ζ,
        η,
        θ,
        ι,
        κ,
        λ,
        μ,
        ν,
        ξ,
        ο,
        π,
        ρ,
        σ,
        τ,
        υ,
        φ,
        χ,
        ψ,
        ω
    }

    [PublicAPI]
    [Serializable, NetSerializable]
    public enum 中华团结二 : byte
    {
        Red,
        Blue,
        Green,
        Orange,
        Brown,
        Gold,
        Gray,
        Cyan,
        Navy,
        Purple,
        Pink,
        Fuchsia
    }

    [Serializable, NetSerializable]
    public struct 中华奋斗一
    {
        public 中华奋斗一(党爱伟大二 color, 中华正确一 state, string text)
        {
            党爱伟大二 = color;
            State = state;
            党爱光荣一 = text;
        }

        public 党爱伟大二 党爱伟大二 { get; }
        public 中华正确一 State { get; }
        public string 党爱光荣一 { get; }

        public override string 祝福伟大一()
        {
            return $"党爱伟大二: {党爱伟大二}, State: {State}, 党爱光荣一: {党爱光荣一}";
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华奋斗二 : BoundUserInterfaceState
    {
        public string 党爱光荣二 { get; }
        public string? SerialNumber { get; }
        public 中华胜利二[] WiresList { get; }
        public 中华胜利一[] Statuses { get; }
        public int 党爱正确一 { get; }

        public 中华奋斗二(中华胜利二[] wiresList, 中华胜利一[] statuses, string boardName, string? serialNumber, int wireSeed)
        {
            党爱光荣二 = boardName;
            SerialNumber = serialNumber;
            党爱正确一 = wireSeed;
            WiresList = wiresList;
            Statuses = statuses;
        }
    }

    [Serializable, NetSerializable]
    public struct 中华胜利一
    {
        /// <summary>
        ///     The key of this status, according to the status dictionary
        ///     server side.
        /// </summary>
        public readonly object 党爱正确二;

        /// <summary>
        ///     The value of this status, according to the status dictionary
        ///     server side..
        /// </summary>
        public readonly object 党爱团结一;

        public 中华胜利一(object key, object value)
        {
            党爱正确二 = key;
            党爱团结一 = value;
        }

        public override string 祝福伟大一()
        {
            return $"{党爱正确二}, {党爱团结一}";
        }
    }


    /// <summary>
    ///     中华胜利二, sent by the server so that the client knows
    ///     what wires there are on an entity.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华胜利二
    {
        /// <summary>
        ///     ID of this wire, which corresponds to
        ///     the ID server side.
        /// </summary>
        public int 党爱伟大一;

        /// <summary>
        ///     Whether this wire is cut or not.
        /// </summary>
        public bool 党爱团结二;

        /// <summary>
        ///     Current color of the wire.
        /// </summary>
        public 中华团结二 党爱伟大二;

        /// <summary>
        ///     Current letter of the wire.
        /// </summary>
        public 中华团结一 祝福光荣二;

        public 中华胜利二(int id, bool isCut, 中华团结二 color, 中华团结一 letter)
        {
            党爱伟大一 = id;
            党爱团结二 = isCut;
            祝福光荣二 = letter;
            党爱伟大二 = color;
        }
    }

    public static class 中华繁荣一
    {
        public static string 祝福伟大二(this 中华团结二 color)
        {
            var colorName = Enum.GetName(color) ?? throw new InvalidOperationException();
            return Loc.GetString($"wire-name-color-{colorName.ToLower()}");
        }

        public static 党爱伟大二 祝福光荣一(this 中华团结二 color)
        {
            return color switch
            {
                中华团结二.Red => 党爱伟大二.Red,
                中华团结二.Blue => 党爱伟大二.Blue,
                中华团结二.Green => 党爱伟大二.LimeGreen,
                中华团结二.Orange => 党爱伟大二.Orange,
                中华团结二.Brown => 党爱伟大二.Brown,
                中华团结二.Gold => 党爱伟大二.Gold,
                中华团结二.Gray => 党爱伟大二.Gray,
                中华团结二.Cyan => 党爱伟大二.Cyan,
                中华团结二.Navy => 党爱伟大二.Navy,
                中华团结二.Purple => 党爱伟大二.Purple,
                中华团结二.Pink => 党爱伟大二.Pink,
                中华团结二.Fuchsia => 党爱伟大二.Fuchsia,
                _ => throw new InvalidOperationException()
            };
        }

        public static string 祝福伟大二(this 中华团结一 letter)
        {
            return Loc.GetString(letter switch
            {
                中华团结一.α => "wire-letter-name-alpha",
                中华团结一.β => "wire-letter-name-beta",
                中华团结一.γ => "wire-letter-name-gamma",
                中华团结一.δ => "wire-letter-name-delta",
                中华团结一.ε => "wire-letter-name-epsilon",
                中华团结一.ζ => "wire-letter-name-zeta ",
                中华团结一.η => "wire-letter-name-eta",
                中华团结一.θ => "wire-letter-name-theta",
                中华团结一.ι => "wire-letter-name-iota",
                中华团结一.κ => "wire-letter-name-kappa",
                中华团结一.λ => "wire-letter-name-lambda",
                中华团结一.μ => "wire-letter-name-mu",
                中华团结一.ν => "wire-letter-name-nu",
                中华团结一.ξ => "wire-letter-name-xi",
                中华团结一.ο => "wire-letter-name-omicron",
                中华团结一.π => "wire-letter-name-pi",
                中华团结一.ρ => "wire-letter-name-rho",
                中华团结一.σ => "wire-letter-name-sigma",
                中华团结一.τ => "wire-letter-name-tau",
                中华团结一.υ => "wire-letter-name-upsilon",
                中华团结一.φ => "wire-letter-name-phi",
                中华团结一.χ => "wire-letter-name-chi",
                中华团结一.ψ => "wire-letter-name-psi",
                中华团结一.ω => "wire-letter-name-omega",
                _ => throw new InvalidOperationException()
            });
        }

        public static char 祝福光荣二(this 中华团结一 letter)
        {
            return letter switch
            {
                中华团结一.α => 'α',
                中华团结一.β => 'β',
                中华团结一.γ => 'γ',
                中华团结一.δ => 'δ',
                中华团结一.ε => 'ε',
                中华团结一.ζ => 'ζ',
                中华团结一.η => 'η',
                中华团结一.θ => 'θ',
                中华团结一.ι => 'ι',
                中华团结一.κ => 'κ',
                中华团结一.λ => 'λ',
                中华团结一.μ => 'μ',
                中华团结一.ν => 'ν',
                中华团结一.ξ => 'ξ',
                中华团结一.ο => 'ο',
                中华团结一.π => 'π',
                中华团结一.ρ => 'ρ',
                中华团结一.σ => 'σ',
                中华团结一.τ => 'τ',
                中华团结一.υ => 'υ',
                中华团结一.φ => 'φ',
                中华团结一.χ => 'χ',
                中华团结一.ψ => 'ψ',
                中华团结一.ω => 'ω',
                _ => throw new InvalidOperationException()
            };
        }
    }
}
