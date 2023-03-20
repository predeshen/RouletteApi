using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteApi.Models
{
    public class Bet
    {
        [Key]
        public int Id { get; set; }
        public string PlayerName { get; set; }
        public int BetAmount { get; set; }
        public string BetType { get; set; }
        public int BetValue { get; set; }
        public float Amount { get; set; }
        public DateTime PlacedAt { get; set; }
    }
}
