
function navBarGenerator(baseAddressURL , URL) {
	
	if ((baseAddressURL != "") && 	(URL != ""))
	{
		$("#navbox").html("");
   		 linkGenerator(baseAddressURL,
		    				URL);
		
    }
}

function linkGenerator(baseAddress , cloudURL)
{
	

	
	var navBar = $("#navbox");								
	var folderList = refineURL(baseAddress , 
								cloudURL);	
	if ((folderList.length > 0) && (folderList != null))										
	{
		var absoluteURLList = generateAbsoluteURL(baseAddress,
		   									   folderList);
		
		var listlen = folderList.length;
		if(cloudURL.endsWith("/"))
			listlen = listlen - 1;
		try	
		{
		for(var i = 0 ; i < listlen ; i++ )
		{
			var div = $("<div></div>");										   
			var link = $("<span></span>");
			var hidden = $("<input type=\"hidden\" />");
			var title = folderList[i].replace(/%20/gi,
											  " ");
			var ahref = absoluteURLList[i];
			console.log("href: " + ahref);
			link.text(title);
			hidden.attr("value",
							ahref);
			link.click(function(){
				var url = $(this).parent().find("input[type='hidden']:eq(0)").val();
				requestNewPage(url);

			
			});							
				
			
			
			div.append(link);
			div.append(hidden);
			
			navBar.append(div);
			
			
		}
		}
		catch(e)
		{console.log("Error: " + e.message);}
	}
}

function refineURL(baseAddress , cloudURL)
{

	// in this line we remove base address from absolute URL
	if (baseAddress != cloudURL)
	{
		var url = cloudURL.replace(baseAddress,
					     			"");
		if (url.endsWith("/"))			
			isEndwidthSlash = true;						
		var folderlist = url.split("/");
		
		return (folderlist);
	}
	else
	{
		var folderList = [];
		return (folderList);
	}
									
}

function generateAbsoluteURL(baseAddress , folderList)
{
	
	
	var absoluteList =[];
	if (!baseAddress.endsWith("/"))
		baseAddress = baseAddress + "/";
	var firstval = baseAddress + folderList[0];
	
	absoluteList.push(firstval);
	
	for (var i = 1 ; i < folderList.length -1 ; i++)  
	{
		if (folderList[i] != ""){
			var nextUrl = 	absoluteList[absoluteList.length -1 ] + "/" + folderList[i];
			absoluteList.push(nextUrl);
		}
	}
	
	return(absoluteList);
}

