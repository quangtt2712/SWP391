const title = document.querySelector('#title')

const switch_btn = document.querySelector('.sign_up_box')
let isSignIn = true


// function render invalid div
const createMsg = (parentNode, msg, className) => {
    const invalidDiv = document.createElement("div")
    invalidDiv.className = className
    invalidDiv.innerHTML = msg
    parentNode.appendChild(invalidDiv)
}
// clear MSG
const clearMsg = (className) => {
    document.querySelectorAll('.' + className + '').forEach((item) => {
        item.remove()
    });
}

// check valid email function
const checkValid_Email = (value, ele) => {
    clearMsg('warning_mail')
    if (value.length == 0) {
        clearMsg('warning_mail')
        return false
    }
    const regex_mail = /[a-zA-Z0-9]@bikes.shop$/g
    // console.log(regex_mail.test(value))
    if (!regex_mail.test(value)) {

        createMsg(ele.parentNode, `Email need end with <span class='mail_regex'>@bikes.shop</span>`, 'warning_mail')
        return false
    }
    return true

}
// 
const setRotate = (ele, deg) => {
    return new Promise((resolve, reject) => {
        ele.classList.add('rotate')
        ele.style.transform = 'rotateY(' + deg + ')'
        setTimeout(function () {
            ele.classList.remove('rotate')
            resolve()
        }, 500)

    })
}
// 
document.querySelector('#email').addEventListener('input', (e) => checkValid_Email(e.target.value, e.target))

const handle_switch_login = (ele) => {
    clearMsg('warning_sign')
    clearMsg('warning_password')
    clearMsg('warning_mail')
    setRotate(document.querySelector('.drop'), '-100deg')
        .then(() => {

            document.querySelector('#email').value = ''
            document.querySelector('#password').value = ''
            document.querySelector('.drop').style.transform = 'none'
            return new Promise(res => {
                setTimeout(() => res(), 200)
            })
        })
        .then(() => {
            if (isSignIn) {
                title.innerText = 'Sign Up Form'
                ele.innerText = 'Sign In ?'

                document.querySelector('.login-input').value = 'sign up'
                isSignIn = false

                return
            }
            ele.innerText = 'Sign Up ?'
            title.innerText = 'Sign In Form'
            document.querySelector('.login-input').value = 'sign in'
            isSignIn = true

        })


}
const containsWhitespace = str => /\s/.test(str);

const checkValid_password = (pw, ele) => {
    clearMsg('warning_password')
    console.log(containsWhitespace(pw))
    if (containsWhitespace(pw)) {
        createMsg(ele.parentNode, 'Password không được chứa khoảng cách', 'warning_password')
        return false
    }
    if (pw.length == 0) {
        clearMsg('warning_password')
        return false
    }
    if (pw.length < 8 || pw.trim().length == 0) {
        createMsg(ele.parentNode, 'Password need more 8 characters and no space', 'warning_password')
        return false
    }
    return true
}
switch_btn.addEventListener('click', (e) => handle_switch_login(e.target))
document.querySelector('#password').addEventListener('input', (e) => checkValid_password(e.target.value, e.target))



document.querySelector('.login-input').addEventListener('click', function (e) {
    const mail = document.querySelector('#email')
    const password = document.querySelector('#password')
    clearMsg('warning_sign')
    if (!(checkValid_Email(mail.value, mail) && checkValid_password(password.value, password))) {
        createMsg(this.parentNode, 'Please fill full details', 'warning_sign')
        return
    }
})