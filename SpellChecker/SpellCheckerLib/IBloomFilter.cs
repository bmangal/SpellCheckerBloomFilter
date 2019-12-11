using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SpellCheckerLib
{
	public interface IBloomFilter
	{
		bool[] BitArrayByHashIndex { get; }
		List<HashAlgorithm> HashAlgorithms { get; }
		int GetWordHashValue(HashAlgorithm algorithm, string word);
		void AddWord(string word);
		bool CheckWordExists(string word);
	}
}
