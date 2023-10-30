<%@ Page Title="Contact Us" Language="C#" MasterPageFile="~/GCWebUsabilityTheme/MasterPages/GCWebUsability.master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="IJPReporting.Contact" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <wet:WetSummary runat="server"></wet:WetSummary>
    <asp:UpdatePanel ID="upAlert" runat="server" UpdateMode="Conditional" class="mrgn-tp-md">
        <ContentTemplate>
            <wet:WetAlert ID="wetAlert" runat="server" EnableViewState="false" Visible="false"></wet:WetAlert>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel runat="server" CssClass="panel panel-default mrgn-tp-md">
        <div class="panel-heading">
                    <asp:Label CssClass="h3" runat="server" Text="Contact Us"></asp:Label>                   
                </div>
            <asp:Panel runat="server">
                
                <div class="mrgn-lft-md mrgn-bttm-md mrgn-tp-md">
                    <wet:WetTextBox ID="nameField" runat="server" LabelText="Your name" Glyphicon="glyphicon glyphicon-user" IsRequired="true"></wet:WetTextBox>
                    <wet:WetTextBox ID="emailField" runat="server" LabelText="Your email" Glyphicon="glyphicon glyphicon-envelope" IsRequired="true"></wet:WetTextBox>
                    <wet:WetDropDownList ID="subjectDdl" runat="server" LabelText="Subject" Glyphicon="glyphicon glyphicon-tags" IsRequired="true"></wet:WetDropDownList>
                    <wet:WetTextBox ID="messageField" runat="server" Rows="5" Columns="20" TextMode="MultiLine" LabelText="Message" Glyphicon="glyphicon glyphicon-pencil" IsRequired="true"></wet:WetTextBox>
                    
                    <wet:WetButton ID="sendEmail" Text="Send Email" runat="server" OnClick="SendEmail_Click"/>
                </div>
                
            </asp:Panel>
    </asp:Panel>      
</asp:Content>
