var budgetreportvalue = "1.Dept. Expense";


function LTSelectall_Click() {
    var LTDeptList = document.getElementsByName("LTDept");
    for (var i = 0; i < LTDeptList.length; i++) {
        LTDeptList[i].checked = true;
    }
}
function LTSelectnone_Click() {
    var LTDeptList = document.getElementsByName("LTDept");
    for (var i = 0; i < LTDeptList.length; i++) {
        LTDeptList[i].checked = false;
    }
}

function KSSelectall_Click() {
    var KSDeptList = document.getElementsByName("KSDept");
    for (var i = 0; i < KSDeptList.length; i++) {
        KSDeptList[i].checked = true;
    }
}
function KSSelectnone_Click() {
    var KSDeptList = document.getElementsByName("KSDept");
    for (var i = 0; i < KSDeptList.length; i++) {
        KSDeptList[i].checked = false;
    }
}

function btnInputQuery_Click() {
    if ($('#dropdownMenuButton_BudgetReport').html().trim() == "BudgetReport▼") { alert("Please choose BudgetReport first."); return; }
    document.getElementById("InputQuery").style.visibility = "visible";
    document.getElementById("btnQuery").style.visibility = "visible";
    if (getBudgetReportValue($('#dropdownMenuButton_BudgetReport').html()) == "5") { document.getElementById("trTBH").style.visibility = "visible"; }
    else { document.getElementById("trTBH").style.visibility = "hidden"; }
}

function btnClose_Click() {
    document.getElementById("InputQuery").style.visibility = "hidden";
    document.getElementById("btnQuery").style.visibility = "hidden";
    document.getElementById("trTBH").style.visibility = "hidden";
}

function isIE() {
    if (!!window.ActiveXObject || "ActiveXObject" in window) {
        return true;
    } else {
        return false;
    }
}

function btnExportExcel_Click() {
    var htmltable = document.getElementById('ReportDataTable');
    var html = htmltable.outerHTML;
    var regex = /<br\s*[\/]?>/gi;
    html = html.replace(regex, ' ');
    var style1 = "<STYLE>  td { vnd.ms-excel.numberformat:@ } </STYLE>";
    var charset1 = "<meta http-equiv='Content-Type' content='text/html; charset=utf-8'>";

    html = "<head>" + charset1 + "</head>" + html;

    //blob convert
    var excelBlob = new Blob([html], {type: 'application/vnd.ms-excel'});
    var fileName = "Budget.xls";
    if(isIE()){
        window.navigator.msSaveOrOpenBlob(excelBlob,fileName);
    }else{
        var oa = document.createElement('a');
        oa.href = URL.createObjectURL(excelBlob);
        oa.download = fileName;
        document.body.appendChild(oa);
        oa.click();
    }
 
    //var regex = /<br\s*[\/]?>/gi;
    //html = html.replace(regex, ' ');
    //var style1 = "<STYLE>  td { vnd.ms-excel.numberformat:@ } </STYLE>";
    //var charset1 = "<meta http-equiv='Content-Type' content='text/html; charset=utf-8'>";
    //window.open("data:application/vnd.ms-excel," + "<head>" + style1 + charset1 + "</head>" + encodeURIComponent(html));
}

function FormatNumber(datastr) {
    // alert(datastr);
    var retstr = "";
    if ((!isNaN(datastr)) && (datastr.toString().length != 10)) {
        retstr = datastr.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    }
    else { retstr = datastr.toString(); }
    return retstr;
}
var getBudgetReportValue = function (budgetreport) {
    var retval;
    switch (budgetreport) {
        case "Dept. Expense":
            retval = "1";
            break;
        case "Scrap":
            retval = "2";
            break;
        case "Travelling":
            retval = "3";
            break;
        case "KPI":
            retval = "4";
            break;
        case "Headcount":
            retval = "5";
            break;
        case "Capex":
            retval = "6";
            break;
    }
    return retval;
}

$(document).ready(function (report, headertext) {

    $('#BudgetReport .dropdown-item').click(function (event) {
        event.preventDefault();
        $('#dropdownMenuButton_BudgetReport').html(this.innerHTML);
        budgetreportvalue = this.innerHTML;

        ChangeGroupBy();

    });

    $('#dropdownMenuButton_BudgetReport').click(function () {

        document.getElementById("InputQuery").style.visibility = "hidden";
        document.getElementById("btnQuery").style.visibility = "hidden";
        document.getElementById("trTBH").style.visibility = "hidden";
    });

    var setheadertext = function (report, headertext) {

        switch (report) {
            case "1":
                if (headertext == "Factory") { headertext = "Factory<BR>廠 別"; }
                if (headertext == "DeptCode") { headertext = "Dept. Code<BR>部門代號"; }
                if (headertext == "DeptName") { headertext = "Dept. Name<BR>部門名稱"; }
                if (headertext == "CostCode") { headertext = "Cost Code"; }
                if (headertext == "Desc") { headertext = "Desc. 名稱"; }
                if (headertext == "YTDJuly") { headertext = "YTD July"; }
                break;
            case "2":
                if (headertext == "Factory") { headertext = "Factory<BR>廠 別"; }
                if (headertext == "DepartmentName") { headertext = "Department Name<BR>部 門"; }
                if (headertext == "ScrapType") { headertext = "Type<BR>類型"; }
                if (headertext == "PN") { headertext = "PN or Asset No.<BR>料號或資產編號"; }
                if (headertext == "Reason") { headertext = "Scrap Reason<BR>處份原因"; }
                if (headertext == "Month") { headertext = "Month<BR>月份"; }
                if (headertext == "QTY") { headertext = "QTY<BR>數量"; }
                if (headertext == "OriginalCost") { headertext = "Original Cost<BR>原購買價格"; }
                if (headertext == "SellingAmount") { headertext = "Selling Amount<BR>出售金額"; }
                break;
            case "3":
                if (headertext == "Factory") { headertext = "Factory<BR>廠 別"; }
                if (headertext == "DepartmentName") { headertext = "Department Name<BR>部 門"; }
                if (headertext == "Employee") { headertext = "Employee<BR>員工姓名"; }
                if (headertext == "Country") { headertext = "Country<BR>國家"; }
                if (headertext == "Days") { headertext = "Days<BR>天數"; }
                if (headertext == "Purpose") { headertext = "Purpose<BR>目的"; }
                if (headertext == "TravellingType") { headertext = "Type<BR>類型"; }
                if (headertext == "Remark") { headertext = "Remark<BR>備註"; }
                break;
            case "4":
                if (headertext == "GoalsObjectives") { headertext = "Goals/Objectives"; }
                if (headertext == "LastYearActual") { headertext = "Last Year<BR>Actual"; }
                if (headertext == "ThisYearYTD") { headertext = "This Year<BR>YTD"; }
                break;
            case "5":
                if (headertext == "DepartmentName") { headertext = "Department Name"; }
                if (headertext == "EmployeeName") { headertext = "Employee<BR>Name"; }
                if (headertext == "FunctionDesc") { headertext = "Func. Desc."; }
                if (headertext == "DLIDL") { headertext = "DL/IDL"; }
                if (headertext == "FOH_ADM_Selling_RD") { headertext = "FOH/ADM/Selling/RD"; }
                break;
            case "6":
                if (headertext == "Factory") { headertext = "Factory<BR>廠 別"; }
                if (headertext == "DepartmentName") { headertext = "Department Name<BR>部 門"; }
                if (headertext == "AssetExp") { headertext = "Asset/Exp<BR>資產或費用"; }
                if (headertext == "AssetExpType") { headertext = "Type<BR>類型"; }
                if (headertext == "Purpose") { headertext = "Purpose<BR>目的"; }
                if (headertext == "InternalOrder") { headertext = "Internal Order<BR>內部訂單號"; }
                if (headertext == "ProjectName") { headertext = "Project Name<BR>專案名稱"; }
                if (headertext == "Remark") { headertext = "Remark<BR>專案備註"; }
                if (headertext == "ProjectBudget") { headertext = "Project Budget<BR>專案總預算"; }
                if (headertext == "YTDJuly") { headertext = "YTD July"; }
                break;
        }

        if (headertext.indexOf("Bud") >= 0 && headertext.length == 6) { headertext = headertext.replace("Bud", " Bud"); }
        if (headertext.indexOf("TY") >= 0 && headertext.length == 5) { headertext = headertext.replace("TY", " TY"); }

        return headertext;
    }

    var ChangeGroupBy = function () {
        var strGroupby = "";
        //alert(budgetreportvalue);
        //alert($('#dropdownMenuButton_BudgetReport').html());
        switch (getBudgetReportValue($('#dropdownMenuButton_BudgetReport').html())) {
            case "1":
                strGroupby += "<option value='1'>Factory 廠別 + Dept. 部門 + CostCode 成本要素</option>";
                break;
            case "2":
                strGroupby += "<option value='1'>Factory廠別+Dept.部門+Type類別+PN or Asset No.料號/資產編號+Scrap Reason處份原因+QTY數量+Original Cost原購買價格+Selling Amount出售金額</option>";
                break;
            case "3":
                strGroupby += "<option value='0'>所有欄位 incl. all columns/information</option>";
                strGroupby += "<option value='1'>Employee 員工姓名</option>";
                strGroupby += "<option value='2'>Employee 員工姓名 + Country 國家 + Purpose 目的</option>";
                strGroupby += "<option value='3'>Employee 員工姓名 + Country 國家 + Days 天數 + Purpose 目的</option>";
                strGroupby += "<option value='4'>Factory 廠別 + Dept. 部門名稱 + Type 類型</option>";
                break;
            case "4":
                strGroupby += "<option value='1'>None 無</option>";
                break;
            case "5":
                strGroupby += "<option value='0'>所有欄位 incl. all columns/information</option>";
                strGroupby += "<option value='1'>Factory 廠別 + DL/IDL</option>";
                strGroupby += "<option value='2'>Factory 廠別 + Dept. 部門 + DL/IDL</option>";
                strGroupby += "<option value='3'>Factory 廠別 + Dept. 部門 + Employee Name + Job Func + Title + FOH/ADM/Selling/RD + DL/IDL</option>";
                strGroupby += "<option value='4'>Factory 廠別 + Dept. 部門 + Job Func + Title + FOH/ADM/Selling/RD + DL/IDL</option>";
                strGroupby += "<option value='5'>Factory 廠別 + FOH/ADM/Selling/RD</option>";
                strGroupby += "<option value='6'>Factory 廠別 + Dept. 部門 + FOH/ADM/Selling/RD</option>";
                strGroupby += "<option value='7'>Factory 廠別 + Dept. 部門 + Employee Name + Title</option>";
                strGroupby += "<option value='8'>Factory 廠別 + Dept. 部門 + Employee Name + Job Func  + Title</option>";
                strGroupby += "<option value='9'>Factory 廠別 + Title + FOH/ADM/Selling/RD + DL/IDL</option>";
                strGroupby += "<option value='A'>Factory 廠別 + Employee Name + Title+ FOH/ADM/Selling/RD + DL/IDL</option>";
                break;
            case "6":
                strGroupby += "<option value='0'>所有欄位 incl. all columns/information</option>";
                strGroupby += "<option value='1'>Factory 廠別 + Dept. 部門 + Type 類型</option>";
                strGroupby += "<option value='2'>Factory 廠別 + Dept. 部門 + Asset/Exp 資產或費用 + Type 類型 + Purpose 目的</option>";
                strGroupby += "<option value='3'>Factory 廠別 + Dept. 部門 + Asset/Exp 資產或費用 + Project Name 專案名稱</option>";
                strGroupby += "<option value='4'>Factory 廠別 + Dept. 部門 + Asset/Exp 資產或費用 + Project Name 專案名稱 + Type 類型 + Purpose 目的</option>";
                break;

        }
        document.getElementById("QueryGroupby").innerHTML = strGroupby;
    }

    var ChangeDepartment = function () {
        var data = { account: sessionStorage.getItem("Account"), report: getBudgetReportValue($('#dropdownMenuButton_BudgetReport').html()) }

        var ajaxRequest = $.ajax({
            type: "POST",
            url: apiServer + "LoginReviewPermission",
            contentType: "application/json",
            dataType: 'json',
            data: JSON.stringify(data),
            headers: {
                Authorization: 'Bearer ' + sessionStorage.getItem("Token")
            },
        });

        ajaxRequest.done(function (responseData) {
            if (responseData.Status == "OK") {

                //alert(JSON.stringify(responseData.Data));
                if (responseData.Data.length == 0) {
                    alert("Sorry,you can't review this report.");
                    document.getElementById("KSZDeptList").innerHTML = "";
                    document.getElementById("LTDeptList").innerHTML = "";
                    //document.getElementById("btnQuery").style.visibility = "hidden";
                }
                else {
                    var KSZDeptListObj = responseData.Data.filter(element => element.Factory == "KSZ");
                    var KSZDeptListStr = "";
                    for (var i = 0; i < KSZDeptListObj.length; i++) {
                        KSZDeptListStr += "<input type='checkbox' name='KSDept' value='" + KSZDeptListObj[i].DepartmentId + "'>" + KSZDeptListObj[i].DepartmentName + "<br />";
                    }
                    document.getElementById("KSZDeptList").innerHTML = KSZDeptListStr;

                    var LTDeptListObj = responseData.Data.filter(element => element.Factory == "LT");
                    var LTDeptListStr = "";
                    for (var i = 0; i < LTDeptListObj.length; i++) {
                        LTDeptListStr += "<input type='checkbox' name='LTDept' value='" + LTDeptListObj[i].DepartmentId + "'>" + LTDeptListObj[i].DepartmentName + "<br />";
                    }
                    document.getElementById("LTDeptList").innerHTML = LTDeptListStr;

                    //if (LTDeptListObj.length > 0 || KSDeptListObj.length > 0) {
                    //    document.getElementById("btnQuery").style.visibility = "visible";
                    //}
                    //else {
                    //    document.getElementById("btnQuery").style.visibility = "hidden";
                    //}
                }
            } else {
                alert(responseData.ErrorMessage);
            }
        });
    }

    $('#UploadFile').bind('click', function () {
        $(location).attr('href', webSiteServer + 'view/Budget/UploadFile.html');
    });

    //$('#BudgetReport').on('change', function () {
    //    ChangeDepartment();
    //    ChangeGroupBy();
    //    if (getBudgetReportValue($('#dropdownMenuButton_BudgetReport').html()) == "5") { document.getElementById("trTBH").style.visibility = "visible"; }
    //    else { document.getElementById("trTBH").style.visibility = "hidden"; }
    //});

    $('#btnQuery').on('click', function () {
        var arrKSZDept = new Array();
        var arrLTDept = new Array();
        $("input[name='KSDept']").each(function () {
            if ($(this).prop("checked")) {
                arrKSZDept.push($(this).val());
            }
        });
        $("input[name='LTDept']").each(function () {

            if ($(this).prop("checked")) {
                arrLTDept.push($(this).val());
            }
        });

        if (arrKSZDept.length == 0 && arrLTDept.length == 0) {
            alert("Sorry,you don't select any department.");
            return;
        }
        if (getBudgetReportValue($('#dropdownMenuButton_BudgetReport').html()) == "4") {
            if (arrKSZDept.length + arrLTDept.length != 1) {
                alert("Sorry,you just can select one department for KPI report.");
                return;
            }
        }
        var JsonObj = { "Account": sessionStorage.getItem("Account"), "Report": getBudgetReportValue($('#dropdownMenuButton_BudgetReport').html()), "BudgetYear": $('#BudgetYear').val(), "LTDept": arrLTDept, "KSZDept": arrKSZDept, "QueryGroupby": $('#QueryGroupby').val(), "TBH": $('#chTBH').val() };
        //alert(JSON.stringify(JsonObj));

        var ajaxRequest = $.ajax({
            type: "POST",
            url: apiServer + "ReviewReport",
            contentType: "application/json",
            dataType: 'json',
            data: JSON.stringify(JsonObj),
            headers: {
                Authorization: 'Bearer ' + sessionStorage.getItem("Token")
            },
        });
        ajaxRequest.done(function (responseData) {
            if (responseData.Status == "OK") {
                //alert(JSON.stringify(responseData.Data));
                document.getElementById("ReportDataTable").innerHTML = "";
                if (responseData.Data.length == 0) {
                    alert("Sorry,No Data of this report.");
                }
                else {
                    var cols = Object.keys(responseData.Data[0]).length;
                    var keyarr = Object.keys(responseData.Data[0]);

                    var tablestr = "";
                    if (JsonObj.Report == "1") {
                        var textcols = parseInt(cols) - 13
                        var TotalYTDJuly = 0;
                        var TotalJanBud = 0;
                        var TotalFebBud = 0;
                        var TotalMarBud = 0;
                        var TotalAprBud = 0;
                        var TotalMayBud = 0;
                        var TotalJunBud = 0;
                        var TotalJulBud = 0;
                        var TotalAugBud = 0;
                        var TotalSepBud = 0;
                        var TotalOctBud = 0;
                        var TotalNovBud = 0;
                        var TotalDecBud = 0;

                        for (var t = 0; t < responseData.Data.length; t++) {
                            TotalYTDJuly += responseData.Data[t].YTDJuly;
                            TotalJanBud += responseData.Data[t].JanBud;
                            TotalFebBud += responseData.Data[t].FebBud;
                            TotalMarBud += responseData.Data[t].MarBud;
                            TotalAprBud += responseData.Data[t].AprBud;
                            TotalMayBud += responseData.Data[t].MayBud;
                            TotalJunBud += responseData.Data[t].JunBud;
                            TotalJulBud += responseData.Data[t].JulBud;
                            TotalAugBud += responseData.Data[t].AugBud;
                            TotalSepBud += responseData.Data[t].SepBud;
                            TotalOctBud += responseData.Data[t].OctBud;
                            TotalNovBud += responseData.Data[t].NovBud;
                            TotalDecBud += responseData.Data[t].DecBud;
                        };
                        tablestr += "<tr class='trTotal'><td class='tdTotal' align=right nowrap colspan='" + textcols.toString() + "' align='right'>Total</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalYTDJuly.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalJanBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalFebBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalMarBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalAprBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalMayBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalJunBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalJulBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalAugBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalSepBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalOctBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalNovBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalDecBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap><span id='AllTotalBud'></span></td>";
                        tablestr += "</tr>";
                    }
                    if (JsonObj.Report == "2") {
                        var textcols = parseInt(cols) - 3
                        var TotalQTY = 0;
                        var TotalOC = 0;
                        var TotalSA = 0;
                        for (var t = 0; t < responseData.Data.length; t++) {
                            TotalQTY += responseData.Data[t].QTY;
                            TotalOC += responseData.Data[t].OriginalCost;
                            TotalSA += responseData.Data[t].SellingAmount;
                        };
                        tablestr += "<tr class='trTotal'><td class='tdTotal' align=right nowrap colspan='" + textcols.toString() + "' align='right'>Total</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalQTY.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalOC.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalSA.toString()) + "</td>";
                        tablestr += "</tr>";
                    }
                    if (JsonObj.Report == "3") {
                        var textcols = parseInt(cols) - 12
                        var TotalJanBud = 0;
                        var TotalFebBud = 0;
                        var TotalMarBud = 0;
                        var TotalAprBud = 0;
                        var TotalMayBud = 0;
                        var TotalJunBud = 0;
                        var TotalJulBud = 0;
                        var TotalAugBud = 0;
                        var TotalSepBud = 0;
                        var TotalOctBud = 0;
                        var TotalNovBud = 0;
                        var TotalDecBud = 0;

                        for (var t = 0; t < responseData.Data.length; t++) {
                            TotalJanBud += responseData.Data[t].JanBud;
                            TotalFebBud += responseData.Data[t].FebBud;
                            TotalMarBud += responseData.Data[t].MarBud;
                            TotalAprBud += responseData.Data[t].AprBud;
                            TotalMayBud += responseData.Data[t].MayBud;
                            TotalJunBud += responseData.Data[t].JunBud;
                            TotalJulBud += responseData.Data[t].JulBud;
                            TotalAugBud += responseData.Data[t].AugBud;
                            TotalSepBud += responseData.Data[t].SepBud;
                            TotalOctBud += responseData.Data[t].OctBud;
                            TotalNovBud += responseData.Data[t].NovBud;
                            TotalDecBud += responseData.Data[t].DecBud;
                        };
                        tablestr += "<tr class='trTotal'><td class='tdTotal' align=right nowrap colspan='" + textcols.toString() + "' align='right'>Total</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalJanBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalFebBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalMarBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalAprBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalMayBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalJunBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalJulBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalAugBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalSepBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalOctBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalNovBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalDecBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap><span id='AllTotalBud'></span></td>";
                        tablestr += "</tr>";
                    }
                    if (JsonObj.Report == "5") {
                        var textcols = parseInt(cols) - 18
                        var TotalJulTY = 0;
                        var TotalAugTY = 0;
                        var TotalSepTY = 0;
                        var TotalOctTY = 0;
                        var TotalNovTY = 0;
                        var TotalDecTY = 0;

                        var TotalJanBud = 0;
                        var TotalFebBud = 0;
                        var TotalMarBud = 0;
                        var TotalAprBud = 0;
                        var TotalMayBud = 0;
                        var TotalJunBud = 0;
                        var TotalJulBud = 0;
                        var TotalAugBud = 0;
                        var TotalSepBud = 0;
                        var TotalOctBud = 0;
                        var TotalNovBud = 0;
                        var TotalDecBud = 0;

                        for (var t = 0; t < responseData.Data.length; t++) {
                            TotalJulTY += responseData.Data[t].JulTY;
                            TotalAugTY += responseData.Data[t].AugTY;
                            TotalSepTY += responseData.Data[t].SepTY;
                            TotalOctTY += responseData.Data[t].OctTY;
                            TotalNovTY += responseData.Data[t].NovTY;
                            TotalDecTY += responseData.Data[t].DecTY;

                            TotalJanBud += responseData.Data[t].JanBud;
                            TotalFebBud += responseData.Data[t].FebBud;
                            TotalMarBud += responseData.Data[t].MarBud;
                            TotalAprBud += responseData.Data[t].AprBud;
                            TotalMayBud += responseData.Data[t].MayBud;
                            TotalJunBud += responseData.Data[t].JunBud;
                            TotalJulBud += responseData.Data[t].JulBud;
                            TotalAugBud += responseData.Data[t].AugBud;
                            TotalSepBud += responseData.Data[t].SepBud;
                            TotalOctBud += responseData.Data[t].OctBud;
                            TotalNovBud += responseData.Data[t].NovBud;
                            TotalDecBud += responseData.Data[t].DecBud;
                        };
                        tablestr += "<tr class='trTotal'>";
                        tablestr += "<td class='tdTotal' align=right nowrap colspan='" + textcols.toString() + "' align='right'>Total</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalJulTY.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalAugTY.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalSepTY.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalOctTY.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalNovTY.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalDecTY.toString()) + "</td>";

                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalJanBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalFebBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalMarBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalAprBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalMayBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalJunBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalJulBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalAugBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalSepBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalOctBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalNovBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalDecBud.toString()) + "</td>";
                        tablestr += "</tr>";
                    }
                    if (JsonObj.Report == "6") {
                        var textcols = parseInt(cols) - 19
                        var TotalProjectBudget = 0;
                        var TotalJulTY = 0;
                        var TotalAugTY = 0;
                        var TotalSepTY = 0;
                        var TotalOctTY = 0;
                        var TotalNovTY = 0;
                        var TotalDecTY = 0;

                        var TotalJanBud = 0;
                        var TotalFebBud = 0;
                        var TotalMarBud = 0;
                        var TotalAprBud = 0;
                        var TotalMayBud = 0;
                        var TotalJunBud = 0;
                        var TotalJulBud = 0;
                        var TotalAugBud = 0;
                        var TotalSepBud = 0;
                        var TotalOctBud = 0;
                        var TotalNovBud = 0;
                        var TotalDecBud = 0;

                        for (var t = 0; t < responseData.Data.length; t++) {
                            TotalProjectBudget += responseData.Data[t].ProjectBudget;
                            TotalJulTY += responseData.Data[t].YTDJul;
                            TotalAugTY += responseData.Data[t].AugTY;
                            TotalSepTY += responseData.Data[t].SepTY;
                            TotalOctTY += responseData.Data[t].OctTY;
                            TotalNovTY += responseData.Data[t].NovTY;
                            TotalDecTY += responseData.Data[t].DecTY;

                            TotalJanBud += responseData.Data[t].JanBud;
                            TotalFebBud += responseData.Data[t].FebBud;
                            TotalMarBud += responseData.Data[t].MarBud;
                            TotalAprBud += responseData.Data[t].AprBud;
                            TotalMayBud += responseData.Data[t].MayBud;
                            TotalJunBud += responseData.Data[t].JunBud;
                            TotalJulBud += responseData.Data[t].JulBud;
                            TotalAugBud += responseData.Data[t].AugBud;
                            TotalSepBud += responseData.Data[t].SepBud;
                            TotalOctBud += responseData.Data[t].OctBud;
                            TotalNovBud += responseData.Data[t].NovBud;
                            TotalDecBud += responseData.Data[t].DecBud;
                        };
                        tablestr += "<tr class='trTotal'>";
                        tablestr += "<td class='tdTotal' align=right nowrap colspan='" + textcols.toString() + "' align='right'>Total</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalProjectBudget.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalJulTY.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalAugTY.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalSepTY.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalOctTY.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalNovTY.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalDecTY.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap><span id='AllTotalTY'></span></td>";

                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalJanBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalFebBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalMarBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalAprBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalMayBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalJunBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalJulBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalAugBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalSepBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalOctBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalNovBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(TotalDecBud.toString()) + "</td>";
                        tablestr += "<td class='tdTotal' align=right nowrap><span id='AllTotalBud'></span></td>";
                        tablestr += "</tr>";

                    }
                    tablestr += "<tr>";
                    var headertext = "";
                    for (var k = 0; k < cols; k++) {
                        headertext = keyarr[k];
                        headertext = setheadertext(JsonObj.Report, headertext);
                        if ((keyarr[k].lastIndexOf("TY") >= 0 || keyarr[k].lastIndexOf("YTD") >= 0) && (keyarr[k].lastIndexOf("QTY") < 0) && (keyarr[k].lastIndexOf("ThisYearYTD") < 0)) {
                            tablestr += "<th class='thReportDataTY' nowrap>" + headertext + "</th>";
                        }
                        else {
                            tablestr += "<th class='thReportData' nowrap>" + headertext + "</th>";
                        }
                        if (JsonObj.Report == "6" && k == (parseInt(cols) - 12 - 1)) {
                            tablestr += "<th class='thReportDataTY' nowrap>Total TY</th>";
                        }
                    }

                    if (JsonObj.Report == "1" || JsonObj.Report == "3" || JsonObj.Report == "6") {
                        tablestr += "<th class='thReportData' nowrap>Total Bud</th>";
                    }

                    tablestr += "</tr>";

                    var allrow_totalbud = 0;
                    var allrow_totalTY = 0;
        
                    for (var r = 0; r < responseData.Data.length; r++) {
                        tablestr += "<tr class='trReportData'>";
                        var row_totalbud = 0;
                        var row_totalTY = 0;
                        for (var c = 0; c < cols; c++) {
                            if ((JsonObj.Report == "4") && (c > 1))
                                tablestr += "<td class='tdReportData' align=right nowrap>" + FormatNumber(responseData.Data[r][keyarr[c]]) + "</td>";
                            else {
                                if (isNaN(responseData.Data[r][keyarr[c]]))
                                    tablestr += "<td class='tdReportData' nowrap>" + responseData.Data[r][keyarr[c]] + "</td>";
                                else {
                                    if ((JsonObj.Report == "2") && (c == 3))  // except column
                                        tablestr += "<td class='tdReportData' nowrap>" + responseData.Data[r][keyarr[c]] + "</td>";
                                    else
                                        tablestr += "<td class='tdReportData' align=right nowrap>" + FormatNumber(responseData.Data[r][keyarr[c]]) + "</td>";
                                }
                            }
                            if (JsonObj.Report == "1" && c > 5) {
                                row_totalbud += parseInt(responseData.Data[r][keyarr[c]]);
                            }
                            if (JsonObj.Report == "3" && c > (parseInt(cols) - 12 - 1)) {
                                row_totalbud += parseInt(responseData.Data[r][keyarr[c]]);
                            }
                            if (JsonObj.Report == "6" && c > (parseInt(cols) - 18 - 1) && c < (parseInt(cols) - 12 - 1)) {
                                row_totalTY += parseInt(responseData.Data[r][keyarr[c]]);
                            }
                            if (JsonObj.Report == "6" && c > (parseInt(cols) - 12 - 1)) {
                                row_totalbud += parseInt(responseData.Data[r][keyarr[c]]);
                            }
                            if (JsonObj.Report == "6" && c == (parseInt(cols) - 12 - 1)) {
                                tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(row_totalTY) + "</td>";
                            }
                        }
                        if (JsonObj.Report == "1" || JsonObj.Report == "3" || JsonObj.Report == "6") {
                            tablestr += "<td class='tdTotal' align=right nowrap>" + FormatNumber(row_totalbud) + "</td>";
                            allrow_totalbud += row_totalbud;
                        }
                        if (JsonObj.Report == "6") {
                            allrow_totalTY += row_totalTY;
                        }
                        tablestr += "</tr>";
                       
                    }

                    document.getElementById("ReportDataTable").innerHTML = tablestr;
                    if (JsonObj.Report == "1" || JsonObj.Report == "3" || JsonObj.Report == "6") {
                        document.getElementById("AllTotalBud").innerText = FormatNumber(allrow_totalbud);
                    }
                    if (JsonObj.Report == "6") {
                        document.getElementById("AllTotalTY").innerText = FormatNumber(allrow_totalTY);
                    }
                }
                document.getElementById("InputQuery").style.visibility = "hidden";
                document.getElementById("btnQuery").style.visibility = "hidden";
                document.getElementById("trTBH").style.visibility = "hidden";
                document.getElementById("ReportName").innerText = $('#dropdownMenuButton_BudgetReport').html() + ' Report';
                $('html,body').animate({ scrollTop: 0 }, 0);
            } else {
                alert(responseData.ErrorMessage);
            }
        });
    });

    ChangeDepartment();
    ChangeGroupBy();

});
