<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CKEditor.ascx.cs" Inherits="IJPReporting.UserControls.CKEditor" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<div class="mrgn-tp-sm mrgn-bttm-md">
    <CKEditor:CKEditorControl ID="ckEditor" BasePath="~/ckeditor" ResizeDir="Vertical" runat="server"></CKEditor:CKEditorControl>
</div>