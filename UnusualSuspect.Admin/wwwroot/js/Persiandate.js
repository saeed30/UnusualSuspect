function DateFuntion(date) {

    if (date != '') {
        var pdate = new Date(Date.parse(date));
        var p = new PersianDate(pdate);
        return p.toString("yyyy/MM/dd - HH:mm");
    }
    else
        return '';
}

function DateOnlyFuntion(date) {
    debugger;
    if (date != '') {
        var pdate = new Date(Date.parse(date));
       
        var p = new PersianDate(pdate);
        return p.toString("yyyy/MM/dd");
    }
    else
        return '';
}
