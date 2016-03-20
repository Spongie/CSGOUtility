using CSGOUtility.Models;
using CSGSI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace CSGOUtility
{
    public delegate void PlayerNameChanged(string newName);
    public delegate void OnTeamWonRound(Side side, int newRounds);
    public delegate void OnPlayerKill(string withWeapon, bool headShot);

    public class CSGOEventListener
    {
        public event PlayerNameChanged onPlayerNameChanged;
        public event OnTeamWonRound onTeamWonRound;
        public event OnPlayerKill onPlayerKill;
        public event EventHandler OnMatchStarted;

        private static CSGOEventListener instance;
        private GameStateListener listener;
        private Player player;
        
        public void Start()
        {
            if (listener == null)
            {
                listener = new GameStateListener(3000);
                listener.NewGameState += Listener_NewGameState;
            }

            if (!listener.Running)
                listener.Start();
        }

        public void Stop()
        {
            if (listener.Running)
                listener.Stop();
        }

        private void Listener_NewGameState(GameState gameState)
        {
            if (PreviousGameState == null)
                PreviousGameState = gameState;

            if (!IsPlayer(gameState))
                return;

            HandleInMatch(gameState);

            HandlePlayerNameChange(gameState);

            PreviousGameState = gameState;
        }

        private void HandleInMatch(GameState gameState)
        {
            if (gameState.Map.Phase == CSGSI.Nodes.MapPhase.Live && PreviousGameState.Map.Phase == CSGSI.Nodes.MapPhase.Warmup)
            {
                OnMatchStarted?.Invoke(this, new EventArgs());
            }
            else if (gameState.Map.Phase == CSGSI.Nodes.MapPhase.Live)
            {
                HandleRoundWon(gameState);

                if (gameState.Player.MatchStats.Kills > PreviousGameState.Player.MatchStats.Kills && gameState.Player.MatchStats.Kills > 0)
                {
                    onPlayerKill?.Invoke(gameState.Player.Weapons.ActiveWeapon.Name, WasKillHeadShot(gameState));
                }
            }
        }

        private bool WasKillHeadShot(GameState gameState)
        {
            return gameState.Player.State.RoundKillHS > PreviousGameState.Player.State.RoundKillHS;
        }

        private void HandleRoundWon(GameState gameState)
        {
            if (gameState.Map.TeamCT.Score > PreviousGameState.Map.TeamCT.Score)
            {
                onTeamWonRound?.Invoke(Side.CounterTerrorist, gameState.Map.TeamCT.Score);
            }
            else if (gameState.Map.TeamT.Score > PreviousGameState.Map.TeamT.Score)
            {
                onTeamWonRound?.Invoke(Side.Terrorist, gameState.Map.TeamT.Score);
            }
        }

        private void HandlePlayerNameChange(GameState gameState)
        {
            if (player == null || DidPlayerNameChange(gameState))
            {
                player = new Player(gameState.Player.Name, gameState.Player.SteamID);
                onPlayerNameChanged?.Invoke(player.Name);
            }
        }

        private bool DidPlayerNameChange(GameState currentGameState)
        {
            return player.Name != currentGameState.Player.Name;
        }

        private bool IsPlayer(GameState currentGameState)
        {
            return player == null || player.SteamID == currentGameState.Player.SteamID;
        }

        public GameState PreviousGameState
        {
            get; set;
        }

        public static CSGOEventListener Instance
        {
            get
            {
                if (instance == null)
                    instance = new CSGOEventListener();

                return instance;
            }
        }
    }

    
}
