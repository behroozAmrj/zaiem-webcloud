/// <reference path="jquery-1.10.2.js" />

function div_only(item) {
    return ('<div></div>');
}

function colDivBuilder(id) {
    var div = '<div id="' + id + '" class="column"></div>';
    return (div);
}

function divContainerBuilder(top , left , itemID , type) {
    var div = $('<div class="container"  style="top:' + top + 'px; left:' + left + 'px ; position:absolute"  ></div>');
    
    div.append(divFunctionalityBuilder(itemID , 
                                        type));
    return (div);
}


function divFunctionalityBuilder(itemID , type)
{
    itemID = "'" + itemID + "'";
    var onclick = " onClick=deleteItems(" + itemID + "," + type + ") ";
    var img = $('<img src="/images/Delete-32.png" class="entityDel" '+ onclick +' />');
    
    return (img);
}

function iconContainerDivBuilder() {
    var div = '<div class="iconContainer"></div>';
    return (div);
}


function imgIConBuilder(name, hnd, type) {

    hnd = "'" + hnd + "'";
    var onclick = " ondblclick=DoWork(" + hnd + "," + type + ") ";
    var img = '<img class="imgIcon"  ' + onclick + '  src="' + name + '" />';
    return (img);
}

function titleContainerDivBuilder() {
    var div = '<div class="titleContainer"></div>';
    return (div);
}

function spanBuilder(titel) {
    var span = '<p>' + titel + '</p>';
    return (span);
}

function handlerBuilder(id, value) {
    var hvalue = '<input type="hidden" id="' + id + '" value="' + value + '" />';
    return (hvalue);
}

function divPopUpWindow(url) {
    var div = $('<div class="pwrapper"></div>');
    var container = $(divContainer(url));// this calls method to build content and place inside of div
    div.append($(formControl(div))); // this calls method to build form control such as close , open, max , min
    div.append(container);
    div.dblclick(function () {
        $(this).css("height",
                    "100%");
        $(this).css("width",
                    "100%");
        $(this).css("top",
                    "0");
        $(this).css("left",
                     "0");
        $(this).css("z-index",
                    "1000");
    });

    div.click(function () {
        //$(".pwrapper").css("z-index",
        //                    "-1");
        //$(this).css("z-index",
        //            "0 ");
    });
    div.draggable();
    div.resizable();


    return (div);

}


function formControl(parent) {
    var div = $('<div class="formControl"></div>');
    var close = $('<div></div>');
    var max = $('<div></div>');
    var min = $('<div></div>');
    close.click(function () {
        parent.remove();
    });
    max.click(function () {
        $(parent).css("height",
                    "100%");
        $(parent).css("width",
                        "100%");
        $(parent).css("top",
                         "0");
        $(parent).css("left",
                            "0");
        $(parent).css("z-index",
                        "1000");
    });
    div.append(close.text('*'));
    div.append(max.text('|=|'));
    div.append(min.text('---'));
    return (div);
}

function Close(parent) {
    
}

function Max(parent)
{
    
}


function min(min) {
    $(parent).css("", "");
}


function divContainer(url) {
    var iframe = $('<iframe src="' + url + '"></iframe>');
    var div = $('<div class="pcontainer"></div>');
    div.append(iframe);
    return (div);
}



///  new style will be build from this to end

function generateWindow(URL , templateWindow , layoutList , callerType)
{
    var pwindow = $(templateWindow);
    var select = pwindow.find(".formControl:eq(0)").find("div:eq(1)").find("select:eq(0)");
    var close = pwindow.find(".formControl:eq(0)").find("div#close:eq(0)");
    var iframe = pwindow.find(".pcontainer:eq(0)").find("iframe:eq(0)");
    pwindow.click(function () {
        $(".pwindow").css("z-index",
                        "1");
        $(this).css("z-index",
                        "10000");
       
       
    });

    pwindow.dblclick(function () {
        var ismax = $(this).find("input[type='hidden']#ismax");
        if (ismax.val() == 0) {
            $(this).css("height",
                        "100%");
            $(this).css("width",
                            "100%");
            $(this).css("top",
                             "0");
            $(this).css("left",
                                "0");
            $(this).css("z-index",
                            "1000");
            ismax.val(1);
        }
        else
        {
            $(this).css("height",
                       "500px" );
            $(this).css("width",
                           "700px" );
            $(this).css("top",
                             "62");
            $(this).css("left",
                                "310");
            $(this).css("z-index",
                            "1000");
            ismax.val(0);

        }

        
    });

    pwindow.mousedown(function () {
        $(".pwindow").css("z-index",
                        "1");
        $(this).css("z-index",
                        "10000");
    });
    if ((layoutList != null) &&
        layoutList.length > 0 ) {
        $.each(layoutList, function (index, value) {
            var option = '<option>' + value + '</option>';
            select.append(option);
        });


        select.change(function () {
            var data = $(this).val();
            $.ajax({
                url: "/VMManagement/ChangeSkin",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                datatype: "json",
                data: JSON.stringify({ Data: data, Type: callerType }),
                success: function (data) {
                    if ((data != null) &&
                        (data != "")) {
                        var url = "?url=" + iframe.attr("src");
                        var adr = "/" + data + url;
                        iframe.attr("src",
                                     adr);
                    }
                    else { }
                }
            });
        });
    }
    else { select.remove(); }
    iframe.attr("src",
                URL);
    $(".pwindow").css("z-index",
                        "1");
    $(pwindow).css("z-index",
                    "10000");

    $(pwindow).css("position",
                   "absolute");

    close.click(function () {pwindow.remove()});
    return (pwindow);
}