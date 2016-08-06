using System;
using System.Collections.Generic;
using System.Linq;
using Zeugma.Models;

namespace Zeugma.Helpers
{
    public static class Algorythm
    {
        // find word by order in the sentence
        public static Predicate<Word> ByOrder(int order)
        {
            return delegate (Word word)
            {
                return word.Order == order;
            };
        }

        // get a sentence and parameters, return a sentence with calculated
        public static Sentence MakeSentence(Sentence sentence, int peoplePresent, int peopleParticipating)
        {

            // minimum number of peoples needed in sight to show full phrase
            var peopleMin = 10;
            // "maximum" minimum number of peoples who need to look at the camera to show the words in the right place
            var peopleMax = 20;

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

            // calculate the % of right placed words among the created words
            if (peopleMax != 0)
            {
                percentPhrase = (peopleParticipating / (peopleMax + (peoplePresent - peopleParticipating))) * 100;
            }
            else
            {
                percentPhrase = 100;
            }

            // calculate number of words to construct

            var numWordsCreated = (int)Math.Ceiling((double)(sentence.Words.Count * percentWords) / 100);
            var numWordsPhrase = (int)Math.Ceiling((double)(sentence.Words.Count * percentPhrase) / 100);

            // get the number of words to construct or show
            var actualNumberCreated = (from retrieve in sentence.Words
                                       where retrieve.Formation == true
                                       select retrieve).Count();

            var actualNumberPhrase = (from retrieve in sentence.Words
                                      where retrieve.Visible == true
                                       select retrieve).Count();

            // if formated words number changed
            if (numWordsCreated != actualNumberCreated)
            {
                // while too many formated words
                while (numWordsCreated > actualNumberCreated)
                {
                    var tempWords = (from retrieve in sentence.Words
                                     where retrieve.Formation == true
                                     select retrieve).ToList();

                    Random random = new Random();
                    int randomIndex = random.Next(tempWords.Count);

                    // find a random formed word and destroy it
                    sentence.Words.Find(ByOrder(tempWords[randomIndex].Order)).Formation = false;
                }
                // while too few formated words
                while (numWordsCreated < actualNumberCreated)
                {
                    var tempWords = (from retrieve in sentence.Words
                                     where retrieve.Formation == false
                                     select retrieve).ToList();

                    Random random = new Random();
                    int randomIndex = random.Next(tempWords.Count);

                    // find a random non formed word and form it
                    sentence.Words.Find(ByOrder(tempWords[randomIndex].Order)).Formation = true;
                }
            }

            // if words in the right place number changed
            if (numWordsPhrase != actualNumberPhrase)
            {
                // while too many visible words, hide one random word
                while (numWordsPhrase > actualNumberPhrase)
                {
                    var tempWords = (from retrieve in sentence.Words
                                     where retrieve.Visible == true
                                     select retrieve).ToList();

                    Random random = new Random();
                    int randomIndex = random.Next(tempWords.Count);

                    sentence.Words.Find(ByOrder(tempWords[randomIndex].Order)).Visible = false;
                }
                // while too few visible words, show one random word
                while (numWordsPhrase < actualNumberPhrase)
                {
                    var tempWords = (from retrieve in sentence.Words
                                     where retrieve.Visible == false
                                     select retrieve).ToList();

                    Random random = new Random();
                    int randomIndex = random.Next(tempWords.Count);

                    sentence.Words.Find(ByOrder(tempWords[randomIndex].Order)).Visible = true;
                }
            }

            /** TO DO : SEND SENTENCE TO VIEW **/

            return sentence;
        }
    }
}
