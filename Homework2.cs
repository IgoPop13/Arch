//В результате выполнения ДЗ будет получен код, отвечающий за движение объектов по игровому полю, устойчивый к появлению новых игровых объектов и дополнительных ограничений, накладываемых на это движение.

//Описание/Пошаговая инструкция выполнения домашнего задания:
//В далекой звездной системе встретились две флотилии космических кораблей. Корабли могут передвигаться по всему пространству звездной системы по прямой, поворачиваться против и по часовой стрелке, стрелять фотонными торпедами. Попадание фотонной торпеды в корабль выводит его из строя.
//От каждой флотилии в сражении принимают участие по три космических корабля.
//Победу в битве одерживает та флотилия, которая первой выведет из строя все корабли соперника.
//Управление флотилиями осуществляется игрокам компьютерными программами (то есть не с клавиатуры).
//Концептуально игра состоит из трех подсистем:

//Игровой сервер, где реализуется вся игровая логика.
//Player - консольное приложение, на котором отображается конкретная битва.
//Агент - приложение, которое запускает программу управления танками от имени игрока и отправляет управляющие команды на игровой сервер.
//Реализовать движение объектов на игровом поле в рамках подсистемы Игровой сервер.

//Критерии оценки:
//За выполнение каждого пункта, перечисленного ниже начисляются баллы:

//ДЗ сдано на проверку - 1 балл
//Оформлен pull/merge request на github/gitlab - 1 балл
//Настроен CI - 2 балла
//Прямолинейное равномерное движение без деформации.
//Само движение реализовано в виде отдельного класса - 1 балл.
//Для движущихся объектов определен интерфейс, устойчивый к появлению новых видов движущихся объектов - 1 балл
//Реализован тесты (1 балл за все):
//Для объекта, находящегося в точке (12, 5) и движущегося со скоростью (-7, 3) движение меняет положение объекта на (5, 8)
//Попытка сдвинуть объект, у которого невозможно прочитать положение в пространстве, приводит к ошибке
//Попытка сдвинуть объект, у которого невозможно прочитать значение мгновенной скорости, приводит к ошибке
//Попытка сдвинуть объект, у которого невозможно изменить положение в пространстве, приводит к ошибке

//Поворот объекта вокруг оси.
//Сам поворот реализован в виде отдельного класса - 1 балл
//Для поворачивающегося объекта определен интерфейс, устойчивый к появлению новых видов движущихся объектов - 1 балл
//Реализован тесты - 1 балл.
//Итого: 10 баллов
//Задание считается принятым, если набрано не менее 7 баллов.


using System;

namespace HomeWorkTwo
{

    interface IMovable
    {
        Move()
    }
    class Move
    {
        
    }
    
    class Stuff
    {
        public static double epsilon = 10 ^ -3; // всего -3 - для проверки пп. 11 и 12, чтобы попасть в диапазон "мало, но не ноль"
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
        }
    }

    public class SquareRoot
    {
    }
}