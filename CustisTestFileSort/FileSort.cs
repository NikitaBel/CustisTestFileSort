using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CustisTestFileSort
{
    public class FileSort
    {
        private string InitFileName;
        private string ResultPath;
        private int recordsize;

        //Кол-во байт оперативной памяти
        private int maxusage;

        /// <summary>
        /// Базовый конструктор для класса
        /// </summary>
        /// <param name="InitFileName">Путь к папке, из которого нужно брать файлы</param>
        /// <param name="ResultPath">Путь к папке, в которой будет сохраняться результат</param>
        public FileSort(string InitFileName, string ResultPath, int recordsize, int maxusage)
        {
            this.InitFileName = InitFileName;
            this.ResultPath = ResultPath + "\\";
            this.recordsize = recordsize;
            this.maxusage = maxusage;
        }

        public void Split()
        {
            int split_num = 1;
            StreamWriter sw = new StreamWriter(
              string.Format(ResultPath + "split{0:d5}.txt", split_num));
            sw.AutoFlush = true;
            using (StreamReader sr = new StreamReader(InitFileName))
            {
                while (sr.Peek() >= 0)
                {
                    // Copy a line
                    sw.WriteLine(sr.ReadLine());

                    // If the file is big, then make a new split,
                    // however if this was the last line then don't bother
                    if (sw.BaseStream.Length > maxusage && sr.Peek() >= 0)
                    {
                        sw.Close();
                        split_num++;
                        sw = new StreamWriter(
                          string.Format(ResultPath + "split{0:d5}.txt", split_num));
                        sw.AutoFlush = true;
                    }
                }
            }
            sw.Close();
        }

        public void SortSplited()
        {
            foreach (string path in Directory.GetFiles(ResultPath, "split*.txt"))
            {
                string[] contents = File.ReadAllLines(path);
                Array.Sort(contents);
                string newpath = path.Replace("split", "sorted");
                File.WriteAllLines(newpath, contents);
                File.Delete(path);
            }
        }

        public void MergeSplited()
        {
            string[] paths = Directory.GetFiles(ResultPath, "sorted*.txt");
            int filesNum = paths.Length;          
            int buffersize = maxusage / filesNum;
            double recordoverhead = 7.5;
            int bufferlen = (int)(buffersize / recordsize / recordoverhead);

            if (bufferlen == 0)
            {
                Console.WriteLine("Ошибка, нельзя произвести сортировку с заданными параметрами");
                return;
            }

            StreamReader[] readers = new StreamReader[filesNum];
            for (int i = 0; i < filesNum; i++)
            {
                readers[i] = new StreamReader(paths[i]);
            }

            Queue<string>[] queues = new Queue<string>[filesNum];
            for (int i = 0; i < filesNum; i++)
            { queues[i] = new Queue<string>(bufferlen); }

            for (int i = 0; i < filesNum; i++)
            {
                LoadQueue(queues[i], readers[i], bufferlen);
            }

            StreamWriter sw = new StreamWriter(ResultPath+"SortedResult.txt",false);
            bool done = false;
            string MinValue;
            int MinIndex, j;

            while (!done)
            {
                MinValue = "";
                MinIndex = -1;
                for (j=0;j< filesNum; j++)
                {
                    if (queues[j] != null)
                    {
                        if (MinIndex<0 || String.CompareOrdinal(queues[j].Peek(), MinValue) < 0)
                        {
                            MinIndex = j;
                            MinValue = queues[j].Peek();
                        }
                    }
                }

                if (MinIndex==-1)
                {
                    done = true;
                    break;
                }

                sw.WriteLine(MinValue);

                queues[MinIndex].Dequeue();

                if (queues[MinIndex].Count == 0)
                {
                    LoadQueue(queues[MinIndex], readers[MinIndex], bufferlen);
                    if (queues[MinIndex].Count == 0)
                    {
                        queues[MinIndex] = null;
                    }
                }
            }
            sw.Close();

            for (int i = 0; i < filesNum; i++)
            {
                readers[i].Close();
                File.Delete(paths[i]);
            }
        }

        void LoadQueue(Queue<string> queue, StreamReader file, int records)
        {
            for (int i = 0; i < records; i++)
            {
                if (file.Peek() < 0) break;
                queue.Enqueue(file.ReadLine());
            }
        }
    }
}
