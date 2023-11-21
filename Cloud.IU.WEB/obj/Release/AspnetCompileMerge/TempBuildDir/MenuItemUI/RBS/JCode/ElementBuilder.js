


function generateElement(URL , contentList)
{
	var row  = addRow();
    $.each(contentList , function(key, value){
		var item  = null;	
		try
		{
	        if (value.endsWith("/"))
	        {
	           	var text = value.replace("/","");
	           	var folder = addFolder(URL , 
	          							text);
	           	folder.click(function (){
					var url= 	$(this).find("input[type='hidden']").val();	
					requestNewPage(url);
	           	});
	            row.append(folder);
	        }
	        else
	        {
	            var text = value.replace("/","");
	           	var file = addFile(URL , 
									text);
	          	file.click(function(){
	                var url = $(this).find("input[type='hidden']").val();	
					downloadFile(url);
					window.open("/VMManagement/downloadFile?url="+url,
								"_blank");
	           	});
	            row.append(file);
	        }
		}
		catch(e)
		{
			if (strEndsWith(value , "/"))
			{
	           	var text = value.replace("/","");
	           	var folder = addFolder(URL , 
	          							text);
	           	folder.click(function (){
					var url= 	$(this).find("input[type='hidden']").val();	
					requestNewPage(url);
	           	});
	            row.append(folder);
	        }
	        else
	        {
	            var text = value.replace("/","");
	           	var file = addFile(URL , 
									text);
	          	file.click(function(){
	                var url = $(this).find("input[type='hidden']").val();	
					downloadFile(url);
					window.open("/VMManagement/downloadFile?url="+url,
								"_blank");
	           	});
	            row.append(file);
	        }
		}
    });
            
    return(row);
}

function strEndsWith(str, suffix) {
    return str.match(suffix+"$")==suffix;
}
function addRow()
{
 var row = $("<div class=\"row\"></div>");
            return(row);
}

function  addFolder(URL , name)
{
	var text = name;
	if (text.length > 10)
	text = text.substring(0,
						  7) + "...";
	
	 var folder = $("<div class=\"item folder\"></div>");
   	 var span = addSpan(text);
   	 if (!URL.endsWith("/"))
   	 	 URL = URL + "/";
   	 var absoluteURL = URL + name;
   	 var type = addType(absoluteURL);
   	    	 
   	 folder.append(span);
   	 folder.append(type);
   	 folder.attr("title",
 	 				name);
	 folder.append(addCheckBox()); 	 				
     return(folder);

}
function addFile(URL , name)
{
	var text = name;
	if (text.length > 10)
	text = text.substring(0,
						  7) + "...";

		var nameArr = name.split('.');
		var className = null;
		switch(nameArr[nameArr.length - 1])
		{
			case "pdf" :
			{
				className  = "pdf";
				break;
			}
			case "png" : case  "gif" : case "jpg" :  case "jpeg" :
			{
				className = "img";
				break;
			}
			case "avi" : case "mp4" : case "mp4" : case "bat" : case "wmv" : 
			{
				className = "movie";
				break;
			}
		}
		 var file = $("<div class=\"item file "+className+"\"></div>");
    	 var span = addSpan(text);
		  if (!URL.endsWith("/"))
				URL = URL + "/";
    	 var absoluteURL = URL + name;
		 var type = addType(absoluteURL);
    	 
    	 file.append(span);
		 file.append(type);
	     file.attr("title",
 	 				name);
		 file.append(addCheckBox());
		 //var link = $("<a href=\"/VMManagement/downloadFile?url="+absoluteURL+"\" target=\"_blank\"></a>");
		 //link.append(file);
         return(file);

}
function addSpan(name)
{
	var span = $("<span>"+name+"</span>");
    	return (span);

}

function addType(type)
{
	var hType = $("<input type='hidden' />");
	hType.attr("value",
				type);
	return (hType);
}

function addCheckBox()
{
	var checkBox = $("<input type=\"checkbox\" />");
	checkBox.click(function(event){
		event.stopPropagation();
		
	});
	return(checkBox);
}



	