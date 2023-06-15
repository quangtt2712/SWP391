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
/*const listshowroom = []*/
search.addEventListener("click", (e) => {
    e.preventDefault()
    if (listcities.value != "" && listDistrictNames.value != "" && listWardNames.value != "") {
        fetch(`/api/Adress/city/${listcities.value}/district/${listDistrictNames.value}/ward/${listWardNames.value}`)
            .then(data => data.json())
            .then(listShowrooms => {
                /*console.log(listShowrooms)*/
                const a = listShowrooms.reduce((a, b) => {
                    a.push({
                        id: b.showroomId,
                        showroomName: b.showroomName,
                        address: b.address,
                        imgshowroom: ""
                    })
                    return a;
                }, [])
                return new Promise(res => res(a))
            })
            .then(list => {
                console.log(list)
                let htmls = list.map(item => `
                    <div class="col mb-5">
					<a href="/Car/Create?id=${item.id}" class="card h-100">


								<img src="" alt="Ảnh chính" data-id ="${item.id}" class = "showroomimg">
						
						<div class="card-body p-4">
							<div class="text-center">
								<!-- Tên sản phẩm-->
								<h5 class="fw-bolder">${item.showroomName}</h5>
                           
								<!-- Giá sản phẩm-->
							${item.address}
							</div>
						</div>
						<!-- Chi tiết sản phẩm-->
					</a>
				</div>
`)
                document.getElementById("listshowroom").innerHTML = htmls.join("")
            })
            .then(() => {
                let att = document.querySelectorAll(".showroomimg")
                att.forEach(attr => {
                    console.log(attr.getAttribute("data-id"))
                    fetch(`/api/Adress/imageShowroom/${attr.getAttribute("data-id")}`)
                        .then(data => data.json())
                        .then(img => {
                            console.log(img)
                            attr.src = img.url;
                        })
                })
            })
    } else {
        alert("Vui lòng chọn đủ phường");
    }
})

