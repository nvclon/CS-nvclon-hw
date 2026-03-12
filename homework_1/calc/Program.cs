class Calculator {
    static string ReadAndCheck(string prompt) {
        Console.Write(prompt);
        string? input = Console.ReadLine();

        if (input == "!") {
            Environment.Exit(0);
        }

        return input ?? string.Empty;
    }

    static void Main() {
        while (true) {
            double num1 = 0;
            double num2 = 0;
            string operation = "";

            while (true) {
                string input = ReadAndCheck("Первое число: ");
                if (double.TryParse(input, out num1)) {
                    break;
                }
                Console.WriteLine("Ошибка. Введите число или ! для выхода.");
            }

            while (true) {
                string input = ReadAndCheck("Второе число: ");
                if (double.TryParse(input, out num2)) {
                    break;
                }
                Console.WriteLine("Ошибка. Введите число или ! для выхода.");
            }

            while (true) {
                operation = ReadAndCheck("Операция (+, -, *, /): ");
                if (operation == "+" || operation == "-" || operation == "*" || operation == "/") {
                    break;
                }
                Console.WriteLine("Ошибка. Введите +, -, *, / или ! для выхода.");
            }

            double result = 0;
            switch (operation) {
                case "+":
                    result = num1 + num2;
                    break;
                case "-":
                    result = num1 - num2;
                    break;
                case "*":
                    result = num1 * num2;
                    break;
                case "/":
                    if (num2 == 0) {
                        Console.WriteLine("Undefined!");
                        continue;
                    }
                    result = num1 / num2;
                    break;
            }

            Console.WriteLine("Результат: " + result);
            Console.WriteLine();
        }
    }
}