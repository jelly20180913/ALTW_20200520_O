$(document).ready(function () {
    if ($.url.param("Show") == 'Go') {
        let _Type = $.url.param("Type");
        let _Month = $.url.param("Month");
        let _Quarter = $.url.param("Quarter");
        let _YY = $.url.param("YY");
        let _Region = $.url.param("Region");
        let _Group = $.url.param("Group");
        let _GroupKey = $.url.param("GroupKey");
        $('#dropdownMenuButton_type').html(_Type);
        $('#dropdownMenuButton_month').html(_Month);
        $('#dropdownMenuButton_quarter').html(_Quarter);
        $('#dropdownMenuButton_yy').html(_YY);
        $('#dropdownMenuButton_region').html(_Region);
        $('#dropdownMenuButton_group').html(_Group);
        $('#dropdownMenuButton_series').html(_GroupKey);
        submitData(_Type, _Month, _Quarter, _Group, _YY, _Region, _GroupKey);
    }
    else if ($.url.param("Show") == 'Pos') {
        let _DateStart = $.url.param("DateStart");
        let _DateEnd = $.url.param("DateEnd");
        $('#dateStart').val(_DateStart);
        $('#dateEnd').val(_DateEnd);
        $('#loadIng').css("display", "block");
        getPosList(_DateStart, _DateEnd);
    }
    getItemCatalog("Series");
    if (sessionStorage.getItem("Account") != 'posadmin') {
        $('#pos_upload').css("display", "none");
    }
    $('#dateStart').datepicker({
        uiLibrary: 'bootstrap4'
    });
    $('#dateEnd').datepicker({
        uiLibrary: 'bootstrap4'
    });
    $('#Submit').bind('click', function () {
        let _Type = $('#dropdownMenuButton_type').text().trim();
        let _Month = $('#dropdownMenuButton_month').text().trim();
        let _Quarter = $('#dropdownMenuButton_quarter').text().trim();
        let _Group = $('#dropdownMenuButton_group').text().trim();
        let _YY = $('#dropdownMenuButton_yy').text().trim();
        let _Region = $('#dropdownMenuButton_region').text().trim();
        let _GroupKey = $('#dropdownMenuButton_series').text().trim();
        $(location).attr('href', webSiteServer + 'View/Edi/UploadFile.html?Show=Go&Type=' + _Type + '&Month=' + _Month + '&Quarter=' + _Quarter + '&Group=' + _Group + '&YY=' + _YY + '&Region=' + _Region + '&GroupKey=' + _GroupKey);
        
    });
    $('#Pos').bind('click', function () {
        let _DateStart = $('#dateStart').val().trim();
        let _DateEnd = $('#dateEnd').val().trim();
        if ((_DateStart == "SHIP DATE Start") || (_DateEnd == "SHIP DATE End")) {
            alert("please choose date ");
        } else { $(location).attr('href', webSiteServer + 'View/Edi/UploadFile.html?Show=Pos&DateStart=' + _DateStart + '&DateEnd=' + _DateEnd); }
    });
    $('#PosToExcel').bind('click', function () {
        let _DateStart = $('#dateStart').val().trim();
        let _DateEnd = $('#dateEnd').val().trim();
        if ((_DateStart == "SHIP DATE Start") || (_DateEnd == "SHIP DATE End")) {
            alert("please choose date ");
        } else {
            $(location).attr('href', apiServer + 'DownloadPos?Token=' + sessionStorage.getItem("Token") + "&dateStart=" + _DateStart + "&dateEnd=" + _DateEnd + "&LoginID=" + sessionStorage.getItem("LoginId"));
        }
    });
    $("#Change").bind('click', function () {
        $("#dialog-confirm").dialog({
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
    $('#type .dropdown-item').click(function (event) {
        event.preventDefault();
        $('#dropdownMenuButton_type').html(this.innerHTML);
    });
    $('#month .dropdown-item').click(function (event) {
        event.preventDefault();
        $('#dropdownMenuButton_month').html(this.innerHTML);
    });
    $('#quarter .dropdown-item').click(function (event) {
        event.preventDefault();
        $('#dropdownMenuButton_quarter').html(this.innerHTML);
    });
    $('#yy .dropdown-item').click(function (event) {
        event.preventDefault();
        $('#dropdownMenuButton_yy').html(this.innerHTML);
    });
    $('#region .dropdown-item').click(function (event) {
        event.preventDefault();
        $('#dropdownMenuButton_region').html(this.innerHTML);
    });
    $('#group .dropdown-item').click(function (event) {
        event.preventDefault();
        $('#dropdownMenuButton_group').html(this.innerHTML);
    });
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
        if (checkData(files[0].name)) {
            // Add the uploaded image content to the form data collection
            if (files.length > 0) {
                data.append("UploadedImage", files[0]);
                data.append("LoginID", sessionStorage.getItem("LoginId"));
                data.append("Account", sessionStorage.getItem("Account"));
            }
            $('#loadIng').css("display", "block");
            // Make Ajax request with the contentType = false, and procesDate = false
            var ajaxRequest = $.ajax({
                type: "POST",
                url: apiServer + "EdiPos",
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
async function getPosList(dateStart, dateEnd) {
    var columnDefs = [{
        title: "Id",
        type: "text"
    }, {
        title: "NAME",
        type: "text"
        }, {
        title: "Market Code",
        type: "text"
    }, {
        title: "Market Segment",
        type: "text"
    },
    {
        title: "Sub-Segment Code",
        type: "text"
    },
    {
        title: "Sub-Segment",
        type: "text"
    }, {
        title: "Type",
        type: "text"
    }, {
        title: "DIST",
        type: "text"
    }, {
        title: "Address",
        type: "text"
    }, {
        title: "CITY",
        type: "text"
    }, {
        title: "STATE",
        type: "text"
    }, {
        title: "ZIP",
        type: "text"
    }, {
        title: "Series",
        type: "text"
    }, {
        title: "ALTW PARTNO",
        type: "text"
    }, {
        title: "QTY",
        type: "text"
    }, {
        title: "COST",
        type: "text"
    }, {
        title: "PRICE",
        type: "text"
    },
    {
        title: "RES.EXT",
        type: "text"
    }, {
        title: "ACCT.",
        type: "text"
    }, {
        title: "Remarks",
        type: "text"
    }, {
        title: "SHIP DATE",
        type: "text"
    }, {
        title: "SHIP DATE2",
        type: "text"
    }, {
        title: "SHIP MONTH",
        type: "text"
    }, {
        title: "SHIP QUARTER",
        type: "text"
    }, {
        title: "COUNTRY CODE",
        type: "text"
        },{
        title: "COUNTRY ",
        type: "text"
    }, {
        title: "Region",
        type: "text"
    }, {
        title: "UpdateTime",
        type: "readonly"
        } ,{
            title: "Corporate Market",
            type: "text"
        }, {
            title: "YY",
            type: "text"
        }
    ];

    var ajaxRequest = $.ajax({
        type: "GET",
        url: apiServer + "EdiPos",
        data: {
            dateStart: dateStart, dateEnd: dateEnd, LoginID: sessionStorage.getItem("LoginId")
        },
        headers: {
            Authorization: 'Bearer ' + sessionStorage.getItem("Token")
        },
    });
    ajaxRequest.done(function (responseData) {
        if (responseData.Status == "OK") {
            var _DataSet = initDataSet_Pos(responseData);
            //freeze header column 
            var mode = false;
            if (sessionStorage.getItem("Account") == 'posadmin') {
                initDataTable_Admin(columnDefs, _DataSet, mode);
            }
            else { initDataTable(columnDefs, _DataSet); }
            $('#loadIng').css("display", "none");
        } else {
            alert(responseData.ErrorMessage);
        }
    });

}
function initDataTable_Admin(columnDefs, _DataSet) {
    $('#table_id').DataTable({
        "order": [[1, "desc"]],
        fixedHeader: true,
        select: true,
        pageLength: 20,
        "sPaginationType": "full_numbers",
        dom: 'Bfrtip',        // Needs button container
        select: 'single',
        responsive: true,
        altEditor: true,     // Enable altEditor
        columns: columnDefs,
        data: _DataSet,
        //paging: false,
        buttons: [
            {
                extend: 'selected', // Bind to Selected row
                text: 'Edit',
                name: 'edit'        // do not change name
            },
            {
                text: "Edit by Name",
                name: 'add'
            }
        ],
        onEditRow: function (datatable, rowdata, success, error) {
            putEdiPos(rowdata);
        },
        onAddRow: function (datatable, rowdata, success, error) {
            if (rowdata[1] == "") alert("please input customer name!\r\n you can update column below \r\n[MarketCode][Market][SubSegmentCode][SubSegment][Address] ");
            else putEdiPos(rowdata);
        },
    });
}
function initDataTable(columnDefs, _DataSet) {
    $('#table_id').DataTable({
        "order": [[1, "desc"]],
        fixedHeader: true,
        select: true,
        pageLength: 20,
        "sPaginationType": "full_numbers",
        dom: 'Bfrtip',        // Needs button container
        select: 'single',
        responsive: true,
        altEditor: true,     // Enable altEditor
        columns: columnDefs,
        data: _DataSet,
        //paging: false,
        buttons: [
            {
                text: "Feedback",
                name: 'add'
            }
        ],
        onAddRow: function (datatable, rowdata, success, error) {
            if (rowdata[0] == "") alert("please input Id");
            else postEmailPos(rowdata);
        },
    });
}
function initDataSet_Pos(responseData) {
    let _DataSet = [];
    for (i = 0; i < responseData.Data.length; i++) {
        let _Data = [];
        _Data.push(responseData.Data[i].Id);
        _Data.push(responseData.Data[i].CustomerName);
        _Data.push(responseData.Data[i].MarketCode);
        _Data.push(responseData.Data[i].Market);
        _Data.push(responseData.Data[i].SubSegmentCode);
        _Data.push(responseData.Data[i].SubSegment);
        _Data.push(responseData.Data[i].Type);
        _Data.push(responseData.Data[i].Distributor);
        _Data.push(responseData.Data[i].Address);
        _Data.push(responseData.Data[i].City);
        _Data.push(responseData.Data[i].State);
        _Data.push(responseData.Data[i].ZIP);
        _Data.push(responseData.Data[i].Series);
        _Data.push(responseData.Data[i].PartNo);
        _Data.push(responseData.Data[i].Quantity);
        _Data.push(responseData.Data[i].Cost);
        _Data.push(responseData.Data[i].Price);
        _Data.push(responseData.Data[i].ResellingExt);
        _Data.push(responseData.Data[i].ACCT);
        _Data.push(responseData.Data[i].Remarks);
        _Data.push(responseData.Data[i].ShipDate);
        _Data.push(responseData.Data[i].ShipDate2);
        _Data.push(responseData.Data[i].ShipMonth);
        _Data.push(responseData.Data[i].ShipQuarter);
        _Data.push(responseData.Data[i].CountryCode);
        _Data.push(responseData.Data[i].Country);
        _Data.push(responseData.Data[i].Region);
        _Data.push(responseData.Data[i].UpdateTime);
        _Data.push(responseData.Data[i].CorporateMarket);
        _Data.push(responseData.Data[i].YY);
        _DataSet.push(_Data);
    }
    return _DataSet;
}
//send correct data to asiya 
function postEmailPos(rowdata) {
    var ajaxRequest = $.ajax({
        type: "POST",
        url: apiServer + "EmailPos",
        data: {
            Id: rowdata[0], CustomerName: rowdata[1], MarketCode: rowdata[2], Market: rowdata[3],
            SubSegmentCode: rowdata[4], SubSegment: rowdata[5], Type: rowdata[6], Distributor: rowdata[7],
            Address: rowdata[8], City: rowdata[9], State: rowdata[10], ZIP: rowdata[11], Series: rowdata[12],
            PartNo: rowdata[13], Quantity: rowdata[14], Cost: rowdata[15], Price: rowdata[16], ResellingExt: rowdata[17],
            ACCT: rowdata[18], Remarks: rowdata[19], ShipDate: rowdata[20], ShipDate2: rowdata[21], ShipMonth: rowdata[22],
            ShipQuarter: rowdata[23], CountryCode: rowdata[24], Country: rowdata[25], Region: rowdata[26],
            LoginID: sessionStorage.getItem("LoginId")
        },
        headers: {
            Authorization: 'Bearer ' + sessionStorage.getItem("Token")
        },
    });
    ajaxRequest.done(function (responseData) {
        if (responseData.Status == "OK") {
            alert("Your feedback is very helpful ,Thanks!");
        } else {
            alert(responseData.ErrorMessage);
        }
    });

}
//update pos data 
function putEdiPos(rowdata) {
    var ajaxRequest = $.ajax({
        type: "PUT",
        url: apiServer + "EdiPos",
        data: {
            Id: rowdata[0], CustomerName: rowdata[1], MarketCode: rowdata[2], Market: rowdata[3],
            SubSegmentCode: rowdata[4], SubSegment: rowdata[5], Type: rowdata[6], Distributor: rowdata[7],
            Address: rowdata[8], City: rowdata[9], State: rowdata[10], ZIP: rowdata[11], Series: rowdata[12],
            PartNo: rowdata[13], Quantity: rowdata[14], Cost: rowdata[15], Price: rowdata[16], ResellingExt: rowdata[17],
            ACCT: rowdata[18], Remarks: rowdata[19], ShipDate: rowdata[20], ShipDate2: rowdata[21], ShipMonth: rowdata[22],
            ShipQuarter: rowdata[23], CountryCode: rowdata[24], Country: rowdata[25], Region: rowdata[26], CorporateMarket: rowdata[28]
            , YY: rowdata[29],
            LoginID: sessionStorage.getItem("LoginId")
        },
        headers: {
            Authorization: 'Bearer ' + sessionStorage.getItem("Token")
        },
    });
    ajaxRequest.done(function (responseData) {
        if (responseData.Status == "OK") {
            alert("modify success! you can click POS button to refresh data ");
            //location.reload();
        } else {
            alert(responseData.ErrorMessage);
        }
    });

}
function postLoginPassword(password) {
    var ajaxRequest = $.ajax({
        type: "Post",
        url: apiServer + "Login",
        dataType: 'json',
        data: { Id: sessionStorage.getItem("LoginId"), password: password, LoginID: sessionStorage.getItem("LoginId") },
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
var submitData = function (type, month, quarter, group, yy, region, groupBy) {
    getGroupBy(type, month, quarter, group, yy, region, groupBy);
}

async function getGroupBy(_Type, _Month, _Quarter, _Group, _YY, _Region, _GroupKey) {
    var columnDefs = [{
        title: "Group",
        type: "readonly"
    }, {
        title: "Q-ty",
        type: "readonly"
    }
    ];

    var ajaxRequest = $.ajax({
        type: "GET",
        url: apiServer + "EdiPos",
        data: { type: _Type, month: _Month, quarter: _Quarter, group: _Group, LoginID: sessionStorage.getItem("LoginId"), yy: _YY, region: _Region, groupKey: _GroupKey},
        headers: {
            Authorization: 'Bearer ' + sessionStorage.getItem("Token")
        },
    });
    ajaxRequest.done(function (responseData) {
        if (responseData.Status == "OK") {
            var _DataSet = initDataSet_PosGroupBy(responseData);
            //freeze header column 
            $('#table_id_group').DataTable({
                "order": [[1, "desc"]],
                retrieve: true,
                fixedHeader: true,
                select: true,
                pageLength: 100,
                "sPaginationType": "full_numbers",
                dom: 'Bfrtip',        // Needs button container
                select: 'single',
                responsive: true,
                altEditor: true,     // Enable altEditor
                columns: columnDefs,
                data: _DataSet,
                paging: false,
                buttons: [
                    {
                        extend: 'collection',
                        text: 'Export',
                        buttons: [
                            'copy',
                            'excel',
                            'csv',
                            'pdf',
                            'print'
                        ]
                    }
                ],
            });
        } else {
            alert(responseData.ErrorMessage);
        }
    });

}
function initDataSet_PosGroupBy(responseData) {
    let _DataSet = [];
    for (i = 0; i < responseData.Data.length; i++) {
        let _Data = [];
        _Data.push(responseData.Data[i].Name);
        _Data.push(responseData.Data[i].Qty);
        _DataSet.push(_Data);
    }
    return _DataSet;
}
var initialDropdown = function (responseData) { 

    for (i = 0; i < responseData.Data.length; i++) {
        var _Row = "    <a   class='#series dropdown-item' href='#'>" + responseData.Data[i].Name + "</a>" +
            " <input type='hidden' id=itemId" + i + "   value=" + responseData.Data[i].ItemId + "> ";
        $('#seriesType').append(_Row);
    }
    $('#seriesType .dropdown-item').click(function (event) {
        event.preventDefault();
        $('#dropdownMenuButton_series').html(this.innerHTML); 
    });
}
function getItemCatalog(className) {
    var ajaxRequest = $.ajax({
        type: "GET",
        url: apiServer + "ItemCatalog",
        dataType: 'json',
        data: { className: className },
        headers: {
            Authorization: 'Bearer ' + sessionStorage.getItem("Token")
        },
    });

    ajaxRequest.done(function (responseData) {
        if (responseData.Status == "OK" && className != "CostCommon") {
            initialDropdown(responseData);
        } else {
            alert(responseData.ErrorMessage);
        }
    });

}

