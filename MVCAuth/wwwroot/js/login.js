document.addEventListener("DOMContentLoaded", () =>{
    Login();
})

async function Login() {
    let loginButton = document.querySelector(".login_button");
    if(loginButton != null){
        loginButton.addEventListener("click", async (event) => {
            event.preventDefault();
            let login = document.querySelector("#login");
            let password = document.querySelector("#password");
    
            let loginUrl = "/auth/login";
    
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