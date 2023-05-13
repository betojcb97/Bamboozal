function formatISODate(isoDate) {
    let date = new Date(isoDate);
    let day = ("0" + date.getDate()).slice(-2); // Get the day and prepend 0 if necessary
    let month = ("0" + (date.getMonth() + 1)).slice(-2); // Get the month (0-indexed) and prepend 0 if necessary
    let year = date.getFullYear();
    let hours = ("0" + date.getHours()).slice(-2);
    let minutes = ("0" + date.getMinutes()).slice(-2);

    return day + '/' + month + '/' + year + ' ' + hours + ':' + minutes;
}