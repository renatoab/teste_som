using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Teste_som
{
    public static class Files
    {
        public static void ReadList(int[] intrand)
        {
            try
            {   // Open the text file using a stream reader.
                string path = Path.GetTempPath();
                using (StreamReader sr = new StreamReader(path + "\\songgen\\rdnum.rd"))
                {
                    // Read the stream to a string, and write the string to the console.
                    string line = sr.ReadToEnd();
                    sr.Close();
                    line = line.Replace("\r", "");
                    string[] lines = line.Split('\n');
                    int nrand = lines.Length;

                    //intrand = new int[nrand];
                    for (int i = 0; i < nrand; i++)
                    {
                        if (lines[i] == "" || i >= intrand.Length)
                        {
                            break;
                        }                        
                        intrand[i] = int.Parse(lines[i]);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Arquivo rdnum.rd não encontrado");
            }
        }

        public static void WriteList(int[] intrand)
        {
            try
            {   // Open the text file using a stream reader.
                string path = Path.GetTempPath();
                if (!Directory.Exists(path + "songgen"))
                {
                    Directory.CreateDirectory(path + "songgen");
                }
                using (StreamWriter sr = new StreamWriter(path + "songgen\\rdnum.rd"))
                {                    
                    int nrand = intrand.Length;
                    for (int i = 0; i < nrand; i++)
                    {
                        sr.WriteLine(intrand[i]);
                    }
                    sr.Close();
                }
            }
            catch
            {
                //MessageBox.Show("Arquivo rdnum.rd não encontrado");
            }
        }

        public static void SaveSequenceFile()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Arquivo rd|*.rd";
            saveFileDialog1.Title = "Save note sequence";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                // Copia do temporário para um arquivo escolhido
                File.Copy(Path.GetTempPath() + "\\songgen\\rdnum.rd", saveFileDialog1.FileName, true);
            }            
        }

        public static void LoadSequenceFile()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Arquivo rd|*.rd";
            openFileDialog1.Title = "Open note sequence";
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName != "")
            {
                // Copia do arquivo escolhido para o temporário
                string path = Path.GetTempPath();
                File.Copy(openFileDialog1.FileName, path + "\\songgen\\rdnum.rd", true);
            }
            else
            {
                throw new Exception();
            }
        }

    }
}
