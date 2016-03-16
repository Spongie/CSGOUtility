using CSGOUtility.Models;
using CSGSI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private static CSGOEventListener instance;
        private GameStateListener listener;
        private Player player;
        
        public void Start()
        {
            listener = new GameStateListener(3000);
            listener.NewGameState += Listener_NewGameState;
            listener.Start();
        }

        private void Listener_NewGameState(GameState gameState)
        {
            if (!IsPlayer(gameState))
                return;

            HandlePlayerNameChange(gameState);

            HandleInMatch(gameState);
        }

        private void HandleInMatch(GameState gameState)
        {
            if (gameState.Map.Phase == CSGSI.Nodes.MapPhase.Live)
            {
                HandleRoundWon(gameState);

                if (gameState.Player.MatchStats.Kills > gameState.Previously.Player.MatchStats.Kills)
                    onPlayerKill?.Invoke(gameState.Player.Weapons.ActiveWeapon.Name, WasKillHeadShot(gameState));
            }
        }

        private static bool WasKillHeadShot(GameState gameState)
        {
            return gameState.Player.State.RoundKillHS > gameState.Previously.Player.State.RoundKillHS;
        }

        private void HandleRoundWon(GameState gameState)
        {
            if (gameState.Map.TeamCT.Score > gameState.Previously.Map.TeamCT.Score)
                onTeamWonRound?.Invoke(Side.CounterTerrorist, gameState.Map.TeamCT.Score);
            else if (gameState.Map.TeamT.Score > gameState.Previously.Map.TeamT.Score)
                onTeamWonRound?.Invoke(Side.Terrorist, gameState.Map.TeamT.Score);
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
