using System;
using System.Collections.Generic;
using System.Text;

namespace SpellCheckerLib
{
	public class WordGenerator
	{
		/// <summary>
		/// Generate a particular number of words with a given word size.
		/// </summary>
		/// <param name="numberOfWords">Number of words</param>
		/// <param name="wordSize">Word size with default value of 5</param>
		/// <returns>Collection of generated words</returns>
		public static List<string> GenerateWords(int numberOfWords, int wordSize = 5)
		{
			List<string> words = new List<string>();

			numberOfWords = (numberOfWords < 0) ? (numberOfWords * -1) : numberOfWords;
			wordSize = (wordSize < 0) ? (wordSize * -1) : wordSize;

			char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
			Random rand = new Random();

			for (int wordIndex = 0; wordIndex < numberOfWords; wordIndex++)
			{
				string word = string.Empty;
				for (int charIndex = 0; charIndex < wordSize; charIndex++)
				{
					word += letters[rand.Next(0, wordSize - 1)];
				}
				words.Add(word);
			}

			return words;
		}

	}
}
