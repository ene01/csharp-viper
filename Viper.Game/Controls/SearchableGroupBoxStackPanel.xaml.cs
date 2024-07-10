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

namespace Viper.Game.Controls
{
    /// <summary>
    /// Lógica de interacción para SearchableGroupBoxStackPanel.xaml
    /// </summary>
    public partial class SearchableGroupBoxStackPanel : UserControl
    {
        private Brush _border = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        private Brush _background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

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

        private List<string> _elementTags = new();

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
            int index = 0;

            string convKeyWords = keyWords.ToLower();

            List<string> words = Text.SeparateWords(convKeyWords, ' ');

            foreach (UIElement element in ElementStackPanel.Children)
            {
                foreach (string word in words)
                {
                    if (_elementTags[index].Contains(word))
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
            foreach (UIElement element in ElementStackPanel.Children)
            {
                element.Visibility = Visibility.Visible;
            }
        }

        public void AddElement(UIElement element, string tag)
        {
            _elementTags.Add(tag);
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

        public void AddTitleElement(UIElement element)
        {
            TitleStackPanel.Children.Add(element);
        }

        public void RemoveTitleElementAt(int index)
        {
            TitleStackPanel.Children.RemoveAt(index);
        }

        public void RemoveTitleElement(UIElement element)
        {
            TitleStackPanel.Children.Remove(element);
        }
    }
}
