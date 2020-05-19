$(function () {
   var apiServer = ''; 
     var origin = window.location.origin;
     if (origin == "http://localhost:55614") { 
         apiServer = 'http://localhost:52006/api/'; 
     }
     else if (origin == "") {
         apiServer = '';
    }
     else if (origin == "") { 
         apiServer = ''; 
     }
     else if (origin == "") {
         apiServer = '';
     }

     $("#username").focus();
    if ((localStorage.getItem("Account") != null) && localStorage.getItem("Password") != null) {
        $('#username').val(localStorage.getItem("Account"));
        $('#password').val(localStorage.getItem("Password"));
        $('#chkRemember').prop("checked",true);
    }
    $(".form-control").keypress(function (e) {

        code = (e.keyCode ? e.keyCode : e.which);

        if (code == 13) {

            //targetForm是表單的ID

            $('#login').click();

        }

    });
    $('#login').click(function () {
        $('#login').attr("disabled", true);
        if ($('#chkRemember').prop("checked"))
            rememberMe();
        $.post(apiServer + 'Token', {
            Username: $('#username').val(),
          //  Origin: "Customer",
            Password: $('#password').val()
        }) 
            .done(function (data) {
                if (data.Status == "OK") {
                    $('#token').val(data.Data.token);
                    sessionStorage.setItem("Token", data.Data.token);
                    sessionStorage.setItem("LoginId", data.Data.loginId);
                    sessionStorage.setItem("Account", $('#username').val());
                    //maybe use spa pattern to resovle reflash page problem
                    // $(location).attr('href', './UploadFile.html');
                    $(location).attr('href', './' + data.Data.indexPage);
                  
                } else {
                    alert("account or password is wrong:" + data.ErrorMessage);
                    $('#login').attr("disabled", false);
                }
            })
            .fail(function (err) {
                alert("unexpected error");
                $('#login').attr("disabled", false);
            });
    });
    var rememberMe = function () {
        localStorage.setItem("Account", $('#username').val());
        localStorage.setItem("Password", $('#password').val());
    }
    $.backstretch([
		"../Image/loginbackground1.jpg", 
		"../Image/loginbackground2.jpg"
    ], {
        fade: 1000, // 動畫時長
        duration: 2000 // change-delay time
    });
})
