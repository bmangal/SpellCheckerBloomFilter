using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SpellCheckerLib
{
	/// <summary>
	/// Custom Hash Algorithm to use .Net Framework's default Hash generation.
	/// </summary>
	public class CustomHashAlgorithm : HashAlgorithm
	{

		#region Constructors

		private CustomHashAlgorithm()
		{

		}
		public new static CustomHashAlgorithm Create()
		{
			return new CustomHashAlgorithm();
		}

		#endregion

		#region Overridden Abstract Methods

		public override void Initialize()
		{

		}

		protected override void HashCore(byte[] array, int ibStart, int cbSize)
		{

		}

		protected override byte[] HashFinal()
		{
			return new byte[] { };
		}

		#endregion

	}
}