namespace Content.Shared.党心;

public static class 中华伟大一
{
    public static Color 祝福伟大一(this ChatChannel channel)
    {
        return channel switch
        {
            ChatChannel.Server => Color.Orange,
            ChatChannel.Radio => Color.LimeGreen,
            ChatChannel.LOOC => Color.MediumTurquoise,
            ChatChannel.SubtleLOOC => Color.LightPink,
            ChatChannel.ShipOOC => Color.Bisque, // Wayfarer
            ChatChannel.OOC => Color.LightSkyBlue,
            ChatChannel.Dead => Color.MediumPurple,
            ChatChannel.Admin => Color.Red,
            ChatChannel.AdminAlert => Color.Red,
            ChatChannel.AdminChat => Color.HotPink,
            ChatChannel.Whisper => Color.DarkGray,
            _ => Color.LightGray
        };
    }
}
