<%@ Page Title="Manage Account" meta:resourceKey="Manage" Language="C#" MasterPageFile="~/GCWebUsabilityTheme/MasterPages/GCWebUsability.Master" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="IJPReporting.Account.Manage" %>

<%--<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>--%>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <wet:WetSummary DisplaySummary="false" runat="server"></wet:WetSummary>

    <div class="row mrgn-tp-md mrgn-bttm-md">
        <div class="col-md-12">
            <asp:Label ID="lblAccount" CssClass="label label-warning pull-right mrgn-tp-sm" runat="server"></asp:Label>
        </div>
    </div>

    <asp:UpdatePanel ID="upSettings" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnEditAccount" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelAccount" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnSaveAccount" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <div class="panel panel-default">
                <div class="panel-heading">
                    <asp:Label ID="lblChangeSettingTitle" CssClass="h4 text-primary" meta:resourcekey="lblChangeSettingTitle" runat="server"></asp:Label>
                </div>
                <div class="panel-body">

                    <wet:WetAlert ID="wetAlertAccountSetting" CssClass="mrgn-tp-md" EnableViewState="false" runat="server" Visible="false"></wet:WetAlert>

                    <!-- Affichage du compte -->
                    <asp:Panel ID="pnlAccountView" CssClass="row" DefaultButton="btnEditAccount" runat="server">
                        <div class="col-md-12">
                            <table id="tableAccount" class="table mrgn-bttm-0">
                                <thead>
                                    <tr>
                                        <td><asp:Label ID="lblUserNameTitle" CssClass="h5" runat="server" meta:resourcekey="lblUserNameTitle"></asp:Label></td>
                                        <td><asp:Label ID="lblUserName" runat="server"></asp:Label></td>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td><asp:Label ID="lblEmailTitle" CssClass="h5" runat="server" meta:resourcekey="lblEmailTitle"></asp:Label></td>
                                        <td><asp:Label ID="lblEmail" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label ID="lblTelephoneTitle" CssClass="h5" runat="server" meta:resourcekey="lblTelephoneTitle"></asp:Label></td>
                                        <td><asp:Label ID="lblTelephone" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label ID="lblLastPasswordChangeTitle" CssClass="h5" runat="server" meta:resourcekey="lblLastPasswordChangeTitle"></asp:Label></td>
                                        <td><asp:Label ID="lblLastPasswordChange" runat="server"></asp:Label></td>
                                    </tr>
                                </tbody>
                            </table>
                            <div class="row mrgn-tp-md">
                                <div class="col-md-12">
                                    <wet:WetButton ID="btnEditAccount" EnableClientValidation="false" CssClass="mrgn-tp-md" OnClick="btnEditAccount_Click" meta:resourcekey="btnEdit" runat="server" />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>

                    <!-- Modification du compte -->
                    <asp:Panel ID="pnlAccountEdit" CssClass="row" Visible="false" DefaultButton="btnSaveAccount" runat="server">
                        <div class="col-md-12">
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:LinkButton ID="lnkChangePassword" PostBackUrl="~/Account/ManagePassword.aspx" meta:resourcekey="lnkChangePassword" runat="server"></asp:LinkButton>
                                </div>
                            </div>
                            <div class="row mrgn-tp-lg">
                                <div class="col-md-12">
                                    <wet:WetTextBox ID="txtTelephone" IsPhoneNumber="true" IsRequired="true" meta:resourcekey="txtTelephone" runat="server"></wet:WetTextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <wet:WetTextBox ID="txtEmail" IsEmail="true" IsRequired="true" meta:resourcekey="txtEmail" runat="server"></wet:WetTextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <wet:WetButton ID="btnCancelAccount" CssClass="mrgn-tp-md" EnableClientValidation="false" OnClick="btnCancelAccount_Click" meta:resourcekey="btnCancel" runat="server" />
                                    <wet:WetButton ID="btnSaveAccount" ButtonType="Primary" CssClass="mrgn-tp-md" OnClick="btnSaveAccount_Click" meta:resourcekey="btnSave" runat="server" />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
