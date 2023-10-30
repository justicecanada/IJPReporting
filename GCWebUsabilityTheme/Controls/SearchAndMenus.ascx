<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchAndMenus.ascx.cs" Inherits="GCWebUsabilityTheme.Controls.SearchAndMenus" %>
<section class="wb-mb-links col-xs-12 visible-sm visible-xs" id="wb-glb-mn">
    <h2>Menu</h2>
    <ul id="ul_sm_topmenu" class="list-inline text-right">

<%--        <asp:LoginView runat="server" ViewStateMode="Disabled">
            <AnonymousTemplate>

                <li><a runat="server" meta:resourcekey="link_register" href="~/Account/Register">Register</a></li>
                <li><a runat="server" meta:resourcekey="link_login" href="~/Account/Login"></a></li>

            </AnonymousTemplate>
            <LoggedInTemplate>

                <li><a runat="server" href="~/Account/Manage" title="Manage your account"><%: HttpContext.Current.GetOwinContext().GetUserManager<IJPReporting.ApplicationUserManager>().FindById(Context.User.Identity.GetUserId()).LastName + ", " + HttpContext.Current.GetOwinContext().GetUserManager<IJPReporting.ApplicationUserManager>().FindById(Context.User.Identity.GetUserId()).FirstName %></a></li>
                <li>
                    <asp:LoginStatus runat="server" LogoutAction="Redirect" LogoutText="Log off" LogoutPageUrl="~/" OnLoggingOut="Unnamed_LoggingOut" />
                </li>

            </LoggedInTemplate>
        </asp:LoginView>--%>

        <li>
            <ul class="pnl-btn list-inline text-right">
                <li><a href="#mb-pnl" title="Menu" aria-controls="mb-pnl" class="overlay-lnk btn btn-sm btn-default" role="button"><span class="glyphicon glyphicon-search"><span class="glyphicon glyphicon-th-list"><span class="wb-inv"><%=Localization.General.srch_menus %></span></span></span></a></li>
            </ul>
        </li>
    </ul>
    <div id="mb-pnl">
    </div>
</section>
