<%@ Page Title="" Language="C#" MasterPageFile="~/GCWebUsabilityTheme/MasterPages/GCWebUsability.master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="IJPReporting.Admin.Admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Administration - User Roles</h2>
    <asp:Repeater id="repeater_userSA" runat="server" EnableViewState="true">
        <HeaderTemplate>
            <h4>Super Admins</h4>
            <table class="table table-striped">
        </HeaderTemplate>
        <ItemTemplate>
            <tr>

                <td>
                    <asp:Label id="lbl_superAdmin" CssClass='<%# "lbl_sa" + Container.ItemIndex %>' runat="server" Text='<%# Eval("Email") %>' OnDataBinding="lbl_superAdmin_DataBinding"></asp:Label>
               </td>
                <td>
                    <button id="bt_rm_sa" class='<%# "btn btn-default btn-xs bt_rm_sa" + Container.ItemIndex %>' type="button" runat="server" onserverclick="bt_rm_sa_ServerClick">
                        <span class="glyphicon glyphicon-remove" aria-hidden="true"></span>
                    </button>
                </td>
            </tr>
            
        </ItemTemplate>
        <FooterTemplate>
            <tr class="form-inline">
                <td class="form-group">
                    <label>New user</label>
                    <input type="email" class="form-control input-sm"/>
                </td>
                <td><button type="button" class="btn btn-default btn-xs">Add
                    </button>
                </td>
            </tr>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    <asp:Label id="lbl_test" runat="server" Text="test"></asp:Label>
    <asp:HiddenField ID="hf_user" runat="server" Value="" />
    <asp:Repeater id="repeater_userAdmin" runat="server">
        <HeaderTemplate>
            <h4>Administrators</h4>
            <table class="table table-striped">
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <asp:Label runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                </td>
                <td>
                    <button type="button" class="btn btn-default btn-xs">
                        <span class="glyphicon glyphicon-remove" aria-hidden="true"></span>
                    </button>
                </td>
                <td></td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            <tr class="form-inline">
                <td class="form-group">
                    <label>New user</label>
                    <input type="email" class="form-control input-sm"/>
                </td>
                <td>
                    <button type="button" class="btn btn-default btn-xs">Add
                    </button>
                </td>
            </tr>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    <asp:Repeater id="repeater_userClient" runat="server">
        <HeaderTemplate>
            <h4>Clients</h4>
            <table class="table table-striped">
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
            <asp:Label id="lbl_client" runat="server" Text='<%# Eval("Email") %>' onDataBinding="lbl_client_DataBinding"></asp:Label>
            </td>
                <td>
                    <button type="button" class="btn btn-default btn-xs">
                        <span class="glyphicon glyphicon-remove" aria-hidden="true"></span>
                    </button>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            <tr class="form-inline">
                <td class="form-group">
                    <label>New user</label>
                    <input type="email" class="form-control input-sm"/>
                </td>
                <td>
                    <button type="button" class="btn btn-default btn-xs">Add
                    </button>
                </td>
            </tr>
            </table>
        </FooterTemplate>
    </asp:Repeater>
   
</asp:Content>
