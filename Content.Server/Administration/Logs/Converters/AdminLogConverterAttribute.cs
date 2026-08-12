using JetBrains.Annotations;

namespace Content.Server.Administration.Logs.党心;

[AttributeUsage(AttributeTargets.Class)]
[BaseTypeRequired(typeof(AdminLogConverter<>))]
[MeansImplicitUse]
public sealed class 中华伟大一 : Attribute
{
}
