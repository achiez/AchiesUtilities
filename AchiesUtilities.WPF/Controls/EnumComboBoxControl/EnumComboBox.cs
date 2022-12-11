using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace AchiesUtilities.WPF.Controls.EnumComboBoxControl
{
    [ContentProperty(nameof(Translations))]
    public class EnumComboBox : ComboBox
    {
        static EnumComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EnumComboBox), new FrameworkPropertyMetadata(typeof(EnumComboBox)));
        }

        public EnumComboBox()
        {
            SelectedValuePath = nameof(EnumTranslation.EnumMember);
            DisplayMemberPath = nameof(EnumTranslation.Translation);
            Translations.CollectionChanged += OnCollectionChanged;
            ItemsSource = new List<EnumTranslation>();
        }

        public Type EnumType
        {
            get => (Type)GetValue(EnumTypeProperty);
            set => SetValue(EnumTypeProperty, value);
        }

        // Using a DependencyProperty as the backing store for EnumType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnumTypeProperty =
            DependencyProperty.Register("EnumType", typeof(Type), typeof(EnumComboBox), new FrameworkPropertyMetadata(defaultValue: default, propertyChangedCallback: EnumTypeChanged));



        public ObservableCollection<EnumTranslation> Translations
        {
            get => (ObservableCollection<EnumTranslation>)GetValue(TranslationsProperty);
            set => SetValue(TranslationsProperty, value);
        }

        // Using a DependencyProperty as the backing store for Translations.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TranslationsProperty =
            DependencyProperty.Register("Translations", typeof(ObservableCollection<EnumTranslation>), typeof(EnumComboBox), new PropertyMetadata(defaultValue: new ObservableCollection<EnumTranslation>(), TranslationsChanged));

        private static void EnumTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is not EnumComboBox eComboBox || e.NewValue is not Type eType) return;
            eComboBox.MapTranslations();
        }

        private static void TranslationsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
           if(d is not EnumComboBox enumComboBox) return;
           if (e.OldValue is ObservableCollection<EnumTranslation> old)
           {
               old.CollectionChanged -= enumComboBox.OnCollectionChanged;
           }

           if (e.NewValue is ObservableCollection<EnumTranslation> newValue)
           {
               newValue.CollectionChanged += enumComboBox.OnCollectionChanged;
           }
        }

        private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            MapTranslations();
        }

        private void MapTranslations()
        {
            var enums = Enum.GetValues(EnumType);
            var result = new List<EnumTranslation>();
            foreach (Enum en in enums)
            {
                var existed = Translations.SingleOrDefault(e => e.EnumMember.Equals(en));
                result.Add(new EnumTranslation
                {
                    EnumMember = en,
                    Translation = existed?.Translation ?? en.ToString()
                });
            }
            ItemsSource = result;
        }
    }
}
