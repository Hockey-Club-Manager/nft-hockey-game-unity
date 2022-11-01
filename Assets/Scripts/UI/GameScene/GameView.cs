﻿using System.Threading.Tasks;
using Near.Models.Game;
using UnityEngine;

namespace UI.GameScene
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] private EventMessages eventMessages;

        private int _gameId;
        private int _numberOfRenderedEvents;
        private GameData _gameData;

        private async void Awake()
        {
            var user = await Near.GameContract.ContractMethods.Views.GetUserInGame();
            if (user != null && user.games[0].winner_index == null)
            {
                _gameId = user.games[0].id;

                Task.Run(GenerateEvent);
                UpdateGameData();
            }
            else
            {
                Debug.Log("Error");
            }
        }

        private async void GenerateEvent()
        {
            while (_gameData?.winner_index == null)
            {
                Debug.Log("Generate event");
                Near.GameContract.ContractMethods.Actions.GenerateEvent(_gameId);

                await Task.Delay(1500);
            }
        }

        private async void UpdateGameData()
        {
            var filter = new GameDataFilter()
            {
                id = _gameId
            };

            do
            {
                Debug.Log("Update indexer");
                _gameData = await Near.GameContract.ContractMethods.Views.GetGame(filter);
                RenderEvents(_gameData);
            } while (_gameData.winner_index == null);
        }

        private void RenderEvents(GameData gameData)
        {
            if (eventMessages.enabled)
            {
                eventMessages.RenderMessages(gameData.events);
            }
        }
    }
}