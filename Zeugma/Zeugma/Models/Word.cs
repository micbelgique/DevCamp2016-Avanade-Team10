using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeugma.Models
{
    public class Word
    {
        public Word(int order, string value)
        {
            Order = order;
            Value = value;
            Formation = false;
            Visible = false;
        }

        // must be unique in each sentence
        public int Order { get; set; }
        public string Value { get; set; }
        public bool Formation { get; set; }
        public bool Visible { get; set; }
    }
}
