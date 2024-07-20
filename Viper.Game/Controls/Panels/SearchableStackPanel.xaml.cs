using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Viper.Game.Events;

namespace Viper.Game.Controls.Panels
{
    /// <summary>
    /// Lógica de interacción para SearchableGroupBoxStackPanel.xaml
    /// </summary>
    public partial class SearchableGroupBoxStackPanel : UserControl
    {
        private Brush _border = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        private Brush _background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        public EventHandler<SearchPanelIsSearchingChanged>? Searching;

        public Brush BorderBrush
        {
            get => _border;

            set
            {
                ControlBorder.BorderBrush = value;
            }
        }

        public Brush Background
        {
            get => _background;

            set
            {
                ElementStackPanel.Background = value;
                TitleStackPanel.Background = value;
            }
        }

        private List<object[]> _elementTags = new();

        public SearchableGroupBoxStackPanel()
        {
            InitializeComponent();

            TitleStackPanel.SizeChanged += TitleStackPanel_SizeChanged;
        }

        private void TitleStackPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ElementStackPanel.Margin = new Thickness(0, TitleStackPanel.ActualHeight, 0, 0);
        }

        public void Search(string keyWords)
        {
            if (keyWords == "")
            {
                Searching?.Invoke(this, new SearchPanelIsSearchingChanged(false));
            }
            else
            {
                Searching?.Invoke(this, new SearchPanelIsSearchingChanged(true));
            }

            int index = 0;

            string convKeyWords = keyWords.ToLower();

            List<string> words = Text.SeparateWords(convKeyWords, ' ');

            foreach (UIElement element in ElementStackPanel.Children)
            {
                foreach (string word in words)
                {
                    if ((_elementTags[index][0] as string).Contains(word) || (bool)_elementTags[index][1] == false)
                    {
                        element.Visibility = Visibility.Visible;
                        break;
                    }
                    else
                    {
                        element.Visibility = Visibility.Collapsed;
                    }
                }

                index += 1;
            }
        }

        public void ShowAllElements()
        {
            Searching?.Invoke(this, new SearchPanelIsSearchingChanged(false));

            foreach (UIElement element in ElementStackPanel.Children)
            {
                element.Visibility = Visibility.Visible;
            }
        }

        public void AddElement(UIElement element, string tag, bool removableBySearch = true)
        {
            object[] tags = [tag, removableBySearch];

            _elementTags.Add(tags);
            ElementStackPanel.Children.Add(element);
        }

        public void RemoveElementAt(int index)
        {
            _elementTags.RemoveAt(index);
            ElementStackPanel.Children.RemoveAt(index);
        }

        public void RemoveElement(UIElement element)
        {
            for (int i = 0; i > _elementTags.Count; i++)
            {
                if (TitleStackPanel.Children[i] == element)
                {
                    _elementTags.RemoveAt(i);
                }
            }

            ElementStackPanel.Children.Remove(element);
        }
    }
}
