using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace RezNetUsage.Core
{
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
                return (foundrows.Any());
            } catch
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
            // TODO: Trouver un meilleur plan que d'utiliser la réflexion pour aller chercher un XML à chaque fois que quelqu'un demande une chambre!
            var _assembly = Assembly.GetExecutingAssembly();
            chambres.ReadXml(new StreamReader(_assembly.GetManifestResourceStream("RezNetUsage.Core.Chambres.xml")));

            return chambres;
        }

        public static Usage GetUsage(Chambres chambres, int mois)
        {
            var usage = new Usage();

            foreach (DataRow row in chambres.Tables[0].Rows)
            {
                Console.WriteLine(
                    "Téléchargement de l'utilisation de la chambre " + (string)row["Appartement"] + " de la phase " +
                    (string)row["Phase"]);
                try
                {
                    usage = new Usage().GetUsage(
                        int.Parse((string)row["Phase"]), int.Parse((string)row["Appartement"]), mois);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("FAILED");
                }
            }

            return usage;
        }
    }
}
