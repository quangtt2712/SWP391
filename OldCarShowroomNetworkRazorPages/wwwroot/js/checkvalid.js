function getDistrictName(eel) {
    console.log(eel.value);
    fetch("/api/Adress/district/" + eel.value)
        .then(response => response.json())
        .then(districtsList => {
            console.log(districtsList);
            const select = document.getElementById("listDistrictName");
            let htmls = districtsList.map(district => `<option value="${district.districtId}" > ${district.name} </option>`);
            select.innerHTML = htmls.join("");
            const ward = document.querySelector("#listWardName");
            ward.value = "";
        })
        .catch(error => {
            console.error('Error:', error);
        });
}

function getWardName(eel) {
    console.log(eel.value);
    fetch("/api/Adress/ward/" + eel.value)
        .then(response => response.json())
        .then(wardsList => {
            console.log(wardsList);
            const select = document.getElementById("listWardName");
            let htmls = wardsList.map(ward => `<option value="${ward.wardId}" > ${ward.name} </option>`);
            select.innerHTML = htmls.join("");
        })
        .catch(error => {
            console.error('Error:', error);
        });
}
window.addEventListener('DOMContentLoaded', function () {
    getDistrictName(document.querySelector("#listCity"));
    getWardName(document.querySelector("#listDistrictName"));
});

const checkvalidphone = document.getElementById("checkvalidphone")

const checkValid_Phone = () => {
    /* const wanrning_phone_div = document.querySelector('.warning_phone')*/
    let regex = /((09|03|07|08|05)+([0-9]{8})\b)/g;
    document.getElementById("warningphone").innerHTML = ""
    if (!regex.test(checkvalidphone.value)) {
        document.getElementById("warningphone").innerHTML = 'Số điện thoại không đúng định dạng.'
        return false
    }
    return true

}
checkvalidphone.addEventListener("input", checkValid_Phone)
const checkvalidimg = document.getElementById("uploadimgmain");
const checkvalid = document.getElementById("checkvalid");
const listcities = document.getElementById("listCity");
const listDistrictNames = document.getElementById("listDistrictName");
const listWardNames = document.getElementById("listWardName");
const checkvalidname = document.getElementById("checkvalidname");
const checkvalidaddress = document.getElementById("checkvalidaddress");
checkvalid.addEventListener("click", (e) => {
    console.log(checkvalidimg.value)
    if (checkvalidimg.value == "") {
        e.preventDefault()
        document.getElementById("warningmain").innerHTML = "Them img"
    }
    if (!checkValid_Phone()) {
        e.preventDefault()
        document.getElementById("warningsave").innerHTML = "Vui lòng nhập đầy đủ thông tin"
    }
    if (listWardNames.value == "") {
        e.preventDefault()

        alert("Vui lòng chọn đủ phường");

    }
    if (checkvalidname.value == "") {
        
        e.preventDefault()
        document.getElementById("warningname").innerHTML = "Vui lòng nhập đầy đủ thông tin"
    }
    if (checkvalidaddress.value == "") {
        e.preventDefault()
        document.getElementById("warningaddress").innerHTML = "Vui lòng nhập đầy đủ thông tin"
    }
    
})

checkvalidimg.addEventListener('change', function () {
    let file = this.files[0]
    console.log(file.name)
    document.getElementById("warningmain").innerHTML = "";
    let file_name = file.name
    let idsx_dot = file_name.lastIndexOf('.') + 1
    let extFile = file_name.substr(idsx_dot, file_name.length).toLowerCase()
    console.log(extFile)
    if (extFile == "jpg" || extFile == "jpeg" || extFile == "png") {
        courseImage.src = URL.createObjectURL(file)
    } else {
        checkvalidimg.value = ''
        document.getElementById("warningmain").innerHTML = "Vui lòng chọn ảnh định dạng .jpg/.jpeg/.png";
    }
    // let idxDot = fileName.lastIndexOf(".") + 1;
    // let extFile = fileName.substr(idxDot, fileName.length).toLowerCase();


})