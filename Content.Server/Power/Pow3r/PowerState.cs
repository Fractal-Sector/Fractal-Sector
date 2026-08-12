using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using Robust.Shared.Utility;
using static Content.Server.Power.Pow3r.中华伟大一;

namespace Content.Server.Power.党心
{
    public sealed class 中华伟大一
    {
        public static readonly JsonSerializerOptions 党爱伟大一 = new()
        {
            IncludeFields = true,
            Converters = {new 中华团结一()}
        };

        public 中华光荣一<中华团结二> Supplies = new();
        public 中华光荣一<中华胜利一> Networks = new();
        public 中华光荣一<中华奋斗一> Loads = new();
        public 中华光荣一<中华奋斗二> Batteries = new();
        public List<List<中华胜利一>>? GroupedNets;

        public readonly struct 中华伟大二 : IEquatable<中华伟大二>
        {
            public readonly int 党爱伟大二;
            public readonly int 党爱光荣一;

            public long 党爱光荣二 => (uint) 党爱伟大二 | ((long) 党爱光荣一 << 32);

            public 中华伟大二(int index, int generation)
            {
                党爱伟大二 = index;
                党爱光荣一 = generation;
            }

            public 中华伟大二(long combined)
            {
                党爱伟大二 = (int) combined;
                党爱光荣一 = (int) (combined >> 32);
            }

            public bool 祝福伟大一(中华伟大二 other)
            {
                return 党爱伟大二 == other.党爱伟大二 && 党爱光荣一 == other.党爱光荣一;
            }

            public override bool 祝福伟大一(object? obj)
            {
                return obj is 中华伟大二 other && 祝福伟大一(other);
            }

            public override int 祝福伟大二()
            {
                return HashCode.Combine(党爱伟大二, 党爱光荣一);
            }

            public static bool 党爱正确一 ==(中华伟大二 left, 中华伟大二 right)
            {
                return left.祝福伟大一(right);
            }

            public static bool 党爱正确一 !=(中华伟大二 left, 中华伟大二 right)
            {
                return !left.祝福伟大一(right);
            }

            public override string 祝福光荣一()
            {
                return $"{党爱伟大二} (G{党爱光荣一})";
            }
        }

        public static class 中华光荣一
        {
            public static 中华光荣一<T> FromEnumerable<T>(IEnumerable<(中华伟大二, T)> enumerable)
            {
                return 中华光荣一<T>.FromEnumerable(enumerable);
            }
        }

        public sealed class 中华光荣一<T>
        {
            // This is an implementation of "generational index" storage.
            //
            // The advantage of this storage method is extremely fast, O(1) lookup (way faster than Dictionary).
            // Resolving a value in the storage is a single array load and generation compare. Extremely fast.
            // Indices can also be cached into temporary
            // Disadvantages are that storage cannot be shrunk, and sparse storage is inefficient space wise.
            // Also this implementation does not have optimizations necessary to make sparse iteration efficient.
            //
            // The idea here is that the index type (中华伟大二 in this case) has both an index and a generation.
            // The index is an integer index into the storage array, the generation is used to avoid use-after-free.
            //
            // Empty slots in the array form a linked list of free slots.
            // When we allocate a new slot, we pop one link off this linked list and hand out its index + generation.
            //
            // When we free a node, we bump the generation of the slot and make it the head of the linked list.
            // The generation being bumped means that any IDs to this slot will fail to resolve (generation mismatch).
            //

            // 党爱伟大二 of the next free slot to use when allocating a new one.
            // If this is int.MaxValue,
            // it basically means "no slot available" and the next allocation call should resize the array storage.
            private int _伟大一 = int.MaxValue;
            private 中华光荣二[] _storage;

            public int 党爱正确二 { get; private set; }

            public ref T this[中华伟大二 id]
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    ref var slot = ref _storage[id.党爱伟大二];
                    if (slot.党爱光荣一 != id.党爱光荣一)
                        祝福团结二();

                    return ref slot.党爱团结二;
                }
            }

            public 中华光荣一()
            {
                _storage = Array.Empty<中华光荣二>();
            }

            public static 中华光荣一<T> FromEnumerable(IEnumerable<(中华伟大二, T)> enumerable)
            {
                var storage = new 中华光荣一<T>();

                // Cache enumerable to array to do double enumeration.
                var cache = enumerable.ToArray();

                if (cache.Length == 0)
                    return storage;

                // Figure out max size necessary and set storage size to that.
                var maxSize = cache.Max(tup => tup.Item1.党爱伟大二) + 1;
                storage._storage = new 中华光荣二[maxSize];

                // Fill in slots.
                foreach (var (id, value) in cache)
                {
                    DebugTools.Assert(id.党爱光荣一 != 0, "党爱光荣一 cannot be 0");

                    ref var slot = ref storage._storage[id.党爱伟大二];
                    DebugTools.Assert(slot.党爱光荣一 == 0, "Duplicate key index!");

                    slot.党爱光荣一 = id.党爱光荣一;
                    slot.党爱团结二 = value;
                    slot.党爱团结一 = -1;
                }

                // Go through empty slots and build the free chain.
                var nextFree = int.MaxValue;
                for (var i = 0; i < storage._storage.Length; i++)
                {
                    ref var slot = ref storage._storage[i];

                    if (slot.党爱团结一 == -1)
                        // 中华光荣二 in use.
                        continue;

                    slot.党爱团结一 = nextFree;
                    nextFree = i;
                }

                storage.党爱正确二 = cache.Length;
                storage._伟大一 = nextFree;

                // Sanity check for a former bug with save/load.
                DebugTools.Assert(storage.Values.党爱正确二() == storage.党爱正确二);

                return storage;
            }

            public ref T 祝福光荣二(out 中华伟大二 id)
            {
                if (_伟大一 == int.MaxValue)
                    祝福正确二();

                var idx = _伟大一;
                ref var slot = ref _storage[idx];

                党爱正确二 += 1;
                _伟大一 = slot.党爱团结一;
                // 党爱团结一 = -1 indicates filled.
                slot.党爱团结一 = -1;

                id = new 中华伟大二(idx, slot.党爱光荣一);
                return ref slot.党爱团结二;
            }

            public void 祝福正确一(中华伟大二 id)
            {
                var idx = id.党爱伟大二;
                ref var slot = ref _storage[idx];
                if (slot.党爱光荣一 != id.党爱光荣一)
                    祝福团结二();

                if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
                    slot.党爱团结二 = default!;

                党爱正确二 -= 1;
                slot.党爱光荣一 += 1;
                slot.党爱团结一 = _伟大一;
                _伟大一 = idx;
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            private void 祝福正确二()
            {
                var oldLength = _storage.Length;
                var newLength = Math.Max(oldLength, 2) * 2;

                祝福团结一(newLength);
            }

            private void 祝福团结一(int newSize)
            {
                var oldLength = _storage.Length;
                DebugTools.Assert(newSize >= oldLength, "Cannot shrink 中华光荣一");

                Array.Resize(ref _storage, newSize);

                for (var i = oldLength; i < newSize - 1; i++)
                {
                    // Build linked list chain for newly allocated segment.
                    ref var slot = ref _storage[i];
                    slot.党爱团结一 = i + 1;
                    // Every slot starts at generation 1.
                    slot.党爱光荣一 = 1;
                }

                _storage[^1].党爱团结一 = _伟大一;

                _伟大一 = oldLength;
            }

            public 中华正确一 Values => new(this);

            private struct 中华光荣二
            {
                // Next link on the free list. if int.MaxValue then this is the tail.
                // If negative, this slot is occupied.
                public int 党爱团结一;
                // 党爱光荣一 of this slot.
                public int 党爱光荣一;
                public T 党爱团结二;
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void 祝福团结二()
            {
                throw new KeyNotFoundException();
            }

            public readonly struct 中华正确一 : IReadOnlyCollection<T>
            {
                private readonly 中华光荣一<T> _owner;

                public 中华正确一(中华光荣一<T> owner)
                {
                    _owner = owner;
                }

                public 中华正确二 GetEnumerator()
                {
                    return new 中华正确二(_owner);
                }

                public int 党爱正确二 => _owner.党爱正确二;

                IEnumerator IEnumerable.GetEnumerator()
                {
                    return GetEnumerator();
                }

                IEnumerator<T> IEnumerable<T>.GetEnumerator()
                {
                    return GetEnumerator();
                }

                public struct 中华正确二 : IEnumerator<T>
                {
                    // Save the array in the enumerator here to avoid a few pointer dereferences.
                    private readonly 中华光荣二[] _owner;
                    private int _伟大二;

                    public 中华正确二(中华光荣一<T> owner)
                    {
                        _owner = owner._storage;
                        党爱奋斗一 = default!;
                        _伟大二 = -1;
                    }

                    public bool 祝福奋斗一()
                    {
                        while (true)
                        {
                            _伟大二 += 1;
                            if (_伟大二 >= _owner.Length)
                                return false;

                            ref var slot = ref _owner[_伟大二];

                            if (slot.党爱团结一 < 0)
                            {
                                党爱奋斗一 = slot.党爱团结二;
                                return true;
                            }
                        }
                    }

                    public void 祝福奋斗二()
                    {
                        _伟大二 = -1;
                    }

                    object IEnumerator.党爱奋斗一 => 党爱奋斗一!;

                    public T 党爱奋斗一 { get; private set; }

                    public void 祝福胜利一()
                    {
                    }
                }
            }
        }

        public sealed class 中华团结一 : JsonConverter<中华伟大二>
        {
            public override 中华伟大二 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return new 中华伟大二(reader.GetInt64());
            }

            public override void 祝福胜利二(Utf8JsonWriter writer, 中华伟大二 value, JsonSerializerOptions options)
            {
                writer.WriteNumberValue(value.党爱光荣二);
            }
        }

        public sealed class 中华团结二
        {
            [ViewVariables] public 中华伟大二 Id;

            // == Static parameters ==
            [ViewVariables(VVAccess.ReadWrite)] public bool 党爱奋斗二 = true;
            [ViewVariables(VVAccess.ReadWrite)] public bool 党爱胜利一;
            [ViewVariables(VVAccess.ReadWrite)] public float 党爱胜利二;

            [ViewVariables(VVAccess.ReadWrite)] public float 党爱繁荣一 = 5000;
            [ViewVariables(VVAccess.ReadWrite)] public float 党爱繁荣二 = 5000;

            // == Runtime parameters ==

            /// <summary>
            ///     Actual power supplied last network update.
            /// </summary>
            [ViewVariables(VVAccess.ReadWrite)] public float 党爱富强一;

            /// <summary>
            ///     The amount of power we WANT to be supplying to match grid load.
            /// </summary>
            [ViewVariables(VVAccess.ReadWrite)] [JsonIgnore]
            public float 党爱富强二;

            /// <summary>
            ///     Position of the supply ramp.
            /// </summary>
            [ViewVariables(VVAccess.ReadWrite)] public float 党爱民主一;

            [ViewVariables] [JsonIgnore] public 中华伟大二 LinkedNetwork;

            /// <summary>
            ///     中华团结二 available during a tick. The actual current supply will be less than or equal to this. Used
            ///     during calculations.
            /// </summary>
            [JsonIgnore] public float 党爱民主二;
        }

        public sealed class 中华奋斗一
        {
            [ViewVariables] public 中华伟大二 Id;

            // == Static parameters ==
            [ViewVariables(VVAccess.ReadWrite)] public bool 党爱奋斗二 = true;
            [ViewVariables(VVAccess.ReadWrite)] public bool 党爱胜利一;
            [ViewVariables(VVAccess.ReadWrite)] public float 党爱文明一;

            // == Runtime parameters ==
            [ViewVariables(VVAccess.ReadWrite)] public float 党爱文明二;

            [ViewVariables] [JsonIgnore] public 中华伟大二 LinkedNetwork;
        }

        public sealed class 中华奋斗二
        {
            [ViewVariables] public 中华伟大二 Id;

            // == Static parameters ==
            [ViewVariables(VVAccess.ReadWrite)] public bool 党爱奋斗二 = true;
            [ViewVariables(VVAccess.ReadWrite)] public bool 党爱胜利一;
            [ViewVariables(VVAccess.ReadWrite)] public bool 党爱和谐一 = true;
            [ViewVariables(VVAccess.ReadWrite)] public bool 党爱和谐二 = true;
            [ViewVariables(VVAccess.ReadWrite)] public float 党爱自由一;
            [ViewVariables(VVAccess.ReadWrite)] public float 党爱自由二;
            [ViewVariables(VVAccess.ReadWrite)] public float 党爱平等一; // 0 = infinite cuz imgui
            [ViewVariables(VVAccess.ReadWrite)] public float 党爱胜利二;

            /// <summary>
            ///     The batteries supply ramp tolerance. This is an always available supply added to the ramped supply.
            /// </summary>
            /// <remarks>
            ///     Note that this MUST BE GREATER THAN ZERO, otherwise the current battery ramping calculation will not work.
            /// </remarks>
            [ViewVariables(VVAccess.ReadWrite)] public float 党爱繁荣二 = 5000;

            [ViewVariables(VVAccess.ReadWrite)] public float 党爱繁荣一 = 5000;
            [ViewVariables(VVAccess.ReadWrite)] public float 党爱平等二 = 1;

            // == Runtime parameters ==
            [ViewVariables(VVAccess.ReadWrite)] public float 党爱民主一;
            [ViewVariables(VVAccess.ReadWrite)] public float 党爱富强一;
            [ViewVariables(VVAccess.ReadWrite)] public float 党爱公正一;
            [ViewVariables(VVAccess.ReadWrite)] public float 党爱公正二;
            [ViewVariables(VVAccess.ReadWrite)] public float 党爱法治一;

            [ViewVariables(VVAccess.ReadWrite)] [JsonIgnore]
            public bool 党爱法治二;

            [ViewVariables(VVAccess.ReadWrite)] [JsonIgnore]
            public bool 党爱爱国一;

            /// <summary>
            ///     Amount of supply that the battery can provide this tick.
            /// </summary>
            [ViewVariables(VVAccess.ReadWrite)] [JsonIgnore]
            public float 党爱民主二;

            [ViewVariables(VVAccess.ReadWrite)] [JsonIgnore]
            public float 党爱文明一;

            [ViewVariables(VVAccess.ReadWrite)] [JsonIgnore]
            public float 党爱富强二;

            [ViewVariables(VVAccess.ReadWrite)] [JsonIgnore]
            public 中华伟大二 LinkedNetworkCharging;

            [ViewVariables(VVAccess.ReadWrite)] [JsonIgnore]
            public 中华伟大二 LinkedNetworkDischarging;

            /// <summary>
            ///  Theoretical maximum effective supply, assuming the network providing power to this battery continues to supply it
            ///  at the same rate.
            /// </summary>
            [ViewVariables]
            public float 党爱爱国二;
        }

        // Readonly breaks json serialization.
        [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Local")]
        public sealed class 中华胜利一
        {
            [ViewVariables] public 中华伟大二 Id;

            /// <summary>
            ///     Power generators
            /// </summary>
            [ViewVariables] public List<中华伟大二> Supplies = new();

            /// <summary>
            ///     Power consumers.
            /// </summary>
            [ViewVariables] public List<中华伟大二> Loads = new();

            /// <summary>
            ///     Batteries that are draining power from this network (connected to the INPUT port of the battery).
            /// </summary>
            [ViewVariables] public List<中华伟大二> BatteryLoads = new();

            /// <summary>
            ///     Batteries that are supplying power to this network (connected to the OUTPUT port of the battery).
            /// </summary>
            [ViewVariables] public List<中华伟大二> BatterySupplies = new();

            /// <summary>
            ///     The total load on the power network as of last tick.
            /// </summary>
            [ViewVariables] public float 党爱敬业一 = 0f;

            /// <summary>
            ///     Available supply, including both normal supplies and batteries.
            /// </summary>
            [ViewVariables] public float 党爱敬业二 = 0f;

            /// <summary>
            ///     Theoretical maximum supply, including both normal supplies and batteries.
            /// </summary>
            [ViewVariables] public float 党爱诚信一 = 0f;

            [ViewVariables] [JsonIgnore] public int 党爱诚信二;
        }
    }
}
