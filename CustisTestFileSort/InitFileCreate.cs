using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace CustisTestFileSort
{
    public class InitFileCreate
    {
        private string InitFilePath;       
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        /// <summary>
        /// Базовый конструктор для класса
        /// </summary>
        /// <param name="InitFilePath">Путь к папке, где будет создан файл</param>
        public InitFileCreate(string InitFilePath)
        {
            this.InitFilePath = InitFilePath+ "\\InitFile.txt";            
        }

        /// <summary>
        /// Генерация файла 
        /// </summary>
        /// <param name="recordLength">Максимальное кол-во символов в одной строке</param>
        /// <param name="recordNum">Кол-во строк в одном файле</param>
        public void GenerateFile(int recordLength, int recordNum)
        {
            Random randomChar = new Random();
            Random randomLength = new Random();           
            StreamWriter sw = new StreamWriter(this.InitFilePath, false);            
            for (int i=0;i< recordNum; i++)
            {
                int currentlength = randomLength.Next(5, recordLength);
                string currentRow = new string(Enumerable.Repeat(chars, currentlength).Select(c => c[randomChar.Next(c.Length)]).ToArray());
                sw.WriteLine(currentRow);            
            }
            sw.Close();
        }

        /// <summary>
        /// Поучение пути, куда были записаны файлы
        /// </summary>
        /// <returns>Путь к файл InitFile.txt</returns>
        public string GetFilePath()
        {
            return this.InitFilePath;
        }

    }
}
