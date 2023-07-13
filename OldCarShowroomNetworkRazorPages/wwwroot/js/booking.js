const select_list = document.querySelector('#listCarName')
// function render invalid div
const createMsg = (parentNode, msg, className) => {
    const invalidDiv = document.createElement("div")
    invalidDiv.className = className
    invalidDiv.innerHTML = msg
    parentNode.appendChild(invalidDiv)
}
// function clear invalid
const clearMsg = (className) => {
    document.querySelectorAll('.' + className + '').forEach((item) => {
        item.remove()
    });
}
select_list.value = ""
select_list.addEventListener('change', (e) => {
    console.log(e.target.value)

    const queryString = window.location.search;
    console.log(queryString);
    const urlParams = new URLSearchParams(queryString);
    const id = urlParams.get('id')
    console.log(id);
    clearMsg('warning_slot')
    fetch(`/api/booking/booking?carId=${id}&isBooked=${e.target.value}&dateTime=${document.querySelector('#dateTime').value}`)
        .then(res => res.status)
        .then(ok => {
          
            console.log(ok)
            if (ok != 404) {
                select_list.value = ""
                createMsg(select_list.parentNode,'Slot này đã được book', 'warning_slot')
             
            }
        })

})
const picker_date_input = document.querySelector('#dateTime')
picker_date_input.addEventListener('change', () => {
    select_list.value = ""
})

const compare_dates = (d1, d2, parentNode) => {
    let date1 = new Date(d1).getTime();
    let date2 = new Date(d2).getTime();

    if (date1 < date2) {
        console.log(`${d1} is less than ${d2}`);
        return false
    } else if (date1 > date2) {
        console.log(`${d1} is greater than ${d2}`);
        return true
    }
};

picker_date_input.addEventListener('change', function () {
    let currentDate = new Date()
    let chosenDate = new Date(this.value)
    console.log(new Date(this.value))
    clearMsg('invalid_date')
    let currentDateByFormat = (currentDate.getMonth() + 1) + '/' + currentDate.getDate() + '/' + currentDate.getFullYear()
    let chosenDateFormat = (chosenDate.getMonth() + 1) + '/' + chosenDate.getDate() + '/' + chosenDate.getFullYear()

    if (compare_dates(currentDateByFormat, chosenDateFormat, this.parentNode)) {
        createMsg(this.parentNode, 'Chỉ được chọn ngày hiện tại trở đi', 'invalid_date')
        this.value = ""
    }

})