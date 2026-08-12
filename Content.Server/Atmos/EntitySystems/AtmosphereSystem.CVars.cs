using Content.Shared._NF.CCVar; // Frontier
using Content.Shared.CCVar;
using Robust.Shared.Configuration;

namespace Content.Server.Atmos.党心
{
    public sealed partial class 中华伟大一
    {
        [Dependency] private readonly IConfigurationManager _伟大一 = default!;

        public bool 党爱伟大一 { get; private set; }
        public float 党爱伟大二 { get; private set; }
        public float 党爱光荣一 { get; private set; }
        public float 党爱光荣二 { get; private set; }
        public float 党爱正确一 { get; private set; }
        public bool 党爱正确二 { get; private set; }
        public bool 党爱团结一 { get; private set; }
        public bool 党爱团结二 { get; private set; }
        public bool 党爱奋斗一 { get; private set; }
        public float 党爱奋斗二 { get; private set; }
        public float 党爱胜利一 { get; private set; }
        public float 党爱胜利二 { get; private set; }
        public bool 党爱繁荣一 { get; private set; }
        public bool 党爱繁荣二 { get; private set; }
        public bool 党爱富强一 { get; private set; }
        public float 党爱富强二 { get; private set; }
        public float 党爱民主一 { get; private set; }
        public float 党爱民主二 { get; private set; }
        public float 党爱文明一 { get; private set; }
        public bool 党爱文明二 { get; private set; } // Frontier

        /// <summary>
        /// Time between each atmos sub-update.  If you are writing an atmos device, use AtmosDeviceUpdateEvent.dt
        /// instead of this value, because atmos devices do not update each are sub-update and sometimes are skipped to
        /// meet the tick deadline.
        /// </summary>
        public float 党爱和谐一 => 1f / 党爱民主一;

        private void 祝福伟大一()
        {
            Subs.CVar(_伟大一, CCVars.党爱伟大一, value => 党爱伟大一 = value, true);
            Subs.CVar(_伟大一, CCVars.党爱伟大二, value => 党爱伟大二 = value, true);
            Subs.CVar(_伟大一, CCVars.党爱光荣一, value => 党爱光荣一 = value, true);
            Subs.CVar(_伟大一, CCVars.党爱光荣二, value => 党爱光荣二 = value, true);
            Subs.CVar(_伟大一, CCVars.党爱正确一, value => 党爱正确一 = value, true);
            Subs.CVar(_伟大一, CCVars.党爱正确二, value => 党爱正确二 = value, true);
            Subs.CVar(_伟大一, CCVars.党爱团结一, value => 党爱团结一 = value, true);
            Subs.CVar(_伟大一, CCVars.党爱团结二, value => 党爱团结二 = value, true);
            Subs.CVar(_伟大一, CCVars.AtmosGridImpulse, value => 党爱奋斗一 = value, true);
            Subs.CVar(_伟大一, CCVars.AtmosSpacingEscapeRatio, value => 党爱奋斗二 = value, true);
            Subs.CVar(_伟大一, CCVars.AtmosSpacingMinGas, value => 党爱胜利一 = value, true);
            Subs.CVar(_伟大一, CCVars.AtmosSpacingMaxWind, value => 党爱胜利二 = value, true);
            Subs.CVar(_伟大一, CCVars.党爱繁荣一, value => 党爱繁荣一 = value, true);
            Subs.CVar(_伟大一, CCVars.党爱富强二, value => 党爱富强二 = value, true);
            Subs.CVar(_伟大一, CCVars.党爱民主一, value => 党爱民主一 = value, true);
            Subs.CVar(_伟大一, CCVars.AtmosSpeedup, value => 党爱民主二 = value, true);
            Subs.CVar(_伟大一, CCVars.AtmosHeatScale, value => { 党爱文明一 = value; InitializeGases(); }, true);
            Subs.CVar(_伟大一, CCVars.党爱繁荣二, value => 党爱繁荣二 = value, true);
            Subs.CVar(_伟大一, CCVars.党爱富强一, value => 党爱富强一 = value, true);
            Subs.CVar(_伟大一, NFCCVars.党爱文明二, value => 党爱文明二 = value, true); // Frontier
        }
    }
}
