<%@ Page Title="" Language="C#" MasterPageFile="~/GCWebUsabilityTheme/MasterPages/GCWebUsability.master" AutoEventWireup="true" CodeBehind="ManageUser.aspx.cs" Inherits="IJPReporting.Admin.ManageUser" %>
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
                    <asp:Label ID="userName" CssClass="h2" runat="server"></asp:Label>              
                </div>
                <div class="mrgn-tp-lg mrgn-lft-md mrgn-rght-md">
                    <wet:WetDropDownList ID="Roles" runat="server" IsRequired="true" LabelText="Role" OnSelectedIndexChanged="Roles_SelectedIndexChanged" AutoPostBack="true"></wet:WetDropDownList>
                    <wet:WetDropDownList ID="RegionsDDL" runat="server" IsRequired="true" LabelText="Regions" AutoPostBack="true" OnSelectedIndexChanged="RegionsDDL_SelectedIndexChanged"></wet:WetDropDownList>
                    <wet:WetCheckBoxList ID="RegionsCBL" runat="server" IsRequired="true" LabelText="Regions" Visible="false"></wet:WetCheckBoxList>
                    <wet:WetDropDownList ID="Communities" runat="server" IsRequired="true" LabelText="Recipient" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="Communities_SelectedIndexChanged"></wet:WetDropDownList>
                    <wet:WetDropDownList ID="Programs" runat="server" IsRequired="true" LabelText="Program" Visible="false"></wet:WetDropDownList>
                    <wet:WetButton ID="cancel" CssClass="mrgn-bttm-sm" runat="server" Text="Back" PostBackUrl="ConfigRoles.aspx"/>
                    <wet:WetButton ID="saveChanges" CssClass="mrgn-bttm-sm btn-primary" runat="server" Text="Save" OnClick="saveChanges_Click" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
