$(document).ready(function () {
    getPosOrderMappingModel();
    $('#fake-file-button-browse').bind('click', function () {
        $('#files-input-upload').click();
    });
    $('#files-input-upload').bind('change', function () {
        $('#fake-file-input-name').val(this.value);
        $('#fake-file-button-upload').removeAttr('disabled');
    });
    $('#fake-file-button-upload').on('click', function () {
        if ($('#dropdownMenuButton').html().trim() == "Model")
        {
            alert("please select the mapping model");
            return; 
        } 
        var data = new FormData(); 
        var files = $("#files-input-upload").get(0).files;
        if (checkData(files[0].name)) {
            // Add the uploaded image content to the form data collection
            if (files.length > 0) {
                data.append("UploadedImage", files[0]);
                data.append("LoginID", sessionStorage.getItem("LoginId"));
                data.append("Model", $('#posOrderMappingModel').val());
            }
            $('#loadIng').css("display", "block");
            // Make Ajax request with the contentType = false, and procesDate = false
            var ajaxRequest = $.ajax({
                type: "POST",
                url: apiServer +"File",
                contentType: false,
                processData: false,
                data: data,
                headers: {
                    Authorization: 'Bearer ' + sessionStorage.getItem("Token")
            },
            });

            ajaxRequest.done(function (responseData) {
                if (responseData.Status == "OK") {
                    alert("file upload success ,spend:"+responseData.Data); 
                } else {
                    alert(responseData.ErrorMessage);
                    $('#errorTxt').val(responseData.ErrorMessage);
                }
                cleanValue();
            });
        }
        else {
            alert("please upload excel or txt file");
        }
    });
    var checkData = function (filesName) {
        var fileName, ok = true;
        extIndex = filesName.lastIndexOf('.');
        if (extIndex != -1) {
            fileName = filesName.substr(extIndex + 1, filesName.length);
        }
        if ((fileName != "xlsx") && (fileName != "xls") && (fileName != "txt") && (fileName != "TXT")) ok = false;
        return ok;
    }
    var cleanValue = function () {
        $("#fake-file-input-name").val('');
        $('#loadIng').css("display", "none");
        $("#files-input-upload").val('');
    }
   
});
function getPosOrderMappingModel( ) {  
        // Make Ajax request with the contentType = false, and procesDate = false
        var ajaxRequest = $.ajax({
            type: "GET",
            url: apiServer + "PosOrderMapping",
            dataType: 'json', 
            headers: {
                Authorization: 'Bearer ' + sessionStorage.getItem("Token")
            },
        });

        ajaxRequest.done(function (responseData) {
            if (responseData.Status == "OK") {
               initialModelDropdown(responseData);
            } else {
                alert(responseData.ErrorMessage);
            }
        });
   
}
var initialModelDropdown = function (responseData) { 
    for (i = 0; i < responseData.Data.length; i++) { 
        var _Row = "    <a   class='dropdown-item' href='#'>" + responseData.Data[i].ModelName + "</a>"+
        " <input type='hidden' id=modelId" + i + "   value=" + responseData.Data[i].Id + "> ";
        $('#posOrderMappingModel').append(_Row);
    }
    $('.dropdown-item').click(function (event) { 
        event.preventDefault();
        $('#dropdownMenuButton').html(this.innerHTML); 
        $('#posOrderMappingModel').val($(this).next().val());
    });
}