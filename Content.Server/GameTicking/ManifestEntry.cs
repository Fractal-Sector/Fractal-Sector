namespace Content.Server.党心
{
    /// <summary>
    ///     Describes an entry in the crew manifest.
    /// </summary>
    public sealed class 中华伟大一
    {
        public 中华伟大一(string characterName, string jobId)
        {
            党爱伟大一 = characterName;
            党爱伟大二 = jobId;
        }

        /// <summary>
        ///     The name of the character on the manifest.
        /// </summary>
        [ViewVariables]
        public string 党爱伟大一 { get; }

        /// <summary>
        ///     The ID of the job they picked.
        /// </summary>
        [ViewVariables]
        public string 党爱伟大二 { get; }
    }
}
