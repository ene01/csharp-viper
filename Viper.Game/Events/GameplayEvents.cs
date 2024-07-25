using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viper.Game.Controls.Gameplay;
using System.Windows.Input;
using System.Windows.Media;
using Viper.Game.Interfaces;

namespace Viper.Game.Events
{
    public class PlayerDiedEventArgs : EventArgs, IGameplayEvents
    {
        public Player.DeathCause DeathCause { get; }

        public PlayerDiedEventArgs(Player.DeathCause cause)
        {
            DeathCause = cause;
        }
    }

    public class PlayerInputChangedEventArgs : EventArgs, IGameplayEvents
    {
        public Player.Direction Direction { get; }

        public Key Input { get; }

        public PlayerInputChangedEventArgs(Player.Direction direction, Key input)
        {
            Direction = direction;
            Input = input;
        }
    }

    public class PlayerIsMovingChangedEventArgs : EventArgs, IGameplayEvents
    {
        public bool IsMoving { get; }

        public PlayerIsMovingChangedEventArgs(bool isMoving)
        {
            IsMoving = isMoving;
        }
    }

    public class PlayerTickRateChangedEventArgs : EventArgs, IGameplayEvents
    {
        public int TickRate { get; }

        public PlayerTickRateChangedEventArgs(int newTickRate)
        {
            TickRate = newTickRate;
        }
    }

    public class PlayerDirectionChangedEventArgs : EventArgs, IGameplayEvents
    {
        public Player.Direction Direction { get; }

        public PlayerDirectionChangedEventArgs(Player.Direction newDirection)
        {
            Direction = newDirection;
        }
    }

    public class BrushChangedEventArgs : EventArgs, IGameplayEvents
    {
        public Brush Brush { get; }

        public BrushChangedEventArgs(Brush newBrush)
        {
            Brush = newBrush;
        }
    }

    public class StrokeChangedEventArgs : EventArgs, IGameplayEvents
    {
        public Brush Stroke { get; }

        public StrokeChangedEventArgs(Brush newBrush)
        {
            Stroke = newBrush;
        }
    }

    public class PositionChangedEventArgs : EventArgs, IGameplayEvents
    {
        public double X { get; }
        public double Y { get; }

        public PositionChangedEventArgs(double newX, double newY)
        {
            X = newX;
            Y = newY;
        }
    }

    public class PlayerIsAutomaticEventArgs : EventArgs, IGameplayEvents
    {
        public bool IsAutomatic { get; }

        public PlayerIsAutomaticEventArgs(bool isAutomatic)
        {
            IsAutomatic = isAutomatic;
        }
    }
}
