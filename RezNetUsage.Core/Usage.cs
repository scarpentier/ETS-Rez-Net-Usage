namespace RezNetUsage.Core {
    using System;

    public partial class Usage {

        /// <summary>
        /// La quantité totale de consommation en aval (download)
        /// </summary>
        public double TotalDownload
        { 
            get {
                if (this.Tables[0].Rows.Count == 0) return 0;
                return Double.Parse(Tables[0].Compute("Sum(Download)", String.Empty).ToString());
            }
        }

        /// <summary>
        /// La quantité totale de consommation en amont (upload)
        /// </summary>
        public double TotalUpload
        {
            get
            {
                if (this.Tables[0].Rows.Count == 0) return 0;
                return Double.Parse(Tables[0].Compute("Sum(Upload)", String.Empty).ToString());
            }
        }

        /// <summary>
        /// La quantité maximale de consommation permise pour le mois
        /// </summary>
        public double Maximum { get; internal set; }

        /// <summary>
        /// La quantité totale de consommation pour le mois
        /// </summary>
        public double Total
        {
            get {
                return TotalDownload + TotalUpload;
            }
        }

        /// <summary>
        /// La quantité restante à consommer avant épuisement
        /// </summary>
        public double Remaining
        {
            get
            {
                return Maximum - Total;
            }
        }

        /// <summary>
        /// L'utilisation moyenne par jour (total / jour du mois)
        /// </summary>
        public double UsageRate
        {
            get
            {
                return Total / DateTime.Today.Day;
            }
        }

        /// <summary>
        /// La consommation moyenne à faire à chaque jour du mois pour optimiser sa consommation à 100% d'utilisation
        /// </summary>
        public double MaximumUsageRate
        {
            get
            {
                return Maximum / DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);
            }
        }

        /// <summary>
        /// La consommation totale prévue pour la fin du mois au rythme de consommation actuel moyen
        /// </summary>
        public double ForecastUsage
        { 
            get
            {
                return UsageRate * DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);
            }
        }

        /// <summary>
        /// Le nombre de jours restant estimé d'utilisation au rythme actuel
        /// </summary>
        public int ForecastRemainingDays
        { 
            get
            {
                return int.Parse(Math.Floor(Remaining / UsageRate).ToString());
            }
        }

        /// <summary>
        /// Le ratio d'utilisation de la consommation maximale
        /// </summary>
        public double UsageRateRatio
        {
            get
            {
                return UsageRate / MaximumUsageRate;
            }
        }

        /// <summary>
        /// Le nombre de MB qu'il faut consommer par jour si on veut arriver à 100% d'utilisation à la fin du moins
        /// </summary>
        public double IdealUsage
        {
            get
            {
                return (Remaining / (DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month) - DateTime.Now.Day));
            }
        }
    }
}
