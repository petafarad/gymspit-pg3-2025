using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    internal class HandEvaluation
    {
        public HandRank Rank { get; set; }
        public List<int> CardValues { get; set; } = new List<int>();
    }
}
