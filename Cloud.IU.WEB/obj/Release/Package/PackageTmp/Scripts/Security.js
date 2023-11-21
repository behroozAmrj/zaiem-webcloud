$(document).ready(function () {
    var isValid = true;
    var username = "";
    var password = "";
    var passconfirm = true;
    var email = "";
    $(".submitt_btn_ok").click(function () {
        var errorListMessage = [];
        

        var isValid = true;
        $("#errorMsg").html("");
        $(".inputValue").each(function (index) {
            isValid = isValid && (($(this).val() != "") && ($(this).val() != null));

            ($(this).val() == "" || $(this).val() == null) ? ($(this).parent().addClass("inputValidation"), errorListMessage.push( $(this).attr("title"))) : $(this).parent().removeClass("inputValidation");
            
        });

        ($("#confirmPassword").val() == $("#passWord").val()) ? isValid = isValid && ($("#confirmPassword").val() == $("#passWord").val()) : errorListMessage.push("password is not match");

        isEmailValid($("#email").val()) ? isValid = isValid && isEmailValid($("#email").val()) : errorListMessage.push("email is not is correct format");

        showMessge(errorListMessage);
        if (isValid)
        {
            username = $("#userName").val();
            password = $("#passWord").val();
            email = $("#email").val();
            var xRequest = new XMLHttpRequest();
            xRequest.open("POST",
                           "/Security/RegisterUserInCloud",
                           true);
            xRequest.onreadystatechange = function (data) {
                if ((xRequest.readyState != 4) || (xRequest.status != 200)) {
                    $("#signup_wrapper").html("sorry your request failed! try again. <a href=\"/Security/SignUp_load\">register again</a>");
                }
                else
                {
                    var htmlResult = JSON.parse(data.target.responseText);
                    $("#signup_wrapper").html(htmlResult);
                }
            }
            xRequest.ontimeout = function () {
                $("#signup_wrapper").html("sorry connecting to server timedout! try again <a href=\"Security/SignUp_load\">register again</a>");
            }
            xRequest.setRequestHeader("Content-Type", "application/json;");
            xRequest.send(JSON.stringify({
                userName: username,
                passWord: password,
                email: email
            }));
        }

    });

    $(".inputValue").on("keyup paste change focusout", function () {

        var value = $(this).val();
        if ((value == "") || (value == null))
            $(this).parent().addClass("inputValidation");
        else
            $(this).parent().removeClass("inputValidation");
    });

    $("#confirmPassword").on("focusout", function () {
        if ($(this).val() != $("#passWord").val()) {
            $(this).parent().addClass("inputValidation");
            
        }
        else
            $(this).removeClass("inputValidation");
    });



    //this private for check javascript
    function isEmailValid(emailAdress) {
        var EMAIL_REGEXP = new RegExp('^[a-z0-9]+(\.[_a-z0-9]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,15})$', 'i');
        return EMAIL_REGEXP.test(emailAdress)
    }

});

