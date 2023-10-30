<%@ Page Title="Manage Password" Language="C#" meta:resourceKey="ManagePassword" MasterPageFile="~/GCWebUsabilityTheme/MasterPages/GCWebUsability.Master" AutoEventWireup="true" CodeBehind="ManagePassword.aspx.cs" Inherits="IJPReporting.Account.ManagePassword" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

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

    <asp:UpdatePanel ID="upAlert" runat="server">
        <ContentTemplate>

            <wet:WetAlert runat="server" ID="alert" CssClass="mrgn-tp-md" AlertType="Danger" Visible="false"></wet:WetAlert>

        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:Panel ID="pnlChangePassword" Visible="false" runat="server">

        <div class="col-md-12">
            <h3 runat="server" meta:resourcekey="ChangePWDF">Change Password Form</h3>
            
            <div>
                <wet:WetTextBox ID="CurrentPasswordLabel" TextMode="Password" LabelText="Password" meta:resourcekey="CurrentPasswordLabel" IsRequired="true" runat="server"></wet:WetTextBox>
                <wet:WetTextBox ID="NewPasswordLabel" TextMode="Password" LabelText="Password" meta:resourcekey="NewPasswordLabel" IsRequired="true" runat="server"></wet:WetTextBox>
                <wet:WetTextBox ID="ConfirmNewPasswordLabel" TextMode="Password" LabelText="Password" meta:resourcekey="ConfirmNewPasswordLabel" IsRequired="true" runat="server"></wet:WetTextBox>
                <asp:CompareValidator runat="server" ControlToCompare="NewPasswordLabel" ControlToValidate="ConfirmNewPasswordLabel" meta:resourcekey="compare"
                        CssClass="text-danger" Display="Dynamic" ErrorMessage="The new password and confirmation password do not match."
                        ValidationGroup="ChangePassword" />
                <wet:WetButton ID="bt_CP" CssClass="mrgn-tp-md" ValidationGroup="ChangePassword" meta:resourcekey="bt_CP" OnClick="ChangePassword_Click" Text="Change Password" runat="server" />
            </div>
        </div>

    </asp:Panel>

</asp:Content>
