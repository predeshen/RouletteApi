using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteApi.Models
{
    public class Spin
    {
        [Key]
        public int Id { get; set; }
        public string WinningNumber { get; set; }
        public DateTime SpunAt { get; set; }
    }
}
