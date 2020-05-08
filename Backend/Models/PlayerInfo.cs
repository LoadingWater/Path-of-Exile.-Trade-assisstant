using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class PlayerInfo
    {
        public PlayerInfo(string league, string accountName)
        {
            League = league;
            AccountName = accountName;
        }
        public string League { get; set; }
        public string AccountName { get; set; }
    }
}
