<%@ Page Title="Forgot password" meta:resourceKey="ForgotPage" Language="C#" MasterPageFile="~/GCWebUsabilityTheme/MasterPages/GCWebUsability.Master" AutoEventWireup="true" CodeBehind="Forgot.aspx.cs" Inherits="IJPReporting.Account.ForgotPassword" Async="true" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1">
    <script type="text/javascript">
        $(function () {
            // wrap all the form with the class for the validation
            $('body').wrapInner('<div class="wb-frmvld"></div>');
        });
    </script>
    <style>
        section#errors-ctl01 {
            display:none;
        }
        label, textarea, input[type=text] {
            width:100% !important;
        }
    </style>

    <asp:UpdatePanel ID="upForgot" runat="server">
        <ContentTemplate>
            <wet:WetAlert runat="server" ID="alert" CssClass="mrgn-tp-md" Visible="false"></wet:WetAlert>
            <wet:WetButton ID="resendConfirmation" meta:resourcekey="ResendConfirmation" runat="server" Visible="false" OnClick="resendConfirmation_Click" />  
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btForgot" />
        </Triggers>
    </asp:UpdatePanel>
    
    <asp:Panel ID="pnlForgot" CssClass="row mrgn-tp-md" DefaultButton="btForgot" runat="server">
        <div class="col-md-8">

            <asp:Label ID="lblForgot" CssClass="h3" runat="server" meta:resourcekey="Forgot"></asp:Label>
                 
            <wet:WetTextBox ID="Email" IsEmail="true" LabelText="Email" meta:resourcekey="tb_email" IsRequired="true" runat="server"></wet:WetTextBox>
            <wet:WetButton ID="btForgot" CssClass="pull-left mrgn-tp-md" meta:resourcekey="bt_forgot" OnClick="btForgot_Click" Text="Email link" runat="server" />

        </div>
    </asp:Panel>

</asp:Content>
