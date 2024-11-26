document.addEventListener("DOMContentLoaded", () =>{
    AdminLogin();
    AdminLogOut();
    updateCredentials();
    goNextPage();
    goPreviousPage();
})

async function AdminLogin() {
    let loginButton = document.querySelector(".admin-login-button");
    if(loginButton != null){
        loginButton.addEventListener("click", async (event) => {
            event.preventDefault();
            let login = document.querySelector("#admin-login");
            let password = document.querySelector("#admin-password");

            let loginUrl = "/admin/login";

            const data = await fetch(loginUrl, {
                method: "POST",
                credentials: 'include',
                "headers": {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    login: login.value,
                    password: password.value
                })
            })

            if (data.redirected) {
                // Redirect to the new location
                window.location.href = data.url;
            } else {
                // Handle other responses, e.g., invalid credentials
                const responseData = await data.json();
                console.log("Login failed:", responseData.message);
            }

        });
    }
}

async function AdminLogOut(){
    let btn = document.querySelector("#admin_log_out_btn");
    if(btn != null){
        btn.addEventListener("click", async () => {
            const isRedirect = await fetch("/admin/logout",{
                method: "GET",
            })

            if(isRedirect.redirected)
            {
                window.location.href = isRedirect.url;
            }
        })
    }
}

function updateCredentials(){
    let containers = document.querySelectorAll(".land_lord--wrapper")
    for(let i = 0; i < containers.length; i++){
        let container = containers[i];
        let btn = container.querySelector(".landlord_update")
        btn.addEventListener("click", (e) => {
            btn.remove()
            
            let login = container.querySelector(".info--login");
            let loginValue = login.textContent.replace("Login:", "").trim()
            let loginInput = document.createElement("input");
            loginInput.type = "text";
            loginInput.value = loginValue;
            login.parentNode.replaceChild(loginInput, login);
            
            let password = container.querySelector(".info--password");
            let passwordValue = password.textContent.replace("Password:", "").trim();
            let passwordInput = document.createElement("input");
            passwordInput.type = "text";
            passwordInput.value = passwordValue;
            password.parentNode.replaceChild(passwordInput, password);
            
            let email = container.querySelector(".info--email");
            let emailValue = email.textContent.replace("Email:", "").trim();
            let emailInput = document.createElement("input");
            emailInput.type = "text";
            emailInput.value = emailValue;
            email.parentNode.replaceChild(emailInput, email);
            
            let updateBtn = document.createElement("button");
            updateBtn.textContent = "Update";
            updateBtn.addEventListener("click", async () => {
                fetch("/admin/update/landlord", {
                    method: "PUT",
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({
                        id: container.id, 
                        login: loginInput.value,
                        password: passwordInput.value,
                        email: emailInput.value
                    })
                })
                    .then(response => {
                        if (!response.ok) {
                            throw new Error("HTTP status " + response.status);
                        }
                        location.reload()
                    })
                    .catch(error => console.error("Error:", error));
            })
            container.appendChild(updateBtn);
        })
    }
}

function deleteCredentials(){
    let containers = document.querySelectorAll(".land_lord--wrapper")
    for(let i = 0; i < containers.length; i++){
        let container = containers[i];
        let btn = container.querySelector(".landlord_delete")
        btn.addEventListener("click", (e) => {
            btn.remove()

            let login = container.querySelector(".info--login");
            let loginValue = login.textContent.replace("Login:", "").trim()

            let password = container.querySelector(".info--password");
            let passwordValue = password.textContent.replace("Password:", "").trim();
            
            let email = container.querySelector(".info--email");
            let emailValue = email.textContent.replace("Email:", "").trim();

            let updateBtn = document.createElement("button");
            updateBtn.textContent = "Update";
            updateBtn.addEventListener("click", async () => {
                fetch("/admin/update/landlord", {
                    method: "PUT",
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({
                        id: container.id, 
                        login: loginInput.value,
                        password: passwordInput.value,
                        email: emailInput.value
                    })
                })
                    .then(response => {
                        if (!response.ok) {
                            throw new Error("HTTP status " + response.status);
                        }
                        location.reload()
                    })

                    .catch(error => console.error("Error:", error));
            })
            container.appendChild(updateBtn);
        })
    }
}

function goNextPage()
{
    let btn = document.querySelector(".next_landlords");
    if(btn != null){
        btn.addEventListener("click", () => {
            const qstr = window.location.search;
            const urlParams = new URLSearchParams(qstr);
            let cnt = urlParams.get("pagenumber") == null ? 1 : urlParams.get("pagenumber");
            let num = parseInt(cnt);
            num += 1;
            window.location = `/landlords?pagenumber=${num}`;
        })
    }
}

function goPreviousPage() {
    let btn = document.querySelector(".previous_landlords");
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
            window.location = `/landlords?pagenumber=${num}`
        })
    }
}