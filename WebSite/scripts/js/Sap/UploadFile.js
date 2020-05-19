$(document).ready(function () {  
    $("#getprice").bind('click', function () { 
        getPriceListByDate(''); ;
    });
    $("#excludetax").bind('click', function () {
        getPriceListExcludeTax();;
    });
    $('#fake-file-button-browse').bind('click', function () {
        $('#files-input-upload').click();
    });
    $('#files-input-upload').bind('change', function () {
        $('#fake-file-input-name').val(this.value);
        $('#fake-file-button-upload').removeAttr('disabled');
    });
    $('#pricetype .dropdown-item').click(function (event) {
        event.preventDefault();
        $('#dropdownMenuButton_pricetype').html(this.innerHTML); 
    });
    $('#fake-file-button-upload').on('click', function () {
        var data = new FormData();
        var files = $("#files-input-upload").get(0).files;
        if (checkData(files[0].name)) {
            // Add the uploaded image content to the form data collection
            if (files.length > 0) {
                data.append("UploadedImage", files[0]);
                data.append("LoginID", sessionStorage.getItem("LoginId"));
                data.append("Account", sessionStorage.getItem("Account"));
                data.append("PriceType", $('#dropdownMenuButton_pricetype').html().trim());
            }
            $('#loadIng').css("display", "block");
            // Make Ajax request with the contentType = false, and procesDate = false
            var ajaxRequest = $.ajax({
                type: "POST",
                url: apiServer + "SAPExcelUpload",
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
                    $('#errorTxt').val(responseData.ErrorMessage);
                }
                cleanValue();
            });
        }
        else {
            alert("please upload excel file");
        }
    });
    var checkData = function (filesName) {
        var fileName, ok = true;
        extIndex = filesName.lastIndexOf('.');
        if (extIndex != -1) {
            fileName = filesName.substr(extIndex + 1, filesName.length);
        }
        if ((fileName != "xlsx") && (fileName != "xls")) ok = false;
        return ok;
    }
    var cleanValue = function () {
        $("#fake-file-input-name").val('');
        $('#loadIng').css("display", "none");
        $("#files-input-upload").val('');
    }
});
function getPriceListByDate(date) {
    var ajaxRequest = $.ajax({
        type: "GET",
        url: apiServer + "SAP_PriceList",
        data: {date:date},
        headers: {
            Authorization: 'Bearer ' + sessionStorage.getItem("Token")
        },
    });
    ajaxRequest.done(function (responseData) {
        if (responseData.Status == "OK") {
           
        } else {
            alert(responseData.ErrorMessage);
        }
    });

}
function getPriceListExcludeTax() {
    var ajaxRequest = $.ajax({
        type: "GET",
        url: apiServer + "SAP_PriceList",
        headers: {
            Authorization: 'Bearer ' + sessionStorage.getItem("Token")
        },
    });
    ajaxRequest.done(function (responseData) {
        if (responseData.Status == "OK") {
            alert("Exclude Tax Success!");
        } else {
            alert(responseData.ErrorMessage);
        }
    });

}
