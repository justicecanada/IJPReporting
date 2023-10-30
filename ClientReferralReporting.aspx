<%@ Page Title="Client/Referral " Language="C#" MasterPageFile="~/GCWebUsabilityTheme/MasterPages/GCWebUsability.master" AutoEventWireup="true" CodeBehind="ClientReferralReporting.aspx.cs" Inherits="IJPReporting.Questionnaires" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<%--    <script src="http://www.appelsiini.net/download/jquery.jeditable.mini.js"></script>--%>

    <script type="text/javascript">
    //<![CDATA[

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            // rebind datatable after postback
            $(".wb-tables").trigger("wb-init.wb-tables");
        });

        

        // datatable
        $(document).ready(function () {
            window["wb-tables"] = {
                "serverSide": true,
                "ajax": {
                    "type": "POST",
                    "contentType": "application/json; charset=utf-8",
                    "url": "API.asmx/GetForms",
                    "data": function (d) {
                        d.language = "<%= this.Language %>";
                        d.serviceTypeId = "<%= !String.IsNullOrEmpty(this.servicesDDL.SelectedValue) ? this.servicesDDL.SelectedValue : "0" %>";
                        d.completionStatusId = "<%= !String.IsNullOrEmpty(completionStatusDDL.SelectedValue) ? completionStatusDDL.SelectedValue : "0" %>";
                        d.acceptanceStatusId = "<%= !String.IsNullOrEmpty(acceptanceStatusDDL.SelectedValue) ? acceptanceStatusDDL.SelectedValue : "0" %>";
                        d.fromRefDate = "<%= !String.IsNullOrEmpty(refDateStart.Text) ? refDateStart.Text : "0" %>";
                        d.toRefDate = "<%= !String.IsNullOrEmpty(refDateEnd.Text) ? refDateEnd.Text : "0" %>";
                        d.regionId = "<%= !String.IsNullOrEmpty(RegionsDDL.SelectedValue) ? RegionsDDL.SelectedValue : "0" %>";
                        d.communityId = "<%= !String.IsNullOrEmpty(Communities.SelectedValue) ? Communities.SelectedValue : "0" %>";
                        d.programId = "<%= !String.IsNullOrEmpty(Programs.SelectedValue) ? Programs.SelectedValue : "0" %>";
                        return JSON.stringify({ parameters: d });
                    }
                },
                "columnDefs": [
                    {
                        "targets": 0,
                        "visible": false,
                        "orderable": false,
                        "serachable": false
                    },
                    {
                        "targets": -1,
                        "serachable": false,
                        "orderable": false,
                        mRender: function (data, type, row) {
                            return '<a id="lnkOpen" href="Referral.aspx?refId=' + row[0] + '&Edit=true"><span class="glyphicon glyphicon-edit"></span></a>' + '<span id="' + row[0] + '" class="download glyphicon glyphicon-export mrgn-lft-sm pointer"></span>'

                        }
                    },
                    {
                        "targets": 1,
                        mRender: function (data, type, row) {
                            return '<a id="lnkOpen" href="Referral.aspx?refId=' + row[0] + '&ReadOnly=true">' + row[1] + '</a>'
                        }
                    },
                     {
                        "targets": 2,
                        mRender: function (data, type, row) {
                            return '<span class="cfn">' + row[2] + '</span>' + '<span id="' + row[0] + '" class="editable glyphicon glyphicon-edit mrgn-lft-sm pointer"></span>'
                        }
                    },
                     {
                        "targets": 3,
                        mRender: function (data, type, row) {
                            return '<span class="refDate">' + row[3] + '</span>' + '<span id="' + row[0] + '" class="editable glyphicon glyphicon-edit mrgn-lft-sm pointer"></span>'
                        }
                    }
                ]
            };

            $('#tblQuestionnaires tbody').on('click', 'span', function () {


                var classList = $(this).attr('class').split(' ');

                var className = classList.length > 0 ? classList[0] : "";
                var formId = $(this).attr('id');

                if (className == "editable") {

                    
                    var text = $(this).parent().find('span:first').text();
                    var input = $('<input id="attribute" type="text" value="' + text + '" />');
                    var edit = $(this).parent().find('span:first').attr('class');
                    $(this).parent().find('span:first').text('').append(input);
                    input.select();
                    input.blur(function () {
                        var text = $('#attribute').val();
                        $.ajax({
                            type: "POST",
                            url: edit == "cfn" ? "API.asmx/UpdateCFN" : "API.asmx/UpdateRefDate",
                            data: edit == "cfn" ? JSON.stringify({ formId: formId, cfn: text }) : JSON.stringify({ formId: formId, refDate: text }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            beforeSend: function () {
                                $('#spinner').show();
                            },
                            success: function (data) {
                                $('#tblQuestionnaires').DataTable().row($(this).parents('tr')).invalidate().draw();
                                $('#spinner').hide();
                            },
                            error: function (r) {
                                $('#spinner').hide();
                            }
                        });
                    });
                } else if (className == "download") {
                    
                    $.ajax({
                            type: "POST",
                            url: "API.asmx/DownloadReferral",
                            data: JSON.stringify({ refId: formId }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            beforeSend: function () {
                                $('#spinner').show();
                            },
                            success: function (data) {
                                //var hiddenElement = document.createElement('a');
                                //hiddenElement.href = 'data:text/csv;charset=utf-8,' + encodeURI(data.data);
                                //hiddenElement.target = '_blank';
                                //hiddenElement.download = data.fileName + ".csv";
                                //hiddenElement.click();
                                download_file("\ufeff" + data.data, data.fileName, 'text/csv;encoding:utf-8');  
                                $('#spinner').hide();
                            },
                            error: function (r) {
                                $('#spinner').hide();
                            }
                        });
                }
            });
            

        });

        function download_file(content, fileName, mimeType) {
            var a = document.createElement('a');
            mimeType = mimeType || 'application/octet-stream';

            if (navigator.msSaveBlob) { // IE10
                navigator.msSaveBlob(new Blob([content], {
                    type: mimeType
                }), fileName);
            } else if (URL && 'download' in a) { //html5 A[download]
                a.href = URL.createObjectURL(new Blob([content], {
                    type: mimeType
                }));
                a.setAttribute('download', fileName);
                document.body.appendChild(a);
                a.click();
                document.body.removeChild(a);
            } else {
        
                location.href = 'data:application/octet-stream,' + encodeURIComponent(content); // only this mime type is supported
            }
        }
    </script>
    <asp:UpdatePanel ID="upAlert" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <wet:WetAlert ID="alert" CssClass="mrgn-tp-md" runat="server" EnableViewState="false" Visible="false"></wet:WetAlert>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="row mrgn-tp-md">
        

        
        <div class="col-md-12">

            <div class="panel panel-primary">
                <div class="panel-heading">
                    <asp:Label CssClass="h3" runat="server" Text="Advanced Search"></asp:Label>
                </div>
                <asp:Panel ID="pnlCallBody" CssClass="panel-body" DefaultButton="btnSearch" runat="server">
                    <div class="row">
                        <div class="col-md-2">
                            <wet:WetDropDownList ID="servicesDDL" CssClass="full-width" LabelText="Services" runat="server"></wet:WetDropDownList>
                        </div>
                        <div class="col-md-2">
                            <wet:WetDropDownList ID="acceptanceStatusDDL" CssClass="full-width" LabelText="Acceptance status" runat="server">
                                <asp:ListItem Value="0">== Please Select ==</asp:ListItem>
                                <asp:listitem Value="1">Accepted</asp:listitem>
                                <asp:listitem Value="2">Not Accepted</asp:listitem>
                                <asp:listitem Value="3">Pending Information</asp:listitem>
                            </wet:WetDropDownList>
                        </div>
                        <div class="col-md-2">
                            <wet:WetDropDownList ID="completionStatusDDL" CssClass="full-width" LabelText="Completion status" runat="server">
                                <asp:ListItem Value="0">== Please Select ==</asp:ListItem>
                                <asp:listitem Value="4">Completed</asp:listitem>
                                <asp:listitem Value="5">Not Completed</asp:listitem>
                                <asp:listitem Value="9">Ongoing</asp:listitem>
                            </wet:WetDropDownList>
                        </div>
                        <div class="col-md-2">
                            <wet:WetTextBox ID="refDateStart" runat="server" IsDate="true" LabelText="Start Referral date"></wet:WetTextBox>
                        </div>
                        <div class="col-md-2">
                            <wet:WetTextBox ID="refDateEnd" runat="server" IsDate="true" LabelText="End Referral date"></wet:WetTextBox>
                        </div>
                        
                    </div>
                    <asp:UpdatePanel runat="server" class="row">
                        <ContentTemplate>
                        <div class="col-md-2">
                            <wet:WetDropDownList ID="RegionsDDL" runat="server" LabelText="Regions" AutoPostBack="true" OnSelectedIndexChanged="RegionsDDL_SelectedIndexChanged"></wet:WetDropDownList>
                        </div>
                        <div class="col-md-2">
                            <wet:WetDropDownList ID="Communities" runat="server" LabelText="Recipient" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="Communities_SelectedIndexChanged"></wet:WetDropDownList>
                        </div>
                        <div class="col-md-2">
                            <wet:WetDropDownList ID="Programs" runat="server"  LabelText="Program" Visible="false"></wet:WetDropDownList>
                        </div>
                            </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="row mrgn-lft-sm">
                            <wet:WetButton ID="btnClear" CssClass="btnMarginTop" ButtonType="Default" OnClick="btnClear_Click" Text="Clear" runat="server" />
                            <wet:WetButton ID="btnSearch" ClientIDMode="Static" UseSubmitBehavior="false" CssClass="btnMarginTop" ButtonType="Primary" Text="Search" runat="server" />
                    </div>
                </asp:Panel>
            </div>

        </div>
          

    </div>

    <asp:UpdatePanel runat="server" class="mrgn-tp-md" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="panel panel-default">
                <div class="panel-heading">
                    <asp:Label CssClass="h3" runat="server" Text="Client/Referral Reporting"></asp:Label>                   
                </div>
                <wet:WetLinkButton ID="btnCreateForm" PostBackUrl="AddReferral.aspx" runat="server" CssClass="pull-right mrgn-tp-md mrgn-rght-md" ButtonType="Primary"><asp:Label runat="server">Add referral</asp:Label></wet:WetLinkButton>
                <div class="mrgn-tp-lg mrgn-lft-md mrgn-rght-md">
                    <table id="tblQuestionnaires" class="table wb-tables table-striped table-hover table-responsive">
                        <thead>
                            <tr>
                                <th>FormId</th>
                                <th><asp:Label runat="server">Referral</asp:Label></th>  
                                <th><asp:Label runat="server">Client File Number</asp:Label></th>
                                <th><asp:Label runat="server">Referral date</asp:Label></th>
                                <th><asp:Label runat="server">Acceptance status</asp:Label></th>
                                <th><asp:Label runat="server">Completion status</asp:Label></th>
                                <th><asp:Label runat="server">Creator</asp:Label></th>
                                <th></th>
                            </tr>
                                        
                        </thead>
                        <tbody>
                        </tbody>
                    </table>                
                </div>
            </div> 
           
      </ContentTemplate>   
    </asp:UpdatePanel>
             <div id="spinner" class="spinner" style="display:none">
                Loading&#8230;
            <div class="cube1"></div>
            <div class="cube2"></div>            
        </div>
</asp:Content>
