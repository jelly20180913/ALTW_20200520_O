$(document).ready(function () {
    var endtime = new Date(); 
    $('#dropdownMenuButton').html(AdjustDate(endtime.getFullYear(), endtime.getMonth()));
    $('#dropdownMenuButton_month').html(Appendzero(endtime.getMonth()));

    function AdjustDate(obj,obj2) {
        if (obj2 == 0) return obj-1;
        else return obj;
    }
    function Appendzero(obj)
    { 
        if (obj == 0) return "12";
        if(obj<10) return "0" +""+ obj;
        else return obj;
    }
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
            data: { date: _Date, LoginId: sessionStorage.getItem("LoginId")},
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
    $('#year .dropdown-item').click(function (event) {
        event.preventDefault();
        $('#dropdownMenuButton').html(this.innerHTML);
    });
    $('#month .dropdown-item').click(function (event) {
        event.preventDefault();
        $('#dropdownMenuButton_month').html(this.innerHTML);
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
            if (!responseData.Data[i].CanDownload) {
                _Link = "  <li> <a class='v-black' href='MonthReport.html?PicName=" + responseData.Data[i].ServerSimpleFileName + "&Date=" + responseData.Data[i].Date + "' target='_blank' title="
                    + responseData.Data[i].FileName + ">  <i class='fas fa-caret-right'></i> " + responseData.Data[i].FileName + "</a> </li> ";
                $('#reportLink' + _Index).append(_Link);
            }
            else {
                _Link = "<div  class='btn-download'>  <a  class='fake-button-download v-black' style='text-decoration:none;' href='" + apiServer + "download?Id=" + responseData.Data[i].Id + "&Token=" + sessionStorage.getItem("Token") + "&LoginID=" + sessionStorage.getItem("LoginId")  + "' target='_blank' title='download' "
                  +">    " + responseData.Data[i].FileName + "</a>  </div> "; 
              $('#reportDownload' ).append(_Link);
            }
           
        }

    }
   
});