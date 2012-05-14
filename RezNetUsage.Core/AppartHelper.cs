using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RezNetUsage.Core
{
    using System.Data;
    using System.IO;
    using System.Reflection;

    public static class AppartHelper
    {
        public static bool IsAppartExist(int phase, int appart)
        {
            try {
                // Instancier dataset Chambres
                var chambres = GetChambres();

                // Vérifier la présence de la chambre
                DataRow[] foundrows = chambres.Tables[0].Select("Phase = " + phase + " AND Appartement = " + appart);

                // Retourner le résultat
                return (foundrows.Count() == 1);
            } catch (Exception ex)
            {
                return false;
            }
        }

        public static void RandomAppart(out int phase, out int appart)
        {
            // Instancier dataset Chambres
            var chambres = GetChambres();

            // Get random row
            var rand = new Random();
            DataRow dr = chambres.Tables[0].Rows[rand.Next(0, chambres.Tables[0].Rows.Count)];

            phase = int.Parse((string)dr["Phase"]);
            appart = int.Parse((string)dr["Appartement"]);
        }

        public static Chambres GetChambres()
        {
            // Instancier dataset Chambres
            var chambres = new Chambres();

            // Loader le data dans le dataset
            var _assembly = Assembly.GetExecutingAssembly();
            chambres.ReadXml(new StreamReader(_assembly.GetManifestResourceStream("RezNetUsage.Core.Chambres.xml")));

            return chambres;
        }

        public static Usage GetUsage(Chambres chambres, int mois)
        {
            Usage usage = new Usage();

            foreach (DataRow row in chambres.Tables[0].Rows)
            {
                Console.WriteLine(
                    "Téléchargement de l'utilisation de la chambre " + (string)row["Appartement"] + " de la phase " +
                    (string)row["Phase"]);
                try
                {
                    usage = UsageFactory.GetUsage(
                        usage, int.Parse((string)row["Phase"]), int.Parse((string)row["Appartement"]), mois);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("FAILED");
                }
            }

            return usage;
        }

        public static Usage GetUsage(int phase, int appart, int mois)
        {
            return UsageFactory.GetUsage(phase, appart, mois);
        }
    }
}
