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
        // must be unique in each sentence
        public int Order { get; set; }
        public bool Formation { get; set; }
        public bool Visible { get; set; }
    }
}
