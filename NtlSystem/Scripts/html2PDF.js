const a4 = [595.28, 841.89];

function genQrCode(str) {
    let QR_CODE = new QRCode("qrcode", {
        width: 200,
        height: 200,
        colorDark: "#000000",
        colorLight: "#ffffff",
        correctLevel: QRCode.CorrectLevel.H,
    });

    QR_CODE.makeCode(str);
}

// create canvas object  
function getCanvas(id) {
    let html_form = $(`#${id}`);

    return html2canvas(html_form, {
        onclone: function (clonedDoc) {
            clonedDoc.getElementById(`${id}`).style.display = 'block';
        }
    });
}

//create pdf  
function createPDF(id, fileName) {
    let html_form = $(`#${id}`);

    let cache_width = html_form.width();

    html_form.width((a4[0] * 1.33333) - 80).css('max-width', 'none');

    getCanvas(id).then(canvas => {
        let img = canvas.toDataURL("image/png");

        let doc = new jsPDF({
            unit: 'px',
            format: 'a4'
        });

        doc.addImage(img, 'JPEG', 20, 20);
        doc.save(`${fileName}.pdf`);
    });

    html_form.width(cache_width);
}

jQuery.fn.html2canvas = function (options) {
    html2canvas.logging = options && options.logging;
    html2canvas.Preload(this[0], $.extend({
        complete: function (images) {
            let queue = html2canvas.Parse(this[0], images, options),
                $canvas = $(html2canvas.Renderer(queue, options));

            $canvas.css({
                position: 'absolute',
                left: 0,
                top: 0
            }).appendTo(document.body);

            $canvas.siblings().toggle();

            $(window).click(function () {
                if (!$canvas.is(':visible')) {
                    $canvas.toggle().siblings().toggle();
                } else {
                    $canvas.siblings().toggle();
                    $canvas.toggle();
                }
            });
        }
    }, options));
};