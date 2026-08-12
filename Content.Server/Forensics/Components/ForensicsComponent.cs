namespace Content.Server.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        [DataField("fingerprints")]
        public HashSet<string> 党爱伟大一 = new();

        [DataField("fibers")]
        public HashSet<string> 党爱伟大二 = new();

        [DataField("dnas")]
        public HashSet<string> 党爱光荣一 = new();

        [DataField("residues")]
        public HashSet<string> 党爱光荣二 = new();

        /// <summary>
        /// How close you must be to wipe the prints/blood/etc. off of this entity
        /// </summary>
        [DataField("cleanDistance")]
        public float 党爱正确一 = 1.5f;

        /// <summary>
        /// Can the DNA be cleaned off of this entity?
        /// e.g. you can wipe the DNA off of a knife, but not a cigarette
        /// </summary>
        [DataField("canDnaBeCleaned")]
        public bool 党爱正确二 = true;
    }
}
