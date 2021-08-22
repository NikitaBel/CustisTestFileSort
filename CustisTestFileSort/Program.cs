using System;

namespace CustisTestFileSort
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Программа генерации файла:");
            Console.WriteLine("Введите путь к папке, где будет создан файл");
            string filepath =  Console.ReadLine();
            InitFileCreate fileCreate = new InitFileCreate(filepath);
            Console.WriteLine("Введите кол-во символов в одной строке файле");
            int recordLength = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Введите кол-во строк файле");
            int recordNum = Convert.ToInt32(Console.ReadLine());
            fileCreate.GenerateFile(recordLength, recordNum);

            Console.WriteLine("Программа сортировки файла:");
            Console.WriteLine("Введите путь к папке, где будет создан отсортированный файл");
            filepath = Console.ReadLine();

            Console.WriteLine("Введите размер оперативной памяти");
            int maxusage = Convert.ToInt32(Console.ReadLine());

            string FullFilePath = fileCreate.GetFilePath();
            FileSort fileSort = new FileSort(FullFilePath, filepath, recordLength, maxusage);
            fileSort.Split();
            fileSort.SortSplited();
            fileSort.MergeSplited();
        }
    }
}
