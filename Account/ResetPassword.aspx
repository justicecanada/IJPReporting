<%@ Page Title="Reset Password" meta:resourceKey="ResetPassword" Language="C#" MasterPageFile="~/GCWebUsabilityTheme/MasterPages/GCWebUsability.Master" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="IJPReporting.Account.ResetPassword" Async="true" %>

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
    <wet:WetAlert runat="server" ID="alert" AlertType="Danger" Visible="false"></wet:WetAlert>

    <div class="col-md-12">
        <h4 runat="server" meta:resourcekey="ResetPWD">Enter your new password</h4>
        <wet:WetTextBox ID="Email" IsEmail="true" LabelText="Email" meta:resourcekey="tb_email" IsRequired="true" runat="server"></wet:WetTextBox>
        <wet:WetTextBox ID="Password" TextMode="Password" LabelText="Password" meta:resourcekey="tb_pwd" IsRequired="true" runat="server"></wet:WetTextBox>
        <wet:WetTextBox ID="ConfirmPassword" TextMode="Password" LabelText="Confirm password" meta:resourcekey="tb_cpwd" IsRequired="true" runat="server"></wet:WetTextBox>
        <asp:CompareValidator runat="server" ControlToCompare="Password" meta:resourcekey="compare" ControlToValidate="ConfirmPassword"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The password and confirmation password do not match." />
        <wet:WetButton ID="btReset" CssClass="pull-left mrgn-tp-md" meta:resourcekey="bt_reg" OnClick="Reset_Click" Text="Reset" runat="server" />

    </div>
</asp:Content>
