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
        public static void AggiuntaRecords(string file, int RecordLength)
        {
            //dichiarazione variabili
            string line = "";
            int contatore = 0;
            Random r = new Random();
            byte[] br, brAppoggio;

            //apertura file
            var reader = new FileStream(file, FileMode.Open, FileAccess.ReadWrite);
            BinaryReader sr = new BinaryReader(reader);
            //posizionamento all'inizio del file
            reader.Seek(0, SeekOrigin.Begin);

            //ciclo while per verificare quando si arriva alla fine del file
            while(reader.Position < reader.Length)
            {
                //posizionamento sull'inizio della riga indicata dal contatore
                reader.Seek(RecordLength * contatore, SeekOrigin.Begin);
                //lettura di tutta la riga e conversione in stringa
                br = sr.ReadBytes(RecordLength);
                line = Encoding.ASCII.GetString(br);
                //visualizzare la posizione dell'ultimo carattere
                var index = line.LastIndexOf(";");
                if (index < 0)
                    break;
                //lettura di tutta la riga fino al ";" e conversione in stringa
                brAppoggio = Encoding.ASCII.GetBytes(line.Substring(0, index));
                line = Encoding.ASCII.GetString(brAppoggio);
                //riposizionamento all'inizio della riga attuale
                reader.Seek(RecordLength * contatore, SeekOrigin.Begin);
                
                //controllo per capire se siamo sulla prima riga o no
                //sovrascrittura della riga con la stringa originale + Tipo record / numero casuale e campo per cancellazione logica
                if (contatore == 0)
                {
                    br = Encoding.ASCII.GetBytes((line + ";miovalore;cancellazione logica;").PadRight(RecordLength - 4));
                    sr.BaseStream.Write(br, 0, br.Length);
                }
                else
                {
                    br = Encoding.ASCII.GetBytes((line + ";" + r.Next(10, 21) + ";true;").PadRight(RecordLength - 4));
                    sr.BaseStream.Write(br, 0, br.Length);
                }
                contatore++;
            }
            reader.Close();   
        }

        public static int NumeroCampi(string file, int RecordLength)
        {
            //dichiarazione variabili
            string line = "";
            int campi = 0;
            byte[] br, brAppoggio;

            //apertura file
            var reader = new FileStream(file, FileMode.Open, FileAccess.ReadWrite);
            BinaryReader sr = new BinaryReader(reader);
            //posizionamento sulla prima riga del file
            reader.Seek(0, SeekOrigin.Begin);
            //lettura di tutta la riga e conversione in stringa
            br = sr.ReadBytes(RecordLength);
            line = Encoding.ASCII.GetString(br);
            //posizione ultimo carattere
            var index = line.LastIndexOf(";");
            //lettura di tutta la riga fino al ";" e conversione in stringa
            brAppoggio = Encoding.ASCII.GetBytes(line.Substring(0, index));
            line = Encoding.ASCII.GetString(brAppoggio);
            //conteggio degli split per capire il numero di campi presenti
            campi = line.Split(';').Length;

            return campi;
        }

        public static int LunghezzaMassima(string file, int RecordLength)
        {
            int max = 0;
            //dichiarazione variabili
            string line = "";
            int campi = 0;
            byte[] br, brAppoggio;

            //apertura file
            var reader = new FileStream(file, FileMode.Open, FileAccess.ReadWrite);
            BinaryReader sr = new BinaryReader(reader);
            //posizionamento sulla prima riga del file
            reader.Seek(0, SeekOrigin.Begin);
            //ciclo per trovare la lunghezza massima
            while(reader.Position < reader.Length)
            {
                //lettura di tutta la riga e conversione in stringa
                br = sr.ReadBytes(RecordLength);
                line = Encoding.ASCII.GetString(br);
                //posizione ultimo carattere
                var index = line.LastIndexOf(";");
                if (index < 0)
                    break;
                else
                {
                    //se l'indice è maggiore del numero massimo attuale, allora il numero attuale cambia
                    if(index>max)
                        max = index;
                }
            }
            return max;
        }
    }
}
