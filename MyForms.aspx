<%@ Page Title="Uploaded forms" Language="C#" MasterPageFile="~/GCWebUsabilityTheme/MasterPages/GCWebUsability.master" AutoEventWireup="true" CodeBehind="MyForms.aspx.cs" Inherits="IJPReporting.MyForms" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="panel panel-default">
                <div class="panel-heading">
                    <asp:Label CssClass="h3" runat="server" Text="Uploaded forms"></asp:Label>                   
                </div>
                <wet:WetLinkButton ID="btnCreateForm" PostBackUrl="UploadForm.aspx" runat="server" CssClass="pull-right mrgn-tp-md mrgn-rght-md"><asp:Label runat="server">Upload</asp:Label></wet:WetLinkButton>
                <div class="mrgn-tp-lg mrgn-lft-md mrgn-rght-md">
                    <asp:Repeater ID="formsRpt" runat="server" ItemType="IJPReporting.Models.Files" OnItemCommand="formsRpt_ItemCommand">
                    <HeaderTemplate>
                        <table class="table wb-tables table-striped table-hover table-responsive" data-wb-tables='{"aoColumnDefs":[{"orderable": false, "targets": -1}]}'>
                            <thead>
                                <tr>
                                    <th><asp:Label runat="server" Text="File name"></asp:Label></th>
                                    <th><asp:Label runat="server" Text="Added date"></asp:Label></th>
                                    <th><asp:Label runat="server" Text="Added by"></asp:Label></th>
                                    <th><asp:Label runat="server" Text="Action"></asp:Label></th>
                                </tr>
                            </thead>
                        
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><i class=" <%# GetExtensionIcon(Item.file_client_name) %> mrgn-rght-sm" style="font-size:20px!important"></i><asp:LinkButton runat="server" CommandName="Save" CommandArgument="<%# Item.file_id %>"><span><%# Item.file_client_name %></span></asp:LinkButton></td>
                            <td data-order='<%# Item.creation_date.ToString("s") %>'><asp:Label runat="server" Text="<%# Item.creation_date %>"></asp:Label></td>
                            <td><asp:Label runat="server" Text="<%# GetUserNameById(Item.creator_id) %>"></asp:Label></td>
                            <td>
                                <asp:LinkButton runat="server" CommandName="Delete" CommandArgument="<%# Item.file_id %>" OnClientClick="return confirm('Are you sure you want to delete this file?')"><span class="glyphicon glyphicon-trash text-danger"></span></asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                </div>
                
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
