const close_btn = document.querySelector('#close')
const popup = document.querySelector('#modal_popup')
const overlay = document.querySelector('#overlay')
const submit_btn = document.querySelector('#update')
let year_input = document.querySelector('#year_input')
let name_input = document.querySelector('#name_input')
let price_input = document.querySelector('#price_input')
// display popup
function showPopUp() {
    popup.style.display = 'block'
    overlay.style.display = 'block'
}
const formatPrice = (str) => {
    if (str.length == 0) {
        return ""
    }

    return str.trim().split('').reverse().reduce((prev, next, index) => {
        return ((index % 3) ? next : (next + '.')) + prev
    })
}
const checkPrice = (value) => {
    if (/[^0-9]/.test(value) || value == "") {
        return false
    }
    return true
}

const check_valid_price = (price, ele) => {
    price = price.replace(/[^0-9]/g, '');
    if (/^0/.test(price)) {
        price = price.replace(/^0/, "")
    }
    ele.value = formatPrice(price.split('.').join(""))
}
const checkYear = (year) => {
    let text = /[^[0-9]+$/
    let current_year = new Date().getFullYear();

    if (year != 0) {
        if ((year != "") && year.length == 4 && (year > 1920 && year <= current_year) && (!text.test(year))) {
            return true;
        }

        if (year.length != 4) {
            return false;
        }
    }
    return false

}
// close popup
// const close_popup = () => {
//     popup.style.display = 'none'
//     overlay.style.display = 'none'
// }
// close_btn.addEventListener('click', () => close_popup())

const check_valid_year = (year, ele) => {
    if (year == "") {
        clearMsg('warning_year')
        return
    }
    clearMsg('warning_year')

    if (!checkYear(year)) {
        createMsg(ele.parentNode, 'Vui lòng nhập đúng định dạng', 'warning_year')
    }
}
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
const check_valid_all = (ele) => {
    let year_input = document.querySelector('#year_input')
    let name_input = document.querySelector('#name_input')
    let price_input = document.querySelector('#price_input')
    console.log(price_input.value, year_input.value, name_input.value.trim())
    clearMsg('warning_submit')

    console.log(checkPrice(price_input.value.split('.').join('')))
    if (!(checkPrice(price_input.value.split('.').join('')) && checkYear(year_input.value) && name_input.value.trim() != "")) {
        createMsg(ele.parentNode, 'Please input full details', 'warning_submit')
        return
    }
    year_input.value = ""
    name_input.value = ""
    price_input.value = ""
    // close_popup()
}



price_input.addEventListener('input', (e) => { check_valid_price(e.target.value, e.target) })
submit_btn.addEventListener('click', (e) => { check_valid_all(e.target) })
year_input.addEventListener('input', (e) => { check_valid_year(e.target.value, e.target) })

const create_btn = document.querySelector('#create_btn')
const modal_popup = document.querySelector('#modal_popup')



// create_btn.addEventListener('click', function () {
//     modal_popup.style.display = 'block'
//     overlay.style.display = 'block'
//     modal_popup.innerHTML = `
//     <div class="input_group">
//             <label for="">Name</label>
//             <input  id="name_input" type="text">
//         </div>
//         <div class="input_group">
//             <label for="">Brand</label>
//             <select name="list_brand" id="brand">
//                 <option value="1">Brand 1</option>
//                 <option value="2">Brand 2</option>
//                 <option value="3">Brand 3</option>
//                 <option value="4">Brand 4</option>

//             </select>
//         </div>
//         <div class="input_group">
//             <label for="">Category</label>
//             <select name="list_category" id="category">
//                 <option value="1">Category 1</option>
//                 <option value="2">Category 2</option>
//                 <option value="3">Category 3</option>
//                 <option value="4">Category 4</option>

//             </select>
//         </div>
//         <div class="input_group">
//             <label for="">Model Year</label>
//             <input onInput={check_valid_year(this.value,this)} id="year_input" type="text">
//         </div>
//         <div class="input_group">
//             <label for="price_input">Price</label>
//             <input  onInput={check_valid_price(this.value,this)} id="price_input" type="text">
//         </div>
//         <div style="display: flex;justify-content: center;flex-direction: column;align-items: center;">
//             <button onClick={check_valid_all(this)} id="update">Create</button>
//         </div>


//         <div onClick={close_popup()} id="close"><i class="fa-solid fa-xmark"></i></div>
//     `
// })