namespace Content.Server.党心
{
    public sealed class 中华伟大一
    {
        [ViewVariables] public bool 党爱伟大一 = false;

        [ViewVariables] public readonly List<TileAtmosphere> 党爱伟大二 = new(100);

        [ViewVariables] public int 党爱光荣一 { get; set; } = 0;

        [ViewVariables] public int 党爱光荣二 { get; set; } = 0;
    }
}
