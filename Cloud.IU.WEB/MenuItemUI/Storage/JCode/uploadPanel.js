$(document).ready(function(){
	
	function closeHoverDiv()
	{
		var uploadPanel = $("#upload_div");
		
		uploadPanel.css("display",
						"none");
		var blackCurtain = $("#blackCurtain");
		blackCurtain.css("display",
						"none");
		var progressBar = $("#progressBar");
        progressBar.css("width", "0px");
		var upload_btn = $("#uploadFile_button");
		upload_btn.attr("disabled",
                            "disabled");
		var files = document.getElementById('upload_file');
        files.files[0] = null;
		var fileName = $("#fileName");
		var fileSize  = $("#fileSize");
		var fileType = $("#fileType");
		fileName.text("fileName: ");
		fileSize.text("fileSize: ");
		fileType.text("fileType: ");							
	}
	
	var webCloudServer = "/VMManagement/uploadStreamFileTozDrive";
	$("#upload_close_buttom").click(function(){
		closeHoverDiv();
	});
	$("#upload_file").on("change" , function(){
	
		var files = document.getElementById('upload_file');
        var file = files.files[0];
		var upload_btn = $("#uploadFile_button");
		if(file != null)
		{
			
			upload_btn.removeAttr("disabled");
			var fileName = $("#fileName");
			var fileSize  = $("#fileSize");
			var fileType = $("#fileType");
			fileName.text("fileName: " +file.name);
			fileSize.text("fileSize: "+ file.size);
			fileType.text("fileType: " + file.type);
			console.log(file.name)
								
		}
		else
		{
			upload_btn.attr("disabled",
                            "disabled");
			var fileName = $("#fileName");
			var fileName = $("#fileName");
			var fileSize  = $("#fileSize");
			var fileType = $("#fileType");
			fileName.text("");
			fileSize.text("");
			fileType.text("");
			$("#progressPercent").text("0%")
		}
		
	});
	
	
	$("#uploadFile_button").click(function(){
		
		var files = document.getElementById('upload_file');
        var file = files.files[0];
		
		if (file == null)
		return;
		var fileName = file.name;
		var loaded = 0;
        var fileSize = file.size;
        var step = 1024 * 500;
        var start = 0;
        var blob = null;
        var failur = false;
		var progressBar = $("#progressBar");
        progressBar.css("width", "0px");
        var reader = new FileReader();
		var url =  $("#currentAddress").val();
		reader.onload = function () {
					var max = 450;//progressBar.attr("max-width");
                    var progress = (loaded / fileSize) * max;
                    progressBar.css("width",
                                      Math.round(progress) + "px");
					var progressPercent = $("#progressPercent");
					progressPercent.text(Math.round((loaded / fileSize) * 100)+"%");
                    var startByte = loaded;
                    var endByte = loaded + blob.size ;
                    loaded += step;
                    try {

                        if (failur == true)
                            return;
                        // loaded += step;
                        if (loaded < fileSize) {

                            var xmlRequest = new XMLHttpRequest();
                            xmlRequest.onreadystatechange = function (data) {
                                if ((xmlRequest.status == 500) && (xmlRequest.readyState == 4))
                                {
                                    failur = true;
                                    alert('uploading failed by server error. please try later');
                                    return;
                                }
                            }
                            var Name = reader.result;
                            var status = "";
                            if (startByte == 0)
                                status = "start";
                            else
                                status = "progress";
                            var header = webCloudServer+"?fileName=" + fileName + "&fileSize=" + fileSize + "&status=" + status + "&startByte=" + startByte + "&endByte=" + (endByte ) + "&URL=" +url;
                                
                            xmlRequest.open("POST", header, false);
                            xmlRequest.setRequestHeader("Content-Type", "application/json;");

                            xmlRequest.setRequestHeader("Content-Length", name.length);
                            var Name = reader.result;
                            var unB = Name;
                            Name = window.btoa(Name);

                            xmlRequest.sendAsBinary(JSON.stringify({ fileContent: Name , unBaseContent : unB }));
                            blob = file.slice(loaded, loaded + step);
                            var progressNum = loaded + step;
                            
                            reader.readAsBinaryString(blob); 
                        }
                        else {
                           
                                
                                var xmlRequest = new XMLHttpRequest();
                                xmlRequest.onreadystatechange = function (data) {
                                    if ((xmlRequest.status == 500) && (xmlRequest.readyState == 4)) {
                                        failur = true;
                                        alert('uploading failed by server error. please try later');
                                        return;
                                    }
                                }
                                var Name = reader.result;
                                var status = "";
                                if (fileSize <= step)
                                    status = "start";
                                else
                                    status = "progress";
                                var header = webCloudServer + "?fileName=" + fileName + "&fileSize=" + fileSize + "&status=" + status + "&startByte=" + startByte + "&endByte=" + (endByte) + "&URL=" + url;

                                xmlRequest.open("POST", header, false);

                                xmlRequest.setRequestHeader("Content-Type", "application/json;");

                                xmlRequest.setRequestHeader("Content-Length", name.length);
                                var Name = reader.result;
                                var unB = Name;
                                Name = window.btoa(Name);
                                xmlRequest.sendAsBinary(JSON.stringify({ fileContent: Name, unBase64Content: unB }));

                                loaded = fileSize;
                                var max = 450;//progressBar.attr("max-width");
                                var progress = (loaded / fileSize) * max;
                                progressBar.css("width",
                                          Math.round(progress) + "px");

                                progressPercent.text(Math.round((loaded / fileSize) * 100) + "%");
                                closeHoverDiv();
                                console.log('last package has been sent => ok');
                                requestNewPage(url);
                        }
                    }
                    catch (e)
                    {
                        console.log(e.message);
                    }
                   
                };
		
		blob = file.slice(start, start + step);
        reader.readAsBinaryString(blob);
	});
	
});