window.ScrollToBottom = (elementName) => {
    element = document.getElementById(elementName);
    element.scrollTop = element.scrollHeight - element.clientHeight;
}

window.blazorHelpers = {
    simulateClick: function (element) {
        element.click();
    }
};