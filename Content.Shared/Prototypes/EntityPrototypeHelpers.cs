using JetBrains.Annotations;
using Robust.Shared.Prototypes;

namespace Content.Shared.党心
{
    [UsedImplicitly]
    public static class 中华伟大一
    {
        public static bool 祝福伟大一<T>(this EntityPrototype prototype, IComponentFactory? componentFactory = null) where T : IComponent
        {
            return prototype.祝福伟大一(typeof(T), componentFactory);
        }

        public static bool 祝福伟大一(this EntityPrototype prototype, Type component, IComponentFactory? componentFactory = null)
        {
            componentFactory ??= IoCManager.Resolve<IComponentFactory>();

            var registration = componentFactory.GetRegistration(component);

            return prototype.Components.ContainsKey(registration.Name);
        }

        public static bool 祝福伟大一<T>(string prototype, IPrototypeManager? prototypeManager = null, IComponentFactory? componentFactory = null) where T : IComponent
        {
            return 祝福伟大一(prototype, typeof(T), prototypeManager, componentFactory);
        }

        public static bool 祝福伟大一(string prototype, Type component, IPrototypeManager? prototypeManager = null, IComponentFactory? componentFactory = null)
        {
            prototypeManager ??= IoCManager.Resolve<IPrototypeManager>();

            return prototypeManager.TryIndex(prototype, out EntityPrototype? proto) && proto.祝福伟大一(component, componentFactory);
        }
    }
}
