$(document).ready(function () {

    if (sessionStorage.getItem("Account") == 'voteadmin') {
        $('#vote_itemlist').css("display", "none");
        getVoteResultList();
    }
    else {
        getItemCatalog("Vote");
    }
    $('#Submit').bind('click', function () {
        submitData();
    });
    $('#SubmitHead').bind('click', function () {
        submitData();
    });
    //change your password
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
});


$('#item .dropdown-item').click(function (event) {
    event.preventDefault();
    $('#dropdownMenuButton_item').html(this.innerHTML);
});
//voteadmin use 
async function getVoteResultList() {
    var columnDefs = [
        {
            title: "Id",
            type: "readonly"
        }, {
            title: "ClassName",
            type: "readonly"
        }, {
            title: "Name",
            type: "text"
        }, {
            title: "Provider",
            type: "readonly"
        }, {
            title: "VoteCount",
            type: "readonly"
        }, {
            title: "Voter",
            type: "readonly"
        }
    ];
    var ajaxRequest = $.ajax({
        type: "GET",
        url: apiServer + "VoteResult",
        headers: {
            Authorization: 'Bearer ' + sessionStorage.getItem("Token")
        },
    });
    ajaxRequest.done(function (responseData) {
        if (responseData.Status == "OK") {
            var _DataSet = initDataSet_VoteResult(responseData);
            //freeze header column 
            $('#table_id').DataTable({
                "order": [[4, "desc"]],
                fixedHeader: true,
                select: true,
                pageLength: 40,
                "sPaginationType": "full_numbers",
                dom: 'Bfrtip',
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
                    },
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
                onEditRow: function (datatable, rowdata, success, error) {
                    putItem_Catalog(rowdata);
                },
                // "rowCallback": function (row, data, index) {
                //    if (data[6] == true) {
                //         $('td', row).css('background-color', 'Yellow');
                //     }
                // },
            });
        } else {
            alert(responseData.ErrorMessage);
        }
    });

}
function putItem_Catalog(rowdata) {
    var ajaxRequest = $.ajax({
        type: "PUT",
        url: apiServer + "VoteItemCatalog",
        data: {
            Id: rowdata[0], Name: rowdata[1]
        },
        headers: {
            Authorization: 'Bearer ' + sessionStorage.getItem("Token")
        },
    });
    ajaxRequest.done(function (responseData) {
        if (responseData.Status == "OK") {
            alert("OK");
            location.reload();
        } else {
            alert(responseData.ErrorMessage);
        }
    });

}
function initDataSet_VoteResult(responseData) {
    let _DataSet = [];
    for (i = 0; i < responseData.Data.length; i++) {
        let _Data = [];
        _Data.push(responseData.Data[i].Id);
        _Data.push(responseData.Data[i].ClassName);
        _Data.push(responseData.Data[i].Name);
        _Data.push(responseData.Data[i].Provider);
        _Data.push(responseData.Data[i].VoteCount);
        _Data.push(responseData.Data[i].Voter);
        _Data.push(responseData.Data[i].Important);
        _DataSet.push(_Data);
    }
    return _DataSet;
}
function postVoteMapping(className, id) {
    var ajaxRequest = $.ajax({
        type: "Post",
        url: apiServer + "VoteMapping",
        dataType: 'json',
        data: { Id: sessionStorage.getItem("LoginId"), ClassName: className, FK_Vote_ItemCatalogIdList: id },
        headers: {
            Authorization: 'Bearer ' + sessionStorage.getItem("Token")
        },
    });
    ajaxRequest.done(function (responseData) {
        if (responseData.Status == "OK") {
            var _ClassName = $('#dropdownMenuButton_item').text().trim();
            alert("OK");
            window.location.reload();
        } else {
            alert(responseData.ErrorMessage);
        }
    });
}
//if you has voted then the submit button unenable
var getIsHasVote = function (className) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "GET",
            url: apiServer + "VoteMapping",
            dataType: 'json',
            data: { Id: sessionStorage.getItem("LoginId"), className: className },
            headers: {
                Authorization: 'Bearer ' + sessionStorage.getItem("Token")
            },
            success: function (responseData) {
                if (responseData.Status == "OK") {
                    if (responseData.Data) {
                        $('#Submit').attr('disabled', true);
                        $('#SubmitHead').attr('disabled', true);
                    }
                    else {
                        $('#Submit').attr('disabled', false);
                        $('#SubmitHead').attr('disabled', false);
                    }
                }
                resolve()
            },
            error: function (error) {
                reject(error)
            },
        })
    })
}
function getVote_ItemCatalogId(className) {
    var ajaxRequest = $.ajax({
        type: "GET",
        url: apiServer + "VoteMapping",
        dataType: 'json',
        data: { Id: sessionStorage.getItem("LoginId"), className: className },
        headers: {
            Authorization: 'Bearer ' + sessionStorage.getItem("Token")
        },
    });

    ajaxRequest.done(function (responseData) {
        if (responseData.Status == "OK") {
            $.each(responseData.Data, function (index, val) {
                $('input[type="checkbox"]').each(function () {
                    if (this.value == val) {
                        $(this).prop("checked", true);
                        $('#Submit').attr('disabled', true);
                        $('#SubmitHead').attr('disabled', true);
                    }
                });
            });
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
//general user use
var getVoteResult = function (className) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "GET",
            url: apiServer + "VoteResult",
            dataType: 'json',
            data: { className: className },
            headers: {
                Authorization: 'Bearer ' + sessionStorage.getItem("Token")
            },
            success: function (responseData) {
                if (responseData.Status == "OK") {
                    dynamicVoteItem(responseData);
                }
                resolve()
            },
            error: function (error) {
                reject(error)
            },
        })
    })
}
//show vote item list
var dynamicVoteItem = function (responseData) {
    $('#voteitem').empty();
    var _LinkCount = 999;
    var _Block = Math.round(responseData.Data.length / _LinkCount);
    if (responseData.Data.length <= 999) _Block = 1;
    for (j = 0; j < _Block; j++) {
        var _Row = "<div class='col-xs-12 col-sm-12'> <ul id=item" + j + " >   </ul>  </div>  ";
        $('#voteitem').append(_Row);
    }
    for (i = 0; i < responseData.Data.length; i++) {
        var _Index = Math.floor(i / _LinkCount);
        var _Color = " ";
        // high vote show yellow backcolor
        // if (responseData.Data[i].Important) _Color = "style='background-color: yellow'";
        var _Link;
        _Link = "  <li>  <input type='text' id='text" + i + "' value='" + responseData.Data[i].VoteCount + "' class='votecount' readonly='readonly'> " +
            " <label for='checkbox" + i + "' " + _Color + " > " +
            responseData.Data[i].Name + " </label>" +
            "   <input type='checkbox' name='checkbox" + i + "' id='checkbox" + i + "' class ='selector' value='" + responseData.Data[i].Id + "'</li> ";
        $('#item' + _Index).append(_Link);
    }
    checkboxRadioBind();
}
function checkboxRadioBind() {
    $('input[type="checkbox"]').checkboxradio();
    //just can choose two item
    $('input[type="checkbox"]').bind('click', function () {
        let _Check = 0;
        $('input[type="checkbox"]').each(function () {
            if (this.checked) _Check++;
        });
        if (_Check > 1) $(this).prop("checked", false);
    });
}
var checkData = function () {
    ok = false;
    $('input[type="checkbox"]').each(function () {
        if (this.checked) ok = true;
    });
    return ok;
}
var submitData = function () {
    var _Id = [];
    var _ClassName = $('#dropdownMenuButton_vote').text().trim()
    $('input[type="checkbox"]').each(function () {
        if (this.checked) _Id.push(this.value);
    });
    if (_Id.length > 0) postVoteMapping(_ClassName, _Id);
    else alert("please choose what you want to vote ");
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
        if (responseData.Status == "OK") {
            initialDropdown(responseData); 
        } else {
            alert(responseData.ErrorMessage);
        }
    });

}
var initialDropdown = function (responseData) {
    $('#voteName').empty();
    for (i = 0; i < responseData.Data.length; i++) {
        var _Row = "    <a   class='#vote dropdown-item' href='#'>" + responseData.Data[i].Name + "</a>";
        $('#voteName').append(_Row);
        if (i == 0) {
            $('#dropdownMenuButton_vote').html(responseData.Data[i].Name);
            init(responseData.Data[i].Name);
        }
    }
    $('#vote .dropdown-item').click(function (event) {
        event.preventDefault();
        $('#dropdownMenuButton_vote').html(this.innerHTML);
        $('#voteName').val($(this).next().val());
        var _ClassName = $('#dropdownMenuButton_vote').text().trim();
        init(_ClassName);
    });
}
async function init(className) {
    await getVoteResult(className);
    await getIsHasVote(className);
}

