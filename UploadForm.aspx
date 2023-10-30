<%@ Page Title="Upload forms" Language="C#" MasterPageFile="~/GCWebUsabilityTheme/MasterPages/GCWebUsability.master" AutoEventWireup="true" CodeBehind="UploadForm.aspx.cs" Inherits="IJPReporting.Upload" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <wet:WetSummary runat="server"></wet:WetSummary>
    <asp:Panel runat="server" CssClass="panel panel-default mrgn-tp-md">
        <div class="panel-heading">
            <asp:Label CssClass="h3" runat="server" Text="Upload"></asp:Label>                   
        </div>
        <asp:Panel runat="server" >    
            <div class="mrgn-lft-md mrgn-bttm-md mrgn-tp-md">
                <wet:WetFileUpload ID="formUpload" Class="mrgn-tp-lg mrgn-lft-md" IsRequired="true" runat="server" LabelText="Upload"/>
                <wet:WetButton ID="UploadBtn" CssClass="mrgn-tp-lg mrgn-lft-md mrgn-bttm-lg" runat="server" OnClick="UploadBtn_Click" Text="Upload"/>
            </div>
        </asp:Panel>
    </asp:Panel>   
</asp:Content>
