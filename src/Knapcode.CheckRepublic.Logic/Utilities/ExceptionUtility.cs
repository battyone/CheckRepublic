using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Knapcode.CheckRepublic.Logic.Utilities
{
    public static class ExceptionUtility
    {
        public static string GetDisplayMessage(Exception exception)
        {
            var stringBuilder = new StringBuilder();

            foreach (var message in GetMessageSequence(exception))
            {
                var isFirstMessage = stringBuilder.Length == 0;
                
                foreach (var line in SplitLines(message))
                {
                    if (!isFirstMessage)
                    {
                        stringBuilder.AppendLine();
                        stringBuilder.Append("  ");
                    }

                    stringBuilder.Append(line);
                }
            }

            return stringBuilder.ToString();
        }

        private static IEnumerable<string> SplitLines(string input)
        {
            using (var stringReader = new StringReader(input))
            {
                string line;
                while ((line = stringReader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        private static IEnumerable<string> GetMessageSequence(Exception exception)
        {
            while (exception != null)
            {
                yield return exception.Message;
                exception = exception.InnerException;
            }
        }
    }
}
