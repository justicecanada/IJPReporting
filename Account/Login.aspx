<%@ Page Title="Log in" meta:resourceKey="LoginPage" Language="C#" MasterPageFile="~/GCWebUsabilityTheme/MasterPages/GCWebUsability.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="IJPReporting.Account.Login" Async="true" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1">
    <script src="../Scripts/bootstrap.min.js"></script>
    <script>

        $(function () {
            // wrap all the form with the class for the validation
            $('body').wrapInner('<div class="wb-frmvld"></div>');
        });

    </script>
    <style>

        section#errors-ctl01 {
            display: none;
        }

        label, textarea, input[type=text] {
            width: 100% !important;
        }

    </style>
    <asp:UpdatePanel ID="upAlert" runat="server">
        <ContentTemplate>
            <wet:WetAlert ID="alert" CssClass="mrgn-tp-md" runat="server" AlertType="Danger" Visible="false"></wet:WetAlert>
            <wet:WetButton ID="resendConfirmation" runat="server" Visible="false" meta:resourcekey="resendConfirmation" OnClick="resendConfirmation_Click" />

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btn_login" />
        </Triggers>
    </asp:UpdatePanel>

    <div class="row">
        <div class="col-md-12">
            <h3 class="text-center" runat="server" meta:resourcekey="Login">Sign in to your account</h3>
            <section id="loginForm">

                <div class="wetForm well well-lg col-md-4 col-md-offset-4 mrgn-tp-sm">

                    <wet:WetTextBox ID="Email" AutoCompleteType="Email" Width="720" IsEmail="true" LabelText="Email" meta:resourcekey="tb_email" IsRequired="true" runat="server"></wet:WetTextBox>
                    <wet:WetTextBox ID="Password" Width="720" TextMode="Password" LabelText="Password" meta:resourcekey="tb_pwd" IsRequired="true" runat="server"></wet:WetTextBox>
                    <%--<div class="checkbox hidden">
                        <label>
                            <input type="checkbox" runat="server" id="RememberMe" />
                            <span runat="server" meta:resourcekey="RememberMe"></span>
                        </label>
                    </div>--%>
                    <wet:WetButton ID="btn_login" CssClass="mrgn-tp-md" meta:resourcekey="bt_login" OnClick="LogIn" Text="Log in" runat="server" />

                    <div>
                        <%--Enable this once you have account confirmation enabled for password reset functionality--%>
                        <div>
                            <asp:HyperLink runat="server" ID="ForgotPasswordHyperLink" meta:resourcekey="ForgotPassword">Forgot your password?</asp:HyperLink>
                        </div>
                        <div>
                            <asp:HyperLink runat="server" ID="RegisterHyperLink" meta:resourcekey="Register">Create a new account</asp:HyperLink>
                        </div>
                    </div>

                </div>

            </section>
        </div>
    </div>
</asp:Content>
