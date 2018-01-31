namespace Tangle.Net.Source.Entity
{
  using System;
  using System.Linq;
  using System.Security.Cryptography;

  using Tangle.Net.Source.Utils;

    public class Seed : TryteString
    {
        #region Constants

        /// <summary>
        /// The length.
        /// </summary>
        public const int Length = 81;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Seed"/> class.
        /// </summary>
        /// <param name="seed">
        /// The seed.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown if the seed has an incorrect length
        /// </exception>
        public Seed(string seed)
          : base(seed)
        {
            if (seed.Length != Length)
            {
                throw new ArgumentException("Seed must be of length " + Length);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Creates a cryptographically random seed
        /// </summary>
        /// <returns>
        /// The <see cref="Seed"/>.
        /// </returns>
        public static Seed Random()
        {
            char[] seedChars;

            using (var rnd = new RNGCryptoServiceProvider())
            {
                byte[] cryptoBytes = new byte[Length];
                rnd.GetBytes(cryptoBytes);
                seedChars = cryptoBytes.Select(x => AsciiToTrytes.TryteAlphabet[x % AsciiToTrytes.TryteAlphabet.Length]).ToArray();
            }

            return new Seed(new string(seedChars));
        }

        #endregion
    }
}