using Bogus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SK.TextGenerator
{
    //https://github.com/dochoffiday/Lorem.NET/blob/master/Lorem.NET/Lorem.cs
    public partial class Lorem
    {
        public static bool Chance(int successes, int attempts)
        {
            var number = Number(1, attempts);

            return number <= successes;
        }

        public static string StringId()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", ""); // Remove period.
            return path.ToLower();
        }

        public static string NumberId() => Number(1, 1000000).ToString();

        public static T Random<T>(params T[] items)
        {
            var index = RandomHelper.Instance.Next(items.Length);

            return items[index];
        }

        public static string YesNo()
            => RandomHelper.Instance.Next(1) == 0 ? "Yes" : "No";

        public static TEnum Enum<TEnum>() where TEnum : struct, IConvertible
        {
            if (typeof(TEnum).IsEnum)
            {
                var v = System.Enum.GetValues(typeof(TEnum));
                return (TEnum)v.GetValue(RandomHelper.Instance.Next(v.Length));
            }
            else
                throw new ArgumentException("Generic type must be an enum.");
        }

        /* http://stackoverflow.com/a/6651661/234132 */

        public static long Number(long min, long max)
        {
            var buf = new byte[8];
            RandomHelper.Instance.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);

            return Math.Abs(longRand % ((max + 1) - min)) + min;
        }

        public static string PhoneNumber()
            => $"+{Number(10, 99)}-0{Number(100, 999)}-{Number(100, 999)}";

        #region DateTime

        public static DateTime DateTime(int startYear = 1950, int startMonth = 1, int startDay = 1)
            => DateTime(new System.DateTime(startYear, startMonth, startDay), System.DateTime.Now);

        public static DateTime DateTime(DateTime min)
            => DateTime(min, System.DateTime.Now);

        /* http://stackoverflow.com/a/1483677/234132 */

        public static DateTime DateTime(DateTime min, DateTime max)
        {
            TimeSpan timeSpan = max - min;
            TimeSpan newSpan = new TimeSpan(0, RandomHelper.Instance.Next(0, (int)timeSpan.TotalMinutes), 0);

            return min + newSpan;
        }

        #endregion DateTime

        #region Text

        public static string Email()
            => string.Format("{0}@{1}.com", Words(1, false), Words(1, false));

        public static string Words(int wordCount, bool uppercaseFirstLetter = true, bool includePunctuation = false) => Words(wordCount, wordCount, uppercaseFirstLetter, includePunctuation);

        public static string Words(int wordCountMin, int wordCountMax, bool uppercaseFirstLetter = true, bool includePunctuation = false)
        {
            var source = string.Join(" ", Source.WordList(includePunctuation).Take(RandomHelper.Instance.Next(wordCountMin, wordCountMax)));

            if (uppercaseFirstLetter)
                source = source.UppercaseFirst();

            return source;
        }

        public static string Names(int count, int? maxCount = null)
            => string.Join(" ", Source.Rearrange(Source.Names).Take(RandomHelper.Instance.Next(count, maxCount ?? count)));

        public static string Sentence(int wordCount)
            => Sentence(wordCount, wordCount);

        public static string Sentence(int wordCountMin, int wordCountMax)
            => string.Format("{0}.", Words(wordCountMin, wordCountMax, true, true)).Replace(",.", ".").Remove("..");

        public static string Paragraph(int wordCount, int sentenceCount)
            => Paragraph(wordCount, wordCount, sentenceCount, sentenceCount);

        public static string Paragraph(int wordCountMin, int wordCountMax, int sentenceCount)
            => Paragraph(wordCountMin, wordCountMax, sentenceCount, sentenceCount);

        public static string Paragraph(int wordCountMin, int wordCountMax, int sentenceCountMin, int sentenceCountMax)
        {
            var source = string.Join(" ", Enumerable.Range(0, RandomHelper.Instance.Next(sentenceCountMin, sentenceCountMax)).Select(x => Sentence(wordCountMin, wordCountMax)));

            //remove traililng space
            return source.Remove(source.Length - 1);
        }

        public static IEnumerable<string> Paragraphs(int wordCount, int sentenceCount, int paragraphCount)
            => Paragraphs(wordCount, wordCount, sentenceCount, sentenceCount, paragraphCount, paragraphCount);

        public static IEnumerable<string> Paragraphs(int wordCountMin, int wordCountMax, int sentenceCount, int paragraphCount)
            => Paragraphs(wordCountMin, wordCountMax, sentenceCount, sentenceCount, paragraphCount, paragraphCount);

        public static IEnumerable<string> Paragraphs(int wordCountMin, int wordCountMax, int sentenceCountMin, int sentenceCountMax, int paragraphCount)
            => Paragraphs(wordCountMin, wordCountMax, sentenceCountMin, sentenceCountMax, paragraphCount, paragraphCount);

        public static IEnumerable<string> Paragraphs(int wordCountMin, int wordCountMax, int sentenceCountMin, int sentenceCountMax, int paragraphCountMin, int paragraphCountMax)
            => Enumerable.Range(0, RandomHelper.Instance.Next(paragraphCountMin, paragraphCountMax)).Select(p => Paragraph(wordCountMin, wordCountMax, sentenceCountMin, sentenceCountMax)).ToArray();

        static Faker _faker = new Faker("en");

        public static Faker Faker => _faker;

        public static string Address(string city) =>
            _faker.Address.StreetAddress() + " - " + _faker.Address.SecondaryAddress() + ", " + city;

        /// <summary>
        ///
        /// </summary>
        /// <param name="orCategories"></param>
        /// <param name="filter">p | g | red| green | blue</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="randomize"></param>
        /// <returns></returns>
        public static string Image(string orCategories = null, string andCategories = null, string filter = null, int width = 640, int height = 480, bool randomize = true)
        {
            if (string.IsNullOrWhiteSpace(orCategories + andCategories))
                return string.Empty;

            if (string.IsNullOrWhiteSpace(filter))
                filter = "/";
            else
                filter += "/";

            var randomToken = randomize ? $"?random={RandomHelper.Instance.Next()}" : "";

            if (!string.IsNullOrWhiteSpace(orCategories))
                return $"https://loremflickr.com/{filter}{width}/{height}/{orCategories}{randomToken}";
            else if (!string.IsNullOrWhiteSpace(andCategories))
                return $"https://loremflickr.com/{filter}{width}/{height}/{andCategories}/all{randomToken}";

            return string.Empty;
        }

        #endregion Text

        #region Color

        /* http://stackoverflow.com/a/1054087/234132 */

        public static string HexNumber(int digits)
        {
            var buffer = new byte[digits / 2];
            RandomHelper.Instance.NextBytes(buffer);
            string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());

            if (digits % 2 == 0)
            {
                return result;
            }

            return result + RandomHelper.Instance.Next(16).ToString("X");
        }

        #endregion Color
    }
}