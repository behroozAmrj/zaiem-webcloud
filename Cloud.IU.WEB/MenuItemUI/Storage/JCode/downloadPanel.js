	function downloadFile(url)
	{
		var wCServer = "/VMManagement/downloadFile";
		var request = new XMLHttpRequest();
		request.load = function(data)
		{
			console.log(data);
		}
		
		request.open("GET",
                       wCServer,
                       true);

        request.setRequestHeader("Content-Type", "application/json;");//
		request.send(JSON.stringify({ URL : url}));
	}

