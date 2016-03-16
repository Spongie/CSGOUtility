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

    public class CSGOEventListener
    {
        public event PlayerNameChanged OnPlayerNameChanged;
        private static CSGOEventListener instance;
        private GameStateListener listener;
        private Player player;
        
        public void Start()
        {
            listener = new GameStateListener(3000);
            listener.NewGameState += Listener_NewGameState;
            listener.Start();
        }

        private void Listener_NewGameState(GameState currentGameState)
        {
            if (!IsPlayer(currentGameState))
                return;

            if (player == null || DidPlayerNameChange(currentGameState))
            {
                player = new Player(currentGameState.Player.Name, currentGameState.Player.SteamID);

                OnPlayerNameChanged?.Invoke(player.Name);
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
