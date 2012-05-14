<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Monthly.aspx.cs" Inherits="RezNetUsage.Web.Monthly" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ OutputCache Duration="3600" VaryByParam="mois" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Label ID="lblPhaseAppartMois" runat="server" Text="Consommation de globale des résidences pour le mois {0}"></asp:Label> 
        [ <asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click">Changer</asp:LinkButton> ]
        <asp:Panel runat="server" ID="panelChangeMois" Visible="false">
            Mois: 
            <asp:DropDownList ID="ddlMois" runat="server">
                <asp:ListItem Value="4">Avril</asp:ListItem>
                <asp:ListItem Value="5">Mai</asp:ListItem>
                <asp:ListItem Value="6">Juin</asp:ListItem>
                <asp:ListItem Value="7">Juillet</asp:ListItem>
                <asp:ListItem Value="8">Août</asp:ListItem>
                <asp:ListItem Value="9">Septembre</asp:ListItem>
                <asp:ListItem Value="10">Octobre</asp:ListItem>
                <asp:ListItem Value="11">Novembre</asp:ListItem>
            </asp:DropDownList>
            &nbsp;<asp:Button ID="btnChanger" runat="server" Text="Go!" 
                onclick="btnChanger_Click" />
        </asp:Panel>
    </ContentTemplate>
    </asp:UpdatePanel>

<table width="1000px"><tr><td width="750px" valign="top">
<h2>Statistiques globales</h2>
    <table width="750px">
                <tr>
                    <td style="width: 225px;">Consommation totale permise</td>
                    <td style="width: 120px; text-align: right;"><asp:Label ID="lblMax" runat="server"></asp:Label></td>
                    <td rowspan="7" valign="top" align="center">

    <rsweb:ReportViewer ID="ReportSummaryPie" runat="server" Font-Names="Verdana" 
        Font-Size="8pt" InteractiveDeviceInfos="(Collection)" 
        WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" 
    ShowBackButton="False" ShowExportControls="False" ShowFindControls="False" 
    ShowPageNavigationControls="False" ShowPrintButton="False" 
    ShowRefreshButton="False" ShowZoomControl="False" Height="200px" Width="350px" ShowToolBar="False" 
                            AsyncRendering="False" KeepSessionAlive="False">
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
    
    
    <h2>Utilisation par phase</h2>
    <rsweb:ReportViewer ID="reportPhases" runat="server" Font-Names="Verdana" 
        Font-Size="8pt" InteractiveDeviceInfos="(Collection)" ShowToolBar="False" 
        WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="750px" 
        Height="250px" AsyncRendering="False" KeepSessionAlive="False">
        <LocalReport ReportPath="Reports\Global\Phases.rdlc">
        </LocalReport>
    </rsweb:ReportViewer>
    </td><td width="250" valign="top">
    <h2>Navigation</h2>
    <ul><li><a href="./">Accueil</a></li></ul>
    <h2>Hall of fame</h2>
    <rsweb:ReportViewer ID="reportHallOfFame" runat="server" Font-Names="Verdana" 
        Font-Size="8pt" InteractiveDeviceInfos="(Collection)" ShowToolBar="False" 
        WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="250px" 
            Height="415px" AsyncRendering="False" KeepSessionAlive="False">
        <LocalReport ReportPath="Reports\Global\HallOfFame.rdlc">
        </LocalReport>
    </rsweb:ReportViewer>
    </td></tr></table>
</asp:Content>
