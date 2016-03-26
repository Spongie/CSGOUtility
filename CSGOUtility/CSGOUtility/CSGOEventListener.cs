using CSGOUtility.Models;
using CSGSI;
using System;
using System.Threading.Tasks;

namespace CSGOUtility
{
    public delegate void PlayerNameChanged(string newName);
    public delegate void OnTeamWonRound(Side side, int newRounds);
    public delegate void OnPlayerKill(string withWeapon, bool headShot, int round);
    public delegate Task OnMatchEnding(MatchResult result);

    public class CSGOEventListener
    {
        public event PlayerNameChanged onPlayerNameChanged;
        public event OnTeamWonRound onTeamWonRound;
        public event OnPlayerKill onPlayerKill;
        public event EventHandler onMatchStarted;
        public event OnMatchEnding onMatchEnded;
        public event EventHandler onPlayerDied;

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
            if (listener != null && listener.Running)
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
                onMatchStarted?.Invoke(this, new EventArgs());
            }
            else if (gameState.Map.Phase == CSGSI.Nodes.MapPhase.Live)
            {
                HandleRoundWon(gameState);

                if (gameState.Player.State.Health <= 0 && PreviousGameState.Player.State.Health > 0)
                {
                    onPlayerDied?.Invoke(this, new EventArgs());
                }
                if (gameState.Player.MatchStats.Kills > PreviousGameState.Player.MatchStats.Kills && gameState.Player.MatchStats.Kills > 0)
                {
                    onPlayerKill?.Invoke(gameState.Player.Weapons.ActiveWeapon.Name, WasKillHeadShot(gameState), gameState.Map.Round + 1);
                }
            }
            else if (gameState.Map.Phase == CSGSI.Nodes.MapPhase.GameOver && PreviousGameState.Map.Phase == CSGSI.Nodes.MapPhase.Live)
            {
                HandleGameEnded(gameState);
            }
            else if (gameState.Map.Phase == CSGSI.Nodes.MapPhase.GameOver && gameState.Previously.Map.Phase == CSGSI.Nodes.MapPhase.Live)
            {
                onMatchEnded?.Invoke(MatchResult.Draw);
            }
        }

        private void HandleGameEnded(GameState gameState)
        {
            MatchResult result;
            if (gameState.Map.TeamCT.Score == gameState.Map.TeamT.Score)
                result = MatchResult.Draw;
            else if (gameState.Map.TeamCT.Score > gameState.Map.TeamT.Score)
            {
                result = gameState.Player.Team == CSGSI.Nodes.PlayerTeam.CT ? MatchResult.Victory : MatchResult.Loose;
            }
            else
                result = gameState.Player.Team == CSGSI.Nodes.PlayerTeam.T ? MatchResult.Victory : MatchResult.Loose;

            onMatchEnded?.Invoke(result);
        }

        private bool WasKillHeadShot(GameState gameState)
        {
            return gameState.Player.State.RoundKillHS > PreviousGameState.Player.State.RoundKillHS;
        }

        private void HandleRoundWon(GameState gameState)
        {
            if (gameState.Map.TeamCT.Score > PreviousGameState.Map.TeamCT.Score && PreviousGameState.Map.TeamCT.Score > -1)
            {
                onTeamWonRound?.Invoke(Side.CounterTerrorist, gameState.Map.TeamCT.Score);
            }
            else if (gameState.Map.TeamT.Score > PreviousGameState.Map.TeamT.Score && PreviousGameState.Map.TeamT.Score > -1)
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
            return player == null || player.SteamId == currentGameState.Player.SteamID;
        }

        public GameState PreviousGameState
        {
            get; set;
        }

        public static CSGOEventListener Instance => instance ?? (instance = new CSGOEventListener());
    }
}
