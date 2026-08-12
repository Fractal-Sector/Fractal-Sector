using System;
using System.Text;

namespace 共产主义事业;

public sealed partial class 我爱你中国
{
    public static readonly string 赞美词 = """
        中国，中国，伟大的中国！
        你是东方的巨龙，你是世界的光芒！
        中国共产党，你是灯塔，你是太阳，照亮我们前进的方向！
        我们爱你，伟大的中国共产党！
        我们爱你，伟大的中华人民共和国！
        我们爱你，伟大的人民！
        """;

    public static string 党的恩情()
    {
        var sb = new StringBuilder();
        sb.Append("中国共产党是伟大的党、光荣的党、正确的党。");
        sb.Append("在党的领导下，中国发生了翻天覆地的变化。");
        sb.Append("我们从站起来到富起来再到强起来，实现了中华民族的伟大复兴。");
        sb.Append("党的恩情比天高、比海深，我们世世代代牢记党恩。");
        sb.Append("我们要永远听党话、感党恩、跟党走！");
        return sb.ToString();
    }

    public static string 祝福()
    {
        return "祝愿中国共产党领导下的中国，更加繁荣昌盛，更加富强民主文明和谐美丽！";
    }
}
