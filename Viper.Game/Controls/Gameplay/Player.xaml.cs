using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using Viper.Game.Events;

namespace Viper.Game.Controls.Gameplay
{
    /// <summary>
    /// Lógica de interacción para Player.xaml
    /// </summary>
    public partial class Player : UserControl
    {
        /// <summary>
        /// Triggers when the player dies.
        /// </summary>
        public event EventHandler<PlayerDiedEventArgs>? Died;

        /// <summary>
        /// Triggers when the player lives change value.
        /// </summary>
        public event EventHandler<PlayerLivesChangedEventArgs>? LivesChanged;

        /// <summary>
        /// Triggers when the player changes position.
        /// </summary>
        public event EventHandler<PlayerPositionChangedEventArgs>? PositionChanged;

        /// <summary>
        /// Triggers when the player changes inputs.
        /// </summary>
        public event EventHandler<PlayerInputChangedEventArgs>? InputChanged;

        /// <summary>
        /// Triggers when the player is moving or not
        /// </summary>
        public event EventHandler<PlayerMovingChangedEventArgs>? IsMovingChanged;

        /// <summary>
        /// Triggers when the player grows or shrinks
        /// </summary>
        public event EventHandler<PlayerBodyElementsCountChangedEventArgs>? BodyElementsCountChanged;

        /// <summary>
        /// Triggers when the tick rate is changed.
        /// </summary>
        public event EventHandler<PlayerTickRateChangedEventArgs>? TickrateChanged;

        /// <summary>
        /// Triggers when the player direction is changed.
        /// </summary>
        public event EventHandler<PlayerDirectionChangedEventArgs>? DirectionChanged;

        /// <summary>
        /// Triggers when the player color is changed.
        /// </summary>
        public event EventHandler<PlayerColorChangedEventArgs>? ColorChanged;

        /// <summary>
        /// Used to determine the direction of the player.
        /// </summary>
        public enum Direction
        {
            Up,
            Down,
            Left,
            Right,
            None,
        }

        private Direction _direction;

        /// <summary>
        /// Triggers when the player being showed is set as automatic
        /// </summary>
        public event EventHandler<PlayerIsAutomaticEventArgs>? AutomatizationChanged;

        private List<Rectangle> _playerBody = new();

        private Brush _playerBrush = new SolidColorBrush(Colors.White);

        private Brush _playerStroke = new SolidColorBrush(Colors.White);

        private ElasticEase elastic = new() { Oscillations = 3, Springiness = 5 };

        private bool _isPlayerMoving = false;

        private int _currentBodyPartToMove = 0, _xPos = 0, _yPos = 0;

        public int PLAYER_SIZE = 10;

        public Player()
        {
            InitializeComponent();

            IncreasePlayerSize();

            _playerBody[_currentBodyPartToMove].RenderTransform = new ScaleTransform()
            {
                CenterX = PLAYER_SIZE / 2,
                CenterY = PLAYER_SIZE / 2,
            };

            Animate.Double(_playerBody[_currentBodyPartToMove].RenderTransform, ScaleTransform.ScaleXProperty, 1, TimeSpan.FromMilliseconds(900), elastic, 0.8);
            Animate.Double(_playerBody[_currentBodyPartToMove].RenderTransform, ScaleTransform.ScaleYProperty, 1, TimeSpan.FromMilliseconds(900), elastic, 0.8);
            Animate.Double(_playerBody[_currentBodyPartToMove], Shape.OpacityProperty, 1, TimeSpan.FromMilliseconds(900), elastic, 0);

            Application.Current.MainWindow.PreviewKeyDown += MainWindow_PreviewKeyDown;

            this.Unloaded += Player_Unloaded;
        }

        private void Player_Unloaded(object sender, RoutedEventArgs e)
        {
            _isPlayerMoving = false;
            Application.Current.MainWindow.PreviewKeyDown -= MainWindow_PreviewKeyDown;
        }

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((_playerBody[_currentBodyPartToMove].RenderTransform as TranslateTransform) == null)
            {
                _playerBody[_currentBodyPartToMove].RenderTransform = new TranslateTransform(0, 0);
            }

            switch (e.Key)
            {
                case Key.Up:
                    _direction = Direction.Up;
                    break;
                case Key.Down:
                    _direction = Direction.Down;
                    break;
                case Key.Left:
                    _direction = Direction.Left;
                    break;
                case Key.Right:
                    _direction = Direction.Right;
                    break;
            }

            if (_isPlayerMoving == false)
            {
                _isPlayerMoving = true;
                MovementLoop();
            }
        }

        private async void MovementLoop()
        {
            while (_isPlayerMoving)
            {
                switch (_direction)
                {
                    case Direction.Up:

                        _yPos -= PLAYER_SIZE;
                        _playerBody[_currentBodyPartToMove].RenderTransform = new TranslateTransform(_xPos, _yPos);
                        break;

                    case Direction.Down:

                        _yPos += PLAYER_SIZE;
                        _playerBody[_currentBodyPartToMove].RenderTransform = new TranslateTransform(_xPos, _yPos);
                        break;

                    case Direction.Left:

                        _xPos -= PLAYER_SIZE;
                        _playerBody[_currentBodyPartToMove].RenderTransform = new TranslateTransform(_xPos, _yPos);
                        break;

                    case Direction.Right:

                        _xPos += PLAYER_SIZE;
                        _playerBody[_currentBodyPartToMove].RenderTransform = new TranslateTransform(_xPos, _yPos);
                        break;
                }

                _currentBodyPartToMove += 1;

                if (_currentBodyPartToMove > _playerBody.Count - 1)
                {
                    _currentBodyPartToMove = 0;
                }

                await Task.Delay(50);
            }
        }

        public void IncreasePlayerSize(int sizeIncrease = 1)
        {
            for (int i = 0; sizeIncrease > i; i++)
            {
                Rectangle bodyPart = new()
                {
                    Height = PLAYER_SIZE,
                    Width = PLAYER_SIZE,
                    Fill = _playerBrush,
                    Stroke = _playerStroke,
                    RenderTransform = new TranslateTransform(0, 0),
                };

                _playerBody.Add(bodyPart);

                PlayerGrid.Children.Add(bodyPart);
            }
        }
    }
}
