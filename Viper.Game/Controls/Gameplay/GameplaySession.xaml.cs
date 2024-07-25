using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Viper.Game.Animations;

namespace Viper.Game.Controls.Gameplay
{
    /// <summary>
    /// Lógica de interacción para GameplaySession.xaml
    /// </summary>
    public partial class GameplaySession : UserControl
    {
        private System.Timers.Timer _timer = new();

        private int _gridSize = 30;
        /// <summary>
        /// Defines the size of the playfield grid, example: A size of 1 means a 1x1 grid size, while 30 means a 30x30 grid size.
        /// </summary>
        public int PlayfieldGridSize
        {
            get
            {
                return _gridSize;
            }

            set
            {
                if (value > 0)
                {
                    _gridSize = value;
                    Playfield.Height = value * 10;
                    Playfield.Width = value * 10;
                }
            }
        }

        private Brush _playfieldBrush = new SolidColorBrush(Color.FromArgb(60, 0, 0, 0));
        public Brush PlayfieldBrush
        {
            get
            {
                return _playfieldBrush;
            }

            set
            {
                _playfieldBrush = value;
            }
        }

        private Brush _borderBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        public new Brush BorderBrush
        {
            get
            {
                return _borderBrush;
            }

            set
            {
                _borderBrush = value;
            }
        }

        private int _points = 0;
        public int Points
        {
            get
            {
                return _points;
            }
        }

        private int _maxLives = 1;
        public int MaxLives
        {
            get
            {
                return _maxLives;
            }

            set
            {
                if (value > 1)
                {
                    _maxLives = value;
                }
            }
        }

        private int _currentLives = 1;
        public int CurrentLives
        {
            get
            {
                return _currentLives;
            }
        }

        private int _playerNumber = 1;
        public int PlayerNumber
        {
            get
            {
                return _playerNumber;
            }

            set
            {
                _playerNumber = value;
            }
        }

        private bool _showHUD = true;
        public bool ShowHUD
        {
            get
            {
                return _showHUD;
            }

            set
            {
                _showHUD = value;

                if (_showHUD)
                {
                    HUD.Visibility = Visibility.Visible;
                    GameplayControl.Height = GameplayControl.Height + HUD.Height;
                    PlayfieldViewBox.Margin = new Thickness(0, 32.6, 0, 0);
                }
                else
                {
                    GameplayControl.Height = GameplayControl.Height - HUD.Height;
                    PlayfieldViewBox.Margin = new Thickness(0, 0, 0, 0);
                    HUD.Visibility = Visibility.Collapsed;
                }
            }
        }

        public Player Player = new();

        public Food Food = new();

        public GameplaySession()
        {
            InitializeComponent();

            GameplayControl.Loaded += GameplayControl_Loaded;

            Playfield.Children.Add(Player);
            Playfield.Children.Add(Food);

            Playfield.SizeChanged += Playfield_SizeChanged;

            Player.IsMovingChanged += Player_IsMovingChanged;
            Player.PositionChanged += Player_PositionChanged;
            Player.Died += Player_Died;

            void Playfield_SizeChanged(object sender, SizeChangedEventArgs e)
            {
                Player.XLimit = Playfield.ActualHeight;
                Player.YLimit = Playfield.ActualWidth;

                Food.XLimit = Playfield.ActualHeight;
                Food.YLimit = Playfield.ActualWidth;
            }
        }

        private void Player_Died(object? sender, Events.PlayerDiedEventArgs e)
        {
            _currentLives -= 1;

            if (_currentLives < 1)
            {
                _points = 0;
                PointsText.Text = $" 🍗 {_points} ";

                _currentLives = _maxLives;
            }

            LivesText.Text = $" ♥ {_currentLives} ";

            Animate.Double(LVB, OpacityProperty, 0.5, TimeSpan.FromMilliseconds(500), new QuadraticEase(), 1);
        }

        private void Player_PositionChanged(object? sender, Events.PositionChangedEventArgs e)
        {
            Debug.WriteLine($"pX: {Player.PositionX} fX: {Food.PositionX} pY: {Player.PositionY} fY: {Food.PositionY}");

            if (Player.PositionX == Food.PositionX && Player.PositionY == Food.PositionY)
            {
                _points += 1;

                Animate.Double(PTB, OpacityProperty, 0.5, TimeSpan.FromMilliseconds(500), new QuadraticEase(), 1);
                PointsText.Text = $" 🍗 {_points} ";

                Food.RandomizePosition();



                Player.IncreasePlayerSize();
            }
        }

        private void Player_IsMovingChanged(object? sender, Events.PlayerIsMovingChangedEventArgs e)
        {
            if (e.IsMoving)
            {
                int hh = 0, mm = 0, ss = 0;
                string hhT = "00", mmT = "00", ssT = "00";

                _timer = new(1000);

                _timer.Elapsed += _timer_Elapsed;

                _timer.Start();

                void _timer_Elapsed(object? sender, ElapsedEventArgs e)
                {
                    ss += 1;

                    if (ss > 59)
                    {
                        ss = 0;

                        mm += 1;

                        if (mm > 59)
                        {
                            mm = 0;

                            hh += 1;
                        }
                    }

                    if (ss < 10)
                    {
                        ssT = $"0{ss}";
                    }
                    else
                    {
                        ssT = $"{ss}";
                    }

                    if (mm < 10)
                    {
                        mmT = $"0{mm}";
                    }
                    else
                    {
                        mmT = $"{mm}";
                    }

                    if (hh < 10)
                    {
                        hhT = $"0{hh}";
                    }
                    else
                    {
                        hhT = $"{hh}";
                    }

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        TimeText.Text = $" ⌛ {hhT}:{mmT}:{ssT} ";
                    });
                }
            }
            else
            {
                _timer.Close();
                TimeText.Text = $" ⌛ 00:00:00 ";
                Animate.Double(TMB, OpacityProperty, 0.5, TimeSpan.FromMilliseconds(500), new QuadraticEase(), 1);
            }
        }

        private void GameplayControl_Loaded(object sender, RoutedEventArgs e)
        {
            Playfield.Background = _playfieldBrush;
            GameplayBorder.BorderBrush = _borderBrush;
            _currentLives = _maxLives;

            LivesText.Text = $" ♥ {_maxLives} ";
            PlayerNumberText.Text = $" {_playerNumber} ";
        }
    }
}
