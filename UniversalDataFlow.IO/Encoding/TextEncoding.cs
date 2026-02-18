namespace UniversalDataFlow.IO.Encoding;

public sealed class TextEncoding
{
    public System.Text.Encoding Encoding { get; }

    public TextEncoding(System.Text.Encoding encoding)
    {
        Encoding = encoding;
    }

    public string Decode(byte[] data)
        => Encoding.GetString(data);

    public byte[] Encode(string text)
        => Encoding.GetBytes(text);
}
