﻿namespace Near.Models.Game
{
    public class Event
    {
        public int ID { get; set; }
    
        public UserInGameInfo user1 { get; set; }
        public UserInGameInfo user2 { get; set; }
        public string player_with_puck { get; set; }
        public string action { get; set; }
        public int zone_number { get; set; }
        private int time { get; set; }
    }
}