document.addEventListener("DOMContentLoaded", () => {
    update_contract();
    delete_contract();
})

function update_contract()
{
    let contract_containers = document.querySelectorAll(".contract-container");
    for(let i = 0; i< contract_containers.length; i++)
    {
        let container = contract_containers[i];
        let update_btn = container.querySelector(".contract_update_btn");
        if(update_btn != null){
            update_btn.addEventListener("click", () => {
                let delete_btn = container.querySelector(".contract_delete_btn");
                update_btn.remove();
                delete_btn.remove();
                let startDate = container.querySelector(".contract-start-date");
                let startDateInput = document.createElement("input");
                startDateInput.type = "text";
                startDateInput.value = startDate.textContent.replace("Start date:", "").trim();
                container.replaceChild(startDateInput, startDate);

                let endDate = container.querySelector(".contract-end-date");
                let endDateInput = document.createElement("input");
                endDateInput.type = "text";
                endDateInput.value = endDate.textContent.replace("End date:", "").trim();
                container.replaceChild(endDateInput, endDate);

                let landlord_name = container.querySelector(".contract-landlord-name");
                let landlord_surname = container.querySelector(".contract-landlord-surname");
                landlord_name.remove();
                landlord_surname.remove();
                let landlord_selection = container.querySelector(".landlords-selector-inv");
                landlord_selection.classList.remove("landlords-selector-inv");
                landlord_selection.classList.add("landlords-selector");

                let flat = container.querySelector(".contract-flat-header");
                flat.remove();
                let flat_selection = container.querySelector(".flats-selector-inv");
                flat_selection.classList.remove("flats-selector-inv");
                flat_selection.classList.add("flats-selector");

                let cost = container.querySelector(".contract-cost");
                let costInput = document.createElement("input");
                costInput.type = "text";
                costInput.value = cost.textContent.replace("Cost:", "").trim();
                container.replaceChild(costInput, cost);
                
                let lessee = container.querySelector(".contract-lessee-name");
                
                let addBtn = document.createElement("button");
                addBtn.textContent = "Update";
                addBtn.addEventListener("click", () => {
                    fetch("/FlatsContract",{
                        method: "POST",
                        body: JSON.stringify({
                            Id: parseInt(container.id),
                            Lid: parseInt(lessee.id),
                            Llid: parseInt(landlord_selection.value),
                            StartDate: new Date(startDateInput.value).toISOString().split('T')[0],
                            EndDate: new Date(endDateInput.value).toISOString().split('T')[0],
                            Cost: parseFloat(costInput.value),
                            Fid: parseInt(flat_selection.value),
                            
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
                container.appendChild(addBtn);
            
            })
        }
    }
}

function delete_contract(){
    let contract_containers = document.querySelectorAll(".contract-container");
    for(let i = 0; i < contract_containers.length; i++){
        let container = contract_containers[i];
        let delete_btn = container.querySelector(".contract_delete_btn");
        delete_btn.addEventListener("click", () => {
            fetch(`/flatcontract/delete/${container.id}`,{
                method: "DELETE",
            })
            .then(response => {
                location.reload()
            })
            .catch(e => console.log(e))
        })
    }
}