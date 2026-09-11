// ================================
// MINHA CONTA
// ================================

// Pegando o token que está salvo no navegador
const token = localStorage.getItem("token");

// Se não existir token, significa que o usuário não está logado
if (!token) {

    window.location.href = "login.html";

}


// ================================
// BOTÃO SAIR
// ================================

const btnSair = document.getElementById("btnSair");

if (btnSair) {

    btnSair.addEventListener("click", () => {

        // Removendo o token salvo no navegador
        localStorage.removeItem("token");

        // Voltando para a página de login
        window.location.href = "login.html";

    });

}