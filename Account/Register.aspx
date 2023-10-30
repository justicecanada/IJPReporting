<%--<%@ Page Title="Create an account" meta:resourceKey="RegisterPage" Language="C#" MasterPageFile="~/GCWebUsabilityTheme/MasterPages/GCWebUsability.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="IJPReporting.Account.Register" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1">
    <script type="text/javascript">
        $(function () {
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
    <wet:WetAlert id="alert" Visible="false" runat="server" AlertType="Danger"></wet:WetAlert>

    <div class="row">
        <h3 class="text-center" runat="server" meta:resourcekey="Register">Create a new account</h3>
        <div class="well well-lg col-md-8 col col-md-offset-2">
            
            <div class="col-md-6">
                <wet:WetTextBox ID="FirstName" AutoCompleteType="FirstName" LabelText="First name" IsRequired="true" meta:resourcekey="tb_prenom" runat="server"></wet:WetTextBox>

                <wet:WetTextBox ID="LastName" AutoCompleteType="LastName" LabelText="Last name" IsRequired="true" meta:resourcekey="tb_nom" runat="server"></wet:WetTextBox>

                <wet:WetTextBox ID="Telephone" AutoCompleteType="BusinessPhone" IsPhoneNumber="true" LabelText="Telephone" IsRequired="true" meta:resourcekey="tb_tel" runat="server"></wet:WetTextBox>

                <wet:WetTextBox ID="Email" AutoCompleteType="Email" IsGovernmentEmail="true" LabelText="Office email" meta:resourcekey="tb_email" IsRequired="true" runat="server"></wet:WetTextBox>

                <wet:WetTextBox ID="Password" TextMode="Password" LabelText="Password" meta:resourcekey="tb_pwd" IsRequired="true" runat="server"></wet:WetTextBox>

                <wet:WetTextBox ID="ConfirmPassword" TextMode="Password" LabelText="Confirm password" meta:resourcekey="tb_cpwd" IsRequired="true" runat="server"></wet:WetTextBox>

                 <asp:CompareValidator runat="server" ControlToCompare="Password" meta:resourcekey="compare" ControlToValidate="ConfirmPassword"
                            CssClass="text-danger" Display="Dynamic" ErrorMessage="The password and confirmation password do not match." />       
            </div>
                  
            <div class="col-md-6">
                <wet:WetDropDownList ID="Roles" runat="server" IsRequired="true" LabelText="Role" OnSelectedIndexChanged="Roles_SelectedIndexChanged"></wet:WetDropDownList>
                <wet:WetCheckBoxList ID="RegionsRBL" IsRequired="true" LabelText="Regions" runat="server" OnSelectedIndexChanged=""></wet:WetCheckBoxList>
                 <wet:WetDropDownList ID="RegionsDDL" runat="server" IsRequired="true" LabelText="Region"></wet:WetDropDownList>
                <wet:WetDropDownList ID="Communities" runat="server" IsRequired="true" LabelText="Community"></wet:WetDropDownList>
                <wet:WetDropDownList ID="Programs" runat="server" IsRequired="true" LabelText="Program"></wet:WetDropDownList>
            </div>
        </div>

        <div class="col-md-2 col col-md-offset-5">
            <wet:WetButton ID="btRegister" CssClass="mrgn-tp-md" meta:resourcekey="bt_reg" OnClick="CreateUser_Click" Text="Register" runat="server" />
        </div>

    </div>
</asp:Content>--%>

<%@ Page Title="Create an account" meta:resourceKey="RegisterPage" Language="C#" MasterPageFile="~/GCWebUsabilityTheme/MasterPages/GCWebUsability.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="IJPReporting.Account.Register" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1">
    <script type="text/javascript">
        $(function () {
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
    <wet:WetAlert id="alert" Visible="false" runat="server" AlertType="Danger"></wet:WetAlert>

    <div class="row">
        <h3 class="text-center" runat="server" meta:resourcekey="Register">Create a new account</h3>
        <div class="well well-lg col-md-4 col col-md-offset-4">
            

            <wet:WetTextBox ID="FirstName" AutoCompleteType="FirstName" LabelText="First name" IsRequired="true" meta:resourcekey="tb_prenom" runat="server"></wet:WetTextBox>


            <wet:WetTextBox ID="LastName" AutoCompleteType="LastName" LabelText="Last name" IsRequired="true" meta:resourcekey="tb_nom" runat="server"></wet:WetTextBox>


            <wet:WetTextBox ID="Telephone" AutoCompleteType="BusinessPhone" IsPhoneNumber="true" LabelText="Telephone" IsRequired="true" meta:resourcekey="tb_tel" runat="server"></wet:WetTextBox>

            <wet:WetDropDownList ID="Regions" runat="server" IsRequired="true" LabelText="Regions"></wet:WetDropDownList>

            <wet:WetTextBox ID="Email" AutoCompleteType="Email" IsGovernmentEmail="true" LabelText="Office email" meta:resourcekey="tb_email" IsRequired="true" runat="server"></wet:WetTextBox>

            <wet:WetTextBox ID="Password" TextMode="Password" LabelText="Password" meta:resourcekey="tb_pwd" IsRequired="true" runat="server"></wet:WetTextBox>


            <wet:WetTextBox ID="ConfirmPassword" TextMode="Password" LabelText="Confirm password" meta:resourcekey="tb_cpwd" IsRequired="true" runat="server"></wet:WetTextBox>

                    <asp:CompareValidator runat="server" ControlToCompare="Password" meta:resourcekey="compare" ControlToValidate="ConfirmPassword"
                        CssClass="text-danger" Display="Dynamic" ErrorMessage="The password and confirmation password do not match." />

            <wet:WetButton ID="btRegister" CssClass="mrgn-tp-md" meta:resourcekey="bt_reg" OnClick="CreateUser_Click" Text="Register" runat="server" />
        </div>

    </div>
</asp:Content>

