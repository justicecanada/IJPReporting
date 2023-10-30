function pageLoad(sender, args) {
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_beginRequest(BeginRequestHandler);
    prm.add_endRequest(EndRequestHandler);
    
}

function BeginRequestHandler(sender, args) {
    $('#spinner').show();
}

function EndRequestHandler(sender, args) {
    $('#spinner').hide();
}
