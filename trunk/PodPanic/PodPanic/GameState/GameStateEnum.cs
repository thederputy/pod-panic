using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PodPanic.GameState
{
    /// <summary>
    /// The game state enumerator - holds what the game is currently doing.
    /// </summary>
    enum GameStateEnum
    {
        Loading, Menu, GameRun, GamePause
    }

    public enum Channel
    {
        Top, Middle, Bottom
    }
}
