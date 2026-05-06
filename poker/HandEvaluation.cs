using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    public class HandEvaluation
    {
        public Card.HandRank Rank { get; set; }
        public List<int> CardValues { get; set; }
    }
}
