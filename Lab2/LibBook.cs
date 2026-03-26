using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LR2
{
    internal class LibBook(string title, string author, int copiesCount) : IGetInfo
    {
        public string Title { get; private set; } = title;
        public string Author { get; private set; } = author;
        public int PublicationYear { get; private set; } = new Random().Next(1960, 2025);
        public int CopiesCount { get; private set; } = copiesCount;
        public int AvaliableCopies {  get; private set; } = new Random().Next(0, copiesCount);
        public void GetFullInfo()
        {
            Console.WriteLine($"{Title}\n{Author}\n{PublicationYear}");
        }
        public void Take()
        {
            if (AvaliableCopies != 0)
            {
                AvaliableCopies--;
                Console.WriteLine($"Из библиотеки была взята книга {Title}. Экземпляров осталось - {AvaliableCopies}");
            }
            Console.WriteLine($"В библиотеке нет доступных книг с названием {Title}");
        }
        public void Return()
        {
            if (AvaliableCopies < CopiesCount) 
            {
                AvaliableCopies++;
                Console.WriteLine($"В библиотеку возвращена книга {Title}. Всего доступных экземпляров - {AvaliableCopies}");
            }
            Console.WriteLine($"В библиотеке хранятся все копии книги {Title}");
        }

        public void BooksDelivery(int count)
        {
            CopiesCount += count;
            AvaliableCopies += count;
            Console.WriteLine($"В библиотеку были доставлены новые экземпляры книг в количестве {count} шт.");
        }
    }
}
