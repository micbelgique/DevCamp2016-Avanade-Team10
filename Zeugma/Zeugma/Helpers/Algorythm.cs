using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeugma.Models;

namespace Zeugma.Helpers
{
    public static class Algorythm
    {
        // get a sentence and parameters, return a sentence with calculated
        public static Sentence MakeSentence(Sentence sentence, int peoplePresent, int peopleParticipating)
        {

            // minimum number of peoples needed in sight to show full phrase
            var peopleMin = 3;
            // "maximum" minimum number of peoples who need to look at the camera to show the words in the right place
            var peopleMax = 6;

            // put all the words of the sentence in a words list
            var words = new List<Word>();
            foreach (var word in sentence.Words)
            {
                words.Add(word);
            }

            // % of words constructed
            var percentWords = 0;

            // % of words in right place
            var percentPhrase = 0;

            // calculate the % of words created
            if (peoplePresent < peopleMin && peopleMin != 0)
            {
                percentWords = (peoplePresent / peopleMin) * 100;
            }
            else
            {
                percentWords = 100;
            }

            // calculate the % of right displayed words among the created words
            if (peopleMax != 0)
            {
                percentPhrase = (peopleParticipating / (peopleMax + (peoplePresent - peopleParticipating))) * 100;
            }
            else
            {
                percentPhrase = 100;
            }

            /** TO DO : SEND SENTENCE TO VIEW **/

            // calculate number of words to construct

            var numWordsCreated = (int)Math.Ceiling((double)(words.Count * percentWords) / 100);
            var numWordsPhrase = (int)Math.Ceiling((double)(words.Count * percentPhrase) / 100);

            // select random words for creation



            // while minimum number of people to construct the words is not reached, calculate the words formation

            return null;
        }

        public static void RandomWords(List<Word> words, int probability, int mode)
        {
            var length = words.Count();

            // get random words in the words list and set them to true;

            /*
            if (mode == 0)
            {
                word.Formation = true;
                word.Visible = false;
            }
            else if (mode == 1)
            {
                word.Formation = true;
                word.Visible = true;
            }*/
        }
    }
}
