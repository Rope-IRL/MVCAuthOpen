document.addEventListener("DOMContentLoaded", () =>
{
    goNextPage()
    goPreviousPage()
    updateFlat()
    deleteButton();
    applyFilter();
    getLogOut();
    create_flat()
})

function goNextPage()
{
    let btn = document.querySelector(".next_flats");
    if(btn != null){
        btn.addEventListener("click", () => {
            const qstr = window.location.search;
            const urlParams = new URLSearchParams(qstr);
            let cnt = urlParams.get("pagenumber") == null ? 1 : urlParams.get("pagenumber");
            let num = parseInt(cnt);
            num += 1;
            window.location = `/flat?pagenumber=${num}`;
        })
    }
}

function goPreviousPage() {
    let btn = document.querySelector(".previous_flats");
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
            window.location = `/flat?pagenumber=${num}`
        })
    }
}

function updateFlat() {
    let btns = document.querySelectorAll(".update_btn");
    if (btns.length > 0) {
        btns.forEach((btn) => {
            btn.addEventListener("click", () => {
                let btn_id = btn.id.replace("update_", "").trim()
                let deleteBtn = document.querySelector("#" +"delete_"+ btn_id)
                btn.remove()
                deleteBtn.remove()

                let flatSelector = "#" + "flat_" + btn_id;
                let flatContainer = document.querySelector(flatSelector);

                let header = flatContainer.querySelector(".flat-header-name");
                let headerInput = document.createElement("input");
                headerInput.id = "header_" + btn_id;
                headerInput.value = header.textContent.trim();
                header.parentNode.replaceChild(headerInput, header);

                let city = flatContainer.querySelector(".flat-header-city");
                let cityInput = document.createElement("input");
                cityInput.id = "city_" + btn_id;
                cityInput.value = city.textContent.replace("City", "").trim();
                city.parentNode.replaceChild(cityInput, city);

                let description = flatContainer.querySelector(".flat-description");
                let descriptionInput = document.createElement("input");
                descriptionInput.id = "description" + btn_id;
                descriptionInput.value = description.textContent.trim();
                description.parentNode.replaceChild(descriptionInput, description);

                let cost = flatContainer.querySelector(".flat-cost");
                let costInput = document.createElement("input");
                costInput.id = "cost" + btn_id;
                costInput.value = cost.textContent.replace("Cost", "").trim();
                cost.parentNode.replaceChild(costInput, cost);

                let address = flatContainer.querySelector(".flat-address");
                let addressInput = document.createElement("input");
                addressInput.id = "adress" + btn_id;
                addressInput.value = address.textContent.replace("Address", "").trim();
                address.parentNode.replaceChild(addressInput, address);

                let numberOfRooms = flatContainer.querySelector(".flat-number_of_rooms");
                let numberOfRoomsInput = document.createElement("input");
                numberOfRoomsInput.id = "numberOfRooms" + btn_id;
                numberOfRoomsInput.value = numberOfRooms.textContent.replace("Number of rooms", "").trim();
                numberOfRooms.parentNode.replaceChild(numberOfRoomsInput, numberOfRooms);

                let numberOfFloors = flatContainer.querySelector(".flat-number_of_floors");
                let numberOfFloorsInput = document.createElement("input");
                numberOfFloorsInput.id = "numberOfFloors" + btn_id;
                numberOfFloorsInput.value = numberOfFloors.textContent.replace("Number of floors", "").trim();
                numberOfFloors.parentNode.replaceChild(numberOfFloorsInput, numberOfFloors);

                let avgmark = flatContainer.querySelector(".flat-avgmark");
                let avgmarkInput = document.createElement("input");
                avgmarkInput.id = "avgmark" + btn_id;
                avgmarkInput.value = avgmark.textContent.replace("Average mark", "").trim();
                avgmark.parentNode.replaceChild(avgmarkInput, avgmark);

                let ll_name = flatContainer.querySelector(".flat-landlord_name")
                let ll_surname = flatContainer.querySelector(".flat-landlord_surname")
                ll_name.remove()
                ll_surname.remove()
                
                let avWifi = flatContainer.querySelector(".is-wifi-available")
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

                let avBathroom = flatContainer.querySelector(".is-bathroom-available")
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
                
                let selection = flatContainer.querySelector(".change-landlord-inv");
                selection.classList.remove("change-landlord-inv");
                selection.classList.add("change-landlord");
                
                let updateBtn = document.createElement("button");
                updateBtn.textContent = "Update";
                updateBtn.addEventListener("click", () => {
                    fetch("/flat",{
                        method: "PUT",
                        body: JSON.stringify({
                            fid: parseInt(btn_id),
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
                            llid: parseInt(selection.value)
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
                flatContainer.appendChild(updateBtn);
            });
        });
    }
   
}

function deleteButton(){
    let btns = document.querySelectorAll(".delete_btn")
    for(let i=0; i<btns.length; i++){
       let btn = btns[i];     
       btn.addEventListener("click", function(){
            let id = btn.id.replace("delete_", "").trim();
           fetch(`/flat/delete/${id}`,{
               method: "DELETE",
           })
               .then(response => {
                   location.reload()
               })
               .catch(e => console.log(e))
        })
    }
        
}

function applyFilter(){
    let btn = document.querySelector("#apply-filters")
    if (btn != null) {
        btn.addEventListener("click", () => {
            let city = document.querySelector("#city_filter")
            let cost = document.querySelector("#cost_filter")
            if(cost.value.toString() == "" && city.value == ""){
                window.location.href = "/Flat"
            }
            else{
                window.location.href = `/flats/filter/${city.value}/${cost.value}`;
            }
        })  
    }
}

async function getLogOut(){
    let btn = document.querySelector("#log_out_btn");
    if(btn != null){
        btn.addEventListener("click", async () => {
            const isRedirect = await fetch("/auth/logout",{
                method: "GET",
            })
            
            if(isRedirect.redirected)
            {
                window.location.href = isRedirect.url;
            }
        })
    }
}

function create_flat(){
    let create_containers = document.querySelectorAll(".create-flat")
    
    for(let i= 0; i<create_containers.length; i++){
        let create_container = create_containers[i]
        
        let create_btn = create_container.querySelector(".create-button")
        
        create_btn.addEventListener("click", (e) => {
            let headerInput = document.createElement("input");
            let headerLabel = document.createElement("div");
            headerLabel.textContent = "Flat header";
            let cityInput = document.createElement("input");
            let cityLabel = document.createElement("div");
            cityLabel.textContent = "Flat city";
            let descriptionInput = document.createElement("input");
            let descriptionLabel = document.createElement("div");
            descriptionLabel.textContent = "Flat description";
            let costInput = document.createElement("input");
            let costLabel = document.createElement("div");
            costLabel.textContent = "Flat cost";
            let addressInput = document.createElement("input");
            let addressLabel = document.createElement("div");
            addressLabel.textContent = "Flat address";
            let numberOfRoomsInput = document.createElement("input");
            let numberOfRoomsLabel = document.createElement("div");
            numberOfRoomsLabel.textContent = "Flat number of rooms";
            let numberOfFloorsInput = document.createElement("input");
            let numberOfFloorsLabel = document.createElement("div");
            numberOfFloorsLabel.textContent = "Flat number of floors";
    
            let avgmarkInput = document.createElement("input");
            let avgMarkLabel = document.createElement("div");
            avgMarkLabel.textContent = "Flat average mark";

            let avWifiInput = document.createElement("input");
            avWifiInput.setAttribute("type", "checkbox");
            let wifiLabel = document.createElement("div");
            wifiLabel.textContent = "Flat wifi available";
            
            let avBathroomInput = document.createElement("input");
            avBathroomInput.setAttribute("type", "checkbox");
            let bathroomLabel = document.createElement("div");
            bathroomLabel.textContent = "Flat bathroom available";

            let selection = document.querySelector(".change-landlord-inv");
            let container_selection = selection.cloneNode(true);
            container_selection.classList.remove("change-landlord-inv");
            container_selection.classList.add("change-landlord");

            create_container.appendChild(headerLabel);
            create_container.appendChild(headerInput);
            
            create_container.appendChild(cityLabel);
            create_container.appendChild(cityInput);
            
            create_container.appendChild(descriptionLabel);
            create_container.appendChild(descriptionInput);
            
            create_container.appendChild(costLabel);
            create_container.appendChild(costInput);
            
            create_container.appendChild(addressLabel);
            create_container.appendChild(addressInput);
            
            create_container.appendChild(avgMarkLabel);
            create_container.appendChild(avgmarkInput);
            
            create_container.appendChild(wifiLabel)
            create_container.appendChild(avWifiInput);
            
            create_container.appendChild(bathroomLabel);
            create_container.appendChild(avBathroomInput);
            
            create_container.appendChild(numberOfRoomsLabel);
            create_container.appendChild(numberOfRoomsInput);

            create_container.appendChild(numberOfFloorsLabel);
            create_container.appendChild(numberOfFloorsInput);
            
            create_container.appendChild(container_selection);
            
            let updateBtn = document.createElement("button");
            updateBtn.textContent = "Create";
            updateBtn.addEventListener("click", () => {
                fetch("/flat/add",{
                    method: "POST",
                    body: JSON.stringify({
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
                        llid: parseInt(selection.value)
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
            create_container.appendChild(updateBtn);
            create_container.style.display = "flex";
            create_container.style.flexDirection = "column";
            create_container.style.gap = "4px"
            
        })
    }
}

 
