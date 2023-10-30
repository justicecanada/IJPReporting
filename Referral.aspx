<%@ Page Title="Referral" Language="C#" MasterPageFile="~/GCWebUsabilityTheme/MasterPages/GCWebUsability.master" AutoEventWireup="true" CodeBehind="Referral.aspx.cs" Inherits="IJPReporting.Referral" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <wet:WetSummary runat="server"></wet:WetSummary>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            
            <asp:Panel runat="server" CssClass="panel panel-default mrgn-tp-md">
                <div class="panel-heading">
                    <asp:Label ID="formNameLabel" CssClass="h3" runat="server"></asp:Label>
                    <asp:Label ID="clientFileNumber" CssClass="h4" runat="server"></asp:Label>
                </div>
                <div class="text-center mrgn-tp-md">
                    <ul id="wizHeader">
                       <asp:Repeater ID="SideBarList" runat="server">
                           <ItemTemplate>
                               <li>
                                   <wet:WetLinkButton runat="server" ID="SideBarButton" onclick="SideBarButton_Click" CssClass="<%# IJPReporting.Managers.WizardManager.GetClassForWizardStep(Container.DataItem) %>" title='<%#Eval("Name")%>' Text='<%#Eval("Name")%>'></wet:WetLinkButton>
                               </li>
                           </ItemTemplate>
                       </asp:Repeater>
                   </ul>
                </div>
                <div class="panel-body">
                    <wet:WetLinkButton ID="SaveWizardBtn" runat="server" OnClick="SaveWizardBtn_Click" ButtonType="Link" ToolTip="Save" CssClass="mediumIcon fas fa-save pull-right mrgn-rght-lg"></wet:WetLinkButton>

                    <asp:PlaceHolder ID="wizardPh" runat="server"></asp:PlaceHolder>
                </div>
            </asp:Panel>
            <div id="spinner" class="spinner" style="display:none">
                Loading&#8230;
                <div class="cube1"></div>
                <div class="cube2"></div>
            </div>
        </ContentTemplate>      
    </asp:UpdatePanel>

    <script type="text/javascript">
    //<![CDATA[
        $(groupClass);
        $(triggerHelp);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(groupClass);

        function triggerHelp() {
            $(document).on('click', '.help', function () {
                $(this).next().next().toggle();
            });
        }
        
        function groupClass() {

            // initier un array
            var groups = [];

            // retrouver les groupes et les insérer dans le array
            $('[class*="group_"]').each(function () {
                var attrClass = $(this).attr("class");
                var key = attrClass.substring(attrClass.indexOf('group_')).split(' ')[0];
                if ($(this).is("fieldset")){
                    $(this).wrap("<div class='form-group'></div>");
                }
                if ($.inArray(key, groups) < 0) groups.push(key);
            });

            // boucler sur le array et wrapper les groupes
            $.each(groups, function (i, item) {
                if (!$('.form-group:has(".' + item + '")').parent().hasClass("bs-callout")) {
                    $('.form-group:has(".' + item + '")').wrapAll('<div class="bs-callout bs-callout bs-callout-primary" />');

                }
            });
        }
    //]]>
    </script>

</asp:Content>
