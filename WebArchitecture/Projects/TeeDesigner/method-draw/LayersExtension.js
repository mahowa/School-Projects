/*
    Layers Extension written by
    Matt Howa
    Final Project Web Software Architecture
    Spring 2016
 */


var dialog="", form;
$(document).ready(function(){
    var v =  svgCanvas.getLayers();

    var layerHtml="";
    v.forEach(function(item,index){
        layerHtml+="<tr onclick='selectLayer(this)' style='background-color: rgba(0, 0, 0, 0.28);color: white; cursor: pointer; text-align: left'><th >" +
            "<img onclick='toggleVisibility(this)' src='images/LayerIcon/visible.png' height='25' width='25' style='margin-right: 15px'/><span class='layerName'>"+item[0]+"</span></th></tr>";

    });

    $('#LayerList').html(layerHtml);

   v = $('#LayerList').get()[0].childNodes;
    var index = v.length -1;
    v[index].click();
    $('.layerName').inlineEdit(replaceWith, connectWith);

});

var toggle = false;

function toggleVisibility(layer){
    toggle = true;
    var layerName = layer.parentNode.children[1].textContent;

    var  s = window.location.hostname.toString();
    s = "http://"+s+"/DesignStudio/TeeDesigner/method-draw/images/LayerIcon/visible.png";
    if(layer.src == s) {
        layer.src = "images/LayerIcon/hidden.png";
        svgCanvas.setLayerVisibility(layerName,false);
    }
    else {
        layer.src = "images/LayerIcon/visible.png";
        svgCanvas.setLayerVisibility(layerName,true);
    }

}

var original_color = "";


var selected = [];
var selectedLayer = "";

function selectLayer(layer){
    if(selected.length > 2)
        return;

    if(toggle){
        toggle = false;
        return;
    }

    var layerName = layer.children[0].innerText;

    if(original_color == "")
        original_color = layer.style.backgroundColor;

    if(layer.style.backgroundColor == "gray")
        layer.style.backgroundColor = original_color;
    else {
        if(!svgCanvas.setCurrentLayer(layerName))
            return;

        layer.style.backgroundColor = "gray";
        selectedLayer=layerName;
        $('#selectedLayer').html(layerName);
    }

    var index = selected.indexOf(layerName);
    if(index >= 0){
        selected.splice(index,1);
    }
    else{
        selected.push(layerName);
    }
}


function newLayer(){
    var layerName = "";

    if(dialog == "") {
        dialog = $("#newLayer_dialog").dialog({
            resizable: false,
            height: 140,
            modal: true,
            buttons: {
                "Create Layer": function () {
                    $(this).dialog("close");
                    layerName = $('#layerName').val();

                    var layers = svgCanvas.getLayers();

                    var appendVal = "";
                    for(var i = 0; i< layers.length; i++){
                        if(layers[i][0]== layerName + appendVal){
                            if(appendVal =="")
                                appendVal = 1;
                            else
                                appendVal++;
                        }
                    }
                    layerName += appendVal;

                    svgCanvas.createLayer(layerName);

                    var layerHtml = "<tr class='layer' onclick='selectLayer(this)' style='background-color: rgba(0, 0, 0, 0.28);color: white; cursor: pointer; text-align: left'><th >" +
                        "<img onclick='toggleVisibility(this)' src='images/LayerIcon/visible.png' height='25' width='25' style='margin-right: 15px'/><span class='layerName'>"+layerName+"</span></th></tr>";



                    //selected.push(layerName);
                    $('#LayerList').append(layerHtml);

                    clearSelectedLayers();


                    var index = v.length-1;
                    v[index].click();
                    $('.layerName').inlineEdit(replaceWith, connectWith);
                }
            }
        });
    }

    dialog.dialog("open");
}

function duplicateLayer(){
    var layerName = selectedLayer + " - Copy";
    svgCanvas.cloneLayer(layerName);
    selected.push(layerName);

    var layerHtml = "<tr class='layer' onclick='selectLayer(this)' style='background-color: rgba(0, 0, 0, 0.28);color: white; cursor: pointer; text-align: left'><th >" +
        "<img onclick='toggleVisibility(this)' src='images/LayerIcon/visible.png' height='25' width='25' style='margin-right: 15px'/><span class='layerName'>"+layerName+"</span> </th></tr>";


    var v = $('#LayerList').get()[0];

    $('#LayerList').append(layerHtml);

    for(var i=0; i< v.childNodes.length; i++){
        if(v.childNodes[i].innerText == selectedLayer)
            v.childNodes[i].click();

    }

    v = v.childNodes;
    var index = v.length -1;
    v[index].click();
    $('.layerName').inlineEdit(replaceWith, connectWith);
}

function deleteLayer(){

    var index = selected.indexOf(selectedLayer);
    if(index >= 0){
        selected.splice(index,1);
    }

    var v = $('#LayerList').get()[0];


    for(var i=0; i< v.childNodes.length; i++){
        if(v.childNodes[i].innerText == selectedLayer)
            v.removeChild(v.childNodes[i]);

    }

    svgCanvas.deleteCurrentLayer();

    index = v.childNodes.length -1;
    if(index<0)
        return;

    v.childNodes[index].click();
}

function mergeLayers(){
    svgCanvas.mergeLayersList(selected);
}

$.fn.inlineEdit = function(replaceWith, connectWith) {

    $(this).hover(function() {
        $(this).addClass('hover');
    }, function() {
        $(this).removeClass('hover');
    });

    $(this).click(function() {

        var elem = $(this);

        var row = elem[0].parentNode;

        if(selectedLayer != elem[0].textContent) {
            clearSelectedLayers();
            row.click();
        }

        elem.hide();
        elem.after(replaceWith);
        replaceWith.focus();

        replaceWith.blur(function() {

            if ($(this).val() != "") {
                connectWith.val($(this).val()).change();
                var prevTxt = elem[0].textContent;


                var layers = svgCanvas.getLayers();

                var appendVal = "";
                for(var i = 0; i< layers.length; i++){
                    if(layers[i][0]== $(this).val() + appendVal && $(this).val() + appendVal!= elem[0].textContent){
                        if(appendVal =="")
                            appendVal = 1;
                        else
                            appendVal++;
                    }
                }
                $(this).val($(this).val()+ appendVal);

                elem.text($(this).val());

                for(var i = 0; i< selected.length; i++){
                    if(selected[i]== prevTxt) {
                        selected[i] = $(this).val();
                        break;
                    }
                }
                selectedLayer =  elem[0].textContent;
                $('#selectedLayer').html($(this).val());
                svgCanvas.renameCurrentLayer($(this).val());
                $(this).val("");
            }

            $(this).remove();
            elem.show();
        });
    });
};

function clearSelectedLayers(){
    selected = [];
    var v = $('#LayerList').get()[0].childNodes;

    for(var i=0; i< v.length; i++){
        if(v[i].style.backgroundColor == "gray") {
            v[i].style.backgroundColor = original_color;
        }
    }
}