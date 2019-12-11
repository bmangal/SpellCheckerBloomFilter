using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SpellCheckerLib
{

	public static class HashAlgorithmExtensions
	{
		/// <summary>
		/// Extension method to generate integer hash value of a word using a particular algorithm
		/// </summary>
		/// <param name="algorithm">Hashing Algorithm</param>
		/// <param name="word">Word</param>
		/// <returns>Numeric hash value</returns>
		public static int ComputeHash(this HashAlgorithm algorithm, string word)
		{
			int hashNumber;

			if (algorithm is CustomHashAlgorithm)
			{
				hashNumber = word.GetHashCode();
			}
			else
			{
				// Get Hash Value using individual Hashing Algorithm
				byte[] hashBytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(word));

				// Convert Hash Value from byte[] -> int
				hashNumber = BitConverter.ToInt32(hashBytes);
			}

			return hashNumber;
		}


	}
}
