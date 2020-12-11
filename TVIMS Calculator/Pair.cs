using System;
using System.Collections.Generic;
using System.Text;

namespace TVIMS_Calculator
{
    class Pair <K, V>
    {
        public K First { get; set; }
        public V Second { get; set; }

        public Pair(K first, V second)
        {
            this.First = first;
            this.Second = second;
        }
    }
}
