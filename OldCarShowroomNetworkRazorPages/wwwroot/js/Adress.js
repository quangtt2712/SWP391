function getDistrictName(eel) {
    console.log(eel.value)
    fetch("/api/Adress/district/" + eel.value)
        .then(districts => districts.json())
        .then(districtsList => {
            console.log(districtsList)
            const select = document.getElementById("listDistrictName")
            let htmls = districtsList.map(district => `<option value="${district.districtId}" > ${district.name} </option>`)
            select.innerHTML = htmls.join("")
            const ward = document.querySelector("#listWardName")
            ward.value = ""
            
        })
}

function getWardName(eel) {
    console.log(eel.value)
    fetch("/api/Adress/ward/" + eel.value)
        .then(wards => wards.json())
        .then(wardsList => {
            console.log(wardsList)
            const select = document.getElementById("listWardName")
            let htmls = wardsList.map(ward => `<option value="${ward.wardId}" > ${ward.name} </option>`)
            select.innerHTML = htmls.join("")
        })
}

window.onload = getDistrictName(document.querySelector("#listCity"));
window.onload = getWardName(document.querySelector("#listDistrictName"));