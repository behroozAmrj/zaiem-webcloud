var pan = null;
var ErrorPanel = function (left, height) {
    this.isVisible = false;
    this.panel = $("<div id=\"panel\"></div>");
    $('body').append(this.panel);

    this.panel.css("left", left + "px");
    //this.panel.css("height", height + "px");

    this.show = function () {

        this.panel.animate({ top: "250px" }, 900, "easeOutElastic");
        this.isVisible = true;
    };

    this.gone = function () {
        var height = "-" + (parseInt(this.panel.css("height")) + 10) + "px";
        this.panel.animate({ top: height }, 1900, "easeInBounce");

    }

    this.addErrorMsg = function (msg) {
        this.panel.append("<li>" + msg + "</li>");
    }

    this.clear = function () {
        this.panel.html("");
    }

    this.listErrorMessage = function (errorList) {
        if (errorList.length > 0) {
            for (var i = 0  ; i < errorList.length ; i++) {
                this.panel.append("<li>" + errorList[i] + "</li>");
            }
        }
        else {
            if (this.isVisible)
                this.gone();
        }
    }
};


function showMessge(list) {
    if ((list.length == 0) &&
        (pan == null))
        return;
    if (pan == null)
        pan = new ErrorPanel(250,
                           300);
    pan.clear();
    pan.isVisible ? pan.panel.animate({ top: "250px" }, 900, "easeOutElastic") : pan.show();
    pan.listErrorMessage(list);
}

