<%@ Page Title="Role Management" Language="C#" MasterPageFile="~/GCWebUsabilityTheme/MasterPages/GCWebUsability.master" AutoEventWireup="true" CodeBehind="ConfigRoles.aspx.cs" Inherits="IJPReporting.Admin.ConfigRoles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<style>
    .green{
        color:green;
    }
    .red{
        color:red;
    }
</style>
<script type="text/javascript">
        //<![CDATA[
        $(function () {
            // fix bug for empty options
            $('select option:not([label])').filter(function () {
                return ($(this).val().trim() == "" && $(this).text().trim() == "");
            }).remove();
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            // re-bind your jQuery events here
            $('select option:not([label])').filter(function () {
                return ($(this).val().trim() == "" && $(this).text().trim() == "");
            }).remove();
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            // rebind datatable after postback
            $(".wb-tables").trigger("wb-init.wb-tables");
        });

    //]]>
    </script>

    <div class="row mrgn-tp-md">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <asp:Label ID="Label5" CssClass="h3" runat="server" meta:resourcekey="lblRoleManagement" Text="Role Management"></asp:Label>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:Repeater ID="rptRoles" runat="server" ItemType="IJPReporting.Models.ApplicationUser" OnItemCommand="rptRoles_ItemCommand">
                                <HeaderTemplate>
                                    <table id="tblCalls" class="table wb-tables table-striped table-hover">
                                        <thead>
                                            <tr>
                                                <th><asp:Label ID="lblUserName" Text="User" meta:resourcekey="lblUserName" runat="server"></asp:Label></th>
                                                <th><asp:Label ID="lblEmail" Text="Email" meta:resourcekey="lblEmail" runat="server"></asp:Label></th>
                                                <th><asp:Label ID="lblUserRole" Text="Role" meta:resourcekey="lblUserRole" runat="server"></asp:Label></th>
                                                <th><asp:Label ID="lblDeleteUser" Text="Delete User" meta:resourcekey="lblDeleteUser" runat="server"></asp:Label></th>

                                            </tr>
                                        </thead>
                                </HeaderTemplate>
                                <ItemTemplate>

                                    <tr id="row" runat="server">
                                        <td>
                                            <asp:Label ID="Label1" Text='<%# Item.FirstName+ " " + Item.LastName %>' runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_userEmail" runat="server" Text='<%# Item.Email %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblRole" Text='<%# Item.RoleName %>'></asp:Label></td>
                                        <td class="text-center">
                                            <asp:LinkButton ID="lb_deleteUser" CssClass="glyphicon glyphicon-remove red form-inline" aria-label="Delete user" ToolTip="Delete user" runat="server" CommandName="delete" CommandArgument='<%# Item.Email %>' onclientclick="return confirm('Are you sure you want to delete this user?')" ></asp:LinkButton>
                                            <asp:LinkButton runat="server" CommandName="Manage" CommandArgument='<%# Item.Id %>'><span class="glyphicon glyphicon-cog"></span></asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
