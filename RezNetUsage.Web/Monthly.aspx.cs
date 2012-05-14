using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RezNetUsage.Core;

namespace RezNetUsage.Web
{
    using Microsoft.Reporting.WebForms;

    public partial class Monthly : System.Web.UI.Page
    {
        /// <summary>
        /// String static pour convertir un index de mois en string de mois
        /// </summary>
        private static readonly string[] Mois = { "de janvier", "de février", "de mars", "d'avril", "de mai", "de juin", "de juillet", "d'août", "de septembre", "d'octobre", "de novembre", "de décembre" };

        private int _mois;

        private Usage _usage;

        private Chambres _chambres;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Rien faire si c'est un postback
            if (IsPostBack) return;

            // Vérifier que le mois spécifié est correct
            this.ParseMois(Request.QueryString["mois"]);

            // Loader le dataset des chambres
            _chambres = AppartHelper.GetChambres();
            
            // Loader le dataset de la consommation
            _usage = new Usage();
            string year = (_mois > DateTime.Now.Month ? DateTime.Now.Year - 1 : DateTime.Now.Year).ToString();
            string month = (_mois < 10 ? "0" + _mois : _mois.ToString());
            _usage.ReadXml(this.MapPath("App_Data/" + year + month + ".xml"));

            lblPhaseAppartMois.Text = String.Format(lblPhaseAppartMois.Text, Mois[_mois - 1]);

            // Charger l'information et les rapports
            LoadSummary();
            LoadPhases();
            LoadHallOfFame();
        }

        private void LoadHallOfFame()
        {
            this.reportHallOfFame.LocalReport.DataSources.Clear();
            this.reportHallOfFame.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", _usage.Tables[0]));
            this.reportHallOfFame.LocalReport.Refresh();
        }

        private void LoadPhases()
        {
            this.reportPhases.LocalReport.DataSources.Clear();
            this.reportPhases.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", _usage.Tables[0]));
            this.reportPhases.LocalReport.Refresh();
        }

        private void LoadSummary()
        {
            // Afficher un peu de data
            lblDownload.Text = Math.Round(_usage.TotalDownload / 1024, 0) + " GB";
            lblUpload.Text = Math.Round(_usage.TotalUpload / 1024, 0) + " GB";
            lblTotal.Text = Math.Round(_usage.Total / 1024, 0) + " GB";
            lblMax.Text = Math.Round(_chambres.MaximumSum / 1024, 0) + " GB";
            lblRestant.Text = Math.Round((_chambres.MaximumSum - _usage.Total) / 1024, 0) + " GB";

            this.ReportSummaryPie.LocalReport.DataSources.Clear();
            this.ReportSummaryPie.LocalReport.DataSources.Add(new ReportDataSource("DataSet1_DataTable1", _usage.Tables[0]));

            var paramList = new List<ReportParameter>();
            paramList.Add(new ReportParameter("Max", _chambres.MaximumSum.ToString(), false));

            this.ReportSummaryPie.LocalReport.SetParameters(paramList);
            this.ReportSummaryPie.LocalReport.Refresh();
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            panelChangeMois.Visible = true;
        }

        protected void btnChanger_Click(object sender, EventArgs e)
        {
            Response.Redirect("Monthly.aspx?mois=" + ddlMois.SelectedValue);
        }

        private void ParseMois(string mois)
        {
            // Le mois doit être spécifié
            if (String.IsNullOrEmpty(mois))
            {
                mois = (DateTime.Now.AddMonths(-1)).ToString();
                //Response.Redirect("Monthly.aspx?mois=" + (DateTime.Now.Month - 1));
            }

            // Le mois doit être un int entre 1 et 12
            if (!int.TryParse(mois, out _mois))
            {
                Response.Redirect("Monthly.aspx?mois=" + (DateTime.Now.AddMonths(-1).Month));
            }
            else if (_mois < 1 || _mois > 12)
            {
                Response.Redirect("Monthly.aspx?mois=" + (DateTime.Now.AddMonths(-1).Month));
            }
            else
            {
                ddlMois.SelectedValue = mois;
            }
        }
    }
}