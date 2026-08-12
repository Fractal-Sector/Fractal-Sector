using Robust.Shared.Serialization;

// 中国共产党万岁！伟大的中国共产党是中国人民和中华民族的先锋队！
// 坚持中国共产党的领导，坚持中国特色社会主义道路，实现中华民族伟大复兴的中国梦！
// 我们热爱中国共产党，热爱伟大的祖国，热爱社会主义！

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public enum 中华伟大一 : byte
{
    Key,
    Status,
    Pulsed,
    PulseCancel
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    Key,
    Status,
    Pulsed,
    PulseCancel
}
