using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace telefon
{
    class Ido
    {
        public int Ora { get; set; }
        public int Perc { get; set; }
        public int MP { get; set; }
        public Ido(string ora, string perc, string mp)
        {
            Ora = int.Parse(ora);
            Perc = int.Parse(perc);
            MP = int.Parse(mp);
        }

        public int mpTicks()
        {
            return 60 * 60 * Ora + 60 * Perc + MP;
        }

    }
    class Hivas
    {
        public int ID { get; set; }
        public Ido Hiv { get; set; }
        public Ido Bont { get; set; }
        public Ido Kezd { get; set; } //ha fogadtak a hivast, akkor a beszelgetes kezdete

        public Hivas(int id, string adat)
        {
            ID = id;
            string[] adatok = adat.Split(' ');
            Hiv = new Ido(adatok[0], adatok[1], adatok[2]);
            Bont = new Ido(adatok[3], adatok[4], adatok[5]);
            Kezd = new Ido("0", "0", "0");
        }

        #region 1. feladat
        public int mpbe(int ora, int perc, int mp)
        //csak azért itt és így, hogy a feladat kiírásának megfeleljen
        {
            return 60 * 60 * ora + 60 * perc + mp;
        }

        public int Hossz()
        {
            //itt használom is egyszer az mpbe függvényt.
            return mpbe(Bont.Ora, Bont.Perc, Bont.MP) - Hiv.mpTicks();
        }
        #endregion

    }


    class Program
    {
        static List<Hivas> hivasok = new List<Hivas>();
        static int N;
        //static int utolso;

        static void Main(string[] args)
        {
            //2. feladat
            Input();

            Console.WriteLine("3. feladat");
            Orankent();

            Console.WriteLine("4. feladat");
            int leghosszabb = MaxHossz();
            Console.WriteLine("A leghosszabb ideig vonalban levo hivo {0}. sorban szerepel, a hivas hossza: {1} masodperc.", hivasok[leghosszabb].ID, hivasok[leghosszabb].Hossz());

            Console.WriteLine("5. feladat");
            Egyidopont();

            Console.WriteLine("6. feladat");
            Utolso();
            //vagy, ha az "utolso" változó globális:
            //Console.WriteLine("Az utolso telefonalo adatai a(z) {0}.sorban vannak, {1} masodpercig vart.", hivasok[utolso].ID, hivasok[utolso].Kezd.mpTicks() - hivasok[utolso].Hiv.mpTicks());

            //7. feladat
            Output();
            Console.WriteLine("Kész");
            Console.ReadKey();

        }

        static void Input()
        {
            using (System.IO.StreamReader sr = new System.IO.StreamReader("hivas.txt"))
            {
                Ido veg = new Ido("12", "0", "0");
                N = 0;
                //az 0. hívás a nyitás előtt "foglalt jelzést ad"
                hivasok.Add(new Hivas(N, "0 0 0 8 0 0"));

                int utolso = 0;
                while (!sr.EndOfStream)
                {
                    N++;
                    hivasok.Add(new Hivas(N, sr.ReadLine()));
                    /*Beszélgetés kezdete = hívás pillanata vagy az előző beszélgetés vége, ami  megelőző legutolsó bontás.
                    Munkaidő előtt a 0. hívó "beszél", munkaidő után nem lehet beszélni.
                    Ha a hívást fogadják, az adott hívás bontása lesz a foglaltság utolsó pillanata
                    */
                    if (hivasok[N].Hiv.mpTicks() < veg.mpTicks() &&
                        hivasok[N].Bont.mpTicks() > hivasok[utolso].Bont.mpTicks())
                    {
                        /*
                        Ha az előző beszélgetés bontása a hívást követően volt, akkor a bontás pillanata lesz a kezdés
                        */
                        if (hivasok[N].Hiv.mpTicks() < hivasok[utolso].Bont.mpTicks())
                        {
                            hivasok[N].Kezd = hivasok[utolso].Bont;
                        }
                        /*ha hívás előtt volt az előző bontása, akkor a hívás ideje lesz a beszélgetés kezdete*/
                        else
                        {
                            hivasok[N].Kezd = hivasok[N].Hiv;
                        }
                        utolso = N;
                    }
                }
            }
        }

        private static void Orankent()
        {
            int[] darab = new int[24];
            for (int i = 0; i < 24; i++)
            {
                darab[i] = 0;
            }
            for (int i = 1; i <= N; i++)
            {
                darab[hivasok[i].Hiv.Ora]++;
            }

            for (int i = 0; i < 24; i++)
            {
                if (darab[i] != 0)
                {
                    Console.WriteLine("{0} ora {1} hivas", i, darab[i]);
                }
            }
        }
     
        private static int MaxHossz()
        {
            int maxh = 1;
            for (int i = 1; i <= N; i++)
            {
                if (hivasok[i].Hossz() > hivasok[maxh].Hossz())
                {
                    maxh = i;
                }
            }
            return maxh;
        }

        private static void Egyidopont()
        {
            Console.Write("Adjon meg egy idopontot! (ora perc masodperc): ");
            string[] adatok = Console.ReadLine().Split(' ');
            int idopont = new Ido(adatok[0], adatok[1], adatok[2]).mpTicks();

            int beszelo = -1;
            int db = 0;
            int i = 1;
            while (i <= N && hivasok[i].Bont.mpTicks() < idopont)
            {
                i++;
            }
            if (i <= N)
            {
                beszelo = i;

                while (hivasok[i].Hiv.mpTicks() <= idopont)
                {
                    if (hivasok[i].Bont.mpTicks() > idopont)
                    {
                        db++;
                    }
                    i++;
                }
            }
            if (db == 0)
            {
                Console.WriteLine("Nem volt beszelo.");
            }
            else
            {
                Console.WriteLine("A varakozok szama: {0} a beszelo a {1}. hivo", db - 1, hivasok[beszelo].ID);
            }
        }  

       private static void Utolso()
        {
            int i = N;
            while (i > 0 && hivasok[i].Kezd.mpTicks() == 0)
            {
                i--;
            }
            Console.WriteLine("Az utolso telefonalo adatai a(z) {0}.sorban vannak, {1} masodpercig vart.", hivasok[i].ID, hivasok[i].Kezd.mpTicks() - hivasok[i].Hiv.mpTicks());
        }

        private static void Output()
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter("sikeres.txt"))
            {
                foreach (Hivas h in hivasok)
                {
                    if (h.Kezd.mpTicks() != 0)
                    {
                        sw.WriteLine("{0} {1} {2} {3} {4} {5} {6}", h.ID, h.Kezd.Ora, h.Kezd.Perc, h.Kezd.MP, h.Bont.Ora, h.Bont.Perc, h.Bont.MP);
                    }
                }
            }
        }
    }
}
