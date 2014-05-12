function uaWizard(configurationID)
{
    dnn.dom.setCookie("uaWizard", configurationID, 365, "/");
}

function uaCheckedChanged(src, path, itemID, actionName)
{
	$.get(
		path + "UAHandler.aspx",
		{ action: actionName, itemID: itemID, checked: $(src).parent().find("input").is(":checked"), configurationID: -1 },
		function(data)
		{
			if (data != "1")
			{
				alert(data);
			}
		}
		);
}

function uaEditItem(objThis)
{
	var objParent = $(objThis).closest("tr").find("div.editItem").parent();
	objParent.find("span").hide();
	objParent.find("div.editItem").show();
	return false;
}

function uaCancelItem(objThis)
{
	var objParent = $(objThis).closest("td");
	objParent.find("span").show();
	objParent.find("div.editItem").hide();
	return false;
}

function uaSaveItem(src, path, itemID, actionName)
{
	var objParent = $(src).closest("td");
	var objInput = objParent.find("input");
	objParent.find("div.editItem").hide();

	var objSend = new Object();
	objSend.itemID = itemID;
	objSend.value = objInput.val();

	$.ajax({
		type: "POST",
		async: true,
		url: path + "UAHandler.aspx?ConfigurationID=-1&action=" + actionName,
		data: objSend,
		contentType: "application/json",
		dataType: "text",
		success: function(msg)
		{
			if (msg == "1")
			{
				objParent.find("span").html(objInput.val()).show();
			}
			else
			{
				alert(msg);
			}
		},
		error: function(XMLHttpRequest, textStatus, errorThrown)
		{
			alert(textStatus);
		}
	});

	return false;
}

/*****		Search Function		*****/

function uaSearchTree(ctlSearchBox, ctlSearch)
{
	var highLightPrefix = "<span class=\"searchOk\">";
	var highLightSuffix = "</span>";

	var searchText = $(ctlSearchBox).val();
	var searchTextLower = searchText.toLowerCase();
	var treeView = $find(ctlSearch);
	var allNodes = treeView.get_allNodes();
	var idsToShow = "-1,";

	for (var i = 0; i < allNodes.length; i++)
	{
		if (allNodes[i].get_value() == "-1")
		{
			idsToShow += allNodes[i].get_value() + ",";
			continue;
		}

		allNodes[i].expand();
		var objNode = $(allNodes[i].get_textElement());

		var textToHighlight = objNode.html();
		if (textToHighlight.indexOf(highLightPrefix) != -1)
		{
			textToHighlight = textToHighlight.replace(highLightPrefix, "");
			textToHighlight = textToHighlight.replace(highLightSuffix, "");
		}

		if (searchText == "")
		{
			idsToShow += allNodes[i].get_value() + ",";
			objNode.html(textToHighlight);
			continue;
		}

		var lookingIndex = textToHighlight.toLowerCase().indexOf(searchTextLower);
		if (lookingIndex != -1)
		{
			idsToShow += allNodes[i].get_value() + ",";

			var currentParent = allNodes[i].get_parent();
			try
			{
				while (currentParent)
				{
					idsToShow += currentParent.get_value() + ",";
					currentParent = currentParent.get_parent();
				}
			}
			catch (exc)
			{ }

			var sub1 = textToHighlight.substr(0, lookingIndex);
			var sub2 = textToHighlight.substr(lookingIndex, searchText.length);
			var sub3 = textToHighlight.substr(lookingIndex + searchText.length);

			textToHighlight = sub1 + highLightPrefix + sub2 + highLightSuffix + sub3;
		}

		objNode.html(textToHighlight);
	}

	for (var i = 0; i < allNodes.length; i++)
	{
		var toCheck = "," + allNodes[i].get_value() + ",";
		if (idsToShow.indexOf(toCheck) == -1)
		{
			$(allNodes[i].get_element()).hide();
		}
		else
		{
			$(allNodes[i].get_element()).show();
		}
	}

	return false;
}

function uaSearchTable(ctlSearchBox, ctlSearch)
{
	var highLightPrefix = "<span class=\"searchOk\">";
	var highLightSuffix = "</span>";

	var searchText = $(ctlSearchBox).val();
	var searchTextLower = searchText.toLowerCase();
	var allNodes = $(ctlSearch + " tr td:nth-child(3) span");

	for (var i = 1; i < allNodes.length; i++)
	{
		var obj = $(allNodes[i]);
		var textToHighlight = obj.html();
		if (textToHighlight.indexOf(highLightPrefix) != -1)
		{
			textToHighlight = textToHighlight.replace(highLightPrefix, "");
			textToHighlight = textToHighlight.replace(highLightSuffix, "");
		}

		if (searchText == "")
		{
			obj.html(textToHighlight);
			obj.parent().parent().show();
			continue;
		}

		var lookingIndex = textToHighlight.toLowerCase().indexOf(searchTextLower);
		if (lookingIndex != -1)
		{
			var sub1 = textToHighlight.substr(0, lookingIndex);
			var sub2 = textToHighlight.substr(lookingIndex, searchText.length);
			var sub3 = textToHighlight.substr(lookingIndex + searchText.length);

			textToHighlight = sub1 + highLightPrefix + sub2 + highLightSuffix + sub3;
			obj.parent().parent().show();
		}
		else
		{
			obj.parent().parent().hide();
		}

		obj.html(textToHighlight);
	}

	for (var i = 0; i < allNodes.length; i++)
	{
		var toCheck = "," + allNodes[i].get_value() + ",";
		if (idsToShow.indexOf(toCheck) == -1)
		{
			$(allNodes[i].get_element()).hide();
		}
		else
		{
			$(allNodes[i].get_element()).show();
		}
	}

	return false;
}
