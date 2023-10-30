<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCRemovableDropdownList.ascx.cs" Inherits="IJPReporting.UserControls.UCRemovableDropdownList" %>
<div class="row">
    <div class="col-md-11">
        <wet:WetDropDownList ID="DDL" runat="server"></wet:WetDropDownList>
    </div>
    <div class="col-md-1" style="margin-top:25px;">
        <wet:WetLinkButton ID="RemoveDDL" runat="server" OnClick="RemoveDDL_Click" EnableClientValidation="false" ButtonType="Link" CssClass="fas fa-minus"></wet:WetLinkButton>
    </div>
</div>