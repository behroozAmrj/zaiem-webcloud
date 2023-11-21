var RBUrlExecute = "../../../Services/GateWayService.asmx/Execute_Method";
var RBUrlService = "../../../Services/GateWayService.asmx/CloudService_ExecuteMethod";
var RBtype = "POST";
var RBdatatype = "json";
var RBcontenttype = "application/json;charset=utf-8";
var RBprocessData = false;
function validateParameters_exec(appName , className ,methodName)
{
	var result = false;
	if ((!appName == "") &&
	    (!className == "")&&
		(!methodName == ""))
		result = true;
	else
		result = false;
	return(result);
}

function validateParameters_Service(appName , serviceName , className ,methodName)
{
	var result = false;
	if ((!appName == "") &&
	    (!className == "")&&
		(!methodName == ""))
		result = true;
	else
		result = false;
	return(result);
}
function excute_Method(appname , classname , methodName , data , onCallbackResult , onErrorMethod , onTimeout)
{
	if(!validateParameters_exec(appname , classname , methodName))
	{
		alert('one or more parameters are null or empty');
		return;
	}
	
	console.log('calling Service Started . . . ');
    var RBdata = JSON.stringify({
                    AppName: appname,
                    ClassName: classname,
                    MethodName: methodName,
                    Params: data
                });
    $.ajax({
            type: RBtype, //GET or POST or PUT or DELETE verb
            url: RBUrlExecute, // Location of the service
            data: RBdata, //Data sent to server
            contentType: RBcontenttype, // content type sent to server
            dataType: RBdatatype, //Expected data format from server
            processdata: false, //True or Fal
            success: onCallbackResult,
			error : onErrorMethod});
}

function execute_Service(appName , serviceName , className , methodName ,data , onCallbackResult , onErrorMethod , onTimeout)
{
	if(!validateParameters_Service(appName ,serviceName , className , methodName))
	{
		alert('one or more parameters are null or empty');
		return;
	}
	
	var RBdata = JSON.stringify({
                     AppName: appName,
                    ServiceName : serviceName,
                    ClassName: className,
                    MethodName: methodName,
                    Params: data
                });
    $.ajax({
            type: RBtype, //GET or POST or PUT or DELETE verb
            url: RBUrlService, // Location of the service
            data: RBdata, //Data sent to server
            contentType: RBcontenttype, // content type sent to server
            dataType: RBdatatype, //Expected data format from server
            processdata: false, //True or Fal
            success: onSuccessMethod,
			error : onErrorMethod});
}