function formatISODate(isoDate) {
    let date = new Date(isoDate);
    let day = ("0" + date.getDate()).slice(-2); // Get the day and prepend 0 if necessary
    let month = ("0" + (date.getMonth() + 1)).slice(-2); // Get the month (0-indexed) and prepend 0 if necessary
    let year = date.getFullYear();
    let hours = ("0" + date.getHours()).slice(-2);
    let minutes = ("0" + date.getMinutes()).slice(-2);

    return day + '/' + month + '/' + year + ' ' + hours + ':' + minutes;
}

function showAlert(message) {
    const alertElement = document.createElement('div');
    alertElement.classList.add('alert-message');
    alertElement.textContent = message;

    const alertContainer = document.getElementById('alertContainer');
    alertContainer.appendChild(alertElement);

    setTimeout(() => {
        alertElement.classList.add('fade-out');
        setTimeout(() => {
            alertElement.remove();
        }, 500); 
    }, 5000); 
}

function createModal(modalLabel, modalId, formId, sendBtnId, attributes) {
    let modal = '';
    modal += `<div class="modal fade" id="` + modalId + `" tabindex="-1" role="dialog" aria-hidden="true">`;
    modal += `<div class="modal-dialog" role="document">
                <div class="modal-content">
                <div class="modal-header">
                <h5 class="modal-title" id="`+ modalLabel + `">` + modalLabel + `</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span> </button>
                </div>
                <div class="modal-body">`;
    modal += `<form id="` + formId + `">`;
    attributes.forEach(function (att) {
        modal += `<div class="` + att["class"] + `">
                     <label for="`+ att["name"] + `">` + att["name"] + `</label>`;
        if (att["type"] === "select") {
            modal += `<select class="form-control" id="` + att["name"] + `" name="` + att["name"] + `"></select>`;
        }
        else if (att["type"] === "textarea") {
            modal += `<textarea class="form-control" id="` + att["name"] + `" name="` + att["name"] + `"></textarea>`;
        }
        else {
            modal += `<input type="` + att["type"] + `" class="form-control" id="` + att["name"] + `" name="` + att["name"] + `">`;
        }
        modal += `</div>`;
    });
    modal += `</form></div>`;
    modal += `<div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary" id="` + sendBtnId + `">Send</button>
                </div>`;
    modal += `</div></div></div>`;
    return modal;
}

function assureLogin() {
    debugger;
    authToken = localStorage.getItem('authToken');
    if (authToken === null || authToken === '') { window.location.href = '/CustomUser/LoginPage'; }
}