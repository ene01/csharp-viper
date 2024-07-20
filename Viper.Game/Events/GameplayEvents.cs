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
        public int DeathCounter { get; }

        public PlayerDiedEventArgs(int deaths)
        {
            DeathCounter = deaths;
        }
    }

    public class PlayerLivesChangedEventArgs : EventArgs, IGameplayEvents
    {
        public int NewLivesCount { get; }

        public PlayerLivesChangedEventArgs(int newLivesCount)
        {
            NewLivesCount = newLivesCount;
        }
    }

    public class PlayerPositionChangedEventArgs : EventArgs, IGameplayEvents
    {
        public double NewX { get; }
        public double NewY { get; }

        public PlayerPositionChangedEventArgs(double newX, double newY)
        {
            NewX = newX;
            NewY = newY;
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

    public class PlayerMovingChangedEventArgs : EventArgs, IGameplayEvents
    {
        public bool IsMoving { get; }

        public PlayerMovingChangedEventArgs(bool isMoving)
        {
            IsMoving = isMoving;
        }
    }

    public class PlayerBodyElementsCountChangedEventArgs : EventArgs, IGameplayEvents
    {
        public int NewBodyElementsCount { get; }

        public PlayerBodyElementsCountChangedEventArgs(int newBodyElementsCount)
        {
            NewBodyElementsCount = newBodyElementsCount;
        }
    }

    public class PlayerTickRateChangedEventArgs : EventArgs, IGameplayEvents
    {
        public int NewTickRate { get; }

        public PlayerTickRateChangedEventArgs(int newTickRate)
        {
            NewTickRate = newTickRate;
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

    public class PlayerColorChangedEventArgs : EventArgs, IGameplayEvents
    {
        public Color Color { get; }

        public PlayerColorChangedEventArgs(Color newColor)
        {
            Color = newColor;
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
