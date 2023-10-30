<%@ Page Title="Add Referral" Language="C#" MasterPageFile="~/GCWebUsabilityTheme/MasterPages/GCWebUsability.master" AutoEventWireup="true" CodeBehind="AddReferral.aspx.cs" Inherits="IJPReporting.CreateForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="upAlert" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <wet:WetAlert ID="alert" CssClass="mrgn-tp-md" runat="server" EnableViewState="false" Visible="false"></wet:WetAlert>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel runat="server" class="mrgn-tp-md">
        <ContentTemplate>
            <div class="panel panel-default">
                <div class="panel-heading">
                    <asp:Label CssClass="h2" runat="server">Add Referral</asp:Label>              
                </div>
                <div class="mrgn-tp-lg mrgn-lft-md mrgn-rght-md">
                    <wet:WetDropDownList ID="formsTypes" runat="server" LabelText="Service/Activity Type" IsRequired="true"></wet:WetDropDownList>
                    <wet:WetTextBox ID="clientFileNumber" runat="server" LabelText="Client file number" IsRequired="true"></wet:WetTextBox>
                    <wet:WetTextBox ID="refDate" runat="server" LabelText="Referral date" IsRequired="true" IsDate="true"></wet:WetTextBox>
                    <wet:WetDropDownList ID="Communities" runat="server" IsRequired="true" LabelText="Recipient" AutoPostBack="true" OnSelectedIndexChanged="Communities_SelectedIndexChanged"></wet:WetDropDownList>
                    <wet:WetDropDownList ID="Programs" runat="server" IsRequired="true" LabelText="Program"></wet:WetDropDownList>
                    
                </div>     
                <div class="mrgn-lft-md mrgn-bttm-md">
                    <wet:WetLinkButton PostBackUrl="ClientReferralReporting.aspx" runat="server"><asp:Label runat="server">Back</asp:Label></wet:WetLinkButton>            
                    <wet:WetButton ID="goToWizard" runat="server" Text="Add" OnClick="goToWizard_Click" ButtonType="Primary" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>   
</asp:Content>
