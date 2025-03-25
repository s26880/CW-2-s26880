// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Linq;

namespace ZarzadzanieKontenerami
{
    public enum ProductType
    {
        BANANY,
        CZEKOLADA,
        RYBA,
        MIĘSO,
        LODY,
        MROŻONA_PIZZA,
        SER,
        KIEŁBASA,
        MASŁO,
        JAJKA
    }

    public static class ProductTypeExtensions
    {
        public static double GetRequiredMinTemperature(this ProductType productType)
        {
            switch (productType)
            {
                case ProductType.BANANY: return 13.3;
                case ProductType.CZEKOLADA: return 18.0;
                case ProductType.RYBA: return 2.0;
                case ProductType.MIĘSO: return -15.0;
                case ProductType.LODY: return -18.0;
                case ProductType.MROŻONA_PIZZA: return -30.0;
                case ProductType.SER: return 7.2;
                case ProductType.KIEŁBASA: return 5.0;
                case ProductType.MASŁO: return 20.5;
                case ProductType.JAJKA: return 19.0;
                default: throw new ArgumentOutOfRangeException(nameof(productType), productType, null);
            }
        }
    }

    public interface IHazardNotifier
    {
        void NotifyHazard(string numerKontenera, string wiadomosc);
    }

    public class OverfillException : Exception
    {
        public OverfillException(string wiadomosc) : base(wiadomosc) { }
    }

    public class HazardOperationException : Exception
    {
        public HazardOperationException(string wiadomosc) : base(wiadomosc) { }
    }

    public abstract class Kontener
    {
        private static int globalCounter = 1;
        protected string numerSeryjny;
        protected double wagaWlasna;
        protected double maksPojemnosc;
        protected double aktualnyLadunek;
        public Kontener(string prefixTypu, double wagaWlasna, double maksPojemnosc)
        {
            this.numerSeryjny = GenerujNumerSeryjny(prefixTypu);
            this.wagaWlasna = wagaWlasna;
            this.maksPojemnosc = maksPojemnosc;
            this.aktualnyLadunek = 0.0;
        }
        private string GenerujNumerSeryjny(string prefixTypu)
        {
            return "KON-" + prefixTypu + "-" + (globalCounter++);
        }
        public string PobierzNumerSeryjny()
        {
            return numerSeryjny;
        }
        public double PobierzWageWlasna()
        {
            return wagaWlasna;
        }
        public double PobierzMaksPojemnosc()
        {
            return maksPojemnosc;
        }
        public double PobierzAktualnyLadunek()
        {
            return aktualnyLadunek;
        }
        public abstract void Zaladuj(double masa);
        public abstract void Rozladuj();
        public double PobierzCalkowitaWage()
        {
            return wagaWlasna + aktualnyLadunek;
        }
    }

    public class KontenerCiekly : Kontener, IHazardNotifier
    {
        private bool niebezpieczny;
        public KontenerCiekly(double wagaWlasna, double maksPojemnosc, bool niebezpieczny)
            : base("L", wagaWlasna, maksPojemnosc)
        {
            this.niebezpieczny = niebezpieczny;
        }
        public void NotifyHazard(string numerKontenera, string wiadomosc)
        {
            Console.Error.WriteLine("ZAGROŻENIE w " + numerKontenera + ": " + wiadomosc);
        }
        public override void Zaladuj(double masa)
        {
            double limit = niebezpieczny ? 0.5 * maksPojemnosc : 0.9 * maksPojemnosc;
            if (aktualnyLadunek + masa > limit)
            {
                NotifyHazard(numerSeryjny, "Przekroczono dopuszczalny limit załadunku.");
                throw new HazardOperationException("Niebezpieczna operacja załadunku: " + PobierzNumerSeryjny());
            }
            if (aktualnyLadunek + masa > maksPojemnosc)
            {
                throw new OverfillException("Przekroczono pojemność: " + PobierzNumerSeryjny());
            }
            aktualnyLadunek += masa;
        }
        public override void Rozladuj()
        {
            aktualnyLadunek = 0.0;
        }
        public bool CzyNiebezpieczny()
        {
            return niebezpieczny;
        }
    }

    public class KontenerGazowy : Kontener, IHazardNotifier
    {
        private double cisnienie;
        public KontenerGazowy(double wagaWlasna, double maksPojemnosc, double cisnienie)
            : base("G", wagaWlasna, maksPojemnosc)
        {
            this.cisnienie = cisnienie;
        }
        public void NotifyHazard(string numerKontenera, string wiadomosc)
        {
            Console.Error.WriteLine("ZAGROŻENIE w " + numerKontenera + ": " + wiadomosc);
        }
        public override void Zaladuj(double masa)
        {
            if (aktualnyLadunek + masa > maksPojemnosc)
            {
                NotifyHazard(numerSeryjny, "Przekroczono dopuszczalną ładowność.");
                throw new OverfillException("Przekroczono pojemność: " + PobierzNumerSeryjny());
            }
            aktualnyLadunek += masa;
        }
        public override void Rozladuj()
        {
            aktualnyLadunek = aktualnyLadunek * 0.05;
        }
        public double PobierzCisnienie()
        {
            return cisnienie;
        }
        public void UstawCisnienie(double cisnienie)
        {
            this.cisnienie = cisnienie;
        }
    }

    public class KontenerChlodniczy : Kontener
    {
        private double temperatura;
        private ProductType produkt;
        public KontenerChlodniczy(double wagaWlasna, double maksPojemnosc, ProductType produkt, double poczatkowaTemperatura)
            : base("C", wagaWlasna, maksPojemnosc)
        {
            this.produkt = produkt;
            UstawTemperature(poczatkowaTemperatura);
        }
        public void UstawTemperature(double nowaTemperatura)
        {
            double wymagana = produkt.GetRequiredMinTemperature();
            if (nowaTemperatura < wymagana)
            {
                throw new HazardOperationException("Temperatura zbyt niska dla " + produkt.ToString());
            }
            this.temperatura = nowaTemperatura;
        }
        public override void Zaladuj(double masa)
        {
            if (aktualnyLadunek + masa > maksPojemnosc)
            {
                throw new OverfillException("Przekroczono pojemność: " + PobierzNumerSeryjny());
            }
            aktualnyLadunek += masa;
        }
        public override void Rozladuj()
        {
            aktualnyLadunek = 0.0;
        }
        public double PobierzTemperature()
        {
            return temperatura;
        }
        public ProductType PobierzProdukt()
        {
            return produkt;
        }
    }

    public class StatekKontenerowy
    {
        private string nazwa;
        private double maksPredkosc;
        private int maksLiczbaKontenerow;
        private double maksCiezar;
        private List<Kontener> kontenery;
        public StatekKontenerowy(string nazwa, double maksPredkosc, int maksLiczbaKontenerow, double maksCiezarWTonach)
        {
            this.nazwa = nazwa;
            this.maksPredkosc = maksPredkosc;
            this.maksLiczbaKontenerow = maksLiczbaKontenerow;
            this.maksCiezar = maksCiezarWTonach;
            kontenery = new List<Kontener>();
        }
        public string PobierzNazwe()
        {
            return nazwa;
        }
        private bool CzyMoznaZaladuj(Kontener kontener)
        {
            if (kontenery.Count >= maksLiczbaKontenerow)
                return false;
            double calkowitaWagaKg = kontenery.Sum(k => k.PobierzCalkowitaWage());
            calkowitaWagaKg += kontener.PobierzCalkowitaWage();
            double maksWagaKg = maksCiezar * 1000.0;
            return calkowitaWagaKg <= maksWagaKg;
        }
        public bool ZaladujKontener(Kontener kontener)
        {
            if (!CzyMoznaZaladuj(kontener))
            {
                Console.WriteLine("Nie można załadować " + kontener.PobierzNumerSeryjny() + " na " + nazwa);
                return false;
            }
            kontenery.Add(kontener);
            Console.WriteLine("Załadowano " + kontener.PobierzNumerSeryjny() + " na " + nazwa);
            return true;
        }
        public void ZaladujKontenery(List<Kontener> listaKontenerow)
        {
            foreach (Kontener k in listaKontenerow)
            {
                ZaladujKontener(k);
            }
        }
        public bool UsunKontener(string numerSeryjny)
        {
            int przed = kontenery.Count;
            kontenery.RemoveAll(k => k.PobierzNumerSeryjny() == numerSeryjny);
            return kontenery.Count < przed;
        }
        public bool ZamienKontener(string staryNumerSeryjny, Kontener nowyKontener)
        {
            for (int i = 0; i < kontenery.Count; i++)
            {
                if (kontenery[i].PobierzNumerSeryjny() == staryNumerSeryjny)
                {
                    Kontener usuniety = kontenery[i];
                    kontenery.RemoveAt(i);
                    if (CzyMoznaZaladuj(nowyKontener))
                    {
                        kontenery.Insert(i, nowyKontener);
                        Console.WriteLine("Zastąpiono " + staryNumerSeryjny + " kontenerem " + nowyKontener.PobierzNumerSeryjny());
                        return true;
                    }
                    else
                    {
                        kontenery.Insert(i, usuniety);
                        Console.WriteLine("Nie można zastąpić " + staryNumerSeryjny);
                        return false;
                    }
                }
            }
            return false;
        }
        public bool PrzeniesKontener(string numerSeryjny, StatekKontenerowy docelowyStatek)
        {
            foreach (Kontener k in kontenery)
            {
                if (k.PobierzNumerSeryjny() == numerSeryjny)
                {
                    if (docelowyStatek.ZaladujKontener(k))
                    {
                        UsunKontener(numerSeryjny);
                        Console.WriteLine("Przeniesiono " + numerSeryjny + " z " + this.nazwa + " na " + docelowyStatek.PobierzNazwe());
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Nie można przenieść " + numerSeryjny);
                        return false;
                    }
                }
            }
            Console.WriteLine("Nie znaleziono " + numerSeryjny + " na " + nazwa);
            return false;
        }
        public void WyswietlInformacje()
        {
            Console.WriteLine("=== " + nazwa + " ===");
            Console.WriteLine("Prędkość: " + maksPredkosc);
            Console.WriteLine("Maksymalna liczba kontenerów: " + maksLiczbaKontenerow);
            Console.WriteLine("Maksymalny ciężar (t): " + maksCiezar);
            Console.WriteLine("Obecna liczba kontenerów: " + kontenery.Count);
            double calkowitaWagaKg = kontenery.Sum(k => k.PobierzCalkowitaWage());
            Console.WriteLine("Łączny ciężar kontenerów (kg): " + calkowitaWagaKg);
            foreach (Kontener k in kontenery)
            {
                Console.WriteLine("- " + k.PobierzNumerSeryjny() + " | " + k.PobierzCalkowitaWage() + " kg");
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                KontenerCiekly mlecznyKontener = new KontenerCiekly(200.0, 1000.0, false);
                KontenerCiekly paliwowyKontener = new KontenerCiekly(300.0, 2000.0, true);
                KontenerGazowy helowyKontener = new KontenerGazowy(150.0, 500.0, 10.0);
                KontenerChlodniczy bananowyKontener = new KontenerChlodniczy(250.0, 800.0, ProductType.BANANY, 14.0);
                mlecznyKontener.Zaladuj(500.0);
                paliwowyKontener.Zaladuj(900.0);
                helowyKontener.Zaladuj(400.0);
                bananowyKontener.Zaladuj(600.0);
                StatekKontenerowy statekA = new StatekKontenerowy("Atlantic Express", 25.0, 3, 10.0);
                StatekKontenerowy statekB = new StatekKontenerowy("Pacific Runner", 20.0, 5, 20.0);
                statekA.ZaladujKontener(mlecznyKontener);
                statekA.ZaladujKontener(paliwowyKontener);
                statekA.ZaladujKontener(helowyKontener);
                statekA.ZaladujKontener(bananowyKontener);
                statekA.WyswietlInformacje();
                statekB.WyswietlInformacje();
                statekA.PrzeniesKontener(helowyKontener.PobierzNumerSeryjny(), statekB);
                statekB.ZaladujKontener(bananowyKontener);
                statekA.WyswietlInformacje();
                statekB.WyswietlInformacje();
                statekA.ZamienKontener(paliwowyKontener.PobierzNumerSeryjny(), bananowyKontener);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Błąd: " + e.Message);
            }
        }
    }
}
