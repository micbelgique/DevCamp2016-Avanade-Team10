﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeugma.Models
{
    public class Sentence
    {
        public List<Word> Words { get; set; }
        public int MinimumPerson { get; set; }
        
    }
}
