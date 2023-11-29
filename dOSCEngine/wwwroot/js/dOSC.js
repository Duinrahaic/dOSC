function addTooltips() {
    $('[data-toggle="tooltip"]').tooltip({
        trigger: 'hover'
    });
    $('[data-toggle="tooltip"]').on('mouseleave', function () {
        $(this).tooltip('hide');
    });
    $('[data-toggle="tooltip"]').on('click', function () {
        $(this).tooltip('dispose');
    });
}

function saveAsFile(filename, bytesBase64) {
    var link = document.createElement('a');
    link.download = filename;
    link.href = "data:application/octet-stream;base64," + bytesBase64;
    document.body.appendChild(link); // Needed for Firefox
    link.click();
    document.body.removeChild(link);
}

function GenerateToasterMessage(HTML, Duration = 3000) {
    var el = document.createElement("div");
    el.className = "snackbar";
    var y = document.getElementById("snackbar-container");
    el.innerHTML = HTML;
    y.append(el);
    el.className = "snackbar show";
    setTimeout(function () { el.className = el.className.replace("snackbar show", "snackbar"); }, Duration);
}