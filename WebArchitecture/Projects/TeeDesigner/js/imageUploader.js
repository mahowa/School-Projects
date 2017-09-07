/**
 * Created by Matt willdenms on 5/2/16.
 */





// Controlls the selector box for switching to load images
$(document).ready( function () {
    $("#myDropzone").hide();
    $("#clear-dropzone").hide();
    $(".demo-wrap").hide();

});

$(document).ready( function () {
    $("#customProduct").click ( function() {
        //
        //var val = $("#sel1").val();
        //if(val == 5) {

            $("#myDropzone").show('slow');
            $("#shirtDiv").hide('slow');
            $("#imageUploadDiv").show('slow');
            $("#clear-dropzone").show('fast');
            $('#thumbs').hide();


        //}
        //else if(val != 5) {
        //    $("#myDropzone").hide('fast');
        //    $("#clear-dropzone").hide('fast');
        //    $("#tshirtFacing").show('fast');
        //    $("#tshirtFacing").attr('src', 'img/crew_front.png');
        //    $(".demo-wrap").hide();
        //
        //}
    });

});




var imageCroppie = (function () {

    function output(node) {
        var existing = $('#result .croppie-result');
        if (existing.length > 0) {
            existing[0].parentNode.replaceChild(node, existing[0]);
        }
        else {
            $('#result')[0].appendChild(node);
        }
    }


    function popupResult(result) {
        var html;
        if (result.html) {
            html = result.html;
        }
        if (result.src) {
            html = '<img src="' + result.src + '" />';
        }
        swal({
            title: '',
            html: true,
            text: html,
            allowOutsideClick: true
        });
        setTimeout(function () {
            $('.sweet-alert').css('margin', function () {
                var top = -1 * ($(this).height() / 2),
                    left = -1 * ($(this).width() / 2);

                return top + 'px 0 0 ' + left + 'px';
            });
        }, 1);
    }

    function bindNavigation() {
        var $body = $('body');
        $('nav a').on('click', function (ev) {
            var lnk = $(ev.currentTarget),
                href = lnk.attr('href'),
                targetTop = $('a[name=' + href.substring(1) + ']').offset().top;

            $body.animate({scrollTop: targetTop});
            ev.preventDefault();
        });
    }


    function demoBasic() {
        var $w = $('.basic-width'),
            $h = $('.basic-height'),
            element = $('.myCroppie');

        element.croppie({
            viewport: {
                width: 100,
                height: 100
            },
            boundary: {
                width: 350,
                height: 350
            }
        });


        $('.basic-result').on('click', function() {
            var w = parseInt($w.val(), 10),
                h = parseInt($h.val(), 10),s
            size = 'viewport';
            if (w || h) {
                size = { width: w, height: h };
            }
            basic.croppie('result', {
                type: 'canvas',
                size: size
            }).then(function (resp) {
                popupResult({
                    src: resp
                });
            });
        });
    }





    function init() {
        bindNavigation();
        //demoBasic();
        loadCrop();
        //demoVanilla();
    }

    return {
        init: init
    };
})();

