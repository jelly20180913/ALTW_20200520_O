$(document).ready(function () {  
  
    $('#Review').bind('click', function () {
        $(location).attr('href', webSiteServer + 'view/Budget/ReviewReport.html');
    });
    if (sessionStorage.getItem("Account") == 'L120001') {
        $("#approve_html").html("when you want to approve ,please press F5(refresh page) first ");
        getFileVersionBudget();
    }
    if (sessionStorage.getItem("Account") == 'L070003') {
        getHeadCountHR();
    }
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
    $('#factory .dropdown-item').click(function (event) {
        event.preventDefault();
        $('#dropdownMenuButton_factory').html(this.innerHTML);
        getLoginUploadReport(this.innerHTML);

        $('#dropdownMenuButton_department').html("Department");
        $('#dropdownMenuButton_budget').html("Budget");
        $('#budgetType').empty();
        //var _Row = "    <a   class='#budget dropdown-item' href='#'>please choose department </a>" + 
        //$('#budgetType').append(_Row);
    });
    $('#fake-file-button-browse').bind('click', function () {
        $('#files-input-upload').click();
    });
    $('#files-input-upload').bind('change', function () {
        $('#fake-file-input-name').val(this.value);
        $('#fake-file-button-upload').removeAttr('disabled');
    });
    $('#fake-file-button-upload').on('click', function () {
        if (!checkDropDownList()) return
        var _ItemId = $('#budgetType').val();
        if ($('#dropdownMenuButton_commonCost').html().trim() != "Common Cost") _ItemId = $('#commonCostType').val();
        var data = new FormData();
        var files = $("#files-input-upload").get(0).files;

        if (checkData(files[0].name)) {
            // Add the uploaded image content to the form data collection
            if (files.length > 0) {
                data.append("UploadedImage", files[0]);
                data.append("LoginID", sessionStorage.getItem("LoginId"));
                data.append("ItemId", _ItemId); 
                data.append("DepartmentId", $('#departmentName').val().trim());
                data.append("Factory", $('#dropdownMenuButton_factory').html().trim());
                data.append("Account", sessionStorage.getItem("Account"));
            }
            $('#loadIng').css("display", "block");
            // Make Ajax request with the contentType = false, and procesDate = false
            var ajaxRequest = $.ajax({
                type: "POST",
                url: apiServer + "BudgetExcelUpload",
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
    var checkDropDownList = function () {
        var ok = true;
        if ($('#dropdownMenuButton_factory').html().trim() == "Factory" && $('#dropdownMenuButton_commonCost').html().trim() == "Common Cost") {
            alert("please choose factory");
            ok = false;
        }
        if ($('#dropdownMenuButton_department').html().trim() == "Department" && $('#dropdownMenuButton_commonCost').html().trim() == "Common Cost") {
            alert("please choose department");
            ok = false;
        }
        if ($('#dropdownMenuButton_budget').html().trim() == "Budget" && $('#dropdownMenuButton_commonCost').html().trim() == "Common Cost") {
            alert("please choose budget");
            ok = false;
        }
        return ok;
    }
});
function getLoginUploadReport(factory) {
    var ajaxRequest = $.ajax({
        type: "GET",
        url: apiServer + "BudgetLoginUploadReport",
        dataType: 'json',
        data: { account: sessionStorage.getItem("Account"), factory: factory },
        headers: {
            Authorization: 'Bearer ' + sessionStorage.getItem("Token")
        },
    });

    ajaxRequest.done(function (responseData) {
        if (responseData.Status == "OK") {
            initialDropdownDepartment(responseData);
            getLoginUploadCommonCost(factory);
        } else {
            alert(responseData.ErrorMessage);
        }
    });

}
function getLoginUploadCommonCost(factory) {
    var ajaxRequest = $.ajax({
        type: "GET",
        url: apiServer + "BudgetLoginUploadCommonCost",
        dataType: 'json',
        data: { account: sessionStorage.getItem("Account"), factory: factory },
        headers: {
            Authorization: 'Bearer ' + sessionStorage.getItem("Token")
        },
    });
    ajaxRequest.done(function (responseData) {
        if (responseData.Status == "OK") {
            initialDropdownCommonCost(responseData);
        } else {
            alert(responseData.ErrorMessage);
        }
    });
}
//L070003 for Peggy use 
async function getHeadCountHR() {
    let _Type = await getItemCatalog_UpdateMode("HR");
    let _DirectType = await getItemCatalog_UpdateMode("DirectType");
    let _DepartmentName = await getDepartment_UpdateMode("DepartmentName");
    let _JobFunction = await getItemCatalog_UpdateMode("JobFunction");
    let _JobFunctionKSZ = await getItemCatalog_UpdateMode("JobFunctionKSZ");
    let _JobFunctionMerge = _JobFunction.concat(_JobFunctionKSZ);
    let _Title = await getItemCatalog_UpdateMode("Title");
    let _TitleKSZ = await getItemCatalog_UpdateMode("TitleKSZ");
    let _TitleMerge = _Title.concat(_TitleKSZ);
    var columnDefs = [{
        title: "Id",
        type: "readonly"
    }, { 
        title: "Account",
        type: "text"
    }, {
        title: "Name",
        type: "text"
    }, { 
        title: "Type",
        type: "select",
        "options": _Type
    }, { 
        title: "DirectType",
        type: "select",
        "options": _DirectType
    }, 
    {
        title: "JobFunction",
        type: "select",
        "options": _JobFunctionMerge
    },
    {
        title: "Title",
        type: "select",
        "options": _TitleMerge
    }
    ];

    var ajaxRequest = $.ajax({
        type: "GET",
        url: apiServer + "HeadCountHR",
        headers: {
            Authorization: 'Bearer ' + sessionStorage.getItem("Token")
        },
    });
    ajaxRequest.done(function (responseData) {
        if (responseData.Status == "OK") {
            // initialDatatable_HeadCountHR(responseData);
            var _DataSet = initDataSet_HeadCountHR(responseData);
            //freeze header column 
            $('#table_id').DataTable({
                "order": [[1, "desc"]],
                fixedHeader: true,
                select: true,
                pageLength: 25,
                "sPaginationType": "full_numbers",
                dom: 'Bfrtip',        // Needs button container
                select: 'single',
                responsive: true,
                altEditor: true,     // Enable altEditor
                columns: columnDefs,
                data: _DataSet,
                buttons: [
                    {
                        extend: 'selected', // Bind to Selected row
                        text: 'Edit',
                        name: 'edit'        // do not change name
                    }
                ],
                onEditRow: function (datatable, rowdata, success, error) {
                    putHeadCountHR(rowdata); 
                },
                //scrollY: "600px",
                //scrollX: true,
                //scrollCollapse: true,
                //paging: false,
                //fixedColumns: {
                //    leftColumns: 1,
                //    rightColumns: 1
                //}

            });
        } else {
            alert(responseData.ErrorMessage);
        }
    });

}
function initDataSet_HeadCountHR(responseData) {
    let _DataSet = [];
    for (i = 0; i < responseData.Data.length; i++) {
        let _Data = [];
        _Data.push(responseData.Data[i].Id); 
        _Data.push(responseData.Data[i].Account);
        _Data.push(responseData.Data[i].AltwName); 
        _Data.push(responseData.Data[i].HR); 
        _Data.push(responseData.Data[i].DirectType); 
        _Data.push(responseData.Data[i].JobFunction);
        _Data.push(responseData.Data[i].Title);
        _DataSet.push(_Data);
    }
    return _DataSet;
}
function putHeadCountHR(rowdata) {
    var ajaxRequest = $.ajax({
        type: "PUT",
        url: apiServer + "HeadCountHR",
        data: { 
              Id: rowdata[0],  Account: rowdata[1], AltwName: rowdata[2],  HR: rowdata[3],  DirectType: rowdata[4], JobFunction: rowdata[5], Title: rowdata[6]
        },
        headers: {
            Authorization: 'Bearer ' + sessionStorage.getItem("Token")
        },
    });
    ajaxRequest.done(function (responseData) {
        if (responseData.Status == "OK") {
            $('#errorTxt').val("modify success!!");
            location.reload();
        } else {
            alert(responseData.ErrorMessage);
        }
    });

}
function getFileVersionBudget() {
    var ajaxRequest = $.ajax({
        type: "GET",
        url: apiServer + "BudgetFileVersionBudget",
        headers: {
            Authorization: 'Bearer ' + sessionStorage.getItem("Token")
        },
    });
    ajaxRequest.done(function (responseData) {
        if (responseData.Status == "OK") {
            initialDatatable(responseData);
            //freeze header column 
            $('#table_id').DataTable({
                fixedHeader: true
            });
        } else {
            alert(responseData.ErrorMessage);
        }
    });

}
function getDepartmentReport(factory, departmentId) {
    var ajaxRequest = $.ajax({
        type: "GET",
        url: apiServer + "BudgetDepartmentReport",
        dataType: 'json',
        data: { factory: factory, departmentId: departmentId },
        headers: {
            Authorization: 'Bearer ' + sessionStorage.getItem("Token")
        },
    });

    ajaxRequest.done(function (responseData) {
        if (responseData.Status == "OK") {
            initialDropdown(responseData);

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
//update employment data use this function to get item list and need async
var getItemCatalog_UpdateMode = function (className) {
    let _Item = [];
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "GET",
            url: apiServer + "ItemCatalog",
            dataType: 'json',
            data: { className: className },
            headers: {
                Authorization: 'Bearer ' + sessionStorage.getItem("Token")
            },
            success: function (responseData) {
                if (responseData.Status == "OK") {
                    for (i = 0; i < responseData.Data.length; i++) {
                        _Item.push(responseData.Data[i].Name);
                    }
                }
                resolve(_Item)
            },
            error: function (error) {
                reject(error)
            },
        })
    })
}
var getDepartment_UpdateMode = function () {
    let _Department = [];
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "GET",
            url: apiServer + "BudgetDepartmentReport",
            dataType: 'json',
            headers: {
                Authorization: 'Bearer ' + sessionStorage.getItem("Token")
            },
            success: function (responseData) {
                if (responseData.Status == "OK") {
                    for (i = 0; i < responseData.Data.length; i++) {
                        _Department.push(responseData.Data[i].DepartmentName);
                    }
                } resolve(_Department)
            }, error: function (error) {
                reject(error)
            },
        })
    })
}
var initialDatatable = function (responseData) {
    $('#fileVersionBudgetHead').empty();
    $('#fileVersionBudget').empty();
    var _Head = " <tr>  " +
        "  <th>Id</th>        " +
        "  <th>Account</th>      " +
        "  <th>Year</th>      " +
        "  <th>Version</th>      " +
        "  <th>Type</th>      " +
        "  <th>Factory</th>      " +
        "  <th>Department</th>      " +
        "  <th>Name</th>      " +
        "  <th >Approve</th> " +
        "</tr>";
    $('#fileVersionBudgetHead').append(_Head);
    for (i = 0; i < responseData.Data.length; i++) {
        let _Approve = "";
        if (!responseData.Data[i].Approve)
            _Approve = "<button  id = '1' value=" + responseData.Data[i].Id + " onclick='approve(this)'>Approve</button>";
        else _Approve = "";
        var _Row = " <tr><td> " + responseData.Data[i].Id + "  </td>" +
            "  <td> " + responseData.Data[i].Account + "  </td> " +
            "  <td> " + responseData.Data[i].Date + "  </td> " +
            "  <td> " + responseData.Data[i].Version + "  </td> " +
            "  <td> " + responseData.Data[i].BudgetName + "  </td> " +
            "  <td> " + responseData.Data[i].Factory + "  </td> " +
            "  <td> " + responseData.Data[i].DepartmentName + "  </td> " +
            "  <td> " + responseData.Data[i].AltwName + "  </td> " +
            "  <td> " + _Approve + "  </td></tr> ";
        $('#fileVersionBudget').append(_Row);
    }

}
//no use 
var initialDatatable_HeadCountHR = function (responseData) {
    $('#fileVersionBudgetHead').empty();
    $('#fileVersionBudget').empty();
    var _Head = " <tr>  " +
        "  <th>Id</th>        " +
        "  <th>DepartmentId</th>        " +
        "  <th>No</th>      " +
        "  <th>AltwName</th>      " +
        "  <th>Staff</th>      " +
        "  <th>Type</th>      " +
        "  <th>PartTime</th>      " +
        "  <th>DirectType</th>      " +
        "  <th>Department</th>      " +
        "  <th>JobFunction</th>      " +
        "  <th >Title</th> " +
        "</tr>";
    $('#fileVersionBudgetHead').append(_Head);
    for (i = 0; i < responseData.Data.length; i++) {
        var _Row = " <tr><td> " + responseData.Data[i].Id + "  </td>" +
            "  <td> " + responseData.Data[i].DepartmentId + "  </td> " +
            "  <td> " + responseData.Data[i].Account + "  </td> " +
            "  <td> " + responseData.Data[i].AltwName + "  </td> " +
            "  <td> " + responseData.Data[i].DirectStaff + "  </td> " +
            "  <td> " + responseData.Data[i].HR + "  </td> " +
            "  <td> " + responseData.Data[i].PartTime + "  </td> " +
            "  <td> " + responseData.Data[i].DirectType + "  </td> " +
            "  <td> " + responseData.Data[i].DepartmentName + "  </td> " +
            "  <td> " + responseData.Data[i].JobFunction + "  </td> " +
            "  <td> " + responseData.Data[i].Title + "  </td></tr> ";
        $('#fileVersionBudget').append(_Row);
    }

}
//Iris approve method
function approve(myObj) {
    var ajaxRequest = $.ajax({
        type: "Put",
        url: apiServer + "BudgetFileVersionBudget",
        dataType: 'json',
        data: { Id: myObj.value },
        headers: {
            Authorization: 'Bearer ' + sessionStorage.getItem("Token")
        },
    });
    ajaxRequest.done(function (responseData) {
        if (responseData.Status == "OK") {
            $('#errorTxt').val("approve success!!");
            location.reload();
        } else {
            alert(responseData.ErrorMessage);
        }
    });
}
//get ItemCatalog  table  about Common Cost authentication for this account can upload
var initialDropdownCommonCost = function (responseData) {
    $('#commonCostType').empty();
    for (i = 0; i < responseData.Data.length; i++) {
        var _Row = "    <a   class='#commonCost dropdown-item' href='#'>" + responseData.Data[i].Name + "</a>" +
            " <input type='hidden' id=itemId" + i + "   value=" + responseData.Data[i].ItemId + "> ";
        $('#commonCostType').append(_Row);
    }
    if (sessionStorage.getItem("Account") == 'K040005' && $('#dropdownMenuButton_factory').html().trim() == 'KSZ') {
        var _Row = "    <a   class='#commonCost dropdown-item' href='#'>臨時工</a>" +
            " <input type='hidden' id=itemId00   value='PartTime' > ";
        $('#commonCostType').append(_Row);
    }
    $('#commonCost .dropdown-item').click(function (event) {
        event.preventDefault();
        $('#dropdownMenuButton_commonCost').html(this.innerHTML);
        $('#commonCostType').val($(this).next().val());
        $('#dropdownMenuButton_budget').html("Budget");
    });
}
//get LoginUploadReport table  about department authentication for this account can upload
var initialDropdownDepartment = function (responseData) {
    $('#departmentName').empty();
    for (i = 0; i < responseData.Data.length; i++) {
        var _Row = "    <a   class='#department dropdown-item' href='#'>" + responseData.Data[i].DepartmentName + "</a>" +
            " <input type='hidden' id=itemId" + i + "   value=" + responseData.Data[i].DepartmentId + "> ";
        $('#departmentName').append(_Row);
    }
    $('#department .dropdown-item').click(function (event) {
        event.preventDefault();
        $('#dropdownMenuButton_department').html(this.innerHTML);
        $('#departmentName').val($(this).next().val());
        getDepartmentReport($('#dropdownMenuButton_factory').html().trim(), $('#departmentName').val().trim())
        $('#dropdownMenuButton_budget').html("Budget");
    });
}

//get DepartmentReport table about the budget type authentication for this account can upload
var initialDropdown = function (responseData) {
    $('#budgetType').empty();
    if (responseData.Data.length > 0) {
        if (responseData.Data[0].DeptExpense == '1') {
            var _Row = "    <a   class='#budget dropdown-item' href='#'>DeptExpense</a>" +
                " <input type='hidden' id=itemId1   value='DeptExpense'> ";
            $('#budgetType').append(_Row);
        }
        if (responseData.Data[0].Scrap == '1') {
            var _Row = "    <a   class='#budget dropdown-item' href='#'>Scrap</a>" +
                " <input type='hidden' id=itemId2   value='Scrap'> ";
            $('#budgetType').append(_Row);
        }
        if (responseData.Data[0].Travelling == '1') {
            var _Row = "    <a   class='#budget dropdown-item' href='#'>Travelling</a>" +
                " <input type='hidden' id=itemId3   value='Travelling'> ";
            $('#budgetType').append(_Row);
        }
        if (responseData.Data[0].KPI == '1') {
            var _Row = "    <a   class='#budget dropdown-item' href='#'>KPI</a>" +
                " <input type='hidden' id=itemId4   value='KPI'> ";
            $('#budgetType').append(_Row);
        }
        if (responseData.Data[0].Headcount == '1') {
            var _Row = "    <a   class='#budget dropdown-item' href='#'>Headcount</a>" +
                " <input type='hidden' id=itemId5   value='Headcount'> ";
            $('#budgetType').append(_Row);
        }
        if (responseData.Data[0].Capex == '1') {
            var _Row = "    <a   class='#budget dropdown-item' href='#'>Capex</a>" +
                " <input type='hidden' id=itemId6   value='Capex'> ";
            $('#budgetType').append(_Row);
        }
    }
    $('#budget .dropdown-item').click(function (event) {
        event.preventDefault();
        $('#dropdownMenuButton_budget').html(this.innerHTML);
        $('#budgetType').val($(this).next().val());
        $('#dropdownMenuButton_commonCost').html("Common Cost");
    });
}
