using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using SpellCheckerLib;

namespace SpellCheckerApp
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Welcome!");

			//var filter = new Kata5BloomFilter();
			var filter = new Kata5BloomFilter(1024, 1024 * 1024);
			string filePath = "wordlist_few.txt";
			Console.WriteLine();
			Console.WriteLine($"Loading SpellChecker Dictionary Words from {filePath}...");

			// Adding words from file 
			AddWordsToBloomFilter(filter, filePath);

			Console.WriteLine();
			Console.WriteLine("Verifying if words exist in SpellChecker.");
			VerifyWordsExist(filter, "verify.txt");

			// Generate Words
			Console.WriteLine();
			Console.WriteLine("Generating Words.");
			List<string> generatedWords = WordGenerator.GenerateWords(6);
			foreach (var word in generatedWords)
			{
				CheckWordExists(filter, word);
			}

			Console.WriteLine();
			Console.WriteLine("Press any key to continue...");
			Console.ReadKey();

		}

		
		/// <summary>
		/// Load words from file and add those words to Bloom Filter.
		/// </summary>
		/// <param name="filter">Bloom Filter object</param>
		/// <param name="filePath">Path of input file with words to add</param>
		private static void AddWordsToBloomFilter(IBloomFilter filter, string filePath)
		{
			// Invalid parameters or file does not exist
			if ((filter == null) || !File.Exists(filePath))
				return;

			using (FileStream fileS = File.OpenRead(filePath))
			{
				using (BufferedStream buffS = new BufferedStream(fileS))
				{
					using (StreamReader strR = new StreamReader(buffS))
					{
						string word;
						while ((word = strR.ReadLine()) != null)
						{
							filter.AddWord(word);
						}
					}
				}
			}

		}

		/// <summary>
		/// Load words to be verified from file and verify if words exists in Bloom Filter. 
		/// </summary>
		/// <param name="filter">Bloom Filter object</param>
		/// <param name="filePath">Path of file with words to verify</param>
		private static void VerifyWordsExist(IBloomFilter filter, string filePath)
		{
			// Invalid parameters or file does not exist
			if ((filter == null) || !File.Exists(filePath))
				return;

			List<string> wordsToCheck = new List<string>();

			using (FileStream fileS = File.OpenRead(filePath))
			{
				using (BufferedStream buffS = new BufferedStream(fileS))
				{
					using (StreamReader strR = new StreamReader(buffS))
					{
						string word;
						while ((word = strR.ReadLine()) != null)
						{
							wordsToCheck.Add(word);
						}
					}
				}
			}

			foreach (var word in wordsToCheck)
			{
				CheckWordExists(filter, word);
			}

		}

		/// <summary>
		/// Check if the given word exists in Bloom Filter.
		/// </summary>
		/// <param name="filter">Bloom Filter object</param>
		/// <param name="word">Word</param>
		/// <param name="printToScreen">Flag to print result to Console window with default value of true</param>
		/// <returns></returns>
		private static bool CheckWordExists(IBloomFilter filter, string word, bool printToScreen = true)
		{
			// Invalid parameters
			if (filter == null)
				return false;

			bool exists = filter.CheckWordExists(word);

			if (printToScreen)
			{
				if (exists)
					Console.WriteLine($"  Found word: {word}");
				else
					Console.WriteLine($"  Did not find word: {word}");
			}

			return exists;
		}


	}
}

