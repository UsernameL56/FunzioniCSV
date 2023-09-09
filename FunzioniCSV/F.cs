using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunzioniCSV
{
    public static class F
    {
        public static void Spaziatura(string file, string appoggio, int RecordLength)
        {
            //dichiarazioni variabili
            string line, sep = ";";
            int n = 0;
            //apertura file
            StreamReader reader = new StreamReader(appoggio);
            StreamWriter writer = new StreamWriter(file);
            //ciclo per leggere tutte le righe fino alla fine del file e aggiungere la spaziatura necesaria per rendere tutto uguale + campo univoco
            while ((line = reader.ReadLine()) != null)
            {
                if(n == 0)
                    writer.WriteLine((line + sep).PadRight(RecordLength - 4) + "##");
                else
                    writer.WriteLine((line + sep + n + sep).PadRight(RecordLength - 4) + "##");
                n++;
            }
            reader.Close();
            writer.Close();
        }
        public static void AggiuntaRecords(string file, int RecordLength)
        {
            //dichiarazione variabili
            string line = "", sep = ";";
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
                else
                {
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
                        br = Encoding.ASCII.GetBytes((line + sep + r.Next(10, 21) + ";true;").PadRight(RecordLength - 4));
                        sr.BaseStream.Write(br, 0, br.Length);
                    }
                    contatore++;
                }
                
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

            reader.Close();
            return campi;
        }

        public static int LunghezzaMassima(string file, int RecordLength)
        {
            
            //dichiarazione variabili
            string line = "";
            int max = 0;
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
            reader.Close();
            return max;
        }

        public static void RecordCoda(string file, int RecordLength, string line)
        {
            //dichiarazione variabili
            byte[] br, brAppoggio;
            Random r = new Random();
            int n = F.NumRighe(file);

            //apertura file
            var writer = new FileStream(file, FileMode.Append, FileAccess.Write, FileShare.Read);
            BinaryWriter sr = new BinaryWriter(writer);
            //scrittura in append
            br = Encoding.ASCII.GetBytes((line + n + ";" + r.Next(10, 21) + ";true;").PadRight(RecordLength - 4) + "##");
            sr.Write(br, 0, br.Length);

            writer.Close();
        }

        public static string[,] TreCampi(string file, int RecordLength, int split1, int split2, int split3)
        {
            //dichiarazione variabili
            string line = "";
            int contatore = 0;
            byte[] br, brAppoggio;
            //richiamo alla funzione per sapere il numero di righe del file
            int x = NumRighe(file);
            string[,] matrice = new string[x, 5];
            //apertura file
            var reader = new FileStream(file, FileMode.Open, FileAccess.ReadWrite);
            BinaryReader sr = new BinaryReader(reader);
            //posizionamento sulla seconda riga
            reader.Seek(RecordLength*1, SeekOrigin.Begin);
            //ciclo per aggiungere i 3 campi scelti dall'utente nella matrice
            while(reader.Position < reader.Length)
            {
                //lettura di tutta la riga + conversione in stringa
                br = sr.ReadBytes(RecordLength);
                line = Encoding.ASCII.GetString(br);
                var index = line.LastIndexOf(";");
                if (index < 0)
                    break;
                else
                {
                    //lettura della riga fino all'ultimo carattere e conversione in stringa
                    brAppoggio = Encoding.ASCII.GetBytes(line.Substring(0, index));
                    line = Encoding.ASCII.GetString(brAppoggio);
                    //split con parametri scelti dall'utente
                    String[] split = line.Split(';');
                    matrice[contatore, 0] = split[split1];
                    matrice[contatore, 1] = split[split2];
                    matrice[contatore, 2] = split[split3];
                }
                contatore++;
            }

            reader.Close();
            return matrice;
        }

        public static string Ricerca(string file, int RecordLength, int input)
        {
            //dichiarazione variabili
            string line = "";
            byte[] br, brAppoggio;

            //apertura file
            var reader = new FileStream(file, FileMode.Open, FileAccess.ReadWrite);
            BinaryReader sr = new BinaryReader(reader);
            //posizionamento sulla riga dove si trova il campo univoco
            reader.Seek(RecordLength * input, SeekOrigin.Begin);
            //lettura di tutta la riga e conversione in stringa
            br = sr.ReadBytes(RecordLength);
            line = Encoding.ASCII.GetString(br);
            //posizione ultimo carattere
            var index = line.LastIndexOf(";");
            //lettura riga fino all'ultimo carattere e conversione in stringa
            brAppoggio = Encoding.ASCII.GetBytes(line.Substring(0, index));
            line = Encoding.ASCII.GetString(brAppoggio);
            reader.Close();
            return line;
        }

        public static int NumRighe(string file)
        {
            //dichiarazione contatore
            int n = 0;
            //apertura file lettura
            StreamReader sr = new StreamReader(file);
            //ciclo per leggere le righe fino alla fine del file con incremento del contatore per tenerne il conto
            while (!sr.EndOfStream)
            {
                sr.ReadLine();
                n++;
            }
            sr.Close();
            return n;
        }





        public static void addUserControl(Panel panel1, UserControl userControl)
        {
            //impostazione del UserControl al centro e con riempimento di tutto il panel
            userControl.Dock = DockStyle.Fill;
            //rimozione dell'UserControl attuale dal panel
            panel1.Controls.Clear();
            //aggiunta del nuovo UserControl al panel
            panel1.Controls.Add(userControl);
            //imposta l'UserControl al primo piano
            userControl.BringToFront();
        }
    }
}
