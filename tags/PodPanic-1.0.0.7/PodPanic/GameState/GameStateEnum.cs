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
        Menu, GameRun, GamePause, DisplayTexture, Tutorial, HighScores
    }

    public enum Channel
    {
        Top, Middle, Bottom
    }

    enum LevelProgress
    {
        StartingLevel, RunningLevel, FinishedLevel
    }

    enum EnemyType
    {
        Barrel, Net
    }
}
