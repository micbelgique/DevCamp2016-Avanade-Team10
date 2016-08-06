using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeugma.Models
{
    public class Sentence
    {
        public Sentence(string sentence)
        {
            // split the string into words
            string[] words = sentence.Split(' ');
            int order = 0;
            Words = new List<Word>();

            // add the words to the sentence
            foreach(var str in words)
            {
                Word word = new Word(order, str);
                order ++;

                this.Words.Add(word);
            }
        }

        public List<Word> Words { get; set; }
    }
}
