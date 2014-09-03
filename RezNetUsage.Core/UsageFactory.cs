using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace RezNetUsage.Core
{
    using System.Text.RegularExpressions;

    public static class UsageFactory
    {
        /// <summary>
        /// Regex pour définir l'utilisation
        /// </summary>
        private readonly static Regex RegexUsage =
        new Regex("<TR><TD>(.*)</TD><TD>(.*)</TD><TD ALIGN=\"RIGHT\">(.*)</TD><TD ALIGN=\"RIGHT\">(.*)</TD></TR>");

        /// <summary>
        /// Regex pour définir l'utilisation maximale permise
        /// </summary>
        private static readonly Regex RegexMax =
            new Regex("<TD>Quota permis pour la p&eacute;riode</TD><TD ALIGN=\"RIGHT\">(.*)</TD></TD></TR>");

        /// <summary>
        /// Le parsing des doubles de cooptel dépendent de la culture locale
        /// </summary>
        private static readonly CultureInfo Culture = CultureInfo.CreateSpecificCulture("en-CA");

        /// <summary>
        /// Ajoute les informations de la consommation internet pour un appart dans un objet Usage donné
        /// </summary>
        /// <param name="usage"></param>
        /// <param name="phase"></param>
        /// <param name="appart"></param>
        /// <param name="mois"></param>
        /// <returns>Objet usage plein de data</returns>
        public static Usage GetUsage(this Usage usage, int phase, int appart, int mois)
        {
            // Les numero d'appartement qui ont seulement 3 chiffres est sous le format 0111 et non 111. Who knows why.
            var appartFormatte = phase == 3 ? appart.ToString("0000") : appart.ToString();

            // Build query string
            // http://ets-res2-772:ets772@www2.cooptel.qc.ca/services/temps/?mois=9&cmd=Visualiser
            var query = string.Format("http://www2.cooptel.qc.ca/services/temps/?mois={0}&cmd=Visualiser", mois);
            var user = string.Format("ets-res{0}-{1}", phase, appartFormatte);
            var pass = string.Format("ets{0}", appart);

            string html;
            try
            {
                // Fetch data into a string
                html = NetHelper.DownloadHtml(query, user, pass);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not download html:" + ex.Message);
            }

            // Match du regeex
            MatchCollection mc = RegexUsage.Matches(html);
            usage.Maximum = Math.Round(Double.Parse(RegexMax.Matches(html)[0].Groups[1].Value, Culture), 0);

            // Objets temporaires
            for (var i = 0; i <= mc.Count - 1; i++)
            {
                string port = mc[i].Groups[1].Value;

                // Cooptel affiche "Journée en cours" à la place de la date d'aujourd'hui
                DateTime day;
                if (mc[i].Groups[2].Value.Contains("en cours"))
                {
                    day = DateTime.UtcNow.AddHours(-5); // Il faut se synroniser sur le fuseau horaire de Cooptel pour savoir quand la journée change pour eux. J'ai aucune idée si ils prennent en compte DST
                }
                else
                {
                    day = DateTime.Parse(mc[i].Groups[2].Value, Culture);
                }

                var download = Double.Parse(mc[i].Groups[4].Value.Trim(' '), Culture);
                var upload = Double.Parse(mc[i].Groups[3].Value.Trim(' '), Culture);

                usage.Tables[0].Rows.Add(phase, appart, port, day, download, upload);
            }

            //// Hey! Cette chambre existe! On l'ajoute à la liste!
            //ChambresHelper ch = new ChambresHelper();
            //ch.AjouterChambre(phase, appart, int.Parse(max.ToString()));

            //// Save usageData
            //string path = ConfigurationManager.AppSettings["MapPath"] + "\\App_Data\\cache\\2008" +
            //     ((mois.Length == 1) ? "0" + mois : mois) + "-" + phase + "-" + appart + ".xml";
            //usageData.WriteXml(path);

            return usage;
        }
    }
}
