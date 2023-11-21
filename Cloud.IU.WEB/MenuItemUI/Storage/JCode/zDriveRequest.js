$(document).ready(function () {

    $("#home").click(function () {
        var baseAddress = $("#baseAddress").val();
        requestNewPage(baseAddress);
    });

});

function initialStorageData() {
    var request;
    if (window.XMLHttpRequest)
        request = new XMLHttpRequest();// Components.classes["@mozilla.org/xmlextras/xmlhttprequest;1"].createInstance(Components.interfaces.nsIXMLHttpRequest);
    else
        request = new ActiveXObject("Microsoft.XMLHTTP");

    request.onload = function (data) {
        console.log(data);
        var text = data.target.responseText
        if (text != "")
        {
			try	
			{
	            var url = JSON.parse(text);
	            if (url != "false") {
	                $("#currentAddress").val(url);
	                $("#baseAddress").val(url);
	            }
	            else
	                console.log('url is false');
			}
			catch(e)
			{
				console.log("some error ocurred during parsing response.try again");
			}
				
        }
    }
    request.onreadystatechange = function(data){
        if ((request.status == 500) && (request.readyState == 4)) {
            var msg = data.target.responseText;
            alert(msg);
            return;
        }

    }
    request.open("POST",
                "/VMManagement/InitialStorage",
                false);

    request.setRequestHeader("Content-Type", "application/json;");//
    request.send(null);

}
var webCloudServer = "/VMManagement/Storage";
window.addEventListener("load", function () {
    console.log('load');
    console.log('url:' + url);
    initialStorageData();
    var url = $("#baseAddress").val();

    requestNewPage(url);
});

function requestNewPage(URL) {
    try {
        var blackCurtain = $("#blackCurtain");
        blackCurtain.css("display",
						"block");
        var loadProgress = $("#loadProgress");
        loadProgress.css("display",
						"block");

        URL = URL.replace(/ /gi,
						  "%20");
        console.log("send Request: " + URL);
        var url = URL;
        try {
            if (!url.endsWith("/"))
                url = url + "/";
        }
        catch (e) {

            if (!url.match("/" + "$") == suffix)
                url = url + "/";
        }

        var request;
        if (window.XMLHttpRequest)
            request = new XMLHttpRequest();// Components.classes["@mozilla.org/xmlextras/xmlhttprequest;1"].createInstance(Components.interfaces.nsIXMLHttpRequest);
        else
            request = new ActiveXObject("Microsoft.XMLHTTP");
        
		request.onreadystatechange = function(data){
			if ((request.status == 500))
			{
				$("#wrapper").html("");
				var blackCurtain = $("#blackCurtain");
				blackCurtain.css("display",
							"none");
				var loadProgress = $("#loadProgress");
				loadProgress.css("display",
									"none");
			}
			
		};
		request.onload = function (data) {
            var blackCurtain = $("#blackCurtain");
            blackCurtain.css("display",
						"none");
            var loadProgress = $("#loadProgress");
            loadProgress.css("display",
						"none");

            console.log('succeed!');
            var textPost = data.target.responseText;
            if ((textPost != "timeout") && (textPost != "null")) {
                console.log(textPost);
                try {
                    var childrens = JSON.parse(data.target.responseText);

                    url = url.replace("%20",
		            				   " ");
                    console.log(url);
                    var row = generateElement(url,
		            						  childrens);
                    // remember to calculate list length to generate row for wrapper            						  
                    $("#wrapper").html("");
                    $("#wrapper").append(row);
                    changeCurrentAddress(url);
                    var baseAddressURL = $("#baseAddress").val();
                    console.log("nvaBar Builder Started...!" + baseAddressURL);

                    navBarGenerator(baseAddressURL,
	           						url);
                }
                catch (e)
                { console.log("error: " + e.message); }
            }
            else {
                var blackCurtain = $("#blackCurtain");
                blackCurtain.css("display",
                                "block");
                var timeoutPanle = $("#timeoutPanle");
                timeoutPanle.css("display",
                                    "block");
            }
        };
        request.onerror = function (errorData) {
            console.log('failed!' + errorData.target.status);
            $("#wrapper").html("");
            var blackCurtain = $("#blackCurtain");
            blackCurtain.css("display",
						"none");
            var loadProgress = $("#loadProgress");
            loadProgress.css("display",
						"none");
        };
        request.open("POST",
                       webCloudServer,
                       true);

        request.setRequestHeader("Content-Type", "application/json;");//
        request.send(JSON.stringify({ operation: "loadContent", URL: url}));

    }
    catch (e) {
        console.log(e.message);
    }
}
function changeCurrentAddress(URL) {
    $("#currentAddress").val(URL);
    $("#cloudURL_text").val(URL);
}


function createNewFolder(location, newFolderName) {

    if ((location != "") &&
			(newFolderName != "")) {
        try {
            if (!location.endsWith("/"))
                location = location + "/";
            if (!newFolderName.endsWith("/"))
                newFolderName = newFolderName + "/";
            var URL = location + newFolderName;
            var url = URL.replace(/ /gi,
								  "%20");
            var request = new XMLHttpRequest();// Components.classes["@mozilla.org/xmlextras/xmlhttprequest;1"].createInstance(Components.interfaces.nsIXMLHttpRequest);
            request.onload = function (data) {

                console.log("new folder created: " + newFolderName);
                var currentAddress = $("#currentAddress").val();
                requestNewPage(currentAddress);

            }

            request.onreadystatechange = function (data) {
                if ((request.status == 500) && (request.readyState == 4))
                {
                    alert('creating folder failed .please try again!');
                }
            }
            request.onerror = function (errorData) {
                console.log('failed!' + errorData.target.status);

                $("#wrapper").html("");
            };
            request.open("POST",
	                      webCloudServer,
	                       true);

            request.setRequestHeader("Content-Type",
                                	 "application/json;");

            request.send(JSON.stringify({ operation: "createContainer", URL: url, content: 'hello' }));
        }//try	 end tag
        catch (e)
        { console.log(e.message); }
    }
    else { console.log("one or more parameters are null"); }
}


function delete_folder(URL) {
    try {
        if (URL == "") {
            console.log("wrong URL");
            return (false);
        }
        var url = URL.replace(/ /gi,
							   "%20");

        var request = new XMLHttpRequest();
        request.load = function () {
            reload();
        };
        request.onreadystatechange = function (data) {
            if ((request.status == 500) && (request.readyState == 4)) {
                alert('deleting folder failed .please try again!');
            }
        }
        request.open("POST",
                       webCloudServer,
                       true);

        request.setRequestHeader("Content-Type", "application/json;");//

        request.send(JSON.stringify({ operation: "deleteContainer", URL: url, content: 'null' }));

    }
    catch (e)
    { console.log(e.message); }
}

function delete_file(URL) {
    try {
        if (URL == "") {
            console.log("wrong URL");
            return (false);
        }
        var url = URL.replace(/ /gi,
							   "%20");

        var request = new XMLHttpRequest();
        request.load = function () {
            reload();
        };

        request.onreadystatechange = function (data) {
            if ((xmlRequest.status == 500) && (xmlRequest.readyState == 4)) {
                alert('creating file failed .please try again!');
            }
        }

        request.open("POST",
                       webCloudServer,
                       true);

        request.setRequestHeader("Content-Type", "application/json;");//

        request.send(JSON.stringify({ operation: "deleteFile", URL: url, content: 'null' }));

    }
    catch (e)
    { console.log(e.message); }
}


function reload() {
    try {
        var URL = $("#currentAddress").val();
        var blackCurtain = $("#blackCurtain");
        blackCurtain.css("display",
						"block");
        var loadProgress = $("#loadProgress");
        loadProgress.css("display",
						"block");

        URL = URL.replace(/ /gi,
						  "%20");
        console.log("send Request: " + URL);
        var url = URL;

        if (!url.endsWith("/"))
            url = url + "/";
        var request = new XMLHttpRequest();// Components.classes["@mozilla.org/xmlextras/xmlhttprequest;1"].createInstance(Components.interfaces.nsIXMLHttpRequest);
        request.onreadystatechange = function (data) {
            if ((xmlRequest.status == 500) && (xmlRequest.readyState == 4)) {
                console.log('reloading failed. please try again!');
            }
        }
        request.onload = function (data) {

            var blackCurtain = $("#blackCurtain");
            blackCurtain.css("display",
						"none");
            var loadProgress = $("#loadProgress");
            loadProgress.css("display",
						"none");

 
            var text = data.target.responseText;
            console.log(text);
            var childrens = JSON.parse(text);

            url = url.replace("%20",
            				   " ");
            console.log(url);
            var row = generateElement(url,
            						  childrens);
            // remember to calculate list length to generate row for wrapper            						  
            $("#wrapper").html("");
            $("#wrapper").append(row);
            changeCurrentAddress(url);
            var baseAddressURL = $("#baseAddress").val();
            console.log("nvaBar Builder Started...!" + baseAddressURL);
            try {
                navBarGenerator(baseAddressURL,
           						url);
            }
            catch (e)
            { console.log("error: " + e.message); }


        };
        request.onerror = function (errorData) {
            console.log('failed!' + errorData.target.status);

            $("#wrapper").html("");
            var blackCurtain = $("#blackCurtain");
            blackCurtain.css("display",
						"none");
            var loadProgress = $("#loadProgress");
            loadProgress.css("display",
						"none");
        };
        request.open("POST",
                       webCloudServer,
                       true);

        request.setRequestHeader("Content-Type", "application/json;");//


        request.send(JSON.stringify({ operation: "loadContent", URL: url, content: 'hello' }));

    }
    catch (e) {
        console.log(e.message);
    }
}


