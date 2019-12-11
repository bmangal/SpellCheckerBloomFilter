using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace SpellCheckerLib
{
	public class Kata5BloomFilter : IBloomFilter
	{
		private int _maxArraySize; 
		private int _arraySize;
		private bool[] _bitArrayByHashIndex;
		private List<HashAlgorithm> _hashingAlgorithms;

		#region Constructors


		// TODO: Can pass Hashing Algorithms as input to Constructor.
		/// <summary>
		/// Kata5BloomFilter Constructor with default values
		/// </summary>
		/// <param name="arraySize" >Initial array size with default value of 1 KB</param>
		/// <param name="maxArraySize">Maximum array size with default value of 64 KB</param>
		public Kata5BloomFilter(
			int arraySize = 1024 * 8,			// 1 KB
			int maxArraySize = 1024 * 8 * 64	// 64 KB
			)
		{
			this._arraySize = Math.Abs(arraySize);
			this._maxArraySize = Math.Abs(maxArraySize);

			this.Init();
		}

		#endregion

		/// <summary>
		/// Initialize required collections and properties.
		/// </summary>
		private void Init()
		{
			this._bitArrayByHashIndex = new bool[_arraySize];

			this._hashingAlgorithms = new List<HashAlgorithm>()
					{
						CustomHashAlgorithm.Create(), // Custom class using .Net's default GetHashCode method
						MD5.Create(), // 16 bytes (Time to hash 500MB: 1462 ms)
						SHA1.Create(), // 20 bytes (1644 ms)
						SHA256.Create(), // 32 bytes (5618 ms)
						SHA384.Create(), // 48 bytes (3839 ms)
						SHA512.Create(), // 64 bytes (3820 ms)
					};
		}

		#region Properties
		public bool[] BitArrayByHashIndex
		{
			get { return this._bitArrayByHashIndex; }
		}

		public List<HashAlgorithm> HashAlgorithms
		{
			get { return this._hashingAlgorithms; }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Gets Hash Value of a word using a particular algorithm.
		/// </summary>
		/// <param name="algorithm">Hashing Algorithm</param>
		/// <param name="word">Word</param>
		/// <returns>Hash Value</returns>
		public int GetWordHashValue(HashAlgorithm algorithm, string word)
		{
			// Invalid input
			if ((algorithm == null) || string.IsNullOrEmpty(word))
				return -1;

			// TODO: We can check word by trimming white spaces, and converting to lower case to keep it case in-sensitive.
			int hashNumber = algorithm.ComputeHash(word);

			// Ensure +ve Index if in case Hashing Algorithm returns -ve value
			hashNumber = (hashNumber < 0) ? (hashNumber * -1) : hashNumber;

			// Ensuring Hash value never goes beyond maximum array size
			/* FYI: We can get incorrect result in this case. 
			 * hash value is beyond the storage and we are truncating value to store. 
			 * Data may overlap with hash value of another word and give incorrect result about word existing by finding true bit of other word.
			 */
			hashNumber %= _maxArraySize;

			return hashNumber;
		}

		/// <summary>
		/// Add a word's hash value to Bloom Filter.
		/// </summary>
		/// <param name="word">Word</param>
		public void AddWord(string word)
		{
			// Adding only non-empty words
			if (string.IsNullOrEmpty(word))
				return;

			int hashNumber;

			foreach (HashAlgorithm algorithm in this._hashingAlgorithms)
			{
				// Get Word's Hash Value
				hashNumber = this.GetWordHashValue(algorithm, word);

				// Hash value is more than current allocated array size? If so allocate more memory
				if (hashNumber >= _bitArrayByHashIndex.Length)
				{
					// Double array size
					int newArraySize = Math.Max(2 * _bitArrayByHashIndex.Length, hashNumber);

					// Ensuring New Array Size does not go beyond maximum array size
					newArraySize = Math.Max(newArraySize, _maxArraySize);

					// Allocate more storage memory
					Array.Resize<bool>(ref this._bitArrayByHashIndex, newArraySize);
				}

				// Marking that particular HashIndex to true/1
				this._bitArrayByHashIndex[hashNumber] = true;
			}
		}

		/// <summary>
		/// Check if the word exists in Bloom Filter's hash bit array.
		/// </summary>
		/// <param name="word">Word</param>
		/// <returns>Success/Failure flag</returns>
		public bool CheckWordExists(string word)
		{
			// Checking only non-empty words
			if (string.IsNullOrEmpty(word))
				return false;

			// TODO: Instead of True/Pass we can calculate % of chances of word being in spellchecker. 
			// We can measure it by what fraction of Algorithms satisfied the match.
			bool result = true;
			int hashNumber;

			foreach (HashAlgorithm algorithm in this._hashingAlgorithms)
			{
				// Get Word's Hash Value
				hashNumber = this.GetWordHashValue(algorithm, word);

				// ANDing with previous result
				result &= this._bitArrayByHashIndex[hashNumber];

				// Found a mismatch? Break and come out of loop. We do not to check further Hashing Algorithms
				if (!result)
					break;
			}

			return result;
		}

		#endregion

	}
}
