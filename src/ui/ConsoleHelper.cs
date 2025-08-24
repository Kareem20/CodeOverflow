namespace CodeOverFlow.ui
{
    public static class ConsoleHelper
    {
        private const string ERROR_MSG = "Invalid option. Please try again.";

        public static void ShowMsg(string msg) => Console.WriteLine(msg);
        public static int PromptInt(string message, Func<int, bool> checker)
        {
            int value;
            string input;
            bool firstPrompt = false;
            do
            {
                if (firstPrompt)
                    Console.WriteLine(ERROR_MSG);
                Console.WriteLine(message);
                input = Console.ReadLine();
                firstPrompt = true;
            } while (!int.TryParse(input, out value) || (checker != null && !checker(value)));
            return value;
        }
        public static T PromptString<T>(string message
            , Func<string, T> converter
            , params (Func<T, bool> checkers, string errorMsgs)[] rules)
        {
            T value;
            while (true)
            {
                Console.WriteLine(message);
                string? input = Console.ReadLine()?.Trim();
                try
                {
                    value = converter(input!); // convert from string to T
                }
                catch
                {
                    Console.WriteLine("Invalid input format!");
                    continue;
                }
                bool error = false;
                foreach (var (checker, errorMsg) in rules)
                {
                    if (!checker(value))
                    {
                        Console.WriteLine(errorMsg);
                        error = true;
                        break;
                    }
                }
                if (!error) break;
            }
            return value;
        }
        public static List<string> PromptCommaSperatedStrings(string message)
        {
            Console.WriteLine(message);
            var input = Console.ReadLine() ?? "";
            return input
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim().ToLower())
                .Where(t => !string.IsNullOrEmpty(t)).Distinct().ToList();
        }
        public static int PromptOptions(string message, params string[] options)
        {
            Console.WriteLine(message);
            for (int i = 0; i < options.Length; i++)
                Console.WriteLine($"{i + 1}: {options[i]}");
            return PromptInt(message, choice => 1 <= choice && choice <= options.Length);
        }
    }
}
