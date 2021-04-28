using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChiaPool.Util
{
    public static class Bech32MEncoding
    {
        private const string Charset = "qpzry9x8gf2tvdw0s3jn54khce6mua7l";
        private const int M = 0x2BC830A3;
        private static int[] Generator = new int[] { 0x3B6A57B2, 0x26508E6D, 0x1EA119FA, 0x3D4233DD, 0x2A1462B3 };


        private static int GetUnicodeNumber(char character)
            => (int)(character);

        /// <summary>
        /// Calculates the Bech32 Checksum
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        private static int PolyMod(List<int> values)
        {
            int chk = 1;

            foreach (int value in values)
            {
                int top = chk >> 25;
                chk = ((chk & 0x1FFFFFF) << 5) ^ value;

                for (int i = 0; i < 5; i++)
                {
                    chk ^= (((top >> i) & 1) != 0)
                        ? Generator[i]
                        : 0;
                }
            }
            return chk;
        }

        /// <summary>
        /// Expand the HRP into values for checksum computation
        /// </summary>
        /// <param name="hrp"></param>
        /// <returns></returns>
        private static List<int> HrpExpand(string hrp)
        {
            var lst = new List<int>();

            foreach (char character in hrp)
            {
                lst.Add(GetUnicodeNumber(character) >> 5);
            }
            lst.Add(0);
            foreach (var character in hrp)
            {
                lst.Add(GetUnicodeNumber(character) & 31);
            }

            return lst;
        }

        private static bool VerifyChecksum(string hrp, List<int> data)
        {
            var values = HrpExpand(hrp);
            values.AddRange(data);

            return PolyMod(values) == M;
        }

        private static List<int> CreateChecksum(string hrp, List<int> data)
        {
            var values = HrpExpand(hrp);
            values.AddRange(data);
            for (int i = 0; i < 6; i++)
            {
                values.Add(0);
            }

            var polymod = PolyMod(values) ^ M;

            var lst = new List<int>();

            for (int i = 0; i < 6; i++)
            {
                lst.Add((polymod >> (5 * (5 - i))) & 31);
            }

            return lst;
        }

        /// <summary>
        /// Compute a Bech32 string given HRP and data values
        /// </summary>
        /// <param name="hrp"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static string Encode(string hrp, List<int> data)
        {
            var combined = new List<int>();
            combined.AddRange(data);
            combined.AddRange(CreateChecksum(hrp, data));

            var sb = new StringBuilder();

            sb.Append($"{hrp}1");

            foreach (int value in combined)
            {
                sb.Append(Charset[value]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Validate a Bech32 string, and determine HRP and data
        /// </summary>
        /// <param name="bech"></param>
        /// <returns></returns>
        private static (string, List<int>) Decode(string bech)
        {
            if (bech.ToLower() != bech && bech.ToUpper() != bech)
            {
                return (null, null);
            }
            foreach (char character in bech)
            {
                int unicodeValue = GetUnicodeNumber(character);
                if (unicodeValue < 33 || unicodeValue > 126)
                {
                    return (null, null);
                }
            }

            bech = bech.ToLower();
            int lastOnePosition = bech.LastIndexOf("1");

            if (lastOnePosition < 1 || lastOnePosition + 7 > bech.Length || bech.Length > 90)
            {
                return (null, null);
            }
            if (bech[(lastOnePosition + 1)..].Any(x => !Charset.Contains(x)))
            {
                return (null, null);
            }

            string hrp = bech.Substring(0, lastOnePosition);
            List<int> data = bech[(lastOnePosition + 1)..].Select(x => Charset.IndexOf(x)).ToList();

            return !VerifyChecksum(hrp, data)
                ? (null, null)
                : (hrp, data.SkipLast(6).ToList());
        }

        /// <summary>
        /// General power-of-2 base conversion
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fromBits"></param>
        /// <param name="toBits"></param>
        /// <param name="pad"></param>
        /// <returns></returns>
        private static List<int> ConvertBits(List<int> data, int fromBits, int toBits, bool pad = true)
        {
            int acc = 0;
            int bits = 0;
            List<int> ret = new List<int>();
            int maxv = (1 << toBits) - 1;
            int max_acc = (1 << (fromBits + toBits - 1)) - 1;

            foreach (int value in data)
            {
                if (value < 0 || (value >> fromBits) != 0)
                {
                    throw new ArgumentException("Invalid Value");
                }

                acc = ((acc << fromBits) | value) & max_acc;
                bits += fromBits;

                while (bits >= toBits)
                {
                    bits -= toBits;
                    ret.Add((acc >> bits) & maxv);
                }
            }
            if (pad)
            {
                if (bits != 0)
                {
                    ret.Add((acc << (toBits - bits)) & maxv);
                }
            }
            else if (bits >= fromBits || ((acc << (toBits - bits)) & maxv) != 0)
            {
                throw new ArgumentException("Invalid Bits");
            }

            return ret;
        }

        private static byte[] GetPuzzleHashBytes(string puzzleHashHex)
        {
            puzzleHashHex = puzzleHashHex.Replace("0x", "");
            return Enumerable.Range(0, puzzleHashHex.Length)
                 .Where(x => x % 2 == 0)
                 .Select(x => Convert.ToByte(puzzleHashHex.Substring(x, 2), 16))
                 .ToArray();
        }
        private static string GetPuzzleHashString(byte[] puzzleHashBytes)
        {
            var sb = new StringBuilder();
            sb.Append("0x");

            foreach (byte b in puzzleHashBytes)
            {
                sb.Append(Convert.ToString(b, 16));
            }

            return sb.ToString();
        }

        public static string EncodePuzzleHash(string puzzleHash, string prefix)
        {
            if (string.IsNullOrWhiteSpace(puzzleHash))
            {
                throw new ArgumentNullException(nameof(puzzleHash));
            }
            var puzzleHashBytes = GetPuzzleHashBytes(puzzleHash);

            return puzzleHashBytes.Length != 32
                ? throw new ArgumentException("Invalid puzzle hash length")
                : Encode(prefix, ConvertBits(puzzleHashBytes.Select(x => (int)x).ToList(), 8, 5));
        }

        public static string DecodePuzzleHash(string address)
        {
            var (hrpgot, data) = Decode(address);

            if (data == null)
            {
                throw new ArgumentException("Invalid Address");
            }

            var decoded = ConvertBits(data, 5, 8, false);
            var puzzleHashBytes = decoded.Select(x => (byte)x).ToArray();
            return GetPuzzleHashString(puzzleHashBytes);
        }
    }
}
