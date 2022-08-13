using Newtonsoft.Json;

namespace Near.Models.Tokens.Players.Goalie
{
    public class Goalie : Player
    {
        [JsonIgnore]
        public GoalieStats Stats { get; set; }
        public string goalie_number { get; set; }
    }
}