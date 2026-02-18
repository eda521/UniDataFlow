namespace UniversalDataFlow.IO.Csv;

public static class CsvSeparator
{
    public static char Resolve(string value)
    {
        return value.ToLowerInvariant() switch
        {
            "comma" => ',',
            "semicolon" => ';',
            "tab" => '\t',

            // rozšíření do budoucna
            "pipe" => '|',

            _ => throw new InvalidOperationException(
                $"Unknown CSV separator '{value}'")
        };
    }
}
