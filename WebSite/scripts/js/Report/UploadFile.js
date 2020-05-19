$(document).ready(function () {
    var endtime = new Date();
    $('#dropdownMenuButton').html(AdjustDate(endtime.getFullYear(), endtime.getMonth()));
    $('#dropdownMenuButton_month').html(Appendzero(endtime.getMonth()));
    function AdjustDate(obj, obj2) {
        if (obj2 == 0) return obj - 1;
        else return obj;
    }
    function Appendzero(obj) {
        if (obj == 0) return "12";
        if (obj < 10) return "0" + "" + obj;
        else return obj;
    }
    $('#fake-file-button-browse').bind('click', function () {
        $('#files-input-upload').click();
    });
    $('#files-input-upload').bind('change', function () {
        $('#fake-file-input-name').val(this.value);
        $('#fake-file-button-upload').removeAttr('disabled');
    });
    $('#fake-file-button-upload').on('click', function () {

        var data = new FormData(); 
        var files = $("#files-input-upload").get(0).files;
        var date = $('#dropdownMenuButton').html().trim() + $('#dropdownMenuButton_month').html().trim();
        if (checkDate(date)) {
            if (checkData(files[0].name)) {
                // Add the uploaded image content to the form data collection
                if (files.length > 0) {
                    data.append("UploadedImage", files[0]);
                    data.append("LoginID", sessionStorage.getItem("LoginId"));
                    data.append("Date", date);
                }
                $('#loadIng').css("display", "block");
                // Make Ajax request with the contentType = false, and procesDate = false
                var ajaxRequest = $.ajax({
                    type: "POST",
                    url: apiServer + "ReportFile",
                    contentType: false,
                    processData: false,
                    data: data,
                    headers: {
                        Authorization: 'Bearer ' + sessionStorage.getItem("Token")
                    },
                });

                ajaxRequest.done(function (responseData) {
                    if (responseData.Status == "OK") {
                        $('#errorTxt').val("file upload success:" + files[0].name);
                    } else {
                        alert(responseData.ErrorMessage);
                        $('#errorTxt').val(responseData.ErrorMessage);
                    }
                    cleanValue();
                });
            }
            else {
                alert("please upload excel/ppt/jpg/png file");
            }
        }
        else {
            alert("please select report date");
        }

      
    });
    var checkData = function (filesName) {
        var fileName, ok = true;
        extIndex = filesName.lastIndexOf('.');
        if (extIndex != -1) {
            fileName = filesName.substr(extIndex + 1, filesName.length);
        }
        if ((fileName != "xlsx") && (fileName != "XLSX") && (fileName != "xls") && (fileName != "pptx") && (fileName != "JPG") && (fileName != "jpg") && (fileName != "PNG") && (fileName != "png")) ok = false;
        return ok;
    }
    var cleanValue = function () {
        $("#fake-file-input-name").val('');
        $('#loadIng').css("display", "none");
        $("#files-input-upload").val('');
    } 
    $('#year .dropdown-item').click(function (event) {
        event.preventDefault();
        $('#dropdownMenuButton').html(this.innerHTML); 
    });
    $('#month .dropdown-item').click(function (event) {
        event.preventDefault();
        $('#dropdownMenuButton_month').html(this.innerHTML); 
    });
    var checkDate = function (date) {
        var date, ok = true;
        if (date.indexOf("Year") > 0 || date.indexOf("Month") > 0) ok = false;
        return ok;
    }
});