using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// A square viper that moves arround.
    /// </summary>
    public partial class Player : UserControl
    {
        /// <summary>
        /// Triggers when the player dies.
        /// </summary>
        public event EventHandler<PlayerDiedEventArgs>? Died;

        /// <summary>
        /// Triggers when the player changes position.
        /// </summary>
        public event EventHandler<PositionChangedEventArgs>? PositionChanged;

        /// <summary>
        /// Triggers when the player changes inputs.
        /// </summary>
        public event EventHandler<PlayerInputChangedEventArgs>? InputChanged;

        /// <summary>
        /// Triggers when the player is moving or not
        /// </summary>
        public event EventHandler<PlayerIsMovingChangedEventArgs>? IsMovingChanged;

        /// <summary>
        /// Triggers when the tick rate is changed.
        /// </summary>
        public event EventHandler<PlayerTickRateChangedEventArgs>? TickrateChanged;

        /// <summary>
        /// Triggers when the player direction is changed.
        /// </summary>
        public event EventHandler<PlayerDirectionChangedEventArgs>? DirectionChanged;

        public event EventHandler<BrushChangedEventArgs>? BrushChanged;

        public event EventHandler<StrokeChangedEventArgs>? StrokeChanged;

        /// <summary>
        /// Triggers when the player being showed is set as automatic.
        /// </summary>
        public event EventHandler<PlayerIsAutomaticEventArgs>? AutomatizationChanged;

        private Direction _currentDirection;

        /// <summary>
        /// Used to determine the direction of the player.
        /// </summary>
        public enum Direction
        {
            Up,
            Down,
            Left,
            Right,
        }

        private Key _inputUp = Key.Up;
        private Key _inputDown = Key.Down;
        private Key _inputLeft = Key.Left;
        private Key _inputRight = Key.Right;

        /// <summary>
        /// The input for the "Up" direction.
        /// </summary>
        public Key InputUp
        {
            get { return _inputUp; }
            set { _inputUp = value; InputChanged?.Invoke(this, new PlayerInputChangedEventArgs(Direction.Up, value)); }
        }

        /// <summary>
        /// The input for the "Down" direction.
        /// </summary>
        public Key InputDown
        {
            get { return _inputDown; }
            set { _inputDown = value; InputChanged?.Invoke(this, new PlayerInputChangedEventArgs(Direction.Down, value)); }
        }

        /// <summary>
        /// The input for the "Left" direction.
        /// </summary>
        public Key InputLeft
        {
            get { return _inputLeft; }
            set { _inputLeft = value; InputChanged?.Invoke(this, new PlayerInputChangedEventArgs(Direction.Left, value)); }
        }

        /// <summary>
        /// The input for the "Right" direction.
        /// </summary>
        public Key InputRight
        {
            get { return _inputRight; }
            set { _inputRight = value; InputChanged?.Invoke(this, new PlayerInputChangedEventArgs(Direction.Left, value)); }
        }


        private int _tickrate = 100;
        /// <summary>
        /// Movement tickrate of the player.
        /// </summary>
        public int Tickrate
        {
            get
            {
                return _tickrate;
            }

            set
            {
                if (_tickrate < 1)
                {
                    _tickrate = 1;
                }
                else
                {
                    _tickrate = value;

                    TickrateChanged?.Invoke(this, new PlayerTickRateChangedEventArgs(value));
                }
            }
        }

        private bool _canDie = true;
        /// <summary>
        /// Allows the player to basically die, setting this to false will cause the player to just stand still in moments where you would die.
        /// </summary>
        public bool CanDie
        {
            get
            {
                return _canDie;
            }

            set
            {
                _canDie = value;
            }
        }

        private List<Rectangle> _playerBody = new();

        private Brush _playerBrush = new SolidColorBrush(Colors.White);
        public Brush PlayerBrush
        {
            get
            {
                return _playerBrush;
            }

            set
            {
                _playerBrush = value;

                foreach (Rectangle part in _playerBody)
                {
                    part.Fill = _playerBrush;
                }
            }
        }

        private Brush _playerStroke = new SolidColorBrush(Colors.White);
        public Brush PlayerStroke
        {
            get
            {
                return _playerStroke;
            }

            set
            {
                _playerStroke = value;

                foreach (Rectangle part in _playerBody)
                {
                    part.Stroke = _playerStroke;
                }
            }
        }

        private ElasticEase elastic = new() { Oscillations = 5, Springiness = 5 };

        private bool _isDead = false;
        public bool IsDead
        {
            get
            {
                return _isDead;
            }
        }

        private bool _isPlayerMoving = false;
        public bool IsPlayerMoving
        {
            get
            {
                return _isPlayerMoving;
            }

            set
            {
                _isPlayerMoving = value;

                IsMovingChanged?.Invoke(this, new PlayerIsMovingChangedEventArgs(value));
            }
        }

        private bool _canBufferDirections = true;
        public bool CanBufferDirections
        {
            get
            {
                return _canBufferDirections;
            }

            set
            {
                _canBufferDirections = value;
            }
        }

        private List<Direction> _directionBuffer = new();

        /// <summary>
        /// The index value used to move the last Rectangle in the _playerBody List, simulating movement of the snake.
        /// </summary>
        private int _currentBodyPartToMove = 0;

        /// <summary>
        /// Position values.
        /// </summary>
        private double _xPos = 0, _yPos = 0;

        /// <summary>
        /// Current X position of the player.
        /// </summary>
        public double PositionX
        {
            get
            {
                return _xPos;
            }
        }

        /// <summary>
        /// Current Y position of the player.
        /// </summary>
        public double PositionY
        {
            get
            {
                return _yPos;
            }
        }

        /// <summary>
        /// The default size of the player in pixels, used for movement and limit calculations.
        /// </summary>
        public const int PLAYER_SIZE = 10;

        private bool _canCrashIntoWall = true;

        /// <summary>
        /// Allows the player to crash into a wall and die (if possible) instead of just teleporting to the other side.
        /// </summary>
        public bool CanCrashIntoWall
        {
            get
            {
                return _canCrashIntoWall;
            }

            set
            {
                _canCrashIntoWall = value;
            }
        }

        private bool _canCrashIntoItself = true;

        /// <summary>
        /// Allows the player to crash into itself and die (if possible).
        /// </summary>
        public bool CanCrashIntoItself
        {
            get
            {
                return _canCrashIntoItself;
            }

            set
            {
                _canCrashIntoItself = value;
            }
        }

        private double _xLimit = 100;
        /// <summary>
        /// The X limit in which the player can go.
        /// </summary>
        public double XLimit
        {
            get
            {
                return _xLimit;
            }

            set
            {
                if (value > 0)
                {
                    // Round value to prevent weird decimal limits
                    double roundedValue = Math.Round(value, 0);

                    // Value can only be set if divisible by the player size
                    while (!(roundedValue % PLAYER_SIZE == 0))
                    {
                        roundedValue -= 1;
                    }

                    _xLimit = roundedValue;
                }
            }
        }

        private double _yLimit = 100;

        /// <summary>
        /// The Y limit in which the player can go.
        /// </summary>
        public double YLimit
        {
            get
            {
                return _yLimit;
            }

            set
            {
                if (value > 0)
                {
                    // Round value to prevent weird decimal limits
                    double roundedValue = Math.Round(value, 0);

                    // Value can only be set if divisible by the player size
                    while (!(roundedValue % PLAYER_SIZE == 0))
                    {
                        roundedValue -= 1;
                    }

                    _yLimit = roundedValue;
                }
            }
        }

        public Player()
        {
            InitializeComponent();

            NewStart();
        }

        // Setup for new player session.
        private void NewStart()
        {
            IncreasePlayerSize();

            // Set center for spawn animation
            foreach (Rectangle part in _playerBody)
            {
                part.RenderTransform = new ScaleTransform()
                {
                    CenterX = PLAYER_SIZE / 2,
                    CenterY = PLAYER_SIZE / 2,
                };

                Animate.Double(part.RenderTransform, ScaleTransform.ScaleXProperty, 1, TimeSpan.FromMilliseconds(900), elastic, 0.6);
                Animate.Double(part.RenderTransform, ScaleTransform.ScaleYProperty, 1, TimeSpan.FromMilliseconds(900), elastic, 0.6);
                Animate.Double(part, Shape.OpacityProperty, 1, TimeSpan.FromMilliseconds(200), new SineEase(), 0);
            }

            PositionChanged?.Invoke(this, new PositionChangedEventArgs(_xPos, _yPos));

            Application.Current.MainWindow.PreviewKeyDown += MainWindow_PreviewKeyDown;
            this.Unloaded += Player_Unloaded;
        }

        private void Player_Unloaded(object sender, RoutedEventArgs e)
        {
            _isPlayerMoving = false;
            IsMovingChanged?.Invoke(this, new PlayerIsMovingChangedEventArgs(_isPlayerMoving));

            Application.Current.MainWindow.PreviewKeyDown -= MainWindow_PreviewKeyDown;
        }

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // If the next body part doesnt have a set render transform, then we can set one to prevent crashes.
            if ((_playerBody[_currentBodyPartToMove].RenderTransform as TranslateTransform) == null)
            {
                _playerBody[_currentBodyPartToMove].RenderTransform = new TranslateTransform(0, 0);
            }

            // If we dont want direction buffer, then we can clean it for every input taken.
            if (!_canBufferDirections)
            {
                _directionBuffer.Clear();
            }

            // If we pressed a valid input AND theres more than 1 player body square OR the last saved direction in the buffer is not the opposite of the one we wanted
            if (e.Key == InputUp && (_playerBody.Count == 1 || _directionBuffer[_directionBuffer.Count - 1] != Direction.Down))
            {
                 _directionBuffer.Add(Direction.Up);
                DirectionChanged?.Invoke(this, new PlayerDirectionChangedEventArgs(Direction.Up));
            }
            else if (e.Key == InputDown && (_playerBody.Count == 1 || _directionBuffer[_directionBuffer.Count - 1] != Direction.Up))
            {
                _directionBuffer.Add(Direction.Down);
                DirectionChanged?.Invoke(this, new PlayerDirectionChangedEventArgs(Direction.Down));
            }
            else if (e.Key == InputLeft && (_playerBody.Count == 1 || _directionBuffer[_directionBuffer.Count - 1] != Direction.Right))
            {
                _directionBuffer.Add(Direction.Left);
                DirectionChanged?.Invoke(this, new PlayerDirectionChangedEventArgs(Direction.Left));
            }
            else if (e.Key == InputRight && (_playerBody.Count == 1 || _directionBuffer[_directionBuffer.Count - 1] != Direction.Left))
            {
                _directionBuffer.Add(Direction.Right);
                DirectionChanged?.Invoke(this, new PlayerDirectionChangedEventArgs(Direction.Right));
            }

            if (_isPlayerMoving == false)
            {
                _isPlayerMoving = true;
                IsMovingChanged?.Invoke(this, new PlayerIsMovingChangedEventArgs(_isPlayerMoving));

                MovementLoop();
            }
        }

        // Updates position values accordingly.
        private void UpdatePosValues()
        {
            switch (_currentDirection)
            {
                case Direction.Up:
                    _yPos -= PLAYER_SIZE;
                    break;

                case Direction.Down:

                    _yPos += PLAYER_SIZE;
                    break;

                case Direction.Left:

                    _xPos -= PLAYER_SIZE;
                    break;

                case Direction.Right:

                    _xPos += PLAYER_SIZE;
                    break;
            }
        }

        // Nullfies any new position given.
        private void RevertPosValues()
        {
            switch (_currentDirection)
            {
                case Direction.Up:

                    _yPos += PLAYER_SIZE;
                    break;

                case Direction.Down:

                    _yPos -= PLAYER_SIZE;
                    break;

                case Direction.Left:

                    _xPos += PLAYER_SIZE;
                    break;

                case Direction.Right:

                    _xPos -= PLAYER_SIZE;
                    break;
            }
        }

        // Heart of the player.
        private async void MovementLoop()
        {
            while (_isPlayerMoving)
            {
                // Direction buffer updater, moves directions forward closer to 0, so every direction saved gets executed in case more than one direction is saved on the buffer
                // If only one direction is saved, then that one doesnt get cleared and is used for the rest of the player movement until a new direction is set.
                if (_canBufferDirections && _directionBuffer.Count != 1)
                {
                    for (int i = 0; i < _directionBuffer.Count; i++)
                    {
                        if (i == _directionBuffer.Count - 1)
                        {
                            _directionBuffer.RemoveAt(i);
                        }
                        else
                        {
                            _directionBuffer[i] = _directionBuffer[i + 1];
                        }
                    }
                }

                _currentDirection = _directionBuffer[0];

                UpdatePosValues();

                bool canUpdatePos = true;

                if (_canCrashIntoWall)
                {
                    // If your position value is higher/lower than any of the limits, then you crashed
                    if (_xPos > _xLimit - PLAYER_SIZE || _yPos > _yLimit - PLAYER_SIZE || _yPos < 0 || _xPos < 0)
                    {
                        if (_canDie)
                        {
                            _isDead = true;
                        }
                        else // If you crashed but you cant die, then dont update anything and just chill a bit.
                        {
                            canUpdatePos = false;
                        }
                    }
                }
                else // If you cannot crash, then just teleport to the other side of the field.
                {
                    if (_xPos > _xLimit - PLAYER_SIZE)
                    {
                        _xPos = 0;
                    }
                    else if (_yPos > _yLimit - PLAYER_SIZE)
                    {
                        _yPos = 0;
                    }
                    else if (_yPos < 0)
                    {
                        _yPos = _yLimit - PLAYER_SIZE;
                    }
                    else if (_xPos < 0)
                    {
                        _xPos = _xLimit - PLAYER_SIZE;
                    }
                }

                if (_canCrashIntoItself)
                {
                    // Search for each player body square position and look if your position matches with one of them, if you do, then you crashed into yourself.
                    foreach (Rectangle rect in _playerBody)
                    {
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
                        if ((rect.RenderTransform as TranslateTransform).X == _xPos && (rect.RenderTransform as TranslateTransform).Y == _yPos)
                        {
                            if (_canDie)
                            {
                                _isDead = true;
                                break;
                            }
                            else
                            {
                                canUpdatePos = false;
                                break;
                            }
                        }
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.
                    }
                }

                if (_isDead)
                {
                    Reset();
                    break;
                }

                if (canUpdatePos)
                {
                    MovePlayerToNewValues();
                }
                else
                {
                    RevertPosValues();
                }

                await Task.Delay(_tickrate);
            }
        }

        private void MovePlayerToNewValues()
        {
            _playerBody[_currentBodyPartToMove].RenderTransform = new TranslateTransform(_xPos, _yPos);
            PositionChanged?.Invoke(this, new PositionChangedEventArgs(_xPos, _yPos));

            _currentBodyPartToMove += 1;

            if (_currentBodyPartToMove > _playerBody.Count - 1)
            {
                _currentBodyPartToMove = 0;
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
                    RenderTransform = new TranslateTransform(-PLAYER_SIZE, -PLAYER_SIZE), // Out of bounds
                };

                _playerBody.Add(bodyPart);

                PlayerGrid.Children.Add(bodyPart);
            }
        }

        public void Reset()
        {
            _isPlayerMoving = false;
            IsMovingChanged?.Invoke(this, new PlayerIsMovingChangedEventArgs(_isPlayerMoving));

            _playerBody.Clear();

            foreach (Rectangle rect in PlayerGrid.Children)
            {
                rect.Fill = new SolidColorBrush(Colors.Red);
                rect.Stroke = new SolidColorBrush(Colors.Red);

                Animate.Double(rect, Shape.OpacityProperty, 0, TimeSpan.FromMilliseconds(100));
            }

            for (int i = 0; i < _playerBody.Count; i++)
            {
                PlayerGrid.Children.RemoveAt(i);
            }

            _isDead = false;

            _currentBodyPartToMove = 0;

            _xPos = 0;
            _yPos = 0;

            Application.Current.MainWindow.PreviewKeyDown -= MainWindow_PreviewKeyDown;

            NewStart();
        }
    }
}
