using System;

namespace RezNetUsage.DataFetcher
{
    using Core;

    class Program
    {
        /// <summary>
        /// Numéro du mois à downloader
        /// </summary>
        private const int Mois = 6; // juin

        static void Main(string[] args)
        {
            Console.WriteLine("Déterminer la liste des chambres...");
            Chambres chambres = AppartHelper.GetChambres();

            Console.WriteLine("Télécharger l'utilisation des chambres...");
            Usage usage = AppartHelper.GetUsage(chambres, Mois);

            Console.WriteLine("Sauvegarder l'information dans un fichier XML...");
            string filename = "20110" + (Mois >= 10 ? Mois.ToString() : "0" + Mois.ToString()) + " .xml";
            usage.WriteXml(filename);
        }
    }
}
