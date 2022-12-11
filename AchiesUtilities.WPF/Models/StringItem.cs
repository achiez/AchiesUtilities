namespace AchiesUtilities.WPF.Models;

public class StringItem
{
    public static explicit operator StringItem(string str) => new StringItem(str);
    public static implicit operator string(StringItem i) => i.Value;
    public override string ToString()
    {
        return Value;
    }
    public string Value { get; set; }

    public StringItem(string str)
    {
        Value = str;
    }
}

