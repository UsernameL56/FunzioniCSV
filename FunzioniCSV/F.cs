using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunzioniCSV
{
    public static class F
    {
        public static void AggiuntaRecords(string line, string file, byte[] br, int RecordLength)
        {
            int contatore = 0;
            Random r = new Random();
            byte[] brAppoggio;

            lock (file)
            {
                using (var reader = new FileStream(file, FileMode.Open, FileAccess.ReadWrite))
                using (var sr = new StreamReader(reader))
                using (var bReader = new BinaryReader(reader))
                {
                    reader.Seek(0, SeekOrigin.Begin);

                    while (reader.Position < reader.Length)
                    {
                        if (contatore == 0)
                        {
                            br = bReader.ReadBytes(RecordLength);
                            line = Encoding.ASCII.GetString(br, 0, br.Length);
                            var index = line.LastIndexOf(';');
                            brAppoggio = Encoding.ASCII.GetBytes(line.Substring(0, index));
                            line = Encoding.ASCII.GetString(brAppoggio, 0, brAppoggio.Length);

                            reader.Seek(RecordLength * contatore, SeekOrigin.Begin);
                            br = Encoding.ASCII.GetBytes((line + ";miovalore;cancellazione logica;").PadRight(RecordLength - 4) + "##");
                            sr.BaseStream.Write(br, 0, br.Length);
                        }
                        else
                        {
                            reader.Seek(RecordLength * contatore, SeekOrigin.Begin);

                            line = sr.ReadLine();

                            var index = line.LastIndexOf(';');
                            brAppoggio = Encoding.ASCII.GetBytes(line.Substring(0, index));
                            line = Encoding.ASCII.GetString(brAppoggio, 0, brAppoggio.Length);

                            reader.Seek(RecordLength * contatore, SeekOrigin.Begin);
                            br = Encoding.ASCII.GetBytes((line + ";" + r.Next(10, 21) + ";true;").PadRight(RecordLength - 4) + "##");
                            sr.BaseStream.Write(br, 0, br.Length);
                        }
                        contatore++;
                    }
                }
            }
        }
    }
}
