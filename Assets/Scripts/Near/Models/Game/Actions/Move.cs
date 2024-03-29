namespace Near.Models.Game.Actions {
    public class Move : Action {
        public string account_id { get; set; }
        public int zone_number { get; set; }
        public int player_number { get; set; }
        public string player_position { get; set; }
        
        public override string GetMessage(string accountId)
        {
            if (accountId == account_id)
            {
                return ColorizeMessage(UserColor);
            }

            return ColorizeMessage(OpponentColor);
        }

        private string ColorizeMessage(string color)
        {
            if (zone_number == 2)
            {
                return $"{color}{player_number} move out of the zone";
            }
            
            return $"{color}{player_number} move into the zone";
        }
    }
}