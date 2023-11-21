$(document).ready(function(){
	
	var webCloudServer = "/VMManagement/RBSuploadAndAddFile";	
	/*
	$("#uploadFile_button").click(function(){
	
		fileupload = document.getElementById('upload_file');
		var currentAddress = $("#currentAddress").val();
		uploadFileToServer(currentAddress,
							fileupload.files[0]);
	
	});
	*/
	$("#upload_file").on("change" , function(){
		$("#progressParent").text("");
		$("#excepMsg").text("");

		var files = document.getElementById('upload_file');
        var file = files.files[0];
		var upload_btn = $("#uploadFile_button");
		$("#fileName").text(file.name);
		$("#fileSize").text(file.size);
		
		if(file != null)
		{
			if (file.type == "application/zip")
			{
			    console.log(file.type);
				upload_btn.removeAttr("disabled");    
			}
		
			else
			{
				$("#excepMsg").text("file type is incorrect format. you must select Zip file Type");
				upload_btn.attr("disabled","disabled");
			}
		}
		else
			upload_btn.attr("disabled","disabled");

		
	});
	
	$("#appName").on("change keyup paste",function(){	
	
		if ($(this).val() != "")
		{
			$("#excepMsg").text("");

		}
		else
		{
			$("#excepMsg").text("application Name is empty");
		}

	});
	
	$("#uploadFile_button").click(function(){
	
	var appName = $("#appName").val();
	if (appName != "")
	{
				
			var files = document.getElementById('upload_file');
	        var file = files.files[0];
			var progressBar = $("#progressBar");
			progressBar.css("width",
								"0px");
			if (file == null)
			return;
			var fileName = file.name;
			var loaded = 0;
	        var fileSize = file.size;
	        var step = 1024* 100;
	        var start = 0;
			
			
	        var reader = new FileReader();
			var url =  $("#currentAddress").val();
			reader.onload = function () {
						var max = 400;//progressBar.attr("max-width");
	                    var progress = (loaded / fileSize) * max;
	                    progressBar.css("width",
	                                      Math.round(progress) + "px");
						
	                    var startByte = loaded;
						var startByte = loaded;
	                    loaded += step;
	                    try {
	                        // loaded += step;
	                        if (loaded < fileSize) {
	
	                            var xmlRequest = new XMLHttpRequest();
								
	                            var Name = reader.result;
	                            var status = "";
	                            status = "load";
	                            var header = webCloudServer+"?fileName=" + fileName +"&appName="+ appName +  "&status=" + status;
	                                
	                            xmlRequest.open("POST", header, false);
	                            xmlRequest.setRequestHeader("Content-Type", "application/json;");
	
	                            xmlRequest.setRequestHeader("Content-Length", name.length);
	                            var Name = reader.result;
	                            var unB = Name;
	                            Name = window.btoa(Name);
	
	                            xmlRequest.sendAsBinary(JSON.stringify({ fileContent: Name}));
	                            blob = file.slice(loaded, loaded + step);
	                            reader.readAsBinaryString(blob); 
	
	                        }
	                        else {
	                            var xmlRequest = new XMLHttpRequest();
	                        
	                            var Name = reader.result;
	                            var status = "";
	                            status = "end";
	                            var header = webCloudServer+"?fileName=" + fileName +"&appName="+ appName + "&status=" + status;
	                                
	                            xmlRequest.open("POST", header, false);
	
	                            xmlRequest.setRequestHeader("Content-Type", "application/json;");
	
	                            xmlRequest.setRequestHeader("Content-Length", name.length);
	                            var Name = reader.result;
	                            var unB = Name;
	                            Name = window.btoa(Name);
	                            xmlRequest.sendAsBinary(JSON.stringify({ fileContent: Name }));
	
	                            loaded = fileSize;
	                            var max = 400;//progressBar.attr("max-width");
								var progress = (loaded / fileSize) * max;
								progressBar.css("width",
	                                      Math.round(progress) + "px");
	
	                        }
	                    }
	                    catch (e)
	                    {
	                        console.log(e.message);
	                    }
	                   
	                };
			
			var blob = file.slice(start, start + step);
	        reader.readAsBinaryString(blob);
		}
		else
		{
			$("#excepMsg").text("application Name is empty");
		}
	});
	
});