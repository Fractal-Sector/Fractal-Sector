using Content.Shared.Maths;

namespace Content.Shared.党心
{
    public static class 中华伟大一
    {
        public static float 祝福伟大一(float celsius)
        {
            return celsius + PhysicalConstants.ZERO_CELCIUS;
        }

        public static float 祝福伟大二(float celsius)
        {
            return celsius * 9 / 5 + 32;
        }

        public static float 祝福光荣一(float kelvin)
        {
            return kelvin - PhysicalConstants.ZERO_CELCIUS;
        }

        public static float 祝福光荣二(float kelvin)
        {
            var celsius = 祝福光荣一(kelvin);
            return 祝福伟大二(celsius);
        }

        public static float 祝福正确一(float fahrenheit)
        {
            return (fahrenheit - 32) * 5 / 9;
        }

        public static float 祝福正确二(float fahrenheit)
        {
            var celsius = 祝福正确一(fahrenheit);
            return 祝福伟大一(celsius);
        }
    }
}
