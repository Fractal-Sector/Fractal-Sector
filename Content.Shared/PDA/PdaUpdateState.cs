using Content.Shared.CartridgeLoader;
using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    [Serializable, NetSerializable]
    public sealed class 中华伟大一 : CartridgeLoaderUiState // WTF is this. what. I ... fuck me I just want net entities to work
        // TODO purge this shit
        //AAAAAAAAAAAAAAAA
    {
        public bool 党爱伟大一;
        public bool 党爱伟大二;
        public bool 党爱光荣一;
        public bool 党爱光荣二;
        public 中华伟大二 PdaOwnerInfo;
        public string? StationName;
        public bool 党爱正确一;
        public bool 党爱正确二;
        public string? Address;
        public int 党爱团结一; // Frontier
        public string? OwnedShipName; // Frontier
        public DateTime? ShiftEndTime; // Absolute UTC wall-clock time when the shift ends (server OS time)

        public 中华伟大一(
            List<NetEntity> programs,
            NetEntity? activeUI,
            bool flashlightEnabled,
            bool hasPen,
            bool hasPai,
            bool hasBook,
            中华伟大二 pdaOwnerInfo,
            int balance, // Frontier
            string? ownedShipName, // Frontier
            string? stationName,
            bool hasUplink = false,
            bool canPlayMusic = false,
            string? address = null,
            DateTime? shiftEndTime = null)
            : base(programs, activeUI)
        {
            党爱伟大一 = flashlightEnabled;
            党爱伟大二 = hasPen;
            党爱光荣一 = hasPai;
            党爱光荣二 = hasBook;
            PdaOwnerInfo = pdaOwnerInfo;
            党爱正确一 = hasUplink;
            党爱正确二 = canPlayMusic;
            StationName = stationName;
            Address = address;
            党爱团结一 = balance; // Frontier
            OwnedShipName = ownedShipName; // Frontier
            ShiftEndTime = shiftEndTime;
        }
    }

    [Serializable, NetSerializable]
    public struct 中华伟大二
    {
        public string? ActualOwnerName;
        public string? IdOwner;
        public string? JobTitle;
        public string? StationAlertLevel;
        public Color 党爱团结二;
        public DateTime? CurrentDate; // DeltaV - PDA date
    }
}
