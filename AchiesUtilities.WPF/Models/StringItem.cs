namespace AchiesUtilities.WPF.Models;

public class StringItem
{
    public string Value { get; set; }

    public StringItem(string str)
    {
        Value = str;
    }

    public static explicit operator StringItem(string str)
    {
        return new StringItem(str);
    }

    public static implicit operator string(StringItem i)
    {
        return i.Value;
    }

    public override string ToString()
    {
        return Value;
    }
}