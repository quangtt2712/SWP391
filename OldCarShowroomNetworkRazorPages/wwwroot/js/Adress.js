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
                        imgshowroom: b.imageShowrooms[0].url
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


								<img src="${item.imgshowroom == null ? "" : item.imgshowroom}" alt="Ảnh chính" data-id ="${item.id}" class = "showroomimg">
						
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
            
    } else {
        alert("Vui lòng chọn đủ phường");
    }
})

