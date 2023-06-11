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
