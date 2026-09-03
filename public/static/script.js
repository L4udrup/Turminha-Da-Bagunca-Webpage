// ================================
// MENU MOBILE
// ================================

const menuToggle = document.getElementById('menuToggle');
const menu = document.getElementById('menu');

if (menuToggle && menu) {

    menuToggle.addEventListener('click', () => {

        menu.classList.toggle('open');

        if (menu.classList.contains('open')) {
            menuToggle.textContent = '✕';
        } else {
            menuToggle.textContent = '☰';
        }

    });

}


// ================================
// FECHAR MENU AO CLICAR EM UM LINK
// ================================

if (menu && menuToggle) {

    document.querySelectorAll('.menu a').forEach(link => {

        link.addEventListener('click', () => {

            menu.classList.remove('open');

            menuToggle.textContent = '☰';

        });

    });

}


// ================================
// FORMULÁRIO DE ORÇAMENTO
// ================================

const quoteForm = document.getElementById('quoteForm');
const formMessage = document.getElementById('formMessage');

if (quoteForm && formMessage) {

    quoteForm.addEventListener('submit', (event) => {

        event.preventDefault();

        const nameInput = document.getElementById('name');

        const name = nameInput
            ? nameInput.value.trim()
            : '';

        formMessage.textContent =
            `Obrigado, ${name}! Sua solicitação foi recebida. Em breve entraremos em contato.`;

        quoteForm.reset();

    });

}


// ================================
// ÁREA DO CLIENTE
// LOGIN / CRIAR CONTA
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

const goToLogin = document.getElementById('goToLogin');

if (goToLogin) {

    goToLogin.addEventListener('click', (event) => {

        event.preventDefault();

        showLogin();

    });

}


// ================================
// LOGIN
// ================================

if (loginForm) {

    loginForm.addEventListener('submit', (event) => {

        event.preventDefault();

        const email =
            document.getElementById('loginEmail').value.trim();

        if (loginMessage) {

            loginMessage.textContent =
                `Login realizado para ${email}.`;

        }

        /*
         * IMPORTANTE:
         *
         * Aqui futuramente você pode colocar
         * a integração com o backend/Django.
         *
         * Por enquanto o formulário não recarrega
         * a página.
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


        // Verifica se as senhas são iguais

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


// ================================
// DESTAQUE DO MENU CONFORME A SEÇÃO
// ================================

const sections =
    document.querySelectorAll('main section[id]');

const navLinks =
    document.querySelectorAll('.menu a');


if (sections.length && navLinks.length) {

    const observer = new IntersectionObserver(

        entries => {

            entries.forEach(entry => {

                if (entry.isIntersecting) {

                    navLinks.forEach(link => {

                        link.classList.remove('active');

                    });


                    const activeLink =
                        document.querySelector(
                            `.menu a[href="#${entry.target.id}"]`
                        );


                    if (activeLink) {

                        activeLink.classList.add('active');

                    }

                }

            });

        },

        {
            threshold: 0.25
        }

    );


    sections.forEach(section => {

        observer.observe(section);

    });

}

document.addEventListener('DOMContentLoaded', () => {

    /* =========================================================
       1. MOSTRAR / OCULTAR SENHA (TOGGLE PASSWORD)
    ========================================================= */
    const toggleButtons = document.querySelectorAll('.toggle-password');

    toggleButtons.forEach(button => {
        button.addEventListener('click', () => {
            const input = button.previousElementSibling;

            if (input && (input.type === 'password' || input.type === 'text')) {
                const isPassword = input.type === 'password';
                
                // Alterna tipo do input
                input.type = isPassword ? 'text' : 'password';
                
                // Alterna ícone (👁️ = mostrar, 🙈 = ocultar)
                button.textContent = isPassword ? '🙈' : '👁️';
            }
        });
    });


    /* =========================================================
       2. LEMBRAR DE MIM (LOCALSTORAGE)
    ========================================================= */
    const loginForm = document.getElementById('loginForm');
    const loginEmailInput = document.getElementById('loginEmail');
    const rememberMeCheckbox = document.getElementById('rememberMe');

    // Carrega o e-mail salvo ao carregar a página
    if (loginEmailInput && rememberMeCheckbox) {
        const savedEmail = localStorage.getItem('rememberedEmail');

        if (savedEmail) {
            loginEmailInput.value = savedEmail;
            rememberMeCheckbox.checked = true;
        }

        // Salva/remove o e-mail no envio do formulário de Login
        if (loginForm) {
            loginForm.addEventListener('submit', () => {
                if (rememberMeCheckbox.checked) {
                    localStorage.setItem('rememberedEmail', loginEmailInput.value.trim());
                } else {
                    localStorage.removeItem('rememberedEmail');
                }
            });
        }
    }


    /* =========================================================
       3. TROCA DE ABAS (LOGIN / CRIAR CONTA) & MENU
    ========================================================= */
    const loginTab = document.getElementById('loginTab');
    const registerTab = document.getElementById('registerTab');
    const registerForm = document.getElementById('registerForm');
    const goToLogin = document.getElementById('goToLogin');

    if (loginTab && registerTab && loginForm && registerForm) {
        loginTab.addEventListener('click', () => {
            loginTab.classList.add('active');
            registerTab.classList.remove('active');
            loginForm.classList.add('active');
            registerForm.classList.remove('active');
        });

        registerTab.addEventListener('click', () => {
            registerTab.classList.add('active');
            loginTab.classList.remove('active');
            registerForm.classList.add('active');
            loginForm.classList.remove('active');
        });

        if (goToLogin) {
            goToLogin.addEventListener('click', (e) => {
                e.preventDefault();
                loginTab.click();
            });
        }
    }

    // Toggle Menu Mobile
    const menuToggle = document.getElementById('menuToggle');
    const menu = document.getElementById('menu');

    if (menuToggle && menu) {
        menuToggle.addEventListener('click', () => {
            menu.classList.toggle('open');
        });
    }

});