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

const listcities = document.getElementById("listCity");
const listDistrictNames = document.getElementById("listDistrictName");
const listWardNames = document.getElementById("listWardName");
const search = document.getElementById("save");
search.addEventListener("click", (e) => {
    e.preventDefault()
    if (listcities.value != "" && listDistrictNames.value != "" && listWardNames.value != "") {
        fetch(`/api/Adress/city/${listcities.value}/district/${listDistrictNames.value}/ward/${listWardNames.value}`)
            .then(data => data.json())
            .then(listShowrooms => {
                console.log(listShowrooms)
                let htmls = listShowrooms.map(showroom => `
                   <div class="col mb-5">
                    <a asp-page="./Create" class="card h-100" asp-route-id="${showroom.showroomId}">
                        <!-- Hình ảnh sản phẩm-->
                            @if(${showroom.image} != null)
                        {
                            <img class="card-img-top" src="${showroom.image.url}" alt="Ảnh showroom" />
                        }
                        
                             <div class="card-body p-4">
                            <div class="text-center">
                                <!-- Tên sản phẩm-->
                                <h5 class="fw-bolder">${showroom.showroomName}</h5>
                                <!-- Giá sản phẩm-->
                            ${showroom.address}
                            </div>
                        </div>
                        
                        <!-- Chi tiết sản phẩm-->
                       
                        

                    </a>
                </div>
`)
                document.getElementById("listshowroom").innerHTML = htmls.join('')
            })
    }
    else {
        alert("abc")
    }
})
