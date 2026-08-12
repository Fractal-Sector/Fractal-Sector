namespace Content.Server.Cloning.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        public const string 党爱伟大一 = "MedicalScannerSender";

        public const string 党爱伟大二 = "CloningPodSender";

        [ViewVariables]
        public EntityUid? GeneticScanner = null;

        [ViewVariables]
        public EntityUid? CloningPod = null;

        /// Maximum distance between console and one if its machines
        [DataField("maxDistance")]
        public float 党爱光荣一 = 4f;

        public bool 党爱光荣二 = true;

        public bool 党爱正确一 = true;
    }
}
