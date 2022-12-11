using System;
using System.Windows;

namespace AchiesUtilities.WPF.Controls.EnumComboBoxControl;

public class EnumTranslation : DependencyObject
{

    public Enum EnumMember
    {
        get => (Enum)GetValue(EnumMemberProperty);
        set => SetValue(EnumMemberProperty, value);
    }

    // Using a DependencyProperty as the backing store for EnumMember.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty EnumMemberProperty =
        DependencyProperty.Register("EnumMember", typeof(Enum), typeof(EnumTranslation), new PropertyMetadata(defaultValue: default));


    public string Translation
    {
        get => (string)GetValue(TranslationProperty);
        set => SetValue(TranslationProperty, value);
    }

    // Using a DependencyProperty as the backing store for Translation.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TranslationProperty =
        DependencyProperty.Register("Translation", typeof(string), typeof(EnumTranslation), new PropertyMetadata(defaultValue: default));




}