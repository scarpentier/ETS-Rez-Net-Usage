<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RezNetUsage.Web.Default1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register src="Usercontrols/ErrorInvalidAppart.ascx" tagname="ErrorInvalidAppart" tagprefix="uc1" %>
<%@ OutputCache Duration="30" VaryByParam="phase; appart; mois" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Label ID="lblPhaseAppartMois" runat="server" Text="Consommation de l'appartement {1} de la phase {0} pour le mois {2}"></asp:Label> 
        [ <asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click">Changer</asp:LinkButton> ]
        <asp:Panel runat="server" ID="panelChangeAppart" Visible="false">
            Phase: 
            <asp:DropDownList ID="ddlPhase" runat="server">
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
                <asp:ListItem>3</asp:ListItem>
                <asp:ListItem>4</asp:ListItem>
            </asp:DropDownList>
            &nbsp;Appart: 
            <asp:TextBox ID="txtAppart" runat="server" Columns="4" MaxLength="4" 
                Width="50px"></asp:TextBox> 
            &nbsp;Mois: 
            <asp:DropDownList ID="ddlMois" runat="server">
                <asp:ListItem Value="0">En cours</asp:ListItem>
                <asp:ListItem Value="1">Janvier</asp:ListItem>
                <asp:ListItem Value="2">Février</asp:ListItem>
                <asp:ListItem Value="3">Mars</asp:ListItem>
                <asp:ListItem Value="4">Avril</asp:ListItem>
                <asp:ListItem Value="5">Mai</asp:ListItem>
                <asp:ListItem Value="6">Juin</asp:ListItem>
                <asp:ListItem Value="7">Juillet</asp:ListItem>
                <asp:ListItem Value="8">Aout</asp:ListItem>
                <asp:ListItem Value="9">Septembre</asp:ListItem>
                <asp:ListItem Value="10">Octobre</asp:ListItem>
                <asp:ListItem Value="11">Novembre</asp:ListItem>
                <asp:ListItem Value="12">Décembre</asp:ListItem>
            </asp:DropDownList>
            &nbsp;<asp:Button ID="btnChanger" runat="server" Text="Go!" 
                onclick="btnChanger_Click" />
        </asp:Panel>
    </ContentTemplate>
    </asp:UpdatePanel>
    <table width="1000px"><tr><td width="700px" valign="top">
    <uc1:ErrorInvalidAppart ID="ErrorInvalidAppart1" runat="server" Visible="false" />
    <asp:Panel ID="panelData" runat="server">
    <h2>Sommaire</h2>
               <table width="700px">
                <tr>
                    <td style="width: 250px;">Consommation totale permise</td>
                    <td style="width: 80px; text-align: right;"><asp:Label ID="lblMax" runat="server"></asp:Label></td>
                    <td rowspan="7" valign="top" align="center">

    <rsweb:ReportViewer ID="ReportSummaryPie" runat="server" Font-Names="Verdana" 
        Font-Size="8pt" InteractiveDeviceInfos="(Collection)" 
        WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" 
    ShowBackButton="False" ShowExportControls="False" ShowFindControls="False" 
    ShowPageNavigationControls="False" ShowPrintButton="False" 
    ShowRefreshButton="False" ShowZoomControl="False" Height="200px" Width="350px" ShowToolBar="False" 
                            SizeToReportContent="True" KeepSessionAlive="True" InteractivityPostBackMode="AlwaysAsynchronous" AsyncRendering="False">
        <LocalReport ReportPath="Reports\SummaryPie.rdlc">
        </LocalReport>
    </rsweb:ReportViewer>
                        </td>
                </tr>
                <tr>
                    <td class="smaller">Download total utilisé</td>
                    <td class="smaller" style="text-align: right;"><asp:Label ID="lblDownload" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="smaller">Upload total utilisé</td>
                    <td class="smaller" style="text-align: right;"><asp:Label ID="lblUpload" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td>Total de l'utilisation combinée</td>
                    <td style="text-align: right;">
                        <asp:Label ID="lblTotal" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="bold">Restant</td>
                    <td class="bold" style="text-align: right;">
                        <asp:Label ID="lblRestant" runat="server" ></asp:Label></td>
                </tr>
                </table>
                 <asp:Panel ID="panelPrevisions" runat="server">
                <h2>Prévisions</h2>
                <table>
                <tr>
                <td valign="top"> <%--Was 450 px--%>
                <asp:Label ID="lblPrevisionRatio" runat="server" Text="Label">Vous consommez <b>{0}%</b> de votre capacité journalière maximale moyenne. </asp:Label>
                <asp:Label ID="lblPrevisionUsage" runat="server" Text="Label">À ce rythme, vous consommerez <b>{0} GB {1}</b> de votre limite. </asp:Label>
                <asp:Label ID="lblPrevisionRecommandation" runat="server" Text="Label"><b>{0}</b> votre consommation ou {1}.</asp:Label>
                </td>
                <%--<td valign="top" align="center" width="280">
                <rsweb:ReportViewer ID="ReportForecastRatio" runat="server" Visible="False"
                         Font-Names="Verdana" Font-Size="8pt" Height="80px" Width="250"
                         InteractiveDeviceInfos="(Collection)" ShowBackButton="False" 
                         ShowDocumentMapButton="False" ShowExportControls="False" 
                         ShowFindControls="False" ShowPageNavigationControls="False" 
                         ShowPrintButton="False" ShowPromptAreaButton="False" ShowRefreshButton="False" 
                         ShowToolBar="False" ShowZoomControl="False" WaitMessageFont-Names="Verdana" 
                         WaitMessageFont-Size="14pt" SizeToReportContent="True" KeepSessionAlive="False" AsyncRendering="False" InteractivityPostBackMode="AlwaysAsynchronous">
                         <LocalReport ReportPath="Reports\ForecastRatio.rdlc">
                         </LocalReport>
                     </rsweb:ReportViewer></td>--%>
                </tr>
                </table>
                     
                     
                </asp:Panel>
                <h2>Utilisateurs</h2>
                    <rsweb:ReportViewer ID="ReportUsers" runat="server" 
        Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" 
        WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Height="200px" 
        ShowBackButton="False" ShowExportControls="False" ShowFindControls="False" 
        ShowPageNavigationControls="False" ShowPrintButton="False" 
        ShowRefreshButton="False" ShowZoomControl="False" Width="700px" 
            ShowToolBar="False" SizeToReportContent="True" InteractivityPostBackMode="AlwaysAsynchronous" KeepSessionAlive="True" AsyncRendering="False">
                        <LocalReport ReportPath="Reports\Users.rdlc">
                        </LocalReport>
    </rsweb:ReportViewer>
    <h2>Ce mois-ci</h2>
    <rsweb:ReportViewer ID="ReportThisMonth" runat="server" 
    Font-Names="Verdana" Font-Size="8pt" InteractiveDeviceInfos="(Collection)" 
    ShowBackButton="False" ShowExportControls="False" ShowFindControls="False" 
    ShowPageNavigationControls="False" ShowPrintButton="False" 
    ShowRefreshButton="False" ShowZoomControl="False" 
    WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="700px" 
            ShowToolBar="False" SizeToReportContent="True" KeepSessionAlive="True" InteractivityPostBackMode="AlwaysAsynchronous" AsyncRendering="False">
        <LocalReport ReportPath="Reports\ThisMonth.rdlc">
        </LocalReport>
    </rsweb:ReportViewer>
    </asp:Panel>
    </td>
    
    <td width="20px">&nbsp;</td>
    <td width="280px" valign="top" class="sidebar">
<!--      <h2>Navigation</h2>
      <ul>
        <li>
            <asp:HyperLink ID="hlMonthly" NavigateUrl="~/Monthly.aspx" runat="server">Statistiques du mois {0}</asp:HyperLink></li>
      </ul>-->
            <%--<h2>Message</h2>
            J'ai terminé l'ETS en avril 2012 et remet cet outil entre les mains des étudiants. Le <a href="https://github.com/scarpentier/ETS-Rez-Net-Usage">code source</a> est disponible et je continuerai de l'héberger jusqu'au <b>30 mai 2013</b>.--%>
      <h2>FAQ</h2>
        <h3>C'est quoi ça?</h3>
        Un outil pour vérifier la consommation d'internet dans les résidences qui fail moins que celle fournie par Cooptel.
        <h3>C'est fait par qui?</h3>
        <a href="http://spacebar.ca">Simon Carpentier</a>, un enthousiaste des Internets qui mène une vie normale malgré son bacc. en TI.
        <h3>Qu'est-ce qui se passe si je dépasse ma limite?</h3>
        Cooptel coupe le port de toutes les chambres de l'appart qui dépasse sa limite. C'est un script qui roule sur les switchs à toutes les heures.
        <h3>J'ai dépassé ma limite et je n'ai plus d'internet :(</h3>
        Pas de panique, vous pouvez acheter de la consommation supplémentaire en contactant le service à la clientèle de Cooptel (ouvert du lundi au vendredi, de 8h30 à 17h00) au 1-888-532-2667 (et faites le 3). Mentionnez que vous êtes étudiant dans les résidences de l'ETS, et vous pourrez acheter 25 GB supplémentaire pour 5$+tx, payable par carte de crédit.
        <h2>Contact</h2>
        <span class="side">Pour toute question, suggestion, commentaire, hate-mail, vous pouvez me contacter par courriel à <a href="mailto:simon.carpentier@spacebar.ca">simon.carpentier@spacebar.ca</a></span>
     <h2>Facebook</h2><iframe src="http://www.facebook.com/plugins/likebox.php?id=125460654139471&amp;width=280&amp;connections=10&amp;stream=false&amp;header=false&amp;height=255" scrolling="no" frameborder="0" style="border:none; overflow:hidden; width:280px; height:255px;" allowTransparency="true"></iframe>
     <h2>Meta</h2>
     <ul>
     <li><a href="http://www.facebook.com/note.php?note_id=121994317833046">Changelog</a></li>
     <li><a href="http://www.facebook.com/note.php?note_id=121993617833116">Roadmap</a></li>
     <li><a href="https://github.com/scarpentier/ETS-Rez-Net-Usage">Code source</a></li></ul>
    </td>
    </tr></table>
    
                    <div class="footer">
        Source: 
    <asp:HyperLink ID="hlSource" runat="server"></asp:HyperLink></div>
    <!--
    <asp:Label runat="server" ID="lblDebug"></asp:Label>
        -->
    <a href="https://github.com/scarpentier/ETS-Rez-Net-Usage"><img style="position: absolute; top: 0; right: 0; border: 0;" src="https://s3.amazonaws.com/github/ribbons/forkme_right_gray_6d6d6d.png" alt="Fork me on GitHub" /></a>
</asp:Content>