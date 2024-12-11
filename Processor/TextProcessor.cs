using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ManageFile.Processor
{
    public static class TextProcessor
    {

        public static string remove_null(string Content)
        {
            string modifiedContent;

            var lines = Content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            var nonEmptyLines = lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
            modifiedContent = string.Join(Environment.NewLine, nonEmptyLines);
            return modifiedContent;
        }
        public static string remove_number_from_start(string content, Dictionary<string, object> Params)
        {
            var lines = content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            string pattern;
            if (Params["condition"].ToString() == "any_number")
            {
                pattern = @"^\d+";
            }
            else
            {
                pattern = @"^\d+\s";
            }
            var processedLines = lines.Select(line => Regex.Replace(line, pattern, ""));
            return string.Join(Environment.NewLine, processedLines);
        }

        public static string remove_contain(string content, Dictionary<string, object> Params)
        {
            JArray jsonArray = JArray.Parse(Params["word"].ToString());
            List<string> words = jsonArray.ToObject<List<string>>();
            var lines = content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            var filteredLines = lines.Where(line => !words.Any(w => line.Contains(w, StringComparison.OrdinalIgnoreCase)));

            return string.Join(Environment.NewLine, filteredLines);
        }
        public static string replace(string content, Dictionary<string, object> Params)
        {

            content = content.Replace(Params["word1"].ToString(), Params["word2"].ToString(), StringComparison.OrdinalIgnoreCase);
            return content;
        }
        public static string remove_between(string content, Dictionary<string, object> Params)
        {
            string pattern = @$"{Regex.Escape(Params["word1"].ToString())}.*?{Regex.Escape(Params["word2"].ToString())}";
            return Regex.Replace(content, pattern, string.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);
        }
        public static string remove_space(string content)
        {
            var lines = content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            var trimmedLines = lines.Select(line => line.Replace(" ", ""));
            return string.Join(Environment.NewLine, trimmedLines);
        }
        public static string remove_lines_starting_with(string content, Dictionary<string, object> Params)
        {
            var lines = content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            var filteredLines = lines.Where(line => !line.StartsWith(Params["word"].ToString(), StringComparison.OrdinalIgnoreCase));
            return string.Join(Environment.NewLine, filteredLines);
        }
        public static string remove_from_start(string content, Dictionary<string, object> Params)
        {
            JArray jsonArray = JArray.Parse(Params["word"].ToString());
            List<string> words = jsonArray.ToObject<List<string>>();
            if (words.Count == 0)
            {
                return content;
            }

            var lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            for (int i = 0; i < lines.Length; i++)
            {
                foreach (string word in words)
                {
                    int index = lines[i].IndexOf(word, StringComparison.Ordinal);
                    if (index != -1)
                    {
                        lines[i] = lines[i].Substring(index + word.Length);
                        break;
                    }
                }
            }

            return string.Join(Environment.NewLine, lines);
        }
        public static string searching(string content, Dictionary<string, object> Params)
        {

            var lines = content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            var filteredLines = lines.Where(line => line.Contains(Params["word"].ToString(), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(Params["count"].ToString()))
            {
                filteredLines = filteredLines.Take(int.Parse(Params["count"].ToString()));
            }
            return string.Join(Environment.NewLine, filteredLines);
        }
    }
}
