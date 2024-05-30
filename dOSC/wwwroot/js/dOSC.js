
$(document).keydown(function(event) {
    // Check if Ctrl key is pressed
    if (event.ctrlKey) {
        // Prevent default action for Ctrl+S
        if (event.key === 's' || event.key === 'S') {
            event.preventDefault();
         }
        // Prevent default action for Ctrl+G
        if (event.key === 'g' || event.key === 'G') {
            event.preventDefault();
         }
    }
});


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
    setTimeout(function () {
        el.className = el.className.replace("snackbar show", "snackbar");
    }, Duration);
}


function GetBlockDimensions(Id) {
    var block = document.getElementById(Id);
    if (block == null) {
        return {Height: 0, Width: 0};
    } else {
        return {Height: block.clientHeight, Width: block.clientWidth};
    }
}

function textAreaAdjust(element) {
    element.style.height = "1px";
    element.style.height = (25 + element.scrollHeight) + "px";
}