namespace Uncas.Core.Web
{
    using System;

    /// <summary>
    /// Handles text for web.
    /// </summary>
    public static class TextHandler
    {
        /// <summary>
        /// Inserts line breaks.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The string with extra line breaks.</returns>
        public static string InsertLineBreaks(string text)
        {
            return InsertLineBreaks(text, 1); // The default...
        }

        /// <summary>
        /// Inserts line breaks.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="numberOfLineBreaks">The number of line breaks.</param>
        /// <returns>The text with line breaks.</returns>
        public static string InsertLineBreaks(
            string text, 
            int numberOfLineBreaks)
        {
            string outString = string.Empty;
            if (string.IsNullOrEmpty(text))
            {
                return outString;
            }

            numberOfLineBreaks = Math.Max(1, numberOfLineBreaks);
            foreach (char karakter in text.ToCharArray())
            {
                if (char.ConvertToUtf32(karakter.ToString(), 0) == 13)
                {
                    for (int i = 0; i < numberOfLineBreaks; i++)
                    {
                        outString += "<br/>";
                    }
                }
                else
                {
                    outString += karakter.ToString();
                }
            }
 
            return outString;
        }
    }
}