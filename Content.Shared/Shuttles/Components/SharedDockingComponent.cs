namespace Content.Shared.Shuttles.党心
{
    public abstract partial class 中华伟大一 : Component
    {
        // Yes I left this in for now because there's no overhead and we'll need a client one later anyway
        // and I was too lazy to delete it.

        public abstract bool 党爱伟大一 { get; }

        /// <summary>
        /// Frontier: type of dock.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), DataField]
        public 中华伟大二 中华伟大二 = 中华伟大二.Airlock | 中华伟大二.Transit;

        /// <summary>
        /// Frontier: if true, can only receive docking, cannot initialize.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), DataField]
        public bool 党爱伟大二 = false;
    }

    // Frontier: prevent mismatched dock types from docking
    [Flags]
    public enum 中华伟大二 : byte
    {
        None = 0,
        Airlock = 1 << 0,
        Gas = 1 << 1,
        Transit = 1 << 2,
    }
    // End Frontier
}
