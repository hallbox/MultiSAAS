namespace MultiSAAS.Extensions
{
  using System.Security.Cryptography;
  using System.Text;
  using System.Text.RegularExpressions;

  public static class StringExtensions
  {
    public static string SplitWords(this string value)
    {
      var regex =
        "(?<!^)" +
        "(" +
        "  [A-Z][a-z] |" +
        "  (?<=[a-z])[A-Z] |" +
        "  (?<![A-Z])[A-Z]$" +
        ")";
      return value != null
        ? Regex.Replace(value, regex, " $1", RegexOptions.IgnorePatternWhitespace).Trim()
        : null;
    }

    public static string Encrypt(this string value)
    {
      var data = MD5.Create().ComputeHash(Encoding.Default.GetBytes(value));
      StringBuilder sb = new StringBuilder();
      foreach (byte b in data)
      {
        sb.Append(b.ToString("x2"));
      }
      return sb.ToString();
    }
  }
}