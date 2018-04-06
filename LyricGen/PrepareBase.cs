using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LyricGen
{
    public static class PrepareBase
    {
        public static string[] listAllVerses;

        public static Random random;

        public static string[] listSelectedVerses;

        public static void readAllFiles()
        {
            initRand();
            try
            {   // Open the text file using a stream reader.
                string path = "..\\..\\..\\LyricGen\\base";
                DirectoryInfo d = new DirectoryInfo(path);
                FileInfo[] Files = d.GetFiles("*.txt");
                listAllVerses = new string[1000];
                int i = 0;
                foreach (FileInfo file in Files)
                {
                    string[] straux = readFile(path + "\\" + file.Name);
                    int icount = i;
                    for (; icount < straux.Length; icount++)
                    {
                        listAllVerses[icount] = straux[icount - i];
                    }
                    i = icount;
                }                
            }
            catch
            {
                //MessageBox.Show("Erro de leitura de arquivo de letras");
            }
        }        
       

        public static string[] readFile(string fileName)
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                // Read the stream to a string, and write the string to the console.
                string line = sr.ReadToEnd();
                sr.Close();
                //line = line.Replace("\r", "");
                string[] lines = line.Split('\n');
                int nrand = lines.Length;
                return lines;
            }
        }

        public static void mountList()
        {
            int irand;
            int[] intList = new int[8];
            listSelectedVerses = new string[8];
            for (int i = 0; i < 8; i++)
            {
                irand = RandGenerator();
                if (listAllVerses[irand] != null)
                {
                    if (listAllVerses[irand].Length < 2)
                    {
                        i--;
                    }          
                    else
                    {
                        listSelectedVerses[i] = listAllVerses[irand];
                    }          
                }
                else
                {
                    i--;
                }                
            }
        }


        public static void initRand()
        {
            int irand = 0;
            random = new Random();
        }

        public static int RandGenerator()
        {            
            return random.Next(0, 500);
        }


    }
}
