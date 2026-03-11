class Calculator {
    static void Main() {
        Console.Write("Первое число: ");
        double num1;
        while (!double.TryParse(Console.ReadLine(), out num1)) {
            Console.Write("Ошибка. Введите число: ");
        }

        Console.Write("Второе число: ");
        double num2;
        while (!double.TryParse(Console.ReadLine(), out num2)) {
            Console.Write("Ошибка. Введите число: ");
        }

        Console.WriteLine("Теперь операция: +, -, *, /");
        string? operation = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(operation)) {
            Console.WriteLine("Неверная операция!");
            return;
        }

        double result = 0;
        switch (operation)
        {
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
                if (num2 != 0) {
                    result = num1 / num2;
                } else {
                    Console.WriteLine("Undefined!");
                    return;
                }
                break;
            default:
                Console.WriteLine("Неверная операция!");
                return;
        }
        Console.WriteLine("Результат: " + result);
        Console.WriteLine("Нажмите Enter, чтобы выйти...");
        Console.ReadLine();
    }
}