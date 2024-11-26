document.addEventListener("DOMContentLoaded", () =>
{
    goNextHousePage()
    goPreviousHousePage()
    updateHouse()
    deleteHouseButton()
    applyHouseFilter()
})

function goNextHousePage()
{
    let btn = document.querySelector(".next_houses");
    if(btn != null){
        btn.addEventListener("click", () => {
            const qstr = window.location.search;
            const urlParams = new URLSearchParams(qstr);
            let cnt = urlParams.get("pagenumber") == null ? 1 : urlParams.get("pagenumber");
            let num = parseInt(cnt);
            num += 1;
            window.location = `/house?pagenumber=${num}`;
        })
    }
}

function goPreviousHousePage() {
    let btn = document.querySelector(".previous_houses");
    if(btn != null){
        btn.addEventListener("click", () => {
            const qstr = window.location.search;
            const urlParams = new URLSearchParams(qstr);
            let cnt = urlParams.get("pagenumber") == null ? 1 : urlParams.get("pagenumber");
            let num = parseInt(cnt);
            num -= 1;
            if (num < 0) {
                num = 1;
            }
            window.location = `/house?pagenumber=${num}`
        })
    }
}

function updateHouse() {
    let btns = document.querySelectorAll(".update_house_btn");
    if (btns.length > 0) {
        btns.forEach((btn) => {
            btn.addEventListener("click", () => {
                let btn_id = btn.id.replace("house_update_", "").trim()
                let deleteBtn = document.querySelector("#" +"house_delete_"+ btn_id)
                btn.remove()
                deleteBtn.remove()

                let houseSelector = "#" + "house_" + btn_id;
                let houseContainer = document.querySelector(houseSelector);

                let header = houseContainer.querySelector(".house-header-name");
                let headerInput = document.createElement("input");
                headerInput.id = "header_" + btn_id;
                headerInput.value = header.textContent.trim();
                header.parentNode.replaceChild(headerInput, header);

                let city = houseContainer.querySelector(".house-header-city");
                let cityInput = document.createElement("input");
                cityInput.id = "city_" + btn_id;
                cityInput.value = city.textContent.replace("City", "").trim();
                city.parentNode.replaceChild(cityInput, city);

                let description = houseContainer.querySelector(".house-description");
                let descriptionInput = document.createElement("input");
                descriptionInput.id = "description" + btn_id;
                descriptionInput.value = description.textContent.trim();
                description.parentNode.replaceChild(descriptionInput, description);

                let cost = houseContainer.querySelector(".house-cost");
                let costInput = document.createElement("input");
                costInput.id = "cost" + btn_id;
                costInput.value = cost.textContent.replace("Cost", "").trim();
                cost.parentNode.replaceChild(costInput, cost);

                let address = houseContainer.querySelector(".house-address");
                let addressInput = document.createElement("input");
                addressInput.id = "adress" + btn_id;
                addressInput.value = address.textContent.replace("Address", "").trim();
                address.parentNode.replaceChild(addressInput, address);

                let numberOfRooms = houseContainer.querySelector(".house-number_of_rooms");
                let numberOfRoomsInput = document.createElement("input");
                numberOfRoomsInput.id = "numberOfRooms" + btn_id;
                numberOfRoomsInput.value = numberOfRooms.textContent.replace("Number of rooms", "").trim();
                numberOfRooms.parentNode.replaceChild(numberOfRoomsInput, numberOfRooms);

                let numberOfFloors = houseContainer.querySelector(".house-number_of_floors");
                let numberOfFloorsInput = document.createElement("input");
                numberOfFloorsInput.id = "numberOfFloors" + btn_id;
                numberOfFloorsInput.value = numberOfFloors.textContent.replace("Number of floors", "").trim();
                numberOfFloors.parentNode.replaceChild(numberOfFloorsInput, numberOfFloors);

                let avgmark = houseContainer.querySelector(".house-avgmark");
                let avgmarkInput = document.createElement("input");
                avgmarkInput.id = "avgmark" + btn_id;
                avgmarkInput.value = avgmark.textContent.replace("Average mark", "").trim();
                avgmark.parentNode.replaceChild(avgmarkInput, avgmark);

                let ll = document.querySelector(".house-landlord_name")

                let avWifi = houseContainer.querySelector(".is-wifi-available")
                let avWifiInput = document.createElement("input");
                avWifiInput.id = "avWifi" + btn_id;
                avWifiInput.setAttribute("type", "checkbox");
                let avWifiValue = avWifi.textContent.trim();
                if(avWifiValue == "Wi-Fi available"){
                    avWifiInput.checked = true;
                }

                let wifiAvailableContainer = document.createElement("div");
                let wifHeaderAvailable = document.createElement("div");
                wifHeaderAvailable.textContent = "Wi-Fi available";
                wifiAvailableContainer.appendChild(avWifiInput);
                wifiAvailableContainer.appendChild(wifHeaderAvailable);
                wifiAvailableContainer.style.display = "flex";
                wifiAvailableContainer.style.flexDirection = "row";
                wifiAvailableContainer.style.gap = "8px";

                let additionalWifiAttributesContainer = document.createElement("div");
                additionalWifiAttributesContainer.appendChild(wifiAvailableContainer);

                avWifi.parentNode.replaceChild(additionalWifiAttributesContainer, avWifi);

                let avBathroom = houseContainer.querySelector(".is-bathroom-available")
                let avBathroomInput = document.createElement("input");
                avBathroomInput.id = "avWifi" + btn_id;
                avBathroomInput.setAttribute("type", "checkbox");
                let avBathroomValue = avBathroom.textContent.trim();
                if(avBathroomValue == "Bathroom available"){
                    avBathroomInput.checked = true;
                }

                let bathroomAvailableContainer = document.createElement("div");
                let bathroomAvailableHeader = document.createElement("div");
                bathroomAvailableHeader.textContent = "Bathroom available";
                bathroomAvailableContainer.appendChild(avBathroomInput);
                bathroomAvailableContainer.appendChild(bathroomAvailableHeader);
                bathroomAvailableContainer.style.display = "flex";
                bathroomAvailableContainer.style.flexDirection = "row";
                bathroomAvailableContainer.style.gap = "8px";

                let additionalBathroomAttributesContainer = document.createElement("div");
                additionalBathroomAttributesContainer.appendChild(bathroomAvailableContainer);

                avBathroom.parentNode.replaceChild(additionalBathroomAttributesContainer, avBathroom);


                let updateBtn = document.createElement("button");
                updateBtn.textContent = "Update";
                updateBtn.addEventListener("click", () => {
                    fetch("/House",{
                        method: "PUT",
                        body: JSON.stringify({
                            pid: parseInt(btn_id),
                            header: headerInput.value,
                            description: descriptionInput.value,
                            avgMark: parseFloat(avgmarkInput.value),
                            city: cityInput.value,
                            address: addressInput.value,
                            numberOfRooms: parseInt(numberOfRoomsInput.value),
                            numberOfFloors: parseInt(numberOfFloorsInput.value),
                            bathroomAvailability: avBathroomInput.checked,
                            wiFiAvailability: avWifiInput.checked,
                            costPerDay: parseFloat(costInput.value),
                            llid: parseInt(ll.id)
                        }),
                        headers: {
                            'Accept': 'application/json',
                            'Content-Type': 'application/json',
                        }
                    })
                        .then(response => {
                            location.reload()
                        })
                        .catch(e => console.log(e))
                })
                houseContainer.appendChild(updateBtn);
            });
        });
    }
}

function deleteHouseButton(){
    let btns = document.querySelectorAll(".delete_house_btn")
    for(let i=0; i<btns.length; i++){
        let btn = btns[i];
        btn.addEventListener("click", function(){
            let id = btn.id.replace("house_delete_", "").trim();
            fetch(`/house/delete/${id}`,{
                method: "DELETE",
            })
                .then(response => {
                    location.reload()
                })
                .catch(e => console.log(e))
        })
    }
}

function applyHouseFilter(){
    let btn = document.querySelector("#house_apply-filters")
    if (btn != null) {
        btn.addEventListener("click", () => {
            let city = document.querySelector("#house_city_filter")
            let cost = document.querySelector("#house_cost_filter")
            window.location.href = `/houses/filter/${city.value}/${cost.value}`;
        })
    }
}




 
