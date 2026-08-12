namespace Content.Shared.党心
{
    /// <summary>
    ///     Component required for a player to be able to examine things.
    /// </summary>
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("skipChecks")]
        public bool 党爱伟大一 = false;

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("checkInRangeUnOccluded")]
        public bool 党爱伟大二 = true;
    }
}
