// ================================
// ELEMENTOS DO LOGIN
// ================================

const loginTab = document.getElementById('loginTab');
const registerTab = document.getElementById('registerTab');

const loginForm = document.getElementById('loginForm');
const registerForm = document.getElementById('registerForm');

const loginMessage = document.getElementById('loginMessage');
const registerMessage = document.getElementById('registerMessage');


// ================================
// TROCAR PARA LOGIN
// ================================

function showLogin() {

    if (!loginTab || !registerTab || !loginForm || !registerForm) {
        return;
    }

    loginTab.classList.add('active');
    registerTab.classList.remove('active');

    loginForm.classList.add('active');
    registerForm.classList.remove('active');

}


// ================================
// TROCAR PARA CRIAR CONTA
// ================================

function showRegister() {

    if (!loginTab || !registerTab || !loginForm || !registerForm) {
        return;
    }

    registerTab.classList.add('active');
    loginTab.classList.remove('active');

    registerForm.classList.add('active');
    loginForm.classList.remove('active');

}


// ================================
// CLIQUE NAS ABAS
// ================================

if (loginTab) {

    loginTab.addEventListener('click', () => {

        showLogin();

    });

}

if (registerTab) {

    registerTab.addEventListener('click', () => {

        showRegister();

    });

}


// ================================
// BOTÃO "ENTRAR"
// DENTRO DE CRIAR CONTA
// ================================

const goToLogin =
    document.getElementById('goToLogin');

if (goToLogin) {

    goToLogin.addEventListener('click', (event) => {

        event.preventDefault();

        showLogin();

    });

}


// ================================
// LEMBRAR DE MIM
// ================================

const loginEmailInput =
    document.getElementById('loginEmail');

const rememberMeCheckbox =
    document.getElementById('rememberMe');

if (loginEmailInput && rememberMeCheckbox) {

    // Carregando o e-mail salvo anteriormente
    const savedEmail =
        localStorage.getItem('rememberedEmail');

    if (savedEmail) {

        loginEmailInput.value = savedEmail;

        rememberMeCheckbox.checked = true;

    }

}


// ================================
// LOGIN
// ================================

if (loginForm) {

    loginForm.addEventListener('submit', (event) => {

        event.preventDefault();

        const email =
            document.getElementById('loginEmail').value.trim();

        // Salvando ou removendo o e-mail lembrado
        if (rememberMeCheckbox) {

            if (rememberMeCheckbox.checked) {

                localStorage.setItem(
                    'rememberedEmail',
                    email
                );

            } else {

                localStorage.removeItem(
                    'rememberedEmail'
                );

            }

        }

        if (loginMessage) {

            loginMessage.textContent =
                `Login realizado para ${email}.`;

        }

        /*
         * Aqui futuramente entra a integração
         * com o endpoint de Login da API.
         */

    });

}


// ================================
// CRIAR CONTA
// ================================

if (registerForm) {

    registerForm.addEventListener('submit', (event) => {

        event.preventDefault();

        const name =
            document.getElementById('registerName').value.trim();

        const password =
            document.getElementById('registerPassword').value;

        const passwordConfirm =
            document.getElementById('registerPasswordConfirm').value;


        // Verificando se as senhas são iguais
        if (password !== passwordConfirm) {

            if (registerMessage) {

                registerMessage.textContent =
                    'As senhas não coincidem.';

            }

            return;

        }


        if (registerMessage) {

            registerMessage.textContent =
                `Conta de ${name} criada com sucesso!`;

        }

        registerForm.reset();

    });

}


// ================================
// ESQUECI MINHA SENHA
// ================================

const forgotPassword =
    document.getElementById('forgotPassword');

if (forgotPassword) {

    forgotPassword.addEventListener('click', (event) => {

        event.preventDefault();

        if (loginMessage) {

            loginMessage.textContent =
                'Digite seu e-mail para receber as instruções de recuperação.';

        }

    });

}