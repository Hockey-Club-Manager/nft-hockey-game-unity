using System;
using Near.Models.Game.Team;
using Near.Models.Tokens;
using Near.Models.Tokens.Players.FieldPlayer;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Event = Near.Models.Game.Event;

namespace UI.ManageTeam.DragAndDrop
{
    public class PlayerCard : UIPlayer
    {
        // [SerializeField] private Text skating;
        // [SerializeField] private Text shooting;
        // [SerializeField] private Text strength;
        // [SerializeField] private Text iq;
        // [SerializeField] private Text morale;

        protected override void Initialize()
        {
            base.Initialize();
            updateAvatar = false;
        }

        public override void SetData(Token token)
        {
            CardData = token;
            
            setPlayerName(token.title);

            if (!string.IsNullOrEmpty(token.media))
            {
                try
                {
                    StartCoroutine(ImageLoader.LoadImage(_avatar, token.media));
                } catch (ApplicationException) {}
            }

            FieldPlayer fieldPlayer = (FieldPlayer)token;
            playerNumber = fieldPlayer.number;
            playerRole = StringToRole(fieldPlayer.player_role);
            position = StringToPosition(fieldPlayer.native_position);
            statistics = new[]
            {
                int.Parse(fieldPlayer.Stats.Skating.ToString()),
                int.Parse(fieldPlayer.Stats.Shooting.ToString()),
                int.Parse(fieldPlayer.Stats.Strength.ToString()),
                int.Parse(fieldPlayer.Stats.Morale.ToString())
            };
        }
    }
}