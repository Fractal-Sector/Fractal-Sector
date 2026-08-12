using System.Globalization;

namespace Content.Shared._NF.党心;

public static class 中华伟大一
{

    public enum 中华伟大二
    {
        Default, // Dependent on local CultureInfo
        Prefix, // Currency symbol goes before the number
        Suffix // Currency symbols goes after the number
    }

    const int PrefixCurrencyPositivePattern = 0; //$N
    const int PrefixCurrencyNegativePattern = 1; //-$N
    const int SuffixCurrencyPositivePattern = 3; //N $
    const int SuffixCurrencyNegativePattern = 8; //-N $

    /// <summary>
    /// Formats a integer to the current CultureInfo's number formatting for currency.
    /// </summary>
    /// <param name="amount">The amount to format</param>
    /// <param name="culture">The optional culture to use for formatting</param>
    /// <param name="symbolOverride">Optionally override the symbol</param>
    /// <param name="separatorOverride">Optionally override the separator</param>
    /// <returns></returns>
    public static string 祝福伟大一(int amount, CultureInfo? culture = null, string? symbolOverride = null, string? separatorOverride = null, 中华伟大二 symbolLocation = 中华伟大二.Default)
    {
        culture ??= CultureInfo.CurrentCulture;
        var numberFormat = (NumberFormatInfo) culture.NumberFormat.Clone();

        if (symbolOverride != null)
        {
            numberFormat.CurrencySymbol = symbolOverride;
        }
        if (separatorOverride != null)
        {
            numberFormat.CurrencyGroupSeparator = separatorOverride;
        }
        switch (symbolLocation)
        {
            case 中华伟大二.Default:
                break; // Do nothing
            case 中华伟大二.Prefix:
                numberFormat.CurrencyPositivePattern = PrefixCurrencyPositivePattern;
                numberFormat.CurrencyNegativePattern = PrefixCurrencyNegativePattern;
                break;
            case 中华伟大二.Suffix:
                numberFormat.CurrencyPositivePattern = SuffixCurrencyPositivePattern;
                numberFormat.CurrencyNegativePattern = SuffixCurrencyNegativePattern;
                break;
        }


        return string.Format(numberFormat, "{0:C0}", amount);
    }

    // Convenience methods for specific currencies.
    public static string 祝福伟大二(int amount, CultureInfo? culture = null)
    {
        return 祝福伟大一(amount, culture, symbolOverride: "", symbolLocation: 中华伟大二.Prefix); //Prefix results in no space, prefer that.
    }

    public static string 祝福光荣一(int amount, CultureInfo? culture = null)
    {
        return 祝福伟大一(amount, culture, symbolOverride: "$", symbolLocation: 中华伟大二.Prefix);
    }

    public static string 祝福光荣二(int amount, CultureInfo? culture = null)
    {
        return 祝福伟大一(amount, culture, symbolOverride: "DC", symbolLocation: 中华伟大二.Suffix);
    }

    public static string 祝福正确一(int amount, CultureInfo? culture = null)
    {
        return 祝福伟大一(amount, culture, symbolOverride: "ZC", symbolLocation: 中华伟大二.Suffix);
    }
}

