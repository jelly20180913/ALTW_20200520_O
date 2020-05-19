$(document).ready(function () {
    let endtime = new Date();
    $('#dropdownMenuButton_month').html(Appendzero_AIPG(endtime.getMonth()));
    function Appendzero_AIPG(obj) {
        obj = obj + 1; 
        if (obj < 10) return "0" + "" + obj;
        else return obj;
    }
    $("#Change").bind('click', function () {
        $("#dialog-change").dialog({
            resizable: false,
            height: 200,
            modal: true,
            buttons: {
                "Yes": function () {
                    let password = $("#pwd").val();
                    postLoginPassword(password);
                    $("#pwd").css("background-color", "#D6D6FF");
                    $(this).dialog("close");
                },
                Cancel: function () {
                    $(this).dialog("close");
                }
            }
        });
    });
    $('#btnQuery').on('click', function () {
        $('#report').empty();
        $('#reportDownload').empty();
        var _Date = $('#dropdownMenuButton').html().trim() + $('#dropdownMenuButton_month').html().trim();
        //$('#loadIng').css("display", "block");
        // Make Ajax request with the contentType = false, and procesDate = false
        var ajaxRequest = $.ajax({
            type: "GET",
            url: apiServer + "ReportFile",
            dataType: 'json',
            data: { date: _Date, LoginId: sessionStorage.getItem("LoginId") },
            headers: {
                Authorization: 'Bearer ' + sessionStorage.getItem("Token")
            },
        });

        ajaxRequest.done(function (responseData) {
            if (responseData.Status == "OK") {
                if (responseData.Data.length == 0) alert("No Data");
                else dynamicReportLink(responseData);
            } else {
                alert(responseData.ErrorMessage);
            }
        });
    }); 
    $('#btnDownloadAll').on('click', function () {
        $("#dialog-confirm").dialog({
            resizable: false,
            height: 200,
            modal: true,
            buttons: {
                "Yes": function () {
                     $(this).dialog("close");
                     $('.btn-download > a').each(function () {  
                     alert("download "+$(this).html()); 
                     $(location).attr('href', $(this).attr("href"));
                     });
                },
                Cancel: function () {
                    $(this).dialog("close");
                }
            }
        });  
    });
    var dynamicReportLink = function (responseData) {
        var _Date = $('#dropdownMenuButton').html().trim() +"/"+ $('#dropdownMenuButton_month').html().trim();
        var _LinkCount =999;
        var _Block = Math.round(responseData.Data.length / _LinkCount);
        if (responseData.Data.length <= 999) _Block = 1;
        for (j = 0; j < _Block; j++) {
            var _Row = "  <div class='col-xs-12 col-sm-12'> <div class='c01-0'> <div> <div class='v-title'><h3><span class='v-f22'> &nbsp;</span>" + _Date + "</h3></div> " +
                   "  </div> <ul id=reportLink" + j + " >   </ul>  </div> </div>  ";
            $('#report').append(_Row);
        }
        for (i = 0; i < responseData.Data.length; i++) {
            var _Index = Math.floor(i / _LinkCount);
            var _Link;
            if (responseData.Data[i].CanDownload)   {
                _Link = "<div  class='btn-download'>  <a  class='fake-button-download v-black' style='text-decoration:none;' href='" + apiServer + "download?Id=" + responseData.Data[i].Id + "&Token=" + sessionStorage.getItem("Token") + "&LoginID=" + sessionStorage.getItem("LoginId") + "' target='_blank' title='download' "
                  +">    " + responseData.Data[i].FileName + "</a>  </div> "; 
                $('#reportLink' + _Index).append(_Link);
            }
           
        }

    }
   
});
function postLoginPassword(password) {
    var ajaxRequest = $.ajax({
        type: "Post",
        url: apiServer + "Login",
        dataType: 'json',
        data: { Id: sessionStorage.getItem("LoginId"), password: password },
        headers: {
            Authorization: 'Bearer ' + sessionStorage.getItem("Token")
        },
    });
    ajaxRequest.done(function (responseData) {
        if (responseData.Status == "OK") {
            $('#errorTxt').val("change password complete!!");
        } else {
            alert(responseData.ErrorMessage);
        }
    });
}