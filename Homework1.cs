// Необходимо реализовать операцию нахождения квадратного уравнения. Предположим, что эта операция описывается следующей функцией c поправкой на конкретный язык программирования. В ООП языках эта функция реализуется в виде метода класса.
//solve(double a, double b, double c): double[]
//здесь a, b, c - коэффициенты квадратного уравнения, функция возвращает список корней квадратного уравнения.
//OK 3. Написать тест, который проверяет, что для уравнения x^2+1 = 0 корней нет (возвращается пустой массив)
//OK 4. Написать минимальную реализацию функции solve, которая удовлетворяет данному тесту.
//OK 5. Написать тест, который проверяет, что для уравнения x^2-1 = 0 есть два корня кратности 1 (x1=1, x2=-1)
//OK 6. Написать минимальную реализацию функции solve, которая удовлетворяет тесту из п.5.
//OK 7. Написать тест, который проверяет, что для уравнения x^2+2x+1 = 0 есть один корень кратности 2 (x1= x2 = -1).
//OK 8. Написать минимальную реализацию функции solve, которая удовлетворяет тесту из п.7.
//OK 9. Написать тест, который проверяет, что коэффициент a не может быть равен 0. В этом случае solve выбрасывает исключение.
//OK Примечание. Учесть, что a имеет тип double и сравнивать с 0 через == нельзя.
//OK 10. Написать минимальную реализацию функции solve, которая удовлетворяет тесту из п.9.
//OK 11. С учетом того, что дискриминант тоже нельзя сравнивать с 0 через знак равенства, подобрать такие коэффициенты квадратного уравнения для случая одного корня кратности два, чтобы дискриминант был отличный от нуля, но меньше заданного эпсилон. Эти коэффициенты должны заменить коэффициенты в тесте из п. 7.
//OK 12. При необходимости поправить реализацию квадратного уравнения.
//OK 13. Посмотреть какие еще значения могут принимать числа типа double, кроме числовых и написать тест с их использованием на все коэффициенты. solve должен выбрасывать исключение.
//OK 14. Написать минимальную реализацию функции solve, которая удовлетворяет тесту из п.13.
//Сделать merge request/pull request и ссылку на него указать при сдаче ДЗ.
// a * x^2 + bx + c = 0
// D = b * b - 4 * a * c

using System;

namespace HomeWorkOne
{
    class Stuff
    {
        public double epsilon = 10 ^ -3; // всего -3 - для проверки пп. 11 и 12, чтобы попасть в диапазон "мало, но не ноль"
        public static bool isEqual (double a, double b)
        {
            return (Math.Abs(a - b) < epsilon);
        }
    }

    class Tests
    {
        private static void log (string testName, bool passed)
        {
            // запись в лог: время начала, время окончания, название теста, результат
        }

        public static void run ()
        {
            log ("testPoint3", testPoint3());
            log ("testPoint5", testPoint5());
            log ("testPoint7", testPoint7());
            log ("testPoint9", testPoint9());
            log ("testPoint11", testPoint11());
            log ("testPoint13", testPoint13());
        }

        // 3. Написать тест, который проверяет, что для уравнения x^2+1 = 0 корней нет (возвращается пустой массив)
        private static bool testPoint3 ()
        {
            bool passed = true;
            double a = 1.0;
            double b = 0.0;
            double c = 1.0;
            double[] solution = SquareRoot.solve(a, b, c);
            passed = Solution.Length == 0;
            return passed;
        }

        // 5. Написать тест, который проверяет, что для уравнения x^2-1 = 0 есть два корня кратности 1 (x1=1, x2=-1)
        private static bool testPoint5 ()
        {
            bool passed = true;
            double a = 1.0;
            double b = 0.0;
            double c = -1.0;
            double[] solution = SquareRoot.solve(a, b, c);
            if (passed)
            {
                passed = passed & solution.Length == 2;
            }
            if (passed)
            {
                passed = passed & (solution[0] > 0.0) & Stuff.isEqual(solution[0], 1.0) & (solution[1] < 0.0) & Stuff.isEqual(solution[1], -1.0);
            }
            return passed;
        }

        //7. Написать тест, который проверяет, что для уравнения x^2+2x+1 = 0 есть один корень кратности 2 (x1= x2 = -1).
        private static bool testPoint7 ()
        {
            bool passed = true;
            double a = 1.0;
            double b = 2.0;
            double c = 1.0;
            double[] solution = SquareRoot.solve(a, b, c);
            if (passed)
            {
                passed = passed & solution.Length == 2;
            }
            if (passed)
            {
                passed = passed & (solution[0] < 0.0) & (solution[1] < 0.0) & Stuff.isEqual(solution[0], -1.0) & Stuff.isEqual(solution[1], -1.0);
            }
            return passed;
        }

        //9. Написать тест, который проверяет, что коэффициент a не может быть равен 0. В этом случае solve выбрасывает исключение.
        private static bool testPoint9 ()
        {
            bool passed = false;
            double a = 0.0;
            double b = 2.0;
            double c = 1.0;
            double[] solution;
            try
            {
                solution = SquareRoot.solve(a, b, c);
            }
            catch (Exception ex)
            {
                passed = true;
            }
            finally
            {
                return passed;
            }
        }

        //11. С учетом того, что дискриминант тоже нельзя сравнивать с 0 через знак равенства, подобрать такие коэффициенты квадратного уравнения для случая одного корня кратности два, чтобы дискриминант был отличный от нуля, но меньше заданного эпсилон. Эти коэффициенты должны заменить коэффициенты в тесте из п. 7.
        private static bool testPoint11 ()
        {
            bool passed = true;
            double a = 80.3571428;
            double b = 150.0;
            double c = 70.0;
            double[] solution = SquareRoot.solve(a, b, c);
            if (passed)
            {
                passed = passed & solution.Length == 2;
            }
            if (passed)
            {
                passed = passed & (solution[0] < 0.0) & (solution[1] < 0.0) & Stuff.isEqual(solution[0], -1.0) & Stuff.isEqual(solution[1], -1.0);
            }
            return passed;
        }

        //13. Посмотреть какие еще значения могут принимать числа типа double, кроме числовых и написать тест с их использованием на все коэффициенты. solve должен выбрасывать исключение.
        private static bool testPoint13 ()
        {
            bool passed = false;
            double[] solution;
            double a;
            double b;
            double c;

            a = Double.NaN;
            b = 0.0;
            c = -1.0;
            try
            {
                solution = SquareRoot.solve(a, b, c);
                return passed;
            }
            catch (Exception ex) {}
            a = Double.NegativeInfinity;
            try
            {
                solution = SquareRoot.solve(a, b, c);
                return passed;
            }
            catch (Exception ex) {}
            a = Double.PositiveInfinity;
            try
            {
                solution = SquareRoot.solve(a, b, c);
                return passed;
            }
            catch (Exception ex) {}

            a = 1.0;
            b = Double.NaN;
            c = -1.0;
            try
            {
                solution = SquareRoot.solve(a, b, c);
                return passed;
            }
            catch (Exception ex) {}
            b = Double.NegativeInfinity;
            try
            {
                solution = SquareRoot.solve(a, b, c);
                return passed;
            }
            catch (Exception ex) {}
            b = Double.PositiveInfinity;
            try
            {
                solution = SquareRoot.solve(a, b, c);
                return passed;
            }
            catch (Exception ex) {}

            a = 1.0;
            b = 0.0;
            c = Double.NaN;
            try
            {
                solution = SquareRoot.solve(a, b, c);
                return passed;
            }
            catch (Exception ex) {}
            c = Double.NegativeInfinity;
            try
            {
                solution = SquareRoot.solve(a, b, c);
                return passed;
            }
            catch (Exception ex) {}
            c = Double.PositiveInfinity;
            try
            {
                solution = SquareRoot.solve(a, b, c);
                return passed;
            }
            catch (Exception ex) {}
            
            passed = true;

            return passed;
        }
    }

    public class SquareRoot
    {
        public static double[] solve (double a, double b, double c)
        {
            double D, x1, x2;

            if (Stuff.isEqual(a, 0.0)) // p. 10
            {
                throw new Exception("This is not a quadratic equation as the first coefficient equals zero.");
            }

            try // p. 13
            {
                D = a + b + c;
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid parameter.");
            }
            
            // solution

            D = b * b - 4 * a * c;
            
            if (D < 0.0)
            {
                return new double[]; // p. 4
            }
            else
            {
                if (Stuff.isEqual(D, 0.0))
                {
                    x1 = -b / (2 * a);
                    x2 = x1;
                    return new double[] { x1, x2 }; // including p. 8
                }
                else
                {
                    if ((b ^ 2) / (4 * c) > a) // условие определения "гораздо больше", получено эмпирически для коэффициентов п. 11, дающих очень малый D; может быть уточнено
                    {
                        // для пп. 11 и 12: если b ^ 2 >> | 4 * c |, x1 = - (b + sign(b) * sqrt (D)) / 2; x2 = c / x1;
                        x1 = -(b + Math.Sign(b) * Math.Sqrt(D)) / 2;
                        x2 = c / x1;
                        return new double[] { x1, x2 };
                    }
                    else
                    {
                        x1 = -b + Math.Sqrt(D) / (2 * a);
                        x2 = -b - Math.Sqrt(D) / (2 * a);
                        return new double[] { x1, x2 }; // including p. 6
                    }
                }
            }
        }
    }
}