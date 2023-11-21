$(document).ready(function(){
	$("#newFolder_buttom").click(function(){
		var newFolder = $("#newFolder_text").val();
		if (newFolder != "")
		{
			var currentAddress = $("#currentAddress").val();
			createNewFolder(currentAddress , 
							newFolder);
			$("#newFolder_text").val("");
		}
	});
	
	$("#deleteItem_buttom").click(function(){
		console.log("delete item buttom");
		$(".item.folder input[type='checkbox']:checked").each(function(){
			var url = $(this).parent().find("input[type='hidden']:eq(0)").val();
			if(!url.endsWith("/"))
				url = url + "/";
			console.log(url);
			delete_folder(url);
			$(this).parent().remove();
			
		});
		
		$(".item.file input[type='checkbox']:checked").each(function(){
			var url = $(this).parent().find("input[type='hidden']:eq(0)").val();
			if(!url.endsWith("/"))
			{
				console.log(url);
				delete_file(url);
				$(this).parent().remove();
			}
		});
	});
	
	$("#upload_buttom").click(function(){
		/*var fileupload = document.getElementById('upload_file');
		uploadFileToServer(fileupload.files[0]);*/
		var blackCurtain = $("#blackCurtain");
		blackCurtain.css("display",
						"block");
							
		var uploadPanel = $("#upload_div");
		uploadPanel.css("display",
						"block");

	});
});