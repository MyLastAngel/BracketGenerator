using System;
using System.Collections.Generic;
using System.IO;

namespace BracketGenerator
{
    // YA - для прохождения теста яндекса.. 
    // Для своего тестирования YA из дебага убирается

    class Program
    {
        static void Main(string[] args)
        {
            var countX2 = 0;
            var result = new List<int>();

#if YA
            var countX2 = 0;
            using (var stream = File.OpenRead(@"input.txt"))
            {
                using (var reader = new StreamReader(stream))
                    countX2 = int.Parse(reader.ReadLine()) * 2;
            }
#else
            // Для теста циклический прогон от 0 до 14, с выводом в консоль
            for (var test = 0; test <= 14; test++)

            {
                countX2 = test * 2;
                var t = System.Diagnostics.Stopwatch.StartNew();

                result.Clear();
#endif

                if (countX2 > 0)
                {
                    // формируем стартовое (=0, ) =1
                    int value = 0;
                    for (var i = 0; i < countX2 / 2; i++)
                        value = (value | (1 << i));

                    // Формируем массив цифр 
                    result.Add(value);
                    GetFromValue(value, 0, countX2, ref result);

                    // Формируем результат
                    var sBuilder = new System.Text.StringBuilder();
                    for (var i = 0; i < result.Count; i++)
                    {
                        // Кривенько, лень было работать с перевернутыми битами
                        sBuilder.AppendLine(Convert.ToString(result[i], 2).PadLeft(countX2, '0').Replace('0', '(').Replace('1', ')'));
                    }

#if YA
                    File.WriteAllText(@"output.txt", sBuilder.ToString());
#else
                    // Вывод результат в консоль
                    t.Stop();

                    Console.WriteLine(sBuilder.ToString());
                    Console.WriteLine($"count : {countX2 / 2} results: {result.Count} за {t.Elapsed}");

                    Console.ReadLine();
                }
#endif
            }
        }

        // рекурсивное формирование последовательности
        static void GetFromValue(int value, int start, int end, ref List<int> result)
        {
            var oneCount = 1;

            for (var i = start + 1; i < end - 1; i++)
            {
                if ((value & (1 << i)) == 0)
                {
                    if (oneCount > 1)
                    {
                        value = ((value | (1 << i)) ^ (1 << (i - 1)));

                        result.Add(value);
                        GetFromValue(value, start, i + 1, ref result);
                    }

                    oneCount--;
                }
                else
                    oneCount++;
            }
        }
    }
}
