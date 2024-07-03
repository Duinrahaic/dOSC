
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

    if (event.ctrlKey==true && (event.which == '61' || event.which == '107' || event.which == '173' || event.which == '109'  || event.which == '187'  || event.which == '189'  ) ) {
        event.preventDefault();
    }
});
// Add wheel event listener to prevent Ctrl+Scroll
document.addEventListener('wheel', function(event) {
    if (event.ctrlKey) {
        event.preventDefault();
    }
}, { passive: false });
function resizeInput(input) {
    const textLength = input.value.length + 3;
    input.style.width = textLength  +  'ch' ;
}
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


function scrollToBottom (element){
    const $element = $(element);

    if ($element.length === 0) {
        console.error('Element not found.');
        return;
    }

    $element.scrollTop($element.prop("scrollHeight"));
};


function copyTextWithAnimation(text) {
    // Copy text to clipboard
    navigator.clipboard.writeText(text);
}

window.customDropdown = {
    setupOutsideClickListener: function (dropdownElement, dotNetHelper) {
        $(document).on('click', function (event) {
            if (!$(dropdownElement).is(event.target) && $(dropdownElement).has(event.target).length === 0) {
                dotNetHelper.invokeMethodAsync('CloseDropdown');
                $(document).off('click'); // Remove the event listener
            }
        });
    }
};


window.mousePosition = {
    x: 0,
    y: 0
};

document.addEventListener('mousemove', function (event) {
    window.mousePosition.x = event.clientX;
    window.mousePosition.y = event.clientY;
});

window.getMousePosition = function () {
    return window.mousePosition;
};