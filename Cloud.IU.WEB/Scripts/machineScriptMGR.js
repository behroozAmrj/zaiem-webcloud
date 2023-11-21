var loadProgress = $("#loadProgress");
var curtain = $("#curtain");
var InsertMachine_div = $("#InsertMachine_div");
var isValid = true;
$(document).ready(function () {
    var loadProgress = $("#loadProgress");
    var curtain = $("#curtain");
    var InsertMachine_div = $("#InsertMachine_div");
    $("#newMachine").click(function () {

        showCurtain(true);
        showInsertBox(true);

    });

    $("#machineInsert_insert_btn").click(function () {


        var machName = $("#machineName_txt").val();
        var imageId = $("#zImagelistddl").val();
        var flavorId = $("#zFlavorListDdl").val();
        var netWork = "";
        var isValid = true;
        var errorListMsg = [];
        $("#errorMsg").text("");
        $(".inputValue").each(function (index) {
            isValid = isValid && ($(this).val() != "" && $(this).val() != null);

            ($(this).val() == "" || $(this).val() == null) ? ($(this).addClass("inputValidation"), errorListMsg.push($(this).attr("title"))) : $(this).removeClass("inputValidation");

            //if ($(this).val() == "" || $(this).val() == null) {
            //    $(this).addClass("inputValidation");
            //    $("#errorMsg").append($(this).attr("title") + " <br/>");
            //    isValid = false;
            //}
            //else {
            //    $(this).removeClass("inputValidation");

            //}

        });

        ($("input[type='radio'][name='networkGrp']:checked").size() == 0) ? (isValid = false, errorListMsg.push("no network selected ")) : netWork = $("input[type='radio'][name='networkGrp']:checked").val();
        /*
        if ($("input[type='radio'][name='networkGrp']:checked").size() == 0) {
            isValid = false;
            $("#errorMsg").append("no network selected <br/>");
        }
        else
            netWork = $("input[type='radio'][name='networkGrp']:checked").val();
            */
        showMessge(errorListMsg);
        if (!isValid) {

            return;
        }
        
        var xmlRequest = new XMLHttpRequest();
        /*
        xmlRequest.onload = function (data) {
            var value = data.target.responseText;
            hideAllDialogs();
            $("#gridPanel").html(value);
        }
        */
        xmlRequest.onreadystatechange = function(data)
        {
            if ((xmlRequest.status == 200) && (xmlRequest.readyState == 4)) {
                var value = data.target.responseText;
                $("#gridPanel").html("");
                $("#gridPanel").html(value);
                hideAllDialogs();
            }
            else
                if ((xmlRequest.status == 500) && (xmlRequest.readyState == 4)) {
                    window.location = "/Security/NewLogin?erc=user loggedOut";
                }

        }

        xmlRequest.onerror = function (data) {
            hideAllDialogs();
            alert(data.target.responseText);
            console.log(data.target.responseText);
        }
        xmlRequest.open("POST",
                            "/MachineManagement/machineInsert",
                            true);
        xmlRequest.setRequestHeader("Content-Type", "application/json;");
        xmlRequest.send(JSON.stringify({
            machineName: machName,
            imageID: imageId,
            flavorID: flavorId,
            network: netWork
        }));
        showInsertBox(false);
        showLoadProgress(true);
    });

    $("#machineInsert_cancle_btn").click(function () {
        showMessge([]);
        hideAllDialogs();
    });


    function showCurtain(visibility) {
        if (visibility)
            curtain.removeClass('hide');
        else 
            if (!curtain.hasClass('hide'))
                curtain.addClass('hide');
    }

    function showInsertBox(visibility) {
        if (visibility)
            InsertMachine_div.removeClass('hide');
        else
            if (!InsertMachine_div.hasClass('hide'))
                InsertMachine_div.addClass('hide');
    }

    function showLoadProgress(visibility) {
        if (visibility)
            loadProgress.removeClass('hide');
        else
            if (!loadProgress.hasClass('hide'))
                loadProgress.addClass('hide');
    }


    function hideAllDialogs()
    {
        $(".dialog").each(function (index) {
            $(this).addClass('hide');
        });
    }

});

function showCurtain(visibility) {
    // curtain.removeClass('hide');
    var loadProgress = $("#loadProgress");
    var curtain = $("#curtain");
    if (visibility)
       
            curtain.removeClass('hide');
    else
        if (!curtain.hasClass('hide'))
                curtain.addClass('hide');
}

function showInsertBox(visibility) {
    if (visibility)
        InsertMachine_div.removeClass('hide');
    else
        if (InsertMachine_div.hasClass('hide'))
            InsertMachine_div.addClass('hide');
}

function showLoadProgress(visibility) {
    var loadProgress = $("#loadProgress");
    var curtain = $("#curtain");
    if (visibility) {

        loadProgress.removeClass('hide');
    }
    else {
        loadProgress.addClass('hide');
    }

}


function hideAllDialogs() {
    $(".dialog").each(function (index) {
        $(this).addClass('hide');
    });
    
}


function doOperation(machineID, opr) {

    if (!confirm("are sure to " + opr + " machine"))
        return;
    if (machineID == "" ||
        opr == "") {
        alert('no parameters is defined reload page and try again!');
        return;
    }
    

    var xmlRequest = new XMLHttpRequest();

    xmlRequest.onreadystatechange = function (data) {
        if ((xmlRequest.status == 200) && (xmlRequest.readyState == 4)) {
            var value = data.target.responseText;
            $("#gridPanel").html("");
            $("#gridPanel").html(value);
            hideAllDialogs();
        }
        else
            if ((xmlRequest.status == 500) && (xmlRequest.readyState == 4)) {
                window.location = "/Security/NewLogin?erc=user loggedOut";
            }
           
        
    }
    xmlRequest.onerror = function (data) {
        console.log(data.target.responseText);
    }
    showCurtain(true);
    showLoadProgress(true);
    xmlRequest.open("POST",
                        "/MachineManagement/machineOperation",
                        false);
    xmlRequest.setRequestHeader("Content-Type", "application/json;");
    xmlRequest.send(JSON.stringify({
        machineID: machineID,
        operation: opr
    }));
}