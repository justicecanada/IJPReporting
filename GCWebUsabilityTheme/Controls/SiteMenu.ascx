<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SiteMenu.ascx.cs" Inherits="GCWebUsabilityTheme.Controls.SiteMenu" %>
<asp:SiteMapDataSource ID="SiteMapDataSource1" runat="server" ShowStartingNode="false" />

<asp:PlaceHolder ID="PlaceHolderSiteMenu" runat="server">

    <nav role="navigation" id="wb-sm" <%--data-ajax-replace="<%=this.SiteMenuFile %>"--%> data-trgt="mb-pnl" class="wb-menu visible-md visible-lg" typeof="SiteNavigationElement">
        <h2 class="wb-inv"><%=Localization.TemplateStrings.tmpl_topics_menu %></h2>
        <div class="container nvbar">
            <div class="row">
                <div class="col-md-12" style="padding-left: 0px;">
                    <asp:Repeater ID="Repeater1" ItemType="SiteMapNode" runat="server" DataSourceID="SiteMapDataSource1">
                        <HeaderTemplate>
                            <ul class="list-inline menu" role="menubar">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li>
                                <a class='<%#Item.HasChildNodes ? "item" : ""%>' runat="server" href='<%#Item.Url%>'>
                                    <asp:Label runat="server" Text='<%#Item.Title%>'></asp:Label>
                                </a>
                                <asp:Repeater ID="Repeater2" ItemType="SiteMapNode" Visible='<%#Item.HasChildNodes%>' DataSource='<%#Item.ChildNodes %>' runat="server">
                                    <HeaderTemplate>
                                        <ul class="sm list-unstyled" role="menu">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <li>
                                            <asp:PlaceHolder Visible='<%#!Item.HasChildNodes%>' runat="server">
                                                <a runat="server" href='<%#Item.Url%>'>
                                                    <asp:Label runat="server" Text='<%#Item.Title%>'></asp:Label>
                                                </a>
                                            </asp:PlaceHolder>
                                            <asp:PlaceHolder Visible='<%#Item.HasChildNodes%>' runat="server">
                                                <details>
                                                    <summary><asp:Label runat="server" Text='<%#Item.Title%>'></asp:Label></summary>
                                                    <ul role="menu">
                                                    <asp:Repeater ID="Repeater3" ItemType="SiteMapNode" DataSource='<%#((SiteMapNode)Container.DataItem).ChildNodes  %>' runat="server">
                                                        <ItemTemplate>
                                                            <li>
                                                                <a runat="server" href='<%#Item.Url%>'>
                                                                    <asp:Label runat="server">
                                                                        <asp:Label runat="server" Text="<%#Item.Title%>"></asp:Label>
                                                                    </asp:Label>
                                                                </a>
                                                            </li>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            </ul>
                                                            </details>
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                                            </asp:PlaceHolder>
                                        </li>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </ul>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </li>
                        </ItemTemplate>
                        <FooterTemplate>
                            </ul>
                        </FooterTemplate>
                    </asp:Repeater>

                    <div class="navbar-right">
                        <asp:LoginView runat="server" ViewStateMode="Disabled">
                            <AnonymousTemplate>
                                <ul class="list-inline menu">
                                    <li><a runat="server" meta:resourcekey="link_register" href="~/Account/Register">Register</a></li>
                                    <li><a runat="server" meta:resourcekey="link_login" href="~/Account/Login"></a></li>
                                </ul>
                            </AnonymousTemplate>
                            <LoggedInTemplate>
                                <ul class="list-inline menu">
                                    <li><a runat="server" href="~/Account/Manage" meta:resourcekey="manage" title="Manage your account"><%: HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(Context.User.Identity.GetUserId()).LastName + ", " + HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(Context.User.Identity.GetUserId()).FirstName %></a></li>
                                    <li>
                                        <asp:LoginStatus runat="server" meta:resourcekey="logoff" LogoutAction="Redirect" LogoutText="Log off" LogoutPageUrl="~/" OnLoggingOut="Unnamed_LoggingOut" />
                                    </li>
                                </ul>
                            </LoggedInTemplate>
                        </asp:LoginView>
                    </div>
                </div>
            </div>
        </div>
    </nav>
</asp:PlaceHolder>
<asp:PlaceHolder ID="PlaceHolderNoSiteMenu" runat="server" Visible="False">
    <span data-trgt="mb-pnl" class="wb-menu hide"></span>
</asp:PlaceHolder>
