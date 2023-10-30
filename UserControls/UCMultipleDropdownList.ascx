<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCMultipleDropdownList.ascx.cs" Inherits="IJPReporting.UserControls.UCMultipleDropdownList" %>
<div class="row">
    <div class="col-md-11">
        <wet:WetDropDownList ID="DDL" runat="server"></wet:WetDropDownList>
    </div>
    <div class="col-md-1" style="margin-top:25px;">
        <wet:WetLinkButton ID="AddDDL" runat="server" OnClick="AddDDL_Click" EnableClientValidation="false" ButtonType="Link" CssClass="customLnk fas fa-plus"></wet:WetLinkButton>
    </div>
</div>
