using System.Data;

namespace RezNetUsage.Web
{
    using System.Globalization;
    using System;
    using System.Collections.Generic;
    using RezNetUsage.Core;

    using Microsoft.Reporting.WebForms;

    public partial class Default1 : System.Web.UI.Page
    {
        /// <summary>
        /// Objet Usage qui est en fait un DataSet avec des méthodes de plus pour calculer des ratios plus facilement
        /// </summary>
        private Usage _usage = null;

        /// <summary>
        /// La phase dans laquelle se trouve l'appart
        /// </summary>
        private int _phase = 2;

        /// <summary>
        /// L'appart dans laquelle vérifier la consommation
        /// </summary>
        private int _appart = 772;

        /// <summary>
        /// Le mois pour lequel on vérifie la consommation
        /// </summary>
        private int _mois;

        /// <summary>
        /// String static pour convertir un index de mois en string de mois
        /// </summary>
        private static readonly string[] Mois = { "de janvier", "de février", "de mars", "d'avril", "de mai", "de juin", "de juillet", "d'août", "de septembre", "d'octobre", "de novembre", "de décembre" };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            bool isok = ParseAppartParameters(
                this.Request.QueryString["phase"], this.Request.QueryString["appart"], this.Request.QueryString["mois"]);

            // Est-ce que tout est beau?
            if (!isok)
            {
                panelData.Visible = false;
                ErrorInvalidAppart1.Visible = true;
                lblPhaseAppartMois.ForeColor = System.Drawing.Color.Red;
                lblPhaseAppartMois.Text = "Veuillez entrer votre numéro d'appart.";
                ddlPhase.SelectedValue = "2";
                txtAppart.Text = "772";
                ddlMois.SelectedValue = "0";
                panelChangeAppart.Visible = true;
                return;
            }

            try {

                // Aller chercher l'utilisation
                _usage = UsageFactory.GetUsage(_phase, _appart, _mois);

                // Mettre les paramètres dans les contrôles
                lblPhaseAppartMois.Text = String.Format(lblPhaseAppartMois.Text, _phase, _appart, Mois[_mois - 1]);
                if (_mois > DateTime.Now.Month) lblPhaseAppartMois.Text += " " + DateTime.Now.AddYears(-1).Year; // Ajouter l'année
                
                hlMonthly.Text = String.Format(hlMonthly.Text, Mois[DateTime.Now.AddMonths(-1).Month - 1]);

                ddlPhase.SelectedValue = _phase.ToString();
                txtAppart.Text = _appart.ToString();
                ddlMois.SelectedValue = (Request.QueryString["mois"] != null) ? _mois.ToString() : "0"; // Remettre "mois en cours" s'il n'est pas spécifié dans le QueryString

                // Loader les rapports
                this.LoadPageData();
                this.LoadReportSummaryPie();
                this.LoadForecast();
                this.LoadUsers();
                this.LoadThisMonth();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Error while downloading HTML"))
                {
                    lblPhaseAppartMois.Text =
                        "Le petit hamster de Cooptel fait la grève parce que le gros méchant site est pas gentil avec lui.";
                } else
                {
                    lblPhaseAppartMois.Text = "Oops! Quelque chose d'étrange c'est produit: " + ex.Message;
                    lblDebug.Text = ex.ToString();
                }

                lblPhaseAppartMois.ForeColor = System.Drawing.Color.Red;
                
                panelData.Visible = false;
                panelChangeAppart.Visible = true;
                ddlPhase.SelectedValue = "2";
                txtAppart.Text = "772";
            }
        }

        /// <summary>
        /// Charge les prévision
        /// </summary>
        private void LoadForecast()
        {
            if (DateTime.Now.Month != _mois)
            {
                panelPrevisions.Visible = false;
                return;
            }

            ////this.ReportForecastRatio.LocalReport.DataSources.Clear();
            //this.ReportForecastRatio.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", new DataTable()));

            ////var paramList = new List<ReportParameter> { new ReportParameter("Ratio", _usage.UsageRateRatio.ToString(), false) };

            //this.ReportForecastRatio.LocalReport.SetParameters(new ReportParameter("Ratio", _usage.UsageRateRatio.ToString())); ;
            ////this.ReportForecastRatio.LocalReport.Refresh();

            bool depasse = (_usage.Maximum < _usage.ForecastUsage);

            lblPrevisionRatio.Text = String.Format(lblPrevisionRatio.Text, Math.Round(_usage.UsageRateRatio * 100,0));
            lblPrevisionUsage.Text = String.Format(
                lblPrevisionUsage.Text,
                Math.Round(Math.Abs(_usage.Maximum - _usage.ForecastUsage) / 1024, 0),
                depasse ? "au-dessus" : "en dessous");

            if (_usage.UsageRateRatio > 0.6 && _usage.UsageRateRatio < 1)
            {
                lblPrevisionRecommandation.Text = "";
            } else {
            lblPrevisionRecommandation.Text = String.Format(lblPrevisionRecommandation.Text, 
                depasse ? "Diminuez" : "Augmentez",
                depasse ? "appellez Cooptel parce qu'autrement, vous aurez de l'internet jusqu'au <b>" + DateTime.Now.AddDays(_usage.ForecastRemainingDays).ToString("d MMMM", CultureInfo.CreateSpecificCulture("fr-CA")) + "</b>" : "allez voir votre voisin parce que présentement, <b>vous sous-utilisez votre connexion</b>"
                );
            }

            // Si c'est le début du mois, on a pte pas assé de data pour afficher des choses
            const int JoursAvantPrevision = 3;

            if (_mois == DateTime.Now.Month && DateTime.Now.Day <= JoursAvantPrevision) // Pas de prévisions avant 3 jours
            {
                //lblPrevisionUsage.Visible = false;
                lblPrevisionUsage.Text = "Il n'y a <b>pas assez de données</b> recueillies pour faire une prévision.";
                lblPrevisionRecommandation.Text = String.Format("Revenez le {0} {1}.", JoursAvantPrevision + 1, DateTime.Now.ToString("MMMM"));
            }

            // Est-ce que l'utilisateur a déjà dépassé sa limite?
            if (_usage.Total > _usage.Maximum)
            {
                lblPrevisionRecommandation.Text =
                    "Vous avez <b>déjà dépassé</b> votre limite. Si vous avez toujours de l'internet, votre port sera coupé d'ici une heure. <b>Contactez le service à la clientèle</b> de Cooptel si vous voulez réactiver le service.";
                lblPrevisionUsage.Visible = false;
            }
        }

        /// <summary>
        /// Charge le rapport mensuel
        /// </summary>
        private void LoadThisMonth()
        {
            // La phase 3, ils sont "spécial" (Cooptel affiche les ports avec un string différent, on utilise un autre rapport)
            if (_phase == 3) this.ReportThisMonth.LocalReport.ReportPath = this.MapPath("Reports/ThisMonth3.rdlc");

            this.ReportThisMonth.LocalReport.DataSources.Clear();
            this.ReportThisMonth.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", _usage.Tables[0]));
            this.ReportThisMonth.LocalReport.Refresh();
        }

        /// <summary>
        /// Charge le rapport des utilisateurs
        /// </summary>
        private void LoadUsers()
        {
            // La phase 3, ils sont "spécial" (Cooptel affiche les ports avec un string différent, on utilise un autre rapport)
            if (_phase == 3) this.ReportUsers.LocalReport.ReportPath = this.MapPath("Reports/Users3.rdlc");

            this.ReportUsers.LocalReport.DataSources.Clear();
            this.ReportUsers.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", _usage.Tables[0]));
            this.ReportUsers.LocalReport.Refresh();
        }

        /// <summary>
        /// Charge le rapport sommaire
        /// </summary>
        private void LoadPageData()
        {
            // Le maximum est seulement disponible pour le mois en cours
            if (_mois == DateTime.Now.Month)
            {
                lblMax.Text = Math.Round(_usage.Maximum / 1024, 2) + " GB";
                lblRestant.Text = Math.Round(_usage.Remaining / 1024, 2) + " GB";
            } else
            {
                lblMax.Text = "";
                lblRestant.Text = "";
            }
   
            // Afficher un peu de data
            lblDownload.Text = Math.Round(_usage.TotalDownload / 1024, 2) + " GB";
            lblUpload.Text = Math.Round(_usage.TotalUpload / 1024, 2) + " GB";
            lblTotal.Text = Math.Round(_usage.Total / 1024, 2) + " GB";
            
            // Formatter l'url "source" en bas de la page
            string url = "http://ets-res" + _phase + "-" + _appart + ":ets" + _appart +
                            "@www2.cooptel.qc.ca/services/temps/?mois=" + _mois + "&cmd=Visualiser";
            hlSource.NavigateUrl = url;
            hlSource.Text = url;
        }

        /// <summary>
        /// Charge le graphique du rapport sommaire
        /// </summary>
        private void LoadReportSummaryPie()
        {
            this.ReportSummaryPie.LocalReport.DataSources.Clear();
            this.ReportSummaryPie.LocalReport.DataSources.Add(new ReportDataSource("DataSet1_DataTable1", _usage.Tables[0]));

            var paramList = new List<ReportParameter>();
            paramList.Add(new ReportParameter("Max", _usage.Maximum.ToString(), false));

            this.ReportSummaryPie.LocalReport.SetParameters(paramList);
            this.ReportSummaryPie.LocalReport.Refresh();
        }

        /// <summary>
        /// Rend visible le panel pour modifier la chambre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            panelChangeAppart.Visible = true;
        }

        /// <summary>
        /// Redirige vers la bonne page en lien avec le contenu des contrôles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnChanger_Click(object sender, EventArgs e)
        {
            if (ddlMois.SelectedValue == "0")
            {
                Response.Redirect(String.Format("?phase={0}&appart={1}", ddlPhase.SelectedValue, txtAppart.Text));   
            } else
            {
                Response.Redirect(String.Format("?phase={0}&appart={1}&mois={2}", ddlPhase.SelectedValue, txtAppart.Text, ddlMois.SelectedValue));   
            }
        }

        private bool ParseAppartParameters(string phase, string appart, string mois)
        {
            int phaseout;
            int appartout;
            int moisout;

            // Par défaut, le mois est le mois en cours
            _mois = DateTime.Now.Month;

            // Est-ce que le mois est spécifié ou valide?
            if (int.TryParse(mois, out moisout))
            {
                // Est-ce que le mois est une valeur entre 1 et 12?
                if (moisout >= 1 && moisout <= 12) 
                {
                    // Le mois est valide
                    _mois = moisout;
                }
            }

            // Est-ce que phase / appart est spécifié?
            if (String.IsNullOrEmpty(phase) || String.IsNullOrEmpty(appart))
            {
                // Phase / appart n'est pas spécifié, on va en prendre un au hasard
                AppartHelper.RandomAppart(out _phase, out _appart);
                return true;
            }

            // Est-ce que phase / appart est convertible en int?
            if (int.TryParse(phase, out phaseout) && int.TryParse(appart, out appartout))
            {
                // Est-ce que phase / appart existe?
                if (AppartHelper.IsAppartExist(phaseout, appartout) || phaseout == 4) // HACK: Permettre la nouvelle phase 4 sans savoir quelles sont les apparts possibles. Ça se peut que ça brise des choses. Oh well.
                {
                    // L'appart existe, tout est OK
                    _phase = phaseout;
                    _appart = appartout;
                    return true;
                } else
                {
                    // L'appart est spécifié mais n'existe pas, on retourne une erreur
                    return false;
                }
            }
            else
            {
                // L'appart est spécifié mais n'est pas valide (convertible en int), on retourne une erreur
                return false;
            }
        }
    }
}