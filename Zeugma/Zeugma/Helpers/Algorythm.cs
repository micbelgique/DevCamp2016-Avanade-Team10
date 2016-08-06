using System;
using System.Diagnostics;
using System.Linq;
using Zeugma.Models;

namespace Zeugma.Helpers
{
    public static class Algorythm
    {
        private static Random rand = new Random();

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
            var peopleMin = 2;
            // "maximum" minimum number of peoples who need to look at the camera to show the words in the right place
            var peopleMax = 3;

            // % of words constructed
            var percentWords = 0;

            // % of words in right place
            var percentPhrase = 0;

            // calculate the % of words created
            if (peoplePresent < peopleMin && peopleMin != 0)
            {
                percentWords = (peoplePresent * 100) / peopleMin;
            }
            else
            {
                percentWords = 100;
            }

            // calculate the % of right placed words among the created words
            if (peopleMax != 0)
            {
                percentPhrase = (peopleParticipating * 100) / (peopleMax + (peoplePresent - peopleParticipating));
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
                while (numWordsCreated < actualNumberCreated)
                {
                    var tempWords = (from retrieve in sentence.Words
                                     where retrieve.Formation == true
                                     select retrieve).ToList();
                    
                    int randomIndex = rand.Next(tempWords.Count);
                    Debug.WriteLine(randomIndex);

                    // find a random formed word and destroy it
                    sentence.Words.Find(ByOrder(tempWords[randomIndex].Order)).Formation = false;
                    actualNumberCreated--;
                }
                // while too few formated words
                while (numWordsCreated > actualNumberCreated)
                {
                    var tempWords = (from retrieve in sentence.Words
                                     where retrieve.Formation == false
                                     select retrieve).ToList();

                    var randomIndex = rand.Next(tempWords.Count);
                    Debug.WriteLine(randomIndex);

                    // find a random non formed word and form it
                    sentence.Words.Find(ByOrder(tempWords[randomIndex].Order)).Formation = true;
                    actualNumberCreated++;
                }
            }

            // if words in the right place number changed
            if (numWordsPhrase != actualNumberPhrase)
            {
                // while too many visible words, hide one random word
                while (numWordsPhrase < actualNumberPhrase)
                {
                    var tempWords = (from retrieve in sentence.Words
                                     where retrieve.Visible == true
                                     select retrieve).ToList();
                    
                    int randomIndex = rand.Next(tempWords.Count);
                    Debug.WriteLine(randomIndex);

                    sentence.Words.Find(ByOrder(tempWords[randomIndex].Order)).Visible = false;
                    actualNumberPhrase--;
                }
                // while too few visible words, show one random word
                while (numWordsPhrase > actualNumberPhrase)
                {
                    var tempWords = (from retrieve in sentence.Words
                                     where retrieve.Visible == false
                                     where retrieve.Formation == true
                                     select retrieve).ToList();
                    
                    int randomIndex = rand.Next(tempWords.Count);
                    Debug.WriteLine(randomIndex);

                    sentence.Words.Find(ByOrder(tempWords[randomIndex].Order)).Visible = true;
                    actualNumberPhrase++;
                }
            }

            return sentence;
        }
    }
}
