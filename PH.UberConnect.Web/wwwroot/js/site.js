// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
let main_host_site = window.location.protocol + "//" + window.location.host;

function ServerPOST(requestUrl, requestData, functionHandler) {
    $.ajax({
        type: "POST",
        url: main_host_site + requestUrl,
        data: requestData,
        error: function (jqXHR, textStatus, errorThrown) {
            functionHandler(jqXHR.responseText, false);
        },
        success: function (data) {
            functionHandler(data, true);
        }
    });
}